using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Core.Database
{
    static class SupabaseContext
    {
        private static Client _client;
        public static Client Client { get => _client; private set => _client = value; }

        [ModuleInitializer]
        internal static async void Initialize()
        {
            var url = "https://ezwghqlbausbbiukjipo.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV6d2docWxiYXVzYmJpdWtqaXBvIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTkxMDk4NzcsImV4cCI6MjAxNDY4NTg3N30.r_FxK23wjIu493aGoEzyF34VRLODDU46-_jccOoi3Vw";

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _client = new Client(url, key, options);
            await Client.InitializeAsync();
        }
    }
}
