namespace Microsoft.JSharp
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    internal sealed class JSharpCodeGenerator : CodeCompiler
    {
        private CodeAttributeDeclarationCollection assemblyAttributes = null;
        private CodeTypeDeclaration currentEnum;
        private CodeParameterDeclarationExpressionCollection currentMethodParameters;
        private bool fDisableEnumHack;
        internal bool fEnableDataformWizWorkaround = true;
        private bool fInArrayInitializer;
        private bool fInSetStatement = false;
        private bool fIsAttributeArg = false;
        private bool forLoopHack = false;
        private bool fOutOrRefUsedInCurrentMethod = false;
        private static readonly string[][] keywords;
        private const GeneratorSupport LanguageSupport = (GeneratorSupport.Win32Resources | GeneratorSupport.ComplexExpressions | GeneratorSupport.PublicStaticMembers | GeneratorSupport.NestedTypes | GeneratorSupport.ParameterAttributes | GeneratorSupport.AssemblyAttributes | GeneratorSupport.DeclareEvents | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareDelegates | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.TryCatchStatements | GeneratorSupport.StaticConstructors | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.ArraysOfArrays);
        private const int MaxLineLength = 80;
        private int NestingLevel = 0;
        private static Regex outputReg;
        private static readonly string[][] primitiveTypes;
        private CodeTypeReference setCodeTypeReference = null;
        private static readonly string[][] vjsAttributes;

        static JSharpCodeGenerator()
        {
            string[][] strArray = new string[14][];
            strArray[10] = new string[] { "System.Byte", "System.Char" };
            strArray[11] = new string[] { "System.Int16", "System.Int32", "System.Int64", "System.SByte" };
            strArray[12] = new string[] { "System.Double", "System.Single" };
            strArray[13] = new string[] { "System.Boolean" };
            primitiveTypes = strArray;
            strArray = new string[12][];
            strArray[1] = new string[] { "do", "if" };
            strArray[2] = new string[] { "for", "int", "new", "try" };
            strArray[3] = new string[] { "byte", "case", "char", "else", "goto", "long", "null", "this", "true", "void" };
            strArray[4] = new string[] { "break", "catch", "class", "const", "false", "final", "float", "short", "super", "throw", "ubyte", "while" };
            strArray[5] = new string[] { "double", "import", "native", "public", "return", "static", "string", "struct", "switch", "throws" };
            strArray[6] = new string[] { "boolean", "default", "extends", "finally", "package", "private" };
            strArray[7] = new string[] { "abstract", "continue", "delegate", "volatile" };
            strArray[8] = new string[] { "interface", "multicast", "protected", "transient" };
            strArray[9] = new string[] { "implements", "instanceof" };
            strArray[11] = new string[] { "synchronized" };
            keywords = strArray;
            strArray = new string[11][];
            strArray[2] = new string[] { "com", "dll" };
            strArray[5] = new string[] { "hidden", "module" };
            strArray[7] = new string[] { "security", "assembly" };
            strArray[8] = new string[] { "attribute" };
            strArray[10] = new string[] { "conditional" };
            vjsAttributes = strArray;
        }

        private void AppendEscapedChar(StringBuilder b, char value)
        {
            if (b == null)
            {
                base.Output.Write(@"\u");
                int num = value;
                base.Output.Write(num.ToString("X4", CultureInfo.InvariantCulture));
            }
            else
            {
                b.Append(@"\u");
                b.Append(((int) value).ToString("X4", CultureInfo.InvariantCulture));
            }
        }

        private bool castRequiredForParam(string paramName, ref string paramType)
        {
            if (this.currentMethodParameters != null)
            {
                foreach (CodeParameterDeclarationExpression expression in this.currentMethodParameters)
                {
                    if ((string.Compare(expression.Name, paramName, false, CultureInfo.InvariantCulture) == 0) && IsPrimitiveType(expression.Type))
                    {
                        paramType = expression.Type.BaseType;
                        return true;
                    }
                }
            }
            return false;
        }

        protected override string CmdArgsFromParameters(CompilerParameters options)
        {
            StringBuilder builder = new StringBuilder(0x80);
            if (options.GenerateExecutable)
            {
                builder.Append("/t:exe ");
                if ((options.MainClass != null) && (options.MainClass.Length > 0))
                {
                    builder.Append("/main:");
                    builder.Append(options.MainClass);
                    builder.Append(" ");
                }
            }
            else
            {
                builder.Append("/t:library ");
            }
            builder.Append("/utf8output ");
            StringEnumerator enumerator = options.ReferencedAssemblies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                builder.Append("/r:");
                builder.Append("\"");
                builder.Append(current);
                builder.Append("\"");
                builder.Append(" ");
            }
            builder.Append("/out:");
            builder.Append("\"");
            builder.Append(options.OutputAssembly);
            builder.Append("\"");
            builder.Append(" ");
            if (options.IncludeDebugInformation)
            {
                builder.Append("/d:DEBUG ");
                builder.Append("/debug ");
            }
            else
            {
                builder.Append("/optimize ");
            }
            if (options.Win32Resource != null)
            {
                builder.Append("/win32res:\"" + options.Win32Resource + "\" ");
            }
            if (options.TreatWarningsAsErrors)
            {
                builder.Append("/warnaserror ");
            }
            if (options.WarningLevel >= 0)
            {
                builder.Append("/w:" + options.WarningLevel + " ");
            }
            if (options.CompilerOptions != null)
            {
                builder.Append(options.CompilerOptions + " ");
            }
            return builder.ToString();
        }

        protected override string CreateEscapedIdentifier(string name)
        {
            if (IsKeyword(name))
            {
                return ("@" + name);
            }
            return name;
        }

        protected override string CreateValidIdentifier(string name)
        {
            if (IsKeyword(name))
            {
                return ("_" + name);
            }
            return name;
        }

        private void GatherThrowStmts(CodeStatementCollection stms, ArrayList arr)
        {
            foreach (CodeStatement statement in stms)
            {
                if (statement is CodeThrowExceptionStatement)
                {
                    arr.Add(((CodeThrowExceptionStatement) statement).ToThrow);
                    continue;
                }
                if (statement is CodeConditionStatement)
                {
                    this.GatherThrowStmts(((CodeConditionStatement) statement).TrueStatements, arr);
                    this.GatherThrowStmts(((CodeConditionStatement) statement).FalseStatements, arr);
                    continue;
                }
                if (statement is CodeTryCatchFinallyStatement)
                {
                    CodeCatchClauseCollection catchClauses = ((CodeTryCatchFinallyStatement) statement).CatchClauses;
                    if (catchClauses.Count > 0)
                    {
                        foreach (CodeCatchClause clause in catchClauses)
                        {
                            this.GatherThrowStmts(clause.Statements, arr);
                        }
                    }
                    this.GatherThrowStmts(((CodeTryCatchFinallyStatement) statement).FinallyStatements, arr);
                    continue;
                }
                if (statement is CodeIterationStatement)
                {
                    this.GatherThrowStmts(((CodeIterationStatement) statement).Statements, arr);
                }
            }
        }

        protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
            if (this.fInArrayInitializer)
            {
                string paramType = null;
                if (this.castRequiredForParam(e.ParameterName, ref paramType))
                {
                    base.Output.Write("(");
                    base.Output.Write(paramType);
                    base.Output.Write(")");
                }
            }
            this.OutputIdentifier(e.ParameterName);
        }

        protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
        {
            base.Output.Write("new ");
            CodeExpressionCollection initializers = e.Initializers;
            if (initializers.Count > 0)
            {
                this.OutputType(e.CreateType);
                if (e.CreateType.ArrayRank == 0)
                {
                    base.Output.Write("[]");
                }
                this.OutputStartingBrace();
                if (string.Compare(e.CreateType.BaseType, "System.Object", false, CultureInfo.InvariantCulture) == 0)
                {
                    this.fInArrayInitializer = true;
                }
                this.OutputExpressionList(initializers, true);
                if (string.Compare(e.CreateType.BaseType, "System.Object", false, CultureInfo.InvariantCulture) == 0)
                {
                    this.fInArrayInitializer = false;
                }
                this.OutputEndingBrace();
            }
            else
            {
                base.Output.Write(this.GetBaseTypeOutput(e.CreateType.BaseType));
                base.Output.Write("[");
                if (e.SizeExpression != null)
                {
                    base.GenerateExpression(e.SizeExpression);
                }
                else
                {
                    base.Output.Write(e.Size);
                }
                base.Output.Write("]");
            }
        }

        protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
        {
            base.GenerateExpression(e.TargetObject);
            base.Output.Write("[");
            bool flag = true;
            foreach (CodeExpression expression in e.Indices)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    base.Output.Write(", ");
                }
                base.GenerateExpression(expression);
            }
            base.Output.Write("]");
        }

        protected override void GenerateAssignStatement(CodeAssignStatement e)
        {
            if (this.fEnableDataformWizWorkaround && (e.Left is CodeFieldReferenceExpression))
            {
                CodeFieldReferenceExpression left = (CodeFieldReferenceExpression) e.Left;
                if ((string.Compare(left.FieldName, "SelectedIndex", false, CultureInfo.InvariantCulture) == 0) || (string.Compare(left.FieldName, "Table", false, CultureInfo.InvariantCulture) == 0))
                {
                    e = new CodeAssignStatement(e.Left, e.Right);
                    e.Left = new CodePropertyReferenceExpression(left.TargetObject, left.FieldName);
                }
            }
            if (e.Left is CodeIndexerExpression)
            {
                this.GenerateJavaIndexerReferenceExpression((CodeIndexerExpression) e.Left, "set");
                base.Output.Write("(");
                bool flag = true;
                foreach (CodeExpression expression2 in ((CodeIndexerExpression) e.Left).Indices)
                {
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        base.Output.Write(",");
                    }
                    base.GenerateExpression(expression2);
                }
                if (!flag)
                {
                    base.Output.Write(",");
                }
                if (this.fInSetStatement && IsPrimitiveType(this.setCodeTypeReference))
                {
                    base.Output.Write("(" + this.setCodeTypeReference.BaseType + ")");
                }
                base.GenerateExpression(e.Right);
                base.Output.Write(")");
            }
            else if (e.Left is CodePropertyReferenceExpression)
            {
                this.GenerateJavaPropertyReferenceExpression((CodePropertyReferenceExpression) e.Left, "set");
                base.Output.Write("(");
                base.GenerateExpression(e.Right);
                base.Output.Write(")");
            }
            else
            {
                base.GenerateExpression(e.Left);
                base.Output.Write(" = ");
                base.GenerateExpression(e.Right);
            }
            if (!this.forLoopHack)
            {
                base.Output.WriteLine(";");
            }
        }

        protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
        {
            this.GenerateJavaEventReferenceExpression(e.Event, "add");
            base.Output.Write("( ");
            base.GenerateExpression(e.Listener);
            base.Output.WriteLine(" );");
        }

        protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
            base.Output.Write("*/");
        }

        protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
            base.Output.Write("/** ");
        }

        private void GenerateAttributes(CodeAttributeDeclarationCollection attributes)
        {
            this.GenerateAttributes(attributes, null, false);
        }

        private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix)
        {
            this.GenerateAttributes(attributes, prefix, false);
        }

        private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix, bool inLine)
        {
            if (attributes.Count != 0)
            {
                IEnumerator enumerator = attributes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    this.GenerateAttributeDeclarationsStart(attributes);
                    if (prefix != null)
                    {
                        if (string.Compare(prefix, "assembly: ", false, CultureInfo.InvariantCulture) == 0)
                        {
                            base.Output.Write("@assembly ");
                        }
                        else if (string.Compare(prefix, "return: ", false, CultureInfo.InvariantCulture) == 0)
                        {
                            base.Output.Write("@attribute.return ");
                        }
                        else
                        {
                            base.Output.Write("@attribute ");
                        }
                    }
                    else
                    {
                        base.Output.Write("@attribute ");
                    }
                    CodeAttributeDeclaration current = (CodeAttributeDeclaration) enumerator.Current;
                    string baseTypeOutput = this.GetBaseTypeOutput(current.Name);
                    base.Output.Write(baseTypeOutput);
                    base.Output.Write("(");
                    bool flag = true;
                    foreach (CodeAttributeArgument argument in current.Arguments)
                    {
                        if (flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            base.Output.Write(", ");
                        }
                        this.fIsAttributeArg = true;
                        this.OutputAttributeArgument(argument);
                        this.fIsAttributeArg = false;
                    }
                    base.Output.Write(")");
                    this.GenerateAttributeDeclarationsEnd(attributes);
                    if (inLine)
                    {
                        base.Output.Write(" ");
                    }
                    else
                    {
                        base.Output.WriteLine();
                    }
                }
            }
        }

        protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
        {
            base.Output.Write("super");
        }

        protected override void GenerateCastExpression(CodeCastExpression e)
        {
            base.Output.Write("((");
            this.OutputType(e.TargetType);
            base.Output.Write(")(");
            if (IsPrimitiveType(e.TargetType))
            {
                base.Output.Write(e.TargetType.BaseType);
                base.Output.Write(")(");
            }
            base.GenerateExpression(e.Expression);
            base.Output.Write("))");
        }

        protected override void GenerateComment(CodeComment e)
        {
            bool fMultiline = false;
            string escapedComment = this.GetEscapedComment(e.Text, ref fMultiline);
            if (e.DocComment)
            {
                base.Output.Write("/** ");
                base.Output.Write(escapedComment);
                base.Output.WriteLine(" */");
            }
            else if (fMultiline)
            {
                base.Output.Write("/* ");
                base.Output.Write(escapedComment);
                base.Output.WriteLine(" */");
            }
            else
            {
                base.Output.Write("// ");
                base.Output.WriteLine(escapedComment);
            }
        }

        protected override void GenerateCompileUnit(CodeCompileUnit e)
        {
            if ((e.AssemblyCustomAttributes != null) && (e.AssemblyCustomAttributes.Count > 0))
            {
                this.assemblyAttributes = e.AssemblyCustomAttributes;
            }
            this.GenerateCompileUnitStart(e);
            if (e.Namespaces[0] != null)
            {
                this.ParseForUnsupportedProxyTree(e.Namespaces[0]);
                this.GenerateNamespace(e.Namespaces[0]);
            }
            this.GenerateCompileUnitEnd(e);
        }

        protected override void GenerateCompileUnitStart(CodeCompileUnit e)
        {
            base.Output.WriteLine("/*******************************************************************************");
            base.Output.WriteLine(" *");
            base.Output.WriteLine(" *     This code was generated by a tool.");
            base.Output.WriteLine(" *     Runtime Version: " + Environment.Version.ToString());
            base.Output.WriteLine(" *");
            base.Output.WriteLine(" *     Changes to this file may cause incorrect behavior and will be lost if ");
            base.Output.WriteLine(" *     the code is regenerated.");
            base.Output.WriteLine(" *");
            base.Output.WriteLine(" ******************************************************************************/");
            base.Output.WriteLine("");
        }

        protected override void GenerateConditionStatement(CodeConditionStatement e)
        {
            base.Output.Write("if (");
            base.GenerateExpression(e.Condition);
            base.Output.Write(")");
            this.OutputStartingBrace();
            base.GenerateStatements(e.TrueStatements);
            if (e.FalseStatements.Count > 0)
            {
                this.OutputEndingBraceElseStyle();
                base.Output.Write("else");
                this.OutputStartingBrace();
                base.GenerateStatements(e.FalseStatements);
            }
            this.OutputEndingBrace();
        }

        protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
        {
            if (base.IsCurrentClass || base.IsCurrentStruct)
            {
                if (e.CustomAttributes.Count > 0)
                {
                    this.GenerateAttributes(e.CustomAttributes);
                }
                this.OutputMemberAccessModifier(e.Attributes);
                this.OutputIdentifier(base.CurrentTypeName);
                base.Output.Write("(");
                this.OutputParameters(e.Parameters);
                base.Output.Write(")");
                this.GenerateThrowsClause(e.Statements);
                CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
                CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
                this.OutputStartingBrace();
                if (baseConstructorArgs.Count > 0)
                {
                    base.Output.Write("super(");
                    this.OutputExpressionList(baseConstructorArgs);
                    base.Output.WriteLine(");");
                }
                else if (chainedConstructorArgs.Count > 0)
                {
                    base.Output.Write("this(");
                    this.OutputExpressionList(chainedConstructorArgs);
                    base.Output.WriteLine(");");
                }
                this.GenerateMethodStatements(e.Statements);
                this.OutputEndingBrace();
            }
        }

        protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
        {
            base.Output.Write("new ");
            this.OutputType(e.DelegateType);
            base.Output.Write("(");
            base.GenerateExpression(e.TargetObject);
            base.Output.Write(".");
            this.OutputIdentifier(e.MethodName);
            base.Output.Write(")");
        }

        protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
        {
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            base.Output.Write("Invoke(");
            this.OutputExpressionList(e.Parameters);
            base.Output.Write(")");
        }

        protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
        {
            base.Output.Write("public static void main(String[] args)");
            this.GenerateThrowsClause(e.Statements);
            this.OutputStartingBrace();
            this.GenerateMethodStatements(e.Statements);
            this.OutputEndingBrace();
        }

        private void GenerateEnum(CodeTypeDeclaration e)
        {
            if (this.fDisableEnumHack)
            {
                base.Output.Write("#error ");
                base.Output.WriteLine("enum parameters are not supported for RPC-based SOAP messages and Document-based SOAP messages with Encoded parameter formatting. Use Visual C# .NET or Visual Basic .NET to generate a proxy for this web reference. Refer to Visual J# .NET documentation for more information.");
            }
            base.Output.WriteLine();
            base.Output.WriteLine("/** @attribute XmlTextAttribute() */");
            base.Output.WriteLine("public String value;");
            base.Output.WriteLine();
            base.Output.Write("public ");
            this.OutputIdentifier(e.Name);
            base.Output.Write("() ");
            this.OutputStartingBrace();
            this.OutputEndingBrace();
            base.Output.WriteLine();
            base.Output.Write("public ");
            this.OutputIdentifier(e.Name);
            base.Output.Write("(String value)");
            this.OutputStartingBrace();
            base.Output.WriteLine("this.value = value;");
            this.OutputEndingBrace();
            base.Output.WriteLine();
            base.Output.Write("public boolean Equals(Object m)");
            this.OutputStartingBrace();
            base.Output.WriteLine("return (m != null && (m instanceof " + e.Name + ") && String.Equals(value, ((" + e.Name + ")m).value));");
            this.OutputEndingBrace();
            base.Output.WriteLine();
            base.Output.Write("public String ToString()");
            this.OutputStartingBrace();
            base.Output.WriteLine("return this.value;");
            this.OutputEndingBrace();
        }

        protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
        {
            if (!base.IsCurrentDelegate && !base.IsCurrentEnum)
            {
                base.Output.Write("private ");
                string name = e.Name;
                string str2 = e.Name;
                this.OutputTypeNamePair(e.Type, str2);
                base.Output.WriteLine(";");
                base.Output.WriteLine("");
                if (e.CustomAttributes.Count > 0)
                {
                    this.GenerateAttributes(e.CustomAttributes);
                }
                base.Output.WriteLine("/** @event */");
                this.OutputMemberAccessModifier(e.Attributes);
                base.Output.Write(" void add_" + name + "(");
                this.OutputTypeNamePair(e.Type, "e");
                base.Output.Write(")");
                this.OutputStartingBrace();
                base.Output.Write("this." + str2 + " = (");
                this.OutputType(e.Type);
                base.Output.WriteLine(")System.Delegate.Combine(this." + str2 + ",e);");
                this.OutputEndingBrace();
                base.Output.WriteLine("");
                base.Output.WriteLine("/** @event */");
                this.OutputMemberAccessModifier(e.Attributes);
                base.Output.Write(" void remove_" + name + "(");
                this.OutputTypeNamePair(e.Type, "e");
                base.Output.Write(")");
                this.OutputStartingBrace();
                base.Output.Write("this." + str2 + " = (");
                this.OutputType(e.Type);
                base.Output.WriteLine(")System.Delegate.Remove(this." + str2 + ",e);");
                this.OutputEndingBrace();
            }
        }

        protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            this.OutputIdentifier(e.EventName);
        }

        protected override void GenerateExpressionStatement(CodeExpressionStatement e)
        {
            base.GenerateExpression(e.Expression);
            if (!this.forLoopHack)
            {
                base.Output.WriteLine(";");
            }
        }

        protected override void GenerateField(CodeMemberField e)
        {
            if (!base.IsCurrentDelegate && !base.IsCurrentInterface)
            {
                if (base.IsCurrentEnum)
                {
                    string name = e.Name;
                    if (e.CustomAttributes.Count > 0)
                    {
                        CodeAttributeDeclaration declaration = e.CustomAttributes[0];
                        if (((declaration.Name.StartsWith("System.Xml.Serialization.XmlEnum") && (declaration.Arguments != null)) && ((declaration.Arguments.Count > 0) && (declaration.Arguments[0].Value != null))) && ((declaration.Arguments[0].Value is CodePrimitiveExpression) && (((CodePrimitiveExpression) declaration.Arguments[0].Value).Value is string)))
                        {
                            name = (string) ((CodePrimitiveExpression) declaration.Arguments[0].Value).Value;
                        }
                    }
                    base.Output.Write("public static final ");
                    this.OutputIdentifier(this.currentEnum.Name);
                    base.Output.Write(" ");
                    this.OutputIdentifier(e.Name);
                    base.Output.Write(" = new ");
                    this.OutputIdentifier(this.currentEnum.Name);
                    base.Output.Write("(");
                    base.Output.Write(this.QuoteSnippetString(name, false));
                    base.Output.WriteLine(");");
                }
                else
                {
                    if (e.CustomAttributes.Count > 0)
                    {
                        this.GenerateAttributes(e.CustomAttributes);
                    }
                    this.OutputMemberAccessModifier(e.Attributes);
                    this.OutputFieldScopeModifier(e.Attributes);
                    this.OutputTypeNamePair(e.Type, e.Name);
                    if (e.InitExpression != null)
                    {
                        base.Output.Write(" = ");
                        base.GenerateExpression(e.InitExpression);
                    }
                    base.Output.WriteLine(";");
                }
            }
        }

        protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
        {
            if (this.fInArrayInitializer && (e.TargetObject == null))
            {
                string paramType = null;
                if (this.castRequiredForParam(e.FieldName, ref paramType))
                {
                    base.Output.Write("(");
                    base.Output.Write(paramType);
                    base.Output.Write(")");
                }
            }
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            if ((this.fEnableDataformWizWorkaround && (string.Compare(e.FieldName, "BindingContext", false, CultureInfo.InvariantCulture) == 0)) || ((string.Compare(e.FieldName, "Response", false, CultureInfo.InvariantCulture) == 0) || (string.Compare(e.FieldName, "Tables", false, CultureInfo.InvariantCulture) == 0)))
            {
                base.Output.Write("get_");
                base.Output.Write(e.FieldName);
                base.Output.Write("()");
            }
            else
            {
                this.OutputIdentifier(e.FieldName);
            }
        }

        protected override void GenerateGotoStatement(CodeGotoStatement e)
        {
            throw new NotSupportedException("goto");
        }

        protected override void GenerateIndexerExpression(CodeIndexerExpression e)
        {
            this.GenerateJavaIndexerReferenceExpression(e, "get");
            base.Output.Write("(");
            bool flag = true;
            foreach (CodeExpression expression in e.Indices)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    base.Output.Write(",");
                }
                base.GenerateExpression(expression);
            }
            base.Output.Write(")");
        }

        protected override void GenerateIterationStatement(CodeIterationStatement e)
        {
            this.forLoopHack = true;
            base.Output.Write("for (");
            base.GenerateStatement(e.InitStatement);
            base.Output.Write("; ");
            base.GenerateExpression(e.TestExpression);
            base.Output.Write("; ");
            base.GenerateStatement(e.IncrementStatement);
            base.Output.Write(")");
            this.OutputStartingBrace();
            this.forLoopHack = false;
            base.GenerateStatements(e.Statements);
            this.OutputEndingBrace();
        }

        private void GenerateJavaEventReferenceExpression(CodeEventReferenceExpression e, string prefix)
        {
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            base.Output.Write(prefix + "_" + e.EventName);
        }

        private void GenerateJavaIndexerReferenceExpression(CodeIndexerExpression e, string prefix)
        {
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            base.Output.Write(prefix + "_Item");
        }

        private void GenerateJavaPropertyReferenceExpression(CodePropertyReferenceExpression e, string prefix)
        {
            if (e.TargetObject != null)
            {
                base.GenerateExpression(e.TargetObject);
                base.Output.Write(".");
            }
            base.Output.Write(prefix + "_" + e.PropertyName);
        }

        protected override void GenerateLabeledStatement(CodeLabeledStatement e)
        {
            base.Indent--;
            base.Output.Write(e.Label);
            base.Output.WriteLine(":");
            base.Indent++;
            if (e.Statement != null)
            {
                base.GenerateStatement(e.Statement);
            }
        }

        protected override void GenerateLinePragmaEnd(CodeLinePragma e)
        {
            base.Output.WriteLine();
            base.Output.WriteLine("#line default");
        }

        protected override void GenerateLinePragmaStart(CodeLinePragma e)
        {
            base.Output.WriteLine("");
            base.Output.Write("#line ");
            base.Output.Write(e.LineNumber);
            base.Output.Write(" ");
            base.Output.Write(this.QuoteSnippetString(e.FileName, false));
            base.Output.WriteLine("");
        }

        protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
        {
            if ((base.IsCurrentClass || base.IsCurrentStruct) || base.IsCurrentInterface)
            {
                this.currentMethodParameters = e.Parameters;
                if (e.CustomAttributes.Count > 0)
                {
                    this.GenerateAttributes(e.CustomAttributes);
                }
                if (e.ReturnTypeCustomAttributes.Count > 0)
                {
                    this.GenerateAttributes(e.ReturnTypeCustomAttributes, "return: ");
                }
                if (!base.IsCurrentInterface)
                {
                    this.OutputMemberAccessModifier(e.Attributes);
                    this.OutputMemberScopeModifier(e.Attributes);
                }
                this.OutputType(e.ReturnType);
                base.Output.Write(" ");
                this.OutputIdentifier(e.Name);
                base.Output.Write("(");
                this.OutputParameters(e.Parameters);
                base.Output.Write(")");
                if (!base.IsCurrentInterface && ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract))
                {
                    this.GenerateThrowsClause(e.Statements);
                    this.OutputStartingBrace();
                    if (this.fOutOrRefUsedInCurrentMethod)
                    {
                        base.Output.Write("#error ");
                        base.Output.WriteLine("'out' and 'ref' parameters are not supported.  Use Visual C# .NET or Visual Basic .NET to generate a proxy for this web reference. Refer to Visual J# .NET documentation for more information.");
                    }
                    this.GenerateMethodStatements(e.Statements);
                    this.OutputEndingBrace();
                }
                else
                {
                    base.Output.WriteLine(";");
                }
                this.currentMethodParameters = null;
                this.fOutOrRefUsedInCurrentMethod = false;
            }
        }

        protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
            if (this.fEnableDataformWizWorkaround && (string.Compare(e.Method.MethodName, "ToString", false, CultureInfo.InvariantCulture) == 0))
            {
                if (e.Method.TargetObject != null)
                {
                    base.Output.Write("String.valueOf(");
                    base.GenerateExpression(e.Method.TargetObject);
                    base.Output.Write(")");
                }
            }
            else
            {
                this.GenerateMethodReferenceExpression(e.Method);
                base.Output.Write("(");
                this.OutputExpressionList(e.Parameters);
                base.Output.Write(")");
            }
        }

        protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                if (e.TargetObject is CodeBinaryOperatorExpression)
                {
                    base.Output.Write("(");
                    base.GenerateExpression(e.TargetObject);
                    base.Output.Write(")");
                }
                else
                {
                    base.GenerateExpression(e.TargetObject);
                }
                base.Output.Write(".");
            }
            this.OutputIdentifier(e.MethodName);
        }

        protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
        {
            base.Output.Write("return");
            if (e.Expression != null)
            {
                base.Output.Write(" ");
                base.GenerateExpression(e.Expression);
            }
            base.Output.WriteLine(";");
        }

        private void GenerateMethodStatements(CodeStatementCollection stms)
        {
            base.GenerateStatements(stms);
        }

        protected override void GenerateNamespace(CodeNamespace e)
        {
            this.GenerateCommentStatements(e.Comments);
            this.GenerateNamespaceStart(e);
            base.GenerateNamespaceImports(e);
            base.Output.WriteLine("");
            if ((this.assemblyAttributes != null) && (this.assemblyAttributes.Count > 0))
            {
                base.Output.WriteLine("");
                this.GenerateAttributes(this.assemblyAttributes, "assembly: ");
                this.assemblyAttributes = null;
            }
            base.GenerateTypes(e);
            this.GenerateNamespaceEnd(e);
        }

        protected override void GenerateNamespaceEnd(CodeNamespace e)
        {
        }

        protected override void GenerateNamespaceImport(CodeNamespaceImport e)
        {
            base.Output.Write("import ");
            this.OutputIdentifier(e.Namespace);
            base.Output.WriteLine(".*;");
        }

        protected override void GenerateNamespaceStart(CodeNamespace e)
        {
            if ((e.Name != null) && (e.Name.Length > 0))
            {
                base.Output.Write("package ");
                this.OutputIdentifier(e.Name);
                base.Output.WriteLine(";");
            }
        }

        protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
            base.Output.Write("new ");
            this.OutputType(e.CreateType);
            base.Output.Write("(");
            this.OutputExpressionList(e.Parameters);
            base.Output.Write(")");
        }

        protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                this.GenerateAttributes(e.CustomAttributes, null, true);
            }
            this.OutputDirection(e.Direction);
            this.OutputTypeNamePair(e.Type, e.Name);
        }

        private void GeneratePrimitiveChar(char c)
        {
            base.Output.Write('\'');
            switch (c)
            {
                case '\u2028':
                case '\u2029':
                    this.AppendEscapedChar(null, c);
                    break;

                case '\\':
                    base.Output.Write(@"\\");
                    break;

                case '\'':
                    base.Output.Write(@"\'");
                    break;

                case '\t':
                    base.Output.Write(@"\t");
                    break;

                case '\n':
                    base.Output.Write(@"\n");
                    break;

                case '\r':
                    base.Output.Write(@"\r");
                    break;

                case '"':
                    base.Output.Write("\\\"");
                    break;

                case '\0':
                    base.Output.Write(@"\0");
                    break;

                default:
                    base.Output.Write(c);
                    break;
            }
            base.Output.Write('\'');
        }

        protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
        {
            if (e.Value is char)
            {
                this.GeneratePrimitiveChar((char) e.Value);
            }
            else if (e.Value is sbyte)
            {
                sbyte num = (sbyte) e.Value;
                base.Output.Write("((byte)" + num.ToString(CultureInfo.InvariantCulture) + ")");
            }
            else if (e.Value is byte)
            {
                byte num2 = (byte) e.Value;
                base.Output.Write("((ubyte)" + num2.ToString(CultureInfo.InvariantCulture) + ")");
            }
            else if (e.Value is short)
            {
                short num3 = (short) e.Value;
                base.Output.Write("((short)" + num3.ToString(CultureInfo.InvariantCulture) + ")");
            }
            else if (e.Value is long)
            {
                long num4 = (long) e.Value;
                base.Output.Write(num4.ToString(CultureInfo.InvariantCulture) + "L");
            }
            else
            {
                base.GeneratePrimitiveExpression(e);
            }
        }

        protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
        {
            if ((base.IsCurrentClass || base.IsCurrentStruct) || base.IsCurrentInterface)
            {
                bool flag = false;
                if (e.HasGet)
                {
                    if (e.CustomAttributes.Count > 0)
                    {
                        this.GenerateAttributes(e.CustomAttributes);
                        flag = true;
                    }
                    base.Output.WriteLine("/** @property */");
                    this.OutputMemberAccessModifier(e.Attributes);
                    this.OutputMemberScopeModifier(e.Attributes);
                    this.OutputType(e.Type);
                    base.Output.Write(" ");
                    base.Output.Write("get_" + e.Name);
                    base.Output.Write("(");
                    if (e.Parameters.Count > 0)
                    {
                        this.OutputParameters(e.Parameters);
                    }
                    base.Output.Write(")");
                    this.GenerateThrowsClause(e.GetStatements);
                    this.OutputStartingBrace();
                    this.GenerateMethodStatements(e.GetStatements);
                    this.OutputEndingBrace();
                }
                if (e.HasSet)
                {
                    if ((e.CustomAttributes.Count > 0) && !flag)
                    {
                        this.GenerateAttributes(e.CustomAttributes);
                    }
                    base.Output.WriteLine("/** @property */");
                    this.OutputMemberAccessModifier(e.Attributes);
                    this.OutputMemberScopeModifier(e.Attributes);
                    base.Output.Write("void ");
                    base.Output.Write("set_" + e.Name);
                    base.Output.Write("(");
                    if (e.Parameters.Count > 0)
                    {
                        this.OutputParameters(e.Parameters);
                        base.Output.Write(", ");
                    }
                    this.OutputType(e.Type);
                    base.Output.Write(" ");
                    this.GeneratePropertySetValueReferenceExpression(null);
                    base.Output.Write(")");
                    this.GenerateThrowsClause(e.SetStatements);
                    this.OutputStartingBrace();
                    this.fInSetStatement = true;
                    this.setCodeTypeReference = e.Type;
                    this.GenerateMethodStatements(e.SetStatements);
                    this.setCodeTypeReference = null;
                    this.fInSetStatement = false;
                    this.OutputEndingBrace();
                }
            }
        }

        protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
            this.GenerateJavaPropertyReferenceExpression(e, "get");
            base.Output.Write("(");
            base.Output.Write(")");
        }

        protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
        {
            base.Output.Write("value");
        }

        protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
        {
            this.GenerateJavaEventReferenceExpression(e.Event, "remove");
            base.Output.Write("( ");
            base.GenerateExpression(e.Listener);
            base.Output.WriteLine(" );");
        }

        protected override void GenerateSingleFloatValue(float s)
        {
            base.Output.Write(s.ToString(CultureInfo.InvariantCulture));
            base.Output.Write('F');
        }

        protected override void GenerateSnippetExpression(CodeSnippetExpression e)
        {
            base.Output.Write(e.Value);
        }

        protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
        {
            base.Output.Write(e.Text);
        }

        protected override void GenerateSnippetStatement(CodeSnippetStatement e)
        {
            base.Output.WriteLine(e.Value);
        }

        protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
        {
            base.Output.Write("this");
        }

        protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
            if (e.ToThrow != null)
            {
                base.Output.Write("throw");
                base.Output.Write(" ");
                base.GenerateExpression(e.ToThrow);
                base.Output.WriteLine(";");
            }
        }

        private void GenerateThrowsClause(CodeStatementCollection stms)
        {
            ArrayList arr = new ArrayList();
            this.GatherThrowStmts(stms, arr);
            if (arr.Count >= 1)
            {
                base.Output.Write(" throws ");
                bool flag = true;
                for (int i = 0; i < arr.Count; i++)
                {
                    CodeExpression expression = (CodeExpression) arr[i];
                    if (expression is CodeObjectCreateExpression)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        CodeObjectCreateExpression expression2 = (CodeObjectCreateExpression) expression;
                        CodeObjectCreateExpression expression3 = (CodeObjectCreateExpression) arr[i - 1];
                        if (expression2.CreateType.BaseType == expression3.CreateType.BaseType)
                        {
                            continue;
                        }
                    }
                    flag = false;
                    break;
                }
                if (flag)
                {
                    this.OutputType(((CodeObjectCreateExpression) arr[0]).CreateType);
                }
                else
                {
                    base.Output.Write("System.Exception");
                }
            }
        }

        protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
        {
            base.Output.Write("try");
            this.OutputStartingBrace();
            base.GenerateStatements(e.TryStatements);
            CodeCatchClauseCollection catchClauses = e.CatchClauses;
            if (catchClauses.Count > 0)
            {
                IEnumerator enumerator = catchClauses.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    this.OutputEndingBraceElseStyle();
                    CodeCatchClause current = (CodeCatchClause) enumerator.Current;
                    base.Output.Write("catch (");
                    this.OutputType(current.CatchExceptionType);
                    base.Output.Write(" ");
                    this.OutputIdentifier(current.LocalName);
                    base.Output.Write(")");
                    this.OutputStartingBrace();
                    base.GenerateStatements(current.Statements);
                }
            }
            CodeStatementCollection finallyStatements = e.FinallyStatements;
            if (finallyStatements.Count > 0)
            {
                this.OutputEndingBraceElseStyle();
                base.Output.Write("finally");
                this.OutputStartingBrace();
                base.GenerateStatements(finallyStatements);
            }
            this.OutputEndingBrace();
        }

        protected override void GenerateTypeConstructor(CodeTypeConstructor e)
        {
            if (base.IsCurrentClass || base.IsCurrentStruct)
            {
                base.Output.Write("static ");
                this.OutputStartingBrace();
                this.GenerateMethodStatements(e.Statements);
                this.OutputEndingBrace();
            }
        }

        protected override void GenerateTypeEnd(CodeTypeDeclaration e)
        {
            this.NestingLevel--;
            if (base.IsCurrentEnum)
            {
                this.GenerateEnum(e);
                this.currentEnum = null;
            }
            if (!base.IsCurrentDelegate)
            {
                this.OutputEndingBrace();
            }
        }

        protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
        {
            this.OutputType(e.Type);
            base.Output.Write(".class");
            if (!this.fIsAttributeArg)
            {
                base.Output.Write(".ToType()");
            }
        }

        protected override void GenerateTypeStart(CodeTypeDeclaration e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                this.GenerateAttributes(e.CustomAttributes);
            }
            if (!base.IsCurrentDelegate)
            {
                this.OutputTypeAttributes(e);
                this.OutputIdentifier(e.Name);
                bool flag = true;
                bool flag2 = false;
                foreach (CodeTypeReference reference in e.BaseTypes)
                {
                    if (flag)
                    {
                        base.Output.Write(" extends ");
                        flag = false;
                        flag2 = true;
                    }
                    else if (flag2)
                    {
                        base.Output.Write(" implements ");
                        flag2 = false;
                    }
                    else
                    {
                        base.Output.Write(", ");
                    }
                    this.OutputType(reference);
                }
                if (base.IsCurrentEnum)
                {
                    this.currentEnum = e;
                }
                this.OutputStartingBrace();
            }
            else
            {
                base.Output.WriteLine("/** @delegate */");
                switch ((e.TypeAttributes & TypeAttributes.VisibilityMask))
                {
                    case TypeAttributes.Public:
                        base.Output.Write("public ");
                        break;
                }
                CodeTypeDelegate delegate2 = (CodeTypeDelegate) e;
                base.Output.Write("delegate ");
                this.OutputType(delegate2.ReturnType);
                base.Output.Write(" ");
                this.OutputIdentifier(e.Name);
                base.Output.Write("(");
                this.OutputParameters(delegate2.Parameters);
                base.Output.WriteLine(");");
            }
            this.NestingLevel++;
        }

        protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
            this.OutputTypeNamePair(e.Type, e.Name);
            if (e.InitExpression != null)
            {
                base.Output.Write(" = ");
                base.GenerateExpression(e.InitExpression);
            }
            if (!this.forLoopHack)
            {
                base.Output.WriteLine(";");
            }
        }

        protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
            this.OutputIdentifier(e.VariableName);
        }

        private string GetBaseTypeOutput(string baseType)
        {
            string strA = this.CreateEscapedIdentifier(baseType);
            if (strA.Length == 0)
            {
                return "void";
            }
            if (string.Compare(strA, "System.SByte", false, CultureInfo.InvariantCulture) == 0)
            {
                return "byte";
            }
            if (string.Compare(strA, "System.Byte", false, CultureInfo.InvariantCulture) == 0)
            {
                return "ubyte";
            }
            if (string.Compare(strA, "System.Int16", false, CultureInfo.InvariantCulture) == 0)
            {
                return "short";
            }
            if (string.Compare(strA, "System.Int32", false, CultureInfo.InvariantCulture) == 0)
            {
                return "int";
            }
            if (string.Compare(strA, "System.Int64", false, CultureInfo.InvariantCulture) == 0)
            {
                return "long";
            }
            if (string.Compare(strA, "System.String", false, CultureInfo.InvariantCulture) == 0)
            {
                return "String";
            }
            if (string.Compare(strA, "System.Object", false, CultureInfo.InvariantCulture) == 0)
            {
                return "Object";
            }
            if (string.Compare(strA, "System.Boolean", false, CultureInfo.InvariantCulture) == 0)
            {
                return "boolean";
            }
            if (string.Compare(strA, "System.Void", false, CultureInfo.InvariantCulture) == 0)
            {
                return "void";
            }
            if (string.Compare(strA, "System.Char", false, CultureInfo.InvariantCulture) == 0)
            {
                return "char";
            }
            if (string.Compare(strA, "System.Single", false, CultureInfo.InvariantCulture) == 0)
            {
                return "float";
            }
            if (string.Compare(strA, "System.Double", false, CultureInfo.InvariantCulture) == 0)
            {
                return "double";
            }
            return strA.Replace('+', '.');
        }

        private string GetEscapedComment(string commentString, ref bool fMultiline)
        {
            StringBuilder b = new StringBuilder(commentString.Length);
            BitArray isEscaped = new BitArray(commentString.Length);
            string str = this.LexUnicodeEscapeSequence(commentString, isEscaped);
            fMultiline = false;
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '\u2028':
                    case '\u2029':
                    case '\n':
                    case '\r':
                        fMultiline = true;
                        break;

                    case '@':
                        if ((i < (str.Length - 1)) && this.IsAttribute(str.Substring(i + 1)))
                        {
                            b.Append("@");
                        }
                        break;

                    case '*':
                        if ((i < (str.Length - 1)) && (str[i + 1] == '/'))
                        {
                            goto Label_00F2;
                        }
                        break;
                }
                if (str[i] != '\0')
                {
                    if (isEscaped.Get(i))
                    {
                        this.AppendEscapedChar(b, str[i]);
                    }
                    else
                    {
                        b.Append(str[i]);
                    }
                }
            }
        Label_00F2:
            return b.ToString();
        }

        protected override string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs)
        {
            return base.GetResponseFileCmdArgs(options, cmdArgs);
        }

        protected override string GetTypeOutput(CodeTypeReference typeRef)
        {
            string typeOutput;
            if (typeRef.ArrayElementType != null)
            {
                typeOutput = this.GetTypeOutput(typeRef.ArrayElementType);
            }
            else
            {
                typeOutput = this.GetBaseTypeOutput(typeRef.BaseType);
            }
            if (typeRef.ArrayRank <= 0)
            {
                return typeOutput;
            }
            char[] chArray = new char[typeRef.ArrayRank + 1];
            chArray[0] = '[';
            chArray[typeRef.ArrayRank] = ']';
            for (int i = 1; i < typeRef.ArrayRank; i++)
            {
                chArray[i] = ',';
            }
            return (typeOutput + new string(chArray));
        }

        private bool IsAttribute(string val)
        {
            int length = 0;
            while (length < val.Length)
            {
                if (!char.IsLetterOrDigit(val[length]))
                {
                    break;
                }
                length++;
            }
            if (length == 0)
            {
                return false;
            }
            return FixedStringLookup.Contains(vjsAttributes, val.Substring(0, length), false);
        }

        private bool IsEncodedProxy(CodeTypeDeclaration e)
        {
            foreach (CodeTypeMember member in e.Members)
            {
                if (((member is CodeMemberMethod) && !(member is CodeTypeConstructor)) && !(member is CodeConstructor))
                {
                    CodeMemberMethod method = (CodeMemberMethod) member;
                    if (this.IsMethodEncoded(method))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsKeyword(string value)
        {
            return FixedStringLookup.Contains(keywords, value, false);
        }

        private bool IsMethodEncoded(CodeMemberMethod e)
        {
            foreach (CodeAttributeDeclaration declaration in e.CustomAttributes)
            {
                if (string.Compare(declaration.Name, "System.Web.Services.Protocols.SoapRpcMethodAttribute") == 0)
                {
                    return true;
                }
                if (string.Compare(declaration.Name, "System.Web.Services.Protocols.SoapDocumentMethodAttribute") == 0)
                {
                    foreach (CodeAttributeArgument argument in declaration.Arguments)
                    {
                        CodeFieldReferenceExpression expression = null;
                        CodeTypeReferenceExpression expression2 = null;
                        if ((((string.Compare(argument.Name, "Use") == 0) && ((expression = argument.Value as CodeFieldReferenceExpression) != null)) && ((string.Compare(expression.FieldName, "Encoded") == 0) && ((expression2 = expression.TargetObject as CodeTypeReferenceExpression) != null))) && (string.Compare(expression2.Type.BaseType, "System.Web.Services.Description.SoapBindingUse") == 0))
                        {
                            return true;
                        }
                    }
                    continue;
                }
            }
            return false;
        }

        private static bool IsPrimitiveType(CodeTypeReference type)
        {
            if (type.ArrayRank > 0)
            {
                return false;
            }
            return FixedStringLookup.Contains(primitiveTypes, type.BaseType, false);
        }

        private bool IsProxyType(CodeTypeDeclaration e)
        {
            if (e.BaseTypes.Count == 1)
            {
                CodeTypeReference reference = e.BaseTypes[0];
                if (string.Compare(reference.BaseType, "System.Web.Services.Protocols.SoapHttpClientProtocol") == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool IsValidIdentifier(string value)
        {
            if ((value == null) || (value.Length == 0))
            {
                return false;
            }
            if (value[0] != '@')
            {
                if (IsKeyword(value))
                {
                    return false;
                }
            }
            else
            {
                value = value.Substring(1);
            }
            return CodeGenerator.IsValidLanguageIndependentIdentifier(value);
        }

        private string LexUnicodeEscapeSequence(string text, BitArray isEscaped)
        {
            int length = text.Length;
            StringBuilder builder = new StringBuilder(length);
            int startIndex = 0;
            int index = 0;
            while (index != -1)
            {
                index = text.IndexOf(@"\u", startIndex);
                if ((index == -1) || ((length - index) < 6))
                {
                    builder.Append(text.Substring(startIndex));
                    break;
                }
                builder.Append(text.Substring(startIndex, index - startIndex));
                try
                {
                    int num4 = int.Parse(text.Substring(index + 2, 4), NumberStyles.AllowHexSpecifier);
                    builder.Append((char) num4);
                    isEscaped.Set(builder.Length - 1, true);
                    startIndex = index + 6;
                    continue;
                }
                catch (Exception)
                {
                    builder.Append(@"\u");
                    startIndex = index + 2;
                    continue;
                }
            }
            return builder.ToString();
        }

        protected override void OutputDirection(FieldDirection dir)
        {
            switch (dir)
            {
                case FieldDirection.In:
                    return;

                case FieldDirection.Out:
                    base.Output.Write("/*out*/");
                    this.fOutOrRefUsedInCurrentMethod = true;
                    return;

                case FieldDirection.Ref:
                    base.Output.Write("/*ref*/");
                    this.fOutOrRefUsedInCurrentMethod = true;
                    return;
            }
        }

        private void OutputEndingBrace()
        {
            base.Indent--;
            base.Output.WriteLine("}");
        }

        private void OutputEndingBraceElseStyle()
        {
            base.Indent--;
            if (base.Options.ElseOnClosing)
            {
                base.Output.Write("} ");
            }
            else
            {
                base.Output.WriteLine("}");
            }
        }

        protected override void OutputFieldScopeModifier(MemberAttributes attributes)
        {
            switch ((attributes & MemberAttributes.ScopeMask))
            {
                case MemberAttributes.Static:
                    base.Output.Write("static ");
                    return;

                case MemberAttributes.Override:
                    return;

                case MemberAttributes.Const:
                    base.Output.Write("static final ");
                    return;
            }
        }

        protected override void OutputIdentifier(string ident)
        {
            base.Output.Write(this.CreateEscapedIdentifier(ident));
        }

        protected override void OutputMemberAccessModifier(MemberAttributes attributes)
        {
            MemberAttributes attributes2 = attributes & MemberAttributes.AccessMask;
            if (attributes2 <= MemberAttributes.Family)
            {
                if (((attributes2 != MemberAttributes.Assembly) && (attributes2 != MemberAttributes.FamilyAndAssembly)) && (attributes2 == MemberAttributes.Family))
                {
                    base.Output.Write("protected ");
                }
            }
            else
            {
                switch (attributes2)
                {
                    case MemberAttributes.FamilyOrAssembly:
                        return;

                    case MemberAttributes.Private:
                        base.Output.Write("private ");
                        return;

                    case MemberAttributes.Public:
                        base.Output.Write("public ");
                        return;

                    default:
                        return;
                }
            }
        }

        protected override void OutputMemberScopeModifier(MemberAttributes attributes)
        {
            switch ((attributes & MemberAttributes.ScopeMask))
            {
                case MemberAttributes.Abstract:
                    base.Output.Write("abstract ");
                    return;

                case MemberAttributes.Final:
                    return;

                case MemberAttributes.Static:
                    base.Output.Write("static ");
                    return;
            }
        }

        private void OutputStartingBrace()
        {
            if (base.Options.BracingStyle == "C")
            {
                base.Output.WriteLine("");
                base.Output.WriteLine("{");
            }
            else
            {
                base.Output.WriteLine(" {");
            }
            base.Indent++;
        }

        protected override void OutputType(CodeTypeReference typeRef)
        {
            base.Output.Write(this.GetTypeOutput(typeRef));
        }

        private void OutputTypeAttributes(CodeTypeDeclaration e)
        {
            TypeAttributes typeAttributes = e.TypeAttributes;
            switch ((typeAttributes & TypeAttributes.VisibilityMask))
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                    base.Output.Write("public ");
                    break;

                case TypeAttributes.NestedPrivate:
                    base.Output.Write("private ");
                    break;

                case TypeAttributes.NestedFamily:
                    base.Output.Write("protected");
                    break;
            }
            if ((this.NestingLevel > 0) && base.IsCurrentClass)
            {
                base.Output.Write("static ");
            }
            if (e.IsStruct)
            {
                base.Output.Write("class ");
            }
            else if (e.IsEnum)
            {
                base.Output.Write("class ");
            }
            else
            {
                TypeAttributes attributes2 = typeAttributes & TypeAttributes.Interface;
                if (attributes2 != TypeAttributes.AutoLayout)
                {
                    if (attributes2 != TypeAttributes.Interface)
                    {
                        return;
                    }
                }
                else
                {
                    if ((typeAttributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
                    {
                        base.Output.Write("final ");
                    }
                    if ((typeAttributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
                    {
                        base.Output.Write("abstract ");
                    }
                    base.Output.Write("class ");
                    return;
                }
                base.Output.Write("interface ");
            }
        }

        private void ParseForUnsupportedProxyTree(CodeNamespace e)
        {
            bool flag = false;
            bool flag2 = false;
            this.fDisableEnumHack = false;
            foreach (CodeTypeDeclaration declaration in e.Types)
            {
                if (this.IsProxyType(declaration))
                {
                    flag = true;
                }
                if (declaration.IsEnum)
                {
                    flag2 = true;
                }
            }
            if (flag && flag2)
            {
                foreach (CodeTypeDeclaration declaration2 in e.Types)
                {
                    if (this.IsProxyType(declaration2) && this.IsEncodedProxy(declaration2))
                    {
                        this.fDisableEnumHack = true;
                        break;
                    }
                }
            }
        }

        protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
        {
            if (outputReg == null)
            {
                outputReg = new Regex(@"(^([^(]+)(\(([0-9]+),([0-9]+)\))?: )?(error|warning) ([A-Z]+[0-9]+): (.*)");
            }
            Match match = outputReg.Match(line);
            if (match.Success)
            {
                CompilerError error = new CompilerError();
                if (match.Groups[3].Success)
                {
                    error.FileName = match.Groups[2].Value;
                    error.Line = int.Parse(match.Groups[4].Value);
                    error.Column = int.Parse(match.Groups[5].Value);
                }
                if (string.Compare(match.Groups[6].Value, "warning", true, CultureInfo.InvariantCulture) == 0)
                {
                    error.IsWarning = true;
                }
                error.ErrorNumber = match.Groups[7].Value;
                error.ErrorText = match.Groups[8].Value;
                results.Errors.Add(error);
            }
        }

        protected override string QuoteSnippetString(string value)
        {
            return this.QuoteSnippetString(value, true);
        }

        private string QuoteSnippetString(string value, bool fMultiLine)
        {
            return this.QuoteSnippetStringCStyle(value, fMultiLine);
        }

        private string QuoteSnippetStringCStyle(string value, bool fMultiLine)
        {
            StringBuilder b = new StringBuilder(value.Length + 5);
            b.Append("\"");
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '\u2028':
                    case '\u2029':
                        this.AppendEscapedChar(b, value[i]);
                        break;

                    case '\\':
                        b.Append(@"\\");
                        break;

                    case '\'':
                        b.Append(@"\'");
                        break;

                    case '\t':
                        b.Append(@"\t");
                        break;

                    case '\n':
                        b.Append(@"\n");
                        break;

                    case '\r':
                        b.Append(@"\r");
                        break;

                    case '"':
                        b.Append("\\\"");
                        break;

                    case '\0':
                        b.Append(@"\0");
                        break;

                    default:
                        b.Append(value[i]);
                        break;
                }
                if ((fMultiLine && (i > 0)) && ((i % 80) == 0))
                {
                    b.Append("\" +\r\n\"");
                }
            }
            b.Append("\"");
            return b.ToString();
        }

        protected override bool Supports(GeneratorSupport support)
        {
            return ((support & (GeneratorSupport.Win32Resources | GeneratorSupport.ComplexExpressions | GeneratorSupport.PublicStaticMembers | GeneratorSupport.NestedTypes | GeneratorSupport.ParameterAttributes | GeneratorSupport.AssemblyAttributes | GeneratorSupport.DeclareEvents | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareDelegates | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.TryCatchStatements | GeneratorSupport.StaticConstructors | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.ArraysOfArrays)) == support);
        }

        protected override string CompilerName
        {
            get
            {
                return "vjc.exe";
            }
        }

        protected override string FileExtension
        {
            get
            {
                return "jsl";
            }
        }

        protected override string NullToken
        {
            get
            {
                return "null";
            }
        }

        internal class FixedStringLookup
        {
            internal static bool Contains(string[][] lookupTable, string value, bool ignoreCase)
            {
                int length = value.Length;
                if ((length <= 0) || ((length - 1) >= lookupTable.Length))
                {
                    return false;
                }
                string[] array = lookupTable[length - 1];
                if (array == null)
                {
                    return false;
                }
                return Contains(array, value, ignoreCase);
            }

            private static bool Contains(string[] array, string value, bool ignoreCase)
            {
                int index = 0;
                int length = array.Length;
                int pos = 0;
                while (pos < value.Length)
                {
                    char ch;
                    if (ignoreCase)
                    {
                        ch = char.ToLower(value[pos], CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        ch = value[pos];
                    }
                    if ((length - index) <= 1)
                    {
                        if (ch != array[index][pos])
                        {
                            return false;
                        }
                        pos++;
                    }
                    else
                    {
                        if (!FindCharacter(array, ch, pos, ref index, ref length))
                        {
                            return false;
                        }
                        pos++;
                    }
                }
                return true;
            }

            private static bool FindCharacter(string[] array, char value, int pos, ref int min, ref int max)
            {
                int index = min;
                while (min < max)
                {
                    index = (min + max) / 2;
                    char ch = array[index][pos];
                    if (value == ch)
                    {
                        int num2 = index;
                        while ((num2 > min) && (array[num2 - 1][pos] == value))
                        {
                            num2--;
                        }
                        min = num2;
                        int num3 = index + 1;
                        while ((num3 < max) && (array[num3][pos] == value))
                        {
                            num3++;
                        }
                        max = num3;
                        return true;
                    }
                    if (value < ch)
                    {
                        max = index;
                    }
                    else
                    {
                        min = index + 1;
                    }
                }
                return false;
            }
        }
    }
}

