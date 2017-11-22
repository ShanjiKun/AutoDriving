using System;
using System.Collections;
using System.Collections.Generic;

public class TestCaseData {

	public string enemyLane;
	public int left;
	public int right;
	public int front;
	public int behind;

	public TestCaseData (string enemyLane, int left, int right, int front, int behind)
	{
		this.enemyLane = enemyLane;
		this.left = left;
		this.right = right;
		this.front = front;
		this.behind = behind;
	}

	public TestCaseData (string text)
	{
		string[] elems = text.Split (","[0]);
		if (elems.Length != 5) {
			this.enemyLane = "L";
			this.left = 5;
			this.right = 0;
			this.front = 0;
			this.behind = 0;
		} else {
			try{
				this.enemyLane = elems[0];
				this.left = Convert.ToInt32(elems[1]);
				this.right = Convert.ToInt32(elems[2]);
				this.front = Convert.ToInt32(elems[3]);
				this.behind = Convert.ToInt32(elems[4]);	
			} catch (Exception e){
				this.enemyLane = "L";
				this.left = 5;
				this.right = 0;
				this.front = 0;
				this.behind = 0;
			}
		}
	}
}
