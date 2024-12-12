using System.Collections.Generic;

public static class HtmlElementExtensions
{
    // פונקציה להחזרת כל הצאצאים של אלמנט HTML כולל את האלמנט עצמו
    public static IEnumerable<HtmlElement> Descendants(this HtmlElement element)
    {
        // יצירת תור (queue) שיאחסן את כל הצאצאים של האלמנט
        var queue = new Queue<HtmlElement>();

        // הוספת האלמנט הראשוני לתור
        queue.Enqueue(element);

        // כל עוד יש אלמנטים בתור, נמשיך לחפש צאצאים
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();  // שולפים את האלמנט הנוכחי
            yield return current;  // מחזירים את האלמנט הנוכחי

            // עבור כל ילד של האלמנט הנוכחי, נוסיף אותו לתור
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);  // הוספת הילד לתור
            }
        }
    }
}
