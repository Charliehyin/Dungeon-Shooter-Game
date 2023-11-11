using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public GameObject item;
    public GameObject costDisplay;
    public float cost;
    Player player;
    float nextBuyTime = 0;
    public bool isWeapon;
    public AudioClip potionSound;
    public AudioClip weaponSound;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        item = Instantiate(item, transform.position + new Vector3(0, 3, 0), Quaternion.identity, transform);

        GameObject canvasGameObject = new GameObject("Canvas");
        Canvas canvas = canvasGameObject.AddComponent<Canvas>();

        canvas.transform.SetParent(transform, false);

        costDisplay = Instantiate(costDisplay, transform.position + new Vector3(0, 5, 0), Quaternion.identity, canvas.transform);
        costDisplay.GetComponent<TextMeshProUGUI>().text = "" + cost;
        costDisplay.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (cost != 0)
        { 
            costDisplay.GetComponent<TextMeshProUGUI>().text = "" + cost;
        }
        else
        {
            costDisplay.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    private void BuyWeapon()
    {
        // If the player doesn't have enough gold, don't buy
        if (!player.goldCounter.purchase(cost))
        {
            return;
        }
        // Play the sound
        AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position);
        if (player.gun != null) // If the player has a gun
        {
            // Store the player's current gun
            GameObject oldGun = player.gun;

            // Place the player's gun into the shopItem
            oldGun.transform.SetParent(transform);
            oldGun.transform.position = transform.position + new Vector3(0, 3, 0);

            // If there is an weapon in the shop, give it to the player
            if (item != null)
            {
                item.transform.SetParent(player.transform);
                item.transform.localPosition = new Vector3(1.8f, -.8f, 0); // Local position relative to the player
                player.gun = item; // Update the player's gun reference
                item = oldGun; // The old gun is now the item in the shop
                cost = 0; // Should be free to purchase now
            }
            else
            {
                // If the shop was empty, do nothing
            }
        }
        // If the shop has an weapon and the player has no gun
        else if (item != null)
        {
            // Give the shop's weapon to the player
            item.transform.SetParent(player.transform);
            item.transform.localPosition = new Vector3(1.8f, -.8f, 0);
            player.gun = item; // Update the player's gun reference
            item.SetActive(false);
            cost = 0; // Should be free to purchase now
        }
    }
    private void BuyPotion()
    {
        if (!player.goldCounter.purchase(cost))
        {
            return;
        }
        // Play the sound
        AudioSource.PlayClipAtPoint(potionSound, Camera.main.transform.position);
        player.healthCounter.updateHealth(item.GetComponent<Potion>().healAmount);
        item.SetActive(false);
        cost = 0;
        costDisplay.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Space) && Time.time >= nextBuyTime)
            {
                nextBuyTime = Time.time + .5f;
                if (isWeapon)
                {
                    BuyWeapon();
                }
                else
                {
                    BuyPotion();
                }
            }
        }
    }
}
