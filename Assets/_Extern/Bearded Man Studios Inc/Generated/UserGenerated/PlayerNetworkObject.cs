using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0.15,0]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private float _axisMagnitude;
		public event FieldEvent<float> axisMagnitudeChanged;
		public InterpolateFloat axisMagnitudeInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float axisMagnitude
		{
			get { return _axisMagnitude; }
			set
			{
				// Don't do anything if the value is the same
				if (_axisMagnitude == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_axisMagnitude = value;
				hasDirtyFields = true;
			}
		}

		public void SetaxisMagnitudeDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_axisMagnitude(ulong timestep)
		{
			if (axisMagnitudeChanged != null) axisMagnitudeChanged(_axisMagnitude, timestep);
			if (fieldAltered != null) fieldAltered("axisMagnitude", _axisMagnitude, timestep);
		}
		[ForgeGeneratedField]
		private float _vertVelocity;
		public event FieldEvent<float> vertVelocityChanged;
		public InterpolateFloat vertVelocityInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float vertVelocity
		{
			get { return _vertVelocity; }
			set
			{
				// Don't do anything if the value is the same
				if (_vertVelocity == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_vertVelocity = value;
				hasDirtyFields = true;
			}
		}

		public void SetvertVelocityDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_vertVelocity(ulong timestep)
		{
			if (vertVelocityChanged != null) vertVelocityChanged(_vertVelocity, timestep);
			if (fieldAltered != null) fieldAltered("vertVelocity", _vertVelocity, timestep);
		}
		[ForgeGeneratedField]
		private bool _grounded;
		public event FieldEvent<bool> groundedChanged;
		public Interpolated<bool> groundedInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool grounded
		{
			get { return _grounded; }
			set
			{
				// Don't do anything if the value is the same
				if (_grounded == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_grounded = value;
				hasDirtyFields = true;
			}
		}

		public void SetgroundedDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_grounded(ulong timestep)
		{
			if (groundedChanged != null) groundedChanged(_grounded, timestep);
			if (fieldAltered != null) fieldAltered("grounded", _grounded, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			axisMagnitudeInterpolation.current = axisMagnitudeInterpolation.target;
			vertVelocityInterpolation.current = vertVelocityInterpolation.target;
			groundedInterpolation.current = groundedInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _axisMagnitude);
			UnityObjectMapper.Instance.MapBytes(data, _vertVelocity);
			UnityObjectMapper.Instance.MapBytes(data, _grounded);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_axisMagnitude = UnityObjectMapper.Instance.Map<float>(payload);
			axisMagnitudeInterpolation.current = _axisMagnitude;
			axisMagnitudeInterpolation.target = _axisMagnitude;
			RunChange_axisMagnitude(timestep);
			_vertVelocity = UnityObjectMapper.Instance.Map<float>(payload);
			vertVelocityInterpolation.current = _vertVelocity;
			vertVelocityInterpolation.target = _vertVelocity;
			RunChange_vertVelocity(timestep);
			_grounded = UnityObjectMapper.Instance.Map<bool>(payload);
			groundedInterpolation.current = _grounded;
			groundedInterpolation.target = _grounded;
			RunChange_grounded(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _axisMagnitude);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _vertVelocity);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _grounded);

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
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (axisMagnitudeInterpolation.Enabled)
				{
					axisMagnitudeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					axisMagnitudeInterpolation.Timestep = timestep;
				}
				else
				{
					_axisMagnitude = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_axisMagnitude(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (vertVelocityInterpolation.Enabled)
				{
					vertVelocityInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					vertVelocityInterpolation.Timestep = timestep;
				}
				else
				{
					_vertVelocity = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_vertVelocity(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (groundedInterpolation.Enabled)
				{
					groundedInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					groundedInterpolation.Timestep = timestep;
				}
				else
				{
					_grounded = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_grounded(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (axisMagnitudeInterpolation.Enabled && !axisMagnitudeInterpolation.current.UnityNear(axisMagnitudeInterpolation.target, 0.0015f))
			{
				_axisMagnitude = (float)axisMagnitudeInterpolation.Interpolate();
				//RunChange_axisMagnitude(axisMagnitudeInterpolation.Timestep);
			}
			if (vertVelocityInterpolation.Enabled && !vertVelocityInterpolation.current.UnityNear(vertVelocityInterpolation.target, 0.0015f))
			{
				_vertVelocity = (float)vertVelocityInterpolation.Interpolate();
				//RunChange_vertVelocity(vertVelocityInterpolation.Timestep);
			}
			if (groundedInterpolation.Enabled && !groundedInterpolation.current.UnityNear(groundedInterpolation.target, 0.0015f))
			{
				_grounded = (bool)groundedInterpolation.Interpolate();
				//RunChange_grounded(groundedInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkObject() : base() { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
