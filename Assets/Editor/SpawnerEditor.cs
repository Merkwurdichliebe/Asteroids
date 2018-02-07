using UnityEditor;

// [CustomEditor (typeof(Spawner))]
public class SpawnerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Separator();
        EditorGUILayout.Slider("Slider name", 5, 0, 10);
    }
}
