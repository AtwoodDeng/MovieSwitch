using UnityEngine;
using System.Collections;

public class background : MonoBehaviour {

	[SerializeField] Renderer movieRender;
	[SerializeField] string backgroundName;
	[SerializeField] AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		Debug.Log("back load "+backgroundName);
		MovieTexture movie = Resources.Load("Movie/"+backgroundName) as MovieTexture;
		movieRender.material.mainTexture = movie;
		movie.Play();
		audioSource.clip = movie.audioClip;
		audioSource.Play();
	}
}
