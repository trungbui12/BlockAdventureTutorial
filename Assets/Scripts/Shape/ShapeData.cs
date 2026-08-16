using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;

        public Row(int size)
        {
            column = new bool[size];
        }

        public void ClearRow()
        {
            if (column == null) return;

            for (int i = 0; i < column.Length; i++)
            {
                column[i] = false;
            }
        }
    }

    public int columns = 0;
    public int rows = 0;
    public Row[] board;

    public void Clear()
    {
        if (board == null) return;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != null)
                board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];

        for (int i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }

    // ✅ VALIDATE (RẤT QUAN TRỌNG)
    public bool IsValid()
    {
        if (board == null || board.Length != rows)
            return false;

        foreach (var row in board)
        {
            if (row == null || row.column == null || row.column.Length != columns)
                return false;
        }

        return true;
    }
}