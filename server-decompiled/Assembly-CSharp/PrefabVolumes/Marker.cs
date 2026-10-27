using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabVolumes
{
	// Token: 0x0200191F RID: 6431
	public class Marker : PrefabVolumeAbs<Marker>
	{
		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600C607 RID: 50695 RVA: 0x0048BB46 File Offset: 0x00489D46
		public bool PartDirty
		{
			get
			{
				return this.partDirty;
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x0600C608 RID: 50696 RVA: 0x0048BB4E File Offset: 0x00489D4E
		// (set) Token: 0x0600C609 RID: 50697 RVA: 0x0048BB56 File Offset: 0x00489D56
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

		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x0600C60A RID: 50698 RVA: 0x0048BB80 File Offset: 0x00489D80
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

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x0600C60B RID: 50699 RVA: 0x0048BC04 File Offset: 0x00489E04
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

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600C60C RID: 50700 RVA: 0x0048B2A9 File Offset: 0x004894A9
		// (set) Token: 0x0600C60D RID: 50701 RVA: 0x0048B2B1 File Offset: 0x004894B1
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

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600C60E RID: 50702 RVA: 0x0048B2BA File Offset: 0x004894BA
		// (set) Token: 0x0600C60F RID: 50703 RVA: 0x0048BC26 File Offset: 0x00489E26
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

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600C610 RID: 50704 RVA: 0x00080BF2 File Offset: 0x0007EDF2
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Marker;
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600C611 RID: 50705 RVA: 0x0048BC44 File Offset: 0x00489E44
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

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600C612 RID: 50706 RVA: 0x0048BCF0 File Offset: 0x00489EF0
		// (set) Token: 0x0600C613 RID: 50707 RVA: 0x0048BCF8 File Offset: 0x00489EF8
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

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600C614 RID: 50708 RVA: 0x0048BD11 File Offset: 0x00489F11
		// (set) Token: 0x0600C615 RID: 50709 RVA: 0x0048BD19 File Offset: 0x00489F19
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

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600C616 RID: 50710 RVA: 0x0048BD22 File Offset: 0x00489F22
		// (set) Token: 0x0600C617 RID: 50711 RVA: 0x0048BD2A File Offset: 0x00489F2A
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

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600C618 RID: 50712 RVA: 0x0048BD48 File Offset: 0x00489F48
		// (set) Token: 0x0600C619 RID: 50713 RVA: 0x0048BD50 File Offset: 0x00489F50
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

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x0600C61A RID: 50714 RVA: 0x0048BD69 File Offset: 0x00489F69
		// (set) Token: 0x0600C61B RID: 50715 RVA: 0x0048BD71 File Offset: 0x00489F71
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

		// Token: 0x0600C61C RID: 50716 RVA: 0x0048BD90 File Offset: 0x00489F90
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

		// Token: 0x0600C61D RID: 50717 RVA: 0x0048BDF4 File Offset: 0x00489FF4
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

		// Token: 0x0600C61E RID: 50718 RVA: 0x0048BE5C File Offset: 0x0048A05C
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

		// Token: 0x0600C61F RID: 50719 RVA: 0x0048BEC0 File Offset: 0x0048A0C0
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

		// Token: 0x040095B0 RID: 38320
		[PublicizedFrom(EAccessModifier.Private)]
		public Marker.MarkerTypes markerType;

		// Token: 0x040095B1 RID: 38321
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Poi> tags;

		// Token: 0x040095B2 RID: 38322
		[PublicizedFrom(EAccessModifier.Private)]
		public int groupId = -1;

		// Token: 0x040095B3 RID: 38323
		[PublicizedFrom(EAccessModifier.Private)]
		public string groupName;

		// Token: 0x040095B4 RID: 38324
		[PublicizedFrom(EAccessModifier.Private)]
		public Color color;

		// Token: 0x040095B5 RID: 38325
		[PublicizedFrom(EAccessModifier.Private)]
		public string partToSpawn;

		// Token: 0x040095B6 RID: 38326
		[PublicizedFrom(EAccessModifier.Private)]
		public byte rotations;

		// Token: 0x040095B7 RID: 38327
		[PublicizedFrom(EAccessModifier.Private)]
		public float partChanceToSpawn = 1f;

		// Token: 0x040095B8 RID: 38328
		[PublicizedFrom(EAccessModifier.Private)]
		public bool partDirty = true;

		// Token: 0x040095B9 RID: 38329
		public static readonly List<Vector3i> MarkerSizes = new List<Vector3i>
		{
			Vector3i.one,
			new Vector3i(25, 0, 25),
			new Vector3i(42, 0, 42),
			new Vector3i(60, 0, 60),
			new Vector3i(100, 0, 100)
		};

		// Token: 0x02001920 RID: 6432
		public enum MarkerTypes : byte
		{
			// Token: 0x040095BB RID: 38331
			None,
			// Token: 0x040095BC RID: 38332
			POISpawn,
			// Token: 0x040095BD RID: 38333
			RoadExit,
			// Token: 0x040095BE RID: 38334
			PartSpawn
		}

		// Token: 0x02001921 RID: 6433
		public enum MarkerSize : byte
		{
			// Token: 0x040095C0 RID: 38336
			One,
			// Token: 0x040095C1 RID: 38337
			ExtraSmall,
			// Token: 0x040095C2 RID: 38338
			Small,
			// Token: 0x040095C3 RID: 38339
			Medium,
			// Token: 0x040095C4 RID: 38340
			Large,
			// Token: 0x040095C5 RID: 38341
			Custom
		}
	}
}
