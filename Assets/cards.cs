using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cards : MonoBehaviour
{
    private int playerAmount = 4;

    public GameObject[] cardArray = new GameObject[13];
    private GameObject[] riverArray = new GameObject[5];
    private GameObject[][] handArray;

    private int[][] handNumArray;
    private int[][] handFaceArray;
    private int[] riverNumArray = new int[5];
    private int[] riverFaceArray = new int[5];
    private ArrayList combinations = new ArrayList();
    private ArrayList winningCards = new ArrayList();

    public Animator[] numberAnimArray = new Animator[13];
    public Animator[] faceAnimArray = new Animator[13];

    public GameObject deck;
    public GameObject deck1;
    public GameObject deck2;
  
    public int gamePhase = -1;
    float shuffleTime = 0;

    void Start()
    {
        for (int i = 0; i < riverArray.Length; i++)
        {
            riverArray[i] = cardArray[i];
        }

        handArray = new GameObject[playerAmount][];
        handNumArray = new int[playerAmount][];
        handFaceArray = new int[playerAmount][];

        for (int i = 0; i < playerAmount; i++)
        {
            handArray[i] = new GameObject[2];
            handNumArray[i] = new int[2];
            handFaceArray[i] = new int[2];
        }

        for (int i = 0; i < playerAmount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                handArray[i][j] = cardArray[(i*2) + j + 5];
            }
        }
    }

    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.LeftShift) && gamePhase != 0)
        {
            gamePhase++;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && gamePhase != 0)
        {
            gamePhase = -1;
        }
        
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
                    deck.transform.position = new Vector2(5.25f, 0.5f);
                    deck1.transform.position = new Vector2(5.25f, 0.5f);
                    deck2.transform.position = new Vector2(5.25f, 0.5f);
                    gamePhase++; 
                }
                shuffleTime -= Time.deltaTime;
                break;
            case 1:
                for (int i = 0; i < handNumArray[0].Length; i++)
                {
                    numberAnimArray[i + 5].SetInteger("num", handNumArray[0][i]);
                    faceAnimArray[i + 5].SetInteger("face", handFaceArray[0][i]);
                }

                cardArray[5].transform.position = new Vector2(-1.2f, -3);
                cardArray[6].transform.position = new Vector2(1.2f, -3);
                cardArray[7].transform.position = new Vector2(-0.66f, 3);
                cardArray[8].transform.position = new Vector2(0.66f, 3);
                cardArray[9].transform.position = new Vector2(-4.66f, 3);
                cardArray[10].transform.position = new Vector2(-3.33f, 3);
                cardArray[11].transform.position = new Vector2(3.33f, 3);
                cardArray[12].transform.position = new Vector2(4.66f, 3);
                break;           
            case 2:
                cardArray[0].transform.position = new Vector2(-3.5f, 0.5f);
                cardArray[1].transform.position = new Vector2(-1.75f, 0.5f);
                cardArray[2].transform.position = new Vector2(0, 0.5f);
                break;
            case 3:
                cardArray[3].transform.position = new Vector2(1.75f, 0.5f);
                break;
            case 4:
                cardArray[4].transform.position = new Vector2(3.5f, 0.5f);              
                break;
            case 5:
                CheckOutcome();
                shuffleTime = 0.5f;
                gamePhase++;
                break;
            case 6:
                for (int p = 0; p < playerAmount; p++)
                {
                    for (int i = 0; i < handNumArray[p].Length; i++)
                    {
                        numberAnimArray[i + 5 + (p * 2)].SetInteger("num", handNumArray[p][i]);
                        faceAnimArray[i + 5 + (p * 2)].SetInteger("face", handFaceArray[p][i]);
                    }
                }
                if (shuffleTime > 0)
                {
                    foreach (GameObject card in winningCards)
                    {
                        card.transform.Translate(Vector2.up * Time.deltaTime);
                    }
                    shuffleTime -= Time.deltaTime;
                }        
                break;
            default:
                winningCards.Clear();
                foreach (GameObject card in cardArray)
                {
                    card.transform.position = new Vector2(10, 10);
                }              
                ResetRiver(riverNumArray, riverFaceArray);
                for (int i = 0; i < playerAmount; i++)
                {
                    ResetHands(handNumArray[i], handFaceArray[i]);
                }
                for (int p = 0; p < playerAmount; p++)
                {
                    for (int i = 0; i < handNumArray[p].Length; i++)
                    {
                        numberAnimArray[i + 5 + (p * 2)].SetInteger("num", 0);
                        faceAnimArray[i + 5 + (p * 2)].SetInteger("face", 0);
                    }
                }
                shuffleTime = 2;
                gamePhase = 0;
                break;
        }

        for (int i = 0; i < riverNumArray.Length; i++)
        {
            numberAnimArray[i].SetInteger("num", riverNumArray[i]);
            faceAnimArray[i].SetInteger("face", riverFaceArray[i]);
        }           
    }

    void ResetRiver(int[] nums, int[] faces)
    {
        combinations.Clear();

        for (int i = 0; i < nums.Length; i++)
        {
            int number = UnityEngine.Random.Range(1, 14);
            int face = UnityEngine.Random.Range(1, 5);
            string combination = number + "" + face;

            while (combinations.Contains(combination))
            {               
                number = UnityEngine.Random.Range(1, 14);
                face = UnityEngine.Random.Range(1, 5);
                combination = number + "" + face;
            }

            nums[i] = number;
            faces[i] = face;
            combinations.Add(combination);
        }
    }

    void ResetHands(int[] nums, int[] faces)
    {   
        for (int i = 0; i < nums.Length; i++)
        {
            int number = UnityEngine.Random.Range(1, 14);
            int face = UnityEngine.Random.Range(1, 5);
            string combination = number + "" + face;

            while (combinations.Contains(combination))
            {
                number = UnityEngine.Random.Range(1, 14);
                face = UnityEngine.Random.Range(1, 5);
                combination = number + "" + face;
            }

            nums[i] = number;
            faces[i] = face;
            combinations.Add(combination);
        }
    }

    float CheckWin(int playerNum, int[] riverNums, int[] riverFaces, int[] handNums, int[] handFaces)
    {
        int[] numberArray = new int[7];
        int[] faceArray = new int[7];

        List<int> main5 = new List<int>();

        //Transformation of Arrays
        for (int i = 0; i < riverNums.Length; i++)
        {
            numberArray[i] = riverNums[i];
            faceArray[i] = riverFaces[i];
        }
        for (int i = 0; i < handNums.Length; i++)
        {
            numberArray[i + riverNums.Length] = handNums[i];
            faceArray[i + riverFaces.Length] = handFaces[i];
        }

        //Each index represents face Freak
        int[] faces = { 0, 0, 0, 0 };

        //Each index represents number Freak
        int[] numbers = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        bool extraPair = false;
        bool straight = false;
        bool flush = false;
        bool straightFlush = false;

        //Creation of Freak Arrays
        for (int i = 0; i < faceArray.Length; i++)
        {
            faces[faceArray[i] - 1]++;
        }
        for (int i = 0; i < numberArray.Length; i++)
        {
            if (numberArray[i] == 1)
            {
                numbers[13]++;
            }

            numbers[numberArray[i] - 1]++;
        }

        int firstPairIndex = getIndexOfMostFrequent(numbers);

        straight = getStraight(numbers);
      
        //ExtraPair Check
        for (int i = 0; i < numbers.Length - 1; i++)
        {           
            if (firstPairIndex != i && numbers[i] >= 2)
            {
                extraPair = true;
            }           
        }

        //Flush Check
        if (getLargestOf(faces) >= 5)
        {
            flush = true;
        }

        //Straight Flush Check
        if (flush)
        {
            int flushSuit = getIndexOfLargestOf(faces);

            int[] flushNums = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < faceArray.Length; i++)
            {
                if (faceArray[i] == flushSuit)
                {
                    flushNums[numberArray[i] - 1]++;
                }
            }

            straightFlush = getStraight(flushNums);
        }

        //Output
        float kicker = 0.00f;

        if (straightFlush)
        {
            int flushSuit = getIndexOfMostFrequent(faces) + 1;
            List<int> straightCards = new List<int>();
            for (int i = 0; i < faceArray.Length; i++)
            {
                if (faceArray[i] == flushSuit)
                {
                    straightCards.Add(i);
                }
            }
            straightCards.Sort((a, b) => numberArray[b].CompareTo(numberArray[a]));
            main5 = straightCards.Take(5).ToList();
            kicker = (float)main5.Max() / 100;
          
            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 9 + kicker;
        }
        else if (getLargestOf(numbers) == 4)
        {
            int quadValue = getIndexOfMostFrequent(numbers) + 1;
            if (quadValue == 14) quadValue = 1;

            for (int i = 0; i < numberArray.Length; i++)
            {
                if (numberArray[i] == quadValue)
                {
                    main5.Add(i);
                }
            }
            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                if (!main5.Contains(i))
                {
                    main5.Add(i);
                    break;
                }
            }

            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 8 + kicker;
        }
        else if (getLargestOf(numbers) == 3 && extraPair)
        {
            int tripValue = getIndexOfMostFrequent(numbers) + 1;
            if (tripValue == 14) tripValue = 1;

            for (int i = 0; i < numberArray.Length; i++)
            {
                if (numberArray[i] == tripValue)
                {
                    main5.Add(i);
                }
            }

            for (int i = numbers.Length - 1; i >= 0; i--)
            {
                if (numbers[i] >= 2 && i != getIndexOfMostFrequent(numbers))
                {
                    int pairValue = i + 1;
                    if (pairValue == 14) pairValue = 1;

                    for (int j = 0; j < numberArray.Length; j++)
                    {
                        if (numberArray[j] == pairValue && main5.Count < 5)
                        {
                            main5.Add(j);
                        }
                    }
                    break;
                }
            }

            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 7 + kicker;
        }
        else if (flush)
        {
            int flushSuit = getIndexOfMostFrequent(faces) + 1;
            List<int> flushCards = new List<int>();

            for (int i = 0; i < faceArray.Length; i++)
            {
                if (faceArray[i] == flushSuit)
                {
                    flushCards.Add(i);
                }
            }

            flushCards.Sort((a, b) => numberArray[b].CompareTo(numberArray[a]));
            main5 = flushCards.Take(5).ToList();
            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 6 + kicker;
        }
        else if (straight)
        {
            for (int i = numbers.Length - 5; i >= 0; i--)
            {
                bool foundStraight = true;
                for (int m = 0; m < 5; m++)
                {
                    if (numbers[i + m] < 1)
                    {
                        foundStraight = false;
                        break;
                    }
                }

                if (foundStraight)
                {
                    for (int cardIdx = 0; cardIdx < numberArray.Length; cardIdx++)
                    {
                        int cardValue = numberArray[cardIdx];
                        if (cardValue == 1) cardValue = 14;

                        for (int m = 0; m < 5; m++)
                        {
                            int straightValue = i + m + 1;
                            if (straightValue == 14) straightValue = 1;

                            if (cardValue == straightValue && !main5.Contains(cardIdx))
                            {
                                main5.Add(cardIdx);
                                break;
                            }
                        }

                        if (main5.Count == 5) break;
                    }
                    break;
                }
            }

            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 5 + kicker;
        }
        else if (getLargestOf(numbers) == 3)
        {
            int tripValue = getIndexOfMostFrequent(numbers) + 1;
            if (tripValue == 14) tripValue = 1;

            for (int i = 0; i < numberArray.Length; i++)
            {
                if (numberArray[i] == tripValue)
                {
                    main5.Add(i);
                }
            }

            List<int> kickers = new List<int>();
            for (int i = 0; i < numberArray.Length; i++)
            {
                if (!main5.Contains(i))
                {
                    kickers.Add(i);
                }
            }
            kickers.Sort((a, b) => numberArray[b].CompareTo(numberArray[a]));
            main5.AddRange(kickers.Take(2));
            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 4 + kicker;
        }
        else if (getLargestOf(numbers) == 2 && extraPair)
        {
            List<int> pairIndices = new List<int>();
            for (int i = numbers.Length - 1; i >= 0; i--)
            {
                if (numbers[i] >= 2)
                {
                    pairIndices.Add(i);
                }
            }

            foreach (int pairIdx in pairIndices.Take(2))
            {
                int pairValue = pairIdx + 1;
                if (pairValue == 14) pairValue = 1;

                for (int i = 0; i < numberArray.Length; i++)
                {
                    if (numberArray[i] == pairValue)
                    {
                        main5.Add(i);
                    }
                }
            }

            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                if (!main5.Contains(i))
                {
                    main5.Add(i);
                    break;
                }
            }

            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 3 + kicker;
        }
        else if (getLargestOf(numbers) == 2)
        {
            int pairValue = getIndexOfMostFrequent(numbers) + 1;
            if (pairValue == 14) pairValue = 1;

            for (int i = 0; i < numberArray.Length; i++)
            {
                if (numberArray[i] == pairValue)
                {
                    main5.Add(i);
                }
            }

            List<int> kickers = new List<int>();
            for (int i = 0; i < numberArray.Length; i++)
            {
                if (!main5.Contains(i))
                {
                    kickers.Add(i);
                }
            }
            kickers.Sort((a, b) => numberArray[b].CompareTo(numberArray[a]));
            main5.AddRange(kickers.Take(3));
            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 2 + kicker;
        }
        else
        {
            List<int> allCards = new List<int>();
            for (int i = 0; i < numberArray.Length; i++)
            {
                allCards.Add(i);  
            }
            allCards.Sort((a, b) => numberArray[b].CompareTo(numberArray[a]));
            main5 = allCards.Take(5).ToList();
            kicker = (float)main5.Max() / 100;

            foreach (int idx in main5)
            {
                if (idx < 5)
                {
                    winningCards.Add(riverArray[idx]);
                }
                else
                {
                    winningCards.Add(handArray[playerNum][idx - 5]);
                }
            }
            return 1 + kicker;
        }
    }

    void CheckOutcome()
    {
        float[] winner = new float[playerAmount];
        float largest = 0;
        for (int p = 0; p < playerAmount; p++)
        {
            winner[p] = CheckWin(p, riverNumArray, riverFaceArray, handNumArray[p], handFaceArray[p]);

            if (winner[p] > largest)
            {
                largest = winner[p];
            }
        }

        int winIndex = getIndexOfMostFrequent(winner);

        winningCards.Clear();        
        print("Winner @ Player" + winIndex + " == " + CheckWin(winIndex, riverNumArray, riverFaceArray, handNumArray[winIndex], handFaceArray[winIndex]));
    }

    bool getStraight(int[] numbers)
    {
        for (int i = 0; i <= numbers.Length - 5; i++)
        {
            int straightCount = 0;

            for (int m = 0; m < 5; m++)
            {
                if (numbers[i + m] >= 1)
                {
                    straightCount++;
                }
                else
                {
                    break;
                }
            }

            if (straightCount == 5)
            {
                return true;
            }
        }

        return false;
    }

    int getLargestOf(int[] array)
    {
        int largest = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > largest)
            {
                largest = array[i];
            }
        }

        return largest;
    }

    int getIndexOfMostFrequent(int[] array)
    {
        int largest = 0;
        int index = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > largest)
            {
                largest = array[i];
                index = i;
            }
        }

        return index;
    }

    int getIndexOfMostFrequent(float[] array)
    {
        float largest = 0;
        int index = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > largest)
            {
                largest = array[i];
                index = i;
            }
        }

        return index;
    }

    int getIndexOfLargestOf(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            if (array[i] > 0)
            {
                return i + 1;
            }           
        }

        return 0;
    }
}