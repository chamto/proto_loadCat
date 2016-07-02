using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//순수데이터만 다룬다. 입출력에 관한 어떠한 처리도 하지 않는다.


//public class BaseData
//{
//}

public class CatData
{
	public UInt32	_uid;

	//viewData
	public Byte 	_length;
	public Color	_color;
	public Byte		_face;

	//lifeData
	public Byte 	_health;	//건강
	public UInt16 	_lifeDays; 	//산날  
	public UInt16	_lifeSpan;	//수명

	//활동시간 , 잠 , 배고픔 , 병 , 갈증..
}


//bag ⊃ feeds
public class BagData
{
	public UInt32	_uid;

	public Byte		_feedCount;

}

public class FeedData
{
	public UInt32	_uid;

	public Int16 	_nutritious;  //영양가 - 좋은영양은 건강+ , 나쁜영양은 건강- 한다.

}


public class DicCats : Dictionary<UInt32, CatData>
{

}

public class GameDataManager  
{
	public DicCats	_dicCats = new DicCats();
	//private Dictionary<UInt32, CatData>		_catList = new Dictionary<UInt32, CatData>();
	//private Dictionary<UInt32, BagData>		_bagList = new Dictionary<UInt32, BagData>();
	//private Dictionary<UInt32, FeedData>	_feedList = new Dictionary<UInt32, FeedData>();


	//add


	//delete


	//update

}
