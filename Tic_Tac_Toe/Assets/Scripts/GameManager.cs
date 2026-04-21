using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set;}

    public event EventHandler <OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;
    public class OnClickedOnGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
        public PlayerType playerType;
    }
    public event EventHandler OnGameStarted;
    public event EventHandler OnCurrentPlayablePlayerTypeChanged;

    public enum PlayerType
    {
        None,
        Cross,
        Circle,
    }

    private PlayerType localPlayerType;
    private NetworkVariable<PlayerType> currentPlayablePlayerType = new NetworkVariable<PlayerType>();
    private PlayerType[,]playerTypeArray;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("More than one GameManger instance!");
        }
        Instance = this;

        playerTypeArray = new PlayerType[3,3];
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn: " + NetworkManager.Singleton.LocalClientId);
        if(NetworkManager.Singleton.LocalClientId == 0 )
        {
            localPlayerType = PlayerType.Cross;
        }
        else
        {
            localPlayerType = PlayerType.Circle;
        }

        if (IsServer)
        {
            
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }

        currentPlayablePlayerType.OnValueChanged += (PlayerType oldPlayerType, PlayerType newPlayerType) =>
        {
            OnCurrentPlayablePlayerTypeChanged?.Invoke(this,EventArgs.Empty);
        };
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            //Start Game
            currentPlayablePlayerType.Value = PlayerType.Cross;
            TriggerOnGameStarterRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost  )]
    private void TriggerOnGameStarterRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }
    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x ,int y, PlayerType playerType){
        Debug.Log("ClickedOnGridPosition "+ x + ", " + y);
        if(playerType != currentPlayablePlayerType.Value)
        {
            return;
        }

        if (playerTypeArray[x,y] != PlayerType.None)
        {
            //Already occupied
            return;
        }
        playerTypeArray[x,y] = playerType;
        
        
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs{
            x = x,
            y = y,
            playerType = playerType,
        });

        switch (currentPlayablePlayerType.Value)
        {
            default:
            case PlayerType.Cross:
                currentPlayablePlayerType.Value = PlayerType.Circle;
                break;
            case PlayerType.Circle:
                currentPlayablePlayerType.Value = PlayerType.Cross;
                break;
        }

    }


    public PlayerType GetLocalPlayerType()
    {
        return localPlayerType;
    }

    public PlayerType GetCurrentPlayablePlayerType()
    {
        return currentPlayablePlayerType.Value;
    }
}
