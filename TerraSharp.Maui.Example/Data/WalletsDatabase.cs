using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraSharp.Maui.Example.Models;

namespace TerraSharp.Maui.Example.Data
{
    public class WalletsDatabase
    {
        SQLiteAsyncConnection Database;

        public WalletsDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<Wallet>();
            await Database.CreateTableAsync<Log>();
        }
        public async Task<List<Wallet>> GetWalletsAsync()
        {
            await Init();
            return await Database.Table<Wallet>().ToListAsync();
        }

        public async Task<List<Log>> GetLogsAsync()
        {
            await Init();
            return await Database.Table<Log>().ToListAsync();
        }

        public async Task<Wallet> GetWalletByIdAsync(int id)
        {
            await Init();
            return await Database.Table<Wallet>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveWalletAsync(Wallet item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteWalletAsync(Wallet item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }


        public async Task<int> SaveLogItemAsync(Log item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }
    }
}
