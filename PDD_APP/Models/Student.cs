using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace PDD_APP.Models
{
    public class Student
    {
        public int id { get; set; }
        public String fname { get; set; }
        public String lname { get; set; }
        public int score { get; set; }

        public Student(){
        
        }

        public Student(String fname, String lname, int score) {
            this.fname = fname;
            this.lname = lname;
            this.score = score;
        }
        public Student(int id, String fname, String lname, int score)
        {
            this.id = id;
            this.fname = fname;
            this.lname = lname;
            this.score = score;
        }

        public static Student get(int id){
            Student student = null;
            String sql = "select * from students where id="+id+" LIMIT 1";
            SQLiteDataReader reader = DBManager.executeReader(sql);
            
            while(reader.Read())
                student = new Student(
                        Int32.Parse(reader["id"].ToString()), 
                        reader["fname"].ToString(), 
                        reader["lname"].ToString(), 
                        Int32.Parse(reader["score"].ToString()));

            return student;
        }

        public void save(){
            String sql = "update students set fname="+fname+", lname="+lname+", score="+score+" where id="+id;
            DBManager.execute(sql);
        }

        public void remove() {
            String sql = "delete from students where id=" + id;
            DBManager.execute(sql);
        }
        public static void remove(int studentId)
        {
            String sql = "delete from students where id=" + studentId;
            DBManager.execute(sql);
        }

        public void add() {
            if (this.fname == null || this.lname == null)
                return;
            add(this);
        }

        public static void add(Student student)
        {
            String sql = "insert into students (id, fname, lname, score) values(NULL, '" + student.fname + "', '" + student.lname + "', '" + student.score + "')";
            DBManager.execute(sql);
        }

        public static void add(Student student, int id)
        {
            String sql = "insert into students (id, fname, lname, score) values('" + id + "', '" + student.fname + "', '" + student.lname + "', '" + student.score + "')";
            DBManager.execute(sql);
        }

        public static Student[] getAll() 
        {
            int count = getStudentsCount();
            Student[] students = new Student[count];
            String sql = "select * from students order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read() && --count > -1)
                students[count] = new Student(
                        Int32.Parse(reader["id"].ToString()),
                        reader["fname"].ToString(),
                        reader["lname"].ToString(),
                        Int32.Parse(reader["score"].ToString()));

            return students;
        }

        public static void deleteAll()
        {
            String sql = "truncate table students";
            SQLiteDataReader reader = DBManager.executeReader(sql);
        }

        public static int getStudentsCount()
        {
            return DBManager.getRowCountIn("students");
        }

        public static int getLastId()
        {
            Student[] students = Student.getAll();
            return students[students.Length - 1].id;
        }

    }
}
