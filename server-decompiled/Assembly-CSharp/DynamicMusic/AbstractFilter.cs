using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A25 RID: 6693
	public abstract class AbstractFilter : IFilter<SectionType>
	{
		// Token: 0x0600CB29 RID: 52009
		public abstract List<SectionType> Filter(List<SectionType> _list);

		// Token: 0x0600CB2A RID: 52010 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractFilter()
		{
		}
	}
}
