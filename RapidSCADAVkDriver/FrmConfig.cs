using Scada;
using Scada.Comm;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpVk {
    public partial class FrmConfig : Form {
        private readonly AppDirs appDirs;
        private readonly KpConfig config;
        private readonly int kpNumber;
        private string configFileName;

       public FrmConfig() {
            InitializeComponent();
        }

        public FrmConfig(AppDirs appDirs, int kpNumber): this() {
            config = new KpConfig();
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.kpNumber = kpNumber;
            configFileName = "";
        }

        private void ConfigToControls() {
            hostTextBox.Text = config.Host;
            channelIdTextBox.Text = config.ChannelId;
        }

        private void ControlsToConfig() {
            config.Host      = hostTextBox.Text;
            config.ChannelId = channelIdTextBox.Text;
        }

        private void FrmConfig_Load(object sender, EventArgs e) {
            configFileName = KpConfig.GetFileName(appDirs.ConfigDir, kpNumber);

            if (File.Exists(configFileName) && !config.Load(configFileName, out string errorMessage))
                ScadaUiUtils.ShowError(errorMessage);

            ConfigToControls();
        }

        private void button1_Click(object sender, EventArgs e) {
            ControlsToConfig();

            if (config.Save(configFileName, out string errorMessage))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errorMessage);
        }
    }
}
