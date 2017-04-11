using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDD_APP.Models
{
    public class Session
    {
        public int id;
        public long timeMilliseconds;

        public Session(long timeMilliseconds)
        {
            this.timeMilliseconds = timeMilliseconds;
        }

        public Session(int id, long timeMilliseconds)
        {
            this.id = id;
            this.timeMilliseconds = timeMilliseconds;
        }

        public static Session get(int id)
        {
            Session session = null;
            String sql = "select * from sessions where id=" + id + " LIMIT 1";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
                session = new Session(
                    Int32.Parse(reader["id"].ToString()),
                    Int64.Parse(reader["time"].ToString()));

            return session;
        }

        public void save()
        {
            String sql = "update sessions set time=" + timeMilliseconds +
                " where id=" + id;
            DBManager.execute(sql);
        }

        public void remove()
        {
            String sql = "delete from sessions where id=" + id;
            DBManager.execute(sql);
        }

        public void add()
        {
            add(this);
        }

        public static void add(Session session)
        {
            String sql = "insert into sessions (id, time) " +
                "values(NULL, '" +
                session.timeMilliseconds + "')";
            DBManager.execute(sql);
            session.id = getLastId();
            //Console.WriteLine(session.id.ToString());
        }

        public static List<Session> getAll()
        {
            List<Session> sessionList = new List<Session>();
            String sql = "select * from sessions order by id desc";
            SQLiteDataReader reader = DBManager.executeReader(sql);

            while (reader.Read())
            {
                //Console.WriteLine(reader["id"] + " " + reader["time"]);
                sessionList.Add(new Session(
                    Int32.Parse(reader["id"].ToString()),
                    Int64.Parse(reader["time"].ToString())));
            }

            return sessionList;
        }

        public static int getSessionsCount()
        {
            return DBManager.getRowCountIn("sessions");
        }

        public static int getLastId()
        {
            List<Session> sessions = Session.getAll();
            return (sessions.Count != 0) ? sessions[0].id : 1;
        }
    }
}
