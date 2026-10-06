using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A4C RID: 6732
	public abstract class AbstractConfiguration : IConfiguration
	{
		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x0600CC70 RID: 52336 RVA: 0x004AA050 File Offset: 0x004A8250
		// (set) Token: 0x0600CC71 RID: 52337 RVA: 0x004AA058 File Offset: 0x004A8258
		public virtual IList<SectionType> Sections
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CC72 RID: 52338
		public abstract int CountFor(LayerType _layer);

		// Token: 0x0600CC73 RID: 52339 RVA: 0x004AA061 File Offset: 0x004A8261
		public AbstractConfiguration()
		{
			this.Sections = new List<SectionType>();
			AbstractConfiguration.AllConfigurations.Add(this);
		}

		// Token: 0x0600CC74 RID: 52340 RVA: 0x004AA07F File Offset: 0x004A827F
		public static AbstractConfiguration CreateWrapper(string _type)
		{
			return (AbstractConfiguration)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CC75 RID: 52341 RVA: 0x004AA098 File Offset: 0x004A8298
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			foreach (string text in _xmlNode.GetAttribute("sections").Split(',', StringSplitOptions.None))
			{
				this.Sections.Add(EnumUtils.Parse<SectionType>(text, false));
			}
		}

		// Token: 0x0600CC76 RID: 52342 RVA: 0x004AA0E4 File Offset: 0x004A82E4
		public static T Get<T>(SectionType _sectionType) where T : IConfiguration
		{
			List<T> list = AbstractConfiguration.AllConfigurations.OfType<T>().ToList<T>().FindAll((T c) => c.Sections.Contains(_sectionType));
			if (list.Count <= 0)
			{
				return default(T);
			}
			return list[AbstractConfiguration.rng.RandomRange(list.Count)];
		}

		// Token: 0x0600CC77 RID: 52343 RVA: 0x004AA148 File Offset: 0x004A8348
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

		// Token: 0x04009B4C RID: 39756
		public static IList<AbstractConfiguration> AllConfigurations = new List<AbstractConfiguration>();

		// Token: 0x04009B4D RID: 39757
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();
	}
}
