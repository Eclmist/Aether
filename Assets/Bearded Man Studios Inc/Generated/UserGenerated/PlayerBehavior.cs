using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[][\"uint\", \"int\", \"ulong\"][][][][]]")]
	[GeneratedRPCVariableNames("{\"types\":[[][\"networkId\", \"position\", \"customization\"][][][][]]")]
	public abstract partial class PlayerBehavior : NetworkBehavior
	{
		public const byte RPC_TRIGGER_JUMP = 0 + 5;
		public const byte RPC_TRIGGER_UPDATE_DETAILS = 1 + 5;
		public const byte RPC_TRIGGER_DAMAGED = 2 + 5;
		public const byte RPC_TRIGGER_ATTACK = 3 + 5;
		public const byte RPC_TRIGGER_DASH = 4 + 5;
		public const byte RPC_TRIGGER_BACK_DASH = 5 + 5;

		public PlayerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;

			networkObject = (PlayerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("TriggerJump", TriggerJump);
			networkObject.RegisterRpc("TriggerUpdateDetails", TriggerUpdateDetails, typeof(uint), typeof(int), typeof(ulong));
			networkObject.RegisterRpc("TriggerDamaged", TriggerDamaged);
			networkObject.RegisterRpc("TriggerAttack", TriggerAttack);
			networkObject.RegisterRpc("TriggerDash", TriggerDash);
			networkObject.RegisterRpc("TriggerBackDash", TriggerBackDash);

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
			Initialize(new PlayerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new PlayerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerJump(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerUpdateDetails(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerDamaged(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerAttack(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerDash(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void TriggerBackDash(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}