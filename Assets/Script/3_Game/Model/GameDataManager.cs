using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//순수데이터만 다룬다. 입출력에 관한 어떠한 처리도 하지 않는다.


public class BaseData
{
	public UInt32	_uid;
}

public class CatData : BaseData
{
	//viewData
	public Byte 	_length;
	public Color	_color;
	public Byte		_face;

	//lifeData
	public Byte 	_health;	//건강
	public Byte 	_maxHealth;	//max건강
	public UInt16 	_lifeDays; 	//산날  
	public UInt16	_lifeSpan;	//수명

	//활동시간 , 잠 , 배고픔 , 병 , 갈증..

	public void InitDefault()
	{
		this._uid 		= 0;
		this._length 	= 1;
		this._color 	= Color.black;
		this._face 		= 0;

		this._health 	= 5;
		this._maxHealth = 5;
		this._lifeDays 	= 0;
		this._lifeSpan 	= 100;
	}

	static public CatData Create()
	{
		CatData data = new CatData ();
		data.InitDefault ();
		return data;
	}
}


//bag ⊃ feeds
public class BagData : BaseData
{
	public Byte		_feedCount;

	public void InitDefault()
	{
		this._uid = 0;
		this._feedCount = 5;
	}

	static public BagData Create()
	{
		BagData data = new BagData ();
		data.InitDefault ();
		return data;
	}

}

public class FeedData : BaseData
{
	public Int16 	_nutritious;  //영양가 - 좋은영양은 건강+ , 나쁜영양은 건강- 한다.

	public void InitDefault()
	{
		this._uid = 0;
		this._nutritious = +1;
	}

	public FeedData Create()
	{
		FeedData data = new FeedData ();
		data.InitDefault ();
		return data;
	}

}


public class DicBaseData : Dictionary<UInt32, BaseData>
{
}


public class GameDataManager  
{
	private UInt32	_keySecquence = 0;
	
	private DicBaseData _dicData = new DicBaseData ();



	private UInt32 createKey()
	{
		//사용후 반환된 키목록에, 키가 있으면 먼저 반환한다.
		//todo code..
		
		return _keySecquence++;
	}

	//init
	public void Init()
	{
		_keySecquence = 0;
		_dicData.Clear ();
	}

	//add
	public UInt32 Add(BaseData data)
	{
		//todo exception..

		data._uid = this.createKey ();
		_dicData.Add (data._uid, data);

		return data._uid;
	}

	//delete


	//update

	//get
	public BaseData GetData(UInt32 uid)
	{
		//todo exception..

		return _dicData [uid];
	}

}

public class PlayRoutine
{
	//init play

	//routine
}

public class GameStage
{
	private GameDataManager _gameData = null;
	private PlayRoutine 	_playRoutine = null;

	//init state
	public void InitState_1()
	{
		_gameData 		= new GameDataManager ();
		_playRoutine 	= new PlayRoutine ();

		_gameData.Add (CatData.Create ());
		_gameData.Add (CatData.Create ());
		_gameData.Add (CatData.Create ());
		_gameData.Add (CatData.Create ());
	}
}

