namespace ChessEngine.Bitboards.Pieces;

public class Knight : Piece
{
    public Knight(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000042UL : 0x4200000000000000UL;
    }

    // generate knight attacks
    private Bitboard MaskKnightAttacks(int square)
    {
        // result attacks bitboard
        Bitboard attacks = 0UL;

        // piece bitboard
        Bitboard bitboard = 0UL;

        bitboard[square] = true;

        if (((bitboard >> 17) & NotHFile) != 0) attacks |= bitboard >> 17;
        if (((bitboard >> 15) & NotAFile) != 0) attacks |= bitboard >> 15;
        if (((bitboard >> 10) & NotGHFile) != 0) attacks |= bitboard >> 10;
        if (((bitboard >> 6) & NotABFile) != 0) attacks |= bitboard >> 6;
        if (((bitboard << 17) & NotAFile) != 0) attacks |= bitboard << 17;
        if (((bitboard << 15) & NotHFile) != 0) attacks |= bitboard << 15;
        if (((bitboard << 10) & NotABFile) != 0) attacks |= bitboard << 10;
        if (((bitboard << 6) & NotGHFile) != 0) attacks |= bitboard << 6;

        // return attack map
        return attacks;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>(); // A list to store the generated moves

        Bitboard friendlyPieces = IsWhite ? position.GetWhitePieces() : position.GetBlackPieces();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
            {
                Bitboard attacks = MaskKnightAttacks(square);
                Bitboard validMoves = attacks & ~friendlyPieces;

                // Add normal moves and captures
                while (validMoves != 0)
                {
                    moves.Add(new Move(square, validMoves.LSB(), position));
                    validMoves &= validMoves - 1;
                }
            }
        }

        return moves; // Return the list of moves generated
    }
}