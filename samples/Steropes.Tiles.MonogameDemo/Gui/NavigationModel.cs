using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.MonogameDemo.Gui
{
  /// <summary>
  ///  A data model for the navigation UI. This class contains the validation
  ///  code for converting text input into numbers to set a new view centre point.
  /// </summary>
  class NavigationModel : INotifyPropertyChanged
  {
    int? mapX;
    int? mapY;
    string mapXText;
    string mapYText;
    bool valid;

    public string MapXText
    {
      get { return mapXText; }
      set
      {
        mapXText = value;
        if (int.TryParse(mapXText, out int result))
        {
          mapX = result;
        }
        else
        {
          mapX = null;
        }
        Revalidate();
      }
    }

    public string MapYText
    {
      get { return mapYText; }
      set
      {
        mapYText = value;
        if (int.TryParse(mapYText, out int result))
        {
          mapY = result;
        }
        else
        {
          mapY = null;
        }
        Valid = Revalidate();
      }
    }

    bool Revalidate()
    {
      return mapX.HasValue && mapY.HasValue;
    }

    public bool Valid
    {
      get { return valid; }
      set
      {
        if (value == valid) return;
        valid = value;
        OnPropertyChanged();
      }
    }

    public bool TryNavigate(IViewportControl gr)
    {
      if (mapX.HasValue && mapY.HasValue)
      {
        gr.CenterPointInMapCoordinates = new MapCoordinate(mapX.Value, mapY.Value);
        return true;
      }
      return false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}