using System;
using System.Collections.Generic;

// Token: 0x02000CBB RID: 3259
public class VideoData
{
	// Token: 0x060063BB RID: 25531 RVA: 0x002757B7 File Offset: 0x002739B7
	public VideoData()
	{
		this.subtitles = new List<VideoSubtitle>();
	}

	// Token: 0x04004DCE RID: 19918
	public string name;

	// Token: 0x04004DCF RID: 19919
	public string url;

	// Token: 0x04004DD0 RID: 19920
	public float defaultSubtitleDuration;

	// Token: 0x04004DD1 RID: 19921
	public List<VideoSubtitle> subtitles;
}
