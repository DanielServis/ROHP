using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour
{
    public cards cardScript;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cardScript.CardsUpdate();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            cardScript.IncrementGamephase();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cardScript.ResetGamephase();
        }
    }
}