using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageBar : MonoBehaviour
{
    int value = 0;
    string label;
    // Start is called before the first frame update
    void Start()
    {
        UpdateValue();
        transform.Find("text").GetComponent<TextMeshProUGUI>().text = label;
    }

    // Update is called once per frame
    void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.B)) 
            IncreaseValue(10); 
    }

    public void SetValue(int amount) { value = amount; UpdateValue(); }


    public void IncreaseValue(int amount)
    {
        value += amount;
        if (value > 100) value = 100;
        UpdateValue();
    }

    void UpdateValue()
    {
        transform.Find("fill").localScale = new Vector3(value / 100.0f, transform.localScale.y, transform.localScale.z);
        transform.Find("text").GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
}
