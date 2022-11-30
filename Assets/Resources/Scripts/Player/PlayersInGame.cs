using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayersInGame : MonoBehaviourPunCallbacks
{
    public PlayerToken[] playerTokens;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void addPlayerToGame()
    {
        int i = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // create player token for each player in room
            playerTokens[i] = GameObject.FindObjectOfType<PlayerToken>();
            Debug.Log("players in game for player: " + player + " i: " + i);

            i++;
        }
    }
}
