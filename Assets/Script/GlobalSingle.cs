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

}
