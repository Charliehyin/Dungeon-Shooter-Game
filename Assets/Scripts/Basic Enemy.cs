using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float roamTimer = 2f;

    public float damage = 1f;
    public float fireRate = 1f;
    public float bulletSpeed = 30f;
    public GameObject bulletPrefab;

    Rigidbody2D rb;
    Vector2 movement;
    float nextRoamTime = 0;
    float nextFireTime= 0;
    Player player;
    Camera cam;
    bool counted = false;

    public float health;
    void OnDisable()
    {
        player.enemyCount -= 1;
        player.goldCounter.updateGold((int)(Random.Range(0, 3)));
    }

    void Start()
    {
        health = 5f;
        rb = GetComponent<Rigidbody2D>();
        CalculateNewMovementVector();
        cam = Camera.main;
        player = GameObject.Find("Player").GetComponent<Player>();

        // Slightly randomize the fire rate
        fireRate = Random.Range(fireRate - .2f, fireRate + .2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextRoamTime)
        {
            CalculateNewMovementVector();
            nextRoamTime = Time.time + roamTimer;
        }
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        // Object is in view
        if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0)
        {
            if (!counted)
            {
                counted = true;
                player.enemyCount += 1;
            }
            if (Time.time >= nextFireTime)
            {
                // Fire a bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().damage = damage;
                bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * bulletSpeed;
                // Ignore collision between the bullet and the enemy
                Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
                Collider2D collider2D = GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(bulletCollider, collider2D, true);
                nextFireTime = Time.time + fireRate;
            }
            // Set rotation to face the player
            Vector3 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Move the enemy
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
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
