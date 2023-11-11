using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openedSprite;
    public GameObject chestContents;
    bool opened;
    SpriteRenderer spriteRenderer;
    Player player;

    GameObject item;

    public float swapCooldown = .5f;
    float nextSwapTime = 0f;

    public AudioClip chestOpenSound;
    
    // Start is called before the first frame update
    void Start()
    {
        opened = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && opened == false)
        {
            opened = true;
            spriteRenderer.sprite = openedSprite;

            //Display the chest contents
            item = Instantiate(chestContents, transform.position, Quaternion.identity, transform);


            // Play the sound
            AudioSource.PlayClipAtPoint(chestOpenSound, Camera.main.transform.position);
        }
    }

    private void SwapItems()
    {
        if (player.gun != null) // If the player has a gun
        {
            // Store the player's current gun
            GameObject oldGun = player.gun;

            // Place the player's gun into the chest
            oldGun.transform.SetParent(transform);
            oldGun.transform.position = transform.position;

            // If there is an item in the chest, give it to the player
            if (item != null)
            {
                item.transform.SetParent(player.transform);
                item.transform.localPosition = new Vector3(1.8f, -.8f, 0); // Local position relative to the player
                player.gun = item; // Update the player's gun reference
                item = oldGun; // The old gun is now the item in the chest
            }
            else
            {
                // If the chest was empty, now it has the player's old gun
                item = oldGun;
                player.gun = null; // The player has no gun now
            }
        }
        // If the chest has an item and the player has no gun
        else if (item != null) 
        {
            // Give the chest's item to the player
            item.transform.SetParent(player.transform);
            item.transform.localPosition = new Vector3(1.8f, -.8f, 0);
            player.gun = item; // Update the player's gun reference
            item = null; // The chest is now empty
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && opened)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time >= nextSwapTime)
            {
                nextSwapTime = Time.time + swapCooldown;
                SwapItems(); // Call the swap function
            }
        }
    }
}
