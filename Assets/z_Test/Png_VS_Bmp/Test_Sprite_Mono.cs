using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class Test_Sprite_Mono : MonoBehaviour 
{

	public enum SpriteOrientation
	{
		TopLeft,
		MiddleCenter
	}

	public Vector2 spriteTopLeft;
	public Vector2 spriteSize;
	public Material spriteMaterial;
	public float defCameraPixels = 768f;
	public SpriteOrientation spriteOrientation;
	public Vector2 progressBar;

	protected Mesh _mesh;
	protected MeshRenderer _renderer;

	void Awake()
	{
		if (spriteMaterial == null || spriteMaterial.mainTexture == null) 
		{
			Debug.LogError("null is material");
			return;
		}

		MeshFilter mf = gameObject.GetComponent<MeshFilter>();
		if (mf == null) 
		{
			mf = gameObject.AddComponent<MeshFilter>();
		}

		_renderer = gameObject.GetComponent<MeshRenderer> ();
		if (_renderer == null) 
		{
			_renderer = gameObject.AddComponent<MeshRenderer>();
		}
		_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		_renderer.receiveShadows = false;
		_renderer.sharedMaterial = spriteMaterial;

		float pixelPerWorldUnit = 
			Camera.main.orthographicSize * 2 / defCameraPixels;

		_mesh = new Mesh ();
		_mesh.name = "SpriteMesh";

		if (spriteOrientation == SpriteOrientation.MiddleCenter) {
			_mesh.vertices = new Vector3[]
			{
				new Vector3 (-spriteSize.x, -spriteSize.y) * pixelPerWorldUnit * 0.5f, //left-down 0
				new Vector3 (-spriteSize.x, spriteSize.y) * pixelPerWorldUnit * 0.5f,  //left-up 1
				new Vector3 (spriteSize.x, -spriteSize.y) * pixelPerWorldUnit * 0.5f, //right-down 2
				new Vector3 (spriteSize.x, spriteSize.y) * pixelPerWorldUnit * 0.5f  //right-up 3
			};
		} else if(spriteOrientation == SpriteOrientation.TopLeft)
		{
			_mesh.vertices = new Vector3[]
			{
				new Vector3(0,-spriteSize.y) * pixelPerWorldUnit,
				new Vector3(0, 0) * pixelPerWorldUnit,
				new Vector3(spriteSize.x,-spriteSize.y) * pixelPerWorldUnit,
				new Vector3(spriteSize.x, 0) * pixelPerWorldUnit
			};
		}

		_mesh.triangles = new int[] {0, 1, 3, 0, 3, 2};

		float texWidth = _renderer.sharedMaterial.mainTexture.width;
		float texHeight = _renderer.sharedMaterial.mainTexture.height;

		_mesh.uv = new Vector2[] 
		{
			new Vector2(1f/texWidth * spriteTopLeft.x,
			            1f-1f/texHeight * (spriteTopLeft.y + spriteSize.y)),
			new Vector2(1f/texWidth * spriteTopLeft.x,
			            1f-1f/texHeight * (spriteTopLeft.y )),
			new Vector2(1f/texWidth * (spriteTopLeft.x + spriteSize.x),
			            1f-1f/texHeight * (spriteTopLeft.y + spriteSize.y)),
			new Vector2(1f/texWidth * (spriteTopLeft.x + spriteSize.x),
			            1f-1f/texHeight * (spriteTopLeft.y))
		};



		_mesh.Optimize ();
		_mesh.RecalculateNormals ();
		_mesh.RecalculateBounds ();
		mf.sharedMesh = _mesh;

	}


	
	public bool _Update_perform = false;
	void Update () 
	{
		if (true == _Update_perform) 
		{
			//code..

			//chamto 20160611
			//각각의 단위가 존재
			//하나는 화면의 세로 픽셀 개수
			//하나는 3d카메라의 월드 세로 길이 (줄여서 월드길이)
			//화면의 픽셀 하나에 대한 월드 길이를 계산할려면?  = 월드길이 / 픽셀 개수 
			//전체월드길이에 대하여 픽셀개수로 나누면 하나의 픽셀에 대한 월드길이가 나온다.
			//* 나눗셈은 등분하겠다는 의미임을 생각하자 !
			float pixelPerWorldUnit = 
				Camera.main.orthographicSize *2 / defCameraPixels;

			if (spriteOrientation == SpriteOrientation.MiddleCenter) {
				_mesh.vertices = new Vector3[]
				{
					new Vector3 (-spriteSize.x, -spriteSize.y) * pixelPerWorldUnit * 0.5f, //left-down 0
					new Vector3 (-spriteSize.x, spriteSize.y) * pixelPerWorldUnit * 0.5f,  //left-up 1
					new Vector3 (spriteSize.x, -spriteSize.y) * pixelPerWorldUnit * 0.5f, //right-down 2
					new Vector3 (spriteSize.x, spriteSize.y) * pixelPerWorldUnit * 0.5f  //right-up 3
				};
			} else if(spriteOrientation == SpriteOrientation.TopLeft)
			{
				_mesh.vertices = new Vector3[]
				{
					new Vector3(0,-spriteSize.y) * pixelPerWorldUnit,
					new Vector3(0, -progressBar.y) * pixelPerWorldUnit,
					new Vector3(spriteSize.x,-spriteSize.y ) * pixelPerWorldUnit,
					new Vector3(spriteSize.x, -progressBar.y) * pixelPerWorldUnit
				};
			}
			
			_mesh.triangles = new int[] {0, 1, 3, 0, 3, 2};
			
			float texWidth = _renderer.sharedMaterial.mainTexture.width;
			float texHeight = _renderer.sharedMaterial.mainTexture.height;

			//opengl의 텍스쳐좌표 원점은 dirextx와 달리 좌하단이다.
			//유니티는 opengl의 텍스쳐좌표 방식을 사용하는것 같다.
			//유니티의 월드좌표계는  dirextx의 왼손좌표계를 사용한다.
			//텍스쳐좌표가 좌하단에서 부터 상단으로 증가되므로 텍스쳐가 뒤집혀 보이게 된다.
			//이 때문에 세로축 uv값에 역수(1-uv)를 취하여 상/하를 다시 뒤집는다.  
			Vector2 texelPerUvUnit = new Vector2(1f/texWidth , 1f/texHeight);

			_mesh.uv = new Vector2[] 
			{
				new Vector2(texelPerUvUnit.x * spriteTopLeft.x,
				            1f- (texelPerUvUnit.y * (spriteTopLeft.y + spriteSize.y ))),	//left-up 1

				new Vector2(texelPerUvUnit.x * spriteTopLeft.x,
				            1f- (texelPerUvUnit.y * (spriteTopLeft.y + progressBar.y))),					//left-down 0

				new Vector2(texelPerUvUnit.x * (spriteTopLeft.x + spriteSize.x),			
				            1f- (texelPerUvUnit.y * (spriteTopLeft.y + spriteSize.y ))),	//right-up 3

				new Vector2(texelPerUvUnit.x * (spriteTopLeft.x + spriteSize.x),
				            1f- (texelPerUvUnit.y * (spriteTopLeft.y + progressBar.y)))				//right-down 2
			};

			//_mesh.uv [0].y += progressBar.y;
			//_mesh.uv [3].y += progressBar.y;

			//----
//			_mesh.uv = new Vector2[] 
//			{
//				new Vector2(0,0),
//				new Vector2(0,1 + 0.2f), //top
//				new Vector2(1,0),
//				new Vector2(1,1 + 0.2f)  //top
//			};

			foreach(Vector2 v2 in _mesh.uv)
			{
				DebugWide.LogBlue(v2); //chamto test
			}
			DebugWide.LogBlue("offset "+_renderer.sharedMaterial.mainTextureOffset);
			DebugWide.LogBlue("scale "+_renderer.sharedMaterial.mainTextureScale);
			//----

			_mesh.Optimize ();
			_mesh.RecalculateNormals ();
			_mesh.RecalculateBounds ();


			_Update_perform = false;
		}
	}
}
