using Fanda.Core;
using Fanda.Domain.Context;

using System;
using System.Text;

namespace Fanda.Service
{
    public interface ISerialNumberRepository
    {
        string NextNumber(Guid yearId, SerialNumberModule serialNumber, DateTime? yearEnd = default);
    }

    public sealed class SerialNumberRepository : ISerialNumberRepository
    {
        //private static volatile SerialNumberRepository _instance;
        private static readonly object syncRoot = new object();

        //private readonly AppSettings _settings;
        private readonly FandaContext _context;

        public SerialNumberRepository(FandaContext context)
        {
            _context = context;
        }

        //public SerialNumberRepository(AppSettings settings)
        //{
        //    _settings = settings;
        //}
        //public static SerialNumberRepository Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (syncRoot)
        //            {
        //                if (_instance == null)
        //                    _instance = new SerialNumberRepository();
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        public string NextNumber(Guid yearId, SerialNumberModule module,
            DateTime? yearEnd = default)
        {
            try
            {
                //var optionsBuilder = DbContextExtensions.CreateDbContextOptionsBuilder(_settings);
                //using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                //using var context = new FandaContext(optionsBuilder.Options);
                string moduleString = module.ToString();

                lock (syncRoot)
                {
                    var serialNumber = _context.SerialNumbers
                        .Find(yearId, moduleString);

                    int firstIndex = serialNumber.SerialFormat.IndexOf('N');    // YYJJJNNNNN = 5
                    int lastIndex = serialNumber.SerialFormat.LastIndexOf('N'); // YYJJJNNNNN = 9
                    string nums = serialNumber.SerialFormat.Substring(firstIndex, lastIndex - firstIndex + 1);

                    int nextNumber = serialNumber.LastNumber + 1;
                    //Interlocked.Increment(ref nextNumber);                      // 100000

                    switch (serialNumber.Reset)
                    {
                        case SerialNumberReset.Max:
                            if (nextNumber.ToString().Length > nums.Length)
                            {
                                //Interlocked.Exchange(ref nextNumber, 1);
                                nextNumber = 1;
                            }
                            break;

                        case SerialNumberReset.Daily:
                            if (serialNumber.LastDate.Day != DateTime.Today.Day)
                            {
                                //Interlocked.Exchange(ref nextNumber, 1);
                                nextNumber = 1;
                            }
                            break;

                        case SerialNumberReset.Monthly:
                            if (serialNumber.LastDate.Month != DateTime.Today.Month)
                            {
                                //Interlocked.Exchange(ref nextNumber, 1);
                                nextNumber = 1;
                            }
                            break;

                        case SerialNumberReset.CalendarYear:
                            if (DateTime.Today.Year > serialNumber.LastDate.Year)
                            {
                                //Interlocked.Exchange(ref nextNumber, 1);
                                nextNumber = 1;
                            }
                            break;

                        case SerialNumberReset.AccountingYear:  // 01/04/2019 - 31/03/2020
                                                                //var accountYear = await context.AccountYears
                                                                //    .FindAsync(yearId);
                            if (DateTime.Today > yearEnd)
                            {
                                //Interlocked.Exchange(ref nextNumber, 1);
                                nextNumber = 1;
                            }
                            break;
                    }

                    StringBuilder sbSerialNumber =
                        new StringBuilder($"{serialNumber.Prefix}{serialNumber.SerialFormat}{serialNumber.Suffix}");

                    sbSerialNumber.Replace("YYYY", DateTime.Today.ToString("yyyy"));
                    sbSerialNumber.Replace("YY", DateTime.Today.ToString("yy"));
                    sbSerialNumber.Replace("MMM", DateTime.Today.ToString("MMM").ToUpper());
                    sbSerialNumber.Replace("MM", DateTime.Today.ToString("MM"));
                    sbSerialNumber.Replace("DD", DateTime.Today.ToString("dd"));
                    sbSerialNumber.Replace("JJJ", $"{DateTime.Today.DayOfYear:D3}");
                    sbSerialNumber.Replace("HH", DateTime.Now.ToString("HH"));
                    sbSerialNumber.Replace("MI", DateTime.Now.ToString("mm"));
                    sbSerialNumber.Replace("SS", DateTime.Now.ToString("ss"));

                    string nextSerial = nextNumber.ToString().PadLeft(nums.Length, '0');
                    sbSerialNumber.Replace(nums, nextSerial);
                    string nextValue = sbSerialNumber.ToString();

                    serialNumber.LastValue = nextValue;
                    serialNumber.LastNumber = nextNumber;
                    serialNumber.LastDate = DateTime.Now;
                    _context.SerialNumbers.Update(serialNumber);
                    _context.SaveChanges();
                    //scope.Complete();
                    return nextValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

//var con = context.Database.GetDbConnection();
//var result = await con.ExecuteScalarAsync<string>("SELECT TOP 1 LastValue FROM SerialNumbers");
