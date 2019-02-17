using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public Rigidbody2D rb;
	public Transform HitRight;
	public Transform HitLeft;
	public LayerMask GroundLayer;
	public GameObject DeathParticle;
	public GameObject Item;
	public int health = 1;
	public bool MovingRight = true;
	bool HITLEFT = false;
	bool HITRIGHT = false;
	float speed = 150f;
	
	void Update() {
		HITLEFT = Physics2D.OverlapCircle(HitLeft.position, 0.15f, GroundLayer);
		HITRIGHT = Physics2D.OverlapCircle(HitRight.position, 0.15f, GroundLayer);

		if (MovingRight && HITRIGHT)  {
			MovingRight = !MovingRight;
			HITRIGHT = false;
		}
		if (!MovingRight && HITLEFT) {
			MovingRight = !MovingRight;
			HITLEFT = false;
		} 
	}
	void FixedUpdate() {
		if (MovingRight) {
			Vector2 vel = rb.velocity;
			vel.x = speed * Time.deltaTime;
			rb.velocity = vel; 
		} else if (!MovingRight) {
			Vector2 vel = rb.velocity;
			vel.x = -speed * Time.deltaTime;
			rb.velocity = vel;
		}
	}

	public void Damage(int damage) {
		health -= damage;
		CheckHealth();
	}

	void CheckHealth() {
		if (health <= 0) Die();
	}

	void Die () {
		Destroy(gameObject);
		// Drop item if has one
		if (Item != null) Instantiate(Item, transform.position, transform.rotation);
		// Play death particle
		GameObject effectIns = (GameObject)Instantiate(DeathParticle, transform.position, transform.rotation);
		Destroy(effectIns, 2f);
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy") {
			MovingRight = !MovingRight;
		}
	}
}