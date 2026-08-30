using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001894 RID: 6292
	public abstract class PrefabVolumeAbs
	{
		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x0600C1D8 RID: 49624 RVA: 0x0047D281 File Offset: 0x0047B481
		public bool Used
		{
			get
			{
				return this.used;
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x0600C1D9 RID: 49625 RVA: 0x0047D289 File Offset: 0x0047B489
		// (set) Token: 0x0600C1DA RID: 49626 RVA: 0x0047D291 File Offset: 0x0047B491
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

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x0600C1DB RID: 49627 RVA: 0x0047D29A File Offset: 0x0047B49A
		// (set) Token: 0x0600C1DC RID: 49628 RVA: 0x0047D2A2 File Offset: 0x0047B4A2
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

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x0600C1DD RID: 49629
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x0600C1DE RID: 49630
		public abstract int SerializedSize { get; }

		// Token: 0x0600C1DF RID: 49631 RVA: 0x0047D2AB File Offset: 0x0047B4AB
		public void Use(Vector3i _startPos, Vector3i _size)
		{
			this.used = true;
			this.startPos = _startPos;
			this.size = _size;
		}

		// Token: 0x0600C1E0 RID: 49632 RVA: 0x0047D2C4 File Offset: 0x0047B4C4
		public virtual void Reset()
		{
			this.used = false;
			this.startPos = default(Vector3i);
			this.size = default(Vector3i);
		}

		// Token: 0x0600C1E1 RID: 49633 RVA: 0x0047D2F6 File Offset: 0x0047B4F6
		public void MarkUnused()
		{
			this.used = false;
		}

		// Token: 0x0600C1E2 RID: 49634 RVA: 0x0047D2FF File Offset: 0x0047B4FF
		public virtual void Read(PooledBinaryReader _br)
		{
			this.used = _br.ReadBoolean();
			this.startPos = StreamUtils.ReadVector3i(_br);
			this.size = StreamUtils.ReadVector3i(_br);
		}

		// Token: 0x0600C1E3 RID: 49635 RVA: 0x0047D325 File Offset: 0x0047B525
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.Used);
			StreamUtils.Write(_bw, this.startPos);
			StreamUtils.Write(_bw, this.size);
		}

		// Token: 0x0600C1E4 RID: 49636 RVA: 0x0047D34C File Offset: 0x0047B54C
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

		// Token: 0x0600C1E5 RID: 49637 RVA: 0x0047D457 File Offset: 0x0047B657
		public void Move(Vector3i _moveVector)
		{
			this.startPos += _moveVector;
		}

		// Token: 0x0600C1E6 RID: 49638 RVA: 0x0047D46C File Offset: 0x0047B66C
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

		// Token: 0x0600C1E7 RID: 49639
		public abstract PrefabVolumeAbs Clone();

		// Token: 0x0600C1E8 RID: 49640
		public abstract void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false);

		// Token: 0x0600C1E9 RID: 49641 RVA: 0x0047D520 File Offset: 0x0047B720
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

		// Token: 0x0600C1EA RID: 49642 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}

		// Token: 0x040092C4 RID: 37572
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool used;

		// Token: 0x040092C5 RID: 37573
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i startPosInternal;

		// Token: 0x040092C6 RID: 37574
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i sizeInternal;

		// Token: 0x02001895 RID: 6293
		public enum EVolumeType : byte
		{
			// Token: 0x040092C8 RID: 37576
			Sleeper,
			// Token: 0x040092C9 RID: 37577
			Teleport,
			// Token: 0x040092CA RID: 37578
			Info,
			// Token: 0x040092CB RID: 37579
			Wall,
			// Token: 0x040092CC RID: 37580
			Trigger,
			// Token: 0x040092CD RID: 37581
			Marker
		}
	}
}
