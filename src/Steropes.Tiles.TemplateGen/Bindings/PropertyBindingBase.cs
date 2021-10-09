using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Steropes.Tiles.TemplateGen.Bindings
{
    public abstract class PropertyBindingBase<TTarget> : IObservable<TTarget>, IDisposable
    {
        readonly ConcurrentDictionary<int, Subscription> subscriptions;
        int subscriptionCounter;

        protected PropertyBindingBase()
        {
            subscriptions = new ConcurrentDictionary<int, Subscription>();
        }

        ~PropertyBindingBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var (_, v) in subscriptions.ToArray())
                {
                    try
                    {
                        v.FireComplete();
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            subscriptions.Clear();
        }

        protected KeyValuePair<int, Subscription>[] GetSubscriptions() => subscriptions.ToArray();
        
        public IDisposable Subscribe(IObserver<TTarget> observer)
        {
            var x = Interlocked.Increment(ref subscriptionCounter);
            var s = new Subscription(x, this, observer);
            if (!subscriptions.TryAdd(x, s))
            {
                throw new InvalidOperationException();
            }
            return s;
        }

        protected class Subscription: IDisposable
        {
            readonly int id;
            readonly PropertyBindingBase<TTarget> source;
            readonly IObserver<TTarget> observer;

            public Subscription(int id, PropertyBindingBase<TTarget> source, IObserver<TTarget> observer)
            {
                this.id = id;
                this.source = source;
                this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
            }

            public void FireComplete()
            {
                observer.OnCompleted();
            }
            
            public void FireError(Exception e)
            {
                observer.OnError(e);
            }
            
            public void FireNext(TTarget e)
            {
                observer.OnNext(e);
            }

            public void Dispose()
            {
                source.subscriptions.TryRemove(id, out _);
            }
        }
    }
}
