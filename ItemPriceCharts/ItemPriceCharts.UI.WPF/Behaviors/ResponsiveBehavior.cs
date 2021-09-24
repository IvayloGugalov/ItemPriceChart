using System.Windows;

namespace ItemPriceCharts.UI.WPF.Behaviors
{
    public class ResponsiveBehavior : DependencyObject
    {
        #region IsResponsive

        public static readonly DependencyProperty IsResponsiveProperty =
            DependencyProperty.RegisterAttached(
                "IsResponsive",
                typeof(bool),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(false, OnIsResponsiveChanged));

        public static bool GetIsResponsive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsResponsiveProperty);
        }

        public static void SetIsResponsive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsResponsiveProperty, value);
        }

        #endregion

        #region HorizontalBreakpoint

        public static readonly DependencyProperty HorizontalBreakpointProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalBreakpoint",
                typeof(double),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(double.MaxValue));

        public static double GetHorizontalBreakpoint(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalBreakpointProperty);
        }

        public static void SetHorizontalBreakpoint(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalBreakpointProperty, value);
        }

        #endregion

        #region HorizontalBreakpointSetters

        public static readonly DependencyProperty HorizontalBreakpointSettersProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalBreakpointSetters",
                typeof(SetterBaseCollection),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(new SetterBaseCollection()));

        public static SetterBaseCollection GetHorizontalBreakpointSetters(DependencyObject obj)
        {
            return (SetterBaseCollection)obj.GetValue(HorizontalBreakpointSettersProperty);
        }

        public static void SetHorizontalBreakpointSetters(DependencyObject obj, SetterBaseCollection value)
        {
            obj.SetValue(HorizontalBreakpointSettersProperty, value);
        }

        #endregion

        #region IsHorizontalBreakpointSettersActive

        public static readonly DependencyProperty IsHorizontalBreakpointSettersActiveProperty =
            DependencyProperty.RegisterAttached(
                "IsHorizontalBreakpointSettersActive",
                typeof(bool),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(false));

        public static bool GetIsHorizontalBreakpointSettersActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHorizontalBreakpointSettersActiveProperty);
        }

        public static void SetIsHorizontalBreakpointSettersActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHorizontalBreakpointSettersActiveProperty, value);
        }

        #endregion

        private static void OnIsResponsiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element) return;

            var currentWindow = Application.Current.MainWindow;

            if (GetIsResponsive(element))
            {
                if (currentWindow != null)
                    currentWindow.SizeChanged += (s, e) => UpdateElement(currentWindow.Width, element);
            }
            else
            {
                if (currentWindow != null)
                    currentWindow.SizeChanged -= (s, e) => UpdateElement(currentWindow.Width, element);
            }
        }

        private static void UpdateElement(double currentWindowWidth, FrameworkElement element)
        {
            var breakpointWidth = GetHorizontalBreakpoint(element);

            if (currentWindowWidth >= breakpointWidth &&
                !GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, true);

                element.Style = CreateResponsiveStyle(element);
            }
            else if (currentWindowWidth < breakpointWidth &&
                     GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, false);

                element.Style = element.Style.BasedOn;
            }
        }

        private static Style CreateResponsiveStyle(FrameworkElement element)
        {
            var responsiveStyle = new Style(element.GetType(), element.Style);

            foreach (var setter in GetHorizontalBreakpointSetters(element))
            {
                responsiveStyle.Setters.Add(setter);
            }

            return responsiveStyle;
        }
    }
}
