namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Services;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class SqlCodeWizard
    {
        private SqlCodeWizard()
        {
        }

        private static CodeStatementCollection BuildCommand(IDataProviderDatabase database)
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(IDbCommand), "dbCommand");
            CodeExpression[] parameters = new CodeExpression[0];
            statement.InitExpression = new CodeObjectCreateExpression(database.CommandType, parameters);
            statements.Add(statement);
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("dbCommand");
            CodeVariableReferenceExpression right = new CodeVariableReferenceExpression("queryString");
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, "CommandText");
            statements.Add(new CodeAssignStatement(left, right));
            CodeVariableReferenceExpression expression4 = new CodeVariableReferenceExpression("dbConnection");
            CodePropertyReferenceExpression expression5 = new CodePropertyReferenceExpression(targetObject, "Connection");
            statements.Add(new CodeAssignStatement(expression5, expression4));
            return statements;
        }

        private static CodeStatementCollection BuildConnection(IDataProviderDatabase database, string connectionString)
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(string), "connectionString");
            statement.InitExpression = new CodePrimitiveExpression(connectionString);
            statements.Add(statement);
            CodeVariableDeclarationStatement statement2 = new CodeVariableDeclarationStatement(typeof(IDbConnection), "dbConnection");
            CodeVariableReferenceExpression expression = new CodeVariableReferenceExpression("connectionString");
            statement2.InitExpression = new CodeObjectCreateExpression(database.ConnectionType, new CodeExpression[] { expression });
            statements.Add(statement2);
            return statements;
        }

        private static CodeStatementCollection BuildDbParameterAssignment(IDataProviderDatabase database, string paramName, DbType paramType, CodeDomProvider provider)
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            string name = "dbParam_" + GetVariableNameFromParameterName(paramName);
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(IDataParameter), name);
            CodeExpression[] parameters = new CodeExpression[0];
            statement.InitExpression = new CodeObjectCreateExpression(database.ParameterType, parameters);
            statements.Add(statement);
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression(name);
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, "ParameterName");
            CodePrimitiveExpression right = new CodePrimitiveExpression(paramName);
            statements.Add(new CodeAssignStatement(left, right));
            CodePropertyReferenceExpression expression4 = new CodePropertyReferenceExpression(targetObject, "Value");
            CodeVariableReferenceExpression expression = new CodeVariableReferenceExpression(GetVariableNameFromParameterName(paramName));
            CodeExpression expression6 = expression;
            if (provider.FileExtension.ToLower().EndsWith("jsl"))
            {
                System.Type typeFromDbType = QueryParameter.GetTypeFromDbType(paramType);
                if (typeFromDbType.IsPrimitive)
                {
                    expression6 = new CodeCastExpression(typeFromDbType.FullName + " ", expression);
                }
            }
            statements.Add(new CodeAssignStatement(expression4, expression6));
            CodePropertyReferenceExpression expression7 = new CodePropertyReferenceExpression(targetObject, "DbType");
            CodeTypeReferenceExpression expression8 = new CodeTypeReferenceExpression(typeof(DbType));
            CodeFieldReferenceExpression expression9 = new CodeFieldReferenceExpression(expression8, Enum.GetName(typeof(DbType), paramType));
            statements.Add(new CodeAssignStatement(expression7, expression9));
            CodeVariableReferenceExpression expression10 = new CodeVariableReferenceExpression("dbCommand");
            CodePropertyReferenceExpression expression11 = new CodePropertyReferenceExpression(expression10, "Parameters");
            CodeMethodInvokeExpression expression12 = new CodeMethodInvokeExpression(expression11, "Add", new CodeExpression[] { targetObject });
            statements.Add(expression12);
            return statements;
        }

        private static CodeStatementCollection BuildExecuteNonQuery()
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(int), "rowsAffected");
            statement.InitExpression = new CodePrimitiveExpression(0);
            statements.Add(statement);
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("dbConnection");
            CodeMethodInvokeExpression expression2 = new CodeMethodInvokeExpression(targetObject, "Open", new CodeExpression[0]);
            statements.Add(expression2);
            CodeTryCatchFinallyStatement statement2 = new CodeTryCatchFinallyStatement();
            CodeVariableReferenceExpression left = new CodeVariableReferenceExpression("rowsAffected");
            CodeVariableReferenceExpression expression4 = new CodeVariableReferenceExpression("dbCommand");
            CodeMethodInvokeExpression right = new CodeMethodInvokeExpression(expression4, "ExecuteNonQuery", new CodeExpression[0]);
            CodeAssignStatement statement3 = new CodeAssignStatement(left, right);
            statement2.TryStatements.Add(statement3);
            CodeMethodInvokeExpression expression6 = new CodeMethodInvokeExpression(targetObject, "Close", new CodeExpression[0]);
            statement2.FinallyStatements.Add(expression6);
            statements.Add(statement2);
            statements.Add(new CodeSnippetStatement(string.Empty));
            CodeMethodReturnStatement statement4 = new CodeMethodReturnStatement(left);
            statements.Add(statement4);
            return statements;
        }

        private static CodeStatement BuildQueryString(string queryString)
        {
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(string), "queryString");
            statement.InitExpression = new CodePrimitiveExpression(queryString);
            return statement;
        }

        private static CodeStatementCollection BuildSelectReturnDataReader()
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("dbConnection");
            CodeMethodInvokeExpression expression2 = new CodeMethodInvokeExpression(targetObject, "Open", new CodeExpression[0]);
            statements.Add(expression2);
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(IDataReader), "dataReader");
            CodeVariableReferenceExpression expression = new CodeVariableReferenceExpression("dataReader");
            CodeVariableReferenceExpression expression4 = new CodeVariableReferenceExpression("dbCommand");
            CodeTypeReferenceExpression expression5 = new CodeTypeReferenceExpression(typeof(CommandBehavior));
            CodeFieldReferenceExpression expression6 = new CodeFieldReferenceExpression(expression5, "CloseConnection");
            CodeMethodInvokeExpression expression7 = new CodeMethodInvokeExpression(expression4, "ExecuteReader", new CodeExpression[] { expression6 });
            statement.InitExpression = expression7;
            statements.Add(statement);
            statements.Add(new CodeSnippetStatement(string.Empty));
            CodeMethodReturnStatement statement2 = new CodeMethodReturnStatement(expression);
            statements.Add(statement2);
            return statements;
        }

        private static CodeStatementCollection BuildSelectReturnDataSet(IDataProviderDatabase database)
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(IDbDataAdapter), "dataAdapter");
            CodeExpression[] parameters = new CodeExpression[0];
            statement.InitExpression = new CodeObjectCreateExpression(database.AdapterType, parameters);
            statements.Add(statement);
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("dataAdapter");
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, "SelectCommand");
            CodeVariableReferenceExpression right = new CodeVariableReferenceExpression("dbCommand");
            statements.Add(new CodeAssignStatement(left, right));
            CodeVariableDeclarationStatement statement2 = new CodeVariableDeclarationStatement(typeof(DataSet), "dataSet");
            statement2.InitExpression = new CodeObjectCreateExpression(typeof(DataSet), new CodeExpression[0]);
            statements.Add(statement2);
            CodeVariableReferenceExpression expression = new CodeVariableReferenceExpression("dataSet");
            CodeMethodInvokeExpression expression5 = new CodeMethodInvokeExpression(targetObject, "Fill", new CodeExpression[] { expression });
            statements.Add(expression5);
            statements.Add(new CodeSnippetStatement(string.Empty));
            CodeMethodReturnStatement statement3 = new CodeMethodReturnStatement(expression);
            statements.Add(statement3);
            return statements;
        }

        private static string GetVariableNameFromParameterName(string paramName)
        {
            return (char.ToLower(paramName[1]) + paramName.Substring(2));
        }

        public static string Run(CodeWizard codeWizard, IServiceProvider serviceProvider, CodeDomProvider codeDomProvider, QueryBuilderType type)
        {
            IUIService service = (IUIService) serviceProvider.GetService(typeof(IUIService));
            IDatabaseManager manager = (IDatabaseManager) serviceProvider.GetService(typeof(IDatabaseManager));
            if (codeDomProvider != null)
            {
                ICodeGenerator generator = codeDomProvider.CreateGenerator();
                if (generator != null)
                {
                    QueryBuilder form = new QueryBuilder(codeWizard, type);
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        StringWriter w = new StringWriter();
                        CodeGeneratorOptions o = new CodeGeneratorOptions();
                        CodeMemberMethod method = new CodeMemberMethod();
                        method.Name = form.MethodName;
                        IDictionary dictionary = new HybridDictionary(true);
                        foreach (QueryParameter parameter in form.Parameters)
                        {
                            string variableNameFromParameterName = GetVariableNameFromParameterName(parameter.Name);
                            if (dictionary[variableNameFromParameterName] == null)
                            {
                                System.Type typeFromDbType = QueryParameter.GetTypeFromDbType(parameter.Type);
                                method.Parameters.Add(new CodeParameterDeclarationExpression(typeFromDbType, variableNameFromParameterName));
                                dictionary[variableNameFromParameterName] = string.Empty;
                            }
                        }
                        method.ReturnType = new CodeTypeReference(form.ReturnType);
                        method.Attributes = MemberAttributes.Final;
                        method.Statements.AddRange(BuildConnection((IDataProviderDatabase) form.Database, ((IDataProviderDatabase) form.Database).ConnectionString));
                        method.Statements.Add(new CodeSnippetStatement(string.Empty));
                        method.Statements.Add(BuildQueryString(form.Query));
                        method.Statements.AddRange(BuildCommand((IDataProviderDatabase) form.Database));
                        method.Statements.Add(new CodeSnippetStatement(string.Empty));
                        int num = 0;
                        dictionary = new HybridDictionary(true);
                        foreach (QueryParameter parameter2 in form.Parameters)
                        {
                            string str2 = GetVariableNameFromParameterName(parameter2.Name);
                            if (dictionary[str2] == null)
                            {
                                method.Statements.AddRange(BuildDbParameterAssignment((IDataProviderDatabase) form.Database, parameter2.Name, parameter2.Type, codeDomProvider));
                                num++;
                                dictionary[str2] = string.Empty;
                            }
                        }
                        if (num > 0)
                        {
                            method.Statements.Add(new CodeSnippetStatement(string.Empty));
                        }
                        if (type == QueryBuilderType.Select)
                        {
                            if (form.ReturnType == typeof(IDataReader))
                            {
                                method.Statements.AddRange(BuildSelectReturnDataReader());
                            }
                            else
                            {
                                method.Statements.AddRange(BuildSelectReturnDataSet((IDataProviderDatabase) form.Database));
                            }
                        }
                        else
                        {
                            method.Statements.AddRange(BuildExecuteNonQuery());
                        }
                        CodeTypeDeclaration e = new CodeTypeDeclaration("TempType");
                        e.Members.Add(method);
                        generator.GenerateCodeFromType(e, w, o);
                        string s = w.ToString();
                        w = new StringWriter();
                        CodeTypeDeclaration declaration2 = new CodeTypeDeclaration("TempType");
                        generator.GenerateCodeFromType(declaration2, w, o);
                        string str4 = w.ToString();
                        w = new StringWriter();
                        StringReader reader = new StringReader(s);
                        StringReader reader2 = new StringReader(str4);
                        string str5 = reader.ReadLine();
                        string str6 = reader2.ReadLine();
                        while (str5 != null)
                        {
                            if (str5 != str6)
                            {
                                w.WriteLine(str5);
                            }
                            else
                            {
                                str6 = reader2.ReadLine();
                            }
                            str5 = reader.ReadLine();
                        }
                        return w.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}

