namespace ChessEngine;

public struct Position
{
    // An array of bitboards, one for each piece type and color
    public Piece[] pieces;

    // The constructor that initializes the bitboards to the initial position
    public Position()
    {
        pieces = new Piece[2];
        pieces[0] = new Pawn( true); // white pawns
        pieces[1] = new Pawn(false); // black pawns
    }

    public Bitboard getOccupiedSquares()
    {
        Bitboard occupied = new Bitboard();
        foreach (Piece piece in pieces)
            occupied |= piece;
        return occupied;
    }

    public Bitboard GetEmptySquares()
    {
        return getOccupiedSquares().GetEmptySquares();
    }
}