using Avalonia.Media;
using ReactiveUI;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class FormattingMetaData : ReactiveObject
    {
        string? title;

        int? border;
        int? margin;
        int? padding;

        Color? borderColor;
        Color? textColor;
        Color? backgroundColor;

        public string? Title
        {
            get { return title; }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }

        public int? Border
        {
            get { return border; }
            set
            {
                this.RaiseAndSetIfChanged(ref border, value);
            }
        }

        public int? Margin
        {
            get { return margin; }
            set
            {
                this.RaiseAndSetIfChanged(ref margin, value);
            }
        }

        public int? Padding
        {
            get { return padding; }
            set
            {
                this.RaiseAndSetIfChanged(ref padding, value);
            }
        }

        public Color? BorderColor
        {
            get { return borderColor; }
            set
            {
                if (value == borderColor)
                {
                    return;
                }

                this.RaiseAndSetIfChanged(ref borderColor, value);
            }
        }

        public Color? TextColor
        {
            get { return textColor; }
            set
            {
                this.RaiseAndSetIfChanged(ref textColor, value);
            }
        }

        public Color? BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                this.RaiseAndSetIfChanged(ref backgroundColor, value);
            }
        }

        public FormattingMetaData CreateCopy()
        {
            return new FormattingMetaData()
            {
                Title = this.Title,
                Border = this.Border,
                Margin = this.Margin,
                Padding = this.Padding,
                BorderColor = this.BorderColor,
                TextColor = this.TextColor,
                BackgroundColor = this.BackgroundColor
            };
        }
    }
}
