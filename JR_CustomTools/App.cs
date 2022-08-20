#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

#endregion

namespace JR_CustomTools
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            //Step 1: Create Ribbon Tab
            try
            {
                app.CreateRibbonTab("JR Custom Tools");
            }
            catch (Exception)
            {
                Debug.Print("Tab already exists");
            }
            //Step 2: Create Ribbon Panels
            RibbonPanel CADPanel = CreateRibbonPanel(app, "JR Custom Tools", "CAD Conversion");

            //Step 3: Create button data instances
            PushButtonData pData1 = new PushButtonData("button1", "Create Detail Sheet\nfrom CAD file", GetAssemblyName(),  "JR_CustomTools.CmdCADtoDetailSheet");

            //Step 4: Add images
            pData1.Image = BitmapToImageSource(JR_CustomTools.Properties.Resources.blueprint_16x16);
            pData1.LargeImage = BitmapToImageSource(JR_CustomTools.Properties.Resources.blueprint_32x32);

            //Step 5: add tooltip info
            pData1.ToolTip = "Creates a detail sheet from an AutoCAD file";

            //Step 6: create buttons
            PushButton pButton1 = CADPanel.AddItem(pData1) as PushButton;

            return Result.Succeeded;
        }

        private ImageSource BitmapToImageSource(System.Drawing.Bitmap bm)
        {
            using (MemoryStream mem = new MemoryStream())                  //Allocation of memory
            {
                bm.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                mem.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = mem;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                return bmi;
            }
        }

        private string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            foreach(RibbonPanel curPanel in app.GetRibbonPanels(tabName))       //Search for existing ribbon
            {
                if(curPanel.Name == panelName)
                    return curPanel;
            }
            RibbonPanel returnPanel = app.CreateRibbonPanel(tabName,panelName);
            return returnPanel;
        }
    }
}
