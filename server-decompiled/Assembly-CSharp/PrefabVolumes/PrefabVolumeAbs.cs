using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x020018BA RID: 6330
	public abstract class PrefabVolumeAbs
	{
		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x0600C35F RID: 50015 RVA: 0x004833AD File Offset: 0x004815AD
		public bool Used
		{
			get
			{
				return this.used;
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x0600C360 RID: 50016 RVA: 0x004833B5 File Offset: 0x004815B5
		// (set) Token: 0x0600C361 RID: 50017 RVA: 0x004833BD File Offset: 0x004815BD
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

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x0600C362 RID: 50018 RVA: 0x004833C6 File Offset: 0x004815C6
		// (set) Token: 0x0600C363 RID: 50019 RVA: 0x004833CE File Offset: 0x004815CE
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

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x0600C364 RID: 50020
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x0600C365 RID: 50021
		public abstract int SerializedSize { get; }

		// Token: 0x0600C366 RID: 50022 RVA: 0x004833D7 File Offset: 0x004815D7
		public void Use(Vector3i _startPos, Vector3i _size)
		{
			this.used = true;
			this.startPos = _startPos;
			this.size = _size;
		}

		// Token: 0x0600C367 RID: 50023 RVA: 0x004833F0 File Offset: 0x004815F0
		public virtual void Reset()
		{
			this.used = false;
			this.startPos = default(Vector3i);
			this.size = default(Vector3i);
		}

		// Token: 0x0600C368 RID: 50024 RVA: 0x00483422 File Offset: 0x00481622
		public void MarkUnused()
		{
			this.used = false;
		}

		// Token: 0x0600C369 RID: 50025 RVA: 0x0048342B File Offset: 0x0048162B
		public virtual void Read(PooledBinaryReader _br)
		{
			this.used = _br.ReadBoolean();
			this.startPos = StreamUtils.ReadVector3i(_br);
			this.size = StreamUtils.ReadVector3i(_br);
		}

		// Token: 0x0600C36A RID: 50026 RVA: 0x00483451 File Offset: 0x00481651
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.Used);
			StreamUtils.Write(_bw, this.startPos);
			StreamUtils.Write(_bw, this.size);
		}

		// Token: 0x0600C36B RID: 50027 RVA: 0x00483478 File Offset: 0x00481678
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

		// Token: 0x0600C36C RID: 50028 RVA: 0x00483583 File Offset: 0x00481783
		public void Move(Vector3i _moveVector)
		{
			this.startPos += _moveVector;
		}

		// Token: 0x0600C36D RID: 50029 RVA: 0x00483598 File Offset: 0x00481798
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

		// Token: 0x0600C36E RID: 50030
		public abstract PrefabVolumeAbs Clone();

		// Token: 0x0600C36F RID: 50031
		public abstract void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false);

		// Token: 0x0600C370 RID: 50032 RVA: 0x0048364C File Offset: 0x0048184C
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

		// Token: 0x0600C371 RID: 50033 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}

		// Token: 0x040093ED RID: 37869
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool used;

		// Token: 0x040093EE RID: 37870
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i startPosInternal;

		// Token: 0x040093EF RID: 37871
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i sizeInternal;

		// Token: 0x020018BB RID: 6331
		public enum EVolumeType : byte
		{
			// Token: 0x040093F1 RID: 37873
			Sleeper,
			// Token: 0x040093F2 RID: 37874
			Teleport,
			// Token: 0x040093F3 RID: 37875
			Info,
			// Token: 0x040093F4 RID: 37876
			Wall,
			// Token: 0x040093F5 RID: 37877
			Trigger,
			// Token: 0x040093F6 RID: 37878
			Marker
		}
	}
}
