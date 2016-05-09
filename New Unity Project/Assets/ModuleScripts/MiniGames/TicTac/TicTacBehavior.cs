using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TicTacBehavior : MonoBehaviour {

    class TicTacField
    {
        public int id;
        public Text text;

        public TicTacField(int i, Text t)
        {
            id = i;
            text = t;
        }
    }

    public enum TicTacPlayer
    {
        HUMAN,
        AI
    }

    public TicTacPlayer currentPlayer;

    public GameObject playerSelection;
    public GameObject tictactoe;
    public GameObject end;

    private bool ended = false;
    private List<TicTacField> freeFields = new List<TicTacField>(), takenFields = new List<TicTacField>();

    // Use this for initialization
    void Start()
    {
        tictactoe.SetActive(false);
        playerSelection.SetActive(true);

        int i = 0;
        foreach(Text t in tictactoe.GetComponentsInChildren<Text>())
        {
            freeFields.Add(new TicTacField(i++, t));
        }
        
        Debug.Log(freeFields.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer == TicTacPlayer.AI && !ended)
        {
            if (freeFields.Count > 0)
            {
                int i = Random.Range(0, freeFields.Count);

                freeFields[i].text.text = "O";
                TicTacField tmpT = freeFields[i];
                freeFields.RemoveAt(i);
                takenFields.Add(tmpT);

                currentPlayer = TicTacPlayer.HUMAN;
            }
            checkWinCondition();
        }
    }

    public void setTile(GameObject t)
    {
        if (currentPlayer == TicTacPlayer.HUMAN && !ended)
        {
            if (freeFields.Count > 0)
            {
                int i = -1;
                for(i = 0; i < freeFields.Count; i++)
                {
                    if (freeFields[i].text == t.GetComponent<Text>())
                    {
                        t.GetComponent<Text>().text = "X";
                        TicTacField tmpT = freeFields[i];
                        freeFields.RemoveAt(i);
                        takenFields.Add(tmpT);
                        currentPlayer = TicTacPlayer.AI;
                        break;
                    } 
                }
            }
            checkWinCondition();
        }
    }

    public void setFirst(int p)
    {
        currentPlayer = (p == 0 ? TicTacPlayer.HUMAN : TicTacPlayer.AI);
        playerSelection.SetActive(false);
        tictactoe.SetActive(true);
    }

    void wonBy(TicTacPlayer p)
    {
        ended = true;

        //fire success or failure module here
        //FOO
    }

    void checkWinCondition()
    {
        TicTacField[] allTaken = LtoA(takenFields);
        
        for(int i = 0; i < allTaken.Length; i++)
        {
            if (i < 3 && allTaken.Length > i + 6)
            {
                if (allTaken[i] != null && allTaken[i + 3] != null && allTaken[i + 6] != null)
                {
                    //straight line vertical
                    if (allTaken[i].text.text.Equals(allTaken[i + 3].text.text) && allTaken[i + 3].text.text.Equals(allTaken[i + 6].text.text))
                    {
                        Debug.Log(allTaken[i].text.text + " wins!");
                        wonBy(allTaken[i].text.text == "X" ? TicTacPlayer.HUMAN : TicTacPlayer.AI);
                        break;
                    }
                }
            }

            if (i % 3 == 0 && allTaken.Length > i + 2)
            {
                if (allTaken[i] != null && allTaken[i + 1] != null && allTaken[i + 2] != null)
                {
                    //straight line horizontal
                    if (allTaken[i].text.text.Equals(allTaken[i + 1].text.text) && allTaken[i + 1].text.text.Equals(allTaken[i + 2].text.text))
                    {
                        Debug.Log(allTaken[i].text.text + " wins!");
                        wonBy(allTaken[i].text.text == "X" ? TicTacPlayer.HUMAN : TicTacPlayer.AI);
                        break;
                    }
                }
            }

            if (i == 0 && allTaken.Length > 8)
            {
                if (allTaken[0] != null && allTaken[4] != null && allTaken[8] != null)
                {
                    //diagonal down right
                    if (allTaken[0].text.text.Equals(allTaken[4].text.text) && allTaken[4].text.text.Equals(allTaken[8].text.text))
                    {
                        Debug.Log(allTaken[0].text.text + " wins!");
                        wonBy(allTaken[0].text.text == "X" ? TicTacPlayer.HUMAN : TicTacPlayer.AI);
                        break;
                    }
                }
            }

            if (i == 2 && allTaken.Length > 6)
            {
                if (allTaken[2] != null && allTaken[4] != null && allTaken[6] != null)
                {
                    //diagonal down left
                    if (allTaken[2].text.text.Equals(allTaken[4].text.text) && allTaken[4].text.text.Equals(allTaken[6].text.text))
                    {
                        Debug.Log(allTaken[2].text.text + " wins!");
                        wonBy(allTaken[2].text.text == "X" ? TicTacPlayer.HUMAN : TicTacPlayer.AI);
                        break;
                    }
                }
            }
        }
    }

    TicTacField[] LtoA(List<TicTacField> input)
    {
        TicTacField[] result = new TicTacField[9];
        foreach (TicTacField ttf in input)
        {
            result[ttf.id] = ttf;
        }
        return result;
    }
}
