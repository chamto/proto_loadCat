using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

public class ResourceManager
{
	
	public Table.File_NodeInfo _nodeInfo = new Table.File_NodeInfo();
	
	//ex) XML_Manager.AsyncFileLoading(CDefine.ASSET_PATH + m_strFileName, value => stream = value)
	public static IEnumerator AsyncFileLoading(string strFilePath, System.Action<MemoryStream> result = null)
	{
		MemoryStream memStream = null;
		#if SERVER || TOOL 
		{
			//CDefine.CommonLog("1__" + strFilePath); //chamto test
			memStream = new MemoryStream(File.ReadAllBytes(strFilePath));
		}
		
		#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
		{
			
			UnityEngine.WWW wwwUrl = new UnityEngine.WWW(strFilePath);
			
			while (!wwwUrl.isDone)
			{   
				if (wwwUrl.error != null)
				{
					CDefine.DebugLogRed("error : " + wwwUrl.error.ToString());
					yield break;
				}
				CDefine.DebugLogGreen("wwwUrl.progress---" + wwwUrl.progress);
				yield return null;
			}
			
			if (wwwUrl.isDone)
			{   
				CDefine.DebugLogGreen("wwwUrl.isDone---size : "+wwwUrl.size);
				CDefine.DebugLogGreen("wwwUrl.isDone---bytesLength : "+wwwUrl.bytes.Length);
				memStream = new MemoryStream(wwwUrl.bytes);
			}
		}
		#endif
		
		if (null != result)
		{   
			result(memStream);
		}
		CDefine.DebugLogGreen("WWW Loading complete");
		yield return memStream;
	}
	
	public IEnumerator UnityFileLoading()
	{
		string strFilePath = CDefine.ASSET_PATH + "townNode.xml";
		CDefine.DebugLog ("-------------" + strFilePath + "-------------");
		
		MemoryStream memStream = null;
		
		{
			
			UnityEngine.WWW wwwUrl = new UnityEngine.WWW(strFilePath);
			
			while (!wwwUrl.isDone)
			{   
				if (wwwUrl.error != null)
				{
					CDefine.DebugLog("error : " + wwwUrl.error.ToString());
					yield break;
				}
				CDefine.DebugLog("wwwUrl.progress---" + wwwUrl.progress);
				yield return null;
			}
			
			if (wwwUrl.isDone)
			{   
				CDefine.DebugLog("wwwUrl.isDone---size : "+wwwUrl.size);
				CDefine.DebugLog("wwwUrl.isDone---bytesLength : "+wwwUrl.bytes.Length);
				memStream = new MemoryStream(wwwUrl.bytes);
			}
		}
		
		_nodeInfo.LoadXMLFromMemory (memStream); //chamto test
		
		CDefine.DebugLog("AsyncLoading complete");
		yield return memStream;
	}
	
	public void Load(bool bAsynchronous)
	{
		//fileLoding 
		Single.coroutine.StartCoroutine (_nodeInfo.LoadXML (),null, bAsynchronous ,"File_NodeInfo");
	}

	public void Load_ASync()
	{
		this.Load (true);
	}

	public void Load_Sync()
	{
		this.Load (false);
	}
	
	public bool IsCompleteLoad()
	{
		return _nodeInfo.bCompleteLoad;
	}
	
}
