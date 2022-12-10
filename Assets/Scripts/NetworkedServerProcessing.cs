using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedServerProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromClient(string msg, int clientConnectionID)
    {
        Debug.Log("msg received = " + msg + ".  connection id = " + clientConnectionID);

        string[] csv = msg.Split('|');
        int signifier = 0;
        if (int.TryParse(csv[0], out _))
        {
            signifier = int.Parse(csv[0]);
        }
        switch (signifier)
        {
            case ClientToServerSignifiers.HereIsMyPosition:
                gameLogic.SetPlayerPosition(float.Parse(csv[1]), float.Parse(csv[2]), clientConnectionID, false);
                break;
            case ClientToServerSignifiers.PressedButton:
                gameLogic.PlayerWithThisIDPressedThisButton(clientConnectionID, csv[1]);
                break;
            case ClientToServerSignifiers.ButtonReleased:
                gameLogic.PlayerWithThisIDReleasedThisButton(clientConnectionID, csv[1], float.Parse(csv[2]), float.Parse(csv[3]),true);
                break;
        }
    }
    static public void SendMessageToClient(string msg, int clientConnectionID)
    {
        networkedServer.SendMessageToClient(msg, clientConnectionID);
    }

    static public void SendMessageToClientWithSimulatedLatency(string msg, int clientConnectionID)
    {
        networkedServer.SendMessageToClientWithSimulatedLatency(msg, clientConnectionID);
    }

    
    #endregion

    #region Connection Events

    static public void ConnectionEvent(int clientConnectionID)
    {
        Debug.Log("New Connection, ID == " + clientConnectionID);

        gameLogic.SetNewPlayer(clientConnectionID);
        gameLogic.SendAllCurrentClients(clientConnectionID);
    }
    static public void DisconnectionEvent(int clientConnectionID)
    {
        Debug.Log("New Disconnection, ID == " + clientConnectionID);
        foreach(Player player in gameLogic.listOfPlayers)
        {
            if(player.id == clientConnectionID)
            {
                foreach(Player p in gameLogic.listOfPlayers)
                {
                    if(p.id != clientConnectionID)
                    {
                        SendMessageToClient(ServerToClientSignifiers.DestroyCertainPlayer.ToString() + '|' + player.id, p.id);
                    }
                }
                gameLogic.listOfPlayers.Remove(player);
                
                break;
            }
        }
        
    }

    #endregion

    #region Setup
    static NetworkedServer networkedServer;
    static GameLogic gameLogic;

    static public void SetNetworkedServer(NetworkedServer NetworkedServer)
    {
        networkedServer = NetworkedServer;
    }
    static public NetworkedServer GetNetworkedServer()
    {
        return networkedServer;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion
}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int HereIsMyPosition = 1;
    public const int PressedButton = 2;
    public const int ButtonReleased = 3;
}


static public class ServerToClientSignifiers
{
    public const int RequestForPositionAndGivingSpeed = 1;
    public const int NewClientJoined = 2;
    public const int SendBackID = 3;
    public const int PressButton = 4;
    public const int ReleaseButton = 5;
    public const int SendAllClients = 6;
    public const int HereNewDataForPlayerByTheID = 7;
    public const int DestroyCertainPlayer = 8;
}

#endregion