﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildingBlock {
	EMPTY, WALL, OPENING
}

public class LevelBuilder : MonoBehaviour {

	public GameObject root;

	private BuildingBlock[][] matrix;
	private static int size = 20;
	private List<int> horizontals;
	private List<int> verticals;

	public void buildWithRecursiveDivision(float completeness, float destroyability) {

		// initialize matrix
		matrix = new BuildingBlock[size] [size];
		for (int i=0; i<size ;i++) {
			for (int j=0; j<size ;j++) {
				if (i==0 || i==size-1 || j==0 || j==size-1)
					matrix[j][i]= BuildingBlock.WALL;
				else
					matrix[j][i]= BuildingBlock.EMPTY;
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
		while (shouldContinue()) {
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
		if(dir == -1) {
			for(int i=size-1; flag && i>=0 ;i--) {
				if (end == -1 && matrix[y][i] == BuildingBlock.EMPTY)
					end = i;

				if (end != -1 && matrix[y][i] == BuildingBlock.EMPTY)
					matrix[y][i] = BuildingBlock.WALL;

				if (end != -1 && matrix[y][i] != BuildingBlock.EMPTY) {
					start = i+1;
					flag = false;
				}
			}
		} else {
			for(int i=0; flag && i<size ;i++) {
				if (start == -1 && matrix[y][i] == BuildingBlock.EMPTY)
					start = i;
				
				if (start != -1 && matrix[y][i] == BuildingBlock.EMPTY)
					matrix[y][i] = BuildingBlock.WALL;
				
				if (start != -1 && matrix[y][i] != BuildingBlock.EMPTY) {
					end = i-1;
					flag = false;
				}
			}
		}

		// add hole and update structures
		int hole = Random.Range (start, end);
		matrix [y-1] [hole] = BuildingBlock.OPENING;
		matrix [y] [hole] = BuildingBlock.OPENING;
		matrix [y+1] [hole] = BuildingBlock.OPENING;

		//TODO...
	}

	private void addVWall(int x) {
			return ;
	}

	private bool shouldContinue() {
		return true;
	}

	public void buildWithKruskal(float destroyability) {

	}

	public void clear() {

	}

}
