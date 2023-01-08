using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DeathbetraysJenkinsBuildWatcher
{
    static class Logger
    {
        public enum LogLevel
        {
            Spam,
            Info,
            Warning,
            Error,
        }

        static private string gs_LogPath = "logs/";
        static private KeyValuePair<string, bool> gs_logFile = new KeyValuePair<string, bool>(gs_LogPath + "log.log", false);
        static private KeyValuePair<string, bool> gs_logFileVerbose = new KeyValuePair<string, bool>(gs_LogPath + "logVerbose.log", false);

        static public LogLevel Level { get { return gs_level; } set { gs_level = value; } }
        static private LogLevel gs_level = LogLevel.Info;

        static public bool EnableLogFile
        {
            get { return Preferences.Prefs.EnableLogging; }
        }
        static public bool EnableLogFileVerbose
        {
            get { return Preferences.Prefs.EnableLoggingVerbose; }
        }

        static private Mutex gs_mutex = new Mutex();


        static public void SetLogFileName(string _region, string _faction)
        {
            string newName = gs_LogPath + _region + "-" + _faction;
            gs_logFile = new KeyValuePair<string, bool>(newName + "Log.log", false);
            gs_logFileVerbose = new KeyValuePair<string, bool>(newName + "LogVerbose.log", false);
        }

        static public void Log(LogLevel _level, string _format, params object[] _params)
        {
            gs_mutex.WaitOne();

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logFormat = "[" + dateTime + "][" + _level.ToString() + "] " + _format;

            // Always log to the verbose log.
            if (EnableLogFileVerbose)
            {
                StreamWriter sw = OpenSteam(ref gs_logFileVerbose);
                sw.WriteLine(logFormat, _params);
                sw.Close();
            }

            // Only log to the normal log if it meets the level requirement.
            if (_level >= gs_level)
            {
                if (EnableLogFile)
                {
                    StreamWriter sw = OpenSteam(ref gs_logFile);
                    sw.WriteLine(logFormat, _params);
                    sw.Close();
                }

                // Ones written to the normal log also get printed to the console.
                ConsoleColor original = Console.ForegroundColor;
                Console.ForegroundColor = GetConsoleColourForLevel(gs_level);
                Console.WriteLine(_format, _params);
                Console.ForegroundColor = original;
            }

            gs_mutex.ReleaseMutex();
        }

        static private StreamWriter OpenSteam(ref KeyValuePair<string, bool> _fileName)
        {
            StreamWriter fileWriter = null;
            if (_fileName.Value)
            {
                fileWriter = File.AppendText(_fileName.Key);
            }
            else
            {
                FileInfo fi = new FileInfo(_fileName.Key);
                if (!fi.Directory.Exists)
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }

                fileWriter = new StreamWriter(_fileName.Key);
                _fileName = new KeyValuePair<string, bool>(_fileName.Key, true);
            }
            return fileWriter;
        }

        static private ConsoleColor GetConsoleColourForLevel(LogLevel _level)
        {
            switch (_level)
            {
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Info:
                    return ConsoleColor.White;
                case LogLevel.Spam:
                default:
                    return ConsoleColor.Gray;
            }
        }
    }
}
