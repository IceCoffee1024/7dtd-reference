using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabVolumes
{
	// Token: 0x0200189C RID: 6300
	public class Marker : PrefabVolumeAbs<Marker>
	{
		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x0600C214 RID: 49684 RVA: 0x0047DB26 File Offset: 0x0047BD26
		public bool PartDirty
		{
			get
			{
				return this.partDirty;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600C215 RID: 49685 RVA: 0x0047DB2E File Offset: 0x0047BD2E
		// (set) Token: 0x0600C216 RID: 49686 RVA: 0x0047DB36 File Offset: 0x0047BD36
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

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600C217 RID: 49687 RVA: 0x0047DB60 File Offset: 0x0047BD60
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

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600C218 RID: 49688 RVA: 0x0047DBE4 File Offset: 0x0047BDE4
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

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x0600C219 RID: 49689 RVA: 0x0047D289 File Offset: 0x0047B489
		// (set) Token: 0x0600C21A RID: 49690 RVA: 0x0047D291 File Offset: 0x0047B491
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

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x0600C21B RID: 49691 RVA: 0x0047D29A File Offset: 0x0047B49A
		// (set) Token: 0x0600C21C RID: 49692 RVA: 0x0047DC06 File Offset: 0x0047BE06
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

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600C21D RID: 49693 RVA: 0x00080065 File Offset: 0x0007E265
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Marker;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600C21E RID: 49694 RVA: 0x0047DC24 File Offset: 0x0047BE24
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

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600C21F RID: 49695 RVA: 0x0047DCD0 File Offset: 0x0047BED0
		// (set) Token: 0x0600C220 RID: 49696 RVA: 0x0047DCD8 File Offset: 0x0047BED8
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

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x0600C221 RID: 49697 RVA: 0x0047DCF1 File Offset: 0x0047BEF1
		// (set) Token: 0x0600C222 RID: 49698 RVA: 0x0047DCF9 File Offset: 0x0047BEF9
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

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x0600C223 RID: 49699 RVA: 0x0047DD02 File Offset: 0x0047BF02
		// (set) Token: 0x0600C224 RID: 49700 RVA: 0x0047DD0A File Offset: 0x0047BF0A
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

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x0600C225 RID: 49701 RVA: 0x0047DD28 File Offset: 0x0047BF28
		// (set) Token: 0x0600C226 RID: 49702 RVA: 0x0047DD30 File Offset: 0x0047BF30
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

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x0600C227 RID: 49703 RVA: 0x0047DD49 File Offset: 0x0047BF49
		// (set) Token: 0x0600C228 RID: 49704 RVA: 0x0047DD51 File Offset: 0x0047BF51
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

		// Token: 0x0600C229 RID: 49705 RVA: 0x0047DD70 File Offset: 0x0047BF70
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

		// Token: 0x0600C22A RID: 49706 RVA: 0x0047DDD4 File Offset: 0x0047BFD4
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

		// Token: 0x0600C22B RID: 49707 RVA: 0x0047DE3C File Offset: 0x0047C03C
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

		// Token: 0x0600C22C RID: 49708 RVA: 0x0047DEA0 File Offset: 0x0047C0A0
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

		// Token: 0x040092D8 RID: 37592
		[PublicizedFrom(EAccessModifier.Private)]
		public Marker.MarkerTypes markerType;

		// Token: 0x040092D9 RID: 37593
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Poi> tags;

		// Token: 0x040092DA RID: 37594
		[PublicizedFrom(EAccessModifier.Private)]
		public int groupId = -1;

		// Token: 0x040092DB RID: 37595
		[PublicizedFrom(EAccessModifier.Private)]
		public string groupName;

		// Token: 0x040092DC RID: 37596
		[PublicizedFrom(EAccessModifier.Private)]
		public Color color;

		// Token: 0x040092DD RID: 37597
		[PublicizedFrom(EAccessModifier.Private)]
		public string partToSpawn;

		// Token: 0x040092DE RID: 37598
		[PublicizedFrom(EAccessModifier.Private)]
		public byte rotations;

		// Token: 0x040092DF RID: 37599
		[PublicizedFrom(EAccessModifier.Private)]
		public float partChanceToSpawn = 1f;

		// Token: 0x040092E0 RID: 37600
		[PublicizedFrom(EAccessModifier.Private)]
		public bool partDirty = true;

		// Token: 0x040092E1 RID: 37601
		public static readonly List<Vector3i> MarkerSizes = new List<Vector3i>
		{
			Vector3i.one,
			new Vector3i(25, 0, 25),
			new Vector3i(42, 0, 42),
			new Vector3i(60, 0, 60),
			new Vector3i(100, 0, 100)
		};

		// Token: 0x0200189D RID: 6301
		public enum MarkerTypes : byte
		{
			// Token: 0x040092E3 RID: 37603
			None,
			// Token: 0x040092E4 RID: 37604
			POISpawn,
			// Token: 0x040092E5 RID: 37605
			RoadExit,
			// Token: 0x040092E6 RID: 37606
			PartSpawn
		}

		// Token: 0x0200189E RID: 6302
		public enum MarkerSize : byte
		{
			// Token: 0x040092E8 RID: 37608
			One,
			// Token: 0x040092E9 RID: 37609
			ExtraSmall,
			// Token: 0x040092EA RID: 37610
			Small,
			// Token: 0x040092EB RID: 37611
			Medium,
			// Token: 0x040092EC RID: 37612
			Large,
			// Token: 0x040092ED RID: 37613
			Custom
		}
	}
}
