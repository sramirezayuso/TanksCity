using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildingBlock {
	EMPTY, WALL, OPENING, HORI, VERT
}

public class IterativeDivision {

	private BuildingBlock[,] matrix;
	private int size;
	private List<int> horizontals;
	private List<int> verticals;

	public IterativeDivision(BuildingBlock[,] matrix, int size) {
		this.matrix = matrix;
		this.size = size;
	}
	
	public void build(float completeness, float destroyability) {
		
		// initialize additional data structures
		horizontals = new List<int>();
		verticals = new List<int>();
		for (int i=2; i<size-2 ;i++) {
			horizontals.Add(i);
			verticals.Add(i);
		}
		
		// constructing the maze
		int hAdded=1, vAdded=1;
		while (shouldContinue(completeness)) {
			bool dir = (Random.Range(0,hAdded+vAdded) > hAdded)?true:false;
			List<int> list = (dir)?horizontals:verticals;
			if (list.Count != 0) {
				int sel = list[Random.Range(0, list.Count)];
				if (dir) {
					addHWall(sel);
					hAdded++;
				} else {
					addVWall(sel);
					vAdded++;
				}
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
				else if (end != -1 && (matrix[y,i] == BuildingBlock.WALL || matrix[y,i] == BuildingBlock.OPENING || matrix[y,i] == BuildingBlock.HORI)) {
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
				else if (start != -1 && (matrix[y,i] == BuildingBlock.WALL || matrix[y,i] == BuildingBlock.OPENING || matrix[y,i] == BuildingBlock.HORI)) {
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
		if(dir == -1) { // upwards
			for(int j=size-1; flag && j>=0 ;j--) {
				if (end == -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					end = j;
				
				if (end != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					matrix[j,x] = BuildingBlock.WALL;
				else if (end != -1 && (matrix[j,x] == BuildingBlock.WALL || matrix[j,x] == BuildingBlock.OPENING || matrix[j,x] == BuildingBlock.VERT)) {
					start = j+1;
					flag = false;
				}
			}
		} else { // downwards
			for (int j=0; flag && j<size ;j++) { 
				if (start == -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					start = j;
				
				if (start != -1 && (matrix[j,x] == BuildingBlock.EMPTY || matrix[j,x] == BuildingBlock.HORI))
					matrix[j,x] = BuildingBlock.WALL;
				else if (start != -1 && (matrix[j,x] == BuildingBlock.WALL || matrix[j,x] == BuildingBlock.OPENING || matrix[j,x] == BuildingBlock.VERT)) {
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
		horizontals = new List<int> ();
		verticals = new List<int> ();
		
		bool hFlag;
		bool[] vFlag = new bool[size];
		for (int j=0; j<size ;j++) 
			vFlag[j] = false;
		
		for (int j=0; j<size ;j++) {
			hFlag = false;
			for (int i=0; i<size ;i++) {
				if (matrix[j,i] == BuildingBlock.EMPTY) {
					hFlag = true;
					vFlag[i] = true;
					unused++;
				} else if (matrix[j,i] == BuildingBlock.HORI) {
					vFlag[i] = true;
					unused++;
				} else if (matrix[j,i] == BuildingBlock.VERT) {
					hFlag = true;
					unused++;
				} else {
					used++;
				}
			}
			if (hFlag)
				horizontals.Add(j);
		}
		
		for (int j=0; j<size ;j++)
			if (vFlag[j])
				verticals.Add(j);
		
		if (used + unused != size*size) {
			Debug.LogError("Matrix of " + (size*size) + " elements.\nUsed = " + used + ", Unused = " + unused );
		} else {
			ratio = (float)used / (used+unused);
		}
		
		return (ratio < completeness);
	}

}

public class Kruskal
{
	public static int HEIGHT = 10;
	public static int WIDTH = 10;
	
	private static List<List<Tree>> sets;
	private static int[,] grid;
	private static List<Triplet> edges;
	
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
			return -1;
		case Direction.S:
			return 1;
		}
		return 0;
	}
	
	public static void SHUFFLE<T>(List<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = (int) (Random.value * list.Count);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	
	public static BuildingBlock[,] build(float destroyability)
	{
		initialise ();
		generateWalls ();
		
		int[,] maze = new int[HEIGHT * 2, WIDTH * 2];
		for ( int y = 0; y < HEIGHT; y++ )
		{
			for ( int x = 0; x < WIDTH; x++ )
			{
				maze[2*y,2*x] = 0;
				
				if ( (grid[y,x] & VALUE(Direction.S)) != 0 )
				{
					maze[(2*y)+1, 2*x] = 0;
				}
				else
				{
					maze[(2*y)+1, 2*x] = 1;
				}
				if ( (grid[y, x] & VALUE(Direction.E)) != 0 )
				{
					maze[2*y, (2*x)+1] = 0; 
					if ( ((grid[y, x] | grid[y, x+1]) & VALUE(Direction.S)) != 0 ) 
					{ 
						maze[(2*y)+1, (2*x)+1] = 0; 
					}
					else
					{
						maze[(2*y)+1, (2*x)+1] = 1; 
					}
				} 
				else 
				{
					maze[2*y, (2*x)+1] = 1; 
					maze[(2*y)+1, (2*x)+1] = 0; 
				}
			}
		}

		BuildingBlock[,] matrix = new BuildingBlock[HEIGHT*2, WIDTH*2];
		for ( int y = 0; y < HEIGHT*2; y++ )
		{
			for ( int x = 0; x < WIDTH*2; x++ )
			{
				if(maze[y, x] == 1)
				{
					matrix[y, x] = BuildingBlock.WALL;
				}
				else
				{
					matrix[y, x] = BuildingBlock.EMPTY;
				}
				if(x == 0 || y == 0 || x == (WIDTH*2) - 1 || y == (HEIGHT*2) -1)
				{
					matrix[y, x] = BuildingBlock.WALL;
				}
			}
		}
		return matrix;

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
		
		string mazeView = "";
		for ( int y = 0; y < HEIGHT*2; y++ )
		{
			for ( int x = 0; x < WIDTH*2; x++ )
			{
				mazeView += matrix[y, x] + " ";
			}
			Debug.Log (mazeView);
			mazeView = "";
		}
	}
	
	private static void initialise()
	{
		sets = new List<List<Tree>>();
		for ( int y = 0; y < HEIGHT; y++ ) {
			List<Tree> treeList = new List<Tree>();
			for ( int x = 0; x < WIDTH; ++x ) {
				treeList.Add(new Tree());
			}
			sets.Add(treeList);
		}
		
		grid = new int[HEIGHT, WIDTH];
		for ( int j=0; j < HEIGHT; j++ ) {
			for ( int i=0; i < WIDTH; i++ ) {
				grid[j, i] = 0;
			}
		}
		
		edges = new List<Triplet> ();
		for ( int y = 0; y < HEIGHT; y++ )
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
	}
	
	private static void generateWalls()
	{
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
	}
}

public class LevelBuilder : MonoBehaviour {

	public GameObject root;

	private BuildingBlock[,] matrix;
	private static int size = 20;
	private List<int> horizontals;
	private List<int> verticals;
	private List<GameObject> missileList = new List<GameObject>();
	private GameObject missile;
	private ObjectManagementPool missilePool;
	private GameObject statue;
	private GameObject player;
	private GameObject firstSpawnPoint;
	private GameObject secondSpawnPoint;

	public void buildWithIterativeDivision(float completeness, float destroyability) {

		// initialize matrix
		matrix = new BuildingBlock[size, size];
		for (int i=0; i<size ;i++) {
			for (int j=0; j<size ;j++) {
				if (i==0 || i==size-1 || j==0 || j==size-1)
					matrix[j,i] = BuildingBlock.WALL;
				else if ( (i==1 && j==1) || (i==1 && j==size-2) || (i==size-2 && j==1) || (i==size-2 && j==size-2) )
					matrix[j,i] = BuildingBlock.OPENING;
				else if (i==1 || i==size-2)
					matrix[j,i] = BuildingBlock.VERT;
				else if (j==1 || j==size-2)
					matrix[j,i] = BuildingBlock.HORI;
				else
					matrix[j,i]= BuildingBlock.EMPTY;
			}
		}

		IterativeDivision builder = new IterativeDivision (matrix, size);
		builder.build (completeness, destroyability);

		buildlevelFromMatrix (matrix, destroyability);
		// print
		string matrixRepresentation = "";
		for (int j=0; j<size ;j++) {
			for (int i=0; i<size ;i++)
				matrixRepresentation = matrixRepresentation + matrix[j,i].ToString().Substring(0, 1);
			matrixRepresentation = matrixRepresentation + "\n";
		}
		Debug.Log (matrixRepresentation);
	}

	public void buildWithKruskal(float destroyability) {
		buildlevelFromMatrix(Kruskal.build (destroyability), destroyability);
	}

	public void clear() {
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in root.transform) children.Add(child.gameObject);
		children.ForEach(child => DestroyImmediate(child));
	}

	private void buildlevelFromMatrix(BuildingBlock[,] matrix, float destroyability)
	{
		int spawns = 0;
		player = Instantiate (Resources.Load("Prefabs/player3d") as GameObject) as GameObject;
		statue = Instantiate (Resources.Load("Prefabs/Statue") as GameObject) as GameObject;
		firstSpawnPoint = new GameObject("SpawnPoint");
		secondSpawnPoint = new GameObject("SpawnPoint");
		GameObject.Find("LevelManager").GetComponent<LevelManager>().player = player;
		GameObject.Find("LevelManager").GetComponent<LevelManager>().statue = statue;
		GameObject.Find("LevelManager").GetComponent<LevelManager>().spawnPoints[0] = firstSpawnPoint;
		GameObject.Find("LevelManager").GetComponent<LevelManager>().spawnPoints[1] = secondSpawnPoint;

		for ( int y = 0; y < 20; y++ )
		{
			for ( int x = 0; x < 20; x++ )
			{
				if (x == 0 || y == 0 || x == (20) - 1 || y == (20) - 1)
				{
					//It's an outer wall
					GameObject block = Instantiate (Resources.Load("Prefabs/Level Pieces/Metal2x2") as GameObject) as GameObject;
					block.transform.parent = root.transform;
					block.transform.localPosition = new Vector3(x*30, 0, y*30);
				}
				else if (x == 20/2 && y == 1)
				{
					// Base
					statue.transform.parent = root.transform;
					statue.transform.localPosition = new Vector3(x*30, -10, y*30);
				}
				else if ((x >= 20/2 - 1 && x <= 20/2 + 1) && (y == 1 || y == 2))
				{
					// Base perimeter
					GameObject block = Instantiate (Resources.Load("Prefabs/Level Pieces/Brick2x2") as GameObject) as GameObject;
					block.transform.parent = root.transform;
					block.transform.localPosition = new Vector3(x*30-5, 0, y*30-5);
				}
				else if ((x == 20-2 || x == 20-3 || x == 20-4 || x == 1 || x == 2 || x == 3) && (y == 20-2 || y == 20-3 || y == 20-4))
				{
					// Empty space for enemy tanks
					if( (x == 20-3 || x == 2) && y == 20-3)
					{
						GameObject spawner = null;
						if (spawns == 0)
						{
							spawner = firstSpawnPoint;
							spawns++;
						}
						else
						{
							spawner = secondSpawnPoint;
						}
						spawner.transform.parent = root.transform;
						spawner.transform.localPosition = new Vector3(x*30, 0, y*30);

						GameObject enemy = Instantiate (Resources.Load("Prefabs/enemy3d") as GameObject) as GameObject;
						enemy.GetComponent<EnemyController>().statue = statue.transform;
						enemy.GetComponent<EnemyController>().player = player.transform;
						enemy.transform.parent = root.transform;
						enemy.transform.localPosition = new Vector3(x*30, -5, y*30);
					}
				}
				else if ((x >= 20/2 - 1 && x <= 20/2 + 1) && (y >= 20/2 - 1 && y <= 20/2 + 1))
				{
					if( x == 20/2 && y == 20/2)
					{
						player.transform.parent = root.transform;
						player.transform.localPosition = new Vector3(x*30, -5, y*30);
					}
				}
				else if (matrix[y, x] == BuildingBlock.WALL)
				{
					float randomNumber = Random.Range(0F, 100F);
					if (randomNumber < destroyability * 100)
					{
						GameObject block = Instantiate (Resources.Load("Prefabs/Level Pieces/Brick2x2") as GameObject) as GameObject;
						block.transform.parent = root.transform;
						block.transform.localPosition = new Vector3(x*30-5, 0, y*30-5);
					}
					else
					{
						GameObject block = Instantiate (Resources.Load("Prefabs/Level Pieces/Metal2x2") as GameObject) as GameObject;
						block.transform.parent = root.transform;
						block.transform.localPosition = new Vector3(x*30, 0, y*30);
					}

				}
			}
		}
	}

}
