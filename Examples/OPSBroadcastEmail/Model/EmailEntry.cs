using System;
using System.ComponentModel;
using OzCommon.Model;
using OzCommonBroadcasts.Model;

namespace OzDemoEmailSender.Model
{
    public class EmailEntry : BaseModel, ICompletedWork, IDataErrorInfo
    {
        #region Properties

        private string recipientAddress;
        private WorkState state;
        private bool isCompleted;
        private string emailContent;
        string emailSubject;

        #region Properties with OnProertyChanged

        public string RecipientAddress
        {
            get { return recipientAddress; }
            set { recipientAddress = value; Raise(() => RecipientAddress); }
        }

        public string EmailSubject
        {
            get { return emailSubject; }
            set { emailSubject = value; Raise(() => EmailSubject); }
        }

        public String EmailContent
        {
            get { return emailContent; }
            set { emailContent = value; Raise(() => EmailContent); }
        }

        [ReadOnlyProperty]
        [ExportIgnoreProperty]
        public WorkState State
        {
            get { return state; }
            set { state = value; Raise(() => State); }
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; Raise(() => IsCompleted); }
        }

        #endregion

        #endregion

        public EmailEntry()
        {
            state = WorkState.Init;
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public string Error { get; private set;}

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "RecipientAddress":
                        if (string.IsNullOrWhiteSpace(RecipientAddress))
                            return "RecipientAddress cannot be empty";
                        break;
                }
                return null;
            }
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(RecipientAddress));
            }
        }

    }
}
