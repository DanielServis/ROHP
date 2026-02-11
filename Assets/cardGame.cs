using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardGame : MonoBehaviour
{
    public GameObject[] cardArray = new GameObject[11];
    public Animator[] numberAnimArray = new Animator[11];
    public Animator[] faceAnimArray = new Animator[11];
    private ArrayList combinations = new ArrayList();
    private ArrayList winningCards = new ArrayList();

    public GameObject deck;
    public GameObject deck1;
    public GameObject deck2;

    private int[][] handNumArray;
    private int[][] handFaceArray;
    private int[] sewerCard = new int[2];

    public int playerAmount = 1;
    public int gamePhase = -1;
    public float shuffleTime = 0;

    //Master Input ->
    public void IncrementGamephase()
    {
        if (gamePhase != 0)
        {
            shuffleTime = 0.5f;
            gamePhase++;
        }
    }

    public void ResetGamephase()
    {
        if (gamePhase != 0)
        {
            gamePhase = -1;
        }
    }

    public void CardsStart(int players)
    {
        playerAmount = players;
        handNumArray = new int[playerAmount][];
        handFaceArray = new int[playerAmount][];

        for (int i = 0; i < playerAmount; i++)
        {         
            handNumArray[i] = new int[2];
            handFaceArray[i] = new int[2];
        }
    }

    //Cards Independance ->
    public void CardsUpdate()
    {
        switch (gamePhase)
        {
            case 0:
                if (shuffleTime > 1.5)
                {
                    deck.transform.position = new Vector2(0, 2);
                    deck.transform.Rotate(Vector3.forward * 180 * Time.deltaTime);
                }
                else if (shuffleTime > 1)
                {
                    deck1.transform.Translate(Vector2.left * 3 * Time.deltaTime);
                    deck2.transform.Translate(Vector2.right * 3 * Time.deltaTime);
                }
                else if (shuffleTime > 0.5)
                {
                    deck1.transform.Translate(Vector2.right * 3 * Time.deltaTime);
                    deck2.transform.Translate(Vector2.left * 3 * Time.deltaTime);
                }
                else if (shuffleTime > 0)
                {
                    deck.transform.Rotate(Vector3.back * 180 * Time.deltaTime);
                }
                else
                {
                    deck.transform.position = new Vector2(1, 0.5f);
                    deck1.transform.position = new Vector2(1, 0.5f);
                    deck2.transform.position = new Vector2(1, 0.5f);
                    for (int i = 0; i < handNumArray[0].Length; i++)
                    {
                        numberAnimArray[i].SetInteger("num", handNumArray[0][i]);
                        faceAnimArray[i].SetInteger("face", handFaceArray[0][i]);
                    }
                    gamePhase++;
                }                
                break;
            case 1:
                //Betting on hand alone
                cardArray[0].transform.position = new Vector2(-1.2f, -3);
                cardArray[1].transform.position = new Vector2(1.2f, -3);
                break;
            case 2:
                numberAnimArray[10].SetInteger("num", sewerCard[0]);
                faceAnimArray[10].SetInteger("face", sewerCard[1]);
                cardArray[10].transform.position = new Vector2(-1, 0.5f);
                //Reveal & Mix-Ups
                break;
            case 3:
                //Final Bet 
                break;
            case 4:
                CheckOutcome();
                shuffleTime = 0.5f;
                gamePhase++;
                break;
            case 5:
                for (int p = 0; p < playerAmount; p++)
                {
                    for (int i = 0; i < handNumArray[p].Length; i++)
                    {
                        numberAnimArray[i + (p * 2)].SetInteger("num", handNumArray[p][i]);
                        faceAnimArray[i + (p * 2)].SetInteger("face", handFaceArray[p][i]);
                    }
                }
                if (shuffleTime > 0)
                {
                    foreach (GameObject card in winningCards)
                    {
                        card.transform.Translate(Vector2.up * Time.deltaTime);
                    }
                }
                break;
            default:
                winningCards.Clear();
                foreach (GameObject card in cardArray)
                {
                    card.transform.position = new Vector2(10, 10);
                }
                for (int i = 0; i < playerAmount; i++)
                {
                    ResetHands(handNumArray[i], handFaceArray[i]);
                }
                ResetSewer();
                for (int p = 0; p < playerAmount; p++)
                {
                    for (int i = 0; i < handNumArray[p].Length; i++)
                    {
                        numberAnimArray[i + (p * 2)].SetInteger("num", 0);
                        faceAnimArray[i + (p * 2)].SetInteger("face", 0);
                    }
                }
                shuffleTime = 2;
                gamePhase = 0;
                break;
        }

        shuffleTime -= Time.deltaTime;
    }

    void CheckOutcome()
    {
        float[] winner = new float[playerAmount];
        float largest = 0;
        for (int p = 0; p < playerAmount; p++)
        {
            winner[p] = CheckWin(p, handNumArray[p], handFaceArray[p]);

            if (winner[p] > largest)
            {
                largest = winner[p];
            }
        }

        int winIndex = getIndexOfLargest(winner);

        winningCards.Clear();
        print("Winner @ Player" + winIndex + " == " + CheckWin(winIndex, handNumArray[winIndex], handFaceArray[winIndex]));
    }

    float CheckWin(int player, int[] numArray, int[] faceArray)
    {


        return 0;
    }

    void ResetHands(int[] nums, int[] faces)
    {
        combinations.Clear();

        for (int i = 0; i < nums.Length; i++)
        {
            int number = Random.Range(1, 11);
            int face = Random.Range(1, 4);
            string combination = number + "" + face;

            while (combinations.Contains(combination))
            {
                number = Random.Range(1, 11);
                face = Random.Range(1, 4);
                combination = number + "" + face;
            }

            nums[i] = number;
            faces[i] = face;
            combinations.Add(combination);
        }      
    }

    void ResetSewer()
    {
        sewerCard[0] = Random.Range(1, 11);
        sewerCard[1] = Random.Range(1, 4);
        string sewer = sewerCard[0] + "" + sewerCard[1];

        while (combinations.Contains(sewer))
        {
            sewerCard[0] = Random.Range(1, 11);
            sewerCard[1] = Random.Range(1, 4);
            sewer = sewerCard[0] + "" + sewerCard[1];
        }
    }

    int getIndexOfLargest(float[] array)
    {
        float largest = array[0];
        int index = 0;

        for (int i = 1; i < array.Length - 1; i++)
        {
            if (array[i] > largest)
            {
                index = i;
                largest = array[i];
            }
        }

        return index;
    }
}