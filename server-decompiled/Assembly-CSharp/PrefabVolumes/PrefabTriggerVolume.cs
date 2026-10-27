using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace PrefabVolumes
{
	// Token: 0x0200191E RID: 6430
	[Preserve]
	public class PrefabTriggerVolume : PrefabVolumeAbs<PrefabTriggerVolume>
	{
		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x0600C5FA RID: 50682 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Trigger;
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x0600C5FB RID: 50683 RVA: 0x0048B9CA File Offset: 0x00489BCA
		public override int SerializedSize
		{
			get
			{
				return 26 + this.TriggersIndices.Count;
			}
		}

		// Token: 0x0600C5FC RID: 50684 RVA: 0x0048B9DA File Offset: 0x00489BDA
		public override void Reset()
		{
			base.Reset();
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C5FD RID: 50685 RVA: 0x0048B9F0 File Offset: 0x00489BF0
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

		// Token: 0x0600C5FE RID: 50686 RVA: 0x0048BA34 File Offset: 0x00489C34
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write((byte)this.TriggersIndices.Count);
			for (int i = 0; i < this.TriggersIndices.Count; i++)
			{
				_bw.Write(this.TriggersIndices[i]);
			}
		}

		// Token: 0x0600C5FF RID: 50687 RVA: 0x0048BA82 File Offset: 0x00489C82
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CopyValues(PrefabVolumeAbs<PrefabTriggerVolume> _target, bool _nonBasicOnly = false)
		{
			if (!_nonBasicOnly)
			{
				base.CopyValues(_target, false);
			}
			((PrefabTriggerVolume)_target).TriggersIndices.AddRange(this.TriggersIndices);
		}

		// Token: 0x0600C600 RID: 50688 RVA: 0x0048BAA5 File Offset: 0x00489CA5
		public void SetTriggersFlag(byte _index)
		{
			if (!this.TriggersIndices.Contains(_index))
			{
				this.TriggersIndices.Add(_index);
			}
		}

		// Token: 0x0600C601 RID: 50689 RVA: 0x0048BAC1 File Offset: 0x00489CC1
		public void RemoveTriggersFlag(byte _index)
		{
			this.TriggersIndices.Remove(_index);
		}

		// Token: 0x0600C602 RID: 50690 RVA: 0x0048BAD0 File Offset: 0x00489CD0
		public void RemoveAllTriggersFlags()
		{
			this.TriggersIndices.Clear();
		}

		// Token: 0x0600C603 RID: 50691 RVA: 0x0048BADD File Offset: 0x00489CDD
		public bool HasTriggers(byte _index)
		{
			return this.TriggersIndices.Contains(_index);
		}

		// Token: 0x0600C604 RID: 50692 RVA: 0x0048BAEC File Offset: 0x00489CEC
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

		// Token: 0x0600C605 RID: 50693 RVA: 0x0048BB23 File Offset: 0x00489D23
		public bool HasAnyTriggers()
		{
			return this.TriggersIndices.Count > 0;
		}

		// Token: 0x040095AF RID: 38319
		public readonly List<byte> TriggersIndices = new List<byte>();
	}
}
