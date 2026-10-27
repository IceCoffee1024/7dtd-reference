using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A84 RID: 6788
	[Preserve]
	public abstract class Content
	{
		// Token: 0x0600CD98 RID: 52632 RVA: 0x004ADD03 File Offset: 0x004ABF03
		public Content()
		{
			Content.AllContent.Add(this);
		}

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x0600CD99 RID: 52633 RVA: 0x004ADD16 File Offset: 0x004ABF16
		// (set) Token: 0x0600CD9A RID: 52634 RVA: 0x004ADD1E File Offset: 0x004ABF1E
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600CD9B RID: 52635 RVA: 0x004ADD27 File Offset: 0x004ABF27
		// (set) Token: 0x0600CD9C RID: 52636 RVA: 0x004ADD2F File Offset: 0x004ABF2F
		public SectionType Section
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600CD9D RID: 52637 RVA: 0x004ADD38 File Offset: 0x004ABF38
		// (set) Token: 0x0600CD9E RID: 52638 RVA: 0x004ADD40 File Offset: 0x004ABF40
		public virtual bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CD9F RID: 52639
		[Preserve]
		public abstract IEnumerator Load();

		// Token: 0x0600CDA0 RID: 52640
		[Preserve]
		public abstract void Unload();

		// Token: 0x0600CDA1 RID: 52641 RVA: 0x004ADD49 File Offset: 0x004ABF49
		[Preserve]
		public static Content CreateWrapper(string _type)
		{
			return (Content)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CDA2 RID: 52642 RVA: 0x004ADD60 File Offset: 0x004ABF60
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			this.Name = _xmlNode.GetAttribute("name");
		}

		// Token: 0x04009C4C RID: 40012
		public static Dictionary<SectionType, int> SamplesFor = new Dictionary<SectionType, int>();

		// Token: 0x04009C4D RID: 40013
		public static Dictionary<SectionType, string> SourcePathFor = new Dictionary<SectionType, string>();

		// Token: 0x04009C4E RID: 40014
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009C4F RID: 40015
		public static List<Content> AllContent = new List<Content>();
	}
}
