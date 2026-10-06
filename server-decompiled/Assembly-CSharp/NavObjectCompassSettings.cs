using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000757 RID: 1879
public class NavObjectCompassSettings : NavObjectSettings
{
	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06003833 RID: 14387 RVA: 0x00170456 File Offset: 0x0016E656
	public bool ShowVerticalCompassIcons
	{
		get
		{
			return this.UpSpriteName != "" || this.DownSpriteName != "";
		}
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0017047C File Offset: 0x0016E67C
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

	// Token: 0x04002DFA RID: 11770
	public bool IconClamped;

	// Token: 0x04002DFB RID: 11771
	public float MinCompassIconScale = 0.5f;

	// Token: 0x04002DFC RID: 11772
	public float MaxCompassIconScale = 1.25f;

	// Token: 0x04002DFD RID: 11773
	public float MaxScaleDistance = -1f;

	// Token: 0x04002DFE RID: 11774
	public float MinFadePercent = -1f;

	// Token: 0x04002DFF RID: 11775
	public string UpSpriteName = "";

	// Token: 0x04002E00 RID: 11776
	public string DownSpriteName = "";

	// Token: 0x04002E01 RID: 11777
	public float ShowUpOffset = 3f;

	// Token: 0x04002E02 RID: 11778
	public float ShowDownOffset = -2f;

	// Token: 0x04002E03 RID: 11779
	public int DepthOffset;

	// Token: 0x04002E04 RID: 11780
	public NavObjectCompassSettings.HotZoneSettings HotZone;

	// Token: 0x02000758 RID: 1880
	public class HotZoneSettings
	{
		// Token: 0x04002E05 RID: 11781
		public NavObjectCompassSettings.HotZoneSettings.HotZoneTypes HotZoneType;

		// Token: 0x04002E06 RID: 11782
		public string SpriteName = "";

		// Token: 0x04002E07 RID: 11783
		public Color Color = Color.white;

		// Token: 0x04002E08 RID: 11784
		public float CustomDistance = -1f;

		// Token: 0x02000759 RID: 1881
		public enum HotZoneTypes
		{
			// Token: 0x04002E0A RID: 11786
			None,
			// Token: 0x04002E0B RID: 11787
			Treasure,
			// Token: 0x04002E0C RID: 11788
			Custom
		}
	}
}
