using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Camgptt.Logging
{/// <summary>
/// This interface exists to allow a Logger to contain other loggers as components, 
/// to allow to NameOfCallingClass to be passed through from the underlying class,
/// to allow exceptions to be directly logged with the optiion of overriding the exception "Message"
/// the methods are seperated so they can be used as Logger.Info(""); etc which is readable
/// </summary>
    internal interface I6LoggerComponent : IDisposable
    {
        internal void Info(string nameOfCallingClass, string message);
        internal void Info(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Fatal(string nameOfCallingClass, string message);
        internal void Fatal(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Error(string nameOfCallingClass, string message);
        internal void Error(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Warn(string nameOfCallingClass, string message);
        internal void Warn(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Debug(string nameOfCallingClass, string message);
        internal void Debug(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Trace(string nameOfCallingClass, string message);
        internal void Trace(string nameOfCallingClass, Exception exception, string? message = null);
        internal void Raw(string nameOfCallingClass, string message);

        public static string NameOfCallingClass()
        {
            string Name;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                Name = declaringType.Name;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return Name;
        }
    }
}
