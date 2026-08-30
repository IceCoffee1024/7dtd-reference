using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A35 RID: 6709
	[Preserve]
	public class ClipPairAdapter : IClipAdapter
	{
		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600CB9C RID: 52124 RVA: 0x004A6ED4 File Offset: 0x004A50D4
		// (set) Token: 0x0600CB9D RID: 52125 RVA: 0x004A6EDC File Offset: 0x004A50DC
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CB9E RID: 52126 RVA: 0x004A6EE5 File Offset: 0x004A50E5
		public ClipPairAdapter()
		{
			this.clipAdapterLo = new ClipAdapter();
			this.clipAdapterHi = new ClipAdapter();
		}

		// Token: 0x0600CB9F RID: 52127 RVA: 0x004A6F03 File Offset: 0x004A5103
		public float GetSample(int idx, params float[] _params)
		{
			return _params[0] * (this.clipAdapterLo.GetSample(idx, null) * (1f - _params[1]) + _params[1] * this.clipAdapterHi.GetSample(idx, null));
		}

		// Token: 0x0600CBA0 RID: 52128 RVA: 0x004A6F32 File Offset: 0x004A5132
		public IEnumerator Load()
		{
			yield return this.clipAdapterLo.Load();
			yield return this.clipAdapterHi.Load();
			yield break;
		}

		// Token: 0x0600CBA1 RID: 52129 RVA: 0x004A6F41 File Offset: 0x004A5141
		public void LoadImmediate()
		{
			this.clipAdapterLo.LoadImmediate();
			this.clipAdapterHi.LoadImmediate();
		}

		// Token: 0x0600CBA2 RID: 52130 RVA: 0x004A6F59 File Offset: 0x004A5159
		public void Unload()
		{
			this.clipAdapterLo.Unload();
			this.clipAdapterHi.Unload();
			this.IsLoaded = false;
		}

		// Token: 0x0600CBA3 RID: 52131 RVA: 0x000027FC File Offset: 0x000009FC
		public void ParseXml(XElement _xmlNode)
		{
		}

		// Token: 0x0600CBA4 RID: 52132 RVA: 0x004A6F78 File Offset: 0x004A5178
		public void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "")
		{
			this.clipAdapterLo.SetPaths(_num, _placement, _section, _layer, "Lo");
			this.clipAdapterHi.SetPaths(_num, _placement, _section, _layer, "Hi");
		}

		// Token: 0x04009AD8 RID: 39640
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterLo;

		// Token: 0x04009AD9 RID: 39641
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterHi;
	}
}
