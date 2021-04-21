using Serilog;
using Serilog.Core;
using System;

namespace Steropes.Tiles.TemplateGen
{
    public static class SLog
    {
        public static ILogger ForContext<TClass>()
        {
            return Log.ForContext(Constants.SourceContextPropertyName, NameWithoutGenerics(typeof(TClass)));
        }

        public static ILogger ForContext(Type t)
        {
            return Log.ForContext(Constants.SourceContextPropertyName, NameWithoutGenerics(t));
        }

        internal static string NameWithoutGenerics(Type t)
        {
            var name = t.FullName ?? t.Name;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

    }
}