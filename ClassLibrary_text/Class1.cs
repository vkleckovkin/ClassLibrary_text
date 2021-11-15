using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary_text
{
    public class Class1
    {
        const string symbol = "?-“/*.,”!'\"";

        //метод без многопотока
        //private static Dictionary<string, int> Text_analis(string text)
        //{
        //    // Убираем ненужные символы
        //    foreach (char x in symbol)
        //    {
        //        text = text.Replace(x, ' ');
        //        GC.Collect();
        //    }

        //    // Убираем абзацы
        //    text = text.Replace(Environment.NewLine, " ");

        //    // Создаём массив слов из текста
        //    string[] Words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    text = string.Empty;
        //    GC.Collect();

        //    // Создаём словарь (слово-количество)
        //    Dictionary<string, int> statistics = new Dictionary<string, int>();

        //    //Считаем количество слов, переводя в нижний регистр
        //    foreach (string x in Words.Select(x => x.ToLower()))
        //        if (statistics.TryGetValue(x, out int c))
        //            statistics[x] = c + 1;
        //        else
        //            statistics.Add(x, 1);
        //    Words = new string[1];
        //    GC.Collect();
        //    return statistics;
        //}
        public static Dictionary<string, int> Text_analis_parallel(string text)
        {       
            // Убираем ненужные символы
            foreach (char x in symbol)
            {
                text = text.Replace(x, ' ');
                GC.Collect();
            }

            // Убираем абзацы
            text = text.Replace(Environment.NewLine, " ");

            // Создаём массив слов из текста
            string[] Words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            text = string.Empty;
            GC.Collect();

            // Создаём словарь (слово-количество)
            ConcurrentDictionary<string, int> statistics = new ConcurrentDictionary<string, int>();

            //Считаем количество слов, переводя в нижний регистр
            Parallel.ForEach(Words.Select(x => x.ToLower()), x =>
            {

                if (statistics.TryGetValue(x, out int c))
                    statistics[x] = c + 1;
                else
                    statistics.TryAdd(x, 1);
                Words = new string[1];
            });         
            var statistics_d = statistics.ToDictionary(entry => entry.Key,
                                                      entry => entry.Value);
            return statistics_d;
        }
    }
}
