using CommunityToolkit.Mvvm.ComponentModel;
using OrderonTailoringCostCalculation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class ReceiptDiscountService
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
            {
                return;
            }
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<ReceiptDiscount>();
        }

        private List<ReceiptDiscount> _receiptDiscount = new List<ReceiptDiscount>();

        public IReadOnlyList<ReceiptDiscount> ReceiptDiscounts => _receiptDiscount.AsReadOnly();

        public async Task<List<ReceiptDiscount>> GetItemsAsync(int receiptID)
        {
            await Init();
            return _receiptDiscount = await database.Table<ReceiptDiscount>().Where(i => i.ReceiptID == receiptID).ToListAsync();
        }

        public async Task<int> SaveItemAsync(ReceiptDiscount item)
        {
            await Init();

            return await database.InsertAsync(item);

        }

        public async Task<int> DeleteItemAsync(ReceiptDiscount item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
