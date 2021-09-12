using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Services;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class SideMenuViewModelTest
    {
        private readonly TestableDispatcherWrapper dispatcherWrapper = new();

        private Mock<ILogOutService> logOutServiceMock;
        private UiEvents uiEvents;

        [SetUp]
        public void SetUp()
        {
            Bootstrapper.Bootstrapper.Start(this.dispatcherWrapper);

            this.uiEvents = new UiEvents(this.dispatcherWrapper);
            this.logOutServiceMock = new Mock<ILogOutService>(MockBehavior.Strict);
        }
    }
}
