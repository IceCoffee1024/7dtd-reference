using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A44 RID: 6724
	[Preserve]
	public class SingleClip : Content
	{
		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x0600CBEB RID: 52203 RVA: 0x004A7FA8 File Offset: 0x004A61A8
		// (set) Token: 0x0600CBEC RID: 52204 RVA: 0x004A7FB0 File Offset: 0x004A61B0
		public AudioClip Clip
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CBED RID: 52205 RVA: 0x004A7FB9 File Offset: 0x004A61B9
		public override void Unload()
		{
			this.Clip.UnloadAudioData();
			this.IsLoaded = false;
		}

		// Token: 0x0600CBEE RID: 52206 RVA: 0x004A7FCE File Offset: 0x004A61CE
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

		// Token: 0x0600CBEF RID: 52207 RVA: 0x004A7FDD File Offset: 0x004A61DD
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.GetAttribute("section"), false);
			this.path = _xmlNode.GetAttribute("path");
		}

		// Token: 0x04009AFA RID: 39674
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;
	}
}
