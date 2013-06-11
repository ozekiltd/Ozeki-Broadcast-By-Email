using OzCommon.Utils.Schedule;

namespace OzDemoEmailSender.Model.Mock
{
    public class MockEmailWorkerFactory : IWorkerFactory<EmailEntry>
    {
        public IWorker CreateWorker(EmailEntry work)
        {
            return new EmailWorker(work);
        }
        
    }
}
