using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDD_APP.Models
{
    class Admin
    {
        public static String hostIpAdress = getHostIp();

        public static void setPass(String password)
        {
            Utils.iniFile.IniWriteValue("General", "password", Utils.CalculateMD5Hash(password));
        }

        public static String getPass() {
            return Utils.iniFile.IniReadValue("General", "password");
        }

        public static void setHostIp(String hostIpAdressArg)
        {
            hostIpAdress = hostIpAdressArg;
            Utils.iniFile.IniWriteValue("General", "hostIp", hostIpAdressArg);
        }

        public static String getHostIp()
        {
            return Utils.iniFile.IniReadValue("General", "hostIp");
        }
    }
}
