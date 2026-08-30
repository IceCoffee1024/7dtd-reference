using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace PrefabVolumes
{
	// Token: 0x0200189B RID: 6299
	[Preserve]
	public class PrefabTriggerVolume : PrefabVolumeAbs<PrefabTriggerVolume>
	{
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x0600C207 RID: 49671 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Trigger;
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x0600C208 RID: 49672 RVA: 0x0047D9AA File Offset: 0x0047BBAA
		public override int SerializedSize
		{
			get
			{
				return 26 + this.TriggersIndices.Count;
			}
		}

		// Token: 0x0600C209 RID: 49673 RVA: 0x0047D9BA File Offset: 0x0047BBBA
		public override void Reset()
		{
			base.Reset();
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C20A RID: 49674 RVA: 0x0047D9D0 File Offset: 0x0047BBD0
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

		// Token: 0x0600C20B RID: 49675 RVA: 0x0047DA14 File Offset: 0x0047BC14
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write((byte)this.TriggersIndices.Count);
			for (int i = 0; i < this.TriggersIndices.Count; i++)
			{
				_bw.Write(this.TriggersIndices[i]);
			}
		}

		// Token: 0x0600C20C RID: 49676 RVA: 0x0047DA62 File Offset: 0x0047BC62
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<PrefabTriggerVolume> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			((PrefabTriggerVolume)_target).TriggersIndices.AddRange(this.TriggersIndices);
		}

		// Token: 0x0600C20D RID: 49677 RVA: 0x0047DA85 File Offset: 0x0047BC85
		public void SetTriggersFlag(byte _index)
		{
			if (!this.TriggersIndices.Contains(_index))
			{
				this.TriggersIndices.Add(_index);
			}
		}

		// Token: 0x0600C20E RID: 49678 RVA: 0x0047DAA1 File Offset: 0x0047BCA1
		public void RemoveTriggersFlag(byte _index)
		{
			this.TriggersIndices.Remove(_index);
		}

		// Token: 0x0600C20F RID: 49679 RVA: 0x0047DAB0 File Offset: 0x0047BCB0
		public void RemoveAllTriggersFlags()
		{
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C210 RID: 49680 RVA: 0x0047DABD File Offset: 0x0047BCBD
		public bool HasTriggers(byte _index)
		{
			return this.TriggersIndices.Contains(_index);
		}

		// Token: 0x0600C211 RID: 49681 RVA: 0x0047DACC File Offset: 0x0047BCCC
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

		// Token: 0x0600C212 RID: 49682 RVA: 0x0047DB03 File Offset: 0x0047BD03
		public bool HasAnyTriggers()
		{
			return this.TriggersIndices.Count > 0;
		}

		// Token: 0x040092D7 RID: 37591
		public readonly List<byte> TriggersIndices = new List<byte>();
	}
}
