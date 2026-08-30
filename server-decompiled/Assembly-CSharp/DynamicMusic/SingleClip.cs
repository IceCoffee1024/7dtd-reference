using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A1D RID: 6685
	[Preserve]
	public class SingleClip : Content
	{
		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x0600CA62 RID: 51810 RVA: 0x004A1BB4 File Offset: 0x0049FDB4
		// (set) Token: 0x0600CA63 RID: 51811 RVA: 0x004A1BBC File Offset: 0x0049FDBC
		public AudioClip Clip
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA64 RID: 51812 RVA: 0x004A1BC5 File Offset: 0x0049FDC5
		public override void Unload()
		{
			this.Clip.UnloadAudioData();
			this.IsLoaded = false;
		}

		// Token: 0x0600CA65 RID: 51813 RVA: 0x004A1BDA File Offset: 0x0049FDDA
		public override IEnumerator Load()
		{
			if (!this.IsLoaded)
			{
				SingleClip.<>c__DisplayClass6_0 CS$<>8__locals1 = new SingleClip.<>c__DisplayClass6_0();
				CS$<>8__locals1.requestTask = LoadManager.LoadAsset<AudioClip>(this.path, null, null, false, false, false);
				yield return new WaitUntil(() => CS$<>8__locals1.requestTask.IsDone);
				this.Clip = CS$<>8__locals1.requestTask.Asset;
				CS$<>8__locals1 = null;
			}
			this.IsLoaded = true;
			yield break;
		}

		// Token: 0x0600CA66 RID: 51814 RVA: 0x004A1BE9 File Offset: 0x0049FDE9
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.GetAttribute("section"), false);
			this.path = _xmlNode.GetAttribute("path");
		}

		// Token: 0x040099C9 RID: 39369
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;
	}
}
