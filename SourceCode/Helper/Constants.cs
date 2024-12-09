using System.Collections.Generic;

namespace DataverseUsefulPlugins.Helper.Constants
{
    public static class EntityNames
    {
        public const string webresource = "webresource";
    }

    public static class Attributes
    {
        public static class webresource
        {
            public const string name = "name";
            public const string webresourcetype = "webresourcetype";
            public const string displayname = "displayname";

            public static string GetExtensionByWebResourceType(int webResourceType)
            {
                // Mapping of web resource types to file extensions
                var typeToExtension = new Dictionary<int, string>
                {
                    { 1, "html" },   // Webpage (HTML)
                    { 2, "css" },    // Style Sheet (CSS)
                    { 3, "js" },     // Script (JavaScript)
                    { 4, "xml" },    // Data (XML)
                    { 5, "png" },    // PNG format
                    { 6, "jpg" },    // JPG format
                    { 7, "gif" },    // GIF format
                    { 8, "xap" },    // Silverlight (XAP)
                    { 9, "xsl" },    // Style Sheet (XSL)
                    { 10, "ico" },   // ICO format
                    { 11, "svg" },   // Vector format (SVG)
                    { 12, "resx" }   // String (RESX)
                };

                return typeToExtension.TryGetValue(webResourceType, out var extension) ? extension : null;
            }
        }
    }
}
    





    
