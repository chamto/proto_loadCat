using UnityEngine;
using System.Collections;

public class CatMove_MonoBehaviour : MonoBehaviour 
{
	public Vector3 destPos = Vector3.zero;


	// Use this for initialization
	void Start () {
	
	}
	
	bool bMove = true;
	Vector3 dir;
	void Update () 
	{

		if(true == Input_Unity.IsTouch())
		{

			destPos = Input_Unity.GetTouchWorldPos ();
			destPos.z = 0;
			dir = destPos - transform.position;

		}


		this.AniDirection (destPos - transform.position);
		this.transform.position += dir * Time.deltaTime; 


	}


	public void AniDirection(Vector3 dir)
	{
		
		//    dir.x
		//left  :  right
		// -    :    +
		//   scale.x
		// +    :    -
		Vector3 aniDir = Vector3.zero; ////애니의 기준방향
		Vector3 scale = this.transform.localScale;
		if (dir.x <= 0) 
		{	//left
			scale.x = Mathf.Abs(scale.x);
			
			aniDir = Vector3.left;
			//Debug.Log("AniDirection left"); //chamto test
		}
		if (0 < dir.x) 
		{	//right
			scale.x = Mathf.Abs(scale.x) * -1;
			
			aniDir = Vector3.right;
			//Debug.Log("AniDirection right"); //chamto test
		}
		this.transform.localScale = scale;
		
		
		//transform.localRotation = Quaternion.LookRotation (Vector3.forward, dir);
		transform.localRotation = Quaternion.FromToRotation (aniDir, dir);
		
	}


	void TouchBegan() 
	{
		CDefine.DebugLog ("began");
	}
	void TouchMoved() 
	{
		CDefine.DebugLog ("moved");
	}
	void TouchEnded() 
	{
		CDefine.DebugLog ("ended");
	}
}
