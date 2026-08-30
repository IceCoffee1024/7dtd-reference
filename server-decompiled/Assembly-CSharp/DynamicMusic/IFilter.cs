using System;
using System.Collections.Generic;

namespace DynamicMusic
{
	// Token: 0x02001A72 RID: 6770
	public interface IFilter<T>
	{
		// Token: 0x0600CCE3 RID: 52451
		List<T> Filter(List<T> _list);
	}
}
