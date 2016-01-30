using UnityEngine;
using System.Collections;
using System;

public class Maze_Cell :  MonoBehaviour, IComparable<Maze_Cell> {

	//position on the grid
	public Vector2 GridPos;
	// size 4, up, left, right, down
	//if null, there's a wall there
	public Maze_Cell [] connections;
	//the child objects that serve as walls for the player
	public GameObject [] walls;


	public float tempDistance;
	// Use this for initialization
	void Awake () {
		connections = new Maze_Cell[4];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//returns -1 if not adjacent, return 0, 1, 2, or 3 if it is, corresponding to the clockwise index of its relation to the one that's checking
	public int isAdjacent(Maze_Cell mazeCell){
		//check to make sure it's in the same row
		if((int) mazeCell.GridPos.y == (int) GridPos.y){
			//check if it's left
			if((int)mazeCell.GridPos.x == (int)GridPos.x - 1){
				return 3;
			}
			//check if it's to the right
			else if ((int)mazeCell.GridPos.x == (int)GridPos.x + 1){
				return 1;
			}
			// it's not adjacent at all
			else{
				return -1;
			}
		}
		//now check the columns
		if((int) mazeCell.GridPos.x == (int) GridPos.x){
			//check if it's above
			if((int)mazeCell.GridPos.y == (int)GridPos.y - 1){
				return 0;
			}
			//check if it's below
			else if ((int)mazeCell.GridPos.y == (int)GridPos.y + 1){
				return 2;
			}
			// it's not adjacent at all
			else{
				return -1;
			}
		}

		return -1;

	}

	public bool MakeConnection(Maze_Cell connection)
	{
		//first check to make sure these are adjacent
		int index = isAdjacent (connection);
		if (index == -1) {
			Debug.Log ("Cell " + GridPos + " cannot connect to cell " + connection.GridPos);
			return false;
		}

		connections [index] = connection;
		walls [index].SetActive (false);
		//set the given cell to connect to this one.
		int indexOther = connection.isAdjacent(this);
		if (indexOther == -1) {
			Debug.Log ("Cell " + connection.GridPos + " cannot connect to cell " + GridPos + ".... somehow");
			connections[index] = null;
			return false;
		}

		connection.connections[indexOther] = this;
		connection.walls [indexOther].SetActive (false);
		return true;
	}

	public bool BreakConnection(Maze_Cell connection){
		//first check to make sure these are adjacent
		int index = isAdjacent (connection);
		if (index == -1) {
			Debug.Log ("Cell " + GridPos + " isn't adjacent to cell " + connection.GridPos);
			return false;
		}
		
		connections [index] = null;
		walls [index].SetActive (true);
		//set the given cell to connect to this one.
		int indexOther = connection.isAdjacent(this);
		if (indexOther == -1) {
			Debug.Log ("Cell " + connection.GridPos + " isn't adjacent to cell " + GridPos + ".... somehow");
			return false;
		}
		
		connection.connections[indexOther] = null;
		connection.walls [indexOther].SetActive (true);
		return true;
	}

	public void MakeWall(int side){
		if (connections [side] != null) {
			if(side >1){
				connections[side].connections[side-2] = null;
				connections[side].walls[side-2].SetActive(true);
			}
			else if(side == 1){
				connections[side].connections[3] = null;
				connections[side].walls[3].SetActive(true);
			}
			else{
				connections[side].connections[2] = null;
				connections[side].walls[2].SetActive(true);
			}
		}
		connections[side] = null;
		walls[side].SetActive(true);

	}

	public int CompareTo(Maze_Cell other){
		return (int) Mathf.Abs(tempDistance - other.tempDistance);
	}

	public string ToString(){
		return gameObject.name;
	}
}
