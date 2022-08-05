using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using System.IO;

namespace cloudDB
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        /// <summary>
        /// Implements the OnShutdown event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Implements the OnStartup event
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = RibbonPanel(application);
            AddSplitButtonGroup(panel);


            return Result.Succeeded;
        }

        /// <summary>
        /// Adds split button group with two push buttons. 
        /// One to export door data and the other to import door data 
        /// </summary>
        /// <param name="panel"></param>
        private void AddSplitButtonGroup(RibbonPanel panel)
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            SplitButtonData sbData = new SplitButtonData("SplitGroup", "SplitGroup");
            SplitButton splitButton = panel.AddItem(sbData) as SplitButton;

            PushButtonData exportData = new PushButtonData("ExportData", "ExportData", thisAssemblyPath, "cloudDB.ExportCommand");
            PushButton exportButton = splitButton.AddPushButton(exportData);
            exportButton.ToolTip = "Export Revit Door Data";

            Uri exportUri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "mongoExp.ico"));
            BitmapImage expBitmapImage = new BitmapImage(exportUri);
            exportButton.LargeImage = expBitmapImage;

            splitButton.AddSeparator();

            PushButtonData importData = new PushButtonData("ImportData", "ImportData", thisAssemblyPath, "cloudDB.ImportCommand");
            PushButton importButton = splitButton.AddPushButton(importData);
            importButton.ToolTip = "Import Revit Door Data";

            Uri importUri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "mongoImp.ico"));
            BitmapImage impBitmapImage = new BitmapImage(importUri);
            importButton.LargeImage = impBitmapImage;

        }

        /// <summary>
        /// Function creates Jacobian Dev tab and cloudDB ribbon panel
        /// </summary>
        /// <param name="a"></param>
        /// <returns name="ribbonPanel"> </returns>
        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            // Tab name
            string tab = "Jacobian Dev";
            // Empty ribbon panel 
            RibbonPanel ribbonPanel = null;


            //Create ribbon Tab
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                TaskDialog.Show(ex.Message, ex.ToString());
            }


            //Create ribbon panel
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, "cloudDB");
            }
            catch (Exception ex)
            {
                TaskDialog.Show(ex.Message, ex.ToString());
            }

            // Search existing tab for your panel.
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels.Where(p => p.Name == "cloudDB"))
            {
                ribbonPanel = p;
            }

            return ribbonPanel;
        }


    }
}
