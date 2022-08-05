using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Net;


namespace cloudDB
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Get all doors from project
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_Doors);

            Result result = ExportBatch(collector, ref message);

            if(result == Result.Succeeded)
            {
                TaskDialog.Show("Door Data Export", "Door Data successfuly exported");
            }
            else
            {
                TaskDialog.Show("Door Data Export", "Something went wrong...");
            }

            return result;

        }
        

        /// <summary>
        /// Function to batch export Door Data
        /// </summary>
        /// <param name="doors"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result ExportBatch(FilteredElementCollector doors, ref string message)
        {
            List<Door> doorData = new List<Door>();

            HttpStatusCode statusCode;
            string jsonResponse, errorMessage;
            Result result = Result.Succeeded;

            foreach(Element element in doors)
            {
                doorData.Add(new DoorData(element));
            }

            //REST request to batch post door data
            statusCode = DoorAPI.PostBatch(out jsonResponse, out errorMessage, "doors", doorData);

            if((int)statusCode == 0)
            {
                message = errorMessage;
                result = Result.Failed;
            }

            return result;

        }


    }
}
