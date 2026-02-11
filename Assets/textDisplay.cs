using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textDisplay : MonoBehaviour
{
    public cardGame cardScript;

    public GameObject shuffleTxt;

    float scaleX = 0;
    float scaleY = 0;
    float scaleZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cardScript.gamePhase == 0)
        {
            shuffleTxt.transform.position = new Vector3(0, 0, -20);
            shuffleTxt.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            scaleX += 0.2f*Time.deltaTime;
            scaleY += 0.2f*Time.deltaTime;
        }
        else
        {
            shuffleTxt.transform.position = new Vector3(0, 10, 0);
            scaleX = 0.25f;
            scaleY = 0.25f;
            scaleZ = 0;

        }
    }
}
