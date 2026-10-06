using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A61 RID: 6753
	[Preserve]
	public class ClipPairAdapter : IClipAdapter
	{
		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x0600CCF4 RID: 52468 RVA: 0x004AB3A0 File Offset: 0x004A95A0
		// (set) Token: 0x0600CCF5 RID: 52469 RVA: 0x004AB3A8 File Offset: 0x004A95A8
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CCF6 RID: 52470 RVA: 0x004AB3B1 File Offset: 0x004A95B1
		public ClipPairAdapter()
		{
			this.clipAdapterLo = new ClipAdapter();
			this.clipAdapterHi = new ClipAdapter();
		}

		// Token: 0x0600CCF7 RID: 52471 RVA: 0x004AB3CF File Offset: 0x004A95CF
		public float GetSample(int idx, params float[] _params)
		{
			return _params[0] * (this.clipAdapterLo.GetSample(idx, null) * (1f - _params[1]) + _params[1] * this.clipAdapterHi.GetSample(idx, null));
		}

		// Token: 0x0600CCF8 RID: 52472 RVA: 0x004AB3FE File Offset: 0x004A95FE
		public IEnumerator Load()
		{
			yield return this.clipAdapterLo.Load();
			yield return this.clipAdapterHi.Load();
			yield break;
		}

		// Token: 0x0600CCF9 RID: 52473 RVA: 0x004AB40D File Offset: 0x004A960D
		public void LoadImmediate()
		{
			this.clipAdapterLo.LoadImmediate();
			this.clipAdapterHi.LoadImmediate();
		}

		// Token: 0x0600CCFA RID: 52474 RVA: 0x004AB425 File Offset: 0x004A9625
		public void Unload()
		{
			this.clipAdapterLo.Unload();
			this.clipAdapterHi.Unload();
			this.IsLoaded = false;
		}

		// Token: 0x0600CCFB RID: 52475 RVA: 0x000027FC File Offset: 0x000009FC
		public void ParseXml(XElement _xmlNode)
		{
		}

		// Token: 0x0600CCFC RID: 52476 RVA: 0x004AB444 File Offset: 0x004A9644
		public void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "")
		{
			this.clipAdapterLo.SetPaths(_num, _placement, _section, _layer, "Lo");
			this.clipAdapterHi.SetPaths(_num, _placement, _section, _layer, "Hi");
		}

		// Token: 0x04009B9E RID: 39838
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterLo;

		// Token: 0x04009B9F RID: 39839
		[PublicizedFrom(EAccessModifier.Private)]
		public ClipAdapter clipAdapterHi;
	}
}
