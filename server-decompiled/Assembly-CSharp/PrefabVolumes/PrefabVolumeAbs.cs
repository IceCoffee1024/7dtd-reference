using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x020018E6 RID: 6374
	public abstract class PrefabVolumeAbs
	{
		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x0600C4B7 RID: 50359 RVA: 0x004878F5 File Offset: 0x00485AF5
		public bool Used
		{
			get
			{
				return this.used;
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x0600C4B8 RID: 50360 RVA: 0x004878FD File Offset: 0x00485AFD
		// (set) Token: 0x0600C4B9 RID: 50361 RVA: 0x00487905 File Offset: 0x00485B05
		public virtual Vector3i startPos
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.startPosInternal;
			}
			set
			{
				this.startPosInternal = value;
			}
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600C4BA RID: 50362 RVA: 0x0048790E File Offset: 0x00485B0E
		// (set) Token: 0x0600C4BB RID: 50363 RVA: 0x00487916 File Offset: 0x00485B16
		public virtual Vector3i size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.sizeInternal;
			}
			set
			{
				this.sizeInternal = value;
			}
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600C4BC RID: 50364
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x0600C4BD RID: 50365
		public abstract int SerializedSize { get; }

		// Token: 0x0600C4BE RID: 50366 RVA: 0x0048791F File Offset: 0x00485B1F
		public void Use(Vector3i _startPos, Vector3i _size)
		{
			this.used = true;
			this.startPos = _startPos;
			this.size = _size;
		}

		// Token: 0x0600C4BF RID: 50367 RVA: 0x00487938 File Offset: 0x00485B38
		public virtual void Reset()
		{
			this.used = false;
			this.startPos = default(Vector3i);
			this.size = default(Vector3i);
		}

		// Token: 0x0600C4C0 RID: 50368 RVA: 0x0048796A File Offset: 0x00485B6A
		public void MarkUnused()
		{
			this.used = false;
		}

		// Token: 0x0600C4C1 RID: 50369 RVA: 0x00487973 File Offset: 0x00485B73
		public virtual void Read(PooledBinaryReader _br)
		{
			this.used = _br.ReadBoolean();
			this.startPos = StreamUtils.ReadVector3i(_br);
			this.size = StreamUtils.ReadVector3i(_br);
		}

		// Token: 0x0600C4C2 RID: 50370 RVA: 0x00487999 File Offset: 0x00485B99
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.Used);
			StreamUtils.Write(_bw, this.startPos);
			StreamUtils.Write(_bw, this.size);
		}

		// Token: 0x0600C4C3 RID: 50371 RVA: 0x004879C0 File Offset: 0x00485BC0
		public virtual void RotateY(bool _bLeft, Vector3i _prefabSize)
		{
			Vector3i size = this.size;
			Vector3i startPos = this.startPos;
			Vector3i vector3i = startPos + size;
			if (_bLeft)
			{
				startPos = new Vector3i(_prefabSize.z - startPos.z, startPos.y, startPos.x);
				vector3i = new Vector3i(_prefabSize.z - vector3i.z, vector3i.y, vector3i.x);
			}
			else
			{
				startPos = new Vector3i(startPos.z, startPos.y, _prefabSize.x - startPos.x);
				vector3i = new Vector3i(vector3i.z, vector3i.y, _prefabSize.x - vector3i.x);
			}
			if (startPos.x > vector3i.x)
			{
				MathUtils.Swap(ref startPos.x, ref vector3i.x);
			}
			if (startPos.z > vector3i.z)
			{
				MathUtils.Swap(ref startPos.z, ref vector3i.z);
			}
			this.startPos = startPos;
			MathUtils.Swap(ref size.x, ref size.z);
			this.size = size;
		}

		// Token: 0x0600C4C4 RID: 50372 RVA: 0x00487ACB File Offset: 0x00485CCB
		public void Move(Vector3i _moveVector)
		{
			this.startPos += _moveVector;
		}

		// Token: 0x0600C4C5 RID: 50373 RVA: 0x00487AE0 File Offset: 0x00485CE0
		public void Resize(int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest, bool _allowXBelow1 = false, bool _allowYBelow1 = false, bool _allowZBelow1 = false)
		{
			Vector3i vector3i = this.size;
			vector3i += new Vector3i(_dEast + _dWest, _dTop + _dBottom, _dNorth + _dSouth);
			this.startPos += new Vector3i(-_dWest, -_dBottom, -_dSouth);
			if (!_allowXBelow1 && vector3i.x < 2)
			{
				vector3i = new Vector3i(1, vector3i.y, vector3i.z);
			}
			if (!_allowYBelow1 && vector3i.y < 2)
			{
				vector3i = new Vector3i(vector3i.x, 1, vector3i.z);
			}
			if (!_allowZBelow1 && vector3i.z < 2)
			{
				vector3i = new Vector3i(vector3i.x, vector3i.y, 1);
			}
			this.size = vector3i;
		}

		// Token: 0x0600C4C6 RID: 50374
		public abstract PrefabVolumeAbs Clone();

		// Token: 0x0600C4C7 RID: 50375
		public abstract void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false);

		// Token: 0x0600C4C8 RID: 50376 RVA: 0x00487B94 File Offset: 0x00485D94
		public static PrefabVolumeAbs CreateByType(PrefabVolumeAbs.EVolumeType _volumeType)
		{
			PrefabVolumeAbs prefabVolumeAbs;
			switch (_volumeType)
			{
			case PrefabVolumeAbs.EVolumeType.Sleeper:
				prefabVolumeAbs = new PrefabSleeperVolume();
				break;
			case PrefabVolumeAbs.EVolumeType.Teleport:
				prefabVolumeAbs = new PrefabTeleportVolume();
				break;
			case PrefabVolumeAbs.EVolumeType.Info:
				prefabVolumeAbs = new PrefabInfoVolume();
				break;
			case PrefabVolumeAbs.EVolumeType.Wall:
				prefabVolumeAbs = new PrefabWallVolume();
				break;
			case PrefabVolumeAbs.EVolumeType.Trigger:
				prefabVolumeAbs = new PrefabTriggerVolume();
				break;
			case PrefabVolumeAbs.EVolumeType.Marker:
				prefabVolumeAbs = new Marker();
				break;
			default:
				throw new ArgumentOutOfRangeException("_volumeType", string.Format("Invalid volumeType {0}", _volumeType));
			}
			return prefabVolumeAbs;
		}

		// Token: 0x0600C4C9 RID: 50377 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}

		// Token: 0x040094B3 RID: 38067
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool used;

		// Token: 0x040094B4 RID: 38068
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i startPosInternal;

		// Token: 0x040094B5 RID: 38069
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i sizeInternal;

		// Token: 0x020018E7 RID: 6375
		public enum EVolumeType : byte
		{
			// Token: 0x040094B7 RID: 38071
			Sleeper,
			// Token: 0x040094B8 RID: 38072
			Teleport,
			// Token: 0x040094B9 RID: 38073
			Info,
			// Token: 0x040094BA RID: 38074
			Wall,
			// Token: 0x040094BB RID: 38075
			Trigger,
			// Token: 0x040094BC RID: 38076
			Marker
		}
	}
}
