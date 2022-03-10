using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Camgptt.Logging
{
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
            string fullName;
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
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }
    }
}
