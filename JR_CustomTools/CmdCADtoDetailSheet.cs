#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forms = System.Windows.Forms;

#endregion

namespace JR_CustomTools
{
    [Transaction(TransactionMode.Manual)]
    public class CmdCADtoDetailSheet : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Open File Dialog                                                          //Add as a utilities method? and return string for file path
            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.Multiselect = false;
            dialog.Filter = "DWG files | *.dwg";

            if (dialog.ShowDialog() != Forms.DialogResult.OK) 
            {
                TaskDialog.Show("Error", "Please select an AutoCAD file");
            }
            string dwgFile = dialog.FileName;

            ElementId elementId = null;                         //Create an empty element ID for the imported element to output to

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Detail Sheet from DWG");

                //Create new Drafting View(To hold Imported View)
                ViewDrafting importView = ViewDrafting.Create(doc, doc.GetDefaultElementTypeId(ElementTypeGroup.ViewTypeDrafting));
                importView.Name = "Test Import View";
                //Import drawing file
                DWGImportOptions importOptions = new DWGImportOptions();
                importOptions.ColorMode = ImportColorMode.BlackAndWhite;
                doc.Import(dwgFile, importOptions, importView, out elementId);

                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
