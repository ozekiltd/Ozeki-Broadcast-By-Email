using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using OzCommon.Model;
using OzCommon.Model.Mock;
using OzCommon.Utils;
using OzCommon.Utils.Schedule;
using OzCommon.View;
using OzCommon.ViewModel;
using OzCommonBroadcasts.Model;
using OzCommonBroadcasts.Model.Csv;
using OzCommonBroadcasts.View;
using OzCommonBroadcasts.ViewModel;
using OzDemoEmailSender.Model;
using OzDemoEmailSender.Model.Config;
using OzDemoEmailSender.ViewModel;
using OzCommon.Utils.DialogService;

namespace OzDemoEmailSender
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly SingletonApp singletonApp;

        public App()
        {
            singletonApp = new SingletonApp("OPSEmailSender");
            InitDependencies();
        }

        void InitDependencies()
        {
            SimpleIoc.Default.Register<IBroadcastMainViewModel>(() => new EmailViewModel());
            SimpleIoc.Default.Register<ICsvImporter<EmailEntry>>(() => new CsvImporter<EmailEntry>());
            SimpleIoc.Default.Register<ICsvExporter<EmailEntry>>(() => new CsvExporter<EmailEntry>());
            SimpleIoc.Default.Register<IGenericSettingsRepository<EmailConfig>>(() => new GenericSettingsRepository<EmailConfig>());
            SimpleIoc.Default.Register<IWorkerFactory<EmailEntry>>(() => new EmailWorkerFactory());
            SimpleIoc.Default.Register<IExtensionContainer>(() => new ExtensionContainer());
            SimpleIoc.Default.Register<IScheduler<EmailEntry>>(() => new Scheduler<EmailEntry>(SimpleIoc.Default.GetInstance<IWorkerFactory<EmailEntry>>()));
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register(() => new ApplicationInformation("Email Sender"));
            SimpleIoc.Default.Register<IUserInfoSettingsRepository>(() => new UserInfoSettingsRepository());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Messenger.Default.Register<NotificationMessage>(this, MessageReceived);

            if (e.Args.Length != 0 && e.Args[0].ToLower() == "-mock")
                SimpleIoc.Default.Register<IClient>(() => new MockClient());
            else
               SimpleIoc.Default.Register<IClient>(() => new Client());

            singletonApp.OnStartup(e);
            var loginWindow = new LoginWindow();
            loginWindow.Show();


            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Messenger.Default.Unregister<NotificationMessage>(this, MessageReceived);
            base.OnExit(e);
        }

        private void MessageReceived(NotificationMessage notificationMessage)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (notificationMessage.Notification == Messages.NavigateToMainWindow)
                {
                    var mainWindow = new BroadcastMainWindow();
                    mainWindow.Show();

                    Current.MainWindow = mainWindow;
                }
            }));
        }
    }
}

