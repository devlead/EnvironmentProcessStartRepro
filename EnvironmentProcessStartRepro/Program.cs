using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace EnvironmentProcessStartRepro
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var assemblyLocation = typeof(Program).Assembly.Location;
            switch (args.FirstOrDefault() ?? "1")
            {
                case "1":
                {
                    var startInfo = GetProcessStartInfoWithCaseInsensitiveEnvVars(assemblyLocation);
                    startInfo.EnvironmentVariables["a"] = "B";
                    startInfo.EnvironmentVariables["A"] = "b";

                    var process = new Process {StartInfo = startInfo};
                    if (process.Start())
                    {
                        process.WaitForExit();
                    }
                    break;
                }

                case "2":
                {
                    Console.WriteLine("Hello from the other side");
                    foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                    {
                        Console.WriteLine("Key: {0}, Value: {1}", entry.Key, entry.Value);
                    }

                    try
                    {
                        var startInfo = new ProcessStartInfo(assemblyLocation, "3") {UseShellExecute = false};
                        startInfo.EnvironmentVariables["C"] = "d";
                        new Process
                        {
                            StartInfo = startInfo
                        }.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }


                    try
                    {
                        var startInfo = new ProcessStartInfo(assemblyLocation, "4") { UseShellExecute = false };
                        new Process
                        {
                            StartInfo = startInfo
                        }.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                        break;
                }

                case "3":
                {
                    Console.WriteLine("Don't get here");
                    break;
                    }

                case "4":
                    {
                        Console.WriteLine("But I get here");
                        break;
                    }

                default:
                {
                    Console.WriteLine("Unknon argument: {0}", string.Join("\r\n", args));
                    break;
                }
            }
        }

        private static ProcessStartInfo GetProcessStartInfoWithCaseInsensitiveEnvVars(string assemblyLocation)
        {
            var startInfo = new ProcessStartInfo(assemblyLocation, "2") {UseShellExecute = false};
            startInfo.SetFieldValue("environmentVariables", new CaseSensitiveStringDictionary());
            return startInfo;
        }
    }
}