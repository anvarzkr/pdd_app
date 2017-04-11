using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PDD_APP.Models
{
    public class Task
    {
        public int id;
        public int type = 1;
        public String category;
        public int block;
        public String task;
        public String clarification;
        public String[] img_format;
        public String[] ans_text;
        public int right_answer;
        public int answers_count = 0;

        public Task(int id, int type, String category, int block, String task, int right_answer, String clarification, int answers_count, String[] img_format, String[] ans_text)
        {
            setupTask(id, type, category, block, task, right_answer, clarification, answers_count, img_format, ans_text);
        }

        public Task(int type, String category, int block, String task, int right_answer, String clarification, int answers_count, String[] img_format, String[] ans_text)
        {
            setupTask(0, type, category, block, task, right_answer, clarification, answers_count, img_format, ans_text);
        }

        private void setupTask(int id, int type, String category, int block, String task, int right_answer, String clarification, int answers_count, String[] img_format, String[] ans_text)
        {
            this.img_format = new String[img_format.Length];
            this.ans_text = new String[ans_text.Length];
            this.id = id;
            this.type = type;
            this.block = block;
            this.category = category;
            this.task = task;
            this.clarification = clarification;
            this.right_answer = right_answer;
            this.answers_count = answers_count;
            for (int i = 0; i < img_format.Length; i++)
                this.img_format[i] = img_format[i];
            for (int i = 0; i < ans_text.Length; i++)
                this.ans_text[i] = ans_text[i];
        }

        public static Task get(int id)
        {
            Task task = null;
            String sql = "select * from tasks where id=" + id + " LIMIT 1";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
                task = new Task(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["type"].ToString()),
                    reader["category"].ToString(),
                    Int32.Parse(reader["block"].ToString()),
                    reader["task"].ToString(),
                    Int32.Parse(reader["right_answer"].ToString()),
                    reader["clarification"].ToString(),
                    Int32.Parse(reader["answers_count"].ToString()),
                    new String[]{
                        reader["img_format_1"].ToString(),
                        reader["img_format_2"].ToString(),
                        reader["img_format_3"].ToString(),
                        reader["img_format_4"].ToString()
                    },
                    new String[]{
                        reader["ans_text_1"].ToString(),
                        reader["ans_text_2"].ToString(),
                        reader["ans_text_3"].ToString(),
                        reader["ans_text_4"].ToString()
                    });

            return task;
        }

        public void save()
        {
            String sql = "update tasks set type=" + type +
                ", category=" + category +
                ", block=" + block +
                ", task=" + task +
                ", right_answer=" + right_answer +
                ", clarification=" + clarification +
                ", answers_count=" + answers_count +
                ", img_format_1=" + img_format[0] +
                ", img_format_2=" + img_format[1] +
                ", img_format_3=" + img_format[2] +
                ", img_format_4=" + img_format[3] +
                ", ans_text_1=" + img_format[0] +
                ", ans_text_2=" + img_format[1] +
                ", ans_text_3=" + img_format[2] +
                ", ans_text_4=" + img_format[3] +
                " where id=" + id;
            DBManager.execute(sql);
        }

        public void remove()
        {
            String sql = "delete from tasks where id=" + id;
            DBManager.execute(sql);
        }

        public void add()
        {
            if (this.category == null
                || this.task == null
                || this.clarification == null
                || this.img_format[0] == null)
                return;
            add(this);
        }

        public static void add(Task task)
        {
            String sql = "insert into tasks (id, type, category, block, task, right_answer, clarification, answers_count, img_format_1, img_format_2, img_format_3, img_format_4, ans_text_1, ans_text_2, ans_text_3, ans_text_4) " +
                "values(NULL, '" +
                task.type + "', '" +
                task.category + "', '" +
                task.block + "', '" +
                task.task + "', '" +
                task.right_answer + "', '" +
                task.clarification + "', '" +
                task.answers_count + "', '" +
                task.img_format[0] + "', '" +
                task.img_format[1] + "', '" +
                task.img_format[2] + "', '" +
                task.img_format[3] + "', '" +
                task.ans_text[0] + "', '" +
                task.ans_text[1] + "', '" +
                task.ans_text[2] + "', '" +
                task.ans_text[3] + "')";
            DBManager.execute(sql);
        }

        public static Task[] getAll()
        {
            int count = getTasksCount();
            Task[] tasks = new Task[count];
            String sql = "select * from tasks order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read() && --count > -1)
                tasks[count] = new Task(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["type"].ToString()),
                    reader["category"].ToString(),
                    Int32.Parse(reader["block"].ToString()),
                    reader["task"].ToString(),
                    Int32.Parse(reader["right_answer"].ToString()),
                    reader["clarification"].ToString(),
                    Int32.Parse(reader["answers_count"].ToString()),
                    new String[]{
                        reader["img_format_1"].ToString(),
                        reader["img_format_2"].ToString(),
                        reader["img_format_3"].ToString(),
                        reader["img_format_4"].ToString()
                    },
                    new String[]{
                        reader["ans_text_1"].ToString(),
                        reader["ans_text_2"].ToString(),
                        reader["ans_text_3"].ToString(),
                        reader["ans_text_4"].ToString()
                    });

            return tasks;
        }

        public static void deleteAll()
        {
            String sql = "truncate table tasks";
            SQLiteDataReader reader = DBManager.executeReader(sql);
        }

        public static int getTasksCount()
        {
            return DBManager.getRowCountIn("tasks");
        }

        /*
         * 
         * returns 5 Task elements
         * 
         */
        public static Task[] get(Category category, int block)
        {
            if (block < 0 || block > 5)
                block = 1;
            if (category == null || category.name_db.Equals(""))
                category = Category.getAll()[0];
            Task[] tasks = new Task[5];
            String sql = "select * from tasks where category='" + category.name_db + "' and block='" + block + "' order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            for (int i = 0; i < 5; i++)
            {
                if (!reader.Read())
                    break;
                //Console.WriteLine();
                tasks[i] = new Task(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["type"].ToString()),
                    reader["category"].ToString(),
                    Int32.Parse(reader["block"].ToString()),
                    reader["task"].ToString(),
                    Int32.Parse(reader["right_answer"].ToString()),
                    reader["clarification"].ToString(),
                    Int32.Parse(reader["answers_count"].ToString()),
                    new String[]{
                        reader["img_format_1"].ToString(),
                        reader["img_format_2"].ToString(),
                        reader["img_format_3"].ToString(),
                        reader["img_format_4"].ToString()
                    },
                    new String[]{
                        reader["ans_text_1"].ToString(),
                        reader["ans_text_2"].ToString(),
                        reader["ans_text_3"].ToString(),
                        reader["ans_text_4"].ToString()
                    });
            }

            return tasks;
        }

        public static Task[] getRandomTasks(int count)
        {
            Task[] allTasks = Task.getAll();
            Task[] tasks = new Task[(count = Math.Min(count, allTasks.Length))];
            //foreach (Task task in allTasks)
            //    MessageBox.Show(task.task);
            Random random = new Random();

            while (--count >= 0)
                tasks[count] = allTasks[random.Next(allTasks.Length)];

            return tasks;
        }

        public static Task[] getTestingTasks()
        {
            Task[] tasks = new Task[14];
            Random random = new Random();

            int count = 0;

            foreach(Category category in Category.getAll()){
                Task[] catTasks = new Task[4];
                for (int i = 1; i <= 4; i++)
                    catTasks[i - 1] = Task.get(category, i)[random.Next(5)];

                for (int i = count; i < count + 4; i++)
                {
                    if (i > 13)
                        break;
                    tasks[i] = catTasks[i - count];
                }

                count += 4;
            }

            return tasks;
        }

        public static int getLastId()
        {
            Task[] tasks = Task.getAll();
            return tasks[tasks.Length - 1].id;
        }
    }
}
