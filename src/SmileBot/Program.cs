using System;
using System.IO;
using System.Threading.Tasks;

namespace SmileBot
{
    internal class Program
    {
        private static void Main () => new Program ().MainAsync ().GetAwaiter ().GetResult ();

        private async Task MainAsync ()
        {
            await new SmileBot ().RunAndBlockAsync ();
        }
    }
}