using System;
using System.Collections.Generic;

namespace DynamicMusic
{
	// Token: 0x02001A4B RID: 6731
	public interface IFilter<T>
	{
		// Token: 0x0600CB5A RID: 52058
		List<T> Filter(List<T> _list);
	}
}
