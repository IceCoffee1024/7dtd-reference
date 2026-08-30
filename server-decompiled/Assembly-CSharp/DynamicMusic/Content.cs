using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A27 RID: 6695
	[Preserve]
	public abstract class Content
	{
		// Token: 0x0600CB2C RID: 52012 RVA: 0x004A5D3B File Offset: 0x004A3F3B
		public Content()
		{
			Content.AllContent.Add(this);
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x0600CB2D RID: 52013 RVA: 0x004A5D4E File Offset: 0x004A3F4E
		// (set) Token: 0x0600CB2E RID: 52014 RVA: 0x004A5D56 File Offset: 0x004A3F56
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x0600CB2F RID: 52015 RVA: 0x004A5D5F File Offset: 0x004A3F5F
		// (set) Token: 0x0600CB30 RID: 52016 RVA: 0x004A5D67 File Offset: 0x004A3F67
		public SectionType Section
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x0600CB31 RID: 52017 RVA: 0x004A5D70 File Offset: 0x004A3F70
		// (set) Token: 0x0600CB32 RID: 52018 RVA: 0x004A5D78 File Offset: 0x004A3F78
		public virtual bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CB33 RID: 52019
		[Preserve]
		public abstract IEnumerator Load();

		// Token: 0x0600CB34 RID: 52020
		[Preserve]
		public abstract void Unload();

		// Token: 0x0600CB35 RID: 52021 RVA: 0x004A5D81 File Offset: 0x004A3F81
		[Preserve]
		public static Content CreateWrapper(string _type)
		{
			return (Content)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CB36 RID: 52022 RVA: 0x004A5D98 File Offset: 0x004A3F98
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			this.Name = _xmlNode.GetAttribute("name");
		}

		// Token: 0x04009A9D RID: 39581
		public static Dictionary<SectionType, int> SamplesFor = new Dictionary<SectionType, int>();

		// Token: 0x04009A9E RID: 39582
		public static Dictionary<SectionType, string> SourcePathFor = new Dictionary<SectionType, string>();

		// Token: 0x04009A9F RID: 39583
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009AA0 RID: 39584
		public static List<Content> AllContent = new List<Content>();
	}
}
