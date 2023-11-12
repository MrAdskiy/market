using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace market
{
    public class Statistics
    {
        public List<Product> GetFromFile()
        {
            var path = @".\allitems.txt";
            var list = new List<Product>();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var value = line.Split('\t');
                    var product = new Product
                    {
                        Category = value[0],
                        Description = value[1],
                        Vendor = value[2]
                    };
                    list.Add(product);
                }
            }
            return list;
        }
        
        private List<string> GetProdByCat(List<Product> listProd,string category)
        {
            var list = listProd.Where(cat => cat.Category == category).
                Select(el => el.Description).ToList();
            return list;
        }

        private bool isWord(string word)
        {
            if (word.Length < 3)
                return false;
            return true;
        }
        
        private List<string> ParseLine(string line)
        {
            var list = new List<string>();
            line = line.ToLower();

            var sb = new StringBuilder();
            foreach (var ch in line)
            {
                if (char.IsLetter(ch) && ch >= 'а' && ch <= 'я')
                    sb.Append(ch);
                else sb.Append(' ');
            }

            list.AddRange(sb.ToString().Split(' ').Where(str => !String.IsNullOrWhiteSpace(str)));
            return list;
        }

        private Dictionary<string, int> GetWords(List<string> list)
        {
            var dict = new Dictionary<string, int>();
            
            for (int i = 0; i < list.Count; i++)
            {
                var listWords = ParseLine(list[i]);
                
                for (int j = 0; j < listWords.Count; j++)
                {
                    if (dict.ContainsKey(listWords[j]))
                        dict[listWords[j]]++;
                    else if(isWord(listWords[j])) 
                        dict.Add(listWords[j], 1);
                }
            }
            return dict;
        }

        public Dictionary<string, List<string>> GetTopKeywords(List<Product> listOfProducts, int top)
        {
            var listOfCategories = listOfProducts.Select(el => el.Category).Distinct();
            var keywords = new Dictionary<string, List<string>>();
            
            foreach (var category in listOfCategories)
            {
                List<string> listProdFromCat = GetProdByCat(listOfProducts, category);
                var dict = GetWords(listProdFromCat);
                var listWords = dict.Select(el => (el.Key, el.Value)).
                    OrderBy(el => -el.Value).Take(top).ToList();
                
                Console.WriteLine($"{listWords.Count} возможных ключевых слов в категории {category}:");

                var list = new List<string>();
                if (!keywords.ContainsKey(category))
                    keywords.Add(category,list);

                foreach (var el in listWords)
                {
                    Console.Write(el.Key + " ");
                    keywords[category].Add(el.Key);
                }
                
                Console.WriteLine("\nДобавлено в коллекцию\n");
            }
            
            return keywords;
        }
        
        
    }
}