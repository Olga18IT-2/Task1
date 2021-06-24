using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Task_1
{
    class Create_json_file
    {
        public void WriteInfo(Work_with_file data, string filePath)
        {
            string newFilePath = Path.GetDirectoryName(filePath) +@"\"+ Path.GetFileNameWithoutExtension(filePath) + ".json";
            string serialized = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(newFilePath, serialized);
            Console.WriteLine("The results of the calculations were successfully saved to a file {0}", newFilePath);
        }
    }

    class Work_with_file
    {
        public string filename { get; set; }
        public int fileSize { get; set; }
        public string lettersCount { get; set; }
        public Dictionary<char, string> letters { get; set; }
        public string wordsCount { get; set; }
        public Dictionary<string, string> words { get; set; }
        public string linesCount { get; set; }
        public string digitsCount { get; set; }
        public string numbersCount { get; set; }
        public string longestWord { get; set; }
        public string wordsWithHyphen { get; set; }
        public string punctuation { get; set; }
        public Work_with_file(string filePath, string text)
        {
            this.filename = GetName(filePath);
            this.fileSize = GetFileSize(text);
            this.lettersCount = GetLettersCount(text).ToString();
            this.letters = GetEveryLetterCount(text);
            this.wordsCount = GetWordsCount(text).ToString();
            this.words = GetEveryWordCount(text);
            this.linesCount = GetLinesCount(text).ToString();
            this.digitsCount = GetDigistCount(text).ToString();
            this.numbersCount = GetNumbersCount(text).ToString();
            this.longestWord = GetLongestWord(text);
            this.wordsWithHyphen = GetWordsWhisHyphenCount(text).ToString();
            this.punctuation = GetPunctuationCount(text).ToString();
        }

        public string GetName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public int GetFileSize(string text)
        {
            return text.Length - GetCount(text, "\r");
        }

        public int GetLettersCount(string text)
        {
            return GetCount(text, "[a-zA-Zа-яёА-ЯЁ]");
        }

        public Dictionary<char, string> GetEveryLetterCount(string text)
        {
           string temp = Regex.Replace(text, "[^a-zA-Zа-яёА-ЯЁ]", "");
           var letters = temp.ToLower().GroupBy(letter => letter).OrderByDescending(letter => letter.Count());
           Dictionary<char, string> tempDict = new Dictionary<char, string>();
           foreach (var letter in letters)
           {
                tempDict.Add(letter.Key, letter.Count().ToString());
           }
            return tempDict;
        }

        public int GetWordsCount(string text)
        {
            return GetCount(text, @"[a-zA-Zа-яёА-ЯЁ]+[-']?[a-zA-Zа-яёА-ЯЁ]*");
        }

        public Dictionary<string, string> GetEveryWordCount(string text)
        {
            string[] words = GetAllWords(text);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
            }
            var everyWordCount = words.GroupBy(a => a).OrderByDescending(a => a.Count());
            Dictionary<string, string> tempDict = new Dictionary<string, string>();
            foreach (var k in everyWordCount)
            {
                tempDict.Add(k.Key, k.Count().ToString());
            }
            return tempDict;
        }

        public int GetLinesCount (string text)
        {
            return GetCount(text, "\n") + 1;
        }

        public int GetDigistCount (string text)
        {
            return GetCount(text, @"\d");
        }

        public int GetNumbersCount (string text)
        {
            return GetCount(text, @"\d*[.,/]?\d+");
        }

        public string GetLongestWord(string text)
        {
            string[] words = GetAllWords(text);
            if (words.Length > 0)
            {
                int max = words.Select(b => b.Length).Max();
                var answer = words.Where(a => a.Length == max).ToArray();
                return answer[0];
            }
            else
            {
                return "";
            }
        }

        public int GetWordsWhisHyphenCount (string text)
        {
            return GetCount(text, @"\w[-]\w");
        }

        public int GetPunctuationCount (string text)
        {
            return GetCount(text, @"[^\w\s\.]|[\.]+|[_]");
        }

        public string[] GetAllWords(string text)
        {
            return GetMatch(text, "[a-zA-Zа-яёА-ЯЁ]+[-']?[a-zA-Zа-яёА-ЯЁ]*").
                Cast<Match>().Select(a => a.Value).ToArray();
        }

        public static MatchCollection GetMatch(string text, string condition)
        {
            Regex r = new Regex(condition);
            return r.Matches(text);
        }

        public static int GetCount(string text, string condition)
        {
            return GetMatch(text, condition).Count;
        }
    }

    class Program
    {
        public static string ReadFile(string file_path)
        {
            try
            {
                FileStream file = new FileStream(file_path, FileMode.Open);
                StreamReader reader = new StreamReader(file);
                string file_content = reader.ReadToEnd();
                reader.Close();
                file.Close();
                return file_content;
            }
            catch
            {
                Console.WriteLine(" The file path is specified incorrectly ! \n");
                return null;
            }
        }
        static void Main(string[] args)
        {
            string text = null, file_path = "";
            while (text == null)
            {
                Console.WriteLine("Enter the path to the file : ");
                file_path = Console.ReadLine();
                text = ReadFile(file_path);
            }
            Work_with_file data = new Work_with_file(file_path, text);
            Create_json_file new_file = new Create_json_file();
            new_file.WriteInfo(data, file_path);
            Console.ReadLine();
        }
    }
}