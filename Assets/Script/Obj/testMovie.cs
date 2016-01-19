using UnityEngine;
using System.Collections;

public class testMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MovieTexture movie = Resources.Load( "Movie/vil2" ) as MovieTexture;
		GetComponent<Renderer>().material.mainTexture = movie;
		movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
