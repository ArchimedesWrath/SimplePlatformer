using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Animator animator;
	public GameObject currentSpawn;
	public GameObject Key;
	public Transform GroundCheck;
	public Transform KeyHoldPoint;
	public LayerMask groundLayer;
	public LayerMask EnemyLayer;
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

	// Going through door logic
	public bool isInDoor = false;
	GameObject Door;

	//Particles 
	public GameObject DeathParticle;

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

		if (Input.GetKeyDown(KeyCode.E) && isInDoor) {
			// Go to the otherside of door?
			EventManager.instnace.RequestDoor(Door);
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (!isInDoor && Door != null) Door = null;

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
			Jump();
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

	void Jump() {
		Vector2 velY = rb.velocity;
		velY.y = jumpHeight * yaxis * Time.deltaTime;
		rb.velocity = velY;
		jump = false;
		grounded = false;
	}

	void Bounce() {
		Vector2 velY = rb.velocity;
		velY.y = jumpHeight * Time.deltaTime;
		rb.velocity = velY;
		jump = false;
		grounded = false;
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
		GameObject effectIns = (GameObject)Instantiate(DeathParticle, gameObject.transform.position, gameObject.transform.rotation);
		Destroy(effectIns, 2f);

		rb.velocity = Vector2.zero;
		jump = false;
		grounded = false;
		Vector3 _Scale = transform.localScale;
		_Scale.x = 1;
		transform.localScale = _Scale;
		facingRight = true;
		transform.position = currentSpawn.transform.position;
	}

	void PickUpCoin(GameObject coin) {
		doubleJump = true;
	}

	bool PickUpKey(GameObject key) {
		if (!hasKey) {
			hasKey = true;
			return true;
		} else {
			return false;
		}
	}

	void SetCheckPoint(GameObject checkpoint) {
		currentSpawn = checkpoint;
	}

	public void UseKey() {
		hasKey = false;
		Key.GetComponent<Key>().Use();
	}

	public void Move(Transform transform) {
		jump = false;
		xaxis = yaxis = 0;
		rb.velocity = Vector2.zero;
		gameObject.transform.position = transform.position;
	}

	public GameObject GetDoor() {
		return Door;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Spikes") {
			// TODO: Make some death and spawn Particles.
			Die();
		} else if (col.gameObject.tag == "Enemy") {
			if (Physics2D.OverlapCircle(GroundCheck.transform.position, 0.15f, EnemyLayer)) {
				Bounce();
				col.gameObject.GetComponent<EnemyController>().Damage(1);
			} else {
				Die();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Coin") {
			if (!doubleJump) {
				PickUpCoin(col.gameObject);
				col.gameObject.GetComponent<Coin>().PickUpCoin();
			}
		} else if (col.gameObject.tag == "Key") {
			if (PickUpKey(col.gameObject)) {
				col.gameObject.GetComponent<Key>().PickUp(KeyHoldPoint);	
				Key = col.gameObject;
			}
		} else if (col.gameObject.tag == "Door") {
			isInDoor = true;
			Door = col.gameObject;
		} else if (col.gameObject.tag == "Checkpoint") {
			SetCheckPoint(col.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.tag == "Door") {
			isInDoor = false;
			Door = null;
		}
	}
}
