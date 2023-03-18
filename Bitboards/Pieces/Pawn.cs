namespace ChessEngine.Bitboards.Pieces;

public class Pawn : Piece
{
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
            if (((bitboard >> 7) & NotAFile) != 0) attacks |= bitboard << 7;
            if (((bitboard >> 9) & NotHFile) != 0) attacks |= bitboard << 9;
        }
        // black pawns
        else
        {
            // generate pawn attacks
            if (((bitboard << 7) & NotHFile) != 0) attacks |= bitboard >> 7;
            if (((bitboard << 9) & NotAFile) != 0) attacks |= bitboard >> 9;
        }

        // return attack map
        return attacks;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>(); // A list to store the generated moves

        Bitboard enemyPieces = IsWhite ? position.GetBlackPieces() : position.GetWhitePieces();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
            {
                Bitboard attacks = MaskPawnAttacks(square) & enemyPieces;
                Bitboard captures = attacks & enemyPieces;

                // Add capture moves
                while (captures != 0)
                {
                    moves.Add(new Move(square, captures.LSB(), position));
                    captures &= captures - 1;
                }

                // Calculate single and double pawn pushes
                int singlePush = IsWhite ? square + 8 : square - 8;
                int doublePush = IsWhite ? square + 16 : square - 16;

                if (this[singlePush] == false && position.GetOccupiedSquares()[singlePush] == false)
                {
                    moves.Add(new Move(square, singlePush, position));

                    // Check for double pawn push
                    if ((IsWhite && square is >= 8 and <= 15) || (!IsWhite && square is >= 48 and <= 55))
                    {
                        if (this[doublePush] == false && position.GetOccupiedSquares()[doublePush] == false)
                            moves.Add(new Move(square, doublePush, position));
                    }
                }
            }
        }

        return moves; // Return the list of moves generated
    }
}