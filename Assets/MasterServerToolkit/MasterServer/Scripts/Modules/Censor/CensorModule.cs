using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    public class CensorModule : BaseServerModule
    {
        [SerializeField] protected List<string> censoredWords;

        [Header("Settings")] [SerializeField] private TextAsset[] wordsLists;
        [SerializeField] [TextArea(5, 10)] private string matchPattern = @"\s*{0}(\s|\W)+";

        public override void Initialize(IServer server)
        {
            ParseTextFiles();
        }

        protected virtual void ParseTextFiles()
        {
            censoredWords = new List<string>();

            var splitter = new char[] {','};

            foreach (var words in wordsLists)
                censoredWords.AddRange(words.text.Split(splitter, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Trim()));
        }

        /// <summary>
        /// Determines whether the text is dirty (has a bad word in it).
        /// </summary>
        /// <returns><c>true</c> if text is dirty; otherwise, <c>false</c>.</returns>
        /// <param name="text">Text to check for dirty words.</param>
        public virtual bool HasCensoredWord(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (var pattern in censoredWords)
            {
                var regex = new Regex(string.Format(matchPattern, pattern), RegexOptions.IgnoreCase);

                if (regex.IsMatch(text)) return true;
            }

            return false;
        }
    }
}