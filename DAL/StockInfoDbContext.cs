using BLL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StockInfoDbContext :DbContext
    {
        public DbSet<TickerInfo> TickerInfo { get; set; }
        


        public StockInfoDbContext(DbContextOptions<StockInfoDbContext> options):base(options)
        {
        }
    }


}
