using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils;
using MusicUtils.Enums;
using Unity.Profiling;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A90 RID: 6800
	[Preserve]
	public class ClipAdapter : IClipAdapter
	{
		// Token: 0x0600CDF8 RID: 52728 RVA: 0x004AEC3C File Offset: 0x004ACE3C
		public float GetSample(int idx, params float[] _params)
		{
			if (idx % 4096 == 0)
			{
				this.reader.Position = 2 * idx;
				this.reader.Read(this.sampleData, 4096);
			}
			return this.sampleData[idx % 4096];
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600CDF9 RID: 52729 RVA: 0x004AEC79 File Offset: 0x004ACE79
		// (set) Token: 0x0600CDFA RID: 52730 RVA: 0x004AEC81 File Offset: 0x004ACE81
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CDFB RID: 52731 RVA: 0x004AEC8A File Offset: 0x004ACE8A
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

		// Token: 0x0600CDFC RID: 52732 RVA: 0x004AEC9C File Offset: 0x004ACE9C
		public void LoadImmediate()
		{
			using (ClipAdapter.s_LoadImmediateMarker.Auto())
			{
				this.reader = new WaveReader(this.path);
				this.sampleData = MemoryPools.poolFloat.Alloc(4096);
				this.IsLoaded = true;
			}
		}

		// Token: 0x0600CDFD RID: 52733 RVA: 0x004AED04 File Offset: 0x004ACF04
		public void Unload()
		{
			MemoryPools.poolFloat.Free(this.sampleData);
			this.sampleData = null;
			this.reader.Cleanup();
			this.reader = null;
			this.IsLoaded = false;
		}

		// Token: 0x0600CDFE RID: 52734 RVA: 0x004AED36 File Offset: 0x004ACF36
		public void ParseXml(XElement _xmlNode)
		{
			this.path = _xmlNode.GetAttribute("value");
		}

		// Token: 0x0600CDFF RID: 52735 RVA: 0x004AED50 File Offset: 0x004ACF50
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

		// Token: 0x04009C7D RID: 40061
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.Load");

		// Token: 0x04009C7E RID: 40062
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadImmediateMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.LoadImmediate");

		// Token: 0x04009C7F RID: 40063
		[PublicizedFrom(EAccessModifier.Private)]
		public const int bufferSize = 4096;

		// Token: 0x04009C80 RID: 40064
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;

		// Token: 0x04009C81 RID: 40065
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] sampleData;

		// Token: 0x04009C82 RID: 40066
		[PublicizedFrom(EAccessModifier.Private)]
		public WaveReader reader;
	}
}
