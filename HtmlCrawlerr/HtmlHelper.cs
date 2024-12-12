using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HtmlCrawlerr
{
    public class HtmlHelper
    {
        // מאפיינים למחלקה
        public string[] AllTags { get; private set; }
        public string[] SelfClosingTags { get; private set; }

        // קונסטרקטור
        public HtmlHelper(string allTagsFilePath, string selfClosingTagsFilePath)
        {
            AllTags = LoadTags(allTagsFilePath);
            SelfClosingTags = LoadTags(selfClosingTagsFilePath);
        }

        // פונקציה לטעינת תגיות מקובץ JSON
        private string[] LoadTags(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<string[]>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tags from {filePath}: {ex.Message}");
                return new string[0]; // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        // GetInstance ממומשת כעת
        public static HtmlHelper GetInstance(string allTagsFilePath, string selfClosingTagsFilePath)
        {
            return new HtmlHelper(allTagsFilePath, selfClosingTagsFilePath);
        }
    }
}
