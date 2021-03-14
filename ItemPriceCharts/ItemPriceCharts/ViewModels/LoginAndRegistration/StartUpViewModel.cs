using ItemPriceCharts.UI.WPF.CommandHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class StartUpViewModel : BindableViewModel
    {
        public bool StartUpViewShown { get; private set; } = true;
        public bool ShowRegisterView { get; private set; }
        public bool ShowLoginView { get; private set; }


        public ICommand ShowRegisterViewCommand { get; }
        public ICommand ShowLoginViewCommand { get; }

        public StartUpViewModel()
        {
            this.ShowRegisterViewCommand = new RelayCommand(this.ShowRegisterViewAction);
            this.ShowLoginViewCommand = new RelayCommand(this.ShowLoginViewAction);
        }

        private void ShowRegisterViewAction(object param)
        {
            this.ShowRegisterView = true;
            this.OnPropertyChanged(nameof(this.ShowRegisterView));

            this.StartUpViewShown = false;
            this.OnPropertyChanged(nameof(this.StartUpViewShown));
        }

        private void ShowLoginViewAction(object param)
        {
            this.ShowLoginView = true;
            this.OnPropertyChanged(nameof(this.ShowLoginView));

            this.StartUpViewShown = false;
            this.OnPropertyChanged(nameof(this.StartUpViewShown));
        }

    }
}
