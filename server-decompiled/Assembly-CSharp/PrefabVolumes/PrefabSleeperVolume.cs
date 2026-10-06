using System;
using System.Collections.Generic;

namespace PrefabVolumes
{
	// Token: 0x020018E9 RID: 6377
	public class PrefabSleeperVolume : PrefabVolumeAbs<PrefabSleeperVolume>
	{
		// Token: 0x0600C4CF RID: 50383 RVA: 0x00487CCC File Offset: 0x00485ECC
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

		// Token: 0x0600C4D0 RID: 50384 RVA: 0x00487D1C File Offset: 0x00485F1C
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

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x0600C4D1 RID: 50385 RVA: 0x00010E62 File Offset: 0x0000F062
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Sleeper;
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x0600C4D2 RID: 50386 RVA: 0x00487DAC File Offset: 0x00485FAC
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

		// Token: 0x0600C4D3 RID: 50387 RVA: 0x00487E60 File Offset: 0x00486060
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

		// Token: 0x0600C4D4 RID: 50388 RVA: 0x00487ED4 File Offset: 0x004860D4
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

		// Token: 0x0600C4D5 RID: 50389 RVA: 0x00487F51 File Offset: 0x00486151
		public void SetTrigger(SleeperVolume.ETriggerType _type)
		{
			this.flags = (this.flags & -8) | (int)_type;
		}

		// Token: 0x0600C4D6 RID: 50390 RVA: 0x00487F64 File Offset: 0x00486164
		public void SetTriggeredByFlag(byte _index)
		{
			if (!this.triggeredByIndices.Contains(_index))
			{
				this.triggeredByIndices.Add(_index);
			}
		}

		// Token: 0x0600C4D7 RID: 50391 RVA: 0x00487F80 File Offset: 0x00486180
		public void ClearTriggeredBy()
		{
			this.triggeredByIndices.Clear();
		}

		// Token: 0x0600C4D8 RID: 50392 RVA: 0x00487F8D File Offset: 0x0048618D
		public void RemoveTriggeredByFlag(byte _index)
		{
			this.triggeredByIndices.Remove(_index);
		}

		// Token: 0x0600C4D9 RID: 50393 RVA: 0x00487F9C File Offset: 0x0048619C
		public bool HasTriggeredBy(byte _index)
		{
			return this.triggeredByIndices.Contains(_index);
		}

		// Token: 0x0600C4DA RID: 50394 RVA: 0x00487FAC File Offset: 0x004861AC
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

		// Token: 0x0600C4DB RID: 50395 RVA: 0x00487FE3 File Offset: 0x004861E3
		public bool HasAnyTriggeredBy()
		{
			return this.triggeredByIndices.Count > 0;
		}

		// Token: 0x040094BD RID: 38077
		public string groupName;

		// Token: 0x040094BE RID: 38078
		public bool isPriority;

		// Token: 0x040094BF RID: 38079
		public bool isQuestExclude;

		// Token: 0x040094C0 RID: 38080
		public short spawnCountMin;

		// Token: 0x040094C1 RID: 38081
		public short spawnCountMax;

		// Token: 0x040094C2 RID: 38082
		public short groupId;

		// Token: 0x040094C3 RID: 38083
		public int flags;

		// Token: 0x040094C4 RID: 38084
		public string minScript;

		// Token: 0x040094C5 RID: 38085
		public readonly List<byte> triggeredByIndices = new List<byte>();
	}
}
