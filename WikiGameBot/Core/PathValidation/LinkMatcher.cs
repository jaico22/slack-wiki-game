using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core.PathValidation
{
    public class LinkMatcher
    {
        public List<WikiLink> wikiLinks { get; set; }

        public WikiLink FindLink(string linkTitle)
        {
            WikiLink BestLink = null; 
            foreach(var link in wikiLinks)
            {
                // Check for exact matches
                var exactLinkTextMatch = string.Compare(link.LinkText?.ToLower(), linkTitle?.ToLower()) == 0;
                var exactPageTitleMatch = string.Compare(link.PageTitle?.ToLower(), linkTitle?.ToLower()) == 0;
                if (exactLinkTextMatch || exactPageTitleMatch)
                    return link;

                // Attempt to identify typos. Set to a threshold of 10% error in the typing as defined by the Levenshtein
                // distance (number of corrections needed to get the strings to match) 
                var maxLevenshteinDistance = (int)Math.Ceiling((decimal)linkTitle.Length * 0.10m);
                var similarLinkTitleMatch = string.IsNullOrEmpty(link.LinkText) == false ?
                     LevenshteinDistance(link.LinkText, linkTitle) <= maxLevenshteinDistance : false;
                var similarPageTitleMatch = string.IsNullOrEmpty(link.PageTitle) == false? 
                    LevenshteinDistance(link.PageTitle, linkTitle) <= maxLevenshteinDistance : false;

                // Don't immediately return a similar match, there might be an exact match somwhere else
                if (similarLinkTitleMatch || similarPageTitleMatch)
                    BestLink = link;
            }

            return BestLink;
        }

        public static int LevenshteinDistance(string s, string t)
        {
            // Clean
            s = s.ToLower().Trim();
            t = t.ToLower().Trim();

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[n, m];
        }
    }
}
