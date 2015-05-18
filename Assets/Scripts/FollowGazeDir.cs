using UnityEngine;
using System.Collections;

public class FollowGazeDir : MonoBehaviour {

	private CardboardHead head;

	// Use this for initialization
	void Start () {
		head = Camera.main.GetComponent<StereoController>().Head;
	}
	
	// Update is called once per frame
	void Update () {
		transform.forward = head.Gaze.direction;
	}
}
