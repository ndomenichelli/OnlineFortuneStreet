using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayersInGame : MonoBehaviourPunCallbacks
{
    // [SerializeField]
    // public PlayerToken playerToken;

    public PlayerToken[] playersInGame;

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
        // Debug.Log("addPlayerToGame");

        // playerToken = GameObject.FindObjectOfType<PlayerToken>();
        
        // Player lastestPlayer = PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length-1];
        
        // // Debug.Log("player actor number: " + player.ActorNumber);
        // playerTokens.Add(lastestPlayer.ActorNumber - 1, playerToken);
        
        // // log
        // // foreach(var pt in playerTokens)
        // // {
		// // 	Debug.Log("Key: " + pt.Key + ", Value: " + pt.Value);
        // // }

        playersInGame = GameObject.FindObjectsOfType<PlayerToken>();

    }
}
