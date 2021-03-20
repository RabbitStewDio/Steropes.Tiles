using System.Collections.Generic;

namespace Steropes.Tiles.TexturePack
{
    public interface ITextureFile<TTile>
    {        
       IEnumerable<TTile> ProduceTiles();
    }
}