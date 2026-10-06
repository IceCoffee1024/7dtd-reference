using System;

namespace DynamicMusic
{
	// Token: 0x02001AA8 RID: 6824
	[PublicizedFrom(EAccessModifier.Internal)]
	public interface INotifiableSelector<T1, T2> : INotifiable<T1>, ISelector<T2>
	{
	}
}
