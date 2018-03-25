using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Steropes.Tiles.Properties;

namespace Steropes.Tiles.Monogame.Tiles
{
  public class TexturePackLoaderException : IOException
  {
    public TexturePackLoaderException()
    {
    }

    public TexturePackLoaderException(string message) : base(message)
    {
    }

    public TexturePackLoaderException(string message, IXmlLineInfo lineInfo) : base(AppendLineInfo(message, lineInfo))
    {
    }

    static string AppendLineInfo(string message, IXmlLineInfo lineInfo)
    {
      if (lineInfo?.HasLineInfo() == true)
      {
        return $"{message} [{lineInfo.LineNumber}:{lineInfo.LinePosition}]";
      }

      return message;
    }

    public TexturePackLoaderException(string message, int hresult) : base(message, hresult)
    {
    }

    public TexturePackLoaderException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TexturePackLoaderException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}