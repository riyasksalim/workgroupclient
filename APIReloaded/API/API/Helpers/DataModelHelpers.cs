using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace API.Helpers
{
    public static class DataModelHelpers
    {

        public const string SET_TRANSACTION_ISOLATION_LEVEL_READ_UNCOMMITTED = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

        public static void AllowDirtyReads(this DbContext ctx)
        {
            ctx.Database.ExecuteSqlCommand(SET_TRANSACTION_ISOLATION_LEVEL_READ_UNCOMMITTED);
        }

        public static void SetCommandTimeOut(this DbContext ctx, int commandTimeout)
        {
            ctx.Database.CommandTimeout = commandTimeout;
        }
    }
}