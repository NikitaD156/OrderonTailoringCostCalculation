using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class ReceiptComplicatedElementService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<ReceiptComplicatedElement>();
        }

        private List<ReceiptComplicatedElement> _receiptComplicatedElements = new List<ReceiptComplicatedElement>();

        public IReadOnlyList<ReceiptComplicatedElement> ReceiptComplicatedElements => _receiptComplicatedElements.AsReadOnly();

        public async Task<List<ReceiptComplicatedElement>> GetItemsAsync(int receiptID)
        {
            await Init();
            return _receiptComplicatedElements = await database.Table<ReceiptComplicatedElement>().Where(i => i.ReceiptID == receiptID).ToListAsync();
        }

        public async Task<int> SaveItemAsync(ReceiptComplicatedElement item)
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

        public async Task<int> DeleteItemAsync(ReceiptComplicatedElement item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
