using System;

namespace DynamicMusic
{
	// Token: 0x02001AD9 RID: 6873
	[PublicizedFrom(EAccessModifier.Internal)]
	public interface INotifiableSelector<T1, T2> : INotifiable<T1>, ISelector<T2>
	{
	}
}
