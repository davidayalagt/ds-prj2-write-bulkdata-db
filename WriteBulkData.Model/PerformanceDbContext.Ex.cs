using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteBulkData.Model
{
    public partial class PerformanceDbContext
    {
        partial void InitializePartial()
        {
            System.Data.Entity.Database.SetInitializer<PerformanceDbContext>(null);
        }
    }
}
