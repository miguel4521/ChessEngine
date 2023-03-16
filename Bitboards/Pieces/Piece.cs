namespace ChessEngine;

public abstract class Piece : Bitboard
{
    public bool IsWhite { get; set; }
    
    protected static Bitboard SetOccupancy(int index, int bitsInMask, Bitboard attackMask)
    {
        // occupancy map
        ulong occupancy = 0UL;

        // loop over the range of bits within attack mask
        for (int count = 0; count < bitsInMask; count++)
        {
            // get LS1B index of attacks mask
            int square = attackMask.LSB();

            // pop LS1B in attack map
            attackMask &= attackMask - 1;

            // make sure occupancy is on board
            if ((index & (1 << count)) != 0)
                // populate occupancy map
                occupancy |= (1UL << square);
        }

        // return occupancy map
        return occupancy;
    }

    public abstract List<Move> GenerateMoves(Position position);
}