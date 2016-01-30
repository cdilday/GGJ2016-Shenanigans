using UnityEngine;
using System.Collections;

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
}
