using System;
using System.Collections.Generic;

// Token: 0x02000CC8 RID: 3272
public class VideoManager
{
	// Token: 0x060063E1 RID: 25569 RVA: 0x00276CF2 File Offset: 0x00274EF2
	public static void Init()
	{
		VideoManager.initialized = true;
		VideoManager.videos = new Dictionary<string, VideoData>();
	}

	// Token: 0x060063E2 RID: 25570 RVA: 0x00276D04 File Offset: 0x00274F04
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

	// Token: 0x060063E3 RID: 25571 RVA: 0x00276D54 File Offset: 0x00274F54
	public static VideoData GetVideoData(string id)
	{
		VideoData videoData;
		if (VideoManager.videos.TryGetValue(id, out videoData))
		{
			return videoData;
		}
		return null;
	}

	// Token: 0x04004DFA RID: 19962
	public static Dictionary<string, VideoData> videos;

	// Token: 0x04004DFB RID: 19963
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool initialized;
}
