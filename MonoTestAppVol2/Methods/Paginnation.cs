using System.Collections.Generic;
using System.Linq;

namespace Pagination
{
    public static class Pagination

    
    {
        public static List<T> Paginate<T>(List<T> items, int page, int pageSize)
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    

    public static int GetNumOfPages<T>(List<T> items, int pageSize)
    {
        return (int)Math.Ceiling((double)items.Count / pageSize);
    }




    }
}