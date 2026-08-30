using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A00 RID: 6656
	[Preserve]
	public abstract class Content
	{
		// Token: 0x0600C9A3 RID: 51619 RVA: 0x0049F947 File Offset: 0x0049DB47
		public Content()
		{
			Content.AllContent.Add(this);
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x0600C9A4 RID: 51620 RVA: 0x0049F95A File Offset: 0x0049DB5A
		// (set) Token: 0x0600C9A5 RID: 51621 RVA: 0x0049F962 File Offset: 0x0049DB62
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x0600C9A6 RID: 51622 RVA: 0x0049F96B File Offset: 0x0049DB6B
		// (set) Token: 0x0600C9A7 RID: 51623 RVA: 0x0049F973 File Offset: 0x0049DB73
		public SectionType Section
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x0600C9A8 RID: 51624 RVA: 0x0049F97C File Offset: 0x0049DB7C
		// (set) Token: 0x0600C9A9 RID: 51625 RVA: 0x0049F984 File Offset: 0x0049DB84
		public virtual bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600C9AA RID: 51626
		[Preserve]
		public abstract IEnumerator Load();

		// Token: 0x0600C9AB RID: 51627
		[Preserve]
		public abstract void Unload();

		// Token: 0x0600C9AC RID: 51628 RVA: 0x0049F98D File Offset: 0x0049DB8D
		[Preserve]
		public static Content CreateWrapper(string _type)
		{
			return (Content)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600C9AD RID: 51629 RVA: 0x0049F9A4 File Offset: 0x0049DBA4
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			this.Name = _xmlNode.GetAttribute("name");
		}

		// Token: 0x0400996C RID: 39276
		public static Dictionary<SectionType, int> SamplesFor = new Dictionary<SectionType, int>();

		// Token: 0x0400996D RID: 39277
		public static Dictionary<SectionType, string> SourcePathFor = new Dictionary<SectionType, string>();

		// Token: 0x0400996E RID: 39278
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x0400996F RID: 39279
		public static List<Content> AllContent = new List<Content>();
	}
}
