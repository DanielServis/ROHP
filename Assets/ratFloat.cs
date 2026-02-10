using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratFloat : MonoBehaviour
{
    public float timer = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > -1)
        {
            transform.Translate(Vector2.up * timer * 2 * Time.deltaTime);
        }
        else { timer = 1; }

        timer = timer - 2 * Time.deltaTime;
    }
}
