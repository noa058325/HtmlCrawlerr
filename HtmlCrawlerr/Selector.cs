using System;
using System.Collections.Generic;

public class Selector
{
    public string TagName { get; set; } // שם התגית
    public string Id { get; set; } // מזהה של התגית
    public List<string> Classes { get; set; } // רשימת מחלקות (CSS)
    public Selector Parent { get; set; } // הורה של הסלקטור
    public Selector Child { get; set; } // ילד של הסלקטור

    // קונסטרקטור
    public Selector()
    {
        Classes = new List<string>();
    }

    // פונקציה לניתוח מחרוזת של סלקטור
    public static Selector Parse(string query)
    {
        var root = new Selector();
        var current = root;

        var parts = query.Split(' ');
        foreach (var part in parts)
        {
            var subParts = part.Split(new char[] { '#', '.' });
            if (subParts[0].Length > 0)
            {
                current.TagName = subParts[0];
            }

            for (int i = 1; i < subParts.Length; i++)
            {
                if (part[i - 1] == '#')
                {
                    current.Id = subParts[i];
                }
                else if (part[i - 1] == '.')
                {
                    current.Classes.Add(subParts[i]);
                }
            }

            var child = new Selector { Parent = current };
            current.Child = child;
            current = child;
        }

        return root;
    }
}
