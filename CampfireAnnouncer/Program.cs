using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using ConsoleApplication;

namespace CampfireAnnouncer {
    public class Program : CommandLineProgram<Program, Arguments> {
        static void Main(string[] args) {
            new Program().RunProgram(args);
        }

        protected override void Run(Arguments arguments) {
            Client.CampfireClient client = new Client.CampfireClient(arguments.Subdomain, arguments.Room, arguments.Token);
            client.PostTextMessage(arguments.Message);
            Out(client.LastResponse);
        }

        protected override void Validate(Arguments arguments) {
            return;
        }

        protected override void Exit(Arguments arguments) {
            if (arguments.WaitForExit) WaitForExit();
        }
    }

    public class Arguments {

        [Option(@"s", @"subdomain", HelpText = @"The Campfire subdomain to be used", Required=true)]
        public string Subdomain = null;

        [Option(@"r", @"room", HelpText = @"The name of the Campfire room", Required = true)]
        public string Room = null;

        [Option(@"t", @"token", HelpText = @"The specified user's API token (instead of password)", Required = true)]
        public string Token = null;

        [Option(@"w", @"wait", HelpText = @"If TRUE, then wait for <Enter> before ending the program")]
        public bool WaitForExit = false;

        [Option(@"m", @"message", HelpText = @"The message to be posted", Required = true)]
        public string Message = @"";

        public override string ToString() {
            return string.Format(@"Subdomain = {0}, Room = {1}, Token = {2}, Wait for Exit = {3}",
                this.Subdomain, this.Room, this.Token, this.WaitForExit);
        }
    }
}
