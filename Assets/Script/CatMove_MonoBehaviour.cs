using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatMove_MonoBehaviour : MonoBehaviour 
{
	private MonoPathFinder _pathFinder = null;
	private Stack<Vector3> _pathPos = null;
	private Vector3 _destPos = Vector3.zero;
	private Rigidbody2D _rb2d = null;


	// Use this for initialization
	void Start () 
	{
		GameObject objMgr = GameObject.Find("0_manager");
		_pathFinder = objMgr.GetComponent<MonoPathFinder> ();
		_rb2d = this.GetComponent<Rigidbody2D> ();
	}

	public void MoveNext()
	{
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("SuperCat"), LayerMask.NameToLayer ("Building"), true);
		this.gameObject.layer = LayerMask.NameToLayer ("SuperCat");
		
		_destPos = _pathPos.Pop ();
		_destPos.z = 0;

		if (null != _pathPos) 
		{
			//chamto test
			temp = "Update - State :" + _STATE + " pathPos :";
			foreach (Vector2 node in _pathPos) 
			{
				temp += node + "->";
			}
			Debug.Log (temp); 
		}

	}



	public void UpdateMoveToPos()
	{
		_dir = _destPos - transform.position;

		//--------------------------------
		//liner moving
		_rb2d.MovePosition(transform.position + (_destPos - transform.position) * 0.1f);
		
		//--------------------------------
		//normal
//		if(_rb2d.velocity.sqrMagnitude <= 20.8f)
//			_rb2d.AddForce (_dir, ForceMode2D.Impulse); // ++ force
//		else
//			_rb2d.AddForce (_dir , ForceMode2D.Force); // == force
		//_rb2d.AddForce (_dir, ForceMode2D.Force); // == force
		//_rb2d.AddForce (_dir.normalized * 15, ForceMode2D.Force);
		//_rb2d.AddForceAtPosition(_dir.normalized * 20, _destPos, ForceMode2D.Force);

//		_rb2d.angularVelocity = 0f;
//		_rb2d.angularDrag = 0f;
//		_rb2d.drag = 0f;
		//--------------------------------
		//순간이동 닌자 느낌
		//_rb2d.MovePosition(_destPos); 

		//--------------------------------
		//급한 느낌
		//_rb2d.AddForceAtPosition(_dir, _destPos, ForceMode2D.Impulse);

	}

	public bool ArriveOn()
	{
		if(float.Epsilon <= _dir.sqrMagnitude && _dir.sqrMagnitude <= 0.15f)
		{
			this.gameObject.layer = LayerMask.NameToLayer ("Default");

			_rb2d.MovePosition(_destPos);
			_rb2d.velocity = Vector2.zero; //cat stop

			Debug.Log("----------- Arrive On ----------- "); //chamto test
			return true;
		}
		
		return false;

	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		//Debug.Log("------OnCollisionExit2D : Building : sqrMag_vel : " + _rb2d.velocity.sqrMagnitude);
		//this.gameObject.layer = LayerMask.NameToLayer ("Default");
		_isContactBuilding = false;
		//Vector2 v2 = _rb2d.position;
		//v2 +=(Vector2)(_dir.normalized * 0.7f);
		//_rb2d.position = v2;
		//_rb2d.MovePosition (v2);
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		//_rb2d.velocity = Vector2.zero; //cat stop

		if (coll.gameObject.tag == "Cat") 
		{

			//Debug.Log("OnCollisionEnter2D : Cat");
		}
		
		if (coll.gameObject.tag == "Building") 
		{
			Debug.Log("------OnCollisionEnter2D : Building : sqrMag_vel : " + _rb2d.velocity.sqrMagnitude);
			_isContactBuilding = true;

//			if(_rb2d.velocity.sqrMagnitude >= 0.2f)
//			{
//				this.gameObject.layer = LayerMask.NameToLayer ("SuperCat");
//			}
//			else
//			{
//				this.gameObject.layer = LayerMask.NameToLayer ("Default");
//			}
			
			
			
		}
	}

	bool _isContactBuilding = false;
	int _STATE = 0; //chamto temp
	Vector3 _dir = Vector3.zero;
	string temp = "";
	float sumTime = 0;
	void Update ()
	{

		if (true == Input_Unity.IsTouch ()) 
		{
			_pathPos = _pathFinder.Search(transform.position, Input_Unity.GetTouchWorldPos ());

			_STATE = 1;
			this.MoveNext();

		}

		switch (_STATE) 
		{
		case 0: //idle
		{
			_dir = _destPos - transform.position;

			//--------------------------------
			//normal
			//_rb2d.AddForce (_dir, ForceMode2D.Force); // == force

			//--------------------------------
			//혼자 먹으려 싸우는 느낌
			//_rb2d.MovePosition(_destPos); 

			//--------------------------------
			//서로 나누어 먹는 느낌
			//_rb2d.AddForceAtPosition(_dir, _destPos, ForceMode2D.Impulse); //++ force


		}
			break;
		case 1: //move to pos
		{

			//Debug.Log(sumTime + " : " + _rb2d.position + " : " + transform.position);
			sumTime += Time.deltaTime;
			//if(sumTime > 0.2f)
			{
				this.UpdateMoveToPos();
				sumTime = 0;
			}


			if(true ==this.ArriveOn())
			{
				_STATE = 2;
			}
		}
			break;
		case 2: //Landing
		{
			if ((null != _pathPos && 0 == _pathPos.Count) 
			    || null == _pathPos) 
			{
				_STATE = 0;
			}


			if(true == _isContactBuilding)
			{
				if (null != _pathPos && 0 != _pathPos.Count) 
				{
					_STATE = 1;
					//_isContactBuilding = false;
					MoveNext();
				}
			}

		}
			break;
		}

//		if(true == Input_Unity.IsTouch())
//		{
//
//			_destPos = Input_Unity.GetTouchWorldPos ();
//			_destPos.z = 0;
//			_dir = _destPos - transform.position;
//
//
//			//Debug.Log("Force : " + rb2d.velocity.sqrMagnitude); //chamto test
//			if(_rb2d.velocity.sqrMagnitude >= 100.0f)
//				_rb2d.AddForce (_dir, ForceMode2D.Force);
//			else
//				_rb2d.AddForce (_dir, ForceMode2D.Impulse);
//			
//			//rb2d.MovePosition(destPos);
//			//rb2d.AddForceAtPosition(_dir, destPos, ForceMode2D.Impulse);
//		}

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


		//NavGraphNode node = _pathFinder._graph.FindNearNode (Input_Unity.GetTouchWorldPos ());
		//Debug.Log ("findNode : "+node); //chamto test


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

}
