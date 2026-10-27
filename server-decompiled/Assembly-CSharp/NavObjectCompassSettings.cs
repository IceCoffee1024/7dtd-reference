using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200075F RID: 1887
public class NavObjectCompassSettings : NavObjectSettings
{
	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x0600384A RID: 14410 RVA: 0x001713EE File Offset: 0x0016F5EE
	public bool ShowVerticalCompassIcons
	{
		get
		{
			return this.UpSpriteName != "" || this.DownSpriteName != "";
		}
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x00171414 File Offset: 0x0016F614
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

	// Token: 0x04002E16 RID: 11798
	public bool IconClamped;

	// Token: 0x04002E17 RID: 11799
	public float MinCompassIconScale = 0.5f;

	// Token: 0x04002E18 RID: 11800
	public float MaxCompassIconScale = 1.25f;

	// Token: 0x04002E19 RID: 11801
	public float MaxScaleDistance = -1f;

	// Token: 0x04002E1A RID: 11802
	public float MinFadePercent = -1f;

	// Token: 0x04002E1B RID: 11803
	public string UpSpriteName = "";

	// Token: 0x04002E1C RID: 11804
	public string DownSpriteName = "";

	// Token: 0x04002E1D RID: 11805
	public float ShowUpOffset = 3f;

	// Token: 0x04002E1E RID: 11806
	public float ShowDownOffset = -2f;

	// Token: 0x04002E1F RID: 11807
	public int DepthOffset;

	// Token: 0x04002E20 RID: 11808
	public NavObjectCompassSettings.HotZoneSettings HotZone;

	// Token: 0x02000760 RID: 1888
	public class HotZoneSettings
	{
		// Token: 0x04002E21 RID: 11809
		public NavObjectCompassSettings.HotZoneSettings.HotZoneTypes HotZoneType;

		// Token: 0x04002E22 RID: 11810
		public string SpriteName = "";

		// Token: 0x04002E23 RID: 11811
		public Color Color = Color.white;

		// Token: 0x04002E24 RID: 11812
		public float CustomDistance = -1f;

		// Token: 0x02000761 RID: 1889
		public enum HotZoneTypes
		{
			// Token: 0x04002E26 RID: 11814
			None,
			// Token: 0x04002E27 RID: 11815
			Treasure,
			// Token: 0x04002E28 RID: 11816
			Custom
		}
	}
}
