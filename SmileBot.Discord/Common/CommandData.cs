using System;
using System.Collections.Generic;
using System.Text;

namespace SmileBot.Common
{
    public class CommandData
    {
        public string Cmd { get; set; }
        public string Desc { get; set; }
        public string[] Usage { get; set; }
    }
}