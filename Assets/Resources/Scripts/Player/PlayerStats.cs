using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{

    ScoreDisplay scoreDisplay;

    public PlayerToken playerToken;

    public int cash;
    public int netWorth;

    public int[] stocks;

    Shop[] shopsOwned;

    public bool[] suitsOwned;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("setup player token: " + PhotonNetwork.NickName);
        scoreDisplay = GameObject.FindObjectOfType<ScoreDisplay>();
        playerToken = GameObject.FindObjectOfType<PlayerToken>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setCash(int newCash)
    {
        this.cash = newCash;
        scoreDisplay.setCashTextDisplay(playerToken);
    } 
    public void setNetWorth(int newNetWorth)
    {
        Debug.Log("set net worth");
        this.netWorth = newNetWorth;
        scoreDisplay.setNetWorthDisplay(playerToken);
    }

    public void payPlayer(PlayerToken player, int amount)
    {
        // give cash 
        this.cash -= amount;
        player.playerStats.cash += amount;

        this.setCash(this.cash);
        player.playerStats.setCash(player.playerStats.cash);

        // add networth
        this.netWorth -= amount;
        player.playerStats.netWorth += amount;

        this.setNetWorth(this.netWorth);
        player.playerStats.setNetWorth(player.playerStats.netWorth);

    }
}
