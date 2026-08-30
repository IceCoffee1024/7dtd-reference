using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class NavObjectCompassSettings : NavObjectSettings
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x060037F0 RID: 14320 RVA: 0x0016FAF2 File Offset: 0x0016DCF2
	public bool ShowVerticalCompassIcons
	{
		get
		{
			return this.UpSpriteName != "" || this.DownSpriteName != "";
		}
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x0016FB18 File Offset: 0x0016DD18
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

	// Token: 0x04002E01 RID: 11777
	public bool IconClamped;

	// Token: 0x04002E02 RID: 11778
	public float MinCompassIconScale = 0.5f;

	// Token: 0x04002E03 RID: 11779
	public float MaxCompassIconScale = 1.25f;

	// Token: 0x04002E04 RID: 11780
	public float MaxScaleDistance = -1f;

	// Token: 0x04002E05 RID: 11781
	public float MinFadePercent = -1f;

	// Token: 0x04002E06 RID: 11782
	public string UpSpriteName = "";

	// Token: 0x04002E07 RID: 11783
	public string DownSpriteName = "";

	// Token: 0x04002E08 RID: 11784
	public float ShowUpOffset = 3f;

	// Token: 0x04002E09 RID: 11785
	public float ShowDownOffset = -2f;

	// Token: 0x04002E0A RID: 11786
	public int DepthOffset;

	// Token: 0x04002E0B RID: 11787
	public NavObjectCompassSettings.HotZoneSettings HotZone;

	// Token: 0x02000756 RID: 1878
	public class HotZoneSettings
	{
		// Token: 0x04002E0C RID: 11788
		public NavObjectCompassSettings.HotZoneSettings.HotZoneTypes HotZoneType;

		// Token: 0x04002E0D RID: 11789
		public string SpriteName = "";

		// Token: 0x04002E0E RID: 11790
		public Color Color = Color.white;

		// Token: 0x04002E0F RID: 11791
		public float CustomDistance = -1f;

		// Token: 0x02000757 RID: 1879
		public enum HotZoneTypes
		{
			// Token: 0x04002E11 RID: 11793
			None,
			// Token: 0x04002E12 RID: 11794
			Treasure,
			// Token: 0x04002E13 RID: 11795
			Custom
		}
	}
}
