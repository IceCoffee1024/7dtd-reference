using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A9B RID: 6811
	public interface IConfiguration<T> : IConfiguration
	{
		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600CE37 RID: 52791
		Dictionary<LayerType, T> Layers { get; }
	}
}
