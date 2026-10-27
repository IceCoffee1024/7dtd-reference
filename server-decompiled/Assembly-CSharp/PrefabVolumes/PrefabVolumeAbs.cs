using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001917 RID: 6423
	public abstract class PrefabVolumeAbs
	{
		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x0600C5CB RID: 50635 RVA: 0x0048B2A1 File Offset: 0x004894A1
		public bool Used
		{
			get
			{
				return this.used;
			}
		}

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x0600C5CC RID: 50636 RVA: 0x0048B2A9 File Offset: 0x004894A9
		// (set) Token: 0x0600C5CD RID: 50637 RVA: 0x0048B2B1 File Offset: 0x004894B1
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

		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x0600C5CE RID: 50638 RVA: 0x0048B2BA File Offset: 0x004894BA
		// (set) Token: 0x0600C5CF RID: 50639 RVA: 0x0048B2C2 File Offset: 0x004894C2
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

		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x0600C5D0 RID: 50640
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x0600C5D1 RID: 50641
		public abstract int SerializedSize { get; }

		// Token: 0x0600C5D2 RID: 50642 RVA: 0x0048B2CB File Offset: 0x004894CB
		public void Use(Vector3i _startPos, Vector3i _size)
		{
			this.used = true;
			this.startPos = _startPos;
			this.size = _size;
		}

		// Token: 0x0600C5D3 RID: 50643 RVA: 0x0048B2E4 File Offset: 0x004894E4
		public virtual void Reset()
		{
			this.used = false;
			this.startPos = default(Vector3i);
			this.size = default(Vector3i);
		}

		// Token: 0x0600C5D4 RID: 50644 RVA: 0x0048B316 File Offset: 0x00489516
		public void MarkUnused()
		{
			this.used = false;
		}

		// Token: 0x0600C5D5 RID: 50645 RVA: 0x0048B31F File Offset: 0x0048951F
		public virtual void Read(PooledBinaryReader _br)
		{
			this.used = _br.ReadBoolean();
			this.startPos = StreamUtils.ReadVector3i(_br);
			this.size = StreamUtils.ReadVector3i(_br);
		}

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x0048B345 File Offset: 0x00489545
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.Used);
			StreamUtils.Write(_bw, this.startPos);
			StreamUtils.Write(_bw, this.size);
		}

		// Token: 0x0600C5D7 RID: 50647 RVA: 0x0048B36C File Offset: 0x0048956C
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

		// Token: 0x0600C5D8 RID: 50648 RVA: 0x0048B477 File Offset: 0x00489677
		public void Move(Vector3i _moveVector)
		{
			this.startPos += _moveVector;
		}

		// Token: 0x0600C5D9 RID: 50649 RVA: 0x0048B48C File Offset: 0x0048968C
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

		// Token: 0x0600C5DA RID: 50650
		public abstract PrefabVolumeAbs Clone();

		// Token: 0x0600C5DB RID: 50651
		public abstract void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false);

		// Token: 0x0600C5DC RID: 50652 RVA: 0x0048B540 File Offset: 0x00489740
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

		// Token: 0x0600C5DD RID: 50653 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}

		// Token: 0x0400959C RID: 38300
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool used;

		// Token: 0x0400959D RID: 38301
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i startPosInternal;

		// Token: 0x0400959E RID: 38302
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i sizeInternal;

		// Token: 0x02001918 RID: 6424
		public enum EVolumeType : byte
		{
			// Token: 0x040095A0 RID: 38304
			Sleeper,
			// Token: 0x040095A1 RID: 38305
			Teleport,
			// Token: 0x040095A2 RID: 38306
			Info,
			// Token: 0x040095A3 RID: 38307
			Wall,
			// Token: 0x040095A4 RID: 38308
			Trigger,
			// Token: 0x040095A5 RID: 38309
			Marker
		}
	}
}
