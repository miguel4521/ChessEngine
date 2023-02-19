namespace ChessEngine;

public class Bitboard
{
    // The 64-bit integer that stores the bitboard
    private ulong bits;

    // The constructor that takes a 64-bit integer as an argument
    public Bitboard(ulong bits)
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
                bits |= (1UL << square);
            else
                bits &= ~(1UL << square);
        }
    }

    // The method that returns the number of bits set in the bitboard
    public int Count()
    {
        /*
         * Use the population count algorithm
         * https://arxiv.org/pdf/1611.07612.pdf
         */
        bits -= (bits >> 1) & 0x5555555555555555UL;
        bits = (bits & 0x3333333333333333UL) + ((bits >> 2) & 0x3333333333333333UL);
        bits = (bits + (bits >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
        bits = (bits * 0x0101010101010101UL) >> 56;
        return (int)bits;
    }

    // The method that returns the index of the least significant bit set in the bitboard
    public int LSB()
    {
        // Check if the bitboard is not empty
        if (bits == 0)
        {
            throw new InvalidOperationException("The bitboard is empty.");
        }

        /*
         * Use the bit scan forward algorithm
         * https://arxiv.org/pdf/1611.07612.pdf
         */
        bits ^= bits - 1;
        bits = (bits & 0x5555555555555555UL) + ((bits >> 1) & 0x5555555555555555UL);
        bits = (bits & 0x3333333333333333UL) + ((bits >> 2) & 0x3333333333333333UL);
        bits = (bits + (bits >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
        bits = (bits * 0x0101010101010101UL) >> 56;
        return (int)bits;
    }

    // The method that clears the least significant bit set in the bitboard
    public void ClearLSB()
    {
        // Check if the bitboard is not empty
        if (bits == 0)
        {
            throw new InvalidOperationException("The bitboard is empty.");
        }

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
    public static Bitboard operator >>(Bitboard a, int n)
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

    public void printBitboard()
    {
        for (int i = 0; i < 64; i++)
        {
            if (i % 8 == 0)
                Console.WriteLine();
            Console.Write(this[i] ? "1" : "0");
        }
    }
}