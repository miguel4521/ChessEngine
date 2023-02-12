namespace ChessEngine;

internal abstract class Program
{
    private static void Main()
    {
        var bbc = new Bbc();
        // init leaper piece attacks
        bbc.InitLeapersAttacks();

        Console.WriteLine(bbc.GenerateMagicNumber());
    }
}