using ChessEngine.Bitboards;
using ChessEngine.Bitboards.Pieces;

namespace ChessEngine;

public struct Position
{
    // An array of bitboards, one for each piece type and color
    public Piece[] pieces;
    public bool whiteToMove = true;

    // The constructor that initializes the bitboards to the initial position
    public Position()
    {
        pieces = new Piece[12];
        pieces[0] = new Pawn(true); // white pawns
        pieces[1] = new Pawn(false); // black pawns
        pieces[2] = new Knight(true); // white knights
        pieces[3] = new Knight(false); // black knights
        pieces[4] = new Bishop(true); // white bishops
        pieces[5] = new Bishop(false); // black bishops
        pieces[6] = new Rook(true); // white rooks
        pieces[7] = new Rook(false); // black rooks
        pieces[8] = new Queen(true); // white queens
        pieces[9] = new Queen(false); // black queens
        pieces[10] = new King(true); // white king
        pieces[11] = new King(false); // black king
    }

    public void MakeMove(Move move)
    {
        // Move the piece
        pieces[move.PieceIndex][move.To] = true;
        pieces[move.PieceIndex][move.From] = false;

        // If the move is a capture, remove the captured piece
        if (move.CapturedPieceIndex != -1)
            pieces[move.CapturedPieceIndex][move.To] = false;
        
        // Switch sides to move
        whiteToMove = !whiteToMove;
    }

    public List<Move> GenerateMoves()
    {
        List<Move> moves = new List<Move>();
        // Generate moves for all pieces, depending on the side to move
        for (int i = whiteToMove ? 0 : 1; i < 12; i += 2)
            moves.AddRange(pieces[i].GenerateMoves(this));
        return moves;
    }

    public int GetPieceIndexAt(int square)
    {
        Dictionary<Type, int> pieceIndex = new Dictionary<Type, int>()
        {
            { typeof(Pawn), 0 },
            { typeof(Knight), 2 },
            { typeof(Bishop), 4 },
            { typeof(Rook), 6 },
            { typeof(Queen), 8 },
            { typeof(King), 10 }
        };
        foreach (Piece piece in pieces)
        {
            if (piece[square])
                return pieceIndex[piece.GetType()];
        }
        
        return -1;
    }

    public Bitboard GetWhitePieces()
    {
        Bitboard whitePieces = new Bitboard();
        for (int i = 0; i < 12; i += 2)
            whitePieces |= pieces[i];
        return whitePieces;
    }

    public Bitboard GetBlackPieces()
    {
        Bitboard blackPieces = new Bitboard();
        for (int i = 1; i < 12; i += 2)
            blackPieces |= pieces[i];
        return blackPieces;
    }

    public Bitboard GetOccupiedSquares()
    {
        Bitboard occupied = new Bitboard();
        foreach (Piece piece in pieces)
            occupied |= piece;
        return occupied;
    }

    public Bitboard GetEmptySquares()
    {
        return GetOccupiedSquares().GetEmptySquares();
    }
}