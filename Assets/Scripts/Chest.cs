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
    
    // Start is called before the first frame update
    void Start()
    {
        opened = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

            // Todo: Give the player chest contents
            Player player = FindAnyObjectByType<Player>();
            player.gun = Instantiate(chestContents, player.transform.position + new Vector3(1.8f, -.8f, 0), Quaternion.identity, player.transform);
        }
    }
}
