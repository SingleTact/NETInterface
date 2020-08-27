//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace SingleTact_Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppCenter.Start("a653d802-28db-4d01-b426-17dabe236a61",
                   typeof(Analytics), typeof(Crashes));
            try
            {
                Application.Run(new GUI());
            }
            catch (System.IO.FileNotFoundException ex)
            {
                // Avoids silently failing when a sub-assembly is missing.

                // Convert assembly details to a readable DLL name.
                string details = ex.FileName.ToLower();
                string missingName = null;

                if (details.Contains("zedgraph"))
                    missingName = "ZedGraph.dll";
                else if (details.Contains("singletactlibrary"))
                    missingName = "SingleTactLibrary.dll";

                if (missingName != null)
                {
                    MessageBox.Show(
                        "Please ensure that " +
                        missingName +
                        " is in the same location as this program.",
                        Application.ProductName);
                }
            }
        }
    }
}
