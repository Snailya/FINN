using System;
using System.Windows;
using System.Runtime.InteropServices;

using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;

namespace FINN.CAD
{
    public class CADHelper
	{
        private static AcadApplication GetInstance()
        {
            AcadApplication acAppComObj = null;
            const string strProgId = "AutoCAD.Application";

            // Get a running instance of AutoCAD
            try
            {
                acAppComObj = (AcadApplication)Marshal.GetActiveObject(strProgId);
            }
            catch // An error occurs if no instance is running
            {
                try
                {
                    // Create a new instance of AutoCAD
                    acAppComObj = (AcadApplication)Activator.CreateInstance(Type.GetTypeFromProgID(strProgId), true);
                }
                catch
                {
                    // If an instance of AutoCAD is not created then message and exit
                    MessageBox.Show("Instance of 'AutoCAD.Application'" +
                                                         " could not be created.");

                    throw new InstanceNotCreatedException();
                }
            }

            return acAppComObj;
        }

        public static void LoadInProcessAssembly(string path)
        {
            var acAppComObj = GetInstance();
            // Optionally, load your assembly and start your command or if your assembly
            // is demandloaded, simply start the command of your in-process assembly.
            var acDocComObj = acAppComObj.ActiveDocument;
            
            acDocComObj.SendCommand($"(command \"NETLOAD\" \"{path}\")");
        }

        public static string GetVersion()
        {
            var intance = CADHelper.GetInstance();
            MessageBox.Show("Now running " + intance.Name +
                                     " version " + intance.Version);
            return intance.Version;
        }

        public static void Execute()
        {
            var acAppComObj = CADHelper.GetInstance();
            var acDocComObj = acAppComObj.ActiveDocument;

            acDocComObj.SendCommand("MyCommand ");
        }
	}
}
