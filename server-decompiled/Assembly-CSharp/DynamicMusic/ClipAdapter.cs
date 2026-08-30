using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils;
using MusicUtils.Enums;
using Unity.Profiling;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A33 RID: 6707
	[Preserve]
	public class ClipAdapter : IClipAdapter
	{
		// Token: 0x0600CB8C RID: 52108 RVA: 0x004A6C74 File Offset: 0x004A4E74
		public float GetSample(int idx, params float[] _params)
		{
			if (idx % 4096 == 0)
			{
				this.reader.Position = 2 * idx;
				this.reader.Read(this.sampleData, 4096);
			}
			return this.sampleData[idx % 4096];
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x0600CB8D RID: 52109 RVA: 0x004A6CB1 File Offset: 0x004A4EB1
		// (set) Token: 0x0600CB8E RID: 52110 RVA: 0x004A6CB9 File Offset: 0x004A4EB9
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x004A6CC2 File Offset: 0x004A4EC2
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

		// Token: 0x0600CB90 RID: 52112 RVA: 0x004A6CD4 File Offset: 0x004A4ED4
		public void LoadImmediate()
		{
			using (ClipAdapter.s_LoadImmediateMarker.Auto())
			{
				this.reader = new WaveReader(this.path);
				this.sampleData = MemoryPools.poolFloat.Alloc(4096);
				this.IsLoaded = true;
			}
		}

		// Token: 0x0600CB91 RID: 52113 RVA: 0x004A6D3C File Offset: 0x004A4F3C
		public void Unload()
		{
			MemoryPools.poolFloat.Free(this.sampleData);
			this.sampleData = null;
			this.reader.Cleanup();
			this.reader = null;
			this.IsLoaded = false;
		}

		// Token: 0x0600CB92 RID: 52114 RVA: 0x004A6D6E File Offset: 0x004A4F6E
		public void ParseXml(XElement _xmlNode)
		{
			this.path = _xmlNode.GetAttribute("value");
		}

		// Token: 0x0600CB93 RID: 52115 RVA: 0x004A6D88 File Offset: 0x004A4F88
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

		// Token: 0x04009ACE RID: 39630
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.Load");

		// Token: 0x04009ACF RID: 39631
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadImmediateMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.LoadImmediate");

		// Token: 0x04009AD0 RID: 39632
		[PublicizedFrom(EAccessModifier.Private)]
		public const int bufferSize = 4096;

		// Token: 0x04009AD1 RID: 39633
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;

		// Token: 0x04009AD2 RID: 39634
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] sampleData;

		// Token: 0x04009AD3 RID: 39635
		[PublicizedFrom(EAccessModifier.Private)]
		public WaveReader reader;
	}
}
