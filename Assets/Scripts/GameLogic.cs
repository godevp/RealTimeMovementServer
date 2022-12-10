using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public const float CharacterSpeed = 0.13f;
    public float fixedDeltaTime;
    public LinkedList<Player> listOfPlayers = new LinkedList<Player>();
    void Start()
    {
        NetworkedServerProcessing.SetGameLogic(this);
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {

    }

    public void SetNewPlayer(int id)
    {
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.SendBackID.ToString() + '|' + id, id);
        Player newPlayer = new Player();
        newPlayer.id = id;
        listOfPlayers.AddLast(newPlayer);
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.RequestForPositionAndGivingSpeed.ToString() + '|' + fixedDeltaTime + '|' + CharacterSpeed, id);
    }
    public void SetPlayerPosition(float posX, float posY, int id, bool forUpdate)
    {
        foreach (Player player in listOfPlayers)
        {
            if (player.id == id)
            {
                player._pos = new Vector2(posX, posY);
                foreach (Player p in listOfPlayers)
                {
                    if (p.id != id && !forUpdate)
                    {
                        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.NewClientJoined.ToString() + '|'
                                                                      + player.id + '|' + player._pos.x + '|' + player._pos.y, p.id);
                    }
                    else if(p.id != id && forUpdate)
                    {
                        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.HereNewDataForPlayerByTheID.ToString() + '|'
                                                                      + player.id + '|' + player._pos.x + '|' + player._pos.y, p.id);
                    }
                }
                break;
            }
        }
    }
    public void PlayerWithThisIDPressedThisButton(int id, string button)
    {
        foreach (Player player in listOfPlayers)
        {
            NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.PressButton.ToString() + '|' + id + '|' + button, player.id);
           
        }

    }
    public void PlayerWithThisIDReleasedThisButton(int id, string button)
    {
        foreach (Player player in listOfPlayers)
        {
            NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.ReleaseButton.ToString() + '|' + id + '|' + button, player.id);
            //SetPlayerPosition(x, y, id);
        }
    }

    public void SendAllCurrentClients(int playerID)
    {
        foreach(Player player in listOfPlayers)
        {
            if(player.id != 0 && player.id != playerID)
            {
                NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.SendAllClients.ToString() + '|'
                                                                       + player.id + '|' + player._pos.x + '|' + player._pos.y, playerID);
            }
        }    
    }
    public float GetCopyOfSpeed()
    {
        return CharacterSpeed;
    }
}
public class Player
{
    public int id;
    public Vector2 _pos;
    public bool pressW;
    public bool pressS;
    public bool pressA;
    public bool pressD;
}