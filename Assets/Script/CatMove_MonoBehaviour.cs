using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cat
{
	public enum eMove
	{
		Normal,
		JumpFly,
		JumpRush,
		Super,
		Ninja,
	}

	public enum eArrive
	{
		Normal,
		Good,
		Grabber,
	}

	public enum eStyle
	{
		Normal,
		Aggressive,
	}
}



public class CatMove_MonoBehaviour : MonoBehaviour 
{
	//public Cat.eMove _moveMode = Cat.eMove.Normal;
	public Cat.eMove _moveMode = Cat.eMove.JumpFly;
	public Cat.eArrive _arriveMode = Cat.eArrive.Normal;
	public Cat.eStyle _style = Cat.eStyle.Normal;

	private MonoPathFinder _pathFinder = null;
	private Stack<Vector3> _pathPos = new Stack<Vector3>();
	private Vector3 _destPos = Vector3.zero;
	private Rigidbody2D _rb2d = null;


	// Use this for initialization
	void Start () 
	{
		GameObject objMgr = GameObject.Find("0_manager");
		_pathFinder = objMgr.GetComponent<MonoPathFinder> ();
		_rb2d = this.GetComponent<Rigidbody2D> ();
		//_prevPosition = this.transform.position;

//		Utility.Line.AddDebugLine (this.transform, this.name, Vector3.zero, Vector3.zero); //chamto test
//		Utility.Line.AddDebugLine (this.transform, this.name+"01", Vector3.zero, Vector3.zero); //chamto test
//		Utility.Line.AddDebugLine (this.transform, this.name+"02", Vector3.zero, Vector3.zero); //chamto test
//		Utility.Line.AddDebugLine (this.transform, this.name+"_path", Vector3.zero, Vector3.zero);
	}



	public void State_MoveNext()
	{
		Physics2D.IgnoreLayerCollision (GlobalConstants.Layer.Num.superCat, GlobalConstants.Layer.Num.building, true);
		this.gameObject.layer = GlobalConstants.Layer.Num.superCat;
		
		_destPos = _pathPos.Pop ();
		_destPos.z = 0;

		//_rb2d.velocity = Vector2.zero; //cat stop
//		if (null != _pathPos) 
//		{
//			//chamto test
//			temp = "Update - State :" + _STATE + " pathPos :";
//			foreach (Vector2 node in _pathPos) 
//			{
//				temp += node + "->";
//			}
//			Debug.Log (temp); 
//		}

	}
	
	public void State_UpdateMoveToPos()
	{
		_dir = _destPos - transform.position;

		//--------------------------------
		//chamto test code 
//		if (transform.position.y < _destPos.y)
//		if(_prevVelocity.y > _rb2d.velocity.y) 
//		{	
//			this.gameObject.layer = GlobalConstants.Layer.Num.default0;
//			_STATE = 3;
//		} 
//		_prevVelocity = _rb2d.velocity;
		//--------------------------------

		//_rb2d.AddForce (_dir, ForceMode2D.Force); // == force
		//_rb2d.AddForce (_dir.normalized * 15, ForceMode2D.Force);
		//_rb2d.AddForceAtPosition(_dir.normalized * 20, _destPos, ForceMode2D.Force);

//		_rb2d.angularVelocity = 0f;
//		_rb2d.angularDrag = 0f;
//		_rb2d.drag = 0f;

		if (Cat.eMove.Normal == _moveMode) 
		{
			//--------------------------------
			//liner moving
			_rb2d.MovePosition(transform.position + (_destPos - transform.position) * 0.1f);
		}
		if (Cat.eMove.JumpFly == _moveMode) 
		{
			if(_rb2d.velocity.sqrMagnitude <= 20.8f)
				_rb2d.AddForce (_dir, ForceMode2D.Impulse); // ++ force
			else
				_rb2d.AddForce (_dir , ForceMode2D.Force); // == force
		}
		if (Cat.eMove.JumpRush == _moveMode) 
		{
			_rb2d.AddForceAtPosition(_dir, _destPos, ForceMode2D.Impulse);
		}
		if (Cat.eMove.Super == _moveMode) 
		{
			_rb2d.MovePosition(transform.position + (_destPos - transform.position) * 0.2f);

		}
		if (Cat.eMove.Ninja == _moveMode) 
		{
			//--------------------------------
			//순간이동 닌자 느낌
			_rb2d.MovePosition(_destPos); 
		}


	}

	public bool State_ArriveOn()
	{
		if(float.Epsilon <= _dir.sqrMagnitude && _dir.sqrMagnitude <= 0.25f)
		{
			this.gameObject.layer = GlobalConstants.Layer.Num.default0;

			_rb2d.velocity = Vector2.zero; //cat stop

			if(Cat.eStyle.Aggressive == _style)
			{
				//다른 고양이 밀치기 효과
				_rb2d.MovePosition(_destPos); 
			}


			//Debug.Log("----------- Arrive On ----------- "); //chamto test
			return true;
		}
		
		return false;

	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		//Debug.Log("------OnCollisionExit2D : Building : sqrMag_vel : " + _rb2d.velocity.sqrMagnitude);
		_isContactBuilding = false;

	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		//_rb2d.velocity = Vector2.zero; //cat stop

		if (coll.gameObject.tag == "Cat") 
		{


		}
		
		if (coll.gameObject.tag == "Building") 
		{
			//Debug.Log("------OnCollisionEnter2D : Building : sqrMag_vel : " + _rb2d.velocity.sqrMagnitude);
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
	void OnCollisionStay2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Building") 
		{
//			DebugWide.LogRed ("OnCollisionStay2D : Cat");
//			if(0 != _pathPos.Count)
//			{
//				//_rb2d.AddForce(_dir * -1f,ForceMode2D.Impulse);
//				//_rb2d.AddForceAtPosition(_dir, _destPos, ForceMode2D.Impulse);
//				
//			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{

		if (other.tag == "Building") 
		{
			//DebugWide.LogWhite("triggerEnter " + _dir.y);
			if(_dir.y < 0 )
			{
				//_rb2d.MovePosition (this.transform.position + _dir.normalized*0.5f);
			}
			//DebugWide.LogWhite("triggerEnter " + _rb2d.velocity.y);
			//this.gameObject.layer = GlobalConstants.Layer.Num.superCat;
		}

	}
	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.tag == "Building") 
		{
			//DebugWide.LogRed("triggerExit " + _rb2d.velocity.y);
			//this.gameObject.layer = GlobalConstants.Layer.Num.default0;
		}
	}
	void OnTriggerStay2D(Collider2D other) 
	{
		if (other.tag == "Building") 
		{
			//DebugWide.LogBlue("triggerStay " + _rb2d.velocity.y);
			//this.gameObject.layer = GlobalConstants.Layer.Num.superCat;
			//_rb2d.MovePosition (this.transform.position + _dir*0.3f);
		}


		//other.attachedRigidbody.AddForce(-0.1 * other.attachedRigidbody.velocity);
	}

	//private Vector3 _prevPosition;
	private Vector3 _sourcePos = Vector3.zero;
	private bool _bArrival = false;
	private float _changeAmountY = 0f;
	void Update()
	{
		//destPos
		Utility.Line.UpdateDebugLine(transform, this.name+"_destPos", transform.position, _destPos); //chamto test
		Utility.Line.UpdateDebugLineScale(this.name+"_destPos", transform.localScale);

		//velocity
		Utility.Line.UpdateDebugLine(transform, this.name+"_rigidVelocity", transform.position, transform.position + (Vector3)_rb2d.velocity ,Color.red); //chamto test
		Utility.Line.UpdateDebugLineScale(this.name+"_rigidVelocity", transform.localScale);

		//1.충돌 가능상태에서만 처리
		if (this.gameObject.layer == GlobalConstants.Layer.Num.superCat) 
		{
			//2.초기 ↗︎방향으로 설정되었을 때
			if (_sourcePos.y < _destPos.y) 
			{
				//3.↘︎ 아래방향으로 떨어지는 시점
				if(_rb2d.velocity.y < 0 )
				{
					//4.음수변화량이 기준치 이상으로 쌓였을 때
					if(_changeAmountY < -1.5f)
					{
						DebugWide.LogBlue("falling!  changeAmount y : " + _changeAmountY); //chamto test
						this.gameObject.layer = GlobalConstants.Layer.Num.default0;
						
					}else{
						DebugWide.LogWhite("changeAmount y : " + _changeAmountY); //chamto test
					}
				}
			}
		}
		_changeAmountY += _rb2d.velocity.y;


		//일정 거리 이동후 default 상태로 전환시키기 
		if (true == _bArrival && (transform.position - _sourcePos).sqrMagnitude >= 0.2f) 
		{
			Utility.Line.UpdateDebugLine(transform, this.name+"_arrivalDistance", _sourcePos, transform.position ,Color.black); //chamto test
			Utility.Line.UpdateDebugLineScale(this.name+"_arrivalDistance", transform.localScale);

			_bArrival = false;
			this.gameObject.layer = GlobalConstants.Layer.Num.default0;
		}

		//if(true == Input_Unity.IsTouch())
		if(TouchPhase.Began == Input_Unity.GetTouchEvent())
		{

			this.gameObject.layer = GlobalConstants.Layer.Num.superCat;

			_destPos = Input_Unity.GetTouchWorldPos ();
			_destPos.z = 0;
			_dir = _destPos - transform.position;

			_sourcePos = transform.position;
			_bArrival = true;
			_changeAmountY = 0f;
			_rb2d.velocity = Vector2.zero;

			//Debug.Log("Force : " + rb2d.velocity.sqrMagnitude); //chamto test
			//if(_rb2d.velocity.sqrMagnitude >= 100.0f)
			//	_rb2d.AddForce (_dir, ForceMode2D.Force);
			//else
				_rb2d.AddForce (_dir * 2, ForceMode2D.Impulse);
		}
		if (TouchPhase.Ended == Input_Unity.GetTouchEvent ()) 
		{
			//_rb2d.velocity = Vector2.zero; //cat stop
		}
		
		this.AniDirection (_destPos - transform.position);
	}


	bool _isContactBuilding = false;
	int _STATE = 0; //chamto temp
	Vector3 _dir = Vector3.zero;
	string temp = "";
	float sumTime1 = 0;
	float sumTime2 = 0;
	//void Update ()
	void FixedUpdate2222 ()
	{
		Utility.Line.UpdateDebugLine(transform, this.name+"_destPos", transform.position, _destPos); //chamto test
		Utility.Line.UpdateDebugLineScale(this.name+"_destPos", transform.localScale);

		if (true == Input_Unity.IsTouch ())
		{

			Vector3 touchPos = Input_Unity.GetTouchWorldPos ();

			//DebugWide.LogRed(GlobalConstants.Hierarchy.gameViewArea); //chamto test
			if( true == GlobalConstants.Hierarchy.gameViewArea.Contains(touchPos))
			{
				if (Cat.eMove.Super == _moveMode) 
				{
					_pathPos.Clear();
					_pathPos.Push(touchPos);
				}else
				{
					//_pathPos = _pathFinder.Search(transform.position, Input_Unity.GetTouchWorldPos ());
					_pathFinder.SearchNonAlloc(transform.position, touchPos, ref _pathPos);

					//chamto test
					Utility.Line.UpdateDebugLine(transform, this.name+"_path", _pathPos.ToArray(),Color.green, Color.black);
				}


				
				_STATE = 1;
				this.State_MoveNext();
			}
		}

		switch (_STATE) 
		{
		case 0: //idle
		{
			_dir = _destPos - transform.position;
			sumTime1 += Time.deltaTime;
			sumTime2 += Time.deltaTime;

			if(Cat.eArrive.Normal ==  _arriveMode)
			{
				//--------------------------------
				//normal
				//_rb2d.AddForce (_dir, ForceMode2D.Force); // == force
			}
			if(Cat.eArrive.Good ==  _arriveMode)
			{
				//--------------------------------
				//서로 나누어 먹는 느낌
//				if(_rb2d.velocity.sqrMagnitude <= 10.8f)
					_rb2d.AddForceAtPosition(_dir, _destPos, ForceMode2D.Impulse); //++ force
//				else
//					_rb2d.velocity = Vector2.zero; //cat stop


			}
			if(Cat.eArrive.Grabber ==  _arriveMode)
			{
				//--------------------------------
				//혼자 먹으려 싸우는 느낌
				_rb2d.MovePosition(_destPos); 

			}

		}
			break;
		case 1: //move to pos
		{
			DebugWide.LogRed("moveToPos  state-1 : "); //chamto test
			this.State_UpdateMoveToPos();

			if(true ==this.State_ArriveOn())
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
					State_MoveNext();
				}
			}

		}
			break;
		case 3: //falling
		{
			DebugWide.LogRed("falling  state-3 : " + _isContactBuilding); //chamto test
			if(true == _isContactBuilding)
			{
				_STATE = 1;
				//_isContactBuilding = false;
				this.gameObject.layer = GlobalConstants.Layer.Num.superCat;
			}
			
		}
			break;
		}//end switch
	
		this.AniDirection (_destPos - transform.position);

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
		option = Physics2D.GetIgnoreLayerCollision (GlobalConstants.Layer.Num.superCat, GlobalConstants.Layer.Num.building);
		Physics2D.IgnoreLayerCollision (GlobalConstants.Layer.Num.superCat, GlobalConstants.Layer.Num.building, true);

		if(this.gameObject.layer != GlobalConstants.Layer.Num.superCat)
			this.gameObject.layer = GlobalConstants.Layer.Num.superCat;
		else
			this.gameObject.layer = GlobalConstants.Layer.Num.default0;

		DebugWide.Log ("began");
	}
	void TouchMoved() 
	{
		DebugWide.Log ("moved");
	}
	void TouchEnded() 
	{
		DebugWide.Log ("ended");
	}

}
