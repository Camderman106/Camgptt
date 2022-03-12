using Camgptt.Logging;
using Camgptt.Logging.LoggerComponents;
using Camgptt.Logging.LoggerComponents.ConsoleComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class Program
    {

        public static void Main(String[] args)
        {
            Logger Log = Logger.GetLogger()
                .AttachConsoleLogger();
            

            using (var bar = Log.AddProgressBar("Loop 0", 100))
            {

                for (int i = 0; i < 100; i++)
                {
                    Log.Info(i.ToString());
                    Thread.Sleep(10);
                    using (var ibar = Log.AddProgressBar("Loop 1", 100))
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            //Log.Info(j.ToString());
                            Thread.Sleep(1);
                            ibar.Tick();
                        }
                        using (var iibar = Log.AddProgressBar("Loop 2", 100))
                        {

                            for (int j = 0; j < 50; j++)
                            {
                                //Log.Info("Log Message Test");
                                Thread.Sleep(1);
                                iibar.Tick();
                            }
                            Log.Warn("Inner loop done");
                        }
                        for (int j = 0; j < 100; j++)
                        {
                            //Log.Info(j.ToString());
                            Thread.Sleep(1);
                            ibar.Tick();
                        }
                        Log.Warn("Inner loop done");
                    }
                    using (var ibar = Log.AddProgressBar("Loop 1.5", 100))
                    {

                        for (int j = 0; j < 100; j++)
                        {
                            //Log.Info(j.ToString());
                            Thread.Sleep(1);
                            ibar.Tick();
                        }
                        Log.Warn("Inner loop done");
                    }
                    bar.Tick();
                }
            }

            Console.ReadLine();
        }
    }
}
