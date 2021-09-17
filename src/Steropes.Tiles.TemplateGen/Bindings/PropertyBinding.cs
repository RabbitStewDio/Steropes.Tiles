using Serilog;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;

namespace Steropes.Tiles.TemplateGen.Bindings
{
    public sealed class PropertyBinding<TSource, TTarget> : PropertyBindingBase<TTarget>
        where TSource : INotifyPropertyChanged
    {
        static readonly ILogger Logger = SLog.ForContext<PropertyBinding<TSource, TTarget>>();
        
        readonly TSource source;
        readonly Func<TSource, TTarget?> propertyGetter;
        readonly string propertyName;

        [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
        public PropertyBinding([NotNull] TSource source, 
                               [NotNull] string propertyName, 
                               [NotNull] Func<TSource, TTarget?> propertyGetter, 
                               bool autoRun = true)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.propertyGetter = propertyGetter ?? throw new ArgumentNullException(nameof(propertyGetter));
            this.source.PropertyChanged += OnPropertyChanged;

            if (autoRun)
            {
                CurrentThreadScheduler.Instance.Schedule(UpdateValue);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            source.PropertyChanged -= OnPropertyChanged;
        }

        void OnPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null || e.PropertyName != propertyName)
            {
                return;
            }

            UpdateValue();
        }

        void UpdateValue()
        {
            TTarget? value;
            try
            {
                value = propertyGetter.Invoke(source);
            }
            catch (Exception ex)
            {
                Logger.Verbose(ex, "Error while invoking getter for source {Source} for property {PropertyName}", source, propertyName);
                foreach (var (_, v) in GetSubscriptions())
                {
                    try
                    {
                        v.FireError(ex);
                    }
                    catch(Exception e)
                    {
                        // ignore
                        Logger.Verbose(e, "Failed to propagate error state {Exception} for property {PropertyName}", ex, propertyName);
                        
                    }
                }

                return;
            }

            foreach (var (_, v) in GetSubscriptions())
            {
                try
                {
                    // Avalonia is not yet fully prepared for null-annotations
                    v.FireNext(value!);
                }
                catch(Exception ex)
                {
                    // ignore
                    Logger.Verbose(ex, "Error while propagating next value {Source} for property {PropertyName}", source, propertyName);
                }
            }
        }
    }
}
