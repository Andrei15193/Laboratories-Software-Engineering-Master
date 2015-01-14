using BillPath.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Windows.Storage;

namespace BillPath.DataAccess.Xml
{
    public class XmlAppDataProvider
        : IAppDataProvider
    {
        public XmlAppDataProvider()
        {
            FileName = "appData.xml";
        }

        public string FileName
        {
            get;
            set;
        }

        public AppData AppData
        {
            get;
            set;
        }

        public void LoadAppData()
        {
            try
            {
                using (StringReader stringReader = new StringReader(FileIO.ReadTextAsync(ApplicationData.Current.LocalFolder.GetFileAsync(FileName).AsTask().Result).AsTask().Result))
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                    AppData = (AppData)_serializer.ReadObject(xmlReader);

                foreach (Account account in AppData.Accounts)
                    foreach (Income income in account.Incomes)
                        income.Account = account;
                foreach (Category category in AppData.Categories)
                    foreach (Expense expense in category.Expenses)
                    {
                        expense.Category = category;
                        expense.Account = AppData.Accounts.First(account => account.CurrencyName.Equals(expense.AccountName));
                    }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                AppData = new AppData();
            }
        }

        public void Save()
        {
            if (AppData != null)
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();

#if DEBUG
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder,
                                                                  new XmlWriterSettings
                                                                  {
                                                                      Async = true,
                                                                      Indent = true,
                                                                      IndentChars = "    ",
                                                                      NewLineChars = Environment.NewLine,
                                                                      NewLineHandling = NewLineHandling.Entitize,
                                                                      NewLineOnAttributes = true
                                                                  }))
#else
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
#endif
                        _serializer.WriteObject(xmlWriter, AppData);

                    Debug.WriteLine("Created file in " + ApplicationData.Current.LocalFolder.Path);

                    FileIO.WriteTextAsync(ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting).AsTask().Result,
                                          stringBuilder.ToString(),
                                          Windows.Storage.Streams.UnicodeEncoding.Utf8).AsTask().Wait();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
        }

        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(AppData));
    }
}