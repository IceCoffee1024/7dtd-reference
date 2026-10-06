using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace PrefabVolumes
{
	// Token: 0x020018ED RID: 6381
	[Preserve]
	public class PrefabTriggerVolume : PrefabVolumeAbs<PrefabTriggerVolume>
	{
		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x0600C4E6 RID: 50406 RVA: 0x00080804 File Offset: 0x0007EA04
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Trigger;
			}
		}

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x0600C4E7 RID: 50407 RVA: 0x0048801E File Offset: 0x0048621E
		public override int SerializedSize
		{
			get
			{
				return 26 + this.TriggersIndices.Count;
			}
		}

		// Token: 0x0600C4E8 RID: 50408 RVA: 0x0048802E File Offset: 0x0048622E
		public override void Reset()
		{
			base.Reset();
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C4E9 RID: 50409 RVA: 0x00488044 File Offset: 0x00486244
		public override void Read(PooledBinaryReader _br)
		{
			base.Read(_br);
			this.TriggersIndices.Clear();
			int num = (int)_br.ReadByte();
			for (int i = 0; i < num; i++)
			{
				this.TriggersIndices.Add(_br.ReadByte());
			}
		}

		// Token: 0x0600C4EA RID: 50410 RVA: 0x00488088 File Offset: 0x00486288
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write((byte)this.TriggersIndices.Count);
			for (int i = 0; i < this.TriggersIndices.Count; i++)
			{
				_bw.Write(this.TriggersIndices[i]);
			}
		}

		// Token: 0x0600C4EB RID: 50411 RVA: 0x004880D6 File Offset: 0x004862D6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<PrefabTriggerVolume> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			((PrefabTriggerVolume)_target).TriggersIndices.AddRange(this.TriggersIndices);
		}

		// Token: 0x0600C4EC RID: 50412 RVA: 0x004880F9 File Offset: 0x004862F9
		public void SetTriggersFlag(byte _index)
		{
			if (!this.TriggersIndices.Contains(_index))
			{
				this.TriggersIndices.Add(_index);
			}
		}

		// Token: 0x0600C4ED RID: 50413 RVA: 0x00488115 File Offset: 0x00486315
		public void RemoveTriggersFlag(byte _index)
		{
			this.TriggersIndices.Remove(_index);
		}

		// Token: 0x0600C4EE RID: 50414 RVA: 0x00488124 File Offset: 0x00486324
		public void RemoveAllTriggersFlags()
		{
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C4EF RID: 50415 RVA: 0x00488131 File Offset: 0x00486331
		public bool HasTriggers(byte _index)
		{
			return this.TriggersIndices.Contains(_index);
		}

		// Token: 0x0600C4F0 RID: 50416 RVA: 0x00488140 File Offset: 0x00486340
		public void ToggleTriggersFlag(byte layer)
		{
			int num = this.TriggersIndices.IndexOf(layer);
			if (num >= 0)
			{
				this.TriggersIndices.RemoveAt(num);
				return;
			}
			this.TriggersIndices.Add(layer);
		}

		// Token: 0x0600C4F1 RID: 50417 RVA: 0x00488177 File Offset: 0x00486377
		public bool HasAnyTriggers()
		{
			return this.TriggersIndices.Count > 0;
		}

		// Token: 0x040094C6 RID: 38086
		public readonly List<byte> TriggersIndices = new List<byte>();
	}
}
