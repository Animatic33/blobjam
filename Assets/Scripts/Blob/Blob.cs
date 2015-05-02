﻿using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour {

	public bool selected = true;
	public float moveSpeed = 5f;
	public float walkSpeed = 5f;
	public float runSpeed = 10f;
	public float jumpForce = 10f;
	public float groundedJumpTolerance = 0.04f;

	public float killY = -10f;

	private Rigidbody rigidBody = null;
	private Animator blobAnimator = null;

	public bool Grounded {
		get {
			return Mathf.Abs(this.rigidBody.velocity.y) < groundedJumpTolerance;
		}
	}

	// Use this for initialization
	void Start () {
		this.rigidBody = this.GetComponent<Rigidbody> ();
		this.blobAnimator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (selected) {
			UpdateInputMovement();

			if(Input.GetButtonDown("Fire1")) {
				this.Split();
			}
		}


		this.UpdateAnimations ();

		if (this.transform.position.y < killY) {
			GameObject.Destroy(this.gameObject);
		}
	}

	public void Split() {
		float targetScale = this.transform.localScale.x * 0.8f;
		if (targetScale < 0.5) {
			return;
		}
		this.transform.localScale = Vector3.one * targetScale;
		GameObject.Instantiate (this);
	}

	void UpdateAnimations() {
		float xSqr = Mathf.Pow(this.rigidBody.velocity.x, 2);
		float zSqr = Mathf.Pow(this.rigidBody.velocity.z, 2);

		float horizontalVelocity = Mathf.Sqrt(xSqr + zSqr);
		this.blobAnimator.SetFloat ("_VelY", this.rigidBody.velocity.y);
		this.blobAnimator.SetFloat ("_VelH", horizontalVelocity);
		this.blobAnimator.SetBool ("Grounded", this.Grounded);

	}

	void UpdateInputMovement() {

		if (Input.GetButton ("Fire2") || Input.GetButton ("Run")) {
			this.moveSpeed = runSpeed;
		} else {
			this.moveSpeed = walkSpeed;
		}

		if (Input.GetButtonDown ("Jump") && this.Grounded) {
			this.rigidBody.AddForce(new Vector3(0f, jumpForce, 0f));
		} 

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		this.transform.position = this.transform.position + new Vector3 (vertical, 0f, -horizontal) * moveSpeed * Time.deltaTime;
	}
}
