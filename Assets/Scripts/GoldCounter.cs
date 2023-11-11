using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public float gold;
    TextMeshProUGUI goldText;
    int goldDigits = 1;
    void Start()
    {
        name = "Gold Counter";
        goldText = GetComponent<TextMeshProUGUI>();
        // Display current health
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "Gold: " + gold;
        // Get number of digits in gold, and shift the text to the left by that many digits
        int newGoldDigits = (int)Mathf.Floor(Mathf.Log10(gold+.01f) + 1);
        if (newGoldDigits > goldDigits)
        {
            goldText.transform.position = new Vector3(goldText.transform.position.x - 2f, goldText.transform.position.y, goldText.transform.position.z);
            goldDigits = newGoldDigits;
        }
    }

    public bool purchase(float cost)
    {
        gold -= cost;
        if (gold < 0)
        {
            gold += cost;
            return false;
        }
        return true;
    }

    public void updateGold(float value)
    {
        gold += value;
    }
}
