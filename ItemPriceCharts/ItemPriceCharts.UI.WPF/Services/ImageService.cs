using System;
using System.Threading;

using NLog;

using ItemPriceCharts.InfraStructure.Constants;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class ImageService : IImageService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ImageService));

        private const string DESTINATION_FILE_NAME = "Profile Image";
        private const int IMAGE_NEW_WIDTH = 540;
        private const int IMAGE_NEW_HEIGHT = 720;

        /// <summary>
        /// Sets the compression level when saving the Profile image to the ItemPriceCharts folder
        /// </summary>
        private const int COMPRESSION_LEVEL = 60;

        private readonly ISystemDialogWrapper systemDialogWrapper;
        private readonly IFileSystemWrapper fileSystemWrapper;
        private readonly IImageProxy imageProxy;

        public ImageService(ISystemDialogWrapper systemDialogWrapper, IFileSystemWrapper fileSystemWrapper, IImageProxy imageProxy)
        {
            this.systemDialogWrapper = systemDialogWrapper;
            this.fileSystemWrapper = fileSystemWrapper;
            this.imageProxy = imageProxy;
        }

        public bool TryGetProfileImagePath(out string profileImagePath)
        {
            profileImagePath = UserCredentialsSettings.ProfileImagePath;

            return profileImagePath is not ("" or null) &&
                   this.fileSystemWrapper.FileExists(profileImagePath);
        }

        public bool TryCreateUserProfileImage(out string profileImagePath)
        {
            profileImagePath = string.Empty;
            var imageFilePath = this.systemDialogWrapper.ChooseImageDialog();

            if (imageFilePath != string.Empty &&
                // Save image to App path in AppData
                this.TryCopyFileToApplicationFolder(imageFilePath, out profileImagePath, destinationFileName: ImageService.DESTINATION_FILE_NAME))
            {
                UserCredentialsSettings.ProfileImagePath = profileImagePath;
                UserCredentialsSettings.WriteToXmlFile();
                return true;
            }

            if (imageFilePath != string.Empty)
            {
                UserCredentialsSettings.ProfileImagePath = imageFilePath;
                UserCredentialsSettings.WriteToXmlFile();

                Logger.Info("Pointing the profile image to original path.");
                return true;
            }

            Logger.Warn("Not creating a profile image.");
            return false;
        }

        private bool TryCopyFileToApplicationFolder(string originalFilePath, out string destinationFilePath, string destinationFileName = null)
        {
            var fileName = this.fileSystemWrapper.GetFileName(originalFilePath);
            if (destinationFileName != null)
            {
                // update the file name
                fileName = this.fileSystemWrapper.ChangeExtension(destinationFileName, "jpeg");
            }

            destinationFilePath = this.fileSystemWrapper.PathCombine(Paths.APPLICATION_APPDATA_PATH, fileName);

            var counter = 0;
            var success = false;

            var bitmap = this.imageProxy.ResizeImage(originalFilePath, newWidth: ImageService.IMAGE_NEW_WIDTH, newHeight: ImageService.IMAGE_NEW_HEIGHT);

            while (counter <= 2)
            {
                counter++;
                try
                {
                    // Compressing because it takes a lot of time to load the image in xaml when the file size is big.
                    this.imageProxy.SaveImageFile(destinationFilePath, bitmap, compressionLevel: ImageService.COMPRESSION_LEVEL);
                    success = true;

                    Logger.Info($"Copied the file {originalFilePath} to application directory.");
                    break;
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Could not copy the file {originalFilePath} to application directory.");
                    Thread.Sleep(TimeSpan.FromSeconds(1 + counter));
                }
                finally
                {
                    bitmap.Dispose();
                }
            }

            return success;
        }
    }
}
