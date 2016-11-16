using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoundCloudViewer
{
    class SoundCloud
    {
        private ChromeDriver Browser;
        private String CurrentInfo = "";
        private String CurrentFullInfo = "";


        public SoundCloud()
        {
            Browser = new ChromeDriver(@"..\..\Selenium");
            Browser.Navigate().GoToUrl("https://soundcloud.com/");
            
        }

        private IWebElement FindElement(String xpath)
        {
            IWebElement temp = null;
            try
            {
                temp = Browser.FindElement(By.XPath(xpath));
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return temp;

        }

        private string GetInfo(string href)
        {
            string info = "";
            var temp = href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var tempInfo = temp[2];
            if (tempInfo != CurrentInfo)
            {
                CurrentInfo = temp[2];
                var url = "http://SoundCloud.com/" + temp[2];
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                if (request != null)
                {
                    //request.UseDefaultCredentials = true;
                    HttpWebResponse response = null;
                    try
                    {
                        response = request.GetResponse() as HttpWebResponse;
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine("");
                    }
                    string regex = @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>";
                    if (new List<string>(response.Headers.AllKeys).Contains("Content-Type"))
                        if (response.Headers["Content-Type"].StartsWith("text/html"))
                        {
                            //WebClient web = new WebClient();
                            //web.UseDefaultCredentials = true;
                            //string page = web.DownloadString(url);
                            string page = "";
                            var encoding = ASCIIEncoding.ASCII;
                            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                            {
                                page = reader.ReadToEnd();
                            }

                            // Extract the title
                            Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                            var temp2 = ex.Match(page).Value.Trim();
                            var temp3 = temp2.Split(new[] { "<title>", " | Free Listening on SoundCloud</title>" }, StringSplitOptions.RemoveEmptyEntries);
                            info = temp3[0];
                            CurrentFullInfo = info;
                        }
                }
            }
            else
            {
                info = CurrentFullInfo;
            }

            return info;
        }

        public void GetInfo()
        {
            MainWindow.SongName = "";
            MainWindow.SongInfo = "";
            IWebElement infoElement = FindElement("//*[@id=\"app\"]/div[4]/div/div/div[1]/div/div[1]/a[2]");
            if (infoElement != null)
            {
                var href = infoElement.GetAttribute("href");
                var info = GetInfo(href);
                if(info != "")
                {
                    MainWindow.SongName = infoElement.GetAttribute("title");
                    MainWindow.SongInfo = info;
                }
                Console.WriteLine("");
            }
            //if (Browser.Url.ToLower().Contains("soundcloud"))
            //{
            //    var songInfo = Browser.Title;
            //    if (songInfo.Contains("by"))
            //    {
            //        var temp = songInfo.Split(new[] { "by" }, StringSplitOptions.None);
            //        MainWindow.SongName = temp[0].Trim();
            //        MainWindow.SongInfo = temp[1].Trim();
            //    }
            //}
        }


    }
}
