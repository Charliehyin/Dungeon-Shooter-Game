using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Restarter : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI message;
    void Start()
    {
        name = "Restarter";
        message = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player clicks, restart the game
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
        }
    }
}
