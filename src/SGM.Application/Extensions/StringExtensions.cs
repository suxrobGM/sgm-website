using System.Text.RegularExpressions;

namespace SGM.Application;

public static class StringExtensions
{
    /// <summary>
    /// Generates slugs
    /// </summary>
    /// <param name="str">Given string</param>
    /// <param name="useHyphen">Flag which indicates that slug will be generated with hyphens instead underscore</param>
    /// <param name="useLowerLetters">Flag which indicates that slug will be generated with lower letters</param>
    /// <returns>Slugified string</returns>
    public static string Slugify(this string str, bool useHyphen = true, bool useLowerLetters = true)
    {
        var url = str.TranslateToLatin();

        // invalid chars           
        url = Regex.Replace(url, @"[^A-Za-z0-9\s-]", "");

        // convert multiple spaces into one space 
        url = Regex.Replace(url, @"\s+", " ").Trim();
        var words = url.Split().Where(str => !string.IsNullOrWhiteSpace(str));
        url = string.Join(useHyphen ? '-' : '_', words);

        if (useLowerLetters)
            url = url.ToLower();

        return url;
    }

    /// <summary>
    /// Translate all letters to latin alphabets
    /// </summary>
    /// <param name="str">Given string</param>
    /// <returns></returns>
    public static string TranslateToLatin(this string str)
    {
        string[] latUp =
        [
            "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U",
            "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya"
        ];
        string[] latLow =
        [
            "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u",
            "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya"
        ];
        string[] rusUp =
        [
            "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У",
            "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"
        ];
        string[] rusLow =
        [
            "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у",
            "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"
        ];
        for (var i = 0; i <= 32; i++)
        {
            str = str.Replace(rusUp[i], latUp[i]);
            str = str.Replace(rusLow[i], latLow[i]);
        }
        return str;
    }
}
