using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using DataverseUsefulPlugins.Helper.Constants;
using static DataverseUsefulPlugins.Helper.Constants.Attributes;

namespace DataverseUsefulPlugins
{
    public class CustomAuditPlugin : PluginBase
    {
        public CustomAuditPlugin(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(CustomAuditPlugin))
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

            var service = localPluginContext.OrganizationService;
            var tracingService = localPluginContext.TracingService;

            // Retrieve configuration for auditing
            var auditConfig = GetAuditConfiguration(service, targetEntity.LogicalName);
            if (auditConfig == null || !auditConfig.AttributesToAudit.Any())
            {
                return; // No configuration for this entity
            }

            // Pre-image contains the current values of the record
            Entity preImage = context.PreEntityImages.Contains("PreImage") ? context.PreEntityImages["PreImage"] : null;

            // Process changes
            foreach (var attribute in targetEntity.Attributes)
            {
                if (auditConfig.AttributesToAudit.Contains(attribute.Key))
                {
                    var newValue = targetEntity[attribute.Key];
                    var oldValue = preImage != null && preImage.Contains(attribute.Key) ? preImage[attribute.Key] : null;

                    if (!AreValuesEqual(oldValue, newValue))
                    {
                        // Log change to the AuditLog table
                        LogAudit(service, targetEntity.LogicalName, targetEntity.Id, attribute.Key, oldValue, newValue, context.UserId, context.MessageName);
                    }
                }
            }            
        }

        private AuditConfiguration GetAuditConfiguration(IOrganizationService service, string entityName)
        {
            // Query the AuditConfiguration table for the entity
            var query = new QueryExpression("customauditconfiguration")
            {
                ColumnSet = new ColumnSet("attributes", "logicalname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("logicalname", ConditionOperator.Equal, entityName)
                    }
                }
            };

            var result = service.RetrieveMultiple(query).Entities.FirstOrDefault();
            if (result != null)
            {
                return new AuditConfiguration
                {
                    EntityName = result.GetAttributeValue<string>("logicalname"),
                    AttributesToAudit = result.GetAttributeValue<string>("attributes")?.Split(',') ?? Array.Empty<string>()
                };
            }
            return null;
        }

        private void LogAudit(IOrganizationService service, string entityName, Guid recordId, string fieldName, object oldValue, object newValue, Guid userId, string operation)
        {
            var auditLog = new Entity("customauditlog")
            {
                ["entityname"] = entityName,
                ["recordid"] = recordId.ToString(),
                ["fieldname"] = fieldName,
                ["oldvalue"] = oldValue?.ToString(),
                ["newvalue"] = newValue?.ToString(),
                ["userid"] = userId,
                ["operation"] = operation,
                ["createdon"] = DateTime.UtcNow
            };

            service.Create(auditLog);
        }

        private bool AreValuesEqual(object oldValue, object newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return true;
            }

            if (oldValue == null || newValue == null)
            {
                return false;
            }

            return oldValue.Equals(newValue);
        }
    }

    public class AuditConfiguration
    {
        public string EntityName { get; set; }
        public string[] AttributesToAudit { get; set; }
    }
}
