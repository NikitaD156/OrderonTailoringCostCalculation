using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class DiscountService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Discount>();
        }
        public async Task<List<Discount>> GetItemsAsync()
        {
            await Init();
            return await database.Table<Discount>().ToListAsync();
        }

        public async Task<Discount> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<Discount>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Discount item)
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

        public async Task<int> DeleteItemAsync(Discount item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
