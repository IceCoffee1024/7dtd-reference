using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200074B RID: 1867
public class NavObjectCompassSettings : NavObjectSettings
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x0600379A RID: 14234 RVA: 0x0016D8D6 File Offset: 0x0016BAD6
	public bool ShowVerticalCompassIcons
	{
		get
		{
			return this.UpSpriteName != "" || this.DownSpriteName != "";
		}
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0016D8FC File Offset: 0x0016BAFC
	public override void Init()
	{
		base.Init();
		this.Properties.ParseBool("icon_clamped", ref this.IconClamped);
		this.Properties.ParseFloat("min_icon_scale", ref this.MinCompassIconScale);
		this.Properties.ParseFloat("max_icon_scale", ref this.MaxCompassIconScale);
		this.Properties.ParseFloat("min_fade_percent", ref this.MinFadePercent);
		this.MaxScaleDistance = this.MaxDistance;
		this.Properties.ParseFloat("max_scale_distance", ref this.MaxScaleDistance);
		this.Properties.ParseString("up_sprite_name", ref this.UpSpriteName);
		this.Properties.ParseString("down_sprite_name", ref this.DownSpriteName);
		this.Properties.ParseFloat("show_up_offset", ref this.ShowUpOffset);
		this.Properties.ParseFloat("show_down_offset", ref this.ShowDownOffset);
		if (this.Properties.Values.ContainsKey("hot_zone_type"))
		{
			NavObjectCompassSettings.HotZoneSettings.HotZoneTypes hotZoneTypes = NavObjectCompassSettings.HotZoneSettings.HotZoneTypes.None;
			if (!Enum.TryParse<NavObjectCompassSettings.HotZoneSettings.HotZoneTypes>(this.Properties.Values["hot_zone_type"], out hotZoneTypes))
			{
				hotZoneTypes = NavObjectCompassSettings.HotZoneSettings.HotZoneTypes.None;
			}
			if (hotZoneTypes != NavObjectCompassSettings.HotZoneSettings.HotZoneTypes.None)
			{
				this.HotZone = new NavObjectCompassSettings.HotZoneSettings();
				this.HotZone.HotZoneType = hotZoneTypes;
				this.Properties.ParseString("hot_zone_sprite", ref this.HotZone.SpriteName);
				if (this.Properties.Values.ContainsKey("hot_zone_sprite"))
				{
					this.HotZone.SpriteName = this.Properties.Values["hot_zone_sprite"];
				}
				if (this.Properties.Values.ContainsKey("hot_zone_color"))
				{
					this.HotZone.Color = StringParsers.ParseColor32(this.Properties.Values["hot_zone_color"]);
				}
				if (this.Properties.Values.ContainsKey("hot_zone_distance"))
				{
					this.HotZone.CustomDistance = StringParsers.ParseFloat(this.Properties.Values["hot_zone_distance"], 0, -1, NumberStyles.Any);
				}
			}
		}
		if (this.Properties.Values.ContainsKey("depth_offset"))
		{
			this.DepthOffset = StringParsers.ParseSInt32(this.Properties.Values["depth_offset"], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x04002D89 RID: 11657
	public bool IconClamped;

	// Token: 0x04002D8A RID: 11658
	public float MinCompassIconScale = 0.5f;

	// Token: 0x04002D8B RID: 11659
	public float MaxCompassIconScale = 1.25f;

	// Token: 0x04002D8C RID: 11660
	public float MaxScaleDistance = -1f;

	// Token: 0x04002D8D RID: 11661
	public float MinFadePercent = -1f;

	// Token: 0x04002D8E RID: 11662
	public string UpSpriteName = "";

	// Token: 0x04002D8F RID: 11663
	public string DownSpriteName = "";

	// Token: 0x04002D90 RID: 11664
	public float ShowUpOffset = 3f;

	// Token: 0x04002D91 RID: 11665
	public float ShowDownOffset = -2f;

	// Token: 0x04002D92 RID: 11666
	public int DepthOffset;

	// Token: 0x04002D93 RID: 11667
	public NavObjectCompassSettings.HotZoneSettings HotZone;

	// Token: 0x0200074C RID: 1868
	public class HotZoneSettings
	{
		// Token: 0x04002D94 RID: 11668
		public NavObjectCompassSettings.HotZoneSettings.HotZoneTypes HotZoneType;

		// Token: 0x04002D95 RID: 11669
		public string SpriteName = "";

		// Token: 0x04002D96 RID: 11670
		public Color Color = Color.white;

		// Token: 0x04002D97 RID: 11671
		public float CustomDistance = -1f;

		// Token: 0x0200074D RID: 1869
		public enum HotZoneTypes
		{
			// Token: 0x04002D99 RID: 11673
			None,
			// Token: 0x04002D9A RID: 11674
			Treasure,
			// Token: 0x04002D9B RID: 11675
			Custom
		}
	}
}
