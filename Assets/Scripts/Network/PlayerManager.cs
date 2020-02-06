using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerManager : PlayerManagerBehavior
{
    [SerializeField]
    private List<Transform> m_SpawnPositionsRed;

    [SerializeField]
    private List<Transform> m_SpawnPositionsBlue;

    private List<PlayerData> m_Players;

    private PlayerData m_LocalPlayer;

    void Awake()
    {
        m_Players = new List<PlayerData>();
        AetherNetworkManager.Instance.sceneChanged += OnSceneLoaded;
    }

    public PlayerData GetLocalPlayerInstance()
    {
        return m_LocalPlayer;
    }

    public List<PlayerData> GetPlayersByTeam(int team)
    {
        List<PlayerData> filteredList = new List<PlayerData>();
        foreach (PlayerData data in m_Players)
            if (data.details.team == team)
                filteredList.Add(data);

        return filteredList;
    }

    public void LoadPlayer(AetherNetworkManager.PlayerDetails details)
    {
        MainThreadManager.Run(() => {
            Vector3 spawnPosition;
            if (details.team == 0)
                spawnPosition = m_SpawnPositionsRed[details.position].position;
            else
                spawnPosition = m_SpawnPositionsBlue[details.position].position;

            PlayerBehavior p = NetworkManager.Instance.InstantiatePlayer(position: spawnPosition);

            PlayerData playerData;
            playerData.player = p;
            playerData.details = details;
            m_LocalPlayer = playerData;
            m_Players.Add(playerData);
        });
    }

    public override void SetupPlayer(RpcArgs args)
    {
        AetherNetworkManager.PlayerDetails details;
        details.team = args.GetNext<int>();
        details.position = args.GetNext<int>();

        LoadPlayer(details);
    }

    private static void DestroyPlayer(PlayerBehavior player)
    {
        Destroy(player);
    }

    public void OnSceneLoaded(Dictionary<NetworkingPlayer, AetherNetworkManager.PlayerDetails> detailsMap)
    {
        if (!networkObject.IsServer)
            return;

        // Spawn host
        LoadPlayer(detailsMap[NetworkManager.Instance.Networker.Me]);

        // Spawn other players
        NetworkManager.Instance.Networker.IteratePlayers(np =>
        {
            if (np == NetworkManager.Instance.Networker.Me)
                return;

            AetherNetworkManager.PlayerDetails details = detailsMap[np];

            networkObject.SendRpc(np, RPC_SETUP_PLAYER, details.team, details.position);
        });
    }

    public struct PlayerData
    {
        public PlayerBehavior player;
        public AetherNetworkManager.PlayerDetails details;
    }
}
