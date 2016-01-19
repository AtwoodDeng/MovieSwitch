using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GuideManager : MonoBehaviour {

	[System.Serializable]
	public struct GuideInfo
	{
		public string name;
		public GameObject guide;
		public float showTime;
		public float duration;
		public bool isShown;
		public string movieName;
	};

	[SerializeField] List<GuideInfo> guideInfoList;

	float timer{
		get{
			return LogicManager.Instance.tempTime;
		}
	}

	void OnEnable()
	{
		BEventManager.Instance.RegisterEvent(EventDefine.OnTokenInit , OnTokenInit);
	}
	void OnDisable()
	{
		BEventManager.Instance.UnregisterEvent(EventDefine.OnTokenInit , OnTokenInit);
	}
	
	void OnTokenInit(EventDefine eventName, object sender, EventArgs args) 
	{
		foreach( GuideInfo gInfo in guideInfoList )
		{
			string movieName = ((MessageEventArgs)args).GetMessage("name");
			if ( gInfo.movieName.Equals( movieName ))
			{
				StartGuide( gInfo );
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
//		foreach( GuideInfo gInfo in guideInfoList )
//		{
//			if ( timer > gInfo.showTime && timer < gInfo.showTime + 1f )
//			{
//				StartGuide( gInfo );
//				guideInfoList.Remove(gInfo);
//				break;
//			}
//		}
	}

	void StartGuide( GuideInfo guideInfo )
	{
		if ( guideInfo.isShown )
			return ;
		guideInfo.isShown = true;
		guideInfo.guide.GetComponent<Guide>().Show(guideInfo);

		StartCoroutine(CheckEnd(guideInfo));
	}

	IEnumerator CheckEnd( GuideInfo guideInfo )
	{
		float checkTimer = 0 ;
		while(true)
		{
			checkTimer += Time.deltaTime;
			if( checkTimer > guideInfo.duration )
			{
				guideInfo.guide.GetComponent<Guide>().EndShow(guideInfo);
				yield break;
			}
			yield return null;
		}
	}
}
