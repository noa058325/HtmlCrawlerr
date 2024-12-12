using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using HtmlCrawlerr;  

class Program
{
    // פונקציה ראשית, שמבצעת את הפעולות העיקריות של התוכנית
    static async Task Main(string[] args)
    {
        // יצירת מופע של HtmlHelper באמצעות קבצי JSON
        var htmlHelper = HtmlHelper.GetInstance(
            @"C:\Users\user\source\repos\HtmlCrawlerr\HtmlCrawlerr\bin\Debug\net8.0\resource\HtmlTags.json",
            @"C:\Users\user\source\repos\HtmlCrawlerr\HtmlCrawlerr\bin\Debug\net8.0\resource\HtmlVoidTags.json");

        // הדפסת כל התגיות שמופיעות בקובץ HtmlTags.json
        Console.WriteLine("All HTML Tags:");
        foreach (var tag in htmlHelper.AllTags)
        {
            Console.WriteLine(tag);  // הדפסת כל תג HTML
        }

        // הדפסת התגיות הסלפי-קלוזינג (תגיות שנסגרות אוטומטית)
        Console.WriteLine("\nSelf-Closing HTML Tags:");
        foreach (var tag in htmlHelper.SelfClosingTags)
        {
            Console.WriteLine(tag);  // הדפסת תגיות סלפי-קלוזינג
        }

        // טוען את עמוד ה-HTML מה-URL המבוקש
        var html = await Load("https://hebrewbooks.org/");

        // מנתח את ה-HTML לעץ של אלמנטים
        var elements = ParseHtmlToElements(html);

        // הדפסת כל האלמנטים שנמצאו בעץ ה-HTML
        foreach (var element in elements)
        {
            Console.WriteLine(element);  // הדפסת כל אלמנט
        }

        // יצירת סלקטור (selector) לאיתור אלמנט עם id="ftritem2"
        var selector = Selector.Parse("div#ftritem2");

        // חיפוש כל האלמנטים שמתאימים לסלקטור
        var matches = elements.SelectMany(e => e.Descendants())
                              .Where(e => MatchesSelector(e, selector))
                              .ToList();

        // הדפסת כל האלמנטים שתאמו לסלקטור
        Console.WriteLine("Matching elements:");
        foreach (var match in matches)
        {
            Console.WriteLine(match.ToFormattedString());  // הדפסת אלמנטים שמתאימים לסלקטור
        }
    }

    // פונקציה לטעינת תוכן HTML מ-URL
    static async Task<string> Load(string url)
    {
        HttpClient client = new HttpClient();  // יצירת אובייקט HttpClient לשליחת בקשות HTTP
        var response = await client.GetAsync(url);  // שליחה של בקשה ל-URL
        var html = await response.Content.ReadAsStringAsync();  // קריאת התוכן כטקסט
        return html;  // מחזירה את תוכן ה-HTML
    }

    // פונקציה שמנתחת את ה-HTML ומחזירה רשימה של HtmlElement
    static List<HtmlElement> ParseHtmlToElements(string html)
    {
        var elements = new List<HtmlElement>();  // יצירת רשימה לאחסון האלמנטים
        var regex = new Regex("<(.*?)>");  // ביטוי רגולרי למציאת תגיות HTML
        var matches = regex.Matches(html);  // חיפוש כל ההתאמות ב-HTML

        HtmlElement currentElement = null;  // אתחול משתנה לאלמנט הנוכחי

        // עבור כל התאמה שנמצאה
        foreach (Match match in matches)
        {
            var tag = match.Groups[1].Value;  // קבלת שם התגית מתוך ההתאמה
            if (tag.StartsWith("/"))  // אם מדובר בתגית סגירה
            {
                currentElement = currentElement.Parent;  // חזרה לאלמנט ההורה
            }
            else
            {
                var newElement = new HtmlElement { Name = tag };  // יצירת אלמנט חדש
                if (currentElement != null)
                {
                    currentElement.AddChild(newElement);  // הוספת האלמנט החדש כהילד של האלמנט הנוכחי
                }
                elements.Add(newElement);  // הוספת האלמנט החדש לרשימה
                if (!tag.EndsWith("/"))  // אם לא מדובר בתגית סלפי-קלוזינג
                {
                    currentElement = newElement;  // מעדכן את האלמנט הנוכחי
                }
            }
        }

        return elements;  // מחזיר את רשימת האלמנטים שנמצאו
    }

    // פונקציה שבודקת אם אלמנט תואם לסלקטור
    static bool MatchesSelector(HtmlElement element, Selector selector)
    {
        if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
        {
            return false;  // אם שם התג לא תואם, מחזירה false
        }

        if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
        {
            return false;  // אם ה-id לא תואם, מחזירה false
        }

        if (selector.Classes.Any() && !selector.Classes.All(c => element.Classes.Contains(c)))
        {
            return false;  // אם יש מחלקות ולא כל המחלקות תואמות, מחזירה false
        }

        return true;  // אם כל התנאים מתקיימים, מחזירה true
    }
}
