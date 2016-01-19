using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LogicManager : MonoBehaviour {

	[SerializeField] Camera viewCamera;
	[SerializeField] public int blockRows;
	[SerializeField] public int blockCols;
	[SerializeField] public float blockDisWidth;
	[SerializeField] public float blockDisHeight;
	[SerializeField] float clickTime = 0.1f;
	[SerializeField] GameObject backBoardPrefab;
	[SerializeField] GameObject tokenPrefab;
	[SerializeField] public float tempTime;
	int tempTokenInfoIndex = 0 ;
	[SerializeField] float AllEndTime = 180f;
	[SerializeField] float endTime = 320f;
	[SerializeField] AudioSource overWhelmMusic;

	public List<List<BackBlock>> backbloctMat = new List<List<BackBlock>>();
	
	public LogicManager() { s_Instance = this; }
	public static LogicManager Instance { get { return s_Instance; } }
	private static LogicManager s_Instance;

	Token tempToken{
		get{
			return _tempToken;
		}
		set{
			if ( _tempToken != value )
			{
				if ( _tempToken != null )
					_tempToken.Unchoose();
				if( value != null )
					value.Choose();
			}
			_tempToken = value;
		}
	}
	Token _tempToken;
	Token hoverToken{
		get{
			return _hoverToken;
		}
		set{
			if ( _hoverToken != value )
			{
				if ( _hoverToken != null )
					_hoverToken.Unhover();
				if( value != null )
					value.Hover();
			}
			_hoverToken = value;
		}
	}
	Token _hoverToken;
	Token tempDragToken;
	Token tempDeleteToken;
	float moveDistance;

	void OnEnable()
	{
		BEventManager.Instance.RegisterEvent(EventDefine.OnDeleteToken , OnDeleteToken);
	}
	void OnDisable()
	{
		BEventManager.Instance.UnregisterEvent(EventDefine.OnDeleteToken , OnDeleteToken);
	}


	void OnDeleteToken(EventDefine eventName, object sender, EventArgs args) 
	{
		Resources.UnloadUnusedAssets();
	}
	// Use this for initialization
	void Awake () {
		SetUpBackBlocks();
		StartCoroutine ( WaitForAllEnd(AllEndTime) );
		StartCoroutine ( WaitForEnd( ));
	}
	
	// Update is called once per frame
	void Update () {

		Ray rayToMouse = viewCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(rayToMouse, out hitInfo, 1.0e8f))
		{
			hoverToken = hitInfo.collider.gameObject.GetComponent<Token>();
		}else
		{
			hoverToken = null;
		}

		if ( hoverToken != null )
			Debug.Log( "Hover " + hoverToken.movieName );

//		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) )
//		{
//				if ( hoverToken != null )
//				{
//					tempToken = hoverToken;
//					// left click
//					if( Input.GetMouseButtonDown(0) )
//					{
//						tempDragToken = tempToken;
//						tempDragToken.BeginMove(hitInfo.point);
//	//					tempToken.BeginMove( hitInfo.point );
//						moveDistance = hitInfo.distance;
//						StartCoroutine( WaitForMouseUp( clickTime , hitInfo.point , true) );
//					}else 
//					//rightclick
//					if ( Input.GetMouseButtonDown( 1 ))
//					{
//						StartCoroutine( WaitForMouseUp( clickTime , hitInfo.point, false) );
//					}
//				}
////				if (!Physics.Raycast(ray, hitInfo.distance - 0.01f))
////					StartCoroutine(coHandleButtonPress(-1));
//		}

		if (Input.GetMouseButtonDown(0)  )
		{
			if ( hoverToken != null )
			{
				tempToken = hoverToken;
				tempDragToken = tempToken;
				tempDragToken.BeginMove(hitInfo.point);
				moveDistance = hitInfo.distance;
				StartCoroutine( WaitForMouseUp( clickTime , hitInfo.point , true) );
			}

		}

		if ( Input.GetMouseButtonDown(1))
		{
			if ( hoverToken != null )
			{
				tempToken = hoverToken;
				tempDeleteToken = tempToken;
				StartCoroutine( WaitForMouseUp( clickTime , hitInfo.point, false) );
			}
		}


		if ( Input.GetMouseButtonUp(0) && tempDragToken != null )
		{
			tempDragToken.EndMove();
			tempDragToken = null;
		}

		// move drag
		if ( tempDragToken != null )
		{
			Vector3 position = rayToMouse.GetPoint( moveDistance );
			tempDragToken.Move( position );
		}

		//delete
		if ( tempDeleteToken != null )
		{
			if ( tempDeleteToken.Delete() )
				tempDeleteToken = null;
		}

		//managee the TokenCreation
		tempTime += Time.deltaTime;
		while( tempTokenInfoIndex < TimeLine.tokenInfoList.Count )
		{
			if ( TimeLine.tokenInfoList[tempTokenInfoIndex].time > tempTime )
				break;
//			Debug.Log("Index = "  + tempTokenInfoIndex.ToString() );
//			Debug.Log("movieName" + TimeLine.tokenInfoList[tempTokenInfoIndex].name);
			if ( tempTime - 1f < TimeLine.tokenInfoList[tempTokenInfoIndex].time )
				CreateNewToken( TimeLine.tokenInfoList[tempTokenInfoIndex] );
			tempTokenInfoIndex++;
		}

		//exit game
		if ( Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	void SetUpBackBlocks(){
		for( int i = 0 ; i < blockRows ; ++ i )
		{
			List<BackBlock> cblist = new List<BackBlock>();
			for ( int j = 0 ; j < blockCols ; ++ j )
			{
				GameObject block = Instantiate( backBoardPrefab ) as GameObject;
				block.name = block.name + i.ToString() + j.ToString();
				cblist.Add( block.GetComponent<BackBlock>());
				float posX = blockDisWidth * ( j + 0.5f - 0.5f * blockCols );
				float posY = blockDisHeight * ( - i - 0.5f + 0.5f * blockRows );
				block.transform.position = new Vector3(posX , posY , 0 );
				block.transform.parent = this.transform;
			}
			backbloctMat.Add( cblist );
		}
	}

//	IEnumerator WaitForMouseUp( float clickTime , Vector3 hitpoint , bool isLeft)
//	{
//		float timer = 0 ;
//		while(true)
//		{
//			timer += Time.deltaTime;
////			Debug.Log("Wait for mouse up"+timer);
//			if( clickTime < timer )
//			{
//				tempToken = null;
//				yield break;
//			}
//			if ( Input.GetMouseButtonUp(0) && isLeft )
//			{
//				UpperSize( tempToken );
//				tempToken = null;
//				yield break;
//			}
//			if ( Input.GetMouseButtonUp( 1 ) && !isLeft )
//			{
//				LowerSize(tempToken );
//				tempToken = null;
//				if ( tempDeleteToken )
//					tempDeleteToken.StopDelete();
//				tempDeleteToken = null;
//				yield break;
//			}
//			yield return null;
//		}
//	}


	IEnumerator WaitForMouseUp( float clickTime , Vector3 hitpoint , bool isLeft)
	{
		float timer = 0 ;
		while(true)
		{
			timer += Time.deltaTime;
			//			Debug.Log("Wait for mouse up"+timer);
//			if( clickTime < timer )
//			{
//				tempToken = null;
//				yield break;
//			}
			if ( Input.GetMouseButtonUp(0) && isLeft )
			{
				if ( clickTime > timer )
					UpperSize( tempToken );
				tempToken = null;
				tempDragToken = null;
				Debug.Log("Left Mouse Up " + timer.ToString());
				yield break;
			}
			if ( Input.GetMouseButtonUp( 1 ) && !isLeft )
			{
				if ( clickTime > timer )
					LowerSize(tempToken );
				tempToken = null;
				if ( tempDeleteToken )
					tempDeleteToken.StopDelete();
				tempDeleteToken = null;
				Debug.Log("Right Mouse Up " + timer.ToString());
				yield break;
			}
			yield return null;
		}
	}

	IEnumerator WaitForAllEnd( float allEndTime )
	{
		while(true)
		{
			if (  tempTime > allEndTime - 32f && tempTime < allEndTime - 31f && !overWhelmMusic.isPlaying)
				overWhelmMusic.Play();

			if( tempTime > allEndTime )
			{
				StopCoroutine( "CreateNewTokenRandomly" );
				StopCoroutine( "CreateNewTokenFixed" );
				Debug.Log("Send all end");
				BEventManager.Instance.PostEvent( EventDefine.OnAllEnd );
				yield break;
			}
			yield return null;
		}
	}

	IEnumerator WaitForEnd(  )
	{
		while(true)
		{
			if (  tempTime > endTime )
			{
				Application.LoadLevel( "front" );
				yield break;
			}
			yield return null;
		}
	}

	void UpperSize( Token token)
	{
		token.UpperSize();
	}

	void LowerSize( Token token )
	{
		token.LowerSize();
	}

//	public void CreateNewToken( TimeLine.TokenInfo tokenInfo)
//	{
//		CreateNewToken( tokenInfo.name , tokenInfo.size , tokenInfo.i , tokenInfo.j, tokenInfo.duration, tokenInfo.maxSize , tokenInfo.eat , tokenInfo.delete);
//	}

	public void CreateNewToken( TimeLine.TokenInfo tokenInfo ,  int maxTry = 300  )
	{
		if ( tokenInfo.i == tokenInfo.j || tokenInfo.j == -1 )
			StartCoroutine( CreateNewTokenRandomly( tokenInfo,  maxTry ));
		else
			StartCoroutine( CreateNewTokenFixed( tokenInfo ));
	}

	IEnumerator CreateNewTokenRandomly(  TimeLine.TokenInfo tokenInfo ,  int maxTry = 300  )
	{
		GameObject tokenObj = Instantiate( tokenPrefab ) as GameObject;
		Token token = tokenObj.GetComponent<Token>();
		int i = tokenInfo.i;
		int j = tokenInfo.j;

		if ( i == -1 || i >= blockRows)
		{
			i = UnityEngine.Random.Range(0,blockRows);
		}
		if ( j == -1 || j >= blockCols )
		{
			j = UnityEngine.Random.Range(0,blockCols);
		}

		BackBlock targetBackblock = backbloctMat[i][j];

		while(maxTry > 0)
		{
			if ( token.Init( targetBackblock , tokenInfo ))
				yield break;
			maxTry --;
			if ( tokenInfo.i.Equals( -1 ) )
				i = UnityEngine.Random.Range(0,blockRows);
			if ( tokenInfo.j.Equals( -1 ))
				j = UnityEngine.Random.Range(0,blockCols);
			targetBackblock = backbloctMat[i][j];
			yield return null;
		}
	}

	IEnumerator CreateNewTokenFixed( TimeLine.TokenInfo tokenInfo  )
	{
		GameObject tokenObj = Instantiate( tokenPrefab ) as GameObject;
		Token token = tokenObj.GetComponent<Token>();
		int i = tokenInfo.i;
		int j = tokenInfo.j;
		
		if ( i == -1 || i >= blockRows)
		{
			i = UnityEngine.Random.Range(0,blockRows);
		}
		if ( j == -1 || j >= blockCols )
		{
			j = UnityEngine.Random.Range(0,blockCols);
		}
		
		BackBlock targetBackblock = backbloctMat[i][j];
		
		while(true)
		{
			if ( token.Init( targetBackblock, tokenInfo ))
				yield break;
			yield return null;
		}
	}
	void OnGUI()
	{
//		GUILayout.TextField("Temp Time:"+tempTime.ToString());
//		if( overWhelmMusic.isPlaying )
//			GUILayout.TextField("Music Time:"+overWhelmMusic.time.ToString());
	}
}
