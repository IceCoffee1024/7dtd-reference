using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A6F RID: 6767
	public interface IConfiguration<T> : IConfiguration
	{
		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x0600CCDF RID: 52447
		Dictionary<LayerType, T> Layers { get; }
	}
}
