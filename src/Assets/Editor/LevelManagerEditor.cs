using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor {

	private bool useRecursiveDivision;
	private bool useKruskal;
	private float destroyability;
	private float completeness;
	
	public override void OnInspectorGUI() {

		LevelBuilder myTarget = (LevelBuilder)target;

		DrawDefaultInspector();

		if(GUILayout.Button("Build Level")) {

			myTarget.clear();

			if (useRecursiveDivision)
				myTarget.buildWithRecursiveDivision();
			else if (useKruskal)
				myTarget.buildWithKruskal();
			else
				Debug.LogError("This shouldn't happen...");

		}

	}
	
}
