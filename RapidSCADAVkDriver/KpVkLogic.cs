using System;
using System.Net.Http;
using Scada.Data.Models;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices {
    public class KpVkLogic : KPLogic {
        private KpVk.Config config = new KpVk.Config();
        private HttpClient httpClient = new HttpClient();

        public KpVkLogic(int number = 0) : base(number) {
            CanSendCmd = true;
            ConnRequired = false;
            WorkState = WorkStates.Normal;
        }

        public override void Session() {
            Thread.Sleep(ScadaUtils.ThreadDelay);
        }

        public override void SendCmd(Command cmd) {
            base.SendCmd(cmd);

            if (WorkState == WorkStates.Error) {
                WriteToLog(Localization.UseRussian ? "Отправка уведомлений невозможна"
                                                   : "Cannot send notifications");

                return;
            }

            if (cmd.CmdNum != 1) {
                WriteToLog(CommPhrases.IllegalCommand);
                return;
            }

            WriteToLog(Localization.UseRussian ? "Отпарвка уведомления..."
                                               : "Sending notification...");

            try {
                var task = httpClient.PostAsync(
                    config.Host,
                    new StringContent(
                        $"{{\"channelId\":\"{config.ChannelId}\",\"message\":\"{cmd.GetCmdDataStr()}\"}}",
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                task.Wait();

                WriteToLog(Localization.UseRussian ? $"Отправлено: {task.Result.StatusCode}"
                                                   : $"Sent: {task.Result.StatusCode}");
            } catch (Exception exception) {
                WriteToLog(Localization.UseRussian ? $"Ошибка: {exception.Message}"
                                                   : $"Error: {exception.Message}");
            }

            CalcCmdStats();
        }

        public override void OnCommLineStart() {
            LoadConfig();
            SetCurData(0, 0, 1);
        }

        private void LoadConfig() {
            bool fatalError = !config.Load(KpVk.Config.GetFileName(AppDirs.ConfigDir, Number), out string errorMessage);

            if (fatalError) {
                WorkState = WorkStates.Error;

                WriteToLog(Localization.UseRussian ? "Не удалось прочитать файл конфигурации"
                                                   : "Failed to read configuration file");

                throw new Exception(errorMessage);
            }

            WriteToLog(Localization.UseRussian ? "Ожидание команд..."
                                               : "Waiting for commands...");
        }
    }
}
