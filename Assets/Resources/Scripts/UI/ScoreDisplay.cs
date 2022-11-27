using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviourPunCallbacks
{
    public Canvas[] playerScoreDisplays;

    Text[] cashs = new Text[StateManager.NumberOfPlayers];

    Text[] netWorths = new Text[StateManager.NumberOfPlayers];

    Text[] updater = new Text[4];

    public PlayerToken[] players;

    public GameObject[] updateDisplay;

    // Start is called before the first frame update
    void Start()
    {
        addPlayer();
        //1-4
        // foreach (var playerScoreDisplay in playerScoreDisplays)
        // {
        //     //getchild 0 is player name, 1 is networth, 2 is cash
        //     Debug.Log("player 1 cash: " + players[i].playerStats.cash);
        //     players[i].playerStats.netWorth = int.Parse(playerScoreDisplay.transform.GetChild(0).GetChild(1).GetComponent<Text>().text);
        //     players[i].playerStats.cash = int.Parse(playerScoreDisplay.transform.GetChild(0).GetChild(2).GetComponent<Text>().text);

        //     netWorths[i] = playerScoreDisplay.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        //     cashs[i] = playerScoreDisplay.transform.GetChild(0).GetChild(2).GetComponent<Text>();

        //     updater[i] = playerScoreDisplay.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();

        //     Debug.Log("Player " + i);
        //     i++;
        // }
    }

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    void addPlayer()
    {
        // playerScoreDisplays = this.transform.GetChild(0).GetComponentsInChildren<Canvas>();
        int i = 0;

        // get amount of players in game
        // Debug.Log("player list: " + PhotonNetwork.PlayerList);

        // players = PhotonNetwork.PlayerList;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // create player token for each player in room
            players[i] = GameObject.FindObjectOfType<PlayerToken>();
            Debug.Log("score display for player: " + player + " i: " + i);

            playerScoreDisplays[i].gameObject.SetActive(true);
            i++;
        }

    }

    public void setCashTextDisplay(PlayerToken player)
    {
        int difference =
            player.playerStats.cash - int.Parse(cashs[player.playerID].text);

        if (difference > 0)
        {
            updater[player.playerID].color = Color.green;
        }
        else
        {
            updater[player.playerID].color = Color.red;
        }

        updater[player.playerID].text = difference.ToString();

        StartCoroutine(showCashUpdate(player));

        cashs[player.playerID].text = player.playerStats.cash.ToString();
    }

    public void setNetWorthDisplay(PlayerToken player)
    {
        netWorths[player.playerID].text =
            player.playerStats.netWorth.ToString();
    }

    IEnumerator showCashUpdate(PlayerToken player)
    {
        //display message
        updateDisplay[player.playerID].SetActive(true);

        //wait 2 seconds
        yield return new WaitForSeconds(2f);

        updateDisplay[player.playerID].SetActive(false);
    }

    public void updateSuits(PlayerToken player, Suit.suitType suit)
    {
        string[] suitsChar = { "♤", "♡", "♧", "♢" };
        string[] suitsOwnedChar = { "♠", "♥", "♣", "♦" };

        for (int i = 0; i < StateManager.NumberOfPlayers; i++)
        {
            if (players[player.playerID].playerStats.suitsOwned[i])
            {
                this
                    .transform
                    .GetChild(0)
                    .GetChild(player.playerID)
                    .GetChild(0)
                    .GetChild(4)
                    .GetChild(i)
                    .GetComponent<Text>()
                    .text = suitsOwnedChar[i];
                this
                    .transform
                    .GetChild(0)
                    .GetChild(player.playerID)
                    .GetChild(0)
                    .GetChild(4)
                    .GetChild(i)
                    .GetComponent<Text>()
                    .fontSize = 20;
            }
            else
            {
                this
                    .transform
                    .GetChild(0)
                    .GetChild(player.playerID)
                    .GetChild(0)
                    .GetChild(4)
                    .GetChild(i)
                    .GetComponent<Text>()
                    .text = suitsChar[i];
            }
        }
    }
}
