using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {

	public CardboardHead head;
	public float acceleration;
	public float initialSpeed;
	public float maxSpeed;
	private float speed;

	// Use this for initialization
	void Start () {
		speed = initialSpeed;
	}

	public void ResetSpeed () {
		speed = initialSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (speed < maxSpeed) {
			speed += Time.deltaTime * acceleration;
		}
		transform.position = transform.position + speed * head.Gaze.direction;
	}
}
