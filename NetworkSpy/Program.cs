using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NetworkSpy
{
    internal class Program
    {
        private static void Main()
        {
            var networkBrowser = new NetworkBrowser();
            var root = networkBrowser.getNetworkComputers();

            var pcUser = new List<string>();

            foreach (string computer in root)
            {
                Console.WriteLine("Testing " + computer);
                var current = ReadUser(computer);
                if (!string.IsNullOrEmpty(current))
                {
                    pcUser.Add(current);
                    Console.WriteLine(current);
                }
                else
                {
                    Console.WriteLine("Failed to fetch User");
                }
            }

            var sb = new StringBuilder();

            foreach (var user in pcUser)
            {
                sb.Append(user);
            }
            File.AppendAllText("log.txt", sb.ToString());
            sb.Clear();
            Console.ReadLine();
        }

        /// <summary>
        /// Reading the User- and System name from the target computer
        /// </summary>
        /// <param name="computer">Target computer</param>
        /// <returns>User- and System information</returns>
        private static string ReadUser(string computer)
        {

            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            cmd.Start();

            cmd.StandardInput.WriteLine($"wmic /node: { computer } computersystem get username");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit(500);

            var output = cmd.StandardOutput.ReadToEnd();
            return computer + ": " + output;
        }
    }
}
