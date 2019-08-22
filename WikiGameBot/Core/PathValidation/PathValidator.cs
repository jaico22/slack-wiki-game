using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiGameBot.Data.Entities;

namespace WikiGameBot.Core.PathValidation
{
    public class PathValidator
    {

        /// <summary>
        /// Validates that <paramref name="path"/> is a valid path from the start and endpoints in <paramref name="gameData"/>
        /// </summary>
        /// <param name="gameData"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ValidationData> Validate(Game gameData, List<string> path)
        {
            ValidationData validationData = new ValidationData();
            var linkExtractor = new LinkExtractor();
            var linkMatcher = new LinkMatcher();
            var links = await linkExtractor.GeneratePageLinks(gameData.StartingUrl);
            linkMatcher.wikiLinks = links;
            bool isFirstLink = true;
            if (path!=null && path.Count > 0)
            {
                string prevLink = path[0];
                foreach (var link in path)
                {
                    if (isFirstLink)
                    {
                        isFirstLink = false;
                    }
                    else
                    {
                        var match = linkMatcher.FindLink(link);
                        if (match != null)
                        {
                            if (RemoveHttpFromUrl(match.Url) == RemoveHttpFromUrl(gameData.EndingUrl))
                            {
                                validationData.IsValid = true;
                                validationData.PathLength = path.Count - 1;
                                return validationData;
                            }
                            linkMatcher.wikiLinks = await linkExtractor.GeneratePageLinks(match.Url);
                            prevLink = link;
                        }
                        else
                        {
                            validationData.IsValid = false;
                            validationData.ValidationMessage = $"I couldn't find a reference to \"{link}\" on the \"{prevLink}\" page :thinking_face: \nThis could be caused by ambiguous links on the page. Try changing {prevLink} to the article title instead to clear up any ambiguity. ";
                            return validationData;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Removes instaces of either "https://" or "http://" from <paramref name="url"/>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string RemoveHttpFromUrl(string url)
        {
            var urlOut = url.Replace("https://", "");
            urlOut = urlOut.Replace("http://", "");
            return urlOut;
        }
    }
}
