using System;
using System.Collections.Generic;

// Token: 0x02000C9D RID: 3229
public class VideoManager
{
	// Token: 0x0600633D RID: 25405 RVA: 0x0026F4CA File Offset: 0x0026D6CA
	public static void Init()
	{
		VideoManager.initialized = true;
		VideoManager.videos = new Dictionary<string, VideoData>();
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x0026F4DC File Offset: 0x0026D6DC
	public static void AddVideo(VideoData data)
	{
		if (!VideoManager.initialized)
		{
			VideoManager.Init();
		}
		if (!VideoManager.videos.ContainsKey(data.name))
		{
			VideoManager.videos.Add(data.name, data);
			return;
		}
		VideoManager.videos[data.name] = data;
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x0026F52C File Offset: 0x0026D72C
	public static VideoData GetVideoData(string id)
	{
		VideoData videoData;
		if (VideoManager.videos.TryGetValue(id, out videoData))
		{
			return videoData;
		}
		return null;
	}

	// Token: 0x04004D01 RID: 19713
	public static Dictionary<string, VideoData> videos;

	// Token: 0x04004D02 RID: 19714
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool initialized;
}
