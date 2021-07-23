using System.Windows;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginRegisterView.xaml
    /// </summary>
    public partial class LoginRegisterView : Window
    {
        public LoginRegisterView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Drag the window when mouse is inside the StackPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
