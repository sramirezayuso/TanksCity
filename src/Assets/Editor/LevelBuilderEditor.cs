using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor {
	
	private string[] algorithms = new string[2]{"Iterative Division", "Kruskal"};
	private int selected = 0;
	private float destroyability = 0.2f;
	private float completeness = 0.8f;
	
	public override void OnInspectorGUI() {

		LevelBuilder myTarget = (LevelBuilder)target;

		DrawDefaultInspector();

		selected = GUILayout.SelectionGrid (selected, algorithms, 2);

		GUILayout.Label("Destroyability");
		destroyability = GUILayout.HorizontalSlider (destroyability, 0.0f, 1.0f);
		if (selected == 0) {
			GUILayout.Label("Completeness");
			completeness = GUILayout.HorizontalSlider (completeness, 0.0f, 1.0f);
		}


		if (GUILayout.Button("Build Level")) {

			myTarget.clear();

			if (selected == 0)
				myTarget.buildWithIterativeDivision(completeness, destroyability);
			else if (selected == 1)
				myTarget.buildWithKruskal(destroyability);
			else
				Debug.LogError("This shouldn't happen...");

		}
	}
	
}
