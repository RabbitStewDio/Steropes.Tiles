using System;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public interface IDetailEditor<T>
  {
    bool Valid { get; }
    bool Visible { get; set; }
    void ApplyFrom(T data);
    T ApplyTo(T data);

    event EventHandler InputReceived;

  }
}