using System;
using System.Collections.Generic;

// Token: 0x02000CBD RID: 3261
public class VideoManager
{
	// Token: 0x060063BC RID: 25532 RVA: 0x002757CA File Offset: 0x002739CA
	public static void Init()
	{
		VideoManager.initialized = true;
		VideoManager.videos = new Dictionary<string, VideoData>();
	}

	// Token: 0x060063BD RID: 25533 RVA: 0x002757DC File Offset: 0x002739DC
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

	// Token: 0x060063BE RID: 25534 RVA: 0x0027582C File Offset: 0x00273A2C
	public static VideoData GetVideoData(string id)
	{
		VideoData videoData;
		if (VideoManager.videos.TryGetValue(id, out videoData))
		{
			return videoData;
		}
		return null;
	}

	// Token: 0x04004DD5 RID: 19925
	public static Dictionary<string, VideoData> videos;

	// Token: 0x04004DD6 RID: 19926
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool initialized;
}
