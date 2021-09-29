using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        public UserAccount UserAccount { get; }


        public UserSettingsViewModel(UserAccount userAccount)
        {
            this.UserAccount = userAccount;
        }

    }
}
