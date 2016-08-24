using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class Category
    {
        public int CategoryRef { get; set; }
        public string CategoryName { get; set; }

        public static List<Category> GetCategoryList()
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                var categoryQuery =
                from c in db.Categories
                orderby c.CategoryName
                select new Category { CategoryRef = c.CategoryRef, CategoryName = c.CategoryName };

                return categoryQuery.ToList();
            }
        }
    }
}
