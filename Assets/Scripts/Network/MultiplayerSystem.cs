using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSystem : MonoBehaviour
{
    [SerializeField]
	private InputField m_IPAddress = null;
	[SerializeField]
	private InputField m_PortNumber = null;
	[SerializeField]
	private bool m_DontChangeSceneOnConnect = false;
	[SerializeField]
	private string m_MasterServerHost = string.Empty;
	[SerializeField]
	private ushort m_MasterServerPort = 15940;
	[SerializeField]
	private string m_NATServerHost = string.Empty;
	[SerializeField]
	private ushort NATServerPort = 15941;
	[SerializeField]
	private bool m_ConnectUsingMatchmaking = false;
	[SerializeField]
	private bool m_UseElo = false;
	[SerializeField]
	private int m_MyElo = 0;
	[SerializeField]
	private int m_EloRequired = 0;
	[SerializeField]
	private int m_MaxPlayers = 0;
	[SerializeField]
	private GameObject m_NetworkManager = null;
	[SerializeField]
	private bool m_UseMainThreadManagerForRPCs = true;
	[SerializeField]
	private bool m_GetLocalNetworkConnections = false;
	[SerializeField]
	private bool m_UseTCP = false;

	private NetworkManager m_Manager = null;
	private NetWorker m_Server;

	private bool m_Matchmaking = false;

	private void Start()
	{
		m_IPAddress.text = "127.0.0.1";
		m_PortNumber.text = "15937";

		if (!m_UseTCP)
		{
			// Do any firewall opening requests on the operating system
			NetWorker.PingForFirewall(ushort.Parse(m_PortNumber.text));
		}

		if (m_UseMainThreadManagerForRPCs)
			Rpc.MainThreadRunner = MainThreadManager.Instance;

		if (m_GetLocalNetworkConnections)
		{
			NetWorker.localServerLocated += LocalServerLocated;
			NetWorker.RefreshLocalUdpListings(ushort.Parse(m_PortNumber.text));
		}
	}

	private void LocalServerLocated(NetWorker.BroadcastEndpoints endpoint, NetWorker sender)
	{
		Debug.Log("Found endpoint: " + endpoint.Address + ":" + endpoint.Port);
	}

	public void Connect()
	{
		if (m_ConnectUsingMatchmaking)
		{
			ConnectToMatchmaking();
			return;
		}
		ushort port;
		if(!ushort.TryParse(m_PortNumber.text, out port))
		{
			Debug.LogError("The supplied port number is not within the allowed range 0-" + ushort.MaxValue);
		    	return;
		}

		NetWorker client;

		if (m_UseTCP)
		{
			client = new TCPClient();
			((TCPClient)client).Connect(m_IPAddress.text, (ushort)port);
		}
		else
		{
			client = new UDPClient();
			if (m_NATServerHost.Trim().Length == 0)
				((UDPClient)client).Connect(m_IPAddress.text, (ushort)port);
			else
				((UDPClient)client).Connect(m_IPAddress.text, (ushort)port, m_NATServerHost, NATServerPort);
		}

		Connected(client);
	}

	public void ConnectToMatchmaking()
	{
		if (m_Matchmaking)
			return;

		m_Matchmaking = true;

		if (m_Manager == null && m_NetworkManager == null)
			throw new System.Exception("A network manager was not provided, this is required for the tons of fancy stuff");
		
		m_Manager = Instantiate(m_NetworkManager).GetComponent<NetworkManager>();

		m_Manager.MatchmakingServersFromMasterServer(m_MasterServerHost, m_MasterServerPort, m_MyElo, (response) =>
		{
			m_Matchmaking = false;
			Debug.LogFormat("Matching Server(s) count[{0}]", response.serverResponse.Count);

			//TODO: YOUR OWN MATCHMAKING EXTRA LOGIC HERE!
			// I just make it randomly pick a server... you can do whatever you please!
			if (response != null && response.serverResponse.Count > 0)
			{
				MasterServerResponse.Server server = response.serverResponse[Random.Range(0, response.serverResponse.Count)];
				//TCPClient client = new TCPClient();
				UDPClient client = new UDPClient();
				client.Connect(server.Address, server.Port);
				Connected(client);
			}
		});
	}

	public void Host()
	{
		if (m_UseTCP)
		{
			m_Server = new TCPServer(m_MaxPlayers);
			((TCPServer)m_Server).Connect();
		}
		else
		{
			m_Server = new UDPServer(m_MaxPlayers);

			if (m_NATServerHost.Trim().Length == 0)
				((UDPServer)m_Server).Connect(m_IPAddress.text, ushort.Parse(m_PortNumber.text));
			else
				((UDPServer)m_Server).Connect(port: ushort.Parse(m_PortNumber.text), natHost: m_NATServerHost, natPort: NATServerPort);
		}

		m_Server.playerTimeout += (player, sender) =>
		{
			Debug.Log("Player " + player.NetworkId + " timed out");
		};

		Connected(m_Server);
	}

	private void TestLocalServerFind(NetWorker.BroadcastEndpoints endpoint, NetWorker sender)
	{
		Debug.Log("Address: " + endpoint.Address + ", Port: " + endpoint.Port + ", Server? " + endpoint.IsServer);
	}

	public void Connected(NetWorker networker)
	{
		if (!networker.IsBound)
		{
			Debug.LogError("NetWorker failed to bind");
			return;
		}

		if (m_Manager == null && m_NetworkManager == null)
		{
			Debug.LogWarning("A network manager was not provided, generating a new one instead");
			m_NetworkManager = new GameObject("Network Manager");
			m_Manager = m_NetworkManager.AddComponent<NetworkManager>();
		}
		else if (m_Manager == null)
			m_Manager = Instantiate(m_NetworkManager).GetComponent<NetworkManager>();

		// If we are using the master server we need to get the registration data
		JSONNode masterServerData = null;
		if (!string.IsNullOrEmpty(m_MasterServerHost))
		{
			string serverId = "myGame";
			string serverName = "Forge Game";
			string type = "Deathmatch";
			string mode = "Teams";
			string comment = "Demo comment...";

			masterServerData = m_Manager.MasterServerRegisterData(networker, serverId, serverName, type, mode, comment, m_UseElo, m_EloRequired);
		}

		m_Manager.Initialize(networker, m_MasterServerHost, m_MasterServerPort, masterServerData);

		if (networker is IServer)
		{
			if (!m_DontChangeSceneOnConnect)
				AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.LOBBY_SCENE_INDEX);
			else
				NetworkObject.Flush(networker); //Called because we are already in the correct scene!
		}
	}

	private void OnApplicationQuit()
	{
		if (m_GetLocalNetworkConnections)
			NetWorker.EndSession();

		if (m_Server != null) m_Server.Disconnect(true);
	}
}