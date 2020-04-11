using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15]")]
	public partial class TowerNetworkObject : NetworkObject
	{
<<<<<<< HEAD
		public const int IDENTITY = 16;
=======
		public const int IDENTITY = 15;
>>>>>>> 2943abb8be6c57b52b2180c5f6f79bd83dfac41b

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private float _captureGauge;
		public event FieldEvent<float> captureGaugeChanged;
		public InterpolateFloat captureGaugeInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float captureGauge
		{
			get { return _captureGauge; }
			set
			{
				// Don't do anything if the value is the same
				if (_captureGauge == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_captureGauge = value;
				hasDirtyFields = true;
			}
		}

		public void SetcaptureGaugeDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_captureGauge(ulong timestep)
		{
			if (captureGaugeChanged != null) captureGaugeChanged(_captureGauge, timestep);
			if (fieldAltered != null) fieldAltered("captureGauge", _captureGauge, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			captureGaugeInterpolation.current = captureGaugeInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _captureGauge);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_captureGauge = UnityObjectMapper.Instance.Map<float>(payload);
			captureGaugeInterpolation.current = _captureGauge;
			captureGaugeInterpolation.target = _captureGauge;
			RunChange_captureGauge(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _captureGauge);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (captureGaugeInterpolation.Enabled)
				{
					captureGaugeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					captureGaugeInterpolation.Timestep = timestep;
				}
				else
				{
					_captureGauge = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_captureGauge(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (captureGaugeInterpolation.Enabled && !captureGaugeInterpolation.current.UnityNear(captureGaugeInterpolation.target, 0.0015f))
			{
				_captureGauge = (float)captureGaugeInterpolation.Interpolate();
				//RunChange_captureGauge(captureGaugeInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TowerNetworkObject() : base() { Initialize(); }
		public TowerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TowerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
