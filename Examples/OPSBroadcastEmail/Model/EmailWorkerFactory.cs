using GalaSoft.MvvmLight.Ioc;
using OPSSDK;
using OzCommon.Utils.Schedule;
using OzCommonBroadcasts.Model;

namespace OzDemoEmailSender.Model
{
    public class EmailWorkerFactory : IWorkerFactory<EmailEntry>
    {
        private IExtensionContainer extensionContainer;
        public EmailWorkerFactory()
        {
            extensionContainer = SimpleIoc.Default.GetInstance<IExtensionContainer>();
        }

        public IWorker CreateWorker(EmailEntry work)
        {
            return new EmailWorker(work, extensionContainer.GetExtension());
        }
    }
}
