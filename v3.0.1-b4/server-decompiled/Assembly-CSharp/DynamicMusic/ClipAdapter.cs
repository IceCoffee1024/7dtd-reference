using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils;
using MusicUtils.Enums;
using Unity.Profiling;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A0C RID: 6668
	[Preserve]
	public class ClipAdapter : IClipAdapter
	{
		// Token: 0x0600CA03 RID: 51715 RVA: 0x004A0880 File Offset: 0x0049EA80
		public float GetSample(int idx, params float[] _params)
		{
			if (idx % 4096 == 0)
			{
				this.reader.Position = 2 * idx;
				this.reader.Read(this.sampleData, 4096);
			}
			return this.sampleData[idx % 4096];
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x0600CA04 RID: 51716 RVA: 0x004A08BD File Offset: 0x0049EABD
		// (set) Token: 0x0600CA05 RID: 51717 RVA: 0x004A08C5 File Offset: 0x0049EAC5
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA06 RID: 51718 RVA: 0x004A08CE File Offset: 0x0049EACE
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

		// Token: 0x0600CA07 RID: 51719 RVA: 0x004A08E0 File Offset: 0x0049EAE0
		public void LoadImmediate()
		{
			using (ClipAdapter.s_LoadImmediateMarker.Auto())
			{
				this.reader = new WaveReader(this.path);
				this.sampleData = MemoryPools.poolFloat.Alloc(4096);
				this.IsLoaded = true;
			}
		}

		// Token: 0x0600CA08 RID: 51720 RVA: 0x004A0948 File Offset: 0x0049EB48
		public void Unload()
		{
			MemoryPools.poolFloat.Free(this.sampleData);
			this.sampleData = null;
			this.reader.Cleanup();
			this.reader = null;
			this.IsLoaded = false;
		}

		// Token: 0x0600CA09 RID: 51721 RVA: 0x004A097A File Offset: 0x0049EB7A
		public void ParseXml(XElement _xmlNode)
		{
			this.path = _xmlNode.GetAttribute("value");
		}

		// Token: 0x0600CA0A RID: 51722 RVA: 0x004A0994 File Offset: 0x0049EB94
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

		// Token: 0x0400999D RID: 39325
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.Load");

		// Token: 0x0400999E RID: 39326
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadImmediateMarker = new ProfilerMarker("DynamicMusic.ClipAdapter.LoadImmediate");

		// Token: 0x0400999F RID: 39327
		[PublicizedFrom(EAccessModifier.Private)]
		public const int bufferSize = 4096;

		// Token: 0x040099A0 RID: 39328
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;

		// Token: 0x040099A1 RID: 39329
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] sampleData;

		// Token: 0x040099A2 RID: 39330
		[PublicizedFrom(EAccessModifier.Private)]
		public WaveReader reader;
	}
}
