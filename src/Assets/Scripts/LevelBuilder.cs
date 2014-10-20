using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildingBlock {
	EMPTY, WALL, OPENING, HORI, VERT
}

public class LevelBuilder : MonoBehaviour {

	public GameObject root;

	private BuildingBlock[,] matrix;
	private static int size = 20;
	private List<int> horizontals;
	private List<int> verticals;

	public void buildWithRecursiveDivision(float completeness, float destroyability) {

		// initialize matrix
		matrix = new BuildingBlock[size, size];
		for (int i=0; i<size ;i++) {
			for (int j=0; j<size ;j++) {
				if (i==0 || i==size-1 || j==0 || j==size-1)
					matrix[j,i] = BuildingBlock.WALL;
				else if ( (i==1 && j==1) || (i==1 && j==size-1) || (i==size-1 && j==1) || (i==size-1 && j==size-1) )
					matrix[j,i] = BuildingBlock.OPENING;
				else if (i==1 || i==size-1)
					matrix[j,i] = BuildingBlock.VERT;
				else if (j==1 || j==size-1)
					matrix[j,i] = BuildingBlock.HORI;
				else
					matrix[j,i]= BuildingBlock.EMPTY;
			}
		}

		// initialize additional data structures
		horizontals = new List<int>();
		verticals = new List<int>();
		for (int i=2; i<size-2 ;i++) {
			horizontals.Add(i);
			verticals.Add(i);
		}

		// constructing the maze
		while (shouldContinue(completeness)) {
			bool dir = (Random.value > 0.5f)?true:false;
			List<int> list = (dir)?horizontals:verticals;
			if (list.Count != 0) {
				int sel = list[Random.Range(0, list.Count)];
				if (dir)
					addHWall(sel);
				else
					addVWall(sel);
			}
		}
	}

	private void addHWall(int y) {
		int dir = (Random.value > 0.5f)?-1:1;
		int start = -1, end = -1;
		bool flag = true;

		// build walls
		if(dir == -1) { // backwards
			for(int i=size-1; flag && i>=0 ;i--) {
				if (end == -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT))
					end = i;

				if (end != -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT))
					matrix[y,i] = BuildingBlock.WALL;

				if (end != -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT)) {
					start = i+1;
					flag = false;
				}
			}
		} else { // forward
			for (int i=0; flag && i<size ;i++) {
				if (start == -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT))
					start = i;
				
				if (start != -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT))
					matrix[y,i] = BuildingBlock.WALL;
				
				if (start != -1 && (matrix[y,i] == BuildingBlock.EMPTY || matrix[y,i] == BuildingBlock.VERT)) {
					end = i-1;
					flag = false;
				}
			}
		}


		// add hole
		int hole = Random.Range (start, end);
		matrix [y-1, hole] = BuildingBlock.OPENING;
		matrix [y, hole] = BuildingBlock.OPENING;
		matrix [y+1, hole] = BuildingBlock.OPENING;

		// update matrix to reflect available positions
		for (int i=start; i<=end ;i++) {
			if (matrix[y-1, i] == BuildingBlock.EMPTY)
				matrix[y-1, i] = BuildingBlock.HORI;
			else if (matrix[y-1, i] == BuildingBlock.VERT || matrix[y-1, i] == BuildingBlock.HORI)
				matrix[y-1, i] = BuildingBlock.OPENING;

			if (matrix[y+1, i] == BuildingBlock.EMPTY)
				matrix[y+1, i] = BuildingBlock.HORI;
			else if (matrix[y+1, i] == BuildingBlock.VERT || matrix[y+1, i] == BuildingBlock.HORI)
				matrix[y+1, i] = BuildingBlock.OPENING;
		}
	}

	private void addVWall(int x) {
		int dir = (Random.value > 0.5f)?-1:1;
		int start = -1, end = -1;
		bool flag = true;
		
		// build walls
		if(dir == -1) { // backwards
			for(int j=size-1; flag && j>=0 ;j--) {
				if (end == -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					end = j;
				
				if (end != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					matrix[j,x] = BuildingBlock.WALL;
				
				if (end != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI)) {
					start = j+1;
					flag = false;
				}
			}
		} else { // forward
			for (int j=0; flag && j<size ;j++) {
				if (start == -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					start = j;
				
				if (start != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					matrix[j,x] = BuildingBlock.WALL;
				
				if (start != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI)) {
					end = j-1;
					flag = false;
				}
			}
		}
		
		
		// add hole
		int hole = Random.Range (start, end);
		matrix [hole, x-1] = BuildingBlock.OPENING;
		matrix [hole, x] = BuildingBlock.OPENING;
		matrix [hole, x+1] = BuildingBlock.OPENING;
		
		// update matrix to reflect available positions
		for (int j=start; j<=end ;j++) {
			if (matrix[j, x-1] == BuildingBlock.EMPTY)
				matrix[j, x-1] = BuildingBlock.VERT;
			else if (matrix[j, x-1] == BuildingBlock.VERT || matrix[j, x-1] == BuildingBlock.HORI)
				matrix[j, x-1] = BuildingBlock.OPENING;
			
			if (matrix[j, x+1] == BuildingBlock.EMPTY)
				matrix[j, x+1] = BuildingBlock.VERT;
			else if (matrix[j, x+1] == BuildingBlock.VERT || matrix[j, x+1] == BuildingBlock.HORI)
				matrix[j, x+1] = BuildingBlock.OPENING;
		}
	}

	private bool shouldContinue(float completeness) {
		int used = 0, unused = 0;
		float ratio = 0;
		bool flag = false;
		List<int> toRemoveV = new List<int> ();
		List<int> toRemoveH = new List<int> ();

		foreach (int y in verticals) {
			for (int i=0; i<size ;i++) {
				if (matrix[y, i] == BuildingBlock.EMPTY && !flag) {
					flag = true;
				} else if (matrix[y, i] == BuildingBlock.HORI) {
					if (!flag)
						flag = true;
					unused++;
				} else if (matrix[y, i] == BuildingBlock.OPENING || matrix[y, i] == BuildingBlock.WALL) {
					used++;
				}
			}
			if (!flag)
				toRemoveV.Add(y);
			else
				flag = false;
		}

		foreach (int x in horizontals) {
			for (int j=0; j<size ;j++) {
				if (matrix[j, x] == BuildingBlock.EMPTY || matrix[j, x] == BuildingBlock.VERT) {
					if (!flag)
						flag = true;
					unused++;
				}
			}
			if (!flag)
				toRemoveH.Add(x);
			else
				flag = false;
		}

		foreach (int y in toRemoveV)
			verticals.Remove(y);

		foreach (int x in toRemoveH)
			horizontals.Remove(x);

		if (used + unused != size*size) {
			Debug.LogError("Matrix of " + (size*size) + " elements.\nUsed = " + used + ", Unused = " + unused );
		} else {
			ratio = used / (used+unused);
		}

		return (ratio < completeness);
	}

	public void buildWithKruskal(float destroyability) {

	}

	public void clear() {

	}

}
