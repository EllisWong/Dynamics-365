using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheYu.Plugins
{
    /// <summary>
    /// Keep ModifiedOn Date when a record updated。
    /// This solution can be used 
    /// Registered Message:Update
    /// Target Entity:{YOUR ENTITY}
    /// Filter Attributes:{YOUR ENTITY ATTRIBUTE TRIGGER FIELD}
    /// Event Stage:Update+Pre-Operation
    /// Execution Mode:Sync
    /// </summary>
    public class RecoverModifiedOnPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var facotry = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var orgService = facotry.CreateOrganizationService(context.UserId);
            var target = (Entity)context.InputParameters["Target"];
            if (target == null)
            {
                return;
            }
            try
            {
                var existingEntity = orgService.Retrieve(target.LogicalName, target.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("modifiedon"));
                if (existingEntity.Contains("modifiedon"))
                {
                    target["modifiedon"] = existingEntity.GetAttributeValue<DateTime>("modifiedon");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
