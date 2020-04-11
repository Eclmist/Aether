using BeardedManStudios.Forge.Networking.Frame;
using System;
using MainThreadManager = BeardedManStudios.Forge.Networking.Unity.MainThreadManager;

namespace BeardedManStudios.Forge.Networking.Generated
{
	public partial class NetworkObjectFactory : NetworkObjectFactoryBase
	{
		public override void NetworkCreateObject(NetWorker networker, int identity, uint id, FrameStream frame, Action<NetworkObject> callback)
		{
			if (networker.IsServer)
			{
				if (frame.Sender != null && frame.Sender != networker.Me)
				{
					if (!ValidateCreateRequest(networker, identity, id, frame))
						return;
				}
			}
			
			bool availableCallback = false;
			NetworkObject obj = null;
			MainThreadManager.Run(() =>
			{
				switch (identity)
				{
					case AetherNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new AetherNetworkObject(networker, id, frame);
						break;
					case ChatManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new ChatManagerNetworkObject(networker, id, frame);
						break;
					case CubeForgeGameNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new CubeForgeGameNetworkObject(networker, id, frame);
						break;
					case DamageNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new DamageNetworkObject(networker, id, frame);
						break;
					case ExampleProximityPlayerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new ExampleProximityPlayerNetworkObject(networker, id, frame);
						break;
					case LobbyPlayerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new LobbyPlayerNetworkObject(networker, id, frame);
						break;
					case LobbySystemNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new LobbySystemNetworkObject(networker, id, frame);
						break;
					case MonsterAttackNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new MonsterAttackNetworkObject(networker, id, frame);
						break;
					case MonsterObjectNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new MonsterObjectNetworkObject(networker, id, frame);
						break;
					case NetworkCameraNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new NetworkCameraNetworkObject(networker, id, frame);
						break;
					case PlayerNetworkManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerNetworkManagerNetworkObject(networker, id, frame);
						break;
					case PlayerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerNetworkObject(networker, id, frame);
						break;
					case SkillsNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new SkillsNetworkObject(networker, id, frame);
						break;
					case SwordSlashNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new SwordSlashNetworkObject(networker, id, frame);
						break;
					case TestNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TestNetworkObject(networker, id, frame);
						break;
					case TowerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TowerNetworkObject(networker, id, frame);
						break;
				}

				if (!availableCallback)
					base.NetworkCreateObject(networker, identity, id, frame, callback);
				else if (callback != null)
					callback(obj);
			});
		}

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}