using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace PrefabVolumes
{
	// Token: 0x020018C1 RID: 6337
	[Preserve]
	public class PrefabTriggerVolume : PrefabVolumeAbs<PrefabTriggerVolume>
	{
		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x0600C38E RID: 50062 RVA: 0x00080550 File Offset: 0x0007E750
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Trigger;
			}
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x0600C38F RID: 50063 RVA: 0x00483AD6 File Offset: 0x00481CD6
		public override int SerializedSize
		{
			get
			{
				return 26 + this.TriggersIndices.Count;
			}
		}

		// Token: 0x0600C390 RID: 50064 RVA: 0x00483AE6 File Offset: 0x00481CE6
		public override void Reset()
		{
			base.Reset();
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C391 RID: 50065 RVA: 0x00483AFC File Offset: 0x00481CFC
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

		// Token: 0x0600C392 RID: 50066 RVA: 0x00483B40 File Offset: 0x00481D40
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write((byte)this.TriggersIndices.Count);
			for (int i = 0; i < this.TriggersIndices.Count; i++)
			{
				_bw.Write(this.TriggersIndices[i]);
			}
		}

		// Token: 0x0600C393 RID: 50067 RVA: 0x00483B8E File Offset: 0x00481D8E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<PrefabTriggerVolume> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			((PrefabTriggerVolume)_target).TriggersIndices.AddRange(this.TriggersIndices);
		}

		// Token: 0x0600C394 RID: 50068 RVA: 0x00483BB1 File Offset: 0x00481DB1
		public void SetTriggersFlag(byte _index)
		{
			if (!this.TriggersIndices.Contains(_index))
			{
				this.TriggersIndices.Add(_index);
			}
		}

		// Token: 0x0600C395 RID: 50069 RVA: 0x00483BCD File Offset: 0x00481DCD
		public void RemoveTriggersFlag(byte _index)
		{
			this.TriggersIndices.Remove(_index);
		}

		// Token: 0x0600C396 RID: 50070 RVA: 0x00483BDC File Offset: 0x00481DDC
		public void RemoveAllTriggersFlags()
		{
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C397 RID: 50071 RVA: 0x00483BE9 File Offset: 0x00481DE9
		public bool HasTriggers(byte _index)
		{
			return this.TriggersIndices.Contains(_index);
		}

		// Token: 0x0600C398 RID: 50072 RVA: 0x00483BF8 File Offset: 0x00481DF8
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

		// Token: 0x0600C399 RID: 50073 RVA: 0x00483C2F File Offset: 0x00481E2F
		public bool HasAnyTriggers()
		{
			return this.TriggersIndices.Count > 0;
		}

		// Token: 0x04009400 RID: 37888
		public readonly List<byte> TriggersIndices = new List<byte>();
	}
}
