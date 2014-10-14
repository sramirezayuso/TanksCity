using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kruskal
{
	public static int HEIGHT = 10;
	public static int WIDTH = 10;

	private static Direction OPPOSITE(Direction dir)
	{
		switch (dir) 
		{
			case Direction.E:
				return Direction.W;
			case Direction.W:
				return Direction.E;
			case Direction.N:
				return Direction.S;
			case Direction.S:
				return Direction.N;
		}
		return Direction.N;
	}

	private static int VALUE(Direction dir)
	{
		switch (dir) 
		{
		case Direction.E:
			return 4;
		case Direction.W:
			return 8;
		case Direction.N:
			return 1;
		case Direction.S:
			return 2;
		}
		return 0;
	}

	private static int DX(Direction dir)
	{
		switch (dir) 
		{
		case Direction.E:
			return 1;
		case Direction.W:
			return -1;
		case Direction.N:
			return 0;
		case Direction.S:
			return 0;
		}
		return 0;
	}

	private static int DY(Direction dir)
	{
		switch (dir) 
		{
		case Direction.E:
			return 0;
		case Direction.W:
			return 0;
		case Direction.N:
			return 1;
		case Direction.S:
			return -1;
		}
		return 0;
	}

	public static void SHUFFLE<T>(List<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			Debug.Log(Random.value);
			int k = (int) (Random.value * list.Count);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static void build(float destroyability)
	{
		List<List<Tree>> sets = new List<List<Tree>>();
		for ( int y = 0; y < HEIGHT; y++ ) {
			List<Tree> treeList = new List<Tree>();
			for ( int x = 0; x < WIDTH; ++x ) {
				treeList.Add(new Tree());
			}
			sets.Add(treeList);
		}

		int[,] grid = new int[HEIGHT, WIDTH];
		for ( int j=0; j < HEIGHT; j++ ) {
			for ( int i=0; i < WIDTH; i++ ) {
				grid[j, i] = 0;
			}
		}

		List<Triplet> edges = new List<Triplet> ();
		for ( int y = 0; y < HEIGHT-1; y++ )
		{
			for ( int x = 0; x < WIDTH; x++ )
			{
				if ( y > 0 )
				{
					edges.Add(new Triplet(x, y, Direction.N));
				}
				if ( x > 0 )
				{
					edges.Add(new Triplet(x, y, Direction.W));
				}
			}
		}
		SHUFFLE(edges);


		while ( !(edges.Count == 0) )
		{
			Triplet triplet = edges[0];
			edges.RemoveAt(0);
			int dx = triplet.getX() + DX(triplet.getDir());
			int dy = triplet.getY() + DY(triplet.getDir());

			Tree set1 = (sets[triplet.getY()])[triplet.getX()];
			Tree set2 = (sets[dy])[dx];
			
			if ( !set1.connected(set2) ) {
				set1.connect(set2);
				grid[triplet.getY(), triplet.getX()] |= VALUE(triplet.getDir());
				grid[dy, dx] |= VALUE(OPPOSITE(triplet.getDir()));
			}
		}

		string gridView = "";
		for ( int y = 0; y < HEIGHT; y++ )
		{
			for ( int x = 0; x < WIDTH; x++ )
			{
				gridView += grid[y, x] + " ";
			}
			Debug.Log (gridView);
			gridView = "";
		}

	}
}

