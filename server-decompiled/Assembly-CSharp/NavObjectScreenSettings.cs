using System;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class NavObjectScreenSettings : NavObjectSettings
{
	// Token: 0x06003837 RID: 14391 RVA: 0x0017075C File Offset: 0x0016E95C
	public override void Init()
	{
		base.Init();
		this.Properties.ParseEnum<NavObjectScreenSettings.ShowTextTypes>("text_type", ref this.ShowTextType);
		this.Properties.ParseFloat("fade_percent", ref this.FadePercent);
		this.Properties.ParseBool("show_offscreen", ref this.ShowOffScreen);
		this.Properties.ParseBool("use_head_offset", ref this.UseHeadOffset);
		this.Properties.ParseFloat("sprite_size", ref this.SpriteSize);
		this.Properties.ParseColor("sprite_fill_color", ref this.SpriteFillColor);
		this.Properties.ParseEnum<NavObjectScreenSettings.SpriteFillTypes>("sprite_fill_type", ref this.SpriteFillType);
		this.Properties.ParseString("sprite_fill_name", ref this.SpriteFillName);
		this.Properties.ParseString("sub_sprite_name", ref this.SubSpriteName);
		this.Properties.ParseVec("sub_sprite_offset", ref this.SubSpriteOffset);
		this.Properties.ParseFloat("sub_sprite_size", ref this.SubSpriteSize);
		this.Properties.ParseInt("font_size", ref this.FontSize);
		this.Properties.ParseColor("font_color", ref this.FontColor);
		this.FadeEndDistance = (this.MaxDistance - this.MinDistance) * this.FadePercent;
	}

	// Token: 0x04002E0D RID: 11789
	public NavObjectScreenSettings.ShowTextTypes ShowTextType;

	// Token: 0x04002E0E RID: 11790
	public int FontSize = 24;

	// Token: 0x04002E0F RID: 11791
	public Color FontColor = Color.white;

	// Token: 0x04002E10 RID: 11792
	public float SpriteSize = 32f;

	// Token: 0x04002E11 RID: 11793
	public float FadePercent = 0.9f;

	// Token: 0x04002E12 RID: 11794
	public float FadeEndDistance;

	// Token: 0x04002E13 RID: 11795
	public bool ShowOffScreen;

	// Token: 0x04002E14 RID: 11796
	public bool UseHeadOffset;

	// Token: 0x04002E15 RID: 11797
	public NavObjectScreenSettings.SpriteFillTypes SpriteFillType;

	// Token: 0x04002E16 RID: 11798
	public string SpriteFillName;

	// Token: 0x04002E17 RID: 11799
	public Color SpriteFillColor = Color.white;

	// Token: 0x04002E18 RID: 11800
	public string SubSpriteName;

	// Token: 0x04002E19 RID: 11801
	public Vector2 SubSpriteOffset;

	// Token: 0x04002E1A RID: 11802
	public float SubSpriteSize = 16f;

	// Token: 0x0200075B RID: 1883
	public enum ShowTextTypes
	{
		// Token: 0x04002E1C RID: 11804
		None,
		// Token: 0x04002E1D RID: 11805
		Distance,
		// Token: 0x04002E1E RID: 11806
		Name,
		// Token: 0x04002E1F RID: 11807
		SpawnName
	}

	// Token: 0x0200075C RID: 1884
	public enum SpriteFillTypes
	{
		// Token: 0x04002E21 RID: 11809
		None,
		// Token: 0x04002E22 RID: 11810
		Health
	}
}
