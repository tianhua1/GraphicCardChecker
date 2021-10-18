using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using GraphicCardChecker.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;

namespace GraphicCardChecker
{
    public class Handler : IHandler
    {
        private const string CURRIES = "https://www.currys.ie/ieen/computing-accessories/components-upgrades/graphics-cards/324_3091_30343_xx_delivery-hd/xx_xx_xx_xx_5-6-criteria.html";
        private const string CURRIESALL = "https://www.currys.ie/ieen/computing-accessories/components-upgrades/graphics-cards/324_3091_30343_xx_xx/xx-criteria.html?dl_search=graphics%2Bcards";

        private const string BRAND = "<span data-product=\"brand\">";
        private const string NAME = "<span data-product=\"name\">";
        private const string PRICE = "<strong class=\"price\" data-product=\"price\">";
        private const string LESSTHEN = "<";
        private const string ARN = "";
        private const string ACCESSKEY = "";
        private const string SECRETKEY = "";
        public async Task HandleAsync()
        {
            using (WebClient client = new WebClient())
            {
                string downloadedString = client.DownloadString(CURRIES);
                var result = new List<GraphicCard>();
                try
                {
                    while (downloadedString.IndexOf(BRAND) != -1)
                    {
                        (string brand, string downloadedString) brand = GetElement(BRAND, downloadedString);
                        (string name, string downloadedString) name = GetElement(NAME, brand.downloadedString);
                        (string price, string downloadedString) price = GetElement(PRICE, name.downloadedString);
                        downloadedString = price.downloadedString;

                        result.Add(
                            new GraphicCard(name.name, brand.brand, price.price)
                        );
                    }
                }
                catch (Exception ex)
                {
                    LambdaLogger.Log($"Caught exception: {ex.Message}\n");
                }
                

                if (result.Any(x => x.SendAlert))
                {
                    await SendMessage(result.Select(x => x.FormatMessage()).Aggregate("", (current, next) => current + next));
                }
                else
                {
                    LambdaLogger.Log("NO CARD TO ALERT");
                }
            }
        }

        private (string, string) GetElement(string element, string downloadedString)
        {
            var brandIndex = downloadedString.IndexOf(element);
            downloadedString = downloadedString.Remove(0, brandIndex + element.Length);
            var brandLessThenIndex = downloadedString.IndexOf(LESSTHEN);
            var item = downloadedString.Substring(0, brandLessThenIndex).Trim();
            return (item, downloadedString);
        }

        private async Task SendMessage(string message)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(ACCESSKEY, SECRETKEY);
            var client = new AmazonSimpleNotificationServiceClient(awsCredentials, region: Amazon.RegionEndpoint.EUWest1);

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = ARN
            };

            try
            {
                LambdaLogger.Log("Message sending \n");
                var response = await client.PublishAsync(request);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Caught exception publishing request1: {ex.Message}");
            }
        }
    }
}
