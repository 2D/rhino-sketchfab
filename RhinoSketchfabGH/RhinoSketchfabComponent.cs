using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;
using System.Net;

namespace RhinoSketchfab
{
    public class RhinoSketchfabComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public RhinoSketchfabComponent()
            : base("RhinoSketchfab", "RSketch",
                "Export model to SketchFab",
                "RhinoIn", "Utils")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Sketchfab account token", GH_ParamAccess.item);
            pManager.AddTextParameter("Model Path", "MP", "Path to model file", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Export Model", "xM", "set true to export file to Sketchfab", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Info", "I", "Information message", GH_ParamAccess.item);
            pManager.AddTextParameter("Output", "O", "Output information", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = null;
            string modelPath = null;
            bool startExport = false;

            string info = "Upload result";
            string output = null;

            if (!DA.GetData(0, ref token))
            {
                output = "Invalid Sketchfab account token";
                goto l99;
            }

            if (!DA.GetData(1, ref modelPath) || !File.Exists(modelPath))
            {
                output = "Invalid model file path";
                goto l99;
            }

            if (!DA.GetData(2, ref startExport) || startExport == false)
            {
                return;
            }

            SketchFab.SketchFab modelUploader = new SketchFab.SketchFab(token);

            string fileName = Path.GetFileName(modelPath);
            string fileExtention = Path.GetExtension(modelPath);

            FileStream fs = new FileStream(modelPath, FileMode.Open, FileAccess.Read);
            byte[] modelFile = new byte[fs.Length];
            fs.Read(modelFile, 0, modelFile.Length);
            fs.Close();

            //File.Delete(modelPath);

            string uploadResult;
            string errorStack = null;

            try
            {
                HttpWebResponse uploadResponse = modelUploader.UploadModel(modelFile, fileName, fileExtention);
                uploadResult = modelUploader.UploadResult(uploadResponse);
            }
            catch (Exception e)
            {
                uploadResult = e.Message;
                errorStack = e.StackTrace;
            }

            output = uploadResult;

            if (errorStack != null)
            {
                output += "\n" + errorStack;
            }

            l99:
            DA.SetData(0, info);
            DA.SetData(1, output);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{2a25577f-f8e1-4da5-8b5f-f93de183fbde}"); }
        }
    }
}
