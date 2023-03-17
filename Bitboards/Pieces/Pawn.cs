namespace ChessEngine.Bitboards.Pieces;

public class Pawn : Piece
{
    private readonly Bitboard _notAFile = 0xFEFEFEFEFEFEFEFEUL;
    private readonly Bitboard _notHFile = 0x7F7F7F7F7F7F7F7FUL;
    public Pawn(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x000000000000FF00UL : 0x00FF000000000000UL;
    }
    
    private Bitboard MaskPawnAttacks(int square)
    {
        // result attacks bitboard
        Bitboard attacks = 0UL;

        // piece bitboard
        Bitboard bitboard = 0UL;

        bitboard[square] = true;

        // white pawns
        if (IsWhite)
        {
            // generate pawn attacks
            if (((bitboard >> 7) & _notAFile) != 0) attacks |= bitboard >> 7;
            if (((bitboard >> 9) & _notHFile) != 0) attacks |= bitboard >> 9;
        }
        // black pawns
        else
        {
            // generate pawn attacks
            if (((bitboard << 7) & _notHFile) != 0) attacks |= bitboard << 7;
            if (((bitboard << 9) & _notAFile) != 0) attacks |= bitboard << 9;    
        }
    
        // return attack map
        return attacks;
    }

    public override List<Move> GenerateMoves(Position position)
    {

        return null;
    }
}