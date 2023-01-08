using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathbetraysJenkinsBuildWatcher
{
    static public class TestCredentials
    {
        public static bool g_useTestCredentials = false;

        public static string g_username = "";
        public static string g_password = "";
        public static string g_url = "http://localhost:8080";
    }

    static public class TestValues
    {
        public static bool g_disableTimer = false;
    }
}
