using System;
using System.IO;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  class SaveException : IOException
  {
    public string File { get; }

    public SaveException(string file, 
                         string message, 
                         Exception innerException) : base(message, innerException)
    {
      File = file;
    }

    public SaveException(string file)
    {
      File = file;
    }

    public SaveException(string message, string file) : base(message)
    {
      File = file;
    }
  }
}