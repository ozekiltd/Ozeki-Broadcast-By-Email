using System;
using OzCommon.Model;
using System.ComponentModel;

namespace OzDemoEmailSender.Model.Config
{
    public class EmailConfig : BaseModel, ICloneable, IDataErrorInfo
    {
        #region Properties
        int concurrentWorks;
        string extensionId;
        string senderEmailAddress;

        #region Properties => OnProertyChanged
        public Int32 ConcurrentWorks
        {
            get { return concurrentWorks; }
            set { concurrentWorks = value; Raise(() => ConcurrentWorks); }
        }

        public string ExtensionId
        {
            get { return extensionId; }
            set { extensionId = value; Raise(() => ExtensionId); }
        }

        public string SenderEmailAddress
        {
            get { return senderEmailAddress; }
            set { senderEmailAddress = value; Raise(() => SenderEmailAddress); }
        }
        #endregion

        #endregion

        public EmailConfig()
        {
            ConcurrentWorks = 1;
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "ConcurrentWorks":
                        if (!(ConcurrentWorks > 0))
                            return "Concurrent works must be greater than 0";
                        break;
                    case "ExtensionId":
                        if (string.IsNullOrWhiteSpace(ExtensionId))
                            return "Extension ID cannot be empty";
                        break;
                }

                return null;
            }
        }

        public string Error { get; set; }

        public bool IsValid
        {
            get
            {
                return (ConcurrentWorks > 0 && !string.IsNullOrWhiteSpace(ExtensionId));
            }
        }

        public object Clone()
        {
            return new EmailConfig { 
                                     ConcurrentWorks = this.ConcurrentWorks,
                                     ExtensionId   = this.ExtensionId,
                                     SenderEmailAddress = this.SenderEmailAddress
                                   };
        }
    }
}
