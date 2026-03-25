using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class ReceiptService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            await DataBaseInitializer.CopyDatabaseIfNotExistsAsync(Constants.DatabaseFilename);

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Receipt>();
        }

        private List<Receipt> _receipts = new List<Receipt>();

        public IReadOnlyList<Receipt> Receipts => _receipts.AsReadOnly();

        public async Task<List<Receipt>> GetItemsAsync()
        {
            await Init();
            return _receipts = await database.Table<Receipt>().ToListAsync();
            
        }

        public async Task<Receipt> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<Receipt>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<ReceiptDetails> GetReceiptWithGarmentAsync(int id)
        {
            await Init();

            var receipt = await database.Table<Receipt>().Where(r => r.ID == id).FirstOrDefaultAsync();
            if (receipt == null)
                return null;
            
            var garment = await database.Table<MinValueGarment>().Where(g => g.ID == receipt.MinValueGarmentID).FirstOrDefaultAsync();

            return new ReceiptDetails
            {
                Receipt = receipt,
                MinValueGarment = garment
            };
        }

        public async Task<int> SaveItemAsync(Receipt item)
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

        public async Task<int> DeleteItemAsync(Receipt item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

    }
}
