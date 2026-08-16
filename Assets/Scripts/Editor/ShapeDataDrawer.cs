using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
public class ShapeDataDrawer : Editor
{
    private ShapeData Data => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawControls();
        EditorGUILayout.Space();

        DrawSizeFields();
        EditorGUILayout.Space();

        EnsureBoardValid();

        if (Data.board != null && Data.columns > 0 && Data.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // -------------------------

    private void DrawControls()
    {
        if (GUILayout.Button("Clear Board"))
        {
            Undo.RecordObject(Data, "Clear Board");
            Data.Clear();
            EditorUtility.SetDirty(Data);
        }
    }

    // -------------------------

    private void DrawSizeFields()
    {
        int oldColumns = Data.columns;
        int oldRows = Data.rows;

        Data.columns = EditorGUILayout.IntField("Columns", Data.columns);
        Data.rows = EditorGUILayout.IntField("Rows", Data.rows);

        if ((oldColumns != Data.columns || oldRows != Data.rows) &&
            Data.columns > 0 && Data.rows > 0)
        {
            if (EditorUtility.DisplayDialog(
                "Warning",
                "Resize will reset the board. Continue?",
                "Yes", "No"))
            {
                Undo.RecordObject(Data, "Resize Board");
                Data.CreateNewBoard();
                EditorUtility.SetDirty(Data);
            }
        }
    }

    // -------------------------

    private void EnsureBoardValid()
    {
        if (Data.board == null || Data.board.Length != Data.rows)
        {
            Data.CreateNewBoard();
            return;
        }

        for (int i = 0; i < Data.rows; i++)
        {
            if (Data.board[i] == null ||
                Data.board[i].column == null ||
                Data.board[i].column.Length != Data.columns)
            {
                Data.CreateNewBoard();
                return;
            }
        }
    }

    // -------------------------

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        EditorGUILayout.BeginVertical(tableStyle);

        for (int row = 0; row < Data.rows; row++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int column = 0; column < Data.columns; column++)
            {
                bool current = Data.board[row].column[column];

                bool newValue = EditorGUILayout.Toggle(current, dataFieldStyle, GUILayout.Width(25), GUILayout.Height(25));

                if (newValue != current)
                {
                    Undo.RecordObject(Data, "Edit Cell");
                    Data.board[row].column[column] = newValue;
                    EditorUtility.SetDirty(Data);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}