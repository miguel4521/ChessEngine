namespace ChessEngine;

public class Pawn : Piece
{
    public Pawn(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x000000000000FF00UL : 0x00FF000000000000UL;
    }

    public override List<Move> GenerateMoves(Bitboard emptySquares)
    {
        List<Move> moves = new List<Move>(); // A list to store the generated moves
        if (IsWhite) // If the pawn is white
        {
            Bitboard
                singleStep =
                    new Bitboard((bits << 8) & emptySquares); // Shift one rank up and intersect with empty squares
            Bitboard
                doubleStep =
                    new Bitboard((bits << 16) & emptySquares &
                                 (singleStep <<
                                  8)); // Shift two ranks up and intersect with empty squares and single-step moves

            while (singleStep != 0) // While there are single-step moves available
            {
                int to = singleStep.LSB(); // Get the index of the least significant bit set to 1
                int from = to - 8; // Get the index of where the pawn came from

                if ((to & 0x38) == 0x38) // If it is a promotion move (the last rank)
                    moves.Add(new Move(from, to)); // Add a quiet promotion move to the list
                else
                    moves.Add(new Move(from, to)); // Add a quiet move to the list

                singleStep &= singleStep - 1; // Clear the least significant bit set to 1
            }

            while (doubleStep != 0) // While there are double-step moves available 
            {
                int to = doubleStep.LSB(); // Get the index of the least significant bit set to 1 
                int from = to - 16; // Get the index of where the pawn came from

                moves.Add(new Move(from, to)); // Add a double pawn push move to list

                doubleStep &= doubleStep - 1; // Clear teh least significant bit set ot 1 
            }
        }
        else
        {
            Bitboard singleStep = new Bitboard((bits >> 8) & emptySquares);
            Bitboard doubleStep = new Bitboard((bits >> 16) & emptySquares & (singleStep >> 8));

            while (singleStep != 0)
            {
                int to = singleStep.LSB();
                int from = to + 8;

                if ((to & 0x07) == 0x07)
                    moves.Add(new Move(from, to));
                else
                    moves.Add(new Move(from, to));

                singleStep &= singleStep - 1;
            }

            while (doubleStep != 0)
            {
                int to = doubleStep.LSB();
                int from = to + 16;

                moves.Add(new Move(from, to));

                doubleStep &= doubleStep - 1;
            }
        }

        return moves; // Return the list of moves 
    }
}