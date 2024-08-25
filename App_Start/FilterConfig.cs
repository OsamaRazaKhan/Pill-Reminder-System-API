using System.Web;
using System.Web.Mvc;

namespace Pill_Reminder_System_api24
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
