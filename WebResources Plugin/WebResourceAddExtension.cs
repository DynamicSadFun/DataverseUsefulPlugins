using Microsoft.Xrm.Sdk;
using System;
using EMA.CE.InternalUsage.Helper.Constants;
using static EMA.CE.InternalUsage.Helper.Constants.Attributes;

namespace EMA.CE.InternalUsage
{
    public class WebResourceAddExtension : PluginBase
    {
        public WebResourceAddExtension(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(WebResourceAddExtension))
        {
        }
        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            if (localPluginContext == null)
            {
                throw new ArgumentNullException(nameof(localPluginContext));
            }

            var context = localPluginContext.PluginExecutionContext;

            // Validate that the Target parameter exists and is an Entity
            if (!(context.InputParameters.TryGetValue(Target, out var target) && target is Entity targetEntity))
            {
                return; // Exit if no valid target entity
            }

            // Ensure the entity is a WebResource
            if (!targetEntity.LogicalName.Equals(EntityNames.webresource, StringComparison.OrdinalIgnoreCase))
            {
                return; // Exit if the target is not a web resource
            }

            // Retrieve logical name and web resource type
            var logicalName = targetEntity.GetAttributeValue<string>(webresource.name);
            var webResourceTypeValue = targetEntity.GetAttributeValue<OptionSetValue>(webresource.webresourcetype)?.Value;

            // Validate logical name and web resource type
            if (string.IsNullOrWhiteSpace(logicalName))
            {
                throw new InvalidPluginExecutionException("The logical name of the web resource is not provided.");
            }

            if (!webResourceTypeValue.HasValue)
            {
                throw new InvalidPluginExecutionException("The web resource type is not provided.");
            }

            // Determine the file extension
            string extension = webresource.GetExtensionByWebResourceType(webResourceTypeValue.Value);
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidPluginExecutionException("Unable to determine the file extension for the provided web resource type.");
            }

            // Check if the extension is already part of the logical name
            if (!logicalName.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
            {
                // Update the logical name and display name with the determined extension
                string updatedName = $"{logicalName}.{extension}";
                targetEntity[webresource.name] = updatedName;
            }
        }
    }
}
