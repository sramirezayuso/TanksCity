using UnityEngine;
using System.Collections;

public class Triplet
{
	private int x;
	private int y;
	private Direction dir;
	
	public Triplet(int x, int y, Direction dir)
	{
		this.x = x;
		this.y = y;
		this.dir = dir;
	}
	
	public int getX()
	{
		return this.x;
	}
	
	public int getY()
	{
		return this.y;
	}
	
	public Direction getDir()
	{
		return this.dir;
	}
}

