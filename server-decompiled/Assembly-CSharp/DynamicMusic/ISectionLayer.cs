using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AE0 RID: 6880
	public interface ISectionLayer : IPlayable, ILayerable, IConfigurable, ICleanable
	{
		// Token: 0x0600CF6C RID: 53100
		void SetParentSection(SectionType _sectionType);
	}
}
