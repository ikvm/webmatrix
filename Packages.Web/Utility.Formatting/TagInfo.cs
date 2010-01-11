namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal class TagInfo
    {
        private FormattingFlags _flags;
        private WhiteSpaceType _following;
        private WhiteSpaceType _inner;
        private string _tagName;
        private ElementType _type;

        public TagInfo(string tagName, FormattingFlags flags) : this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Other)
        {
        }

        public TagInfo(string newTagName, TagInfo info)
        {
            this._tagName = newTagName;
            this._inner = info.InnerWhiteSpaceType;
            this._following = info.FollowingWhiteSpaceType;
            this._flags = info.Flags;
            this._type = info.Type;
        }

        public TagInfo(string tagName, FormattingFlags flags, ElementType type) : this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, type)
        {
        }

        public TagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace) : this(tagName, flags, innerWhiteSpace, followingWhiteSpace, ElementType.Other)
        {
        }

        public TagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace, ElementType type)
        {
            this._tagName = tagName;
            this._inner = innerWhiteSpace;
            this._following = followingWhiteSpace;
            this._flags = flags;
            this._type = type;
        }

        public virtual bool CanContainTag(TagInfo info)
        {
            return true;
        }

        public FormattingFlags Flags
        {
            get
            {
                return this._flags;
            }
        }

        public WhiteSpaceType FollowingWhiteSpaceType
        {
            get
            {
                return this._following;
            }
        }

        public WhiteSpaceType InnerWhiteSpaceType
        {
            get
            {
                return this._inner;
            }
        }

        public bool IsComment
        {
            get
            {
                return ((this._flags & FormattingFlags.Comment) != FormattingFlags.None);
            }
        }

        public bool IsInline
        {
            get
            {
                return ((this._flags & FormattingFlags.Inline) != FormattingFlags.None);
            }
        }

        public bool IsXml
        {
            get
            {
                return ((this._flags & FormattingFlags.Xml) != FormattingFlags.None);
            }
        }

        public bool NoEndTag
        {
            get
            {
                return ((this._flags & FormattingFlags.NoEndTag) != FormattingFlags.None);
            }
        }

        public bool NoIndent
        {
            get
            {
                if ((this._flags & FormattingFlags.NoIndent) == FormattingFlags.None)
                {
                    return this.NoEndTag;
                }
                return true;
            }
        }

        public bool PreserveContent
        {
            get
            {
                return ((this._flags & FormattingFlags.PreserveContent) != FormattingFlags.None);
            }
        }

        public string TagName
        {
            get
            {
                return this._tagName;
            }
        }

        public ElementType Type
        {
            get
            {
                return this._type;
            }
        }
    }
}

