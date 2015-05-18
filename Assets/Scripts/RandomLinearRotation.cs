using UnityEngine;
using System.Collections;

public class RandomLinearRotation : MonoBehaviour {
	
	private float dt;
	private Vector3 rotAxis;

	// Use this for initialization
	void Start () {
		transform.LookAt( Random.insideUnitSphere );
		rotAxis = Random.insideUnitSphere;
		dt = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (
			transform.position,
			rotAxis,
			dt
		);
	}
}
