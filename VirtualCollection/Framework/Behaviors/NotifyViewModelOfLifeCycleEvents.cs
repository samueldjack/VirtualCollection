using System.Windows;
using System.Windows.Interactivity;
using VirtualCollection.Framework.MVVM;

namespace VirtualCollection.Framework.Behaviors
{
    public class NotifyViewModelOfLifeCycleEvents : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += HandleLoaded;
            AssociatedObject.Unloaded += HandleUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= HandleLoaded;
            AssociatedObject.Unloaded -= HandleUnloaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.NotifyLoaded();
            }
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.NotifyUnloaded();
            }
        }

        private IViewModel ViewModel
        {
            get { return AssociatedObject.DataContext as IViewModel; }
        }
    }
}
