using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VirtualCollection.Framework.Extensions
{
    public static class INotifyPropertyChangedExtensions
    {
        public static IObservable<Unit> ObservePropertyChanged<T>(this INotifyPropertyChanged source, Expression<Func<T>> propertyExpression)
        {
            return ObservePropertyChanged(source, propertyExpression.GetPropertyName());
        }

        public static IObservable<Unit> ObservePropertyChanged(this INotifyPropertyChanged source, string property)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => (sender, e) => h(e),
                h => source.PropertyChanged += h,
                h => source.PropertyChanged -= h)
                .Where(e => e.PropertyName == property)
                .Select(_ => Unit.Default);
        } 
    }
}
