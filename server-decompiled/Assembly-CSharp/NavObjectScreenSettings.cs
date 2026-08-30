using System;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class NavObjectScreenSettings : NavObjectSettings
{
	// Token: 0x060037F4 RID: 14324 RVA: 0x0016FDF8 File Offset: 0x0016DFF8
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

	// Token: 0x04002E14 RID: 11796
	public NavObjectScreenSettings.ShowTextTypes ShowTextType;

	// Token: 0x04002E15 RID: 11797
	public int FontSize = 24;

	// Token: 0x04002E16 RID: 11798
	public Color FontColor = Color.white;

	// Token: 0x04002E17 RID: 11799
	public float SpriteSize = 32f;

	// Token: 0x04002E18 RID: 11800
	public float FadePercent = 0.9f;

	// Token: 0x04002E19 RID: 11801
	public float FadeEndDistance;

	// Token: 0x04002E1A RID: 11802
	public bool ShowOffScreen;

	// Token: 0x04002E1B RID: 11803
	public bool UseHeadOffset;

	// Token: 0x04002E1C RID: 11804
	public NavObjectScreenSettings.SpriteFillTypes SpriteFillType;

	// Token: 0x04002E1D RID: 11805
	public string SpriteFillName;

	// Token: 0x04002E1E RID: 11806
	public Color SpriteFillColor = Color.white;

	// Token: 0x04002E1F RID: 11807
	public string SubSpriteName;

	// Token: 0x04002E20 RID: 11808
	public Vector2 SubSpriteOffset;

	// Token: 0x04002E21 RID: 11809
	public float SubSpriteSize = 16f;

	// Token: 0x02000759 RID: 1881
	public enum ShowTextTypes
	{
		// Token: 0x04002E23 RID: 11811
		None,
		// Token: 0x04002E24 RID: 11812
		Distance,
		// Token: 0x04002E25 RID: 11813
		Name,
		// Token: 0x04002E26 RID: 11814
		SpawnName
	}

	// Token: 0x0200075A RID: 1882
	public enum SpriteFillTypes
	{
		// Token: 0x04002E28 RID: 11816
		None,
		// Token: 0x04002E29 RID: 11817
		Health
	}
}
