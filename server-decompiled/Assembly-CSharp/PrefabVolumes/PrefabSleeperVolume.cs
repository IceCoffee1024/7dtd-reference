using System;
using System.Collections.Generic;

namespace PrefabVolumes
{
	// Token: 0x020018BD RID: 6333
	public class PrefabSleeperVolume : PrefabVolumeAbs<PrefabSleeperVolume>
	{
		// Token: 0x0600C377 RID: 50039 RVA: 0x00483784 File Offset: 0x00481984
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

		// Token: 0x0600C378 RID: 50040 RVA: 0x004837D4 File Offset: 0x004819D4
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

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x0600C379 RID: 50041 RVA: 0x00010E62 File Offset: 0x0000F062
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Sleeper;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x0600C37A RID: 50042 RVA: 0x00483864 File Offset: 0x00481A64
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

		// Token: 0x0600C37B RID: 50043 RVA: 0x00483918 File Offset: 0x00481B18
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

		// Token: 0x0600C37C RID: 50044 RVA: 0x0048398C File Offset: 0x00481B8C
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

		// Token: 0x0600C37D RID: 50045 RVA: 0x00483A09 File Offset: 0x00481C09
		public void SetTrigger(SleeperVolume.ETriggerType _type)
		{
			this.flags = (this.flags & -8) | (int)_type;
		}

		// Token: 0x0600C37E RID: 50046 RVA: 0x00483A1C File Offset: 0x00481C1C
		public void SetTriggeredByFlag(byte _index)
		{
			if (!this.triggeredByIndices.Contains(_index))
			{
				this.triggeredByIndices.Add(_index);
			}
		}

		// Token: 0x0600C37F RID: 50047 RVA: 0x00483A38 File Offset: 0x00481C38
		public void ClearTriggeredBy()
		{
			this.triggeredByIndices.Clear();
		}

		// Token: 0x0600C380 RID: 50048 RVA: 0x00483A45 File Offset: 0x00481C45
		public void RemoveTriggeredByFlag(byte _index)
		{
			this.triggeredByIndices.Remove(_index);
		}

		// Token: 0x0600C381 RID: 50049 RVA: 0x00483A54 File Offset: 0x00481C54
		public bool HasTriggeredBy(byte _index)
		{
			return this.triggeredByIndices.Contains(_index);
		}

		// Token: 0x0600C382 RID: 50050 RVA: 0x00483A64 File Offset: 0x00481C64
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

		// Token: 0x0600C383 RID: 50051 RVA: 0x00483A9B File Offset: 0x00481C9B
		public bool HasAnyTriggeredBy()
		{
			return this.triggeredByIndices.Count > 0;
		}

		// Token: 0x040093F7 RID: 37879
		public string groupName;

		// Token: 0x040093F8 RID: 37880
		public bool isPriority;

		// Token: 0x040093F9 RID: 37881
		public bool isQuestExclude;

		// Token: 0x040093FA RID: 37882
		public short spawnCountMin;

		// Token: 0x040093FB RID: 37883
		public short spawnCountMax;

		// Token: 0x040093FC RID: 37884
		public short groupId;

		// Token: 0x040093FD RID: 37885
		public int flags;

		// Token: 0x040093FE RID: 37886
		public string minScript;

		// Token: 0x040093FF RID: 37887
		public readonly List<byte> triggeredByIndices = new List<byte>();
	}
}
