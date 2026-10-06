using System;

namespace DynamicMusic
{
	// Token: 0x02001AA5 RID: 6821
	public interface INotifiable<T>
	{
		// Token: 0x0600CE44 RID: 52804
		void Notify(T _state);
	}
}
