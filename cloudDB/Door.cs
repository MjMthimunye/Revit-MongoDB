using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudDB
{
    /// <summary>
    /// Door class with get/set autoproperties
    /// </summary>
    public class Door
    {
        public string _id { get; set; }
        public string FamilyType { get; set; }
        public string Mark { get; set; }
        public string DoorFinish { get; set; }

    }
}
