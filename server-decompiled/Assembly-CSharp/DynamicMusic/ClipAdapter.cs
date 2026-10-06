using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils;
using MusicUtils.Enums;
using Unity.Profiling;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A5F RID: 6751
	[Preserve]
	public class ClipAdapter : IClipAdapter
	{
		// Token: 0x0600CCE4 RID: 52452 RVA: 0x004AB140 File Offset: 0x004A9340
		public float GetSample(int idx, params float[] _params)
		{
			if (idx % 4096 == 0)
			{
				this.reader.Position = 2 * idx;
				this.reader.Read(this.sampleData, 4096);
			}
			return this.sampleData[idx % 4096];
		}

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600CCE5 RID: 52453 RVA: 0x004AB17D File Offset: 0x004A937D
		// (set) Token: 0x0600CCE6 RID: 52454 RVA: 0x004AB185 File Offset: 0x004A9385
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CCE7 RID: 52455 RVA: 0x004AB18E File Offset: 0x004A938E
		public IEnumerator Load()
		{
			using (ClipAdapter.s_LoadMarker.Auto())
			{
				this.reader = new WaveReader(this.path);
				this.sampleData = MemoryPools.poolFloat.Alloc(4096);
				this.IsLoaded = true;
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600CCE8 RID: 52456 RVA: 0x004AB1A0 File Offset: 0x004A93A0
		public void LoadImmediate()
		{
			using (ClipAdapter.s_LoadImmediateMarker.Auto())
			{
				this.reader = new WaveReader(this.path);
				this.sampleData = MemoryPools.poolFloat.Alloc(4096);
				this.IsLoaded = true;
			}
		}

		// Token: 0x0600CCE9 RID: 52457 RVA: 0x004AB208 File Offset: 0x004A9408
		public void Unload()
		{
			MemoryPools.poolFloat.Free(this.sampleData);
			this.sampleData = null;
			this.reader.Cleanup();
			this.reader = null;
			this.IsLoaded = false;
		}

		// Token: 0x0600CCEA RID: 52458 RVA: 0x004AB23A File Offset: 0x004A943A
		public void ParseXml(XElement _xmlNode)
		{
			this.path = _xmlNode.GetAttribute("value");
		}

		// Token: 0x0600CCEB RID: 52459 RVA: 0x004AB254 File Offset: 0x004A9454
		public void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "")
		{
			this.path = string.Concat(new string[]
			{
				GameIO.GetApplicationPath(),
				"/Data/Music/",
				_num.ToString("000"),
				DMSConstants.PlacementAbbrv[_placement],
				DMSConstants.SectionAbbrvs[_section],
				DMSConstants.LayerAbbrvs[_layer],
				stress,
				".wav"
			});
		}

		// Token: 0x04009B94 RID: 39828
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.Load");

		// Token: 0x04009B95 RID: 39829
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadImmediateMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.LoadImmediate");

		// Token: 0x04009B96 RID: 39830
		[PublicizedFrom(EAccessModifier.Private)]
		public const int bufferSize = 4096;

		// Token: 0x04009B97 RID: 39831
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;

		// Token: 0x04009B98 RID: 39832
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] sampleData;

		// Token: 0x04009B99 RID: 39833
		[PublicizedFrom(EAccessModifier.Private)]
		public WaveReader reader;
	}
}
