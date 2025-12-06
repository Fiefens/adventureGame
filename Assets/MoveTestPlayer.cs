using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAroundY = Input.GetAxis("Horizontal");
        float speed = Input.GetAxis("Vertical");

        gameObject.transform.Rotate(0, rotationAroundY, 0);

        GetComponent<CharacterController>().Move(transform.forward * speed * 4 * Time.deltaTime);
    }

}
