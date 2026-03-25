using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class ComplicatedElementService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<ComplicatedElement>();
        }
        public async Task<List<ComplicatedElement>> GetItemsAsync()
        {
            await Init();
            return await database.Table<ComplicatedElement>().ToListAsync();
        }

        public async Task<ComplicatedElement> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<ComplicatedElement>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(ComplicatedElement item)
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

        public async Task<int> DeleteItemAsync(ComplicatedElement item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
