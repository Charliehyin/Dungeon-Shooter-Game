using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    void Start()
    {
        name = "PlayerBullet";
        Collider2D bulletCollider = GetComponent<Collider2D>();

        // Ignore collision between the bullet and the player
        Collider2D playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCollider, playerCollider, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    // Remove the bullet when it leaves the screen
    private void OnBecameInvisible()
    {
        // Destroy this bullet GameObject
        Destroy(gameObject);
    }
}
