using System;
using GalaSoft.MvvmLight.Ioc;
using OPSSDK;
using OzCommon.Model;
using OzCommon.Utils.Schedule;
using OPSSDKCommon.Model.Message;
using OzDemoEmailSender.Model.Config;

namespace OzDemoEmailSender.Model
{
    class EmailWorker : IWorker
    {
        readonly EmailEntry emailEntry;
        readonly IAPIExtension apiExtension;
        string messageId;
        readonly IGenericSettingsRepository<EmailConfig> settingsRepository;

        public EmailWorker(EmailEntry emailEntry, IAPIExtension extension)
        {
            this.emailEntry = emailEntry;
            apiExtension = extension;
            settingsRepository = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<EmailConfig>>();
            apiExtension.MessageSubmitted += apiExtension_MessageSubmitted;
        }

        void apiExtension_MessageSubmitted(object sender, MessageResultEventArgs e)
        {
            if (messageId == e.CallbackId)
            {
                OnWorkCompleted(e.Result ? WorkState.Success : WorkState.DeliveringFailed);

                apiExtension.MessageSubmitted -= apiExtension_MessageSubmitted;
            }
        }

        public event EventHandler<WorkResult> WorkCompleted;

        public void StartWork()
        {
            if (emailEntry.State == WorkState.Success)
            {
                OnWorkCompleted(WorkState.Success);
                return;
            }

            if (!emailEntry.IsValid)
            {
                OnWorkCompleted(emailEntry.State);
                return;
            }

            emailEntry.State = WorkState.InProgress;

            var message = new EmailMessage(emailEntry.RecipientAddress, emailEntry.EmailSubject, emailEntry.EmailContent, settingsRepository.GetSettings().SenderEmailAddress);
            messageId = message.MessageId;

            apiExtension.SendMessageAsync(message, result =>
                {
                    switch (result.RoutingState)
                    {
                        case RoutingState.DestinationAccepted:
                            if (emailEntry.State == WorkState.InProgress)
                                emailEntry.State = WorkState.Routed;
                            break;
                        case RoutingState.DestinationNotFound:
                            OnWorkCompleted(WorkState.DeliveringFailed);
                            apiExtension.MessageSubmitted -= apiExtension_MessageSubmitted;
                            break;
                    }
                });
        }

        public void CancelWork()
        {

        }

        private void OnWorkCompleted(WorkState e)
        {
            emailEntry.State = e;

            if (e == WorkState.Success)
                emailEntry.IsCompleted = true;

            var handler = WorkCompleted;
            if (handler != null)
                handler(this, new WorkResult() { IsSuccess = emailEntry.IsCompleted });
        }
    }
}
