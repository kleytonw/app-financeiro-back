using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using OFXSharp;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class OfxController : ControllerBase
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public string Type { get; set; }
    }

    [HttpPost("import")]
    [AllowAnonymous]
    public async Task<IActionResult> ImportOfx(IFormFile ofxFile)
    {
        if (ofxFile == null || ofxFile.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        try
        {
            // Read the OFX file content
            string ofxData;
            using (var reader = new StreamReader(ofxFile.OpenReadStream()))
            {
                ofxData = await reader.ReadToEndAsync();
            }

            // Parse the OFX data
            var transactions = ParseOfxToList(ofxData);

            // Return the transactions as JSON
            return Ok(new
            {
                transacoes = transactions,
                //total = transactions.Sum(x => x.Amount)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the file: {ex.Message}");
        }
    }

    private List<Transaction> ParseOfxToList(string ofxData)
    {
        var transactionList = new List<Transaction>();

        var a = new StringReader(ofxData);

        var ofxDocumentParser = new OFXDocumentParser();
        var ofxDocument = ofxDocumentParser.Import(ofxData);

        foreach (var trans in ofxDocument.Transactions)
        {
            transactionList.Add(new Transaction
            {
                Date = trans.Date,
                Amount = trans.Amount,
                Memo = trans.Memo,
                Type = trans.TransType.ToString()
            });
        }

        return transactionList;
    }
}
