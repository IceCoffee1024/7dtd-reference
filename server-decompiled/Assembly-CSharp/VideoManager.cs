using System;
using System.Collections.Generic;

// Token: 0x02000CAF RID: 3247
public class VideoManager
{
	// Token: 0x060063C9 RID: 25545 RVA: 0x002723C6 File Offset: 0x002705C6
	public static void Init()
	{
		VideoManager.initialized = true;
		VideoManager.videos = new Dictionary<string, VideoData>();
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x002723D8 File Offset: 0x002705D8
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

	// Token: 0x060063CB RID: 25547 RVA: 0x00272428 File Offset: 0x00270628
	public static VideoData GetVideoData(string id)
	{
		VideoData videoData;
		if (VideoManager.videos.TryGetValue(id, out videoData))
		{
			return videoData;
		}
		return null;
	}

	// Token: 0x04004D9F RID: 19871
	public static Dictionary<string, VideoData> videos;

	// Token: 0x04004DA0 RID: 19872
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool initialized;
}
