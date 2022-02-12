using System;
using Microsoft.Win32;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class SystemDialogWrapper : ISystemDialogWrapper
    {

        public string ChooseImageDialog()
        {
            var imagePath = string.Empty;

            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                FilterIndex = 2,
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = true,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                imagePath = openFileDialog.FileName;
            }

            return imagePath;
        }
    }

    /// <summary>
    /// Wrapper on the OpenFileDialog Class
    /// </summary>
    public interface ISystemDialogWrapper
    {
        /// <summary>
        /// Creates an OpenFileDialog with filter set on image files and opens the dialog.
        /// </summary>
        /// <returns>Path to the chosen image.</returns>
        string ChooseImageDialog();
    }
}
