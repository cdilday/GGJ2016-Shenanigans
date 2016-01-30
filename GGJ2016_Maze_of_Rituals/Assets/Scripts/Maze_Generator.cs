using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Maze_Generator : MonoBehaviour {

	//top left corner is 0,0. bottom right is width, height.

	public GameObject prefab;	
	public Maze_Cell [,] maze;
	public GameObject [,] mazeObjects;
	public int height;
	public int width;


	// Use this for initialization
	void Start () {
		Generate_Initial ();
		Maze_Cell [] path = AStarPathfinder(new Vector2(0,0), new Vector2(5,5));
		foreach (Maze_Cell cell in path) {
			Debug.Log (cell.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Generate_Initial (){

		//first fill the maze with cells and wall them if they're on an edge
		mazeObjects = new GameObject[width, height];
		maze = new Maze_Cell[width, height];
		for (int x = 0; x < width; x++) {
			for( int y = 0; y < height; y++){
				GameObject temp = Instantiate(prefab, new Vector3( x + 0.5f, (y * -1) - 0.5f, 0f), Quaternion.identity) as GameObject;
				maze[x,y] = temp.GetComponent<Maze_Cell>();
				maze[x,y].GridPos = new Vector2(x, y);
				//set the parent to the generator and name them for easier reading in editor
				temp.transform.SetParent(transform);
				temp.name = "Cell " + x + ", " + y;
				mazeObjects[x,y] = temp;

				//now make walls if they're edges
				//up edge
				if(y == 0){
					maze[x,y].MakeWall(0);
				}
				//right edge
				if(x == width - 1){
					maze[x,y].MakeWall(1);
				}
				//down edge
				if(y == height - 1){
					maze[x,y].MakeWall(2);
				}
				//left edge
				if(x == 0)
				{
					maze[x,y].MakeWall(3);
				}
			}
		}

		//now set the connections
		for (int x = 0; x < width; x++) {
			for( int y = 0; y < height; y++){
				//every connection needs to check if it's in bounds to prevent out of range errors
				//up connection
				if(y != 0 && maze[x, y-1].connections[2] == null){
					maze[x,y].MakeConnection(maze[x, y-1]);
				}
				//right edge
				if(x != width - 1 && maze[x+1,y].connections[3] == null){
					maze[x,y].MakeConnection(maze[x+1, y]);
				}
				//down edge
				if(y != height - 1 && maze[x,y+1].connections[0] == null){
					maze[x,y].MakeConnection(maze[x, y+1]);
				}
				//left edge
				if(x != 0 && maze[x-1,y].connections[1] == null){
					maze[x,y].MakeConnection(maze[x-1, y]);
				}
			}
		}

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

}
