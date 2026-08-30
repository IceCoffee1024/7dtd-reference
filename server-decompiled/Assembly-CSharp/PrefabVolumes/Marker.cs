using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabVolumes
{
	// Token: 0x020018C2 RID: 6338
	public class Marker : PrefabVolumeAbs<Marker>
	{
		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x0600C39B RID: 50075 RVA: 0x00483C52 File Offset: 0x00481E52
		public bool PartDirty
		{
			get
			{
				return this.partDirty;
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x0600C39C RID: 50076 RVA: 0x00483C5A File Offset: 0x00481E5A
		// (set) Token: 0x0600C39D RID: 50077 RVA: 0x00483C62 File Offset: 0x00481E62
		public string GroupName
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.groupName;
			}
			set
			{
				if (this.groupName != value)
				{
					this.color = default(Color);
					this.groupId = -1;
					this.groupName = value;
				}
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x0600C39E RID: 50078 RVA: 0x00483C8C File Offset: 0x00481E8C
		public Color GroupColor
		{
			get
			{
				if (this.color == default(Color))
				{
					GameRandom tempGameRandom = GameRandomManager.Instance.GetTempGameRandom(this.GroupId);
					this.color = new Color32((byte)tempGameRandom.RandomRange(0, 256), (byte)tempGameRandom.RandomRange(0, 256), (byte)tempGameRandom.RandomRange(0, 256), (this.MarkerType == Marker.MarkerTypes.PartSpawn) ? 32 : 128);
				}
				return this.color;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x0600C39F RID: 50079 RVA: 0x00483D10 File Offset: 0x00481F10
		public int GroupId
		{
			get
			{
				if (this.groupId == -1)
				{
					this.groupId = this.GroupName.GetHashCode();
				}
				return this.groupId;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x0600C3A0 RID: 50080 RVA: 0x004833B5 File Offset: 0x004815B5
		// (set) Token: 0x0600C3A1 RID: 50081 RVA: 0x004833BD File Offset: 0x004815BD
		public override Vector3i startPos
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

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x0600C3A2 RID: 50082 RVA: 0x004833C6 File Offset: 0x004815C6
		// (set) Token: 0x0600C3A3 RID: 50083 RVA: 0x00483D32 File Offset: 0x00481F32
		public override Vector3i size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.sizeInternal;
			}
			set
			{
				if (this.sizeInternal != value)
				{
					this.sizeInternal = value;
					this.partDirty = true;
				}
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x0600C3A4 RID: 50084 RVA: 0x00080719 File Offset: 0x0007E919
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Marker;
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600C3A5 RID: 50085 RVA: 0x00483D50 File Offset: 0x00481F50
		public override int SerializedSize
		{
			get
			{
				int num = 26;
				string text = this.GroupName;
				int num2 = num + (1 + ((text != null) ? new int?(text.Length) : null)).GetValueOrDefault() + 100;
				string text2 = this.PartToSpawn;
				return num2 + (1 + ((text2 != null) ? new int?(text2.Length) : null)).GetValueOrDefault() + 1 + 4;
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600C3A6 RID: 50086 RVA: 0x00483DFC File Offset: 0x00481FFC
		// (set) Token: 0x0600C3A7 RID: 50087 RVA: 0x00483E04 File Offset: 0x00482004
		public Marker.MarkerTypes MarkerType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.markerType;
			}
			set
			{
				if (this.markerType != value)
				{
					this.markerType = value;
					this.partDirty = true;
				}
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600C3A8 RID: 50088 RVA: 0x00483E1D File Offset: 0x0048201D
		// (set) Token: 0x0600C3A9 RID: 50089 RVA: 0x00483E25 File Offset: 0x00482025
		public FastTags<TagGroup.Poi> Tags
		{
			get
			{
				return this.tags;
			}
			set
			{
				this.tags = value;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x0600C3AA RID: 50090 RVA: 0x00483E2E File Offset: 0x0048202E
		// (set) Token: 0x0600C3AB RID: 50091 RVA: 0x00483E36 File Offset: 0x00482036
		public string PartToSpawn
		{
			get
			{
				return this.partToSpawn;
			}
			set
			{
				if (this.partToSpawn != value)
				{
					this.partToSpawn = value;
					this.partDirty = true;
				}
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600C3AC RID: 50092 RVA: 0x00483E54 File Offset: 0x00482054
		// (set) Token: 0x0600C3AD RID: 50093 RVA: 0x00483E5C File Offset: 0x0048205C
		public byte Rotations
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.rotations;
			}
			set
			{
				if (this.rotations != value)
				{
					this.rotations = value;
					this.partDirty = true;
				}
			}
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x0600C3AE RID: 50094 RVA: 0x00483E75 File Offset: 0x00482075
		// (set) Token: 0x0600C3AF RID: 50095 RVA: 0x00483E7D File Offset: 0x0048207D
		public float PartChanceToSpawn
		{
			get
			{
				return this.partChanceToSpawn;
			}
			set
			{
				if (!Mathf.Approximately((float)this.rotations, value))
				{
					this.partChanceToSpawn = value;
					this.partDirty = true;
				}
			}
		}

		// Token: 0x0600C3B0 RID: 50096 RVA: 0x00483E9C File Offset: 0x0048209C
		public override void Reset()
		{
			base.Reset();
			this.markerType = Marker.MarkerTypes.None;
			this.tags = FastTags<TagGroup.Poi>.none;
			this.groupId = -1;
			this.groupName = "new";
			this.color = default(Color);
			this.partToSpawn = null;
			this.rotations = 0;
			this.partChanceToSpawn = 0f;
			this.partDirty = false;
		}

		// Token: 0x0600C3B1 RID: 50097 RVA: 0x00483F00 File Offset: 0x00482100
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<Marker> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			Marker marker = (Marker)_target;
			marker.MarkerType = this.MarkerType;
			marker.GroupName = this.GroupName;
			marker.Tags = this.Tags;
			marker.PartToSpawn = this.PartToSpawn;
			marker.Rotations = this.Rotations;
			marker.PartChanceToSpawn = this.PartChanceToSpawn;
		}

		// Token: 0x0600C3B2 RID: 50098 RVA: 0x00483F68 File Offset: 0x00482168
		public override void Read(PooledBinaryReader _br)
		{
			base.Read(_br);
			this.MarkerType = (Marker.MarkerTypes)_br.ReadByte();
			this.GroupName = StreamUtils.ReadString(_br);
			this.Tags = FastTags<TagGroup.Poi>.Parse(_br.ReadString());
			this.PartToSpawn = StreamUtils.ReadString(_br);
			this.Rotations = _br.ReadByte();
			this.PartChanceToSpawn = _br.ReadSingle();
		}

		// Token: 0x0600C3B3 RID: 50099 RVA: 0x00483FCC File Offset: 0x004821CC
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write((byte)this.MarkerType);
			StreamUtils.Write(_bw, this.GroupName);
			_bw.Write(this.Tags.ToString());
			StreamUtils.Write(_bw, this.PartToSpawn);
			_bw.Write(this.Rotations);
			_bw.Write(this.PartChanceToSpawn);
		}

		// Token: 0x04009401 RID: 37889
		[PublicizedFrom(EAccessModifier.Private)]
		public Marker.MarkerTypes markerType;

		// Token: 0x04009402 RID: 37890
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Poi> tags;

		// Token: 0x04009403 RID: 37891
		[PublicizedFrom(EAccessModifier.Private)]
		public int groupId = -1;

		// Token: 0x04009404 RID: 37892
		[PublicizedFrom(EAccessModifier.Private)]
		public string groupName;

		// Token: 0x04009405 RID: 37893
		[PublicizedFrom(EAccessModifier.Private)]
		public Color color;

		// Token: 0x04009406 RID: 37894
		[PublicizedFrom(EAccessModifier.Private)]
		public string partToSpawn;

		// Token: 0x04009407 RID: 37895
		[PublicizedFrom(EAccessModifier.Private)]
		public byte rotations;

		// Token: 0x04009408 RID: 37896
		[PublicizedFrom(EAccessModifier.Private)]
		public float partChanceToSpawn = 1f;

		// Token: 0x04009409 RID: 37897
		[PublicizedFrom(EAccessModifier.Private)]
		public bool partDirty = true;

		// Token: 0x0400940A RID: 37898
		public static readonly List<Vector3i> MarkerSizes = new List<Vector3i>
		{
			Vector3i.one,
			new Vector3i(25, 0, 25),
			new Vector3i(42, 0, 42),
			new Vector3i(60, 0, 60),
			new Vector3i(100, 0, 100)
		};

		// Token: 0x020018C3 RID: 6339
		public enum MarkerTypes : byte
		{
			// Token: 0x0400940C RID: 37900
			None,
			// Token: 0x0400940D RID: 37901
			POISpawn,
			// Token: 0x0400940E RID: 37902
			RoadExit,
			// Token: 0x0400940F RID: 37903
			PartSpawn
		}

		// Token: 0x020018C4 RID: 6340
		public enum MarkerSize : byte
		{
			// Token: 0x04009411 RID: 37905
			One,
			// Token: 0x04009412 RID: 37906
			ExtraSmall,
			// Token: 0x04009413 RID: 37907
			Small,
			// Token: 0x04009414 RID: 37908
			Medium,
			// Token: 0x04009415 RID: 37909
			Large,
			// Token: 0x04009416 RID: 37910
			Custom
		}
	}
}
