using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Interop;
using FINN.CAD.Exceptions;

namespace FINN.CAD
{
    public static class CADHelper
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
                    acAppComObj.Visible = true;
                }
                catch (Exception ex)
                {
                    // If an instance of AutoCAD is not created then message and exit
                    MessageBox.Show($"Instance of 'AutoCAD.Application' could not be created: {ex.Message}");

                    throw new InstanceNotCreatedException();
                }
            }

            return acAppComObj;
        }

        public static void LoadInProcessAssembly(string path)
        {
            var acAppComObj = GetInstance();
            // Optionally, load your assembly and start your command or if your assembly
            // is demand loaded, simply start the command of your in-process assembly.
            var acDocComObj = acAppComObj.ActiveDocument;

            try
            {
                acDocComObj.SendCommand($"(command \"NETLOAD\" \"{path}\") \r");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Command error: {ex.Message}");
            }
        }

        public static string GetVersion()
        {
            var instance = GetInstance();
            MessageBox.Show("Now running " + instance.Name + " version " + instance.Version);
            return instance.Version;
        }

        public static void Execute(string command, bool inNew = false)
        {
            var acAppComObj = GetInstance();

            AcadDocument acDocComObj = inNew ? acAppComObj.Documents.Add() : acAppComObj.ActiveDocument;

            acDocComObj.SendCommand($"{command} \r");
        }
    }
}