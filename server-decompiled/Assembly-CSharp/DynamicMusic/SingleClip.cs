using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A70 RID: 6768
	[Preserve]
	public class SingleClip : Content
	{
		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x0600CD43 RID: 52547 RVA: 0x004AC474 File Offset: 0x004AA674
		// (set) Token: 0x0600CD44 RID: 52548 RVA: 0x004AC47C File Offset: 0x004AA67C
		public AudioClip Clip
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD45 RID: 52549 RVA: 0x004AC485 File Offset: 0x004AA685
		public override void Unload()
		{
			this.Clip.UnloadAudioData();
			this.IsLoaded = false;
		}

		// Token: 0x0600CD46 RID: 52550 RVA: 0x004AC49A File Offset: 0x004AA69A
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

		// Token: 0x0600CD47 RID: 52551 RVA: 0x004AC4A9 File Offset: 0x004AA6A9
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.GetAttribute("section"), false);
			this.path = _xmlNode.GetAttribute("path");
		}

		// Token: 0x04009BC0 RID: 39872
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;
	}
}
