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
}
