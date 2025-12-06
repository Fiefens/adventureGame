using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{

    int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>().gameObject.tag == "target")
        {
            other.GetComponent<Collider>().gameObject.GetComponent<ManageTargetHealth>().DecreaseHealth(damage);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
