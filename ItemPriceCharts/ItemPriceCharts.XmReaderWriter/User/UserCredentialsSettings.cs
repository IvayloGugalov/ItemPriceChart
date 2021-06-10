using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

using ItemPriceCharts.XmReaderWriter.XmlActions;

namespace ItemPriceCharts.XmReaderWriter.User
{
    public static class UserCredentialsSettings
    {
        [DataMember(EmitDefaultValue = true)]
        public static string RememberAccount { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public static string LoginExpiresDate { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public static string Email { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public static string Username { get; set; }

        public static void ReadSettings()
        {
            using var reader = XmlReader.Create(XmlCreateFile.XML_FILE_PATH);
            try
            {
                reader.ReadXmlFile(new Dictionary<string, Action>()
                {
                    { nameof(RememberAccount), () =>  RememberAccount = reader.ReadElementContentAsString() },
                    { nameof(LoginExpiresDate),  () => LoginExpiresDate = reader.ReadElementContentAsString() },
                    { nameof(Email), () => Email = reader.ReadElementContentAsString() },
                    { nameof(Username), () => Username = reader.ReadElementContentAsString() }
                });
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }
        }

        public static (string username, string email) UsernameAndEmail => (Username, Email);

        public static bool ShouldEnableAutoLogin =>
            bool.TryParse(RememberAccount, out var shouldRememberAccount) &&
            shouldRememberAccount &&
            DateTime.TryParse(LoginExpiresDate, out var loginExpiresDate) &&
            DateTime.UtcNow <= loginExpiresDate;

        public static void WriteToXmlFile()
        {
            using var writer = XmlWriteData.CreateWriter(XmlCreateFile.XML_FILE_PATH);
            writer.WriteElementBody("UserAccount");

            writer.WriteTo(nameof(Username), Username);
            writer.WriteTo(nameof(Email), Email);
            writer.WriteTo(nameof(RememberAccount), RememberAccount);
            writer.WriteTo(nameof(LoginExpiresDate), LoginExpiresDate);
        }
    }
}
