using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class MultiplierService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Multiplier>();
        }
        private List<Multiplier> _multiplier = new List<Multiplier>();

        public IReadOnlyList<Multiplier> Multipliers => _multiplier.AsReadOnly();

        public async Task<List<Multiplier>> GetItemsAsync()
        {
            await Init();
            return _multiplier = await database.Table<Multiplier>().ToListAsync();

        }

        public async Task<Multiplier> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<Multiplier>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Multiplier item)
        {
            await Init();
            if (item.ID != 0)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(Multiplier item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
