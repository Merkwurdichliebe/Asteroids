using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(ExplodeWhenKilled))]
public class ExplodeWhenKilledEditor : Editor {

	public override void OnInspectorGUI()
	{
		// DrawDefaultInspector();
		ExplodeWhenKilled script = (ExplodeWhenKilled)target;

		// Start horizontal section
		GUILayout.BeginHorizontal();

		// Set minimum width for both items
		EditorGUIUtility.labelWidth = 0;
		EditorGUIUtility.fieldWidth = 10;
		script.destroyObjectOnExplosion = EditorGUILayout.Toggle("Destroy GameObject", script.destroyObjectOnExplosion);

		// Set minimum width for both items
		EditorGUIUtility.labelWidth = 70;
		EditorGUIUtility.fieldWidth = 120;
		script.explosionPrefab = (GameObject)EditorGUILayout.ObjectField("Explosion", script.explosionPrefab, typeof(GameObject), false);

		// End horizontal section
		GUILayout.EndHorizontal();
	}
}
