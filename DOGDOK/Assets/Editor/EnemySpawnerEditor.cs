using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnArea))]
public class EnemySpawnerEditor : Editor
{
    private SerializedProperty spawnPosition;
    private SerializedProperty spawnSize;

    private void OnEnable()
    {
        spawnPosition = serializedObject.FindProperty("spawnPosition");
        spawnSize = serializedObject.FindProperty("spawnSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnPosition);
        EditorGUILayout.PropertyField(spawnSize);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        EnemySpawnArea spawner = (EnemySpawnArea)target;
        Transform spawnerTransform = spawner.transform;

        // Update the spawn position with the position of the GameObject itself
        spawner.spawnPosition = spawnerTransform.position;

        Handles.color = Color.green;
        Handles.DrawWireCube(spawner.spawnPosition, spawner.spawnSize);

        EditorGUI.BeginChangeCheck();
        Vector3 newSpawnPosition = Handles.PositionHandle(spawner.spawnPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spawner, "Change Spawn Position");
            spawner.spawnPosition = newSpawnPosition;
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newSpawnSize = Handles.ScaleHandle(spawner.spawnSize, spawner.spawnPosition, Quaternion.identity, HandleUtility.GetHandleSize(spawner.spawnPosition));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spawner, "Change Spawn Size");
            spawner.spawnSize = newSpawnSize;
        }
    }
}
