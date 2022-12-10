using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public float CharacterSpeed = 0.05f;
    public LinkedList<Player> listOfPlayers = new LinkedList<Player>();
   
    void Start()
    {
        NetworkedServerProcessing.SetGameLogic(this);
    }

    void Update()
    {
        //Debug.Log(listOfPlayers.Count);
        foreach(Player p in listOfPlayers)
        {
            if(p.moving)
            {
                p.charVelocity = Vector2.zero;
                if (p.pressedWD)
                {
                    p.charVelocity.x = CharacterSpeed;
                    p.charVelocity.y = CharacterSpeed;

                }
                else if (p.pressedWA)
                {
                    p.charVelocity.x = -CharacterSpeed;
                    p.charVelocity.y = CharacterSpeed;

                }
                else if (p.pressedSD)
                {
                    p.charVelocity.x = CharacterSpeed;
                    p.charVelocity.y = -CharacterSpeed;

                }
                else if (p.pressedSA)
                {
                    p.charVelocity.x = -CharacterSpeed;
                    p.charVelocity.y = -CharacterSpeed;

                }
                else if (p.pressedD)
                    p.charVelocity.x = CharacterSpeed;
                else if (p.pressedA)
                    p.charVelocity.x = -CharacterSpeed;
                else if (p.pressedW)
                    p.charVelocity.y = CharacterSpeed;
                else if (p.pressedS)
                    p.charVelocity.y = -CharacterSpeed;

               p._pos += p.charVelocity *= Time.fixedDeltaTime;
               SetPlayerPosition(p._pos.x, p._pos.y, p.id, true);
            }
        }
    }

    public void SetNewPlayer(int id)
    {
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.SendBackID.ToString() + '|' + id, id);
        Player newPlayer = new Player();
        newPlayer.id = id;
        listOfPlayers.AddLast(newPlayer);
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.RequestForPosition.ToString(), id);
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
                    else if(forUpdate)
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
            if (player.id == id)
            {
                player.moving = true;
                switch (button)
                {
                    case "W":
                        player.pressedW = true;
                        break;
                    case "S":
                        player.pressedS = true;
                        break;
                    case "D":
                        player.pressedD = true;
                        break;
                    case "A":
                        player.pressedA = true;
                        break;
                    case "WA":
                        player.pressedWA = true;
                        break;
                    case "WD":
                        player.pressedWD = true;
                        break;
                    case "SA":
                        player.pressedSA = true;
                        break;
                    case "SD":
                        player.pressedSD = true;
                        break;
                }
            }
        }

    }
    public void PlayerWithThisIDReleasedThisButton(int id, string button)
    {
        foreach (Player player in listOfPlayers)
        {
            if (player.id == id)
            {
                player.moving = false;
                switch (button)
                {
                    case "W":
                        player.pressedW = false;
                        player.pressedWA = false;
                        player.pressedWD = false;
                        break;
                    case "S":
                        player.pressedS = false;
                        player.pressedSA = false;
                        player.pressedSD = false;

                        break;
                    case "D":
                        player.pressedD = false;
                        player.pressedWD = false;
                        player.pressedSD = false;
                        break;
                    case "A":
                        player.pressedA = false;
                        player.pressedWA = false;
                        player.pressedSA = false;

                        break;

                }
            }
           
        }
    }

    public void SendAllCurrentClients(int playerID)
    {
        foreach(Player player in listOfPlayers)
        {
            if(player.id != 0 && player.id != playerID)
            {
                NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.SendAllClients.ToString() + '|'
                                                                       + player.id + '|' + player._pos.x + '|' + player._pos.y,
                                                                        playerID);
            }
        }    
    }
}
public class Player
{
    public int id;
    public bool moving = false;
    public Vector2 _pos;
    public Vector2 charVelocity;
    public bool pressedW;
    public bool pressedS;
    public bool pressedA;
    public bool pressedD;
    public bool pressedWA;
    public bool pressedWD;
    public bool pressedSA;
    public bool pressedSD;

}