using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x020019F9 RID: 6649
	public abstract class AbstractConfiguration : IConfiguration
	{
		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x0600C98F RID: 51599 RVA: 0x0049F790 File Offset: 0x0049D990
		// (set) Token: 0x0600C990 RID: 51600 RVA: 0x0049F798 File Offset: 0x0049D998
		public virtual IList<SectionType> Sections
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600C991 RID: 51601
		public abstract int CountFor(LayerType _layer);

		// Token: 0x0600C992 RID: 51602 RVA: 0x0049F7A1 File Offset: 0x0049D9A1
		public AbstractConfiguration()
		{
			this.Sections = new List<SectionType>();
			AbstractConfiguration.AllConfigurations.Add(this);
		}

		// Token: 0x0600C993 RID: 51603 RVA: 0x0049F7BF File Offset: 0x0049D9BF
		public static AbstractConfiguration CreateWrapper(string _type)
		{
			return (AbstractConfiguration)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600C994 RID: 51604 RVA: 0x0049F7D8 File Offset: 0x0049D9D8
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			foreach (string text in _xmlNode.GetAttribute("sections").Split(',', StringSplitOptions.None))
			{
				this.Sections.Add(EnumUtils.Parse<SectionType>(text, false));
			}
		}

		// Token: 0x0600C995 RID: 51605 RVA: 0x0049F824 File Offset: 0x0049DA24
		public static T Get<T>(SectionType _sectionType) where T : IConfiguration
		{
			List<T> list = AbstractConfiguration.AllConfigurations.OfType<T>().ToList<T>().FindAll((T c) => c.Sections.Contains(_sectionType));
			if (list.Count <= 0)
			{
				return default(T);
			}
			return list[AbstractConfiguration.rng.RandomRange(list.Count)];
		}

		// Token: 0x0600C996 RID: 51606 RVA: 0x0049F888 File Offset: 0x0049DA88
		public static int GetBufferSize(SectionType _sectionType, LayerType _layerType)
		{
			IEnumerable<int> enumerable = from c in AbstractConfiguration.AllConfigurations.OfType<IConfiguration>()
				where c.Sections.Contains(_sectionType)
				select c.CountFor(_layerType);
			if (enumerable != null && enumerable.Count<int>() != 0)
			{
				return enumerable.Max();
			}
			return 0;
		}

		// Token: 0x04009955 RID: 39253
		public static IList<AbstractConfiguration> AllConfigurations = new List<AbstractConfiguration>();

		// Token: 0x04009956 RID: 39254
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();
	}
}
