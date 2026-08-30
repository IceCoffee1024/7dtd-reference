using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A48 RID: 6728
	public interface IConfiguration<T> : IConfiguration
	{
		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600CB56 RID: 52054
		Dictionary<LayerType, T> Layers { get; }
	}
}
