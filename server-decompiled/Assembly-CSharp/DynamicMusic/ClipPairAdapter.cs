using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A92 RID: 6802
	[Preserve]
	public class ClipPairAdapter : IClipAdapter
	{
		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600CE08 RID: 52744 RVA: 0x004AEE9C File Offset: 0x004AD09C
		// (set) Token: 0x0600CE09 RID: 52745 RVA: 0x004AEEA4 File Offset: 0x004AD0A4
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE0A RID: 52746 RVA: 0x004AEEAD File Offset: 0x004AD0AD
		public ClipPairAdapter()
		{
			this.clipAdapterLo = new ClipAdapter();
			this.clipAdapterHi = new ClipAdapter();
		}

		// Token: 0x0600CE0B RID: 52747 RVA: 0x004AEECB File Offset: 0x004AD0CB
		public float GetSample(int idx, params float[] _params)
		{
			return _params[0] * (this.clipAdapterLo.GetSample(idx, null) * (1f - _params[1]) + _params[1] * this.clipAdapterHi.GetSample(idx, null));
		}

		// Token: 0x0600CE0C RID: 52748 RVA: 0x004AEEFA File Offset: 0x004AD0FA
		public IEnumerator Load()
		{
			yield return this.clipAdapterLo.Load();
			yield return this.clipAdapterHi.Load();
			yield break;
		}

		// Token: 0x0600CE0D RID: 52749 RVA: 0x004AEF09 File Offset: 0x004AD109
		public void LoadImmediate()
		{
			this.clipAdapterLo.LoadImmediate();
			this.clipAdapterHi.LoadImmediate();
		}

		// Token: 0x0600CE0E RID: 52750 RVA: 0x004AEF21 File Offset: 0x004AD121
		public void Unload()
		{
			this.clipAdapterLo.Unload();
			this.clipAdapterHi.Unload();
			this.IsLoaded = false;
		}

		// Token: 0x0600CE0F RID: 52751 RVA: 0x000027FC File Offset: 0x000009FC
		public void ParseXml(XElement _xmlNode)
		{
		}

		// Token: 0x0600CE10 RID: 52752 RVA: 0x004AEF40 File Offset: 0x004AD140
		public void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "")
		{
			this.clipAdapterLo.SetPaths(_num, _placement, _section, _layer, "Lo");
			this.clipAdapterHi.SetPaths(_num, _placement, _section, _layer, "Hi");
		}

		// Token: 0x04009C87 RID: 40071
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterLo;

		// Token: 0x04009C88 RID: 40072
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterHi;
	}
}
