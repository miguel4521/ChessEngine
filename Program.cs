using ChessEngine.Bitboards.Pieces;

namespace ChessEngine;

internal abstract class Program
{
    private static void Main()
    {
        SlidingPiece.InitAttacks();
        Position pos = new Position();
        pos.GenerateMoves();
    }
}