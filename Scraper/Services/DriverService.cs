using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Scraper.Models;
using Newtonsoft.Json;

namespace Scraper.Services
{
    public class DriverService
    {
        Driver _model = new Driver();

        public string GetDriverInfo(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var rows = doc.DocumentNode.SelectNodes(
                "//table[@class='infobox biography vcard']//tr");

            var thInfo = doc.DocumentNode.SelectSingleNode(
                "//table[@class='infobox biography vcard']//tr//th[@class='infobox-header']");

            Dictionary<string, string> driverDic = new Dictionary<string, string>();

            foreach (var item in rows)
            {
                var th = item.SelectSingleNode(".//th");
                var td = item.SelectSingleNode(".//td");

                if (th != null && th.HasClass("infobox-header"))
                {
                    if (th.InnerText != "Formula One World Championship career")
                        break;
                }
                else if (th != null && td != null)
                {
                    driverDic.Add(th.InnerText, td.InnerText);

                    //Console.Write(th.InnerText + " : ");
                    //Console.WriteLine(td.InnerText);
                }
            }
            
            return JsonConvert.SerializeObject(driverDic, Formatting.Indented);
        }

        public void GetAllDrivers()
        {
            var url = "https://en.wikipedia.org/wiki/2022_Formula_One_World_Championship";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var rows = doc.DocumentNode.SelectNodes(
                "//div[@style='overflow-x: auto; margin: 1em 0']//tbody//tr/td/table//tbody//tr" +
                "//td[@style='text-align:left']");

            int i = 0;
            foreach (var item in rows)
            {
                Console.WriteLine(item.InnerText.Trim());
                i++;
                if (i > 20)
                {
                    break;
                }
            }
        }

        public void ScrapDriverInfoToModel(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var rows = doc.DocumentNode.SelectNodes(
                "//table[@class='infobox biography vcard']//tr");

            string name = url.Substring(url.IndexOf("/wiki/")).Replace("/wiki/", "").Replace("_", " ").Trim();


            foreach (var item in rows)
            {
                var td = item.SelectSingleNode(".//td");

                if (td != null)
                {
                    if (td.SelectSingleNode(".//span") != null && td.SelectSingleNode(".//span").HasClass("flagicon"))
                    {
                        _model.Nationality = td.InnerText.Trim();
                    }
                }                
            }

            _model.Name = name;

            Console.WriteLine(JsonConvert.SerializeObject(_model, Formatting.Indented));
        }
    }
}
