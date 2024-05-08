namespace Guting.GameOfLife
{
    internal sealed class Program
    {
        private readonly CancellationTokenSource _tokenSource;

        private Program()
        {
            _tokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += CancelKeyPress;
        }

        private static async Task Main(string[] args)
        {
            try
            {
                var p = new Program();
                await p.Run(args);
            }
            catch (Exception e)
            {
                WriteLine($"{e.GetType()}: {e.Message}", ConsoleColor.Red);
            }
        }

        private static void WriteLine(string? value, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _tokenSource.Cancel();
        }

        private async Task Run(string[] args)
        {
            await Run(_tokenSource.Token);
        }

        private async Task Run(CancellationToken cancellationToken)
        {
            var lifeGame = new AllLife();
        }
    }
}