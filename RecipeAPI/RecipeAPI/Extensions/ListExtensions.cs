using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.Model.Extensions
{
    public static class ListExtensions
    {
        public static List<T> MergeWith<T>(this List<T> thisList, params List<T>[] newList)
        {
            if(thisList == null)
            {
                thisList = new List<T>();
            }

            thisList.AddRange(newList.SelectMany(element => element));

            return thisList;
        }
    }
}
