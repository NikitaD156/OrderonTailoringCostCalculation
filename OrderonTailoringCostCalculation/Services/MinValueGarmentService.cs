using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class MinValueGarmentService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<MinValueGarment>();
        }
        private List<MinValueGarment> _minValueGarment = new List<MinValueGarment>();

        public IReadOnlyList<MinValueGarment> MinValueGarments => _minValueGarment.AsReadOnly();

        public async Task<List<MinValueGarment>> GetItemsAsync()
        {
            await Init();
            return _minValueGarment = await database.Table<MinValueGarment>().ToListAsync();

        }

        public async Task<MinValueGarment> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<MinValueGarment>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(MinValueGarment item)
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

        public async Task<int> DeleteItemAsync(MinValueGarment item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
