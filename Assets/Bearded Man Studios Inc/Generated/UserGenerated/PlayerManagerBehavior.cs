using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"int\", \"int\"][\"uint\", \"int\"][][\"uint\", \"int\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"teamId\", \"position\"][\"networkId\", \"teamId\"][][\"networkId\", \"teamId\"]]")]
	public abstract partial class PlayerManagerBehavior : NetworkBehavior
	{
		public const byte RPC_TRIGGER_SETUP_PLAYER = 0 + 5;
		public const byte RPC_TRIGGER_PLAYER_SYNC = 1 + 5;
		public const byte RPC_TRIGGER_SETUP_REVEAL = 2 + 5;
		public const byte RPC_REQUEST_PLAYER_SYNC = 3 + 5;

		public PlayerManagerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;

			networkObject = (PlayerManagerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("TriggerSetupPlayer", TriggerSetupPlayer, typeof(int), typeof(int));
			networkObject.RegisterRpc("TriggerPlayerSync", TriggerPlayerSync, typeof(uint), typeof(int));
			networkObject.RegisterRpc("TriggerSetupReveal", TriggerSetupReveal);
			networkObject.RegisterRpc("RequestPlayerSync", RequestPlayerSync, typeof(uint), typeof(int));

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
			Initialize(new PlayerManagerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new PlayerManagerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// int teamId
		/// int position
		/// </summary>
		public abstract void TriggerSetupPlayer(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint networkId
		/// int teamId
		/// </summary>
		public abstract void TriggerPlayerSync(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerSetupReveal(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void RequestPlayerSync(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}