using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using ScraperFramework.Shared.Pocos;

namespace WebScraper.Extractors
{
    internal static class BingExtractor
    {
        private const string ExtractTextAdsScript = @"
            (function() {
                const sem_ads = document.evaluate(""//*[contains(@class, 'sb_adTA')]"",
                    document.body, null, XPathResult.ANY_TYPE, null);
                            
                const ads = [];
                let ad = sem_ads.iterateNext();
                while (ad != null) {
                    const title = document.evaluate(""./h2/a"", ad, null, 
                        XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerText;
                    const url = document.evaluate("".//div[contains(@class, 'b_adurl')]/cite"",
                        ad, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerText;
                                
                    ads.push({
                        Title: title, Url: url
                    });
                                
                    ad = sem_ads.iterateNext();
                }
                            
                return ads;
            })(); 
        ";

        public static async Task<IEnumerable<AdResult>> ExtractTextAds(ChromiumWebBrowser browser)
        {
            JavascriptResponse res = await browser.EvaluateScriptAsync(ExtractTextAdsScript);

            List<AdResult> ads = new List<AdResult>();
            if (res.Result != null)
            {
                // https://stackoverflow.com/questions/32312207/expandoobject-object-and-getproperty
                // It helps to think of an ExpandoObject as a dictionary mapping strings to objects. 
                // When you treat an ExpandoObject as a dynamic variable 
                // any invocation of a property gets routed to that dictionary.
                foreach (dynamic ad in (List<object>)res.Result)
                {
                    ads.Add(new AdResult
                    {
                        Title = ad.Title,
                        Url = ad.Url
                    });
                }
            }

            return ads;
        }
    }
}
