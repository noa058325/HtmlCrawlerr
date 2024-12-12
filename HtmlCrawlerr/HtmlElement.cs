using System;
using System.Collections.Generic;
using System.Linq;

public class HtmlElement
{
    // מאפיינים למחלקה
    public string Id { get; set; } // מזהה ייחודי לתגית (לדוגמה: id="content")
    public string Name { get; set; } // שם התגית (לדוגמה: div, p)
    public Dictionary<string, string> Attributes { get; set; } // רשימת תכונות של התגית
    public List<string> Classes { get; set; } // רשימת מחלקות CSS
    public string InnerHtml { get; set; } // התוכן בתוך התגית
    public HtmlElement Parent { get; set; } // הורה של האלמנט
    public List<HtmlElement> Children { get; set; } // רשימת אלמנטים ילדים של האלמנט

    // קונסטרקטור ברירת מחדל
    public HtmlElement()
    {
        Attributes = new Dictionary<string, string>();
        Classes = new List<string>();
        Children = new List<HtmlElement>();
    }

    // קונסטרקטור עם פרמטרים
    public HtmlElement(string name, string id = "", string innerHtml = "")
    {
        Name = name;
        Id = id;
        InnerHtml = innerHtml;
        Attributes = new Dictionary<string, string>();
        Classes = new List<string>();
        Children = new List<HtmlElement>();
    }

    // פונקציה להוספת תכונה לתגית
    public void AddAttribute(string key, string value)
    {
        Attributes[key] = value;
    }

    // פונקציה להוספת מחלקה לתגית
    public void AddClass(string className)
    {
        Classes.Add(className);
    }

    // פונקציה להוספת ילד לתגית
    public void AddChild(HtmlElement child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    // פונקציה להחזרת מחרוזת שמייצגת את התגית ואת תוכנה
    public override string ToString()
    {
        var classes = string.Join(" ", Classes);
        var attributes = string.Join(" ", Attributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));
        var openTag = $"<{Name} id=\"{Id}\" class=\"{classes}\" {attributes}>";
        var closeTag = $"</{Name}>";
        var childrenHtml = string.Join("", Children.Select(child => child.ToString()));

        return $"{openTag}{InnerHtml}{childrenHtml}{closeTag}";
    }

    // פונקציה להחזרת מחרוזת מעוצבת עם רווחים
    public string ToFormattedString(int indent = 0)
    {
        var indentString = new string(' ', indent * 2);
        var classes = string.Join(" ", Classes);
        var attributes = string.Join(" ", Attributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));
        var openTag = $"{indentString}<{Name} id=\"{Id}\" class=\"{classes}\" {attributes}>";
        var closeTag = $"{indentString}</{Name}>";
        var childrenHtml = string.Join(Environment.NewLine, Children.Select(child => child.ToFormattedString(indent + 1)));

        return $"{openTag}{Environment.NewLine}{childrenHtml}{Environment.NewLine}{closeTag}";
    }

    // פונקציה להחזרת כל הצאצאים של האלמנט (ילדים וילדים של ילדים)
    public IEnumerable<HtmlElement> Descendants()
    {
        var queue = new Queue<HtmlElement>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
    }

    // פונקציה להחזרת כל ההורים של האלמנט
    public IEnumerable<HtmlElement> Ancestors()
    {
        var current = this.Parent;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }
}
