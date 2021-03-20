using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public class Command : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool enabled;

        public Command()
        {
            enabled = true;
        }

        public virtual void OnActionPerformed(object source, EventArgs args)
        { }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value == enabled) return;
                enabled = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Install(Button menuItem)
        {
            void UpdateState(object sender, PropertyChangedEventArgs e)
            {
                menuItem.Enabled = Enabled;
            }

            void OnDispose(object sender, EventArgs args)
            {
                // click handler is taken care of by emptying out the event handlers (this is done in the form component).
                menuItem.Disposed -= OnDispose;
                PropertyChanged -= UpdateState;
            }

            PropertyChanged += UpdateState;
            menuItem.Click += OnActionPerformed;
            menuItem.Disposed += OnDispose;

            menuItem.Enabled = Enabled;
        }

        public void Install(ToolStripItem menuItem)
        {
            void UpdateState(object sender, PropertyChangedEventArgs e)
            {
                menuItem.Enabled = Enabled;
            }

            void OnDispose(object sender, EventArgs args)
            {
                // click handler is taken care of by emptying out the event handlers (this is done in the form component).
                menuItem.Disposed -= OnDispose;
                PropertyChanged -= UpdateState;
            }

            PropertyChanged += UpdateState;
            menuItem.Click += OnActionPerformed;
            menuItem.Disposed += OnDispose;

            menuItem.Enabled = Enabled;
        }
    }
}
