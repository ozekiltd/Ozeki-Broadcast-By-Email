using GalaSoft.MvvmLight.Ioc;
using OzCommon.Model;
using OzDemoEmailSender.Model;
using OzCommonBroadcasts.ViewModel;
using OzDemoEmailSender.Model.Config;

namespace OzDemoEmailSender.ViewModel
{
    public class EmailViewModel : BroadcastMainViewModel<EmailEntry>
    {
        private readonly IGenericSettingsRepository<EmailConfig> settingsRepository;

        public EmailViewModel()
        {
            settingsRepository = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<EmailConfig>>();
        }

        protected override object GetSettingsViewModel()
        {
            return new SettingsViewModel();
        }

        protected override int GetMaxConcurrentWorkers()
        {
            var config = settingsRepository.GetSettings();
            return config != null ? config.ConcurrentWorks : 1; //Default value is 1
        }

        protected override string GetApiExtensionID()
        {
            var settings = settingsRepository.GetSettings();
            return settings == null ? "" : settings.ExtensionId;
        }
    }
}
