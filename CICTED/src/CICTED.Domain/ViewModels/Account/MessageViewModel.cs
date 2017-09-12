using System;
using System.Collections.Generic;
using System.Text;

namespace CICTED.Domain.ViewModels.Account
{
    public class MessageViewModel
    {
        public string MessageTitle { get; set; }
        public List<string> Description { get; set; } = new List<string>();
        public bool HasErrors{ get; set; }
    }
}
