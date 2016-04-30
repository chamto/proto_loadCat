using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WWWTest_MonoBehaviour : MonoBehaviour {

	private string url = "https://media.giphy.com/media/3rgXBsmYd60rL3w7sc/giphy.gif";


	// Use this for initialization
	void Start () 
	{
		url = "file://"+Application.dataPath + "/StreamingAssets/" + "townNode.xml";
		Debug.Log ("url : " + url);

		StartDownload ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	
	
	public void StartDownload()
	{
		StartCoroutine(DownloadFile());
	}
	
	public IEnumerator DownloadFile()
	{

		WWW client = new WWW(url);
		while (!client.isDone)
		{
			Debug.Log("progress  : " + client.progress);
			yield return 0;
		}
		Debug.Log("end  : "+client.progress);

		Debug.Log ("bytesLength : "+client.bytes.Length);
	}

}
