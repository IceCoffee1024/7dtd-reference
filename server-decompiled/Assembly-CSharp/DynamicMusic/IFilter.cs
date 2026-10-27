using System;
using System.Collections.Generic;

namespace DynamicMusic
{
	// Token: 0x02001ACF RID: 6863
	public interface IFilter<T>
	{
		// Token: 0x0600CF4F RID: 53071
		List<T> Filter(List<T> _list);
	}
}
