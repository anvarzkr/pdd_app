using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PDD_APP.Models
{
    public class Category
    {
        public int id;
        public String name;
        public String name_db;

        public Category(int id, String name, String name_db)
        {
            setupCategory(id, name, name_db);
        }

        public Category(String name, String name_db)
        {
            setupCategory(0, name, name_db);
        }

        private void setupCategory(int id, String name, String name_db)
        {
            this.id = id;
            this.name = name;
            this.name_db = name_db;
        }

        public static Category get(int id)
        {
            Category category = null;
            String sql = "select * from categories where id=" + id + " LIMIT 1";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
                category = new Category(
                    Int32.Parse(reader["id"].ToString()),
                    reader["name"].ToString(),
                    reader["name_db"].ToString());

            return category;
        }

        public void save()
        {
            String sql = "update categories set name=" + name + 
                ", name_db=" + name_db +
                " where id=" + id;
            DBManager.execute(sql);
        }

        public void remove()
        {
            String sql = "delete from categories where id=" + id;
            DBManager.execute(sql);
        }

        public void add()
        {
            if (this.name == null || this.name_db == null)
                return;
            add(this);
        }

        public static void add(Category category)
        {
            String sql = "insert into categories (id, name, name_db) " + 
                "values(NULL, '" + 
                category.name + "', '" + 
                category.name_db + "')";
            DBManager.execute(sql);
        }

        public static Category[] getAll()
        {
            int count = getCategoriesCount();
            Category[] categories = new Category[count];
            String sql = "select * from categories order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read() && --count > -1)
                categories[count] = new Category(
                    Int32.Parse(reader["id"].ToString()),
                    reader["name"].ToString(),
                    reader["name_db"].ToString());

            return categories;
        }

        public static void deleteAll()
        {
            String sql = "truncate table categories";
            SQLiteDataReader reader = DBManager.executeReader(sql);
        }

        public static int getCategoriesCount()
        {
            return DBManager.getRowCountIn("categories");
        }

        public static Category[] getRandomCategories(int count){
            Category[] allCategorys = Category.getAll();
            Category[] categories = new Category[(count = Math.Min(count, allCategorys.Length))];
            foreach (Category category in allCategorys)
                MessageBox.Show(category.name);
            Random random = new Random();

            while (--count > 0)
                categories[count] = Category.get(random.Next(allCategorys.Length));

            return categories;
        }
    }
}
