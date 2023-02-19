namespace ChessEngine;

public struct Move
{
    public int From { get; set;}  
    public int To { get; set;}  

    public Move(int from,int to )  
    {  
        From=from ;  
        To=to ;  
    }
}