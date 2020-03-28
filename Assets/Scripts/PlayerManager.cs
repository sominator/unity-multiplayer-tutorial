using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Back1;
    public GameObject Back2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;

    GameObject cardPlayed;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        DropZone = GameObject.Find("DropZone");
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void DealCards()
    {
        RpcDealCards();
    }

    [ClientRpc]
    void RpcDealCards()
    {
        for (var i = 0; i < 5; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<DragDrop>().PlayerManager = gameObject;
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCard.tag = "PlayerCard";

            GameObject enemyCard = null;
            
            if (isServer)
            {
                enemyCard = Instantiate(Back2, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (isClientOnly)
            {
                enemyCard = Instantiate(Back1, new Vector3(0, 0, 0), Quaternion.identity);
            }

            if (enemyCard != null)
            {
                enemyCard.transform.SetParent(EnemyArea.transform, false);
                enemyCard.tag = "EnemyCard";
            }
        }
    }

    [ClientRpc]
    void RpcShowCard(GameObject card)
    {
        card.transform.SetParent(DropZone.transform, false);
    }

    [Command]
    public void CmdShowCard(string name)
    {
        GameObject card;
        if (name == "Card1(Clone)")
        {
            card = Instantiate(Card1);
        }
        else
        {
            card = Instantiate(Card2);
        }
        NetworkServer.Spawn(card);
        RpcShowCard(card);
    }

    [ClientRpc]
    public void RpcHello()
    {
        Debug.Log("Hello Rpc!");
    }

    [Command]
    public void CmdHello()
    {
        Debug.Log("Hello Cmd! " + "hasAuthority: " + hasAuthority + " isLocalPlayer: " + isLocalPlayer);
    }

    public void SayHello()
    {
        if (!isLocalPlayer) return;
        CmdHello();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown("space"))
        {
            CmdHello();
        }
    }

    //isClient is true for all objects, even those of other players
    //isLocalPlayer is only true for that client's player object
    //hasAuthority is true for other objects the client has specific authority over in addition to the player object
}
