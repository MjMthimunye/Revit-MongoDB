using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace cloudDB
{
    public class DoorData : Door
    {
        /// <summary>
        /// DoorData constructor to generate the data to
        /// serialise for REST POST request.
        /// </summary>
        public DoorData(Element door)
        {
            //Get Door Finish parameter
            Parameter doorFinish = door.get_Parameter(BuiltInParameter.DOOR_FINISH);
            string dFinish;

            //Check if Door Finish Paramater has a value
            if (doorFinish.HasValue == true)
            {
                dFinish = doorFinish.AsString();
            }
            else
            {
                dFinish = "";
            }

            _id = door.UniqueId;
            FamilyType = door.Name;
            Mark = door.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
            DoorFinish = dFinish;
        }

    }
}
