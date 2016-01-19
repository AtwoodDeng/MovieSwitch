using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;

public class BackBlock : MonoBehaviour {

	public bool isOcupied = false;
	public bool isActive = false;
	[SerializeField] tk2dSprite whiteBlock;
	[SerializeField] tk2dSprite whiteFrame;
	[SerializeField] float fadeTime = 1f;
	[SerializeField] float halfViewAlpha = 0.4f;
	public Token occupyToken = null;
	[SerializeField] Color occupyColor;


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
		isOcupied = false;
	}


	// Use this for initialization
	void Awake () {
//		whiteBlock.gameObject.GetComponent<Renderer>().material.renderQueue = 3000 - 5;
		whiteBlock.transform.localScale = Vector3.one * 0.01f;
		whiteBlock.color = Global.changeAlpha(whiteBlock.color, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Active()
	{
		if ( isActive )
			return;
		HOTween.To( whiteBlock.transform , fadeTime , "localScale" , Vector3.one );
		HOTween.To( whiteBlock , fadeTime , "color", Global.changeAlpha(whiteBlock.color , halfViewAlpha ) );
		isActive = true;
//		whiteBlock.gameObject.SetActive(true);
	}
	public void Disactive()
	{
		if ( !isActive )
			return;
		HOTween.To( whiteBlock.transform , fadeTime , "localScale" , Vector3.one * 0.01f );
		HOTween.To( whiteBlock , fadeTime , "color", Global.changeAlpha(whiteBlock.color , 0 ) );
		isActive = false;
//		whiteBlock.gameObject.SetActive(false);
	}

	public void Occupy(Token token)
	{
		HOTween.Kill(whiteBlock );
		if ( token.ifInit )
		{
			whiteFrame.color = occupyColor;
			HOTween.To( whiteFrame , fadeTime , "color", Global.changeAlpha(whiteBlock.color , 0 ) );
		}
		isOcupied = true;
		occupyToken = token;
	}

	public void Unoccupy()
	{
		isOcupied = false;
		occupyToken = null;
	}
}
