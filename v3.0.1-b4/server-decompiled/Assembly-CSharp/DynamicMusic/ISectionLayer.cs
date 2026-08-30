using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A5C RID: 6748
	public interface ISectionLayer : IPlayable, ILayerable, IConfigurable, ICleanable
	{
		// Token: 0x0600CB77 RID: 52087
		void SetParentSection(SectionType _sectionType);
	}
}
