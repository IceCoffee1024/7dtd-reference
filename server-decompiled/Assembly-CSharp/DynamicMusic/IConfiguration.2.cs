using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001ACC RID: 6860
	public interface IConfiguration<T> : IConfiguration
	{
		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x0600CF4B RID: 53067
		Dictionary<LayerType, T> Layers { get; }
	}
}
