using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        name = "EnemyBullet";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Don't collide with player bullets
        if (!collision.collider.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }
    // Remove the bullet when it leaves the screen
    private void OnBecameInvisible()
    {
        // Destroy this bullet GameObject
        Destroy(gameObject);
    }
}
