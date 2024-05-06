using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace NewTaskBack
{
    public class AutoCreateTask : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService service =
                    serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    tracingService.Trace("1");
                    if (entity.GetAttributeValue<bool>("crd4a_novatarefa") == true)
                    {

                        Entity newUserTask = new Entity("task");
                        newUserTask.Attributes.Add("subject", "Atualizar informação pessoal");
                        newUserTask.Attributes.Add("description", "Em caso de dúvida contactar XPTO");
                        newUserTask.Attributes.Add("actualdurationminutes", 30);
                        newUserTask.Attributes.Add("regardingobjectid", entity.ToEntityReference());

                        //create the task record
                        Guid taskGuid = service.Create(newUserTask);

                        string script = "alert('A tarefa foi criada');";
                        context.SharedVariables["ScriptToExecute"] = script;
                    }



                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in newusertask.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("CreateTask: {0}", ex.ToString());
                    throw;
                }
            }





        }
    }
}
