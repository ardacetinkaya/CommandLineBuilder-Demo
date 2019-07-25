namespace Konsol
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading;
    using System.Threading.Tasks;

    public class PrintCommand : Command
    {
        public PrintCommand(string name, string description = null) : base(name, description)
        {
            this.AddOption(new Option(new[] { "-d", "--destination" })
            {
                Description = "Get print destination. 0:Excel 1:CSV 2:PDF",
                Argument = new Argument<int>("pid"),
            });

            this.AddArgument(new Argument<List<string>>
            {
                Name = "columns",
                Description = $"Add column names for output report.Columns: Name Origin Code Price StockDate Count",
                Arity = ArgumentArity.ZeroOrMore
            });

            this.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole, int>(new Print().Run);
        }

        class Print
        {
            public async Task<int> Run(CancellationToken ct, List<string> columns, IConsole console, int productId)
            {
                try
                {
                    Console.WriteLine("Printing...");
                    return await Task.FromResult(0);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
        }
    }



}
