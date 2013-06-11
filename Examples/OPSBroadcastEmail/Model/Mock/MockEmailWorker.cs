using System;
using System.Threading;
using System.Threading.Tasks;
using OzCommon.Utils.Schedule;

namespace OzDemoEmailSender.Model.Mock
{
    class EmailWorker : IWorker
    {
        private readonly Random random;
        private readonly EmailEntry classImpl;

        public event EventHandler<WorkResult> WorkCompleted;

        public EmailWorker()
        {
            random = new Random();
        }

        public EmailWorker(EmailEntry impl)
            : this()
        {
            classImpl = impl;
        }

        public void StartWork()
        {
            Task.Factory.StartNew(() =>
            {
                if (classImpl.State == WorkState.Success)
                {
                    OnWorkCompleted(classImpl.State);
                    classImpl.IsCompleted = true;
                    return;
                }

                classImpl.State = WorkState.InProgress;

                Thread.Sleep(random.Next(0, 2000));

                if (random.Next(0, 2) == 0)
                {
                    classImpl.State = WorkState.Success;
                    OnWorkCompleted(classImpl.State);
                    classImpl.IsCompleted = true;
                }
                else
                {
                    classImpl.State = WorkState.DeliveringFailed;
                    OnWorkCompleted(classImpl.State);
                }
            });
        }

        public void CancelWork()
        {
            
        }

        private void OnWorkCompleted(WorkState e)
        {
            var workResult = e == WorkState.Success;

            var handler = WorkCompleted;
            if (handler != null) handler(this, new WorkResult() { IsSuccess = workResult });
        }
    }
}
