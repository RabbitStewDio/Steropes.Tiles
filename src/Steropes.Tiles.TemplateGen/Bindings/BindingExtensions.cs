using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Bindings
{
    public static class BindingExtensions
    {
        class ExtractFirstPropertyAccessVisitor : ExpressionVisitor
        {
            string? PropertyName { get; set; }

            public string? ExtractPropertyName(Expression? expression)
            {
                PropertyName = null;
                Visit(expression);
                return PropertyName;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == null)
                {
                    return node;
                }

                if (node is MemberExpression me && PropertyName == null)
                {
                    PropertyName = me.Member.Name;
                }

                return base.Visit(node);
            }
        }

        public static IObservable<TTarget?> BindProperty<TSource, TTarget>(this TSource source, Expression<Func<TSource, TTarget?>> memberExpression, bool autoRun = true)
            where TSource : INotifyPropertyChanged
        {
            var v = new ExtractFirstPropertyAccessVisitor();
            var propertyName = v.ExtractPropertyName(memberExpression.Body);
            if (propertyName == null)
            {
                throw new ArgumentException("Cannot determine property name for binding.");
            }

            return BindProperty(source, propertyName, memberExpression.Compile(), autoRun);
        }

        public static IObservable<TTarget?> BindProperty<TSource, TTarget>(this TSource source, string propertyName, Func<TSource, TTarget?> propertyGetter, bool autoRun = true)
            where TSource : INotifyPropertyChanged
        {
            return new PropertyBinding<TSource, TTarget>(source, propertyName, propertyGetter, autoRun);
        }

        public static IObservable<TTarget?> BindProperty<TSource, TTarget>(this IObservable<TSource?> source, Expression<Func<TSource, TTarget>> memberExpression)
            where TSource : INotifyPropertyChanged
        {
            var v = new ExtractFirstPropertyAccessVisitor();
            var propertyName = v.ExtractPropertyName(memberExpression.Body);
            if (propertyName == null)
            {
                throw new ArgumentException("Cannot determine property name for binding.");
            }

            return BindProperty(source, propertyName, memberExpression.Compile());
        }

        public static IObservable<TTarget?> BindProperty<TSource, TTarget>(this IObservable<TSource?> source, string propertyName, Func<TSource, TTarget?> propertyGetter)
            where TSource : INotifyPropertyChanged
        {
            return new PropertyContinuationBinding<TSource, TTarget>(source, propertyName, propertyGetter);
        }

        public static IObservable<TTarget?> ConvertTo<TSource, TTarget>(this IObservable<TSource?> source)
        {
            return source.Select<TSource?, TTarget?>(x =>
            {
                if (x is TTarget t)
                {
                    return t;
                }

                return default;
            });
        }
        
        public static (TObj, bool) TryRaiseAndSetIfChanged<TObj, TRet>(
            this TObj reactiveObject,
            ref TRet backingField,
            TRet newValue,
            [CallerMemberName] string? propertyName = null)
            where TObj : IReactiveObject
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return (reactiveObject, false);
            }

            reactiveObject.RaisePropertyChanging(propertyName);
            backingField = newValue;
            reactiveObject.RaisePropertyChanged(propertyName);
            return (reactiveObject, true);
        }

        public static (TObj, bool) AndRaise<TObj>(this (TObj, bool) reactiveObject,
                                                  [CallerMemberName] string? propertyName = null)
            where TObj : IReactiveObject
        {
            if (reactiveObject.Item2)
            {
                reactiveObject.Item1.RaisePropertyChanged(propertyName);
            }

            return reactiveObject;
        }
    }
}
