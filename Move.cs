namespace ChessEngine;

public struct Move
{
    public int From { get; }  
    public int To { get; }
    public int PieceIndex { get; }
    public int CapturedPieceIndex { get; }

    public Move(int from,int to, Position position )  
    {  
        From=from ;  
        To=to;
        PieceIndex = position.GetPieceIndexAt(from);
        CapturedPieceIndex = position.GetPieceIndexAt(to);
    }
    
    public override string ToString()
    {
        return  From + " " + To;
    }
}