using System;
using System.IO;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureCS.API.Lib.TextTranslation
{

    /// <summary>
    /// This is a helper class for deploying an Azure Resource Manager template
    /// More info about template deployments can be found here https://go.microsoft.com/fwLink/?LinkID=733371
    /// </summary>
    internal class DeploymentHelper
    {
        string subscriptionId = "your-subscription-id";
        string clientId = "your-service-principal-clientId";
        string clientSecret = "your-service-principal-client-secret";
        string resourceGroupName = "resource-group-name";
        string deploymentName = "deployment-name";
        string resourceGroupLocation = "resource-group-location"; // must be specified for creating a new resource group
        string pathToTemplateFile = "path-to-template.json-on-disk";
        string pathToParameterFile = "path-to-parameters.json-on-disk";
        string tenantId = "95d31a25-0b4b-46f9-921c-e8213ea333af";

        public async void Run()
        {
            // Try to obtain the service credentials
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, clientSecret);

            // Read the template and parameter file contents
            JObject templateFileContents = GetJsonFileContents(pathToTemplateFile);
            JObject parameterFileContents = GetJsonFileContents(pathToParameterFile);

            // Create the resource manager client
            var resourceManagementClient = new ResourceManagementClient(serviceCreds);
            resourceManagementClient.SubscriptionId = subscriptionId;

            // Create or check that resource group exists
            EnsureResourceGroupExists(resourceManagementClient, resourceGroupName, resourceGroupLocation);

            // Start a deployment
            DeployTemplate(resourceManagementClient, resourceGroupName, deploymentName, templateFileContents, parameterFileContents);
        }

        /// <summary>
        /// Reads a JSON file from the specified path
        /// </summary>
        /// <param name="pathToJson">The full path to the JSON file</param>
        /// <returns>The JSON file contents</returns>
        private JObject GetJsonFileContents(string pathToJson)
        {
            JObject templatefileContent = new JObject();
            using (StreamReader file = File.OpenText(pathToJson))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    templatefileContent = (JObject)JToken.ReadFrom(reader);
                    return templatefileContent;
                }
            }
        }

        /// <summary>
        /// Ensures that a resource group with the specified name exists. If it does not, will attempt to create one.
        /// </summary>
        /// <param name="resourceManagementClient">The resource manager client.</param>
        /// <param name="resourceGroupName">The name of the resource group.</param>
        /// <param name="resourceGroupLocation">The resource group location. Required when creating a new resource group.</param>
        private static void EnsureResourceGroupExists(ResourceManagementClient resourceManagementClient, string resourceGroupName, string resourceGroupLocation)
        {
            if (resourceManagementClient.ResourceGroups.CheckExistence(resourceGroupName) != true)
            {
                Console.WriteLine($"Creating resource group '{resourceGroupName}' in location '{resourceGroupLocation}'");
                var resourceGroup = new ResourceGroup();
                resourceGroup.Location = resourceGroupLocation;
                resourceManagementClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);
            }
            else
            {
                Console.WriteLine($"Using existing resource group '{resourceGroupName}'");
            }
        }

        /// <summary>
        /// Starts a template deployment.
        /// </summary>
        /// <param name="resourceManagementClient">The resource manager client.</param>
        /// <param name="resourceGroupName">The name of the resource group.</param>
        /// <param name="deploymentName">The name of the deployment.</param>
        /// <param name="templateFileContents">The template file contents.</param>
        /// <param name="parameterFileContents">The parameter file contents.</param>
        private static void DeployTemplate(ResourceManagementClient resourceManagementClient, string resourceGroupName, string deploymentName, JObject templateFileContents, JObject parameterFileContents)
        {
            Console.WriteLine($"Starting template deployment '{deploymentName}' in resource group '{resourceGroupName}'");
            var deployment = new Deployment();

            deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = templateFileContents,
                Parameters = parameterFileContents["parameters"].ToObject<JObject>()
            };

            var deploymentResult = resourceManagementClient.Deployments.CreateOrUpdate(resourceGroupName, deploymentName, deployment);
            Console.WriteLine($"Deployment status: {deploymentResult.Properties.ProvisioningState}");
        }
    }

}