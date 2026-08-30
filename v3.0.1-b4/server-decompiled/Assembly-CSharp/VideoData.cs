using System;
using System.Collections.Generic;

// Token: 0x02000C9B RID: 3227
public class VideoData
{
	// Token: 0x0600633C RID: 25404 RVA: 0x0026F4B7 File Offset: 0x0026D6B7
	public VideoData()
	{
		this.subtitles = new List<VideoSubtitle>();
	}

	// Token: 0x04004CFA RID: 19706
	public string name;

	// Token: 0x04004CFB RID: 19707
	public string url;

	// Token: 0x04004CFC RID: 19708
	public float defaultSubtitleDuration;

	// Token: 0x04004CFD RID: 19709
	public List<VideoSubtitle> subtitles;
}
