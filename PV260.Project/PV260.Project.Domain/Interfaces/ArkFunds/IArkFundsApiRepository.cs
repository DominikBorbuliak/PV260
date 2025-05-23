﻿using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.ArkFunds;

public interface IArkFundsApiRepository
{
    Task<IList<ArkFundsHolding>> GetCurrentHoldingsAsync();
}
