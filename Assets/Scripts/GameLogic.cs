using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    const float CharacterSpeed = 0.13f;
    float fixedDeltaTime;
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
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.SendBackID.ToString() + '|' + id , id);
        Player newPlayer = new Player();
        newPlayer.id = id;
        listOfPlayers.AddLast(newPlayer);
        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.RequestForPositionAndGivingSpeed.ToString() + '|'+ fixedDeltaTime + '|' + CharacterSpeed , id);
    }
    public void SetPlayerPosition(float posX, float posY, int id)
    {
        foreach (Player player in listOfPlayers)
        {
            if (player.id == id)
            {
                player._pos = new Vector2(posX,posY);
               foreach(Player p in listOfPlayers)
               {
                    if(p.id != id)
                    {
                        NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.NewClientJoined.ToString() + '|'
                                                                      + player.id + '|' + player._pos.x + '|' + player._pos.y, p.id);
                    }
               }
                break;
            }
        }
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