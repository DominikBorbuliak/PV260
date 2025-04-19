using PV260.Project.BusinessLayer.Models;

namespace PV260.Project.BusinessLayer.Interfaces.DataAccessLayer;

public interface IArkFundsApiRepository
{
    Task<IList<ArkFundsHolding>> GetCurrentHoldingsAsync();
}
