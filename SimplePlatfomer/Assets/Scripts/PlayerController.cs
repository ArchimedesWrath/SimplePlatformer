using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Animator animator;
	public GameObject currentSpawn;
	public Transform GroundCheck;
	public LayerMask groundLayer;
	float speed = 250f;
	float jumpHeight = 500f;
	public bool jump = false;
	public bool grounded;
	public bool facingRight = true;
	Rigidbody2D rb;
	float xaxis;
	float yaxis;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update() {
		// Get Horizontal Movement
		xaxis = Input.GetAxisRaw("Horizontal");
		yaxis = Input.GetAxisRaw("Vertical");

		SetAnimationVariables();

		if(yaxis > 0) jump = true;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		// Set RB Velocity 
		Vector2 velX = rb.velocity;
		velX.x = xaxis * speed * Time.deltaTime;
		rb.velocity = velX;

		// Calmp Movement Speed
		if (rb.velocity.magnitude > speed) {
			rb.velocity *= speed / rb.velocity.magnitude;
		}

		// Apply Jump
		if (jump && grounded) {
			Vector2 velY = rb.velocity;
			velY.y = jumpHeight * yaxis * Time.deltaTime;
			rb.velocity = velY;
			jump = false;
			grounded = false;
		} else if (jump && !grounded) {
			jump = false;
		}

		if (xaxis < 0 && facingRight) {
			Flip();
		} else if (xaxis > 0 && !facingRight) {
			Flip();
		}

		grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, groundLayer);
	}

	void Flip() {
		facingRight = !facingRight;

		Vector3 _Scale = transform.localScale;
		_Scale.x *= -1;
		transform.localScale = _Scale;
	}

	void SetAnimationVariables() {
		// Logic for jumping / falling animations
		if (rb.velocity.y > 0) {
			animator.SetBool("IsRaising", true);
			animator.SetBool("IsFalling", false);
		} else if (rb.velocity.y < 0) {
			animator.SetBool("IsRaising", false);
			animator.SetBool("IsFalling", true);
		} else {
			animator.SetBool("IsFalling", false);
			animator.SetBool("IsRaising", false);
		}

		if (rb.velocity.y == 0) {
			animator.SetFloat("Speed", Mathf.Abs(xaxis));
		} else {
			animator.SetFloat("Speed", 0);
		}
	}

	void Die() {
		// Play death particle
		rb.velocity = Vector2.zero;
		jump = false;
		grounded = false;
		Vector3 _Scale = transform.localScale;
		_Scale.x = 1;
		transform.localScale = _Scale;
		facingRight = true;
		transform.position = currentSpawn.transform.position;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Spikes") {
			// TODO: Make some death and spawn Particles.
			Die();
		}
	}
}
