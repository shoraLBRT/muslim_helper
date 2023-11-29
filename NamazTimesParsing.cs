namespace muslim_helper
{
    internal class NamazTimesParsing
    {
        static List<string> NamazNamesList;
        internal static string allNamazes = " ";

        private static Dictionary<string, string>? namazTimesDictionary;

        private static Dictionary<string, string>? NamazTimesDictionary { get => namazTimesDictionary; set => namazTimesDictionary = value; }

        public Dictionary<string, string> ReturnNamazDictionary()
        {
            return NamazTimesDictionary;
        }
        public async Task<string> DoTheParsing()
        {
            if (NamazNamesList != null)
                NamazNamesList.Clear();
            else NamazNamesList = FormNamazNames();

            if (NamazTimesDictionary != null)
                NamazTimesDictionary.Clear();

            NamazTimesDictionary = await Parsing(url: "https://time-in.ru/prayer/cherkessk?method=3&adjust=3&asr=0");

            if (NamazTimesDictionary == null)
                return "Парсинг не удался";

            return "Успешно спарсено";
        }
        public string ShowConcreteNamaz(string namazName)
        {
            if (NamazTimesDictionary != null)
                return NamazTimesDictionary[namazName];
            else return "времена намаза ещё не получены";
        }

        public string ShowAllNamazes()
        {
            if (NamazTimesDictionary != null)
            {
                allNamazes = null;
                foreach (var item in NamazTimesDictionary)
                {
                    allNamazes += item.Key + "➖➖" + item.Value + "\n";
                }
            }
            return allNamazes;
        }
        private static List<string> FormNamazNames()
        {
            List<string> namazNames = new()
            {
                "Фаджр",
                "Восход",
                "Зухр",
                "Аср",
                "Магриб",
                "Иша"
            };
            return namazNames;
        }
        private static async Task<Dictionary<string, string>> Parsing(string url)
        {
            try
            {
                Dictionary<string, string> result = new();
                using (HttpClientHandler htch = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None
                })
                {
                    using (var client = new HttpClient(htch))
                    {
                        using (HttpResponseMessage response = client.GetAsync(url).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var html = response.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument doc = new();
                                    doc.LoadHtml(html);
                                    var namazTimes = doc.DocumentNode.SelectNodes("//article[@class='prayer-city']//td[contains(text(),'Сегодня')]/following-sibling::td");
                                    if (namazTimes != null && namazTimes.Count > 0)
                                    {
                                        for (int i = 0; i < namazTimes.Count; i++)
                                        {
                                            string strNamazName = NamazNamesList[i];
                                            string strNamazTime = namazTimes[i].InnerText;
                                            if (strNamazName != null && strNamazTime != null)
                                            {
                                                result.Add(strNamazName, strNamazTime);
                                            }
                                            else Console.WriteLine("проблема с названиями/временем намазов");
                                        }
                                        return result;
                                    }
                                    else
                                    {
                                        Console.WriteLine("no Tables");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("проблема с подключением к серверу получения времени намазов" + ex);
            }
            return null;
        }
    }
}
