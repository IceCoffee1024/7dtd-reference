using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D2F RID: 3375
public class VideoFromXML
{
	// Token: 0x060066C0 RID: 26304 RVA: 0x0028A824 File Offset: 0x00288A24
	public static IEnumerator CreateVideos(XmlFile xmlFile)
	{
		XElement root = xmlFile.XmlDoc.Root;
		bool hasElements = root.HasElements;
		VideoManager.Init();
		VideoFromXML.Parse(root);
		yield break;
	}

	// Token: 0x060066C1 RID: 26305 RVA: 0x0028A834 File Offset: 0x00288A34
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
