using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float maxHealth;
    TextMeshProUGUI healthText;
    public AudioClip damageSound;
    void Start()
    {
        name = "Health Counter";
        health = 3f;
        healthText = GetComponent<TextMeshProUGUI>();
        // Display current health

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public void updateHealth(float change)
    {
        if(change < 0)
        {
            AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
        }
        health += change;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
        if(health <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loss");
        }
    }
}
