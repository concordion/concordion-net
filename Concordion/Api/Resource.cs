using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Util;
using System.IO;

namespace Concordion.Api
{
    public class Resource
    {
        #region Fields

        private static readonly char pathSeparator = '\\';
        
        #endregion

        #region Properties

        public string Name
        {
            get;
            private set;
        }
        
        public Uri ResourceUri
        {
            get;
            private set;
        }

        public string Path
        {
            get
            {
                return ResourceUri.LocalPath;
            }
        }

        private string[] Parts
        {
            get
            {
                return ResourceUri.Segments;
            }
        }

        public Resource Parent
        {
            get
            {
                if (Path.Equals("/"))
                {
                    return null;
                }

                StringBuilder parentPath = new StringBuilder("/");
                for (int i = 1; i < Parts.Length - 1; i++)
                {
                    parentPath.Append(Parts[i] + "/");
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
            ResourceUri = new Uri(Uri.UriSchemeFile + ":///" + path, UriKind.Absolute);

            if (ResourceUri.LocalPath.EndsWith(@"\"))
            {
                IsPackage = true;
            }

            var parts = path.Split('/');
            if (parts.Length == 0)
            {
                Name = "";
            }
            else
            {
                Name = parts[parts.Length - 1];
            }
        } 

        #endregion

        #region Methods
        
        public Resource GetRelativeResource(string relativePath)
        {
            Check.IsFalse(relativePath.StartsWith("/"), "Relative path should not start with a slash");

            String subPath = relativePath;

            Resource p = Package;

            while (subPath.StartsWith("../"))
            {
                p = p.Parent;
                if (p == null)
                {
                    throw new Exception("Path '" + relativePath + "' relative to '" + Path + "' " +
                    "evaluates above the root package.");
                }
                subPath = subPath.Replace("../", "");
            }

            Check.IsFalse(subPath.Contains("../"), "The ../ operator is currently only supported at the start of expressions");

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

            string[] therePieces = resource.Package.Path.Split('/');
            string[] herePieces = Package.Path.Split('/');

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

            string r = "";

            for (int i = sharedPiecesCount; i < herePieces.Length; i++)
            {
                r += "../";
            }

            for (int i = sharedPiecesCount; i < therePieces.Length; i++)
            {
                r += therePieces[i] + "/";
            }

            if (resource.IsPackage)
            {
                return r;
            }
            return r + resource.Name;
        }

        #endregion

    }
}
