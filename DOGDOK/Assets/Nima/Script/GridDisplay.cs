using UnityEditor;
using UnityEngine;

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
