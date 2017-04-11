using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDD_APP
{
    class DBManager
    {
        private static SQLiteConnection sqLiteConnection = getSqLiteConnection();
        private static SQLiteCommand command;
        private static SQLiteDataReader reader;

        public static SQLiteConnection getSqLiteConnection()
        {
            if (sqLiteConnection == null)
                makeSqlConnection();

            return sqLiteConnection;
        }

        private static void makeSqlConnection()
        {
            sqLiteConnection = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
            sqLiteConnection.Open();

            String sql = "create table if not exists students (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, fname varchar(20) NOT NULL, lname varchar(20) NOT NULL, score int NOT NULL)";
            execute(sql);
            sql = "create table if not exists categories (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name varchar(20) NOT NULL, name_db varchar(20) NOT NULL)";
            execute(sql);
            sql = "create table if not exists tasks (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, type int NOT NULL, category TEXT NOT NULL, block int NOT NULL, task TEXT NOT NULL, right_answer int NOT NULL, clarification TEXT, answers_count int NOT NULL, img_format_1 TEXT NOT NULL, img_format_2 TEXT NOT NULL, img_format_3 TEXT NOT NULL, img_format_4 TEXT NOT NULL, ans_text_1 TEXT, ans_text_2 TEXT, ans_text_3 TEXT, ans_text_4 TEXT)";
            execute(sql);
            sql = "create table if not exists sessions (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, time Long NOT NULL)";
            execute(sql);
            sql = "create table if not exists tests (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, sessionId int NOT NULL, studentId int NOT NULL, time int NOT NULL, rAns int NOT NULL, ansCount int NOT NULL)";
            execute(sql);
        }

        public static int execute(String sql) {
            command = new SQLiteCommand(sql, sqLiteConnection);
            return command.ExecuteNonQuery();
        }

        public static SQLiteDataReader executeReader(String sql) {
            command = new SQLiteCommand(sql, sqLiteConnection);
            reader = command.ExecuteReader();

            return reader;
        }

        public static object executeScalar(String sql)
        {
            command = new SQLiteCommand(sql, sqLiteConnection);
            return command.ExecuteScalar();
        }

        public static int getRowCountIn(String table) 
        {
            String sql = "select count(*) from "+table;
            return Int32.Parse(DBManager.executeScalar(sql).ToString());
        }
    }
}
