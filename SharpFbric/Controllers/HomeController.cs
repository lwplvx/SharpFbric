using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpFbric.Models;

namespace SharpFbric.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Ssh()
        {
            var str = OpenAndRunCommand();

            ViewBag.comandResultStr = str;
          //  SendFIleOverSFTP();
            return View();
        }
        private string OpenAndRunCommand()
        {
            Renci.SshNet.SshClient ssh = new Renci.SshNet.SshClient("119.23.71.29", "root", "Zyp885299");
            ssh.Connect();

            var resultStr = string.Empty;
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "whoami")}";
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "ls")}";
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "ps")}";
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "top")}";
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "pwd")}";
            resultStr = $"{resultStr}\r\n{RunCommand(ssh, "exit")}";
            // ssh.Disconnect();
            return resultStr;
        }
        private string RunCommand(Renci.SshNet.SshClient ssh, string line)
        {
            var resultStr = string.Empty;
            var cmd = ssh.RunCommand(line);
            if (!string.IsNullOrWhiteSpace(cmd.Error))
                resultStr += cmd.Error;
            else
                resultStr += cmd.Result;

            return resultStr;
        }

        private void SendFIleOverSFTP()
        {
            Renci.SshNet.SftpClient sftp = new Renci.SshNet.SftpClient("119.23.71.29", "root", "Zyp885299");
            sftp.Connect();
            FileInfo fi = new FileInfo(@"/Users/luweiping/Downloads/nginx-1.12.0-1.el7.ngx.x86_64.rpm");
            var allLength = fi.Length;
            sftp.UploadFile(new System.IO.FileStream(fi.FullName,
                System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite),
                "/home/parallels/Downloads/nginx-1.12.0-1.el7.ngx.x86_64.rpm", (pro) =>
                {
                    Console.WriteLine((pro * 1.0d / allLength * 1.0d).ToString("P"));
                });
            Console.WriteLine("finished.");
            while (true)
            {
                System.Threading.Thread.Sleep(500);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
