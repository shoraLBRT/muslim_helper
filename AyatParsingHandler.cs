using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace muslim_helper
{
    internal class AyatParsingHandler
    {
        private Dictionary<int, string>? kulievAyatDictionary;

        string path = @"C:\Users\Альберт\Desktop\quran\fullquran.html";
        private async Task<Dictionary<int, string>> AyatParsing()
        {
            Dictionary<int, string> result = new();
            using (FileStream fileStream = File.OpenRead(path))
            {
                Console.WriteLine("filestrem is opened the File");
                byte[] buffer = new byte[fileStream.Length];
                if (buffer != null)
                {     
                    Console.WriteLine("buffer is recorded");
                }
                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                if (textFromFile != null)
                {
                    Console.WriteLine("textFromFile is recorded");
                }
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
            kulievAyatDictionary = await AyatParsing();
            return;
        }
        public async Task<string> GetAyatFromNumber(int numberOfAyat)
        {
            if (kulievAyatDictionary == null)
            {
                await FormAyatDictionary();
            }
            if (kulievAyatDictionary[numberOfAyat] != null)
            {
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
            return "По этому индексу аят отсутствует";
        }
    }
}
