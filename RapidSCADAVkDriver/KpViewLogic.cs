using Scada.Comm.Devices.KpVk;
using Scada.Data.Models;
using System;
using System.Threading;

namespace Scada.Comm.Devices {
    public class KpVkLogic : KPLogic {
        private KpVk.Config config;
        private bool     fatalError;
        private string   state;
        private bool     writeState;

        public KpVkLogic(int number = 0): base(number) {
            CanSendCmd   = true;
            ConnRequired = false;
            WorkState    = WorkStates.Normal;

            config = new KpVk.Config();
            fatalError   = false;
            state        = "";
            writeState   = false;
        }

        public override void Session() {
            if (writeState) {
                WriteToLog("");
                WriteToLog(state);
                writeState = false;
            }

            Thread.Sleep(ScadaUtils.ThreadDelay);
        }

        public override void SendCmd(Command cmd) {
            base.SendCmd(cmd);

            if (fatalError) {
                WriteToLog(state);
                return;
            }
            this.AddEvent(new KPEvent());

            WriteToLog("Got command!");

            CalcCmdStats();
        }

        public override void OnCommLineStart() {
            writeState = true;
            LoadConfig();
            SetCurData(0, 0, 1);
        }

        private void LoadConfig() {
            fatalError = !config.Load(KpVk.Config.GetFileName(AppDirs.ConfigDir, Number), out string errorMessage);

            if (fatalError) {
                state = "Отправка уведомлений невозможна";
                throw new Exception(errorMessage);
            }

            state = "Ожидание команд...";
        }
    }
}
