using System.IO;

namespace Steropes.Tiles.TexturePack
{
    public readonly struct FilePath
    {
        readonly string path;

        public FilePath(string path)
        {
            this.path = path;
        }

        public static explicit operator string(FilePath p)
        {
            return p.path;
        }

        public override string ToString()
        {
            return path;
        }
    }

    /// <summary>
    ///  A crude abstraction layer to remove hard dependencies on file
    ///  IO or directory structures.
    /// </summary>
    public interface IFileSystemAdapter
    {
        bool TryRead(FilePath path, out Stream stream);
        Stream Read(FilePath path);
        FilePath CombinePath(FilePath contextBasePath, string file);
        FilePath GetBasePath(FilePath file);

        /// <summary>
        ///  Normalizes the incoming path. This should be done first to
        ///  map a virtual path into an physical location.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FilePath MakeAbsolute(string path);
    }

    public class DefaultFileSystemAdapter : IFileSystemAdapter
    {
        readonly string basePath;

        public DefaultFileSystemAdapter(string basePath = null)
        {
            this.basePath = basePath;
        }

        public bool TryRead(FilePath path, out Stream result)
        {
            try
            {
                result = File.OpenRead((string) path);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                result = default;
                return false;
            }
            catch (FileNotFoundException)
            {
                result = default;
                return false;
            }
        }

        public Stream Read(FilePath path)
        {
            if (TryRead(path, out var r))
            {
                return r;
            }

            throw new FileNotFoundException(null, (string) path);
        }

        public FilePath CombinePath(FilePath p1, string p2)
        {
            return new FilePath(Path.Combine((string) p1, p2));
        }

        public FilePath GetBasePath(FilePath file)
        {
            var directoryInfo = Directory.GetParent((string) file);
            return new FilePath(directoryInfo.FullName);
        }

        public FilePath MakeAbsolute(string path)
        {
            var combinedPath = Path.Combine(basePath, path);
            return new FilePath(Path.GetFullPath(combinedPath));
        }
    }
}