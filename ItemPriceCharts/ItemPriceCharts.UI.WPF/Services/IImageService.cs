namespace ItemPriceCharts.UI.WPF.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Tries to retrieve the absolute path to the profile image of the user.
        /// </summary>
        /// <param name="profileImagePath"></param>
        /// <returns></returns>
        bool TryGetProfileImagePath(out string profileImagePath);

        /// <summary>
        /// Tries to create the user profile image inside the "...\AppData\ItemPriceCharts\",
        /// or gets the initial absolute path to the image chosen by the user.
        /// </summary>
        /// <param name="profileImagePath"></param>
        /// <returns></returns>
        bool TryCreateUserProfileImage(out string profileImagePath);

    }
}