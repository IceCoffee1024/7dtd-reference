using System;
using UnityEngine;

// Token: 0x02000762 RID: 1890
public class NavObjectScreenSettings : NavObjectSettings
{
	// Token: 0x0600384E RID: 14414 RVA: 0x001716F4 File Offset: 0x0016F8F4
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

	// Token: 0x04002E29 RID: 11817
	public NavObjectScreenSettings.ShowTextTypes ShowTextType;

	// Token: 0x04002E2A RID: 11818
	public int FontSize = 24;

	// Token: 0x04002E2B RID: 11819
	public Color FontColor = Color.white;

	// Token: 0x04002E2C RID: 11820
	public float SpriteSize = 32f;

	// Token: 0x04002E2D RID: 11821
	public float FadePercent = 0.9f;

	// Token: 0x04002E2E RID: 11822
	public float FadeEndDistance;

	// Token: 0x04002E2F RID: 11823
	public bool ShowOffScreen;

	// Token: 0x04002E30 RID: 11824
	public bool UseHeadOffset;

	// Token: 0x04002E31 RID: 11825
	public NavObjectScreenSettings.SpriteFillTypes SpriteFillType;

	// Token: 0x04002E32 RID: 11826
	public string SpriteFillName;

	// Token: 0x04002E33 RID: 11827
	public Color SpriteFillColor = Color.white;

	// Token: 0x04002E34 RID: 11828
	public string SubSpriteName;

	// Token: 0x04002E35 RID: 11829
	public Vector2 SubSpriteOffset;

	// Token: 0x04002E36 RID: 11830
	public float SubSpriteSize = 16f;

	// Token: 0x02000763 RID: 1891
	public enum ShowTextTypes
	{
		// Token: 0x04002E38 RID: 11832
		None,
		// Token: 0x04002E39 RID: 11833
		Distance,
		// Token: 0x04002E3A RID: 11834
		Name,
		// Token: 0x04002E3B RID: 11835
		SpawnName
	}

	// Token: 0x02000764 RID: 1892
	public enum SpriteFillTypes
	{
		// Token: 0x04002E3D RID: 11837
		None,
		// Token: 0x04002E3E RID: 11838
		Health
	}
}
