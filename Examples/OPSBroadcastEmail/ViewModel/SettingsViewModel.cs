using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using OzCommon.Model;
using OzCommon.ViewModel;
using OzDemoEmailSender.Model.Config;

namespace OzDemoEmailSender.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public RelayCommand CancelCommand { get; protected set; }
        public RelayCommand SaveCommand { get; protected set; }
        readonly IGenericSettingsRepository<EmailConfig> settingsRepository;

        public EmailConfig EmailConfig { get; set; }

        public SettingsViewModel()
        {
            InitCommands();
            settingsRepository = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<EmailConfig>>();
            SetSettings();
        }

        private void SetSettings()
        {
            EmailConfig = new EmailConfig();

            var config = settingsRepository.GetSettings();
            if (config != null)
                EmailConfig = config.Clone() as EmailConfig;

        }

        public void InitCommands()
        {
            CancelCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(Messages.DismissSettingsWindow)));
            SaveCommand = new RelayCommand(() =>
                                               {
                                                   settingsRepository.SetSettings(EmailConfig);
                                                   Messenger.Default.Send(new NotificationMessage(Messages.DismissSettingsWindow));
                                               }, () => EmailConfig.IsValid);
        }
    }
}
