﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugins
{
    public class NIFPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the IOrganizationService instance which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            
            // The InputParameters collection contains all the data passed in the message request.  
                if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
                {
                    // Obtain the target entity from the input parameters.
                    Entity entity = (Entity)context.InputParameters["Target"];
                try
                {
                    if (!entity.Attributes.Contains("new_nif"))
                    {
                        throw new InvalidPluginExecutionException("O campo NIF é obrigatório.");
                    }
                    else
                    {
                        if (entity.GetAttributeValue<string>("new_nif") == string.Empty)
                        { 
                            throw new InvalidPluginExecutionException("O campo NIF é obrigatório.");
                        }
                    }
                    var attributeValue = entity.GetAttributeValue<string>("new_nif");
                    if (attributeValue == string.Empty)
                    {
                        tracingService.Trace("NIF: " + attributeValue);
                        throw new InvalidPluginExecutionException("O campo NIF é obrigatório.");

                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FillAField in the Plugins Solution.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FillAField: {0}", ex.ToString());
                    throw;
                }
                
                }
                
        }
    }
}
