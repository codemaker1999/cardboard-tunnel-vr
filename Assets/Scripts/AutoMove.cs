using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {

	public float acceleration;
	public float initialSpeed;
	private float speed;

	// Use this for initialization
	void Start () {
		initialSpeed = 0.1f;
		speed = initialSpeed;
	}

	public void ResetSpeed () {
		speed = initialSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (speed < 2.0f) {
			speed += Time.deltaTime * acceleration;
		}
		transform.position = transform.position + speed * Camera.main.transform.forward;
	}
}
