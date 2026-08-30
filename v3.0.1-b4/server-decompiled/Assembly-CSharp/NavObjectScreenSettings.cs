using System;
using UnityEngine;

// Token: 0x0200074E RID: 1870
public class NavObjectScreenSettings : NavObjectSettings
{
	// Token: 0x0600379E RID: 14238 RVA: 0x0016DBDC File Offset: 0x0016BDDC
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

	// Token: 0x04002D9C RID: 11676
	public NavObjectScreenSettings.ShowTextTypes ShowTextType;

	// Token: 0x04002D9D RID: 11677
	public int FontSize = 24;

	// Token: 0x04002D9E RID: 11678
	public Color FontColor = Color.white;

	// Token: 0x04002D9F RID: 11679
	public float SpriteSize = 32f;

	// Token: 0x04002DA0 RID: 11680
	public float FadePercent = 0.9f;

	// Token: 0x04002DA1 RID: 11681
	public float FadeEndDistance;

	// Token: 0x04002DA2 RID: 11682
	public bool ShowOffScreen;

	// Token: 0x04002DA3 RID: 11683
	public bool UseHeadOffset;

	// Token: 0x04002DA4 RID: 11684
	public NavObjectScreenSettings.SpriteFillTypes SpriteFillType;

	// Token: 0x04002DA5 RID: 11685
	public string SpriteFillName;

	// Token: 0x04002DA6 RID: 11686
	public Color SpriteFillColor = Color.white;

	// Token: 0x04002DA7 RID: 11687
	public string SubSpriteName;

	// Token: 0x04002DA8 RID: 11688
	public Vector2 SubSpriteOffset;

	// Token: 0x04002DA9 RID: 11689
	public float SubSpriteSize = 16f;

	// Token: 0x0200074F RID: 1871
	public enum ShowTextTypes
	{
		// Token: 0x04002DAB RID: 11691
		None,
		// Token: 0x04002DAC RID: 11692
		Distance,
		// Token: 0x04002DAD RID: 11693
		Name,
		// Token: 0x04002DAE RID: 11694
		SpawnName
	}

	// Token: 0x02000750 RID: 1872
	public enum SpriteFillTypes
	{
		// Token: 0x04002DB0 RID: 11696
		None,
		// Token: 0x04002DB1 RID: 11697
		Health
	}
}
