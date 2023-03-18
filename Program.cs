using ChessEngine.Bitboards.Pieces;

namespace ChessEngine;

internal abstract class Program
{
    private static void Main()
    {
        SlidingPiece.InitAttacks();
        Position pos = new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Console.WriteLine(pos.ToFen());
        pos.GenerateMoves();
        pos.MakeMove(new Move(0, 23, pos));
        Console.WriteLine(pos.ToFen());
    }
}