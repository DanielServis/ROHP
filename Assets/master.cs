using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour
{
    public cardGame cardScript;

    public int menuPhase = 0;


    // Start is called before the first frame update
    void Start()
    {
        cardScript.CardsStart(5);
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