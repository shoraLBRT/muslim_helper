using System.Text;

namespace muslim_helper
{
    internal class AyatParsingHandler
    {
        private static Dictionary<int, string>? kulievAyatDictionary;

        string path = @"C:\Program Files (x86)\SHORAfiles\Project Files\muslim_helper\fullquran.html";

        public AyatParsingHandler()
        {
            if(kulievAyatDictionary == null)
                FormAyatDictionary();
        }
        private async Task<Dictionary<int, string>> AyatParsing()
        {
            Dictionary<int, string> result = new();
            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fileStream.Length];
                if (buffer != null)
                    Console.WriteLine("buffer is recorded");

                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                if (textFromFile != null)
                    Console.WriteLine("textFromFile is recorded");

                HtmlAgilityPack.HtmlDocument quranhtml = new();
                quranhtml.LoadHtml(textFromFile);
                var kulievAyatsNodes = quranhtml.DocumentNode.SelectNodes("//div[@class='container']//p[@class='ayat translate-3']/text()");

                if (kulievAyatsNodes != null && kulievAyatsNodes.Count > 0)
                {
                    for (int i = 0; i < kulievAyatsNodes.Count; i++)
                    {
                        int kulievAyatsNumber = i;
                        string kulievAyat = kulievAyatsNodes[i].InnerText;
                        result.Add(kulievAyatsNumber, kulievAyat);
                    }
                    Console.WriteLine("succesful");
                }
                else Console.WriteLine("nodes are null or empty");
                return result;
            }
        }
        public async Task FormAyatDictionary()
        {
            kulievAyatDictionary?.Clear();
            kulievAyatDictionary = await AyatParsing();
            return;
        }
        public async Task<string> GetAyatFromNumber(int numberOfAyat)
        {
            if (kulievAyatDictionary == null)
                await FormAyatDictionary();
            if (kulievAyatDictionary[numberOfAyat] == null)
                return "По этому индексу аят отсутствует";

            string resultAyat = kulievAyatDictionary[numberOfAyat];
            resultAyat.Trim();
            while (resultAyat.Length <= 70)
            {
                numberOfAyat -= 1;
                resultAyat = kulievAyatDictionary[numberOfAyat] + "☪" + resultAyat;
            }
            while ((resultAyat[resultAyat.Length - 1] != '.')
                && (resultAyat[resultAyat.Length - 1] != '!')
                && (resultAyat[resultAyat.Length - 1] != '?')
                && (resultAyat[resultAyat.Length - 2] != '.')
                && (resultAyat[resultAyat.Length - 2] != '!')
                && (resultAyat[resultAyat.Length - 2] != '?'))
            {
                numberOfAyat += 1;
                resultAyat = resultAyat + "☪" + kulievAyatDictionary[numberOfAyat];
            }
            return resultAyat;
        }
    }
}
