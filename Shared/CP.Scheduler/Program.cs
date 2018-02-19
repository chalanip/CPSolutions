using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Scheduler
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                ConsumeData objasync = new ConsumeData();
                var result = await objasync.HitPostApi();
            }).GetAwaiter().GetResult();
        }
    }
}
