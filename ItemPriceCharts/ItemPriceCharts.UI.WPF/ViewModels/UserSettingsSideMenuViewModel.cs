using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class UserSettingsSideMenuViewModel : BaseViewModel
    {
        public UserAccount UserAccount { get; }


        public ICommand LogOutCommand { get; }

        public UserSettingsSideMenuViewModel(UserAccount userAccount)
        {
            this.UserAccount = userAccount;


            //this.ShowLogOutModalCommand = new RelayCommand(_ => UiEvents.RequestLogOut.Raise(null));
        }
    }
}
