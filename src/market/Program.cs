using System;
using System.Collections.Generic;
using System.Linq;

namespace market
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var stats = new Statistics();
            List<Product> listOfProducts = stats.GetFromFile();
            var listOfCategories = listOfProducts.Select(el => el.Category).Distinct();
            var listOfDescriptions = listOfProducts.Select(el => el.Description).Distinct();
            var listOfVendors = listOfProducts.Select(el => el.Vendor).Distinct();
            
            Console.WriteLine($"Всего категорий: {listOfCategories.Count()}\nВсего товаров: {listOfDescriptions.Count()}\n" +
                              $"Всего производителей: {listOfVendors.Count()}");

            var listOfKeywords = stats.GetTopKeywords(listOfProducts, 15);

            //вот тут ручками можно поменять категорию))
            var cat = listOfCategories.ToArray()[1];
            Console.WriteLine($"Категория: {cat}");

            foreach (var el in listOfKeywords[cat])
            {
                var list = listOfKeywords.Where(value => value.Value.Exists(word => word == el)).
                    Select(category => category).ToList();
                if (list.Count >= 5)
                    Console.WriteLine($"Слово {el} встречается ещё в {list.Count-1} категориях");
                else 
                    foreach (var a in list)
                        if (a.Key != cat)
                            Console.WriteLine($"Слово {el} встречается в категории {a.Key}");
            }
        }
    }
}