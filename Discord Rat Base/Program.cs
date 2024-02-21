using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Rat_Base
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.Main();
        }
    }
}
