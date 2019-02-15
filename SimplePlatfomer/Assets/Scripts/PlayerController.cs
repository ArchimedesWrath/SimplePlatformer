using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Animator animator;
	public GameObject currentSpawn;
	public GameObject Key;
	public Transform GroundCheck;
	public LayerMask groundLayer;
	float speed = 250f;
	float jumpHeight = 500f;
	public bool jump = false;
	public bool grounded;
	bool facingRight = true;
	Rigidbody2D rb;
	float xaxis;
	float yaxis;

	// Double Jump variables
	public bool doubleJump = false;

	// Key Logic
	public bool hasKey = false;

	// Use this for initialization
	void Awake () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
		
	}

	void Update() {
		// Get Horizontal Movement
		xaxis = Input.GetAxisRaw("Horizontal");
		yaxis = Input.GetAxisRaw("Vertical");

		SetAnimationVariables();

		if(yaxis > 0) jump = true;
		if (Input.GetKeyDown(KeyCode.W) && doubleJump && !grounded) {
			Vector2 velY = rb.velocity;
			velY.y = jumpHeight * yaxis * Time.deltaTime;
			rb.velocity = velY;
			jump = false;
			doubleJump = false;
		}

		if (rb.velocity.y > 9.5f) {
			Vector2 velY = rb.velocity;
			velY.y = 9.5f;
			rb.velocity = velY;
		}
		
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
		} else if (jump && !grounded && !doubleJump) {
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

		Vector3 _Scale = transform.GetChild(0).transform.localScale;
		_Scale.x *= -1;
		transform.GetChild(0).transform.localScale = _Scale;
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
		Vector3 _Scale = transform.GetChild(0).transform.localScale;
		_Scale.x = 1;
		transform.GetChild(0).transform.localScale = _Scale;
		facingRight = true;
		transform.position = currentSpawn.transform.position;
	}

	bool PickUpKey(GameObject key) {
		if (!hasKey) {
			hasKey = true;
			return true;
		} else {
			return false;
		}
	}

	public void UseKey() {
		hasKey = false;
		gameObject.transform.GetChild(2).GetComponent<Key>().Use();
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Spikes") {
			// TODO: Make some death and spawn Particles.
			Die();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Coin") {
			if (!doubleJump) {
				doubleJump = true;
				Destroy(col.gameObject);
			}
		} else if (col.gameObject.tag == "Key") {
			if (PickUpKey(col.gameObject)) {
				//Destroy(col.gameObject);
				col.gameObject.transform.SetParent(gameObject.transform);
			}
		} 
	}
}
