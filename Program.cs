using ChessEngine.Bitboards.Pieces;

namespace ChessEngine;

internal abstract class Program
{
    private static void Main()
    {
        Bishop.InitBishopAttacks();
        Rook.InitRookAttacks();
        Position pos = new Position();
        List<Move> moves = pos.pieces[6].GenerateMoves(pos);
        foreach (Move move in moves)
        {
            Console.WriteLine(move);
        }
    }
}