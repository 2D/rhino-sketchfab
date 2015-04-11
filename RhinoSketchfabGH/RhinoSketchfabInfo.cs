using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace RhinoSketchfab
{
    public class RhinoSketchfabInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "RhinoSketchfab";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Export model to SketchFab";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e394e7ab-879e-401c-a06d-8d6289fcef5b");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "RhinoIn";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
