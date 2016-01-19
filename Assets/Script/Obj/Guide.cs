using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Guide : MonoBehaviour {

	[SerializeField] tk2dSprite[] spriteList;
	[SerializeField] float changeColorTime = 2f;
	[SerializeField] Renderer blurRender;

	public void Awake()
	{
		if( blurRender != null )
			blurRender.material.renderQueue = 3000 + 15;
		this.GetComponent<Renderer>().material.renderQueue = 3000 + 20;
	}

	public void Show(GuideManager.GuideInfo guideInfo )
	{
		if ( blurRender )
			blurRender.gameObject.SetActive(true);
		foreach(tk2dSprite sprite in spriteList )
			HOTween.To( sprite , changeColorTime , "color" , Global.changeAlpha( sprite.color , 1.0f));
	}

	public void EndShow(GuideManager.GuideInfo guideInfo)
	{
		if ( blurRender )
			blurRender.gameObject.SetActive(false);
		foreach(tk2dSprite sprite in spriteList )
			HOTween.To( sprite , changeColorTime , "color" , Global.changeAlpha( sprite.color , 0f));
	}
}
