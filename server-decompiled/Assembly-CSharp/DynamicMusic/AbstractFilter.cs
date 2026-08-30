using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x020019FE RID: 6654
	public abstract class AbstractFilter : IFilter<SectionType>
	{
		// Token: 0x0600C9A0 RID: 51616
		public abstract List<SectionType> Filter(List<SectionType> _list);

		// Token: 0x0600C9A1 RID: 51617 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractFilter()
		{
		}
	}
}
