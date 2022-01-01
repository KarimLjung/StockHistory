﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using DAL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public partial class StockhistoryContext
    {
        private StockhistoryContextProcedures _procedures;

        public virtual StockhistoryContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new StockhistoryContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public StockhistoryContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class StockhistoryContextProcedures
    {
        private readonly StockhistoryContext _context;

        public StockhistoryContextProcedures(StockhistoryContext context)
        {
            _context = context;
        }

        public virtual async Task<List<GetTickerInfoResult>> GetTickerInfoAsync(string TickerId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "TickerId",
                    Size = 64,
                    Value = TickerId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<GetTickerInfoResult>("EXEC @returnValue = [dbo].[GetTickerInfo] @TickerId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}