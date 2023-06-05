using System;
using System.Xml;

namespace Scada.Comm.Devices.KpVk {
    internal class Config {
        public static string GetFileName(string configDir, int kpNumber) {
            return configDir + "KpVk_" + CommUtils.AddZeros(kpNumber, 3) + ".xml";
        }

        public string Host { get; set; }
        public string ChannelId { get; set; }

        public Config() {
            SetToDefault();
        }

        public void SetToDefault() {
            Host = "https://vk.bot.rapidscada.fominmv.ru";
            ChannelId = "";
        }

        public bool Load(string fileName, out string errorMessage) {
            SetToDefault();

            try {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(fileName);

                XmlElement rootElement = xmlDocument.DocumentElement;

                Host = rootElement.GetChildAsString("Host");
                ChannelId = rootElement.GetChildAsString("ChannelId");

                errorMessage = "";

                return true;
            } catch (Exception exception) {
                errorMessage = CommPhrases.LoadKpSettingsError + ":" + Environment.NewLine + exception.Message;
                return false;
            }
        }

        public bool Save(string fileName, out string errorMessage) {
            try {
                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

                xmlDocument.AppendChild(xmlDeclaration);

                XmlElement rootElement = xmlDocument.CreateElement("KpVkConfig");

                xmlDocument.AppendChild(rootElement);

                rootElement.AppendElem("Host", Host);
                rootElement.AppendElem("ChannelId", ChannelId);

                xmlDocument.Save(fileName);

                errorMessage = "";

                return true;
            } catch (Exception exception) {
                errorMessage = CommPhrases.SaveKpSettingsError + ":" + Environment.NewLine + exception.Message;
                return false;
            }
        }
    }
}
