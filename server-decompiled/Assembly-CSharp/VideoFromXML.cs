using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D5C RID: 3420
public class VideoFromXML
{
	// Token: 0x0600676B RID: 26475 RVA: 0x00292198 File Offset: 0x00290398
	public static IEnumerator CreateVideos(XmlFile xmlFile)
	{
		XElement root = xmlFile.XmlDoc.Root;
		bool hasElements = root.HasElements;
		VideoManager.Init();
		VideoFromXML.Parse(root);
		yield break;
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x002921A8 File Offset: 0x002903A8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Parse(XElement root)
	{
		foreach (XElement xelement in root.Elements("Video"))
		{
			VideoData videoData = new VideoData();
			videoData.name = xelement.Attribute("name").Value;
			videoData.url = xelement.Attribute("path").Value;
			if (xelement.HasAttribute("defaultSubtitleDuration"))
			{
				videoData.defaultSubtitleDuration = float.Parse(xelement.Attribute("defaultSubtitleDuration").Value);
			}
			foreach (XElement xelement2 in xelement.Elements("Subtitle"))
			{
				VideoSubtitle videoSubtitle = default(VideoSubtitle);
				videoSubtitle.timestamp = double.Parse(xelement2.GetAttribute("timestamp"));
				videoSubtitle.subtitleId = xelement2.GetAttribute("id");
				if (xelement2.HasAttribute("duration"))
				{
					videoSubtitle.duration = float.Parse(xelement2.GetAttribute("duration"));
				}
				else
				{
					videoSubtitle.duration = videoData.defaultSubtitleDuration;
				}
				videoData.subtitles.Add(videoSubtitle);
			}
			VideoManager.AddVideo(videoData);
		}
	}
}
