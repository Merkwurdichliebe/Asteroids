using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor {

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("Testing", MessageType.Info);
		DrawDefaultInspector();
	}
}
