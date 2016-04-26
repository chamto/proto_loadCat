using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonoPathFinder : MonoBehaviour 
{

	private SparseGraph 	_graph = new SparseGraph (true);
	private Graph_SearchDFS _searchDFS = new Graph_SearchDFS();

	// Use this for initialization
	void Start () 
	{
		_graph.AddNode (new NavGraphNode (0, Vector2.zero));
		_graph.AddEdge (new GraphEdge (0, 1));


		_searchDFS.Init (_graph, 0, 10);
		List<int> pathList = _searchDFS.GetPathToTarget ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
