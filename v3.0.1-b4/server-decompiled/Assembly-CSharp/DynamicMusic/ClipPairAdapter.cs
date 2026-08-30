using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A0E RID: 6670
	[Preserve]
	public class ClipPairAdapter : IClipAdapter
	{
		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x0600CA13 RID: 51731 RVA: 0x004A0AE0 File Offset: 0x0049ECE0
		// (set) Token: 0x0600CA14 RID: 51732 RVA: 0x004A0AE8 File Offset: 0x0049ECE8
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA15 RID: 51733 RVA: 0x004A0AF1 File Offset: 0x0049ECF1
		public ClipPairAdapter()
		{
			this.clipAdapterLo = new ClipAdapter();
			this.clipAdapterHi = new ClipAdapter();
		}

		// Token: 0x0600CA16 RID: 51734 RVA: 0x004A0B0F File Offset: 0x0049ED0F
		public float GetSample(int idx, params float[] _params)
		{
			return _params[0] * (this.clipAdapterLo.GetSample(idx, null) * (1f - _params[1]) + _params[1] * this.clipAdapterHi.GetSample(idx, null));
		}

		// Token: 0x0600CA17 RID: 51735 RVA: 0x004A0B3E File Offset: 0x0049ED3E
		public IEnumerator Load()
		{
			yield return this.clipAdapterLo.Load();
			yield return this.clipAdapterHi.Load();
			yield break;
		}

		// Token: 0x0600CA18 RID: 51736 RVA: 0x004A0B4D File Offset: 0x0049ED4D
		public void LoadImmediate()
		{
			this.clipAdapterLo.LoadImmediate();
			this.clipAdapterHi.LoadImmediate();
		}

		// Token: 0x0600CA19 RID: 51737 RVA: 0x004A0B65 File Offset: 0x0049ED65
		public void Unload()
		{
			this.clipAdapterLo.Unload();
			this.clipAdapterHi.Unload();
			this.IsLoaded = false;
		}

		// Token: 0x0600CA1A RID: 51738 RVA: 0x000027FC File Offset: 0x000009FC
		public void ParseXml(XElement _xmlNode)
		{
		}

		// Token: 0x0600CA1B RID: 51739 RVA: 0x004A0B84 File Offset: 0x0049ED84
		public void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "")
		{
			this.clipAdapterLo.SetPaths(_num, _placement, _section, _layer, "Lo");
			this.clipAdapterHi.SetPaths(_num, _placement, _section, _layer, "Hi");
		}

		// Token: 0x040099A7 RID: 39335
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterLo;

		// Token: 0x040099A8 RID: 39336
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterHi;
	}
}
