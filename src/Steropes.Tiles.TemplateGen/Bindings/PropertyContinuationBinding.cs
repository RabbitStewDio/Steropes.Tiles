using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Steropes.Tiles.TemplateGen.Bindings
{
    public sealed class PropertyContinuationBinding<TSource, TTarget> : PropertyBindingBase<TTarget?>, IObserver<TSource?>
        where TSource : INotifyPropertyChanged
    {
        readonly Func<TSource, TTarget?> propertyGetter;
        readonly string propertyName;
        IDisposable? subscription;
        TSource? source;
        TTarget? propertyValue;
        bool firstEventFired;

        public PropertyContinuationBinding(IObservable<TSource?> sourceObservable, string propertyName, Func<TSource, TTarget?> propertyGetter)
        {
            this.propertyGetter = propertyGetter;
            this.propertyName = propertyName;
            this.source = default;
            
            this.subscription = sourceObservable.Subscribe(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.subscription?.Dispose();
            this.subscription = null;
        }

        public void OnCompleted()
        {
            foreach (var s in GetSubscriptions())
            {
                s.Value.FireComplete();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var s in GetSubscriptions())
            {
                s.Value.FireError(error);
            }
        }

        public void OnNext(TSource? value)
        {
            if (source != null)
            {
                source.PropertyChanged -= OnPropertyChanged;
            }

            source = value;

            if (source != null)
            {
                source.PropertyChanged += OnPropertyChanged;
            }

            UpdatePropertyValue();
        }

        void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != propertyName)
            {
                return;
            }

            UpdatePropertyValue();
        }

        void UpdatePropertyValue()
        {
            try
            {
                TTarget? newPropertyValue = source == null ? default : propertyGetter(source);
                if (firstEventFired && EqualityComparer<TTarget>.Default.Equals(this.propertyValue, newPropertyValue))
                {
                    return;
                }

                this.firstEventFired = true;
                this.propertyValue = newPropertyValue;
            }
            catch (Exception e)
            {
                foreach (var s in GetSubscriptions())
                {
                    s.Value.FireError(e);
                }

                return;
            }

            foreach (var s in GetSubscriptions())
            {
                s.Value.FireNext(propertyValue);
            }
        }
    }
}
