using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TheYu.Plugins
{
    /// <summary>
    /// Filtering Data When RetrieveMultiple Message is triggered.
    /// Registered Message:RetrieveMultiple
    /// Target Entity:{YOUR ENTITY}
    /// Filter Attributes:{YOUR ENTITY ATTRIBUTE TRIGGER FIELD}
    /// Event Stage:Update+Pre-Operation
    /// Execution Mode:Sync
    /// </summary>
    public class RetrieveMultipleFilterPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var facotry = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var orgService = facotry.CreateOrganizationService(context.UserId);

   
            try
            {
                //Below method is worked for UCI 9.0+ ,
                if (context.InputParameters.Contains("Query") && 
                    context.InputParameters["Query"] is FetchExpression)
                {
                    var fetchExpression = (FetchExpression)context.InputParameters["Query"];

                    XDocument fetchXmlDoc = XDocument.Parse(fetchExpression.Query);

                    var entityElement = fetchXmlDoc.Descendants("entity").FirstOrDefault();

                    //Create a new custom filter element for your query.
                    var customFilterElement = new XElement("filter", new XAttribute("type", "and"));
                    //Here is demo,I will put a custom filter : statecode is active 
                    string conditionXml = $"<condition attribute='statecode' operator='eq' value='0'>";
                    var newFilterConditionXElement = XElement.Parse(conditionXml);
                    customFilterElement.Add(newFilterConditionXElement);

                    //After that,repale the Input Parameter Query with your new fetchExpression.
                    fetchExpression = new FetchExpression(fetchXmlDoc.ToString());
                    context.InputParameters["Query"] = fetchExpression;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
