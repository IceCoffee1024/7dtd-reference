using System;

namespace DynamicMusic
{
	// Token: 0x02001AD6 RID: 6870
	public interface INotifiable<T>
	{
		// Token: 0x0600CF58 RID: 53080
		void Notify(T _state);
	}
}
