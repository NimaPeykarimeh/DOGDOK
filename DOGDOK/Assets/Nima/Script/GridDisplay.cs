using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridDisplay))]
public class GridDisplayEditor : Editor
{
    private SerializedProperty gridSize;
    private SerializedProperty cellSize;

    private void OnEnable()
    {
        gridSize = serializedObject.FindProperty("gridSize");
        cellSize = serializedObject.FindProperty("cellSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(gridSize);
        EditorGUILayout.PropertyField(cellSize);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        GridDisplay gridDisplay = (GridDisplay)target;

        // Draw the grid in the Scene view
        DrawGrid(gridDisplay.gridSize, gridDisplay.cellSize, gridDisplay.transform.position);
    }

    private void DrawGrid(Vector2Int gridSize, float cellSize, Vector3 origin)
    {
        float halfSizeX = gridSize.x * cellSize * 0.5f;
        float halfSizeY = gridSize.y * cellSize * 0.5f;

        Handles.color = Color.green;

        // Draw the grid lines along the X-axis
        for (int x = 0; x <= gridSize.x; x++)
        {
            Vector3 start = new Vector3(origin.x - halfSizeX + x * cellSize, origin.y, origin.z - halfSizeY);
            Vector3 end = new Vector3(origin.x - halfSizeX + x * cellSize, origin.y, origin.z + halfSizeY);
            Handles.DrawLine(start, end);
        }

        // Draw the grid lines along the Y-axis
        for (int y = 0; y <= gridSize.y; y++)
        {
            Vector3 start = new Vector3(origin.x - halfSizeX, origin.y, origin.z - halfSizeY + y * cellSize);
            Vector3 end = new Vector3(origin.x + halfSizeX, origin.y, origin.z - halfSizeY + y * cellSize);
            Handles.DrawLine(start, end);
        }
    }
}

public class GridDisplay : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public float cellSize = 1.0f;

    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        Vector3 startPos = transform.position - new Vector3(gridSize.x * cellSize / 2, 0, gridSize.y * cellSize / 2);
        for (int x = 0; x < gridSize.x + 1; x++)
        {
            Gizmos.DrawLine(startPos + new Vector3(x * cellSize, 0, 0), startPos + new Vector3(x * cellSize, 0, gridSize.y * cellSize));
        }

        for (int z = 0; z < gridSize.y + 1; z++)
        {
            Gizmos.DrawLine(startPos + new Vector3(0, 0, z * cellSize), startPos + new Vector3(gridSize.x * cellSize, 0, z * cellSize));
        }
    }
}
