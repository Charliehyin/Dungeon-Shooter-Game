using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public float enemyCount;
    bool canChangeRoom;
    public GameObject gun;
    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.name = "Player";
        BoxCollider2D boxCollider2D = rb.GetComponent<BoxCollider2D>();
        canChangeRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        if (enemyCount == 0 && canChangeRoom == false)
        {
            clearRoom();
        }
    }

    void clearRoom()
    {
        canChangeRoom = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            // Take damage from the enemy
        }
        if (collision.collider.CompareTag("Left Door") && canChangeRoom)
        { 
            // Move the camera 200 units to the left
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - 200, Camera.main.transform.position.y, Camera.main.transform.position.z);

            // Move the player 50 units to the left
            transform.position = new Vector2(transform.position.x - 50, transform.position.y);

        }
        if (collision.collider.CompareTag("Right Door") && canChangeRoom)
        {
            // Move the camera 200 units to the right
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 200, Camera.main.transform.position.y, Camera.main.transform.position.z);

            // Move the player 50 units to the right
            transform.position = new Vector2(transform.position.x + 50, transform.position.y);
        }
        if (collision.collider.CompareTag("Up Door") && canChangeRoom)
        {
            // Move the camera 120 units up
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 120, Camera.main.transform.position.z);

            // Move the player 48 units up
            transform.position = new Vector2(transform.position.x, transform.position.y + 48);
        }
        if (collision.collider.CompareTag("Down Door") && canChangeRoom)
        {
            // Move the camera 120 units to the right
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 120, Camera.main.transform.position.z);

            // Move the player 48 units down
            transform.position = new Vector2(transform.position.x, transform.position.y - 48);
        }
    }
}
