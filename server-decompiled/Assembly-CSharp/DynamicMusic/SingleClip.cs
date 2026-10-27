using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AA1 RID: 6817
	[Preserve]
	public class SingleClip : Content
	{
		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x0600CE57 RID: 52823 RVA: 0x004AFF70 File Offset: 0x004AE170
		// (set) Token: 0x0600CE58 RID: 52824 RVA: 0x004AFF78 File Offset: 0x004AE178
		public AudioClip Clip
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE59 RID: 52825 RVA: 0x004AFF81 File Offset: 0x004AE181
		public override void Unload()
		{
			this.Clip.UnloadAudioData();
			this.IsLoaded = false;
		}

		// Token: 0x0600CE5A RID: 52826 RVA: 0x004AFF96 File Offset: 0x004AE196
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

		// Token: 0x0600CE5B RID: 52827 RVA: 0x004AFFA5 File Offset: 0x004AE1A5
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.GetAttribute("section"), false);
			this.path = _xmlNode.GetAttribute("path");
		}

		// Token: 0x04009CA9 RID: 40105
		[PublicizedFrom(EAccessModifier.Private)]
		public string path;
	}
}
