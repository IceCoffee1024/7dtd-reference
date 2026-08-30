using System;

namespace DynamicMusic
{
	// Token: 0x02001A7B RID: 6779
	public interface INotifiableFilter<T1, T2> : INotifiable<T1>, IFilter<T2>
	{
	}
}
