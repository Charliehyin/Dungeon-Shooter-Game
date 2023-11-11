using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float roamTimer = 1f;

    public float damage = 1f;
    public float fireRate = .5f;
    public float bulletSpeed = 20f;
    public float bulletSize = 3f;

    public float secondaryFireRate = 1f;
    public float secondaryBulletSpeed = 30f;
    public float numberOfBullets = 10;
    public float sprayAngle = 30f;

    public GameObject bulletPrefab;

    Rigidbody2D rb;
    Vector2 movement;
    float nextRoamTime = 0;
    float nextFireTime = 0;
    float nextSecondaryFireTime = 0;
    Player player;
    Camera cam;

    public float health;

    void Start()
    {
        health = 150f;
        rb = GetComponent<Rigidbody2D>();
        CalculateNewMovementVector();
        cam = Camera.main;
        player = GameObject.Find("Player").GetComponent<Player>();
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
            if (Time.time >= nextFireTime)
            {
                // Fire a bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().damage = damage;
                bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * bulletSpeed;
                bullet.transform.localScale = bullet.transform.localScale * bulletSize;
                // Ignore collision between the bullet and the enemy
                Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
                Collider2D collider2D = GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(bulletCollider, collider2D, true);
                nextFireTime = Time.time + fireRate;
            }

            if (Time.time >= nextSecondaryFireTime)
            {
                // Fire a spray of bullets at the player
                for (float angle = -sprayAngle; angle < sprayAngle; angle+=2*sprayAngle/numberOfBullets)
                {
                    // Calculate the direction from the shootPoint to the targetPosition
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<EnemyBullet>().damage = damage;
                    bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, angle) * (player.transform.position - transform.position).normalized * secondaryBulletSpeed;
                    // Ignore collision between the bullet and the enemy
                    Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
                    Collider2D collider2D = GetComponent<Collider2D>();
                    Physics2D.IgnoreCollision(bulletCollider, collider2D, true);
                }
                nextSecondaryFireTime = Time.time + secondaryFireRate;
            }

            // Set rotation to face the player
            Vector3 dir = player.transform.position - transform.position;
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg-90;
            transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);

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
            // Win the game
            UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
        }
    }

    void CalculateNewMovementVector()
    {
        // Roam in a new random direction
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
