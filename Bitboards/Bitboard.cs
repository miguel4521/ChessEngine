using System.Numerics;

namespace ChessEngine;

public class Bitboard
{
    // The 64-bit integer that stores the bitboard
    protected ulong bits;
    
    protected static readonly Bitboard NotAFile = 0xFEFEFEFEFEFEFEFEUL;
    protected static readonly Bitboard NotHFile = 0x7F7F7F7F7F7F7F7FUL;
    protected static readonly Bitboard NotABFile = 0xFCFCFCFCFCFCFCFCUL;
    protected static readonly Bitboard NotGHFile = 0x3F3F3F3F3F3F3F3FUL;

    public enum Squares
    {
        A1, B1, C1, D1, E1, F1, G1, H1,
        A2, B2, C2, D2, E2, F2, G2, H2,
        A3, B3, C3, D3, E3, F3, G3, H3,
        A4, B4, C4, D4, E4, F4, G4, H4,
        A5, B5, C5, D5, E5, F5, G5, H5,
        A6, B6, C6, D6, E6, F6, G6, H6,
        A7, B7, C7, D7, E7, F7, G7, H7,
        A8, B8, C8, D8, E8, F8, G8, H8
    }

    // The constructor that takes a 64-bit integer as an argument
    private Bitboard(ulong bits)
    {
        this.bits = bits;
    }

    // The constructor that takes no arguments and initializes the bitboard to zero
    public Bitboard() : this(0)
    {
    }

    // The indexer that gets or sets the bit at a given square
    public bool this[int square]
    {
        get
        {
            // Check if the square is valid
            if (square < 0 || square > 63)
                throw new ArgumentOutOfRangeException(nameof(square));

            // Return the bit at the square
            return (bits & (1UL << square)) != 0;
        }
        set
        {
            // Check if the square is valid
            if (square < 0 || square > 63)
                throw new ArgumentOutOfRangeException(nameof(square));

            // Set or clear the bit at the square
            if (value)
                bits |= 1UL << square;
            else
                bits &= ~(1UL << square);
        }
    }

    // The method that returns the number of bits set in the bitboard
    public int Count()
    {
        int count = 0;
        Bitboard n = this;
        while (n > 0)
        {
            if ((n & 1) == 1)
                count++;
            n >>= 1;
        }
        return count;
    }

    // The method that returns the index of the least significant bit set in the bitboard
    public int LSB()
    {
        return BitOperations.TrailingZeroCount(bits);
    }

    // The method that clears the least significant bit set in the bitboard
    public void ClearLSB()
    {
        // Check if the bitboard is not empty
        if (bits == 0)
            throw new InvalidOperationException("The bitboard is empty.");

        // Clear the least significant bit
        bits &= bits - 1;
    }

    // The method that returns a string representation of the bitboard
    public override string ToString()
    {
        // Use a string builder to append the bits
        var sb = new System.Text.StringBuilder();

        // Loop through the ranks from 8 to 1
        for (int rank = 7; rank >= 0; rank--)
        {
            // Loop through the files from A to H
            for (int file = 0; file < 8; file++)
            {
                // Calculate the square index
                int square = rank * 8 + file;

                // Append the bit at the square
                sb.Append(this[square] ? '1' : '0');
                // Append a space after each bit
                sb.Append(' ');
            }

            // Append a newline after each rank
            sb.AppendLine();
        }

        // Return the string
        return sb.ToString();
    }

    public Bitboard GetEmptySquares()
    {
        return new Bitboard(~bits);
    }

    // The implicit conversion operator that converts a bitboard to a 64-bit integer
    public static implicit operator ulong(Bitboard b)
    {
        return b.bits;
    }

    // The implicit conversion operator that converts a 64-bit integer to a bitboard
    public static implicit operator Bitboard(ulong bits)
    {
        return new Bitboard(bits);
    }

    // The bitwise and operator that returns the intersection of two bitboards
    public static Bitboard operator &(Bitboard a, Bitboard b)
    {
        return new Bitboard(a.bits & b.bits);
    }

    // The bitwise or operator that returns the union of two bitboards
    public static Bitboard operator |(Bitboard a, Bitboard b)
    {
        return new Bitboard(a.bits | b.bits);
    }

    // The bitwise xor operator that returns the symmetric difference of two bitboards
    public static Bitboard operator ^(Bitboard a, Bitboard b)
    {
        return new Bitboard(a.bits ^ b.bits);
    }

    // The bitwise not operator that returns the complement of a bitboard
    public static Bitboard operator ~(Bitboard a)
    {
        return new Bitboard(~a.bits);
    }

    // The left shift operator that returns a bitboard shifted left by a given number of bits
    public static Bitboard operator <<(Bitboard a, int n)
    {
        return new Bitboard(a.bits << n);
    }

    // The right shift operator that returns a bitboard shifted right by a given number of bits
    public static Bitboard operator >> (Bitboard a, int n)
    {
        return new Bitboard(a.bits >> n);
    }

    // The equals operator that returns true if two bitboards have the same bits
    public static bool operator ==(Bitboard a, Bitboard b)
    {
        return a.bits == b.bits;
    }

    // The not equals operator that returns false if two bitboards have the same bits
    public static bool operator !=(Bitboard a, Bitboard b)
    {
        return a.bits != b.bits;
    }

    // The equals method that returns true if the object is a bitboard with the same bits
    public override bool Equals(object obj)
    {
        return obj is Bitboard b && this == b;
    }

    // The get hash code method that returns the hash code of the bitboard
    public override int GetHashCode()
    {
        return bits.GetHashCode();
    }
}