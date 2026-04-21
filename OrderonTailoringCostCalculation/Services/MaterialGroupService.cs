using System;
using System.Collections.Generic;
using System.Text;
using OrderonTailoringCostCalculation.Models;
using SQLite;

namespace OrderonTailoringCostCalculation.Services
{
    public class MaterialGroupService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<MaterialGroup>();
        }
        private List<MaterialGroup> _materialGroup = new List<MaterialGroup>();

        public IReadOnlyList<MaterialGroup> MaterialGroups => _materialGroup.AsReadOnly();

        public async Task<List<MaterialGroup>> GetItemsAsync()
        {
            await Init();
            return _materialGroup = await database.Table<MaterialGroup>().ToListAsync();

        }

        public async Task<MaterialGroup> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<MaterialGroup>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(MaterialGroup item)
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

        public async Task<int> DeleteItemAsync(MaterialGroup item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
