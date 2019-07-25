namespace Konsol
{
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;

    class Program
    {
        private static Task<int> Main(string[] args)
        {
            ReportCommand rcmd = new ReportCommand("report");
            PrintCommand pcmd = new PrintCommand("print");

            Parser parser = new CommandLineBuilder(new Command("konsol"))
                .AddCommand(rcmd)
                .AddCommand(pcmd)
                .UseHelp()
                .UseDefaults()
                .Build();

            return parser.InvokeAsync(args);
        }
    }
}
