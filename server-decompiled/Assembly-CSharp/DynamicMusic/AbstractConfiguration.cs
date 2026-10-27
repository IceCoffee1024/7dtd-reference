using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A7D RID: 6781
	public abstract class AbstractConfiguration : IConfiguration
	{
		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x0600CD84 RID: 52612 RVA: 0x004ADB4C File Offset: 0x004ABD4C
		// (set) Token: 0x0600CD85 RID: 52613 RVA: 0x004ADB54 File Offset: 0x004ABD54
		public virtual IList<SectionType> Sections
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CD86 RID: 52614
		public abstract int CountFor(LayerType _layer);

		// Token: 0x0600CD87 RID: 52615 RVA: 0x004ADB5D File Offset: 0x004ABD5D
		public AbstractConfiguration()
		{
			this.Sections = new List<SectionType>();
			AbstractConfiguration.AllConfigurations.Add(this);
		}

		// Token: 0x0600CD88 RID: 52616 RVA: 0x004ADB7B File Offset: 0x004ABD7B
		public static AbstractConfiguration CreateWrapper(string _type)
		{
			return (AbstractConfiguration)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CD89 RID: 52617 RVA: 0x004ADB94 File Offset: 0x004ABD94
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			foreach (string text in _xmlNode.GetAttribute("sections").Split(',', StringSplitOptions.None))
			{
				this.Sections.Add(EnumUtils.Parse<SectionType>(text, false));
			}
		}

		// Token: 0x0600CD8A RID: 52618 RVA: 0x004ADBE0 File Offset: 0x004ABDE0
		public static T Get<T>(SectionType _sectionType) where T : IConfiguration
		{
			List<T> list = AbstractConfiguration.AllConfigurations.OfType<T>().ToList<T>().FindAll((T c) => c.Sections.Contains(_sectionType));
			if (list.Count <= 0)
			{
				return default(T);
			}
			return list[AbstractConfiguration.rng.RandomRange(list.Count)];
		}

		// Token: 0x0600CD8B RID: 52619 RVA: 0x004ADC44 File Offset: 0x004ABE44
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

		// Token: 0x04009C35 RID: 39989
		public static IList<AbstractConfiguration> AllConfigurations = new List<AbstractConfiguration>();

		// Token: 0x04009C36 RID: 39990
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();
	}
}
