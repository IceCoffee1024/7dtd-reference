using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A82 RID: 6786
	public abstract class AbstractFilter : IFilter<SectionType>
	{
		// Token: 0x0600CD95 RID: 52629
		public abstract List<SectionType> Filter(List<SectionType> _list);

		// Token: 0x0600CD96 RID: 52630 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractFilter()
		{
		}
	}
}
