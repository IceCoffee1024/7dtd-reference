using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A83 RID: 6787
	public interface ISectionLayer : IPlayable, ILayerable, IConfigurable, ICleanable
	{
		// Token: 0x0600CD00 RID: 52480
		void SetParentSection(SectionType _sectionType);
	}
}
