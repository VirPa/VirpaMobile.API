using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Virpa.Mobile.API.v1 {
    public class Program {
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
