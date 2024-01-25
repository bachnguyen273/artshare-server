using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Infrastructure.Repositories
{
    public class WatermarkRepository : GenericRepository<Watermark>, IWatermarkRepository
    {
        public WatermarkRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
