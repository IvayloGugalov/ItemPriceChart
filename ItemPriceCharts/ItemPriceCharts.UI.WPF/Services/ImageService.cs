using System;
using System.Threading;

using NLog;

using ItemPriceCharts.InfraStructure.Constants;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class ImageService : IImageService
    {
        private const string DESTINATION_FILE_NAME = "Profile Image";

        private static readonly Logger Logger = LogManager.GetLogger(nameof(ImageService));

        private readonly ISystemDialogWrapper systemDialogWrapper;
        private readonly IFileSystemWrapper fileSystemWrapper;

        public ImageService(ISystemDialogWrapper systemDialogWrapper, IFileSystemWrapper fileSystemWrapper)
        {
            this.systemDialogWrapper = systemDialogWrapper;
            this.fileSystemWrapper = fileSystemWrapper;
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
                var extension = this.fileSystemWrapper.GetExtension(originalFilePath);
                // update the file name
                fileName = this.fileSystemWrapper.ChangeExtension(destinationFileName, extension);
            }

            destinationFilePath = this.fileSystemWrapper.PathCombine(Paths.APPLICATION_APPDATA_PATH, fileName);

            var counter = 0;
            var success = false;

            while (counter <= 2)
            {
                counter++;
                try
                {
                    this.fileSystemWrapper.FileCopy(originalFilePath, destinationFilePath);
                    success = true;

                    Logger.Info($"Copied the file {originalFilePath} to application directory.");
                    break;
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Could not copy the file {originalFilePath} to application directory.");
                    Thread.Sleep(TimeSpan.FromSeconds(1 + counter));
                }
            }

            return success;
        }
    }
}
