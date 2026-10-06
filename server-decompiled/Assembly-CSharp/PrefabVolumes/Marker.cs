using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabVolumes
{
	// Token: 0x020018EE RID: 6382
	public class Marker : PrefabVolumeAbs<Marker>
	{
		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x0600C4F3 RID: 50419 RVA: 0x0048819A File Offset: 0x0048639A
		public bool PartDirty
		{
			get
			{
				return this.partDirty;
			}
		}

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x0600C4F4 RID: 50420 RVA: 0x004881A2 File Offset: 0x004863A2
		// (set) Token: 0x0600C4F5 RID: 50421 RVA: 0x004881AA File Offset: 0x004863AA
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

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x0600C4F6 RID: 50422 RVA: 0x004881D4 File Offset: 0x004863D4
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

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x0600C4F7 RID: 50423 RVA: 0x00488258 File Offset: 0x00486458
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

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x0600C4F8 RID: 50424 RVA: 0x004878FD File Offset: 0x00485AFD
		// (set) Token: 0x0600C4F9 RID: 50425 RVA: 0x00487905 File Offset: 0x00485B05
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

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x0600C4FA RID: 50426 RVA: 0x0048790E File Offset: 0x00485B0E
		// (set) Token: 0x0600C4FB RID: 50427 RVA: 0x0048827A File Offset: 0x0048647A
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

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x0600C4FC RID: 50428 RVA: 0x000809CD File Offset: 0x0007EBCD
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Marker;
			}
		}

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x0600C4FD RID: 50429 RVA: 0x00488298 File Offset: 0x00486498
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

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x0600C4FE RID: 50430 RVA: 0x00488344 File Offset: 0x00486544
		// (set) Token: 0x0600C4FF RID: 50431 RVA: 0x0048834C File Offset: 0x0048654C
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

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x0600C500 RID: 50432 RVA: 0x00488365 File Offset: 0x00486565
		// (set) Token: 0x0600C501 RID: 50433 RVA: 0x0048836D File Offset: 0x0048656D
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

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x0600C502 RID: 50434 RVA: 0x00488376 File Offset: 0x00486576
		// (set) Token: 0x0600C503 RID: 50435 RVA: 0x0048837E File Offset: 0x0048657E
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

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x0600C504 RID: 50436 RVA: 0x0048839C File Offset: 0x0048659C
		// (set) Token: 0x0600C505 RID: 50437 RVA: 0x004883A4 File Offset: 0x004865A4
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

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x0600C506 RID: 50438 RVA: 0x004883BD File Offset: 0x004865BD
		// (set) Token: 0x0600C507 RID: 50439 RVA: 0x004883C5 File Offset: 0x004865C5
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

		// Token: 0x0600C508 RID: 50440 RVA: 0x004883E4 File Offset: 0x004865E4
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

		// Token: 0x0600C509 RID: 50441 RVA: 0x00488448 File Offset: 0x00486648
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

		// Token: 0x0600C50A RID: 50442 RVA: 0x004884B0 File Offset: 0x004866B0
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

		// Token: 0x0600C50B RID: 50443 RVA: 0x00488514 File Offset: 0x00486714
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

		// Token: 0x040094C7 RID: 38087
		[PublicizedFrom(EAccessModifier.Private)]
		public Marker.MarkerTypes markerType;

		// Token: 0x040094C8 RID: 38088
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Poi> tags;

		// Token: 0x040094C9 RID: 38089
		[PublicizedFrom(EAccessModifier.Private)]
		public int groupId = -1;

		// Token: 0x040094CA RID: 38090
		[PublicizedFrom(EAccessModifier.Private)]
		public string groupName;

		// Token: 0x040094CB RID: 38091
		[PublicizedFrom(EAccessModifier.Private)]
		public Color color;

		// Token: 0x040094CC RID: 38092
		[PublicizedFrom(EAccessModifier.Private)]
		public string partToSpawn;

		// Token: 0x040094CD RID: 38093
		[PublicizedFrom(EAccessModifier.Private)]
		public byte rotations;

		// Token: 0x040094CE RID: 38094
		[PublicizedFrom(EAccessModifier.Private)]
		public float partChanceToSpawn = 1f;

		// Token: 0x040094CF RID: 38095
		[PublicizedFrom(EAccessModifier.Private)]
		public bool partDirty = true;

		// Token: 0x040094D0 RID: 38096
		public static readonly List<Vector3i> MarkerSizes = new List<Vector3i>
		{
			Vector3i.one,
			new Vector3i(25, 0, 25),
			new Vector3i(42, 0, 42),
			new Vector3i(60, 0, 60),
			new Vector3i(100, 0, 100)
		};

		// Token: 0x020018EF RID: 6383
		public enum MarkerTypes : byte
		{
			// Token: 0x040094D2 RID: 38098
			None,
			// Token: 0x040094D3 RID: 38099
			POISpawn,
			// Token: 0x040094D4 RID: 38100
			RoadExit,
			// Token: 0x040094D5 RID: 38101
			PartSpawn
		}

		// Token: 0x020018F0 RID: 6384
		public enum MarkerSize : byte
		{
			// Token: 0x040094D7 RID: 38103
			One,
			// Token: 0x040094D8 RID: 38104
			ExtraSmall,
			// Token: 0x040094D9 RID: 38105
			Small,
			// Token: 0x040094DA RID: 38106
			Medium,
			// Token: 0x040094DB RID: 38107
			Large,
			// Token: 0x040094DC RID: 38108
			Custom
		}
	}
}
