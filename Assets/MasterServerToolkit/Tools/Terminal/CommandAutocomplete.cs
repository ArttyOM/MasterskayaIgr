using System.Collections.Generic;

namespace MasterServerToolkit.CommandTerminal
{
    public class CommandAutocomplete
    {
        private List<string> known_words = new();
        private List<string> buffer = new();

        public void Register(string word)
        {
            known_words.Add(word.ToLower());
        }

        public string[] Complete(ref string text)
        {
            var partial_word = EatLastWord(ref text).ToLower();
            string known;
            buffer.Clear();

            for (var i = 0; i < known_words.Count; i++)
            {
                known = known_words[i];

                if (known.StartsWith(partial_word)) buffer.Add(known);
            }

            return buffer.ToArray();
        }

        private string EatLastWord(ref string text)
        {
            var last_space = text.LastIndexOf(' ');
            var result = text.Substring(last_space + 1);

            text = text.Substring(0, last_space + 1); // Remaining (keep space)
            return result;
        }
    }
}