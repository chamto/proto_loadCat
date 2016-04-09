using UnityEngine;
using System.Collections;

public class GlobalConstants : MonoBehaviour 
{

	public class Hash_Ani
	{
		private static int _hide = Animator.StringToHash("Base Layer.state_hide");
		public static int hide {get {	return _hide; }}

		private static int _rush = Animator.StringToHash("Base Layer.state_rush");
		public static int rush {get {	return _rush; }}

		private static int _eat = Animator.StringToHash("Base Layer.state_eat");
		public static int eat {get {	return _eat; }}

	}

	// Use this for initialization
	void Start () 
	{
		//Debug.Log ("aaaaa");	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
