using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AAF RID: 6831
	public interface ISectionLayer : IPlayable, ILayerable, IConfigurable, ICleanable
	{
		// Token: 0x0600CE58 RID: 52824
		void SetParentSection(SectionType _sectionType);
	}
}
