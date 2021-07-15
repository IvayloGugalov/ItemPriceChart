using System.Windows;

using Microsoft.Xaml.Behaviors;

namespace ItemPriceCharts.UI.WPF.Converters
{
    public class CloseWindowBehavior : Behavior<Window>
    {
        public bool CloseTrigger
        {
            get => (bool)GetValue(CloseTriggerProperty);
            set => SetValue(CloseTriggerProperty, value);
        }

        public static readonly DependencyProperty CloseTriggerProperty =
            DependencyProperty.Register(
                nameof(CloseTrigger),
                typeof(bool),
                typeof(CloseWindowBehavior),
                new PropertyMetadata(false, OnCloseTriggerChanged));

        private static void OnCloseTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CloseWindowBehavior behavior)
            {
                behavior.OnCloseTriggerChanged();
            }
        }

        private void OnCloseTriggerChanged()
        {
            // when closetrigger is true, close the window
            if (this.CloseTrigger)
            {
                this.AssociatedObject.Close();
            }
        }
    }
}
