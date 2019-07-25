namespace Konsol
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    public class ReportCommand : Command
    {
        public ReportCommand(string name, string description = null) : base(name, description)
        {
            this.AddOption(new Option(new[] { "-pid", "--product-id" })
            {
                Description = "Get id of the product",
                Argument = new Argument<int>("pid"),
            });

            this.AddArgument(new Argument<List<string>>
            {
                Name = "columns",
                Description = $"Add column names for output report.Columns: Name - Origin - Code - Price - StockDate - Count",
                Arity = ArgumentArity.ZeroOrMore
            });

            this.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole, int>(new Report().Generate);
        }

        class Report
        {
            public async Task<int> Generate(CancellationToken ct, List<string> columns, IConsole console, int productId)
            {
                try
                {
                    if (productId <= 0)
                    {
                        Console.WriteLine("Invalid product");
                        return 1;
                    }

                    Console.WriteLine("Press q to quit.");
                    return await FetchReport(productId, columns);
                }
                catch (Exception)
                {
                    return 1;
                }
            }

            private async Task<int> FetchReport(int productId, List<string> columns = null)
            {
                var reset = new ManualResetEvent(false);
                var isFinished = false;

                Task reportTask = new Task(() =>
                {
                    try
                    {
                        //Some simple loading proccess demo
                        Console.WriteLine($"Report is for: {productId.ToString() }");
                        if (columns != null && columns.Any())
                        {
                            Console.WriteLine($"Columns: { string.Join(";", columns.ToArray())}");
                        }
                        Console.Write("Loading...");
                        for (int i = 0; i < 25; i++)
                        {
                            Thread.Sleep(2000);
                            Console.Write(".");
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);

                        }
                        Console.WriteLine("Completed.");
                        isFinished = true;
                        //Process end

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Some error is occured.");
                    }
                    finally
                    {
                        reset.Set();
                    }
                });

                reportTask.Start();


                while (!isFinished)
                {
                    if (reset.WaitOne(250))
                    {
                        return 0;
                    }
                    if (Console.KeyAvailable)
                    {
                        break;
                    }
                    ConsoleKey cmd = Console.ReadKey(true).Key;
                    if (cmd == ConsoleKey.Q)
                    {
                        break;
                    }
                }
                return await Task.FromResult(0);
            }
        }
    }
}
