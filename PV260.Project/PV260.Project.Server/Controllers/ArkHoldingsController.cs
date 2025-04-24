using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Server.Dtos;

namespace PV260.Project.Server.Controllers;

public class ArkHoldingsController : ApiController
{
    [Authorize]
    [HttpGet("getArkHoldingsDiff", Name = "getArkHoldingsDiff")]
    public async Task<ActionResult<List<ArkHoldingsDiffDto>>> GetArkHoldingsDiffs()
    {
        return Ok(GetDummyData());
    }

    private static List<ArkHoldingsDiffDto> GetDummyData()
    {
        return
        [
            new ArkHoldingsDiffDto
            {
                Date = "03/04/2025",
                Date2 = "03/05/2025",
                Fund = "ARKK",
                Company = "TESLA INC",
                Ticker = "TSLA",
                Cusip = "88160R101",
                Shares = "2,251,498",
                Shares2 = "2,251,498",
                SharesDiff = "0",
                MarketValue = "$640,888,905.70",
                MarketValue2 = "$640,888,905.70",
                MarketValueDiff = "$0.00",
                Weight = "11.18%",
                Weight2 = "11.18%",
                WeightDiff = "0%"
            },

            new ArkHoldingsDiffDto
            {
                Date = "03/04/2025",
                Date2 = "03/05/2025",
                Fund = "ARKK",
                Company = "ROKU INC",
                Ticker = "ROKU",
                Cusip = "77543R102",
                Shares = "6,346,685",
                Shares2 = "6,200,000",
                SharesDiff = "-146,685",
                MarketValue = "$509,384,938.10",
                MarketValue2 = "$497,201,500.00",
                MarketValueDiff = "-$12,183,438.10",
                Weight = "8.88%",
                Weight2 = "8.65%",
                WeightDiff = "-0.23%",
            },

            new ArkHoldingsDiffDto
            {
                Date = "03/04/2025",
                Date2 = "03/05/2025",
                Fund = "ARKK",
                Company = "ROBLOX CORP -CLASS A",
                Ticker = "RBLX",
                Cusip = "771049103",
                Shares = "6,634,959",
                Shares2 = "6,700,000",
                SharesDiff = "65,041",
                MarketValue = "$412,561,750.62",
                MarketValue2 = "$417,658,000.00",
                MarketValueDiff = "$5,096,249.38",
                Weight = "7.19%",
                Weight2 = "7.25%",
                WeightDiff = "0.06%",
            }
        ];
    }
}