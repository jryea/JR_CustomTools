using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.IO;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Reflection;

namespace JR_CustomTools
{
    static class Utilities
    {
        public static List<View> GetAllDraftingViews(Document curDoc)
        {
            //get all drafting views
            FilteredElementCollector m_colViews = new FilteredElementCollector(curDoc);
            m_colViews.OfCategory(BuiltInCategory.OST_Views);
            m_colViews.OfClass(typeof(ViewDrafting));

            List<View> m_views = new List<View>();
            foreach (View x in m_colViews)
            {
                if (x.IsTemplate == false)
                {
                    m_views.Add(x);
                }
            }

            //sort list
            m_views.Sort();
            return m_views;
        }
    }
}