using System;
using System.Collections.Generic;

// Token: 0x02000CAD RID: 3245
public class VideoData
{
	// Token: 0x060063C8 RID: 25544 RVA: 0x002723B3 File Offset: 0x002705B3
	public VideoData()
	{
		this.subtitles = new List<VideoSubtitle>();
	}

	// Token: 0x04004D98 RID: 19864
	public string name;

	// Token: 0x04004D99 RID: 19865
	public string url;

	// Token: 0x04004D9A RID: 19866
	public float defaultSubtitleDuration;

	// Token: 0x04004D9B RID: 19867
	public List<VideoSubtitle> subtitles;
}
