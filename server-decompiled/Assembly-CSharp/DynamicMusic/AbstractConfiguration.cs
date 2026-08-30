using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A20 RID: 6688
	public abstract class AbstractConfiguration : IConfiguration
	{
		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x0600CB18 RID: 51992 RVA: 0x004A5B84 File Offset: 0x004A3D84
		// (set) Token: 0x0600CB19 RID: 51993 RVA: 0x004A5B8C File Offset: 0x004A3D8C
		public virtual IList<SectionType> Sections
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CB1A RID: 51994
		public abstract int CountFor(LayerType _layer);

		// Token: 0x0600CB1B RID: 51995 RVA: 0x004A5B95 File Offset: 0x004A3D95
		public AbstractConfiguration()
		{
			this.Sections = new List<SectionType>();
			AbstractConfiguration.AllConfigurations.Add(this);
		}

		// Token: 0x0600CB1C RID: 51996 RVA: 0x004A5BB3 File Offset: 0x004A3DB3
		public static AbstractConfiguration CreateWrapper(string _type)
		{
			return (AbstractConfiguration)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CB1D RID: 51997 RVA: 0x004A5BCC File Offset: 0x004A3DCC
		public virtual void ParseFromXml(XElement _xmlNode)
		{
			foreach (string text in _xmlNode.GetAttribute("sections").Split(',', StringSplitOptions.None))
			{
				this.Sections.Add(EnumUtils.Parse<SectionType>(text, false));
			}
		}

		// Token: 0x0600CB1E RID: 51998 RVA: 0x004A5C18 File Offset: 0x004A3E18
		public static T Get<T>(SectionType _sectionType) where T : IConfiguration
		{
			List<T> list = AbstractConfiguration.AllConfigurations.OfType<T>().ToList<T>().FindAll((T c) => c.Sections.Contains(_sectionType));
			if (list.Count <= 0)
			{
				return default(T);
			}
			return list[AbstractConfiguration.rng.RandomRange(list.Count)];
		}

		// Token: 0x0600CB1F RID: 51999 RVA: 0x004A5C7C File Offset: 0x004A3E7C
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

		// Token: 0x04009A86 RID: 39558
		public static IList<AbstractConfiguration> AllConfigurations = new List<AbstractConfiguration>();

		// Token: 0x04009A87 RID: 39559
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();
	}
}
