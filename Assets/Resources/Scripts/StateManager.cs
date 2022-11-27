using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StateManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    ScoreDisplay scoreDisplay;
    public CameraFollow cameraFollow;

    // game settings
    // public const int NumberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
    public const int NumberOfPlayers = 4;
    public const int buyToRent = 10;

    // initial playerid, starts at 0
    public int CurrentPlayerID = 0;

    public int DiceTotal;
    public bool isDoneRolling = false;
    public bool isDoneClicking = false;
    public bool isDoneAnimating = false;

    public GameObject NoLegalMovesPopup;

    public GameObject stopHere;
    public GameObject buyHere;

    public PlayerToken[] playerTokens;

    void Start()
    {
        scoreDisplay = GameObject.FindObjectOfType<ScoreDisplay>();
        cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
    }

    [PunRPC]
    void addPlayer2()
    {
        // playerScoreDisplays = this.transform.GetChild(0).GetComponentsInChildren<Canvas>();
        int i = 0;

        // get amount of players in game
        // Debug.Log("player list: " + PhotonNetwork.PlayerList);

        // players = PhotonNetwork.PlayerList;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // create player token for each player in room
            playerTokens[i] = GameObject.FindObjectOfType<PlayerToken>();
            Debug.Log("state manager for player: " + player + " i: " + i);

            i++;
        }

        // Debug.Log("Current player id: " + CurrentPlayerID);
        // cameraFollow.target = playerTokens[CurrentPlayerID].transform;
    }

    public void NewTurn()
    {
        isDoneRolling = false;
        isDoneClicking = false;
        isDoneAnimating = false;

        currentPhase = TurnPhase.BEFORE_ROLL;

        CurrentPlayerID = (CurrentPlayerID + 1) % NumberOfPlayers;

        Debug.Log("Current player id: " + CurrentPlayerID);
        Debug.Log("playertokens new turn: " + playerTokens[CurrentPlayerID]);

        cameraFollow.target = playerTokens[CurrentPlayerID].transform;
    }
    public enum TurnPhase
    {
        BEFORE_ROLL,
        MOVEMENT,
        SPACE_EVENTS,
        STOCKS
    };
    public TurnPhase currentPhase;

    //public void AdvancePhase()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //is the turn done?
        if (isDoneRolling && isDoneClicking && isDoneAnimating)
        {
            //currentPhase = TurnPhase.SPACE_EVENTS;   
            stopHere.SetActive(true);
            //NewTurn();
        }
        if (currentPhase == TurnPhase.SPACE_EVENTS)
        {
            // shop events
            if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("Shop"))
            {
                Shop thisShop = playerTokens[CurrentPlayerID].currentSpace.transform.GetComponent<Shop>();

                // check what space player is on and do those space events (shop buying/paying)
                if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("Shop") && thisShop.ownedBy == null)
                {
                    // shop not owned
                    buyHere.SetActive(true);
                }
                else if (thisShop.ownedBy != null)
                {
                    // shop is owned, landing player pays owner, then check for buyout
                    playerTokens[CurrentPlayerID].playerStats.payPlayer(thisShop.ownedBy, thisShop.buyPrice / buyToRent);
                    NewTurn();
                }
            }
            else if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("Suit"))
            {
                // TODO: 100 cards thing

                NewTurn();
            }
            else if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("Warp"))
            {

                Warp thisWarp2 = GameObject.Find("Warp (7)").GetComponent<Warp>();

                // if on first warp
                if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("7"))
                {
                    thisWarp2 = GameObject.Find("Warp (33)").GetComponent<Warp>();
                }
                // if on second warp
                else if (playerTokens[CurrentPlayerID].currentSpace.name.Contains("33"))
                {
                    thisWarp2 = GameObject.Find("Warp (7)").GetComponent<Warp>();
                }

                // set current player to warped space
                playerTokens[CurrentPlayerID].currentSpace = thisWarp2;

                // remove player from current space
                playerTokens[CurrentPlayerID].currentSpace.playerTokensHere[CurrentPlayerID] = null;

                // set postion of current player to warped space

                playerTokens[CurrentPlayerID].SetNewTargetPosition(thisWarp2.transform.position);

                // TODO: make animation smooth
                StartCoroutine(WarpTo(thisWarp2));

                NewTurn();

            }
            // temp code for generic spaces
            else
            {
                NewTurn();
            }
        }
        else
        {
            buyHere.SetActive(false);
        }
    }
    public void CheckLegalMoves()
    {
        if (DiceTotal == 0)
        {
            StartCoroutine(NoLegalMovesCoroutine());
            return;
        }

        PlayerToken[] playerTokens = GameObject.FindObjectsOfType<PlayerToken>();

        // Debug.Log("player tokens when checking moves: " + playerTokens[0]);

        bool hasLegalMove = false;
        foreach (PlayerToken pt in playerTokens)
        {

            Debug.Log("pt.playerID: " + pt.playerID);
            Debug.Log("CurrentPlayerID: " + CurrentPlayerID);

            if (pt.playerID == CurrentPlayerID)
            {
                if (pt.CanLegallyMoveAhead(DiceTotal))
                {
                    // todo: highlight spaces that can be moved to
                    hasLegalMove = true;
                }

            }
        }
        if (hasLegalMove == false)
        {
            Debug.Log("no legal moves");
            StartCoroutine(NoLegalMovesCoroutine());
            return;
        }
    }
    IEnumerator NoLegalMovesCoroutine()
    {
        //display message
        NoLegalMovesPopup.SetActive(true);

        //wait 1 second
        yield return new WaitForSeconds(1f);

        NewTurn();
    }
    IEnumerator WarpTo(Warp target)
    {
        Vector3 velocity = Vector3.zero;

        playerTokens[CurrentPlayerID].transform.position = Vector3.SmoothDamp(
                    playerTokens[CurrentPlayerID].transform.position, target.transform.position,
                    ref velocity,
                    0.05f * Time.deltaTime
        );

        yield return new WaitForSeconds(5f);
    }
    public int getBuyToRent()
    {
        return buyToRent;
    }
}
