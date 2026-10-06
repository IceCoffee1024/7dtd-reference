using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A53 RID: 6739
	[Preserve]
	public abstract class Content
	{
		// Token: 0x0600CC84 RID: 52356 RVA: 0x004AA207 File Offset: 0x004A8407
		public Content()
		{
			Content.AllContent.Add(this);
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x0600CC85 RID: 52357 RVA: 0x004AA21A File Offset: 0x004A841A
		// (set) Token: 0x0600CC86 RID: 52358 RVA: 0x004AA222 File Offset: 0x004A8422
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600CC87 RID: 52359 RVA: 0x004AA22B File Offset: 0x004A842B
		// (set) Token: 0x0600CC88 RID: 52360 RVA: 0x004AA233 File Offset: 0x004A8433
		public SectionType Section
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600CC89 RID: 52361 RVA: 0x004AA23C File Offset: 0x004A843C
		// (set) Token: 0x0600CC8A RID: 52362 RVA: 0x004AA244 File Offset: 0x004A8444
		public virtual bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CC8B RID: 52363
		[Preserve]
		public abstract IEnumerator Load();

		// Token: 0x0600CC8C RID: 52364
		[Preserve]
		public abstract void Unload();

		// Token: 0x0600CC8D RID: 52365 RVA: 0x004AA24D File Offset: 0x004A844D
		[Preserve]
		public static Content CreateWrapper(string _type)
		{
			return (Content)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CC8E RID: 52366 RVA: 0x004AA264 File Offset: 0x004A8464
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			this.Name = _xmlNode.GetAttribute("name");
		}

		// Token: 0x04009B63 RID: 39779
		public static Dictionary<SectionType, int> SamplesFor = new Dictionary<SectionType, int>();

		// Token: 0x04009B64 RID: 39780
		public static Dictionary<SectionType, string> SourcePathFor = new Dictionary<SectionType, string>();

		// Token: 0x04009B65 RID: 39781
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009B66 RID: 39782
		public static List<Content> AllContent = new List<Content>();
	}
}
