using System;
using System.Collections;

/// <summary>
/// Global Single
/// </summary>
public class Single
{
	
	public static WideUseCoroutine coroutine
	{
		get
		{
			return CSingleton<WideUseCoroutine>.Instance;
		}
	}

	public static IEnumerator startCoroutine
	{
		set
		{
			coroutine.Start_Async(value);

		}
	}


	public static ResourceManager resource
	{
		get
		{
			return CSingleton<ResourceManager>.Instance;
		}
	}

	public static HierarchyPreLoader hierarchy
	{
		get
		{
			return CSingleton<HierarchyPreLoader>.Instance;
		}
	}

	public static DayAndNight_MonoBehaviour dayAndNight
	{
		get
		{
			return CSingletonMono<DayAndNight_MonoBehaviour>.Instance;
		}
	}
	
	public static GameStage gameStage
	{
		get
		{
			return CSingleton<GameStage>.Instance;
		}
	}
}
