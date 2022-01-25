using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Fraud;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class FraudService : IFraudService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;

        public FraudService(ICustomerRepository customerRepository, IGenericRepository<Transaction> transactionRepository)
        {
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task StartFraudScan()
        {
            var lastScannedDate = GetLastScannedDate();

            SetNewLastDate(DateTime.UtcNow);

            var countries = await _customerRepository.GetAllCountries();

            foreach (var country in countries)
            {
                await ProccessCountryInBatches(country, lastScannedDate);
            }
        }

        private async Task ProccessCountryInBatches(string country, DateTime lastScannedDate)
        {
            var fraudList = new List<FraudDTO>();
            const int batchSize = 3000;
            int currentBatch = 1;

            while (true)
            {
                var batch = await _customerRepository.GetPagedListAsync(
                filter: c => c.Country == country,
                include: q => q.Include(c => c.Dispositions).ThenInclude(d => d.Account),
                page: currentBatch, pageSize: batchSize);

                foreach (var customer in batch.Results)
                {
                    foreach (var account in customer.Dispositions)
                    {
                        var currentTransactionsBatch = 1;
                        while (true)
                        {
                            var accountTransactions = await _transactionRepository.GetPagedListAsync(
                                filter: t => t.AccountNavigation.AccountId == account.Account.AccountId && t.Date > lastScannedDate,
                                orderBy: q => q.OrderBy(t => t.Date),
                                page: currentTransactionsBatch, pageSize: batchSize);

                            foreach (var transaction in accountTransactions.Results)
                            {
                                ProccessTransaction(fraudList, customer, account, accountTransactions, transaction);
                            }

                            currentTransactionsBatch++;
                            if (currentTransactionsBatch > batch.PageCount) break;
                        }
                    }
                }

                currentBatch++;
                if (currentBatch > batch.PageCount) break;
            }

            if (fraudList.Count <= 0) return;

            SendListToCountryMail(country, fraudList);
        }

        private static void ProccessTransaction(List<FraudDTO> fraudList, Customer customer, Disposition account, PagedResult<Transaction> accountTransactions, Transaction transaction)
        {
            var transactionsNextThreeDays = accountTransactions.Results.Where(
                    t => t.Date >= transaction.Date &&
                    t.Date <= transaction.Date.AddHours(72));

            if (IsFraudTransaction(fraudList, transactionsNextThreeDays))
            {
                fraudList.AddRange(transactionsNextThreeDays.Select(t => new FraudDTO
                {
                    AccountId = account.Account.AccountId,
                    CustomerId = customer.CustomerId,
                    TransactionId = t.TransactionId
                }));
            }

            if (IsFraudTransaction(fraudList, transaction))
            {
                fraudList.Add(new FraudDTO
                {
                    TransactionId = transaction.TransactionId,
                    CustomerId = customer.CustomerId,
                    AccountId = account.Account.AccountId,
                });
            }
        }

        private static void SendListToCountryMail(string country, List<FraudDTO> fraudList)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"Sending a mail to {country}@testbanken.se with {fraudList.Count} fraud objects");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = default;
        }

        private void SetNewLastDate(DateTime date)
        {
            File.WriteAllLines(@".\FraudDetection.txt", new[] { date.ToString() });
        }

        private static bool IsFraudTransaction(List<FraudDTO> fraudList, IEnumerable<Transaction> transactionsNextThreeDays)
        {
            return transactionsNextThreeDays.Sum(t => t.Amount) >= FraudConstants.MultiTransactionFraudAmount &&
                                            transactionsNextThreeDays.Any(t => fraudList.Any(f => f.TransactionId == t.TransactionId));
        }

        private static bool IsFraudTransaction(List<FraudDTO> fraudList, Transaction transaction)
        {
            return transaction.Amount >= FraudConstants.SingleTransactionFraudAmount &&
                                            !fraudList.Any(f => f.TransactionId == transaction.TransactionId);
        }

        private DateTime GetLastScannedDate()
        {
            if (!File.Exists(@".\FraudDetection.txt")) SetNewLastDate(default(DateTime));

            var text = File.ReadAllLines(@".\FraudDetection.txt");
            if (!string.IsNullOrEmpty(text[0])) return DateTime.Parse(text[0]);

            return default(DateTime);
        }
    }
}
