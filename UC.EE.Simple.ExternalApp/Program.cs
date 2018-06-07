using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using Uniconta.DataModel;

namespace UC.EE.Simple.ExternalApp
{
    class Program
    {
        static string username, password;
        static void Main(string[] args)
        {
            AskCredentials(out username, out password);

            #region Create session and log in
            // Create connection
            var connection = new UnicontaConnection(APITarget.Live);
            // Create session
            var session = new Session(connection);
            // Login user. The guid required here is the partner API key which you will get when you ask for it as a partner.
            var apiKey = new Guid("00000000-0000-0000-0000-000000000000");
            if (apiKey.Equals(Guid.Empty))
            {
                Console.WriteLine("You need to set the API key");
                return;
            }

            var logged = session.LoginAsync(username, password, Uniconta.Common.User.LoginType.API, apiKey).Result;

            if (logged != Uniconta.Common.ErrorCodes.Succes)
            {
                Console.WriteLine("Login failed");
                return;
            }
            #endregion

            #region Selecting a company for the session
            // Select the company for session, otherwise it will be default company in Uniconta
            var companies = session.GetCompanies().Result;
            var company = session.GetCompany(companies[0].CompanyId).Result; // Getting specific company

            var defaultCompany = session.User._DefaultCompany; // returns an ID of the company

            session.OpenCompany(defaultCompany, true); // true sets the default company for the session, not as a default company for the user
            #endregion

            #region API examples
            // Set the API you want to use. Query, Crud and other APIs with different functionality
            var api = new CrudAPI(session, company);
            var qapi = new QueryAPI(session, company);
            #endregion

            #region Create records
            // Insert example
            CrmProspectClient prospect = new CrmProspectClient();

            prospect.Name = "UC corp.";
            prospect.CompanyRegNo = "12315151";
            prospect.Address1 = "Tree 1, Branch 2";
            
            // Single record insert
            var error = api.Insert(prospect).Result;

            Thread.Sleep(500); // This is just to make sure that insert is done before I request data
            var tempProspects = api.Query<CrmProspectClient>().Result; // This is without any filters and gets all the entities

            // This is local LINQ query, instead of using for loop or other loops to find correct entity
            var master = tempProspects.Where(pr => pr.Name == "UC corp.").FirstOrDefault();

            var contacts = new List<ContactClient>();

            ContactClient contact = new ContactClient();
            contact.Name = "Jane Doe";
            contact.Email = "test@testdoe.com";
            contact.SetMaster(master);

            contacts.Add(contact);

            contact = new ContactClient();
            contact.Name = "John Doe";
            contact.Email = "test@testdoe.com";
            contact.SetMaster(master);

            contacts.Add(contact);

            // Bulk insert, warning, dont insert too many records. Try to do it in batches.
            error = api.Insert(contacts).Result;
            #endregion

            #region Read records
            // Plain no filter query, gets all the records on this entity in session company.
            var debtorOrders = api.Query<DebtorOrderClient>().Result;

            // Just to show, what we got
            foreach (var creditor in debtorOrders)
                Console.WriteLine("Order: " + creditor.OrderNumber + " - " + creditor.Account);

            // Querying rows that are all related to master record.
            var orderLines = api.Query<DebtorOrderLineClient>(debtorOrders[0]).Result;

            // Setting up a filter
            DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime toDate = DateTime.Today;
            var filter = new List<PropValuePair>
            {
                PropValuePair.GenereteWhereElements("Date", typeof(DateTime), $"{fromDate.ToString("d/M-yyyy", CultureInfo.InvariantCulture)}..{toDate.ToString("d/M-yyyy", CultureInfo.InvariantCulture)}"),
                PropValuePair.GenereteOrderByElement("Account", true) // This is sorting filter.
            };

            var invoices = api.Query<DebtorInvoiceClient>(filter).Result;

            // Creating SQL filter
            filter = new List<PropValuePair>();
            filter.Add(PropValuePair.GenereteWhere("Account <= '1234' and Name like 'Something'"));

            var debtorClients = api.Query<DebtorClient>(filter).Result;

            // Pagination filter
            var page = 0;
            var pageSize = 10;

            filter = new List<PropValuePair>();
            filter.Add(PropValuePair.GenereteOrderByElement("RowId", false));
            filter.Add(PropValuePair.GenereteSkipN(pageSize * page));
            filter.Add(PropValuePair.GenereteTakeN(pageSize * (page + 1)));

            debtorClients = api.Query<DebtorClient>(filter).Result;

            #endregion

            #region Update records
            // Update example
            var prospects = api.Query<CrmProspect>().Result;
            prospects[0]._Address1 = "Tree 2, Branch 3";
            error = api.Update(prospects[0]).Result;

            // Update example using streamingmanager to make sure only fields you updated will be updated
            prospects = api.Query<CrmProspect>().Result;
            var updProspect = prospects[0];
            var originalProspect = StreamingManager.Clone(updProspect);

            updProspect._Address1 = "Tree 2, Branch 3";
            error = api.Update(originalProspect, updProspect).Result;

            // And again, you can update in bulk.
            #endregion

            #region Delete records
            // Delete näide
            prospects = api.Query<CrmProspect>().Result;
            error = api.Delete(prospects[0]).Result;
            #endregion

            #region Additional possibilities

            // MultiCrud enables you to insert, update and delete at the same time
            // api.MultiCrud()

            // NoResponse if no response is required with these operations
            // api.InsertNoResponse();
            // api.UpdateNoResponse();
            // api.DeleteNoResponse();

            // Cache
            SQLCache cache = api.CompanyEntity.GetCache(typeof(InvItemClient));
            if (cache == null)
                cache = api.CompanyEntity.LoadCache(typeof(InvItemClient), api).Result;
            var item = cache.Get("1001");
            
            // Gets all records from cache
            var invEntities = cache.GetRecords as InvItemClient[];
            // Get specific items
            var specItems = invEntities.Where(i => i.KeyName.Contains("old") && i.Available > 3);
            #endregion

            #region UserDocuments
            // Insert UserDocuments
            var file = File.ReadAllBytes(@"PATH TO FILE");
            UserDocsClient newUserDoc = new UserDocsClient
            {
                Created = DateTime.Now,
                DocumentType = FileextensionsTypes.DOCX,
                Text = "My file name for UC",
                _Data = file,
            };
            newUserDoc.SetMaster(prospect);

            ErrorCodes errorCode = api.Insert(newUserDoc).Result;


            // Read UserDocuments
            var prospectDocuments = api.Query<UserDocsClient>(prospect).Result;
            if (prospectDocuments.Length != 0)
            {
                var readResult = api.Read(prospectDocuments[0]).Result;
                var fileBytes = prospectDocuments[0]._Data;

                if (prospectDocuments[0].DocumentType == FileextensionsTypes.DOCX)
                    File.WriteAllBytes(@"PATH TO FILE", fileBytes);
            }
            #endregion

            /*Other APIs:
             
             GeneralLedger:
            •	ReportAPI
            •	BankStatementAPI
            •	DocumentAPI
            •	FinancialYearAPI
            •	PeriodTotalAPI
            •	PostingAPI
            •	StandardGLAccountAPI
            DebtorCreditor:
            •	DebtorOrderAPI
            •	InvoiceAPI
            •	ReportAPI
            •	TransactionAPI
            Inventory:
            •	PostingAPI
            •	ReportAPI
            System:
            •	CompanyAPI
            •	NumberSerieAPI
            •	CompanyAccessAPI
            •	UserAPI

             */

            // Always try to close your session when done.
            session.LogOut();
        }

        private static void AskCredentials(out string username, out string password)
        {
            Console.Write("Insert username: ");
            username = Console.ReadLine();
            Console.Write("Insert password: ");
            password = String.Empty;
            do
            {
                var cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                    break;
                password += cki.KeyChar;
            }
            while (true);
        }

    }
}
