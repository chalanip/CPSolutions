
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Utility
{
    /// <summary>
    /// Class to handle all the log routines.
    /// </summary>
    public static class Log
    {
        //static ILog log = LogManager.GetLogger("myLog");
        static readonly Logger log = LogManager.GetLogger("cpLog");

        /// <summary>
        /// Log trace start.
        /// </summary>
        public static void TraceStart()
        {
            try
            {
                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Debug(string.Format("Start method :: {0}.{1} [Line: {2}]"
                        , reflectedType.FullName
                        , stackTrace.GetMethod().Name
                        , stackTrace.GetFileLineNumber()));
            }
            catch { }
        }

        /// <summary>
        /// Log trace end.
        /// </summary>
        public static void TraceEnd()
        {
            try
            {
                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Debug(string.Format("End method :: {0}.{1} [Line: {2}]"
                        , reflectedType.FullName
                        , stackTrace.GetMethod().Name
                        , stackTrace.GetFileLineNumber()));
            }
            catch { }
        }

        /// <summary>
        /// Log trace execute.
        /// </summary>
        /// <param name="description"></param>
        public static void TraceExecute(string description = "")
        {
            try
            {
                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Debug(string.Format("Executing :: {0}.{1} [Line: {2}] {3}"
                        , reflectedType.FullName
                        , stackTrace.GetMethod().Name
                        , stackTrace.GetFileLineNumber()
                        , description));
            }
            catch { }
        }

        /// <summary>
        /// Log an exception.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="lineNo">Line on which the exception occurred.</param>
        /// <param name="caller">Module that triggered the exception.</param>
        public static void Exception(Exception exception)
        {
            try
            {
                var error = exception.ToString().Replace(Environment.NewLine, string.Concat(Environment.NewLine, "\t"));

                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Error(string.Format("Exception occurred at :: {0}.{1} [Line: {2}]{3}\t{4}"
                        , reflectedType.FullName
                        , stackTrace.GetMethod().Name
                        , stackTrace.GetFileLineNumber()
                        , Environment.NewLine
                        , error));
            }
            catch { }
        }

        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="error">Error to log.</param>
        /// <param name="lineNo">Line on which the error occurred.</param>
        /// <param name="caller">Module that triggered the error.</param>
        public static void Error(string error)
        {
            try
            {
                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Error(string.Format("Error occurred at :: {0}.{1} [Line: {2}]  {3}"
                        , reflectedType.FullName
                        , stackTrace.GetMethod().Name
                        , stackTrace.GetFileLineNumber()
                        , error));
            }
            catch { }
        }

        public static void Data(string refText, dynamic dataObj)
        {
            try
            {
                var objectData = ObjectDumper.Dump(dataObj);

                var stackTrace = new StackTrace(true).GetFrame(1);

                var reflectedType = stackTrace.GetMethod().ReflectedType;
                if (reflectedType != null)
                    log.Error(string.Format("Object dump for '{0}' at :: {1}.{2} [Line: {3}]{4}{5}"
                         , refText
                         , reflectedType.FullName
                         , stackTrace.GetMethod().Name
                         , stackTrace.GetFileLineNumber()
                         , Environment.NewLine
                         , objectData));
            }
            catch { }
        }
    }
}
