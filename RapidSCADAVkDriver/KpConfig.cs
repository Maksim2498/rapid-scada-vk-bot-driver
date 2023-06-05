using Scada.Data.Entities;
using System;
using System.Xml;

namespace Scada.Comm.Devices.KpVk {
    internal class KpConfig {
        public static string GetConfigName(string configDir, int kpNumber) {
            return configDir + "KpVk_" + CommUtils.AddZeros(kpNumber, 3) + ".xml";
        }

        public string Host      { get; set; }
        public string ChannelId { get; set; }

        public KpConfig() {
            SetToDefault();
        }

        public void SetToDefault() {
            Host      = "https://vk.bot.rapidscada.fominmv.ru";
            ChannelId = "";
        }

        public bool Load(string fileName, out string errorMessage) {
            SetToDefault();

            try {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                XmlElement rootElem = xmlDoc.DocumentElement;

                Host      = rootElem.GetChildAsString("Host"     );
                ChannelId = rootElem.GetChildAsString("ChannelId");

                errorMessage = "";

                return true;
            } catch (Exception exception) {
                errorMessage = CommPhrases.LoadKpSettingsError + ":" + Environment.NewLine + exception.Message;
                return false;
            }
        }

        public bool Save(string fileName, out string errorMessage) {
            try {
                XmlDocument xmlDoc     = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("KpVkConfig");

                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("Host",      Host     );
                rootElem.AppendElem("ChannelId", ChannelId);

                xmlDoc.Save(fileName);

                errorMessage = "";

                return true;
            } catch (Exception exception) {
                errorMessage = CommPhrases.SaveKpSettingsError + ":" + Environment.NewLine + exception.Message;
                return false;
            }
        }
    }
}
