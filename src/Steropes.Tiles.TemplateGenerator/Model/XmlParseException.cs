using JetBrains.Annotations;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Steropes.Tiles.TemplateGenerator.Model
{
    public class XmlParseException : IOException
    {
        public XmlParseException()
        { }

        public XmlParseException(string message) : base(message)
        { }

        public XmlParseException(string message, IXmlLineInfo lineInfo) : base(AppendLineInfo(message, lineInfo))
        { }

        static string AppendLineInfo(string message, IXmlLineInfo lineInfo)
        {
            if (lineInfo?.HasLineInfo() == true)
            {
                return $"{message} [{lineInfo.LineNumber}:{lineInfo.LinePosition}]";
            }

            return message;
        }

        public XmlParseException(string message, int hresult) : base(message, hresult)
        { }

        public XmlParseException(string message, Exception innerException) : base(message, innerException)
        { }

        protected XmlParseException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
