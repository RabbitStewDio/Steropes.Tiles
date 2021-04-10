using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace Steropes.Tiles.TemplateGen.ViewModels
{
    public class RecentFileViewModel : ReactiveObject
    {
        readonly MainWindowViewModel parentModel;
        readonly string filename;

        public RecentFileViewModel(MainWindowViewModel parentModel, string filename)
        {
            this.parentModel = parentModel;
            this.filename = filename;
            OpenFileCommand = ReactiveCommand.CreateFromTask(OnOpenFile);
        }

        
        public string FileName => filename;

        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

        async Task OnOpenFile()
        {
            await parentModel.OpenFile(filename);
        }
    }
}
