using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"int\"][][]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"playerCount\"][][]]")]
	public abstract partial class PlayerNetworkManagerBehavior : NetworkBehavior
	{
		public const byte RPC_SET_PLAYER_COUNT = 0 + 5;
		public const byte RPC_SET_CLIENT_READY = 1 + 5;
		public const byte RPC_SET_ALL_READY = 2 + 5;

		public PlayerNetworkManagerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;

			networkObject = (PlayerNetworkManagerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("SetPlayerCount", SetPlayerCount, typeof(int));
			networkObject.RegisterRpc("SetClientReady", SetClientReady);
			networkObject.RegisterRpc("SetAllReady", SetAllReady);

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					bool changePos = (transformFlags & 0x01) != 0;
                    bool changeRotation = (transformFlags & 0x02) != 0;
                    if (changePos || changeRotation)
                    {
                        MainThreadManager.Run(()=>
                        {
                            if (changePos)
                                transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
                            if (changeRotation)
                                transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
                        });
                    }
				}
			}

			MainThreadManager.Run(() =>
			{
				gameObject.SetActive(true);
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new PlayerNetworkManagerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new PlayerNetworkManagerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// int playerCount
		/// </summary>
		public abstract void SetPlayerCount(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SetClientReady(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SetAllReady(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}