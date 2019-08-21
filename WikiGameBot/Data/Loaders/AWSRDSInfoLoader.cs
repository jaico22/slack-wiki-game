using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Data.Loaders
{
    public class AWSRDSInfoLoader : IDBServerInfoLoader
    {
        public string GetConnectionString()
        {
            var credentialsJSON = GetAWSSecret("wikibot-database-creds");
            SQLServerCredentials credentials = JsonConvert.DeserializeObject<SQLServerCredentials>(credentialsJSON);
            return $"Server={credentials.host},{credentials.port};Initial Catalog=wiki-bot-data;" +
                $"User Id={credentials.username};Password={credentials.password};";
        }

        private string GetAWSSecret(string secretName)
        {
            MemoryStream memoryStream = new MemoryStream();
            AmazonSecretsManagerConfig amazonSecretsManagerConfig = new AmazonSecretsManagerConfig();
            amazonSecretsManagerConfig.RegionEndpoint = RegionEndpoint.USEast2;

            string AccessKeyID = Environment.GetEnvironmentVariable("AWS_ACCESSKEYID");
            string SecretKey = Environment.GetEnvironmentVariable("AWS_SECRETKEY"); 
            string VersionStage = null;

            IAmazonSecretsManager client = new AmazonSecretsManagerClient
                 (AccessKeyID, SecretKey, amazonSecretsManagerConfig);

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = VersionStage == null ? "AWSCURRENT" : VersionStage; // VersionStage defaults to AWSCURRENT if unspecified.
            GetSecretValueResponse response = null;

            try
            {
                response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine("The requested secret " + secretName + " was not found");
            }
            catch (InvalidRequestException e)
            {
                Console.WriteLine("The request was invalid due to: " + e.Message);
            }
            catch (InvalidParameterException e)
            {
                Console.WriteLine("The request had invalid params: " + e.Message);
            }

            return response?.SecretString;
        }
    }
}
