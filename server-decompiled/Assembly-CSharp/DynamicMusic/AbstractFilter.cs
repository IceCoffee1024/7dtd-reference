using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A51 RID: 6737
	public abstract class AbstractFilter : IFilter<SectionType>
	{
		// Token: 0x0600CC81 RID: 52353
		public abstract List<SectionType> Filter(List<SectionType> _list);

		// Token: 0x0600CC82 RID: 52354 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractFilter()
		{
		}
	}
}
