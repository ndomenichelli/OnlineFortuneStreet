using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviourPunCallbacks
{
    public Sprite[] DiceImages;

    StateManager stateManager;

    public GameObject rollButton;

    // Start is called before the first frame update
    void Start()
    {
        //amount of dice
        //DiceValues = new int[1];
        stateManager = GameObject.FindObjectOfType<StateManager>();
        spacesDisplay = GameObject.FindObjectOfType<SpaceToMoveDisplay>();

        if (photonView.IsMine)
        {
            // this.gameObject.SetActive(true);
        }
        else
        {
            rollButton.SetActive(false);
        }
    }

    SpaceToMoveDisplay spacesDisplay;

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    public void NewTurn()
    {
        if (photonView.IsMine)
        {
            rollButton.SetActive(false);
        }
        else
        {
            rollButton.SetActive(true);
        }
    }

    public void rollAndSendNumber()
    {
        // roll 1-7 and send to all players so game is synced

        int numberToSend = Random.Range(1, 8);
        this
            .GetComponent<PhotonView>()
            .RPC("RollTheDice", RpcTarget.AllBuffered, numberToSend);
    }

    [PunRPC]
    public void RollTheDice(int number)
    {
        if (stateManager.isDoneRolling)
        {
            // already rolled
            return;
        }

        if (photonView.IsMine)
        {
            rollButton.SetActive(true);
        }
        else
        {
            rollButton.SetActive(false);
        }

        stateManager.DiceTotal = number;
        spacesDisplay.setDisplay(stateManager.DiceTotal);

        this.transform.GetChild(0).GetComponent<Image>().sprite =
            DiceImages[stateManager.DiceTotal - 1];

        // after roll, hide roll button
        rollButton.SetActive(false);

        // multiple dice code
        //DiceTotal = 0;
        //for (int i = 0; i < DiceValues.Length; i++)
        //{
        //    DiceValues[i] = Random.Range(1, 8);
        //    DiceTotal += DiceValues[i];
        //    // TODO:
        //    // update visual for dice rolling
        //    //this.transform.GetChild(i).GetComponent<Text>().text = "" + DiceTotal;
        //    this.transform.GetChild(i).GetComponent<Image>().sprite = DiceImages[i];
        //    //etc
        //    Debug.Log("Dice number: " + i);
        //}
        //Debug.Log("Rolled: " + DiceTotal);
        //hard code roll
        stateManager.DiceTotal = 2;

        stateManager.isDoneRolling = true;
        stateManager.currentPhase = StateManager.TurnPhase.MOVEMENT;
        stateManager.CheckLegalMoves();

        //Debug.Log("done rolling");
    }
}
