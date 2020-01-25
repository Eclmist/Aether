using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class PlayerManager : MonoBehaviour
{
    private static PlayerBehavior localPlayer;

    void Start()
    {
        LoadPlayer();
    }

    public static void LoadPlayer()
    {
        if (NetworkManager.Instance)
        {
            Vector3 spawnPosition = Vector3.one;
            localPlayer = NetworkManager.Instance.InstantiatePlayer(position: spawnPosition);
        }
        else
        {
            localPlayer = FindObjectOfType<PlayerBehavior>();
        }
    }

    public static PlayerBehavior GetLocalPlayerInstance()
    {
        return localPlayer;
    }

    private static void DestroyPlayer(PlayerBehavior player)
    {
        Destroy(player);
    }
}
