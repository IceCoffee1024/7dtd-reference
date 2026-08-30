using System;

namespace DynamicMusic
{
	// Token: 0x02001A52 RID: 6738
	public interface INotifiable<T>
	{
		// Token: 0x0600CB63 RID: 52067
		void Notify(T _state);
	}
}
