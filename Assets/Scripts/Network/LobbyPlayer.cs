using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbyPlayer : LobbyPlayerBehavior
{
    [SerializeField]
    private Text m_PlayerName;

    public override void SetName(RpcArgs args)
    {
        m_PlayerName.text = args.GetNext<string>();
    }

    public void UpdateName(string name)
    {
        m_PlayerName.text = name;
        networkObject.SendRpc(RPC_SET_NAME, Receivers.All, name);
    }

    public void UpdateNameFor(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_SET_NAME, m_PlayerName.text);
    }
}
