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
                gameLogic.SetPlayerPosition(float.Parse(csv[1]), float.Parse(csv[2]), clientConnectionID);
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
    }
    static public void DisconnectionEvent(int clientConnectionID)
    {
        Debug.Log("New Disconnection, ID == " + clientConnectionID);
        foreach(Player player in gameLogic.listOfPlayers)
        {
            if(player.id == clientConnectionID)
            {
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
}

static public class ServerToClientSignifiers
{
    public const int RequestForPositionAndGivingSpeed = 1;
    public const int NewClientJoined = 2;
}

#endregion