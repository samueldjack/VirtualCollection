using System;
using System.Windows;
using System.Windows.Controls;
using VirtualCollection.VirtualCollection;

namespace VirtualCollection
{
    public partial class BusynessIndicator : UserControl
    {
        public static readonly DependencyProperty BusyBodyProperty =
            DependencyProperty.Register("BusyBody", typeof(INotifyBusyness), typeof(BusynessIndicator), new PropertyMetadata(default(INotifyBusyness), HandleSourceChanged));

        private static void HandleSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifier = d as BusynessIndicator;

            if (e.OldValue != null)
            {
                (e.OldValue as INotifyBusyness).IsBusyChanged -= notifier.IsBusyChanged;
            }

            if (e.NewValue != null)
            {
                (e.NewValue as INotifyBusyness).IsBusyChanged += notifier.IsBusyChanged;
            }
        }

        private void IsBusyChanged(object sender, EventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                UpdateState();
            }
            else
            {
                Dispatcher.BeginInvoke(UpdateState);
            }
        }

        private void UpdateState()
        {
            if (BusyBody != null && BusyBody.IsBusy)
            {
                VisualStateManager.GoToState(this, "Busy", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Idle", true);
            }
        }

        public INotifyBusyness BusyBody
        {
            get { return (INotifyBusyness)GetValue(BusyBodyProperty); }
            set { SetValue(BusyBodyProperty, value); }
        }

        public BusynessIndicator()
        {
            InitializeComponent();

            VisualStateManager.GoToState(this, "Idle", true);
        }
    }
}
