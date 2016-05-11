using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatMove_MonoBehaviour : MonoBehaviour 
{
	private MonoPathFinder _pathFinder = null;
	private List<int> _pathPos = null;
	private Vector3 _destPos = Vector3.zero;
	private Rigidbody2D _rb2d = null;


	// Use this for initialization
	void Start () 
	{
		GameObject objMgr = GameObject.Find("0_manager");
		_pathFinder = objMgr.GetComponent<MonoPathFinder> ();
		_rb2d = this.GetComponent<Rigidbody2D> ();
	}
	


	void Update () 
	{
		Vector3 dir = Vector3.zero;

		if (true == Input_Unity.IsTouch ()) 
		{
			
			_destPos = Input_Unity.GetTouchWorldPos ();
			_destPos.z = 0;
			dir = _destPos - transform.position;
		}

		if(true == Input_Unity.IsTouch())
		{

			_destPos = Input_Unity.GetTouchWorldPos ();
			_destPos.z = 0;
			dir = _destPos - transform.position;


			//Debug.Log("Force : " + rb2d.velocity.sqrMagnitude); //chamto test
			if(_rb2d.velocity.sqrMagnitude >= 100.0f)
				_rb2d.AddForce (dir, ForceMode2D.Force);
			else
				_rb2d.AddForce (dir, ForceMode2D.Impulse);
			
			//rb2d.MovePosition(destPos);
			//rb2d.AddForceAtPosition(dir, destPos, ForceMode2D.Impulse);
		}

		this.AniDirection (_destPos - transform.position);
		//this.transform.position += dir * Time.deltaTime; 


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


		NavGraphNode node = _pathFinder._graph.FindNearNode (Input_Unity.GetTouchWorldPos ());
		Debug.Log ("findNode : "+node); //chamto test


		//chamto test code - layer collision test
		bool option = true;
		option = Physics2D.GetIgnoreLayerCollision (LayerMask.NameToLayer ("SuperCat"), LayerMask.NameToLayer ("Building"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("SuperCat"), LayerMask.NameToLayer ("Building"), true);

		if(this.gameObject.layer != LayerMask.NameToLayer ("SuperCat"))
			this.gameObject.layer = LayerMask.NameToLayer ("SuperCat");
		else
			this.gameObject.layer = LayerMask.NameToLayer ("Default");

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


	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Cat") 
		{
			//Debug.Log("OnCollisionEnter2D : Cat");
		}

		if (coll.gameObject.tag == "Building") 
		{
			//Debug.Log("OnCollisionEnter2D : Building");

			if(_rb2d.velocity.sqrMagnitude >= 30.0f)
			{
				//Vector2 pos = (Vector2)(transform.position) + rb2d.velocity.normalized * 1.5f;
				
				//rb2d.velocity
				
				//rb2d.MovePosition(pos);
			}



		}
	}




}
