using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    public float roamTimer = 2f;

    Rigidbody2D rb;
    Vector2 movement;
    float nextRoamTime;
    Player player;

    public float health;
    void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.enemyCount += 1;
    }
    void OnDisable()
    {
        player.enemyCount -= 1;
    }

    void Start()
    {
        health = 5f;
        rb = GetComponent<Rigidbody2D>();
        CalculateNewMovementVector();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextRoamTime)
        {
            CalculateNewMovementVector();
            nextRoamTime = Time.time + roamTimer;
            Debug.Log("New Roam " + movement);
        }
    }

    void FixedUpdate()
    {
        // Apply the roaming movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a player bullet
        if (collision.gameObject.tag == "PlayerBullet")
        {
            // Take damage from the bullet
            takeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CalculateNewMovementVector()
    {
        // Roam in a new random direction
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
