using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using DocxTemplater;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace OrderonTailoringCostCalculation.Services
{
    public class ReceiptWriteService
    {
        private readonly ComplicatedElementService _complicatedElementService = new();
        private readonly ReceiptComplicatedElementService _receiptComplicatedElementService = new();

        public async Task<string> WriteReceipt(Receipt receipt)
        {
            await FileInitializer.CopyFileIfNotExistsAsync(Constants.MaterialsTemplateName);

            var template = DocxTemplate.Open(Constants.MaterialsTemplate);

            Collection<ComplicatedElement> complicatedElements = new();

            await _receiptComplicatedElementService.GetItemsAsync(receipt.ID);
            if (_receiptComplicatedElementService.ReceiptComplicatedElements != null)
            {
                foreach (var element in _receiptComplicatedElementService.ReceiptComplicatedElements)
                {
                    ComplicatedElement complicatedElement = await _complicatedElementService.GetItemAsync(element.ComplicatedElementID);
                    complicatedElements.Add(complicatedElement);
                }
            }

            template.BindModel("ds", new { Elements = complicatedElements });
            template.BindModel("mn", new { Value = receipt.MinValue });
            template.BindModel("tv", new { Total = receipt.TotalValue});
            template.BindModel("ce", new { ceTotal = receipt.ComplicatedElementsValue });
            template.BindModel("pc", new { Percent = receipt.DiscountValue });

            string currentReceiptName = "Чек_1" + ".docx";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, currentReceiptName);

            template.Save(filePath);

            using var fileStream = File.OpenRead(filePath);

            var fileSaverResult = await FileSaver.Default.SaveAsync(
            currentReceiptName,
            fileStream,
            CancellationToken.None);

            return filePath;
        }
    }
}
