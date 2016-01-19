using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {

	[SerializeField] string movieName;

	// Use this for initialization
	void Awake () {

		MovieTexture movie = Resources.Load("Movie/" + movieName) as MovieTexture;
		Renderer render = GetComponent<Renderer>();
		render.material.mainTexture = movie;
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip = movie.audioClip;
		audioSource.loop = true;
		audioSource.Play();
		movie.loop = true;
		movie.Play();

	}
	
	void Play()
	{
		Application.LoadLevel("test2");
	}
}