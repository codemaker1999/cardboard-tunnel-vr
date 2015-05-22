using UnityEngine;
using System.Collections;

public class DestroyLater : MonoBehaviour {

	public float afterSeconds;
	private float createdAt;

	// Use this for initialization
	void Start () {
		createdAt = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Time.time - createdAt > afterSeconds ) {
			Destroy(gameObject);
		}
	}
}
