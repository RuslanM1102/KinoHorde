using System.Threading.Tasks;
using KinopoiskParser;
using System;
using DesktopApplication.Core.Database;
using Supabase;
using Supabase.Gotrue;

namespace ConsoleDebug
{
    public partial class Program
    {
        static async Task Main(string[] args)
        {
            var client = SupabaseContext.Client;
            client.Auth.AddDebugListener((str,e) => Console.WriteLine(str));

            var auth = await client.Auth.SignIn(Constants.Provider.Discord);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = auth.Uri.AbsoluteUri, UseShellExecute = true });
        }
    }
}