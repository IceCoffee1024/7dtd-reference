using System;

namespace DynamicMusic
{
	// Token: 0x02001A79 RID: 6777
	public interface INotifiable<T>
	{
		// Token: 0x0600CCEC RID: 52460
		void Notify(T _state);
	}
}
