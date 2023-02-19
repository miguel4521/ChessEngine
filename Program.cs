namespace ChessEngine;

internal abstract class Program
{
    private static void Main()
    {
        Position pos = new Position();
        Bitboard whitePawns = pos.bitboards[0]; // get the bitboard for white pawns
        pos.bitboards[2] = new Bitboard(0b0000000000000000000000000000000000000000000000000000000000000000); // set the bitboard for white rooks to zero
    }
}