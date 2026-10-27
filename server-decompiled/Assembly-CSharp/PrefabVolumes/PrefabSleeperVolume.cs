using System;
using System.Collections.Generic;

namespace PrefabVolumes
{
	// Token: 0x0200191A RID: 6426
	public class PrefabSleeperVolume : PrefabVolumeAbs<PrefabSleeperVolume>
	{
		// Token: 0x0600C5E3 RID: 50659 RVA: 0x0048B678 File Offset: 0x00489878
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

		// Token: 0x0600C5E4 RID: 50660 RVA: 0x0048B6C8 File Offset: 0x004898C8
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

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600C5E5 RID: 50661 RVA: 0x00010E62 File Offset: 0x0000F062
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Sleeper;
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600C5E6 RID: 50662 RVA: 0x0048B758 File Offset: 0x00489958
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

		// Token: 0x0600C5E7 RID: 50663 RVA: 0x0048B80C File Offset: 0x00489A0C
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

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x0048B880 File Offset: 0x00489A80
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

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x0048B8FD File Offset: 0x00489AFD
		public void SetTrigger(SleeperVolume.ETriggerType _type)
		{
			this.flags = (this.flags & -8) | (int)_type;
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x0048B910 File Offset: 0x00489B10
		public void SetTriggeredByFlag(byte _index)
		{
			if (!this.triggeredByIndices.Contains(_index))
			{
				this.triggeredByIndices.Add(_index);
			}
		}

		// Token: 0x0600C5EB RID: 50667 RVA: 0x0048B92C File Offset: 0x00489B2C
		public void ClearTriggeredBy()
		{
			this.triggeredByIndices.Clear();
		}

		// Token: 0x0600C5EC RID: 50668 RVA: 0x0048B939 File Offset: 0x00489B39
		public void RemoveTriggeredByFlag(byte _index)
		{
			this.triggeredByIndices.Remove(_index);
		}

		// Token: 0x0600C5ED RID: 50669 RVA: 0x0048B948 File Offset: 0x00489B48
		public bool HasTriggeredBy(byte _index)
		{
			return this.triggeredByIndices.Contains(_index);
		}

		// Token: 0x0600C5EE RID: 50670 RVA: 0x0048B958 File Offset: 0x00489B58
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

		// Token: 0x0600C5EF RID: 50671 RVA: 0x0048B98F File Offset: 0x00489B8F
		public bool HasAnyTriggeredBy()
		{
			return this.triggeredByIndices.Count > 0;
		}

		// Token: 0x040095A6 RID: 38310
		public string groupName;

		// Token: 0x040095A7 RID: 38311
		public bool isPriority;

		// Token: 0x040095A8 RID: 38312
		public bool isQuestExclude;

		// Token: 0x040095A9 RID: 38313
		public short spawnCountMin;

		// Token: 0x040095AA RID: 38314
		public short spawnCountMax;

		// Token: 0x040095AB RID: 38315
		public short groupId;

		// Token: 0x040095AC RID: 38316
		public int flags;

		// Token: 0x040095AD RID: 38317
		public string minScript;

		// Token: 0x040095AE RID: 38318
		public readonly List<byte> triggeredByIndices = new List<byte>();
	}
}
