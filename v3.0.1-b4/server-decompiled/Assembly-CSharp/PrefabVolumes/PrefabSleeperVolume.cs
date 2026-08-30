using System;
using System.Collections.Generic;

namespace PrefabVolumes
{
	// Token: 0x02001897 RID: 6295
	public class PrefabSleeperVolume : PrefabVolumeAbs<PrefabSleeperVolume>
	{
		// Token: 0x0600C1F0 RID: 49648 RVA: 0x0047D658 File Offset: 0x0047B858
		public override void Reset()
		{
			base.Reset();
			this.groupName = "GroupGenericZombie";
			this.isPriority = false;
			this.isQuestExclude = false;
			this.spawnCountMin = 5;
			this.spawnCountMax = 6;
			this.groupId = 0;
			this.flags = 0;
			this.minScript = null;
		}

		// Token: 0x0600C1F1 RID: 49649 RVA: 0x0047D6A8 File Offset: 0x0047B8A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<PrefabSleeperVolume> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			PrefabSleeperVolume prefabSleeperVolume = (PrefabSleeperVolume)_target;
			prefabSleeperVolume.groupId = this.groupId;
			prefabSleeperVolume.groupName = this.groupName;
			prefabSleeperVolume.isPriority = this.isPriority;
			prefabSleeperVolume.isQuestExclude = this.isQuestExclude;
			prefabSleeperVolume.spawnCountMin = this.spawnCountMin;
			prefabSleeperVolume.spawnCountMax = this.spawnCountMax;
			prefabSleeperVolume.triggeredByIndices.AddRange(this.triggeredByIndices);
			prefabSleeperVolume.flags = this.flags;
			prefabSleeperVolume.minScript = this.minScript;
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x0600C1F2 RID: 49650 RVA: 0x00010E62 File Offset: 0x0000F062
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Sleeper;
			}
		}

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x0600C1F3 RID: 49651 RVA: 0x0047D738 File Offset: 0x0047B938
		public override int SerializedSize
		{
			get
			{
				int num = 25;
				string text = this.groupName;
				int num2 = num + (1 + ((text != null) ? new int?(text.Length) : null)).GetValueOrDefault() + 1 + 1 + 2 + 2 + 2 + 4;
				string text2 = this.minScript;
				return num2 + (1 + ((text2 != null) ? new int?(text2.Length) : null)).GetValueOrDefault();
			}
		}

		// Token: 0x0600C1F4 RID: 49652 RVA: 0x0047D7EC File Offset: 0x0047B9EC
		public override void Read(PooledBinaryReader _br)
		{
			base.Read(_br);
			this.groupName = _br.ReadString();
			this.isPriority = _br.ReadBoolean();
			this.isQuestExclude = _br.ReadBoolean();
			this.spawnCountMin = _br.ReadInt16();
			this.spawnCountMax = _br.ReadInt16();
			this.groupId = _br.ReadInt16();
			this.flags = _br.ReadInt32();
			this.minScript = _br.ReadString();
		}

		// Token: 0x0600C1F5 RID: 49653 RVA: 0x0047D860 File Offset: 0x0047BA60
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write(this.groupName);
			_bw.Write(this.isPriority);
			_bw.Write(this.isQuestExclude);
			_bw.Write(this.spawnCountMin);
			_bw.Write(this.spawnCountMax);
			_bw.Write(this.groupId);
			_bw.Write(this.flags);
			_bw.Write(this.minScript ?? "");
		}

		// Token: 0x0600C1F6 RID: 49654 RVA: 0x0047D8DD File Offset: 0x0047BADD
		public void SetTrigger(SleeperVolume.ETriggerType _type)
		{
			this.flags = (this.flags & -8) | (int)_type;
		}

		// Token: 0x0600C1F7 RID: 49655 RVA: 0x0047D8F0 File Offset: 0x0047BAF0
		public void SetTriggeredByFlag(byte _index)
		{
			if (!this.triggeredByIndices.Contains(_index))
			{
				this.triggeredByIndices.Add(_index);
			}
		}

		// Token: 0x0600C1F8 RID: 49656 RVA: 0x0047D90C File Offset: 0x0047BB0C
		public void ClearTriggeredBy()
		{
			this.triggeredByIndices.Clear();
		}

		// Token: 0x0600C1F9 RID: 49657 RVA: 0x0047D919 File Offset: 0x0047BB19
		public void RemoveTriggeredByFlag(byte _index)
		{
			this.triggeredByIndices.Remove(_index);
		}

		// Token: 0x0600C1FA RID: 49658 RVA: 0x0047D928 File Offset: 0x0047BB28
		public bool HasTriggeredBy(byte _index)
		{
			return this.triggeredByIndices.Contains(_index);
		}

		// Token: 0x0600C1FB RID: 49659 RVA: 0x0047D938 File Offset: 0x0047BB38
		public void ToggleTriggeredByFlag(byte layer)
		{
			int num = this.triggeredByIndices.IndexOf(layer);
			if (num >= 0)
			{
				this.triggeredByIndices.RemoveAt(num);
				return;
			}
			this.triggeredByIndices.Add(layer);
		}

		// Token: 0x0600C1FC RID: 49660 RVA: 0x0047D96F File Offset: 0x0047BB6F
		public bool HasAnyTriggeredBy()
		{
			return this.triggeredByIndices.Count > 0;
		}

		// Token: 0x040092CE RID: 37582
		public string groupName;

		// Token: 0x040092CF RID: 37583
		public bool isPriority;

		// Token: 0x040092D0 RID: 37584
		public bool isQuestExclude;

		// Token: 0x040092D1 RID: 37585
		public short spawnCountMin;

		// Token: 0x040092D2 RID: 37586
		public short spawnCountMax;

		// Token: 0x040092D3 RID: 37587
		public short groupId;

		// Token: 0x040092D4 RID: 37588
		public int flags;

		// Token: 0x040092D5 RID: 37589
		public string minScript;

		// Token: 0x040092D6 RID: 37590
		public readonly List<byte> triggeredByIndices = new List<byte>();
	}
}
