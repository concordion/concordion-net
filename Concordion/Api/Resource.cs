using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Util;
using Concordion.Internal;
using System.IO;

namespace Concordion.Api
{
    public class Resource
    {
        #region Fields

        private static readonly char PATH_SEPARATOR = '\\';
        private static readonly string RELATIVE_PATH_INDICATOR = ".." + PATH_SEPARATOR;
        
        #endregion

        #region Properties

        public string Name
        {
            get;
            private set;
        }

        public string UriPath
        {
            get
            {
                return new Uri(Uri.UriSchemeFile + ":///" + Path, UriKind.Absolute).AbsoluteUri;
            }
        }

        public virtual string Path
        {
            get;
            set;
        }

        private string[] Parts
        {
            get;
            set;
        }

        public Resource Parent
        {
            get
            {
                if (Path.Equals(System.IO.Path.GetPathRoot(Path)))
                {
                    return null;
                }

                StringBuilder parentPath = new StringBuilder("\\");
                for (int i = 0; i < Parts.Length - 1; i++)
                {
                    parentPath.Append(Parts[i] + PATH_SEPARATOR);
                }
                return new Resource(parentPath.ToString());
            }
        }

        public bool IsPackage
        {
            get;
            private set;
        }

        public Resource Package
        {
            get
            {
                if (IsPackage)
                {
                    return this;
                }
                return Parent;
            }
        }

        #endregion

        #region Constructors
        
        public Resource(string path)
        {
            Path = path.Replace('/', PATH_SEPARATOR);

            if (System.IO.Path.IsPathRooted(path))
            {
                Path = Path.Replace(System.IO.Path.GetPathRoot(path), @"\");
            }

            if (Path.EndsWith(PATH_SEPARATOR.ToString()))
            {
                IsPackage = true;
            }

            Parts = Path.Split(new string[] { PATH_SEPARATOR.ToString() }, StringSplitOptions.RemoveEmptyEntries);
            if (Parts.Length == 0)
            {
                Name = "";
            }
            else
            {
                Name = Parts[Parts.Length - 1];
                //Name = Name.Replace(".html", String.Empty);
            }
        } 

        #endregion

        #region Methods
        
        public Resource GetRelativeResource(string relativePath)
        {
            //Check.IsFalse(relativePath.StartsWith(PATH_SEPARATOR.ToString()), "Relative path should not start with a slash");

            string subPath = relativePath;

            Resource p = Package;

            while (subPath.StartsWith(RELATIVE_PATH_INDICATOR))
            {
                p = p.Parent;
                if (p == null)
                {
                    throw new Exception("Path '" + relativePath + "' relative to '" + Path + "' " +
                    "evaluates above the root package.");
                }
                subPath = subPath.RemoveFirst(RELATIVE_PATH_INDICATOR);
            }

            Check.IsFalse(subPath.Contains(RELATIVE_PATH_INDICATOR), String.Format("The {0} operator is currently only supported at the start of expressions", RELATIVE_PATH_INDICATOR));

            return new Resource(p.Path + subPath);
        } 

        public string GetRelativePath(Resource resource)
        {
            if (resource.Path == Path)
            {
                return Name;
            }

            // Find common stem and ignore it
            // Use ../ to move up the path from here to common stem
            // Append the rest of the path from resource

            string[] therePieces = resource.Package.Path.Split(new string[] {PATH_SEPARATOR.ToString()}, StringSplitOptions.RemoveEmptyEntries);
            string[] herePieces = Package.Path.Split(new string[] {PATH_SEPARATOR.ToString()}, StringSplitOptions.RemoveEmptyEntries);

            int sharedPiecesCount = 0;
            for (int i = 0; i < herePieces.Length; i++)
            {
                if (therePieces.Length <= i)
                {
                    break;
                }
                if (therePieces[i].Equals(herePieces[i]))
                {
                    sharedPiecesCount++;
                }
                else
                {
                    break;
                }
            }

            StringBuilder r = new StringBuilder();

            for (int i = sharedPiecesCount; i < herePieces.Length; i++)
            {
                r.Append(RELATIVE_PATH_INDICATOR);
            }

            for (int i = sharedPiecesCount; i < therePieces.Length; i++)
            {
                r.Append(therePieces[i]);
                r.Append(PATH_SEPARATOR);
            }

            if (resource.IsPackage)
            {
                return r.ToString();
            }
            return r.ToString() + resource.Name;
        }

        #endregion

    }
}
