using System.Diagnostics;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class ProcessWrapper : IProcessWrapper
    {
        public void StartProcess(string fileName) => Process.Start(fileName);
        public void StartProcess(string fileName, string arguments) => Process.Start(fileName, arguments);
    }

    public interface IProcessWrapper
    {
        void StartProcess(string fileName);
        void StartProcess(string fileName, string arguments);
    }
}
