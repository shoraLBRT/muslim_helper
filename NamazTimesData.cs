using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muslim_helper
{
    internal class NamazTimesData
    {
        private static string allNamazes = " ";
        private Dictionary<string, string>? namazTimesDictionary;

        public NamazTimesData(NamazTimesParsing namazTimesParsing)
        {
            if (namazTimesDictionary == null)
                namazTimesDictionary = namazTimesParsing.DoTheParsing().Result;
        }

        public string ShowConcreteNamaz(string namazName)
        {
            if (namazTimesDictionary != null)
                return namazTimesDictionary[namazName];
            else return "времена намаза ещё не получены";
        }

        public string ShowAllNamazes()
        {
            if (namazTimesDictionary != null)
            {
                allNamazes = null;
                foreach (var item in namazTimesDictionary)
                {
                    allNamazes += item.Key + "➖➖" + item.Value + "\n";
                }
            }
            return allNamazes;
        }
        public Dictionary<string, string> ReturnNamazDictionary()
        {
            return namazTimesDictionary;
        }
    }
}
