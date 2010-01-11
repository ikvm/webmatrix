namespace Microsoft.Matrix.Packages.Web.Utility
{
    using Microsoft.Matrix.Packages.Web.Utility.Formatting;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Collections.Specialized;

    internal sealed class HtmlFormatter
    {
        private static TagInfo commentTag = new TagInfo("", FormattingFlags.Comment | FormattingFlags.NoEndTag, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Any);
        private static TagInfo directiveTag = new TagInfo("", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo nestedXmlTag = new TagInfo("", FormattingFlags.AllowPartialTags, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo otherServerSideScriptTag = new TagInfo("", FormattingFlags.NoEndTag | FormattingFlags.Inline, ElementType.Any);
        private static IDictionary tagTable = new HybridDictionary(true);
        private static TagInfo unknownHtmlTag = new TagInfo("", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo unknownXmlTag = new TagInfo("", FormattingFlags.Xml, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);

        static HtmlFormatter()
        {
            tagTable["a"] = new TagInfo("a", FormattingFlags.Inline, ElementType.Inline);
            tagTable["acronym"] = new TagInfo("acronym", FormattingFlags.Inline, ElementType.Inline);
            tagTable["address"] = new TagInfo("address", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["applet"] = new TagInfo("applet", FormattingFlags.Inline, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["area"] = new TagInfo("area", FormattingFlags.NoEndTag);
            tagTable["asp:Label"] = new TagInfo("asp:Label", FormattingFlags.Inline, ElementType.Inline);
            tagTable["b"] = new TagInfo("b", FormattingFlags.Inline, ElementType.Inline);
            tagTable["base"] = new TagInfo("base", FormattingFlags.NoEndTag);
            tagTable["basefont"] = new TagInfo("basefont", FormattingFlags.NoEndTag, ElementType.Block);
            tagTable["bdo"] = new TagInfo("bdo", FormattingFlags.Inline, ElementType.Inline);
            tagTable["bgsound"] = new TagInfo("bgsound", FormattingFlags.NoEndTag);
            tagTable["big"] = new TagInfo("big", FormattingFlags.Inline, ElementType.Inline);
            tagTable["blink"] = new TagInfo("blink", FormattingFlags.Inline);
            tagTable["blockquote"] = new TagInfo("blockquote", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["body"] = new TagInfo("body", FormattingFlags.None);
            tagTable["br"] = new TagInfo("br", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Inline);
            tagTable["button"] = new TagInfo("button", FormattingFlags.Inline, ElementType.Inline);
            tagTable["caption"] = new TagInfo("caption", FormattingFlags.None);
            tagTable["cite"] = new TagInfo("cite", FormattingFlags.Inline, ElementType.Inline);
            tagTable["center"] = new TagInfo("center", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["code"] = new TagInfo("code", FormattingFlags.Inline, ElementType.Inline);
            tagTable["col"] = new TagInfo("col", FormattingFlags.NoEndTag);
            tagTable["colgroup"] = new TagInfo("colgroup", FormattingFlags.None);
            tagTable["dd"] = new TagInfo("dd", FormattingFlags.None);
            tagTable["del"] = new TagInfo("del", FormattingFlags.None);
            tagTable["dfn"] = new TagInfo("dfn", FormattingFlags.Inline, ElementType.Inline);
            tagTable["dir"] = new TagInfo("dir", FormattingFlags.None, ElementType.Block);
            tagTable["div"] = new TagInfo("div", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["dl"] = new TagInfo("dl", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["dt"] = new TagInfo("dt", FormattingFlags.Inline);
            tagTable["em"] = new TagInfo("em", FormattingFlags.Inline, ElementType.Inline);
            tagTable["embed"] = new TagInfo("embed", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough, ElementType.Inline);
            tagTable["fieldset"] = new TagInfo("fieldset", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["font"] = new TagInfo("font", FormattingFlags.Inline, ElementType.Inline);
            tagTable["form"] = new TagInfo("form", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["frame"] = new TagInfo("frame", FormattingFlags.NoEndTag);
            tagTable["frameset"] = new TagInfo("frameset", FormattingFlags.None);
            tagTable["head"] = new TagInfo("head", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
            tagTable["h1"] = new TagInfo("h1", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h2"] = new TagInfo("h2", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h3"] = new TagInfo("h3", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h4"] = new TagInfo("h4", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h5"] = new TagInfo("h5", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h6"] = new TagInfo("h6", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["hr"] = new TagInfo("hr", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["html"] = new TagInfo("html", FormattingFlags.NoIndent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
            tagTable["i"] = new TagInfo("i", FormattingFlags.Inline, ElementType.Inline);
            tagTable["iframe"] = new TagInfo("iframe", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Inline);
            tagTable["img"] = new TagInfo("img", FormattingFlags.NoEndTag | FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["input"] = new TagInfo("input", FormattingFlags.NoEndTag, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["ins"] = new TagInfo("ins", FormattingFlags.None);
            tagTable["isindex"] = new TagInfo("isindex", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough, ElementType.Block);
            tagTable["kbd"] = new TagInfo("kbd", FormattingFlags.Inline, ElementType.Inline);
            tagTable["label"] = new TagInfo("label", FormattingFlags.Inline, ElementType.Inline);
            tagTable["legend"] = new TagInfo("legend", FormattingFlags.None);
            tagTable["li"] = new LITagInfo();
            tagTable["link"] = new TagInfo("link", FormattingFlags.NoEndTag);
            tagTable["listing"] = new TagInfo("listing", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["map"] = new TagInfo("map", FormattingFlags.Inline, ElementType.Inline);
            tagTable["marquee"] = new TagInfo("marquee", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["menu"] = new TagInfo("menu", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["meta"] = new TagInfo("meta", FormattingFlags.NoEndTag);
            tagTable["nobr"] = new TagInfo("nobr", FormattingFlags.NoEndTag | FormattingFlags.Inline, ElementType.Inline);
            tagTable["noembed"] = new TagInfo("noembed", FormattingFlags.None, ElementType.Block);
            tagTable["noframes"] = new TagInfo("noframes", FormattingFlags.None, ElementType.Block);
            tagTable["noscript"] = new TagInfo("noscript", FormattingFlags.None, ElementType.Block);
            tagTable["object"] = new TagInfo("object", FormattingFlags.None, ElementType.Inline);
            tagTable["ol"] = new OLTagInfo();
            tagTable["option"] = new TagInfo("option", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough);
            tagTable["p"] = new PTagInfo();
            tagTable["param"] = new TagInfo("param", FormattingFlags.NoEndTag);
            tagTable["pre"] = new TagInfo("pre", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["q"] = new TagInfo("q", FormattingFlags.Inline, ElementType.Inline);
            tagTable["rt"] = new TagInfo("rt", FormattingFlags.None);
            tagTable["ruby"] = new TagInfo("ruby", FormattingFlags.None, ElementType.Inline);
            tagTable["s"] = new TagInfo("s", FormattingFlags.Inline, ElementType.Inline);
            tagTable["samp"] = new TagInfo("samp", FormattingFlags.None, ElementType.Inline);
            tagTable["script"] = new TagInfo("script", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Inline);
            tagTable["select"] = new TagInfo("select", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Block);
            tagTable["small"] = new TagInfo("small", FormattingFlags.Inline, ElementType.Inline);
            tagTable["span"] = new TagInfo("span", FormattingFlags.Inline, ElementType.Inline);
            tagTable["strike"] = new TagInfo("strike", FormattingFlags.Inline, ElementType.Inline);
            tagTable["strong"] = new TagInfo("strong", FormattingFlags.Inline, ElementType.Inline);
            tagTable["style"] = new TagInfo("style", FormattingFlags.PreserveContent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
            tagTable["sub"] = new TagInfo("sub", FormattingFlags.Inline, ElementType.Inline);
            tagTable["sup"] = new TagInfo("sup", FormattingFlags.Inline, ElementType.Inline);
            tagTable["table"] = new TagInfo("table", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["tbody"] = new TagInfo("tbody", FormattingFlags.None);
            tagTable["td"] = new TDTagInfo();
            tagTable["textarea"] = new TagInfo("textarea", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["tfoot"] = new TagInfo("tfoot", FormattingFlags.None);
            tagTable["th"] = new TagInfo("th", FormattingFlags.None);
            tagTable["thead"] = new TagInfo("thead", FormattingFlags.None);
            tagTable["title"] = new TagInfo("title", FormattingFlags.Inline);
            tagTable["tr"] = new TRTagInfo();
            tagTable["tt"] = new TagInfo("tt", FormattingFlags.Inline, ElementType.Inline);
            tagTable["u"] = new TagInfo("u", FormattingFlags.Inline, ElementType.Inline);
            tagTable["ul"] = new TagInfo("ul", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["xml"] = new TagInfo("xml", FormattingFlags.Xml, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["xmp"] = new TagInfo("xmp", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["var"] = new TagInfo("var", FormattingFlags.Inline, ElementType.Inline);
            tagTable["wbr"] = new TagInfo("wbr", FormattingFlags.NoEndTag | FormattingFlags.Inline, ElementType.Inline);
        }

		public void Format(string input, TextWriter output, HtmlFormatterOptions options)
		{
			bool makeXhtml = options.MakeXhtml;
			int maxLineLength = options.MaxLineLength;
			string indentString = new string(options.IndentChar, options.IndentSize);
			char[] chars = input.ToCharArray();
			System.Collections.Generic.Stack<FormatInfo> formatInfoStack = new System.Collections.Generic.Stack<FormatInfo>();
			System.Collections.Generic.Stack<HtmlWriter> writerStack = new System.Collections.Generic.Stack<HtmlWriter>();
			FormatInfo curTagFormatInfo = null;
			FormatInfo prevTokenFormatInfo = null;
			string s = string.Empty;
			bool flag2 = false;
			bool hasNoEndTag = false;
			bool flag4 = false;
			HtmlWriter writer = new HtmlWriter(output, indentString, maxLineLength);
			writerStack.Push(writer);
			Token curToken = HtmlTokenizer.GetFirstToken(chars);
			Token prevToken = curToken;
			while (curToken != null)
			{
				string text;
				string tagName;
				TagInfo tagInfo;
				writer = (HtmlWriter) writerStack.Peek();
				switch (curToken.Type)
				{
					case Token.Whitespace:
						if (curToken.Text.Length > 0)
						{
							writer.Write(' ');
						}
						goto Label_Get_Next_Token;

					case Token.TagName:
					case Token.Comment:
					case Token.InlineServerScript:
						hasNoEndTag = false;
						if (curToken.Type != Token.Comment)
						{
							goto Label_Token_TagName_Or_InlineServerScript;
						}

						tagName = curToken.Text;
						tagInfo = new TagInfo(curToken.Text, commentTag);
						goto Label_Process_Comment;

					case Token.AttrName:
						if (!makeXhtml)
						{
							goto Label_0164;
						}
						text = string.Empty;
						if (curTagFormatInfo.tagInfo.IsXml)
						{
							break;
						}
						text = curToken.Text.ToLower();
						goto Label_0127;

					case Token.AttrVal:
						if ((!makeXhtml || (prevToken.Type == 13)) || (prevToken.Type == 14))
						{
							goto Label_0227;
						}
						writer.Write('"');
						writer.Write(curToken.Text.Replace("\"", "&quot;"));
						writer.Write('"');
						goto Label_Get_Next_Token;

					case Token.TextToken:
					case Token.ClientScriptBlock:
					case Token.Style:
					case Token.ServerScriptBlock:
						s = s + curToken.Text;
						goto Label_Get_Next_Token;

					case Token.SelfTerminating:
						curTagFormatInfo.isEndTag = true;
						if (!curTagFormatInfo.tagInfo.NoEndTag)
						{
							formatInfoStack.Pop();
							if (curTagFormatInfo.tagInfo.IsXml)
							{
								HtmlWriter writer2 = (HtmlWriter) writerStack.Pop();
								writer = (HtmlWriter) writerStack.Peek();
								writer.Write(writer2.Content);
							}
						}
						if ((prevToken.Type == Token.Whitespace) && (prevToken.Text.Length > 0))
						{
							writer.Write("/>");
						}
						else
						{
							writer.Write(" />");
						}
						goto Label_Get_Next_Token;

					case Token.Error:
						if (prevToken.Type == Token.OpenBracket)
						{
							writer.Write('<');
						}
						writer.Write(curToken.Text);
						goto Label_Get_Next_Token;

					case Token.CloseBracket:
						if (!makeXhtml)
						{
							goto Label_027A;
						}
						if (!flag4)
						{
                            goto Label_Process_CloseBracket; // proc CloseBracket
						}
						flag4 = false;
						goto Label_Get_Next_Token;

					case Token.DoubleQuote:
						writer.Write('"');
						goto Label_Get_Next_Token;

					case Token.SingleQuote:
						writer.Write('\'');
						goto Label_Get_Next_Token;

					case Token.EqualsChar:
						writer.Write('=');
						goto Label_Get_Next_Token;

					case Token.XmlDirective:
						writer.WriteLineIfNotOnNewLine();
						writer.Write('<');
						writer.Write(curToken.Text);
						writer.Write('>');
						writer.WriteLineIfNotOnNewLine();
						curTagFormatInfo = new FormatInfo(directiveTag, false);
						flag4 = true;
						goto Label_Get_Next_Token;

					default:
						goto Label_Get_Next_Token;
				}

				text = curToken.Text;

			Label_0127:
				writer.Write(text);
				if (HtmlTokenizer.GetNextToken(curToken).Type != 15)
				{
					writer.Write("=\"" + text + "\"");
				}
				goto Label_Get_Next_Token;

			Label_0164:
				if (!curTagFormatInfo.tagInfo.IsXml)
				{
					if (options.AttributeCasing == HtmlFormatterCase.UpperCase)
					{
						writer.Write(curToken.Text.ToUpper());
					}
					else if (options.AttributeCasing == HtmlFormatterCase.LowerCase)
					{
						writer.Write(curToken.Text.ToLower());
					}
					else
					{
						writer.Write(curToken.Text);
					}
				}
				else
				{
					writer.Write(curToken.Text);
				}
				goto Label_Get_Next_Token;

			Label_0227:
				writer.Write(curToken.Text);
				goto Label_Get_Next_Token;

			Label_Process_CloseBracket: // write closebucket
                if (hasNoEndTag && !curTagFormatInfo.tagInfo.IsComment) // flag3 = NoEndTag
				{
					writer.Write(" />");
				}
				else
				{
					writer.Write('>');
				}
				goto Label_Get_Next_Token;

			Label_027A:
				writer.Write('>');
				goto Label_Get_Next_Token;

			Label_Token_TagName_Or_InlineServerScript:
				if (curToken.Type == Token.InlineServerScript)
				{
					string newTagName = curToken.Text.Trim().Substring(1);
					tagName = newTagName;
					if (newTagName.StartsWith("%@"))
					{
						tagInfo = new TagInfo(newTagName, directiveTag);
					}
					else
					{
						tagInfo = new TagInfo(newTagName, otherServerSideScriptTag);
					}
				}
				else
				{
					tagName = curToken.Text;
					tagInfo = tagTable[tagName] as TagInfo;
					if (tagInfo == null)
					{
						if (tagName.IndexOf(':') > -1)
						{
							tagInfo = new TagInfo(tagName, unknownXmlTag);
						}
						else if (writer is XmlWriter)
						{
							tagInfo = new TagInfo(tagName, nestedXmlTag);
						}
						else
						{
							tagInfo = new TagInfo(tagName, unknownHtmlTag);
						}
					}
					else if ((options.ElementCasing == HtmlFormatterCase.LowerCase) || makeXhtml)
					{
						tagName = tagInfo.TagName;
					}
					else if (options.ElementCasing == HtmlFormatterCase.UpperCase)
					{
						tagName = tagInfo.TagName.ToUpper();
					}
				}

			Label_Process_Comment:
				if (curTagFormatInfo == null)
				{
					curTagFormatInfo = new FormatInfo(tagInfo, false);
					curTagFormatInfo.indent = 0;
					formatInfoStack.Push(curTagFormatInfo);
					writer.Write(s);
					if (tagInfo.IsXml)
					{
						HtmlWriter writer3 = new XmlWriter(writer.Indent, tagInfo.TagName, indentString, maxLineLength);
						writerStack.Push(writer3);
						writer = writer3;
					}
					if (prevToken.Type == Token.ForwardSlash)
					{
						writer.Write("</");
					}
					else
					{
						writer.Write('<');
					}
					writer.Write(tagName);
					s = string.Empty;
				}
				else
				{
					WhiteSpaceType followingWhiteSpaceType;
					prevTokenFormatInfo = new FormatInfo(tagInfo, prevToken.Type == Token.ForwardSlash);
					followingWhiteSpaceType = curTagFormatInfo.isEndTag ? curTagFormatInfo.tagInfo.FollowingWhiteSpaceType : curTagFormatInfo.tagInfo.InnerWhiteSpaceType;
					bool isInline = curTagFormatInfo.tagInfo.IsInline;
					bool flag6 = false;
					bool flag7 = false;
					if (writer is XmlWriter)
					{
						XmlWriter writer4 = (XmlWriter) writer;
						if (writer4.IsUnknownXml)
						{
							flag7 = ((curTagFormatInfo.isBeginTag && (curTagFormatInfo.tagInfo.TagName.ToLower() == writer4.TagName.ToLower())) || (prevTokenFormatInfo.isEndTag && (prevTokenFormatInfo.tagInfo.TagName.ToLower() == writer4.TagName.ToLower()))) && !FormattedTextWriter.IsWhiteSpace(s);
						}
						if (curTagFormatInfo.isBeginTag)
						{
							if (FormattedTextWriter.IsWhiteSpace(s))
							{
								if ((writer4.IsUnknownXml && prevTokenFormatInfo.isEndTag) && (curTagFormatInfo.tagInfo.TagName.ToLower() == prevTokenFormatInfo.tagInfo.TagName.ToLower()))
								{
									isInline = true;
									flag6 = true;
									s = "";
								}
							}
							else if (!writer4.IsUnknownXml)
							{
								writer4.ContainsText = true;
							}
						}
					}
					bool frontWhiteSpace = true;
					if (curTagFormatInfo.isBeginTag && curTagFormatInfo.tagInfo.PreserveContent)
					{
						writer.Write(s);
					}
					else
					{
						switch (followingWhiteSpaceType)
						{
							case WhiteSpaceType.NotSignificant:
								if (!isInline && !flag7)
								{
									writer.WriteLineIfNotOnNewLine();
									frontWhiteSpace = false;
								}
								break;

							case WhiteSpaceType.Significant:
								if ((FormattedTextWriter.HasFrontWhiteSpace(s) && !isInline) && !flag7)
								{
									writer.WriteLineIfNotOnNewLine();
									frontWhiteSpace = false;
								}
								break;

							default:
								if (((followingWhiteSpaceType == WhiteSpaceType.CarryThrough) && (flag2 || FormattedTextWriter.HasFrontWhiteSpace(s))) && (!isInline && !flag7))
								{
									writer.WriteLineIfNotOnNewLine();
									frontWhiteSpace = false;
								}
								break;
						}
						if ((curTagFormatInfo.isBeginTag && !curTagFormatInfo.tagInfo.NoIndent) && !isInline)
						{
							writer.Indent++;
						}
						if (flag7)
						{
							writer.Write(s);
						}
						else
						{
							writer.WriteLiteral(s, frontWhiteSpace);
						}
					}
					if (prevTokenFormatInfo.isEndTag)
					{
						if (!prevTokenFormatInfo.tagInfo.NoEndTag)
						{
							//ArrayList list = new ArrayList();
							List<FormatInfo> formatInfoList = new List<FormatInfo>();
							FormatInfo info4 = null;
							bool flag9 = false;
							bool flag10 = false;
							if ((prevTokenFormatInfo.tagInfo.Flags & FormattingFlags.AllowPartialTags) != FormattingFlags.None)
							{
								flag10 = true;
							}
							if (formatInfoStack.Count > 0)
							{
								info4 = (FormatInfo) formatInfoStack.Pop();
								formatInfoList.Add(info4);
								while ((formatInfoStack.Count > 0) && (info4.tagInfo.TagName.ToLower() != prevTokenFormatInfo.tagInfo.TagName.ToLower()))
								{
									if ((info4.tagInfo.Flags & FormattingFlags.AllowPartialTags) != FormattingFlags.None)
									{
										flag10 = true;
										break;
									}
									info4 = (FormatInfo) formatInfoStack.Pop();
									formatInfoList.Add(info4);
								}
								if (info4.tagInfo.TagName.ToLower() != prevTokenFormatInfo.tagInfo.TagName.ToLower())
								{
									for (int i = formatInfoList.Count - 1; i >= 0; i--)
									{
										formatInfoStack.Push(formatInfoList[i]);
									}
								}
								else
								{
									flag9 = true;
									for (int j = 0; j < (formatInfoList.Count - 1); j++)
									{
										FormatInfo info5 = (FormatInfo) formatInfoList[j];
										if (info5.tagInfo.IsXml && (writerStack.Count > 1))
										{
											HtmlWriter writer5 = (HtmlWriter) writerStack.Pop();
											writer = (HtmlWriter) writerStack.Peek();
											writer.Write(writer5.Content);
										}
										if (!info5.tagInfo.NoEndTag)
										{
											writer.WriteLineIfNotOnNewLine();
											writer.Indent = info5.indent;
											if (makeXhtml && !flag10)
											{
												writer.Write("</" + info5.tagInfo.TagName + ">");
											}
										}
									}
									writer.Indent = info4.indent;
								}
							}
							if (flag9 || flag10)
							{
								if ((((!flag6 && !flag7) && (!prevTokenFormatInfo.tagInfo.IsInline && !prevTokenFormatInfo.tagInfo.PreserveContent)) && ((FormattedTextWriter.IsWhiteSpace(s) || FormattedTextWriter.HasBackWhiteSpace(s)) || (prevTokenFormatInfo.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant))) && (!(prevTokenFormatInfo.tagInfo is TDTagInfo) || FormattedTextWriter.HasBackWhiteSpace(s)))
								{
									writer.WriteLineIfNotOnNewLine();
								}
								writer.Write("</");
								writer.Write(tagName);
							}
							else
							{
								flag4 = true;
							}
							if (prevTokenFormatInfo.tagInfo.IsXml && (writerStack.Count > 1))
							{
								HtmlWriter writer6 = (HtmlWriter) writerStack.Pop();
								writer = (HtmlWriter) writerStack.Peek();
								writer.Write(writer6.Content);
							}
						}
						else
						{
							flag4 = true;
						}
					}
                    // prevTokenFormatInfo.isEndTag == false
					else
					{
						bool flag11 = false;
						while (!flag11 && (formatInfoStack.Count > 0))
						{
							FormatInfo info6 = (FormatInfo) formatInfoStack.Peek();
							if (!info6.tagInfo.CanContainTag(prevTokenFormatInfo.tagInfo))
							{
								formatInfoStack.Pop();
								writer.Indent = info6.indent;
								if (makeXhtml)
								{
									if (!info6.tagInfo.IsInline)
									{
										writer.WriteLineIfNotOnNewLine();
									}
									writer.Write("</" + info6.tagInfo.TagName + ">");
								}
							}
							flag11 = true;
						}
						prevTokenFormatInfo.indent = writer.Indent;
						if (((!flag7 && !prevTokenFormatInfo.tagInfo.IsInline) && !prevTokenFormatInfo.tagInfo.PreserveContent) && ((FormattedTextWriter.IsWhiteSpace(s) || FormattedTextWriter.HasBackWhiteSpace(s)) || ((s.Length == 0) && ((curTagFormatInfo.isBeginTag && (curTagFormatInfo.tagInfo.InnerWhiteSpaceType == WhiteSpaceType.NotSignificant)) || (curTagFormatInfo.isEndTag && (curTagFormatInfo.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant))))))
						{
							writer.WriteLineIfNotOnNewLine();
						}
						if (!prevTokenFormatInfo.tagInfo.NoEndTag)
						{
							formatInfoStack.Push(prevTokenFormatInfo);
						}
						else
						{
							hasNoEndTag = true;
						}
						if (prevTokenFormatInfo.tagInfo.IsXml)
						{
							HtmlWriter writer7 = new XmlWriter(writer.Indent, prevTokenFormatInfo.tagInfo.TagName, indentString, maxLineLength);
							writerStack.Push(writer7);
							writer = writer7;
						}
						writer.Write('<');
						writer.Write(tagName);
					}
					flag2 = FormattedTextWriter.HasBackWhiteSpace(s);
					s = string.Empty;
					curTagFormatInfo = prevTokenFormatInfo;
				}

			Label_Get_Next_Token:
				prevToken = curToken;
				curToken = HtmlTokenizer.GetNextToken(curToken);
			}
			if (s.Length > 0)
			{
				writer.Write(s);
			}
			while (writerStack.Count > 1)
			{
				HtmlWriter writer8 = (HtmlWriter) writerStack.Pop();
				writer = (HtmlWriter) writerStack.Peek();
				writer.Write(writer8.Content);
			}
			writer.Flush();
		}

        private delegate bool CanContainTag(TagInfo info);

        private class FormatInfo
        {
            public int indent;
            public bool isEndTag;
            public TagInfo tagInfo;

            public FormatInfo(TagInfo info, bool isEnd)
            {
                this.tagInfo = info;
                this.isEndTag = isEnd;
            }

            public bool isBeginTag
            {
                get
                {
                    return !this.isEndTag;
                }
            }
        }
    }
}

