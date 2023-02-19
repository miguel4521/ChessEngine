namespace ChessEngine;

public struct Position
{
    // An array of bitboards, one for each piece type and color
    public Bitboard[] bitboards;

    // The constructor that initializes the bitboards to the initial position
    public Position()
    {
        bitboards = new Bitboard[12];
        bitboards[0] = new Bitboard(0b0000000011111111000000000000000000000000000000000000000000000000); // white pawns
        bitboards[1] = new Bitboard(0b0000000000000000000000000000000000000000000000001111111100000000); // black pawns
        bitboards[2] = new Bitboard(0b1000000100000000000000000000000000000000000000000000000000000000); // white rooks
        bitboards[3] = new Bitboard(0b0000000000000000000000000000000000000000000000000000000010000001); // black rooks
        bitboards[4] =
            new Bitboard(0b0100001000000000000000000000000000000000000000000000000000000000); // white knights
        bitboards[5] =
            new Bitboard(0b0000000000000000000000000000000000000000000000000000000001000010); // black knights
        bitboards[6] =
            new Bitboard(0b0010010000000000000000000000000000000000000000000000000000000000); // white bishops
        bitboards[7] =
            new Bitboard(0b0000000000000000000000000000000000000000000000000000000000100100); // black bishops
        bitboards[8] = new Bitboard(0b0000100000000000000000000000000000000000000000000000000000000000); // white queen
        bitboards[9] = new Bitboard(0b0000000000000000000000000000000000000000000000000000000000001000); // black queen
        bitboards[10] = new Bitboard(0b0001000000000000000000000000000000000000000000000000000000000000); // white king
        bitboards[11] = new Bitboard(0b0000000000000000000000000000000000000000000000000000000000010000); // black king
    }

    // A method that returns all empty squares
    public Bitboard GetEmptySquares()
    {
        Bitboard emptySquares = new Bitboard();
        foreach (var bitboard in bitboards)
            emptySquares |= bitboard;
        return ~emptySquares;
    }

    // Gets bitboard of legal moves for pawns
}