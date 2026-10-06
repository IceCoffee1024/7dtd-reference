using System;
using System.Collections.Generic;

namespace DynamicMusic
{
	// Token: 0x02001A9E RID: 6814
	public interface IFilter<T>
	{
		// Token: 0x0600CE3B RID: 52795
		List<T> Filter(List<T> _list);
	}
}
