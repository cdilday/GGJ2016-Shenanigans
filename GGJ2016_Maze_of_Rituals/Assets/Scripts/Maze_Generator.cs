using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Maze_Generator : MonoBehaviour {

	//top left corner is 0,0. bottom right is width, height.

	public GameObject prefab;
	public GameObject spawnPoint;
	public GameObject collectible;
	public Maze_Cell [,] maze;
	public GameObject [,] mazeObjects;
	public int height;
	public int width;

	public IEnumerator GenerateMaze()
	{
		GameObject.Find ("Player").transform.position = new Vector3 (((int)(width / 2)) + 0.5f, ((int)(height / -2)) - 0.5f, -0.03f);
		mazeObjects = new GameObject[width, height];
		maze = new Maze_Cell[width, height];

		List<Maze_Cell> activeCells = new List<Maze_Cell>();
		//make a new mazecell at a random position
		int x = UnityEngine.Random.Range (0, width);
		int y = UnityEngine.Random.Range (0, height);

		GameObject temp = Instantiate(prefab, new Vector3( x + 0.5f, (y * -1) - 0.5f, 0f), Quaternion.identity) as GameObject;
		maze[x,y] = temp.GetComponent<Maze_Cell>();
		maze[x,y].GridPos = new Vector2(x, y);
		//set the parent to the generator and name them for easier reading in editor
		temp.transform.SetParent(transform);
		temp.name = "Cell " + x + ", " + y;
		mazeObjects[x,y] = temp;
		activeCells.Add (maze[x,y]);
		yield return new WaitForSeconds(0.00001f);
		while (activeCells.Count > 0) {
			GenerationStep(activeCells);
			//uncomment this to see the generation live
			//yield return new WaitForSeconds(0.00001f);
		}

		//place the important things
		//spawn point
		x = (int)(width / 2);
		y = (int)(height / 2);

		GameObject sp = Instantiate(spawnPoint, new Vector3(x + 0.5f, (y *-1) -0.5f, -0.01f), Quaternion.identity) as GameObject;
		//break walls on top of the spawn point
		//start with middle
		if (maze [x, y].connections [0] == null) {
			maze[x,y].MakeConnection(maze[x,y-1]);
		}
		if (maze [x, y].connections [1] == null) {
			maze[x,y].MakeConnection(maze[x+1,y]);
		}
		if (maze [x, y].connections [2] == null) {
			maze[x,y].MakeConnection(maze[x,y+1]);
		}
		if (maze [x, y].connections [3] == null) {
			maze[x,y].MakeConnection(maze[x-1,y]);
		}
		//Now the up one, only need to check left and right
		if (maze [x, y-1].connections [1] == null) {
			maze[x,y-1].MakeConnection(maze[x+1,y-1]);
		}
		if (maze [x, y-1].connections [3] == null) {
			maze[x,y-1].MakeConnection(maze[x-1,y-1]);
		}
		//Now down, same checks
		if (maze [x, y+1].connections [1] == null) {
			maze[x,y+1].MakeConnection(maze[x+1,y+1]);
		}
		if (maze [x, y+1].connections [3] == null) {
			maze[x,y+1].MakeConnection(maze[x-1,y+1]);
		}
		//Now check right, only need to check verticals
		if (maze [x+1, y].connections [0] == null) {
			maze[x+1,y].MakeConnection(maze[x+1,y-1]);
		}
		if (maze [x+1, y].connections [2] == null) {
			maze[x+1,y].MakeConnection(maze[x+1,y+1]);
		}
		//Now check left, only need to check verticals. Spawn point should be clear after this
		if (maze [x-1, y].connections [0] == null) {
			maze[x-1,y].MakeConnection(maze[x-1,y-1]);
		}
		if (maze [x-1, y].connections [2] == null) {
			maze[x-1,y].MakeConnection(maze[x-1,y+1]);
		}

		//Maze is very 1 path centric. Let's open it up a bit;
		int numToBreak = (int)(Mathf.Min (width, height)/ 5);
		int tempx = UnityEngine.Random.Range (1, width - 2);
		int tempy = UnityEngine.Random.Range (1, height - 2);
		for (int j = 0; j < numToBreak; j++) {
			while(maze[tempx,tempy].getEdgeCount() == 0){
				tempx = UnityEngine.Random.Range (1, width - 2);
				tempy = UnityEngine.Random.Range (1, height - 2);
			}
			int wallNum = maze[tempx,tempy].getRandomWall();
			switch(wallNum){
			case 0:
				maze[tempx,tempy].MakeConnection(maze[tempx, tempy-1]);
				break;
			case 1:
				maze[tempx,tempy].MakeConnection(maze[tempx+1, tempy]);
				break;
			case 2:
				maze[tempx,tempy].MakeConnection(maze[tempx, tempy+1]);
				break;
			case 3:
				maze[tempx,tempy].MakeConnection(maze[tempx-1, tempy]);
				break;
			default:
				break;
			}
		}

		//now place the collectibles. First, let's get all the maze points with 3 walls
		List<Maze_Cell> deadEnds = new List<Maze_Cell>();
		GetAllDeadEnds (deadEnds);
		//sort it by distance from the spawn point
		foreach (Maze_Cell end in deadEnds) {
			end.tempDistance = Mathf.Abs (Vector2.Distance(end.GridPos, new Vector2(x,y)));
		}
		deadEnds = deadEnds.OrderBy(s=>s.tempDistance).ToList();
		//now make and place the collectibles
		for(int i = 0; i < 3; i++){
			GameObject item = Instantiate(collectible, new Vector3( deadEnds[deadEnds.Count - 1 - (i*2)].GridPos.x + 0.5f,
			                                                       (deadEnds[deadEnds.Count - 1 - (i*2)].GridPos.y* -1) - 0.5f,
			                                                       - 0.1f), Quaternion.identity) as GameObject;
			item.GetComponent<Collectible>().giveType(i);
		}
	}

	void GenerationStep(List<Maze_Cell> activeCells){
		int currIndex = activeCells.Count- 1;
		Maze_Cell currCell = activeCells [currIndex];
		int dir = currCell.getRandomOpenDirection();
		if (dir == -1) {
			activeCells.RemoveAt(currIndex);
			return;
		}
		if(inBounds(currCell, dir)){
			int x, y;
			switch(dir){
			case 0:
				x = (int) currCell.GridPos.x;
				y = (int) currCell.GridPos.y - 1;
				break;
			case 1:
				x = (int) currCell.GridPos.x + 1;
				y = (int) currCell.GridPos.y;
				break;
			case 2:
				x = (int) currCell.GridPos.x;
				y = (int) currCell.GridPos.y + 1;
				break;
			case 3:
				x = (int) currCell.GridPos.x - 1;
				y = (int) currCell.GridPos.y;
				break;
			default:
				x = (int) currCell.GridPos.x + 1;
				y = (int) currCell.GridPos.y;
				break;
			}
			Maze_Cell neightbor = maze[x,y];
			if(neightbor == null){
				GameObject temp = Instantiate(prefab, new Vector3( x + 0.5f, (y * -1) - 0.5f, 0f), Quaternion.identity) as GameObject;
				maze[x,y] = temp.GetComponent<Maze_Cell>();
				maze[x,y].GridPos = new Vector2(x, y);
				//set the parent to the generator and name them for easier reading in editor
				temp.transform.SetParent(transform);
				temp.name = "Cell " + x + ", " + y;
				mazeObjects[x,y] = temp;
				activeCells.Add (maze[x,y]);
				//make the connection
				currCell.MakeConnection(maze[x,y]);

			}
			else{
				currCell.MakeWall(dir);
				//check adjacent walls, activate if needed
				if(y != 0 && maze[x, y-1] != null && maze[x, y-1].walls[2].activeSelf){
					maze[x,y].MakeWall(0);
				}
				if(x != width - 1 && maze[x+1, y] != null && maze[x+1, y].walls[3].activeSelf){
					maze[x,y].MakeWall(1);
				}
				if(y != height - 1 && maze[x, y+1] != null && maze[x, y+1].walls[0].activeSelf){
					maze[x,y].MakeWall(2);
				}
				if(x != 0 && maze[x-1, y] != null && maze[x-1, y].walls[1].activeSelf){
					maze[x,y].MakeWall(3);
				}
			}
		}
		else{
			currCell.MakeWall(dir);
		}
	}

	//checks if the current cell's direction is in bounds
	public bool inBounds(Maze_Cell cell, int direction)
	{
		switch (direction) {
		case 0:
			if((int)cell.GridPos.y == 0)
				return false;
			return true;
		case 1:
			if((int)cell.GridPos.x == width -1)
				return false;
			return true;
		case 2:
			if((int)cell.GridPos.y == height - 1)
				return false;
			return true;
		case 3:
			if((int)cell.GridPos.x == 0)
				return false;
			return true;
		}
		return false;
	}

	//uses A* to find a path from the start position to the target position
	//TODO: Currently actually Djikstra's but don't tell anyone
	public Maze_Cell [] AStarPathfinder(Vector2 start, Vector2 target){
		//first set up all maze_cells with a temp Distance that is huge
		for (int x = 0; x < width; x++) {
			for( int y = 0; y < height; y++){
				maze[x,y].tempDistance = Mathf.Infinity;
			}
		}
		// this will be the path at the end to return
		List<Maze_Cell> path = new List<Maze_Cell>();
		//this is the cdictionary used to store cell for previously traversed cells
		Dictionary<Maze_Cell, Maze_Cell> prev = new Dictionary<Maze_Cell, Maze_Cell>();

		List<Maze_Cell> priorityQueue = new List<Maze_Cell>();

		Maze_Cell currCell;
		maze [(int)start.x, (int)start.y].tempDistance = 0;
		priorityQueue.Insert(0, maze[(int)start.x,(int) start.y]);

		while(priorityQueue.Count != 0){
			priorityQueue.Sort();
			currCell = priorityQueue[0];
			priorityQueue.RemoveAt(0);

			if( (int)currCell.GridPos.x == (int) target.x && (int)currCell.GridPos.y == (int) target.y){
				//reached goal, populate the path array and return it
				Debug.Log ("Made it!");
				Maze_Cell temp;
				while(prev.TryGetValue(currCell, out temp)){
					path.Insert (0, currCell);
					currCell = prev[currCell];
				}
				path.Insert(0, currCell);
				break;
			}

			foreach(Maze_Cell adj in currCell.connections){
				if(adj != null){
					float alt = currCell.tempDistance + Vector2.Distance(currCell.GridPos, adj.GridPos);
					if(alt < adj.tempDistance){
						adj.tempDistance = alt;
						prev[adj] = currCell;
						priorityQueue.Add (adj);
					}
				}
			}
		}
		return path.ToArray ();
		
	}

	void GetAllDeadEnds(List<Maze_Cell> deadEnds)
	{
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (maze[x,y].getEdgeCount() == 3){
					deadEnds.Add(maze[x,y]);
				}
			}
		}

	}

}
