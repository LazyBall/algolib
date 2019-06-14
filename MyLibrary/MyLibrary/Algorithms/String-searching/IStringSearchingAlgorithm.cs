using System.Collections.Generic;

namespace MyLibrary.Algorithms.StringSearch
{
    /// <summary>
    /// Представляет алгоритм поиска подстроки в строке.
    /// </summary>
    public interface IStringSearchingAlgorithm
    {
        ///  <summary>
        ///  Возвращает все вхождения образца в строку, в которой осуществляется поиск.
        ///  </summary>
        ///  <returns>
        ///  Перечисление индексов вхождения образца в строку, в которой осуществляется поиск,
        ///  в порядке возрастания.
        ///  </returns>
        ///  <param name = "pattern" > Строка, вхождения которой нужно найти.</param>
        ///  <param name = "text" > Строка, в которой осуществляется поиск.</param>
        IEnumerable<int> GetAllEntries(string pattern, string text);

        ///  <summary>
        ///  Возвращает первое вхождение образца в строку, в которой осуществляется поиск.
        ///  </summary>
        ///  <returns>
        ///  Индекс первого вхождения или -1, если вхождение не найдено.
        ///  </returns>
        ///  <param name = "pattern" > Строка, вхождение которой нужно найти.</param>
        ///  <param name = "text" > Строка, в которой осуществляется поиск.</param>
        int GetFirstEntry(string pattern, string text);

    }
}