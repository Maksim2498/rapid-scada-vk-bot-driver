using Scada.Comm.Devices.KpVk;

namespace Scada.Comm.Devices {
    public class KpVkView : KPView {
        public KpVkView() : this(0) {}

        public KpVkView(int number): base(number) {
            CanShowProps = true; 
        }

        public override string KPDescr => Localization.UseRussian ? "Библиотека КП для взаимодействия с ВКонтакте."
                                                                  : "Device library for VK iteraction.";

        public override string Version =>  "1.0.0.0";

        public override void ShowProps() {
            new ConfigForm(AppDirs, Number).ShowDialog();
        }
    }
}
