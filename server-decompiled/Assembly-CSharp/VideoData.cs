using System;
using System.Collections.Generic;

// Token: 0x02000CC6 RID: 3270
public class VideoData
{
	// Token: 0x060063E0 RID: 25568 RVA: 0x00276CDF File Offset: 0x00274EDF
	public VideoData()
	{
		this.subtitles = new List<VideoSubtitle>();
	}

	// Token: 0x04004DF3 RID: 19955
	public string name;

	// Token: 0x04004DF4 RID: 19956
	public string url;

	// Token: 0x04004DF5 RID: 19957
	public float defaultSubtitleDuration;

	// Token: 0x04004DF6 RID: 19958
	public List<VideoSubtitle> subtitles;
}
