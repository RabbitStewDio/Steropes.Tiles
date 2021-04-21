using Avalonia.Media;
using JetBrains.Annotations;
using ReactiveUI;
using Steropes.Tiles.TemplateGen.Bindings;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class CellMappingDeclaration : ReactiveObject
    {
        string? key;
        string? name;
        string? comment;
        Color? highlightColor;

        [CanBeNull] public string? Key
        {
            get => key;
            set => this.TryRaiseAndSetIfChanged(ref key, value);
        }

        [CanBeNull] public string? Name
        {
            get => name;
            set => this.TryRaiseAndSetIfChanged(ref name, value);
        }

        [CanBeNull] public string? Comment
        {
            get => comment;
            set => this.TryRaiseAndSetIfChanged(ref comment, value);
        }

        public Color? HighlightColor
        {
            get => highlightColor;
            set => this.TryRaiseAndSetIfChanged(ref highlightColor, value);
        }

        public CellMappingDeclaration CreateCopy()
        {
            return new CellMappingDeclaration()
            {
                Key = Key,
                Name = Name,
                Comment = Comment,
                HighlightColor = HighlightColor
            };
        }
    }
}
