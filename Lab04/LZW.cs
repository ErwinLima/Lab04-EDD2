using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04
{
    public class LZW
    {
        public static List<int> encode(string text)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }
            string w = string.Empty;
            List<int> resul = new List<int>();

            foreach (var c in text)
            {
                string temp = w + c;
                if (dictionary.ContainsKey(temp))
                {
                    w = temp;
                }
                else
                {
                    resul.Add(dictionary[w]);
                    dictionary.Add(temp, dictionary.Count);
                    w = c.ToString();
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                resul.Add(dictionary[w]);
            }

            return resul;
        }

        public static string Decompress(List<int> compressed)
        {
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = "";
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }
    }
}
