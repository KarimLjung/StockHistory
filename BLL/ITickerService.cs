using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ITickerService
    {
        Task<string> GetTickerInformation(string ticker);
    }
}
