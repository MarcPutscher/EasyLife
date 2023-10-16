using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Serviceklasse für die Bilanzprofile.
    /// </summary>
    public class BalanceService
    {
        public static SQLiteAsyncConnection db;

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        /// <returns></returns>
        public static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<HelperBalanceprofile>();
        }

        /// <summary>
        /// Fügt ein Bilanzprofil hinzu.
        /// </summary>
        /// <param name="input">Das Bilanzprofil das in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<bool> Add_Balanceprofile(Balanceprofile input)
        {
            if (input == null)
            {
                return false;
            }

            await Init();

            try
            {
                HelperBalanceprofile convertedinput = Konverter.Serilize(input);

                await db.InsertAsync(convertedinput);

                input.Id = convertedinput.Id;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Bearbeitet ein Bilanzprofil.
        /// </summary>
        /// <param name="item">Das Bilanzprofil das in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Balanceprofile(Balanceprofile input)
        {
            await Init();

            if (input == null)
            {
                return ;
            }

            try
            {
                HelperBalanceprofile convertedinput = Konverter.Serilize(input);

                convertedinput.Id = input.Id;

                await db.UpdateAsync(convertedinput);
            }
            catch
            {
                return;
            }

        }

        /// <summary>
        /// Löscht ein Bilanzprofil.
        /// </summary>
        /// <param name="input">Die Id zu dem Bilanzprofil das in der Datenbank gelöscht werden soll.</param>
        /// <returns></returns>
        public static async Task<bool> Remove_Balanceprofile(int input)
        {
            await Init();

            try
            {
                await db.DeleteAsync<HelperBalanceprofile>(input);

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gibt alle Bilanzprofile die in der Datenbank sind zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Balanceprofile>> Get_all_Balanceprofile()
        {
            await Init();

            try
            {
                List<Balanceprofile> balanceprofilesList = new List<Balanceprofile>();

                var result = await db.Table<HelperBalanceprofile>().ToListAsync();

                if(result == null)
                {
                    return new List<Balanceprofile>();
                }

                foreach (HelperBalanceprofile helperBalanceprofile in result)
                {
                    if(helperBalanceprofile != null)
                    {
                        balanceprofilesList.Add(Konverter.Deserilize(helperBalanceprofile));
                    }
                }

                return balanceprofilesList;
            }
            catch
            {
                return new List<Balanceprofile>();
            }
        }

        /// <summary>
        /// Gibt ein spezifisches Bilanzprofil zurück.
        /// </summary>
        /// <param name="input">Die Bialnzprofilid die zum finden des spezifischen Bilanzprofil notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Balanceprofile> Get_specific_Balanceprofile(int input)
        {
            await Init();

            return Konverter.Deserilize(await db.FindAsync<HelperBalanceprofile>(input));
        }
    }

    /// <summary>
    /// ISt eine Hilfsklasse um Bilanzprofile zu erstellen.
    /// </summary>
    public class HelperBalanceprofile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Outcome_Account { get; set; }

        public string Income_Account { get; set; }

        public string Outcome_Cash { get; set; }

        public string Income_Cash { get; set; }

        public string Ignore {  get; set; }
    }

    /// <summary>
    /// Ist eine Konvertierungsklasse um Bilanzprofile de-/serealisieren.
    /// </summary>
    public class Konverter
    {
        public static HelperBalanceprofile Serilize( Balanceprofile input)
        {
            HelperBalanceprofile output = new HelperBalanceprofile();

            foreach(string zw in input.Outcome_Account)
            {
                output.Outcome_Account += zw;
                output.Outcome_Account += "#";
            }

            foreach (string zw in input.Income_Account)
            {
                output.Income_Account += zw;
                output.Income_Account += "#";
            }

            foreach (string zw in input.Outcome_Cash)
            {
                output.Outcome_Cash += zw;
                output.Outcome_Cash += "#";
            }

            foreach (string zw in input.Income_Cash)
            {
                output.Income_Cash += zw;
                output.Income_Cash += "#";
            }

            foreach (string zw in input.Ignore)
            {
                output.Ignore += zw;
                output.Ignore += "#";
            }

            return output;
        }

        public static Balanceprofile Deserilize(HelperBalanceprofile input)
        {
            Balanceprofile output = new Balanceprofile();

            if(String.IsNullOrEmpty(input.Outcome_Account) == false)
            {
                while (input.Outcome_Account.Count() > 0)
                {
                    output.Outcome_Account.Add(input.Outcome_Account.Substring(0, input.Outcome_Account.IndexOf("#")));
                    input.Outcome_Account = input.Outcome_Account.Remove(0, input.Outcome_Account.IndexOf("#") + 1);
                }
            }
            else
            {
                output.Outcome_Account = new List<string>();
            }

            if(String.IsNullOrEmpty(input.Income_Account) == false)
            {
                while (input.Income_Account.Count() > 0)
                {
                    output.Income_Account.Add(input.Income_Account.Substring(0, input.Income_Account.IndexOf("#")));
                    input.Income_Account = input.Income_Account.Remove(0, input.Income_Account.IndexOf("#") + 1);
                }
            }
            else
            {
                output.Income_Account = new List<string>();
            }

            if(String.IsNullOrEmpty(input.Outcome_Cash) == false)
            {
                while (input.Outcome_Cash.Count() > 0)
                {
                    output.Outcome_Cash.Add(input.Outcome_Cash.Substring(0, input.Outcome_Cash.IndexOf("#")));
                    input.Outcome_Cash = input.Outcome_Cash.Remove(0, input.Outcome_Cash.IndexOf("#") + 1);
                }
            }
            else
            {
                output.Outcome_Cash = new List<string>();
            }

            if(String.IsNullOrEmpty(input.Income_Cash) == false)
            {
                while (input.Income_Cash.Count() > 0)
                {
                    output.Income_Cash.Add(input.Income_Cash.Substring(0, input.Income_Cash.IndexOf("#")));
                    input.Income_Cash = input.Income_Cash.Remove(0, input.Income_Cash.IndexOf("#") + 1);
                }
            }
            else
            {
                output.Income_Cash = new List<string>();
            }

            if (String.IsNullOrEmpty(input.Ignore) == false)
            {
                while (input.Ignore.Count() > 0)
                {
                    output.Ignore.Add(input.Ignore.Substring(0, input.Ignore.IndexOf("#")));
                    input.Ignore = input.Ignore.Remove(0, input.Ignore.IndexOf("#") + 1);
                }
            }
            else
            {
                output.Ignore = new List<string>();
            }

            output.Id = input.Id;

            return output;
        }
    }
}
