using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Unity
{
	public partial class NetworkManager : MonoBehaviour
	{
		public delegate void InstantiateEvent(INetworkBehavior unityGameObject, NetworkObject obj);
		public event InstantiateEvent objectInitialized;
		protected BMSByte metadata = new BMSByte();

		public GameObject[] ChatManagerNetworkObject = null;
		public GameObject[] CubeForgeGameNetworkObject = null;
		public GameObject[] ExampleProximityPlayerNetworkObject = null;
		public GameObject[] LobbyPlayerNetworkObject = null;
		public GameObject[] LobbySystemNetworkObject = null;
		public GameObject[] NetworkCameraNetworkObject = null;
		public GameObject[] PlayerNetworkObject = null;
		public GameObject[] TestNetworkObject = null;
		public GameObject[] PlayerManagerNetworkObject = null;

		protected virtual void SetupObjectCreatedEvent()
		{
			Networker.objectCreated += CaptureObjects;
		}

		protected virtual void OnDestroy()
		{
		    if (Networker != null)
				Networker.objectCreated -= CaptureObjects;
		}
		
		private void CaptureObjects(NetworkObject obj)
		{
			if (obj.CreateCode < 0)
				return;
				
			if (obj is ChatManagerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (ChatManagerNetworkObject.Length > 0 && ChatManagerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(ChatManagerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<ChatManagerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is CubeForgeGameNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (CubeForgeGameNetworkObject.Length > 0 && CubeForgeGameNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(CubeForgeGameNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<CubeForgeGameBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is ExampleProximityPlayerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (ExampleProximityPlayerNetworkObject.Length > 0 && ExampleProximityPlayerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(ExampleProximityPlayerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<ExampleProximityPlayerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is LobbyPlayerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (LobbyPlayerNetworkObject.Length > 0 && LobbyPlayerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(LobbyPlayerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<LobbyPlayerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is LobbySystemNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (LobbySystemNetworkObject.Length > 0 && LobbySystemNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(LobbySystemNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<LobbySystemBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is NetworkCameraNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (NetworkCameraNetworkObject.Length > 0 && NetworkCameraNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(NetworkCameraNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkCameraBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is PlayerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (PlayerNetworkObject.Length > 0 && PlayerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(PlayerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<PlayerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is TestNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (TestNetworkObject.Length > 0 && TestNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(TestNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<TestBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is PlayerManagerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (PlayerManagerNetworkObject.Length > 0 && PlayerManagerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(PlayerManagerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<PlayerManagerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
		}

		protected virtual void InitializedObject(INetworkBehavior behavior, NetworkObject obj)
		{
			if (objectInitialized != null)
				objectInitialized(behavior, obj);

			obj.pendingInitialized -= InitializedObject;
		}

		[Obsolete("Use InstantiateChatManager instead, its shorter and easier to type out ;)")]
		public ChatManagerBehavior InstantiateChatManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(ChatManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<ChatManagerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<ChatManagerBehavior>().networkObject = (ChatManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateCubeForgeGame instead, its shorter and easier to type out ;)")]
		public CubeForgeGameBehavior InstantiateCubeForgeGameNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(CubeForgeGameNetworkObject[index]);
			var netBehavior = go.GetComponent<CubeForgeGameBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<CubeForgeGameBehavior>().networkObject = (CubeForgeGameNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateExampleProximityPlayer instead, its shorter and easier to type out ;)")]
		public ExampleProximityPlayerBehavior InstantiateExampleProximityPlayerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(ExampleProximityPlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<ExampleProximityPlayerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<ExampleProximityPlayerBehavior>().networkObject = (ExampleProximityPlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateLobbyPlayer instead, its shorter and easier to type out ;)")]
		public LobbyPlayerBehavior InstantiateLobbyPlayerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(LobbyPlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<LobbyPlayerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<LobbyPlayerBehavior>().networkObject = (LobbyPlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateLobbySystem instead, its shorter and easier to type out ;)")]
		public LobbySystemBehavior InstantiateLobbySystemNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(LobbySystemNetworkObject[index]);
			var netBehavior = go.GetComponent<LobbySystemBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<LobbySystemBehavior>().networkObject = (LobbySystemNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateNetworkCamera instead, its shorter and easier to type out ;)")]
		public NetworkCameraBehavior InstantiateNetworkCameraNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(NetworkCameraNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkCameraBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<NetworkCameraBehavior>().networkObject = (NetworkCameraNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiatePlayer instead, its shorter and easier to type out ;)")]
		public PlayerBehavior InstantiatePlayerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(PlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<PlayerBehavior>().networkObject = (PlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateTest instead, its shorter and easier to type out ;)")]
		public TestBehavior InstantiateTestNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(TestNetworkObject[index]);
			var netBehavior = go.GetComponent<TestBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<TestBehavior>().networkObject = (TestNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiatePlayerManager instead, its shorter and easier to type out ;)")]
		public PlayerManagerBehavior InstantiatePlayerManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(PlayerManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerManagerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<PlayerManagerBehavior>().networkObject = (PlayerManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}

		/// <summary>
		/// Instantiate an instance of ChatManager
		/// </summary>
		/// <returns>
		/// A local instance of ChatManagerBehavior
		/// </returns>
		/// <param name="index">The index of the ChatManager prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public ChatManagerBehavior InstantiateChatManager(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (ChatManagerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: ChatManager. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(ChatManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<ChatManagerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<ChatManagerBehavior>().networkObject = (ChatManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of CubeForgeGame
		/// </summary>
		/// <returns>
		/// A local instance of CubeForgeGameBehavior
		/// </returns>
		/// <param name="index">The index of the CubeForgeGame prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public CubeForgeGameBehavior InstantiateCubeForgeGame(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (CubeForgeGameNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: CubeForgeGame. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(CubeForgeGameNetworkObject[index]);
			var netBehavior = go.GetComponent<CubeForgeGameBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<CubeForgeGameBehavior>().networkObject = (CubeForgeGameNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of ExampleProximityPlayer
		/// </summary>
		/// <returns>
		/// A local instance of ExampleProximityPlayerBehavior
		/// </returns>
		/// <param name="index">The index of the ExampleProximityPlayer prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public ExampleProximityPlayerBehavior InstantiateExampleProximityPlayer(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (ExampleProximityPlayerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: ExampleProximityPlayer. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(ExampleProximityPlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<ExampleProximityPlayerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<ExampleProximityPlayerBehavior>().networkObject = (ExampleProximityPlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of LobbyPlayer
		/// </summary>
		/// <returns>
		/// A local instance of LobbyPlayerBehavior
		/// </returns>
		/// <param name="index">The index of the LobbyPlayer prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public LobbyPlayerBehavior InstantiateLobbyPlayer(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (LobbyPlayerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: LobbyPlayer. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(LobbyPlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<LobbyPlayerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<LobbyPlayerBehavior>().networkObject = (LobbyPlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of LobbySystem
		/// </summary>
		/// <returns>
		/// A local instance of LobbySystemBehavior
		/// </returns>
		/// <param name="index">The index of the LobbySystem prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public LobbySystemBehavior InstantiateLobbySystem(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (LobbySystemNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: LobbySystem. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(LobbySystemNetworkObject[index]);
			var netBehavior = go.GetComponent<LobbySystemBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<LobbySystemBehavior>().networkObject = (LobbySystemNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of NetworkCamera
		/// </summary>
		/// <returns>
		/// A local instance of NetworkCameraBehavior
		/// </returns>
		/// <param name="index">The index of the NetworkCamera prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public NetworkCameraBehavior InstantiateNetworkCamera(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (NetworkCameraNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: NetworkCamera. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(NetworkCameraNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkCameraBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<NetworkCameraBehavior>().networkObject = (NetworkCameraNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of Player
		/// </summary>
		/// <returns>
		/// A local instance of PlayerBehavior
		/// </returns>
		/// <param name="index">The index of the Player prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public PlayerBehavior InstantiatePlayer(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (PlayerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: Player. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(PlayerNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<PlayerBehavior>().networkObject = (PlayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of Test
		/// </summary>
		/// <returns>
		/// A local instance of TestBehavior
		/// </returns>
		/// <param name="index">The index of the Test prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public TestBehavior InstantiateTest(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (TestNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: Test. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(TestNetworkObject[index]);
			var netBehavior = go.GetComponent<TestBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<TestBehavior>().networkObject = (TestNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of PlayerManager
		/// </summary>
		/// <returns>
		/// A local instance of PlayerManagerBehavior
		/// </returns>
		/// <param name="index">The index of the PlayerManager prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public PlayerManagerBehavior InstantiatePlayerManager(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (PlayerManagerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: PlayerManager. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(PlayerManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerManagerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<PlayerManagerBehavior>().networkObject = (PlayerManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
	}
}
