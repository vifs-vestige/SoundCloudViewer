using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundCloudViewer
{
    class SoundCloud
    {
        private ChromeDriver Browser;


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

        public void GetInfo()
        {
            MainWindow.SongName = "";
            MainWindow.SongInfo = "";
            if (Browser.Url.ToLower().Contains("soundcloud"))
            {
                var songInfo = Browser.Title;
                if (songInfo.Contains("by"))
                {
                    var temp = songInfo.Split(new[] { "by" }, StringSplitOptions.None);
                    MainWindow.SongName = temp[0].Trim();
                    MainWindow.SongInfo = temp[1].Trim();
                }
            }
        }
    }
}
