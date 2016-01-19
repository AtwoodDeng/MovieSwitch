using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Assertions;
using Holoville.HOTween;
//using DG.Tweening;
using System;

public class Token : MonoBehaviour {

	public enum MoveType
	{
		Bounce,
		Smooth
	}

	[SerializeField] Renderer movieRender;
	[SerializeField] public string movieName;
	[SerializeField] tk2dSprite whiteBlock;
	[SerializeField] tk2dSprite eatBlock;
	[SerializeField] tk2dSprite deleteBlock;
	[SerializeField] Renderer blurRender;
	[SerializeField] AudioSource audioSource;
	[SerializeField] public int size = 1;
	[SerializeField] public int maxSize = 3;
	[SerializeField] AnimationCurve stickCurve;
	[SerializeField] float stickTime = 0.4f;
	[SerializeField] float resizeTime = 0.4f;
	[SerializeField] float chooseTime = 0.4f;
	[SerializeField] float hoverTime = 0.1f;
	[SerializeField] float eatTime = 0.15f;
	[SerializeField] float deleteTime = 1.0f;
	[SerializeField] MoveType moveStickType;
	[SerializeField] float deleteblockMax = 15f;
	[SerializeField] Color reverseColor;
	[SerializeField] float fadeTime = 1.5f;

	public bool move = true;
	public bool resize = true;
	public bool eat = false;
	public bool delete = false;
	bool isMoving = false;
	Vector3 moveBias;
	List<BackBlock> targetBackBlock = null;
	List<BackBlock> tempBackBlock = null;
	float deleteTimer = 0f;

	void OnEnable()
	{
		BEventManager.Instance.RegisterEvent(EventDefine.OnAllEnd , OnAllEnd);
	}
	void OnDisable()
	{
		BEventManager.Instance.UnregisterEvent(EventDefine.OnAllEnd , OnAllEnd);
	}

	void OnAllEnd(EventDefine eventName, object sender, EventArgs args) 
	{
		Debug.Log("Allend");
		EndLife();
	}
	
	void Awake()
	{
		whiteBlock.renderer.material.renderQueue = 3000 + 10;
		eatBlock.renderer.material.renderQueue = 3000 + 10;
		deleteBlock.renderer.material.renderQueue = 3000 + 10;
		blurRender.material.renderQueue = 3000 + 15 ;
		movieRender.material.renderQueue = 3000 + 5;
	}

	public bool ifInit = false;
	public bool Init( BackBlock block, TimeLine.TokenInfo tokenInfo )
	{
		maxSize = tokenInfo.maxSize;
		move = tokenInfo.move;
		resize = tokenInfo.resize;
		eat = tokenInfo.eat;
		delete = tokenInfo.delete;
		if ( tokenInfo.size == -1 )
			size = UnityEngine.Random.Range(1 , 4);
		else
			size = tokenInfo.size;
		if ( !TryAttachTo( block ) )
			return false;
		AdjustSize( size );
		SetMovie( tokenInfo.name );
		StartCoroutine(CheckEnd(tokenInfo.duration));
		ifInit = false;
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage("name" , movieName );
		BEventManager.Instance.PostEvent( EventDefine.OnTokenInit , msg );
		return true;
	}

	public void Move(  Vector3 mousePosition ){
		if ( !move )
			return;
		this.transform.position = mousePosition + moveBias ;
	}

	public void BeginMove( Vector3 hitPosition ) {
		if ( !move )
			return;
		isMoving = true;
		moveBias = this.transform.position - hitPosition;
		SetTempBackblock( null , false );
	}

	public void EndMove() {
		if ( !move )
			return;
		isMoving = false;
		StickToTargetBlock( stickTime , moveStickType );
		SetTargetBlock(null);
	}

	public bool Delete( ) {
		if ( !delete )
			return false;
		deleteTimer += Time.deltaTime;
		if ( deleteTimer > deleteTime )
		{
			EndLife();
			return true;
		}
		setDeleteBlock();
		return false;
	}

	public void StopDelete()
	{
		StartCoroutine(StopDeletePlay());
	}

	IEnumerator StopDeletePlay()
	{
		while( deleteTimer > 0.1f)
		{
			deleteTimer *= 0.9f;
			setDeleteBlock();
			yield return null;
		}
	}

	void setDeleteBlock()
	{
		deleteBlock.color = Global.changeAlpha(deleteBlock.color , deleteTimer / deleteTime );
		deleteBlock.scale = new Vector3( deleteTimer / deleteTime * deleteblockMax , 0.5f / size  , deleteBlock.scale.z );
	}

	public void SetMovie(string name)
	{
		movieName = name;
		Debug.Log("SetMovie" + name );
		MovieTexture movie = Resources.Load("Movie/" + name ) as MovieTexture;
		if ( movie == null )
			Debug.LogError("There is no "+name);
		movieRender.material.mainTexture = movie;
		audioSource.clip = movie.audioClip;
		audioSource.volume = GetAudioSoundAsSize();
		if ( eat )
		{
			movie.Pause();
			audioSource.Pause();
			eatBlock.color = Global.changeAlpha( eatBlock.color , 0.4f );
			if ( name.StartsWith( "Reverse") )
			{
				eatBlock.color = reverseColor;
//				movie.Play();
			}
		}else{
			movie.Play();
			audioSource.Play();
		}
	}


	public float GetAudioSoundAsSize()
	{
		return Mathf.Pow(1.0f * size / maxSize , 2.0f);
	}

	public bool TryAttachTo( BackBlock backblock , bool withInitPos = true )
	{
		List<BackBlock> checkBackBlock = CheckBackBlock( backblock );
		if ( checkBackBlock == null )
		{
			return false;
		}
		SetTempBackblock(checkBackBlock);
		float moveTime = stickTime;
		if ( withInitPos )
			moveTime = 0;
		StickToTargetBlock(moveTime , MoveType.Smooth , false ); //  just for position

		return true;
	}

	List<BackBlock> CheckBackBlock( BackBlock back )
	{
		int i = (int) (back.name[back.name.Length-2]-'0');
		int j = (int) (back.name[back.name.Length-1]-'0');

		//get Check mat list
		List<BackBlock> checkList = new List<BackBlock>();
		
		checkList.Add(back);

		if ( i + size -1 >= LogicManager.Instance.blockRows )
			i = LogicManager.Instance.blockRows - size ;
		if ( j + size -1 >= LogicManager.Instance.blockCols )
			j = LogicManager.Instance.blockCols - size ;

		for( int ii = i ; ii < i + size ; ++ii )
		{
			for ( int jj = j ; jj < j + size ; ++jj )
			{
				checkList.Add(LogicManager.Instance.backbloctMat[ii][jj]);
			}
		}

		foreach( BackBlock block in checkList)
		{
			if ( block.isOcupied && ( block.occupyToken != null && !block.occupyToken.eat) )
			{
				Debug.Log(block.name+" is ocupied");
				return null;
			}
		}

		return checkList;
	}

	void SetTargetBlock( List<BackBlock> newBlockList , bool isReplace = true )
	{
		if ( targetBackBlock != null )
			foreach(BackBlock block in targetBackBlock )
			{
				block.Disactive();
			}
		if ( newBlockList != null )
			foreach(BackBlock block in newBlockList )
			{
				block.Active();
			}
		if ( isReplace )
			targetBackBlock = newBlockList;
	}

	void OnTriggerEnter(Collider collider)
	{
		if ( isMoving )
		{
			if ( collider.GetComponent<BackBlock>() != null )
			{
//				Debug.Log("Collison on" + collider.name );
				List<BackBlock> checkList = CheckBackBlock(collider.GetComponent<BackBlock>());
				if ( checkList != null )
				{
					SetTargetBlock( checkList );
				}else
				{
					SetTargetBlock( null );
				}
			}
		}
	}

	void SetTempBackblock( List<BackBlock> newBackblock , bool ifReplace = true )
	{
		if ( tempBackBlock != null)
			foreach(BackBlock block in tempBackBlock )
			{
				block.Unoccupy();
			}
		if ( newBackblock != null)
			foreach(BackBlock block in newBackblock )
			{
				block.Occupy(this);
			}
		if (ifReplace )
			tempBackBlock = newBackblock;
	}

	BackBlock checkEat( List<BackBlock> backblocks )
	{
		foreach(BackBlock bb in backblocks)
		{
			if( bb.occupyToken != null && bb.occupyToken != this && bb.occupyToken.eat )
				return bb;
		}
		return null;
	}

	void StickToTargetBlock ( float duration ,  MoveType moveType = MoveType.Bounce , bool ifOccupy = true  )
	{ 
		if ( targetBackBlock != null )
		{
			if ( checkEat(targetBackBlock) != null )
			{
				BeEaten( checkEat(targetBackBlock).occupyToken );
				return;
			}
			if ( ifOccupy )
				SetTempBackblock( targetBackBlock );
		}
		if (tempBackBlock == null)
			return;
		SetTempBackblock(tempBackBlock);
		Vector3 blockSize = new Vector3( LogicManager.Instance.blockDisWidth , - LogicManager.Instance.blockDisHeight , 0 );
		Vector3 centerPosition = tempBackBlock[1].transform.position + blockSize * ( size - 1 ) * 0.5f;
		Vector3 toward = centerPosition - this.transform.position;
		switch( moveType )
		{
		case MoveType.Bounce:
			StartCoroutine(StickBounce(toward , centerPosition , duration ));
			break;
		case MoveType.Smooth:
			StartCoroutine(StickSmooth(toward , centerPosition , duration ));
			break;
		default:
			break;
		};

		SetTargetBlock(null);
	}

	void BeEaten( Token token )
	{
		Debug.Log(movieName +" be eaten");
//		if ( targetBackBlock == null || targetBackBlock[1].occupyToken ==null)
//			return;
		token.EatToken(this);
		EndLife();
	}

	public void EatToken(Token token)
	{
		eat = false;
		HOTween.To(eatBlock , eatTime , "color" , Global.changeAlpha( eatBlock.color, 0 ));


		((MovieTexture)movieRender.material.mainTexture).Play();
		audioSource.Play();

		UpperSize();
	}

	IEnumerator StickBounce( Vector3 toward , Vector3 target , float TotalTime )
	{
		float timer = 0;
		while(true)
		{
			timer += Time.deltaTime;
			if ( TotalTime < timer )
			{
				yield break;
			}
			this.transform.position = target - stickCurve.Evaluate(timer/TotalTime) * toward;
			yield return null;
		}
	}

	IEnumerator StickSmooth( Vector3 toward , Vector3 target , float TotalTime )
	{
		float timer = 0;
		while(true)
		{
			timer += Time.deltaTime;
			if ( TotalTime < timer )
				yield break;
			transform.position = target - ( 1f - timer / TotalTime ) * toward;
			yield return null;
		}
	}

	public void UpperSize()
	{
		if( !resize )
			return ;
		if ( size < maxSize )
		{
			AdjustSize( size + 1 );
		}
	}

	public void LowerSize()
	{
		if( !resize )
			return;
//		Debug.Log("LowerSize");
		if ( size > 1 )
		{
			AdjustSize( size - 1 );
		}
	}

	bool AdjustSize( int toSize )
	{
		if ( tempBackBlock != null )
		{
			int oriSize = size;
			size = toSize;
			SetTempBackblock( null , false );
			List<BackBlock> newBlock = CheckBackBlock( tempBackBlock[0] );
			if ( newBlock != null )
			{
				SetTargetBlock(newBlock);
				StickToTargetBlock(stickTime , MoveType.Bounce);
				SetTempBackblock(newBlock);
			}else{
				size = oriSize;
				return false;
			}

		 }else
		{
			return false;
		}
		HOTween.To( transform , resizeTime , "localScale" , Vector3.one * toSize );
		HOTween.To( audioSource , resizeTime , "volume" , GetAudioSoundAsSize() );
		size = toSize;
		return true;
	}

	IEnumerator CheckEnd( float duration )
	{
		float timer = 0;
		MovieTexture movie = movieRender.material.mainTexture as MovieTexture;
		while( true )
		{
//			if ( !eat )
				timer += Time.deltaTime;
			if ( timer > duration && duration > 0 )
			{
				break;
			}
			if ( timer > duration - fadeTime && duration > 0 )
			{
				if ( audioSource != null )
				{
					HOTween.To( audioSource , fadeTime - 0.05f , "volume" , 0 );
				}
			}
			if ( (!movie.isPlaying) && (!eat ) )
			{
				break;
			}
			yield return null;
		}
		EndLife();
		yield break;
	}

	void EndLife()
	{
		Debug.Log(movieName + " end life");
		HOTween.To( transform , resizeTime , "localScale" , Vector3.one * 0.01f);
		HOTween.To( audioSource , resizeTime , new TweenParms().Prop("volume",0).OnComplete(DestorySelf));
		SetTempBackblock(null);
		if ( ((MovieTexture)movieRender.material.mainTexture) != null )
			((MovieTexture)movieRender.material.mainTexture).Stop();
		audioSource.Stop();

	}

	void DestorySelf()
	{
		Destroy(gameObject);
		BEventManager.Instance.PostEvent(EventDefine.OnDeleteToken);
	}

	public void Hover()
	{
		if ( !isMoving && (move || resize || delete) )
			HOTween.To( whiteBlock , hoverTime , "color" , Global.changeAlpha( whiteBlock.color , 0.3f ));
	}

	public void Unhover()
	{
		HOTween.Kill( whiteBlock );
		HOTween.To( whiteBlock , hoverTime , "color" , Global.changeAlpha( whiteBlock.color , 0.0f ));
	}

	public void Choose()
	{
		HOTween.To( whiteBlock , chooseTime , "color" , Global.changeAlpha( whiteBlock.color , 0.0f ));
	}

	public void Unchoose()
	{
		HOTween.Kill( whiteBlock );
		HOTween.To( whiteBlock , chooseTime , "color" , Global.changeAlpha( whiteBlock.color , 0.0f ));
	}

}
