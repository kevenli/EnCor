using System;
using System.Collections.Generic;
using System.Text;
using EnCor;
using System.ServiceProcess;

namespace EnCor.AppRuntime
{
    class Program
    {
        static readonly string ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        const string CMD_Install = "/install";
        const string CMD_Uninstall = "/uninstall";
        const string CMD_StartService = "/cs";
        const string CMD_Help1 = "/?";
        const string CMD_Help2 = "/help";

        const string DEFAULT_ServiceName = "EnCor Service";

        static void Main(string[] args)
        {
            List<string> argList = new List<string>(args);
            try
            {
                if (args.Length == 0)
                {
                    RunAsConsole();
                }
                else if (argList[0].ToLower() == CMD_Install)
                {// to install : encorapp.exe /install servicename:[servicename]
                    string serviceName = GetServiceNameFromArgs(argList);
                    if (string.IsNullOrEmpty(serviceName))
                    {
                        serviceName = DEFAULT_ServiceName;
                    }

                    InstallService(serviceName);
                }
                else if (argList[0].ToLower() == CMD_Uninstall)
                {// to uninstall 
                    string serviceName = GetServiceNameFromArgs(argList);
                    if (string.IsNullOrEmpty(serviceName))
                    {
                        serviceName = DEFAULT_ServiceName;
                    }

                    UnstaillService(serviceName);

                }
                else if (argList[0].ToLower() == CMD_StartService)
                {// to start service

                    RunAsService();
                }
                else if (argList[0].ToLower() == CMD_Help1 || argList[0].ToLower() == CMD_Help2)
                {// show help
                    ShowHelp();
                }
                
            }
            catch (CommandException)
            {
                ShowHelp();
            }
        }

        static string GetServiceNameFromArgs(List<string> args)
        {
            foreach (string s in args)
            {
                if (s.ToLower().StartsWith("servicename:"))
                {
                    return s.Split(':')[1];
                }
            }
            return null;
        }

        static void InstallService(string serviceName)
        {
            //string serviceDispName = "EnCor Runtime";

            ServiceInstaller c = new ServiceInstaller();

            try
            {
                if (c.InstallService(ApplicationPath + " /cs", serviceName, serviceName))
                {
                    Console.WriteLine("Serivce {0} installed successful.", serviceName);
                }
                else
                {
                    Console.WriteLine("Service {0} install failed.", serviceName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when installing the service: \r\n{0}", ex);
            }
        }

        static void UnstaillService(string serviceName)
        {
            ServiceInstaller c = new ServiceInstaller();
            try
            {
                if (c.UnInstallService(serviceName))
                {
                    Console.WriteLine("Serivce {0} uninstalled successful.", serviceName);
                }
                else
                {
                    Console.WriteLine("Service {0} uninstall failed.", serviceName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when uninstalling the service {0}: \r\n{1}",serviceName, ex);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("EnCorApp Usage:");
            Console.WriteLine("");
            Console.WriteLine("    EnCorApp : Run application in console mode.");
            Console.WriteLine("    EnCorApp /? : Show this usage ");
            Console.WriteLine("    EnCorApp /install [servicename:{ServiceName}] : install application as service.");
            Console.WriteLine("    EnCorApp /uninstall [servicename:{ServiceName}] : uninstall service");
        }

        static void RunAsService()
        {
            ServiceBase.Run(new StartService());
        }

        static void RunAsConsole()
        {
            try
            {
                Runtime.Startup();

                Console.WriteLine("EXIT to exit.");

                string input;
                do
                {
                    input = Console.ReadLine();

                }
                while (input.ToLower() != "exit");

                Console.WriteLine("EnCor exiting...");
                Runtime.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when starting EnCorApp: {0}", ex);
            }
        }
    }
}
