using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToReach : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    { 
        if (other.name == "Player") 
        { 
            GameObject.Find("GameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.enter_place_called, gameObject.name); 
        } 
    }





}
