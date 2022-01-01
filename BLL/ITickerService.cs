using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ITickerService
    {
        Task<TickerInfo> GetTickerInformation(string ticker);
        Task<IEnumerable<TickerInfos>> GetTickerInformations();
    }
}
