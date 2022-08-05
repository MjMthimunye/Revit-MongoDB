using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace cloudDB
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class ImportCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiApp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiApp.ActiveUIDocument.Document;

            //REST request to GET door data 
            List<Door> doors = DoorAPI.Get("doors");
            
            //Set Door finish values using unique
            if(doors != null && doors.Count > 0)
            {
                using(Transaction trans = new Transaction(doc, "Import Door Data"))
                {
                    trans.Start();

                    foreach(Door door in doors)
                    {
                        string uId = door._id;
                        Element element = doc.GetElement(uId);

                        string dFinish = door.DoorFinish;
                        Parameter doorFinish = element.get_Parameter(BuiltInParameter.DOOR_FINISH);

                        doorFinish.Set(dFinish);
                    }

                    trans.Commit();
                }
            }

            TaskDialog.Show("Import Door Data", "Door Data successfuly imported");

            return Result.Succeeded;
        }

    }
}
