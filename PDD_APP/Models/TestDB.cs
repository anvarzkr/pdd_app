using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDD_APP.Models
{
    public class TestDB
    {
        public int id;
        public int sessionId;
        public int time;
        public int studentId;   
        public int rAns;
        public int ansCount;

        public TestDB(int sessionId, int time, int studentId, int rAns, int ansCount)
        {
            this.time = time;
            this.sessionId = sessionId;
            this.studentId = studentId;
            this.rAns = rAns;
            this.ansCount = ansCount;
        }

        public TestDB(int id, int sessionId, int time, int studentId, int rAns, int ansCount)
        {
            this.id = id;
            this.sessionId = sessionId;
            this.time = time;
            this.studentId = studentId;
            this.rAns = rAns;
            this.ansCount = ansCount;
        }

        public static TestDB get(int id)
        {
            TestDB testDB = null;
            String sql = "select * from tests where id=" + id + " LIMIT 1";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
                testDB = new TestDB(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["sessionId"].ToString()),
                    Int32.Parse(reader["time"].ToString()),
                    Int32.Parse(reader["studentId"].ToString()),
                    Int32.Parse(reader["rAns"].ToString()),
                    Int32.Parse(reader["ansCount"].ToString()));

            return testDB;
        }

        public void save()
        {
            String sql = "update tests set time=" + time +
                ", sessionId=" + sessionId +
                ", studentId=" + studentId +
                ", rAns=" + rAns +
                ", ansCount=" + ansCount +
                " where id=" + id;
            DBManager.execute(sql);
        }

        public void remove()
        {
            String sql = "delete from tests where id=" + id;
            DBManager.execute(sql);
        }

        public void add()
        {
            add(this);
        }

        public static void add(TestDB testDB)
        {
            String sql = "insert into tests (id, sessionId, time, studentId, rAns, ansCount) " +
                "values(NULL, '" +
                testDB.sessionId + "', '" +
                testDB.time + "', '" +
                testDB.studentId + "', '" +
                testDB.rAns + "', '" +
                testDB.ansCount + "')";
            DBManager.execute(sql);
        }

        public static TestDB[] getAll()
        {
            int count = getTestDBCount();
            TestDB[] testDBs = new TestDB[count];
            String sql = "select * from tests order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read() && --count > -1)
                testDBs[count] = new TestDB(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["sessionId"].ToString()),
                    Int32.Parse(reader["time"].ToString()),
                    Int32.Parse(reader["studentId"].ToString()),
                    Int32.Parse(reader["rAns"].ToString()),
                    Int32.Parse(reader["ansCount"].ToString()));

            return testDBs;
        }

        public static List<TestDB> getAll(int sessionId)
        {
            List<TestDB> testsList = new List<TestDB>();
            String sql = "select * from tests where sessionId=" + sessionId + " order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
                testsList.Add(new TestDB(
                    Int32.Parse(reader["id"].ToString()),
                    Int32.Parse(reader["sessionId"].ToString()),
                    Int32.Parse(reader["time"].ToString()),
                    Int32.Parse(reader["studentId"].ToString()),
                    Int32.Parse(reader["rAns"].ToString()),
                    Int32.Parse(reader["ansCount"].ToString())));

            return testsList;
        }

        public static int getTestDBCount()
        {
            return DBManager.getRowCountIn("tests");
        }

        public static int getLastId()
        {
            TestDB[] testDBs = TestDB.getAll();
            return testDBs[testDBs.Length - 1].id;
        }
    }
}
