using System;
using UnityEngine;

// Token: 0x0200163C RID: 5692
public class vp_SimpleHUD : MonoBehaviour
{
	// Token: 0x17001521 RID: 5409
	// (get) Token: 0x0600B298 RID: 45720 RVA: 0x0042A346 File Offset: 0x00428546
	public float m_HealthWidth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HealthStyle.CalcSize(new GUIContent(this.FormattedHealth)).x;
		}
	}

	// Token: 0x17001522 RID: 5410
	// (get) Token: 0x0600B299 RID: 45721 RVA: 0x0042A363 File Offset: 0x00428563
	public GUIStyle MessageStyle
	{
		get
		{
			if (vp_SimpleHUD.m_MessageStyle == null)
			{
				vp_SimpleHUD.m_MessageStyle = new GUIStyle("Label");
				vp_SimpleHUD.m_MessageStyle.alignment = TextAnchor.MiddleCenter;
				vp_SimpleHUD.m_MessageStyle.font = this.MessageFont;
			}
			return vp_SimpleHUD.m_MessageStyle;
		}
	}

	// Token: 0x17001523 RID: 5411
	// (get) Token: 0x0600B29A RID: 45722 RVA: 0x0042A3A0 File Offset: 0x004285A0
	public GUIStyle HealthStyle
	{
		get
		{
			if (this.m_HealthStyle == null)
			{
				this.m_HealthStyle = new GUIStyle("Label");
				this.m_HealthStyle.font = this.BigFont;
				this.m_HealthStyle.alignment = TextAnchor.MiddleRight;
				this.m_HealthStyle.fontSize = 28;
				this.m_HealthStyle.wordWrap = false;
			}
			return this.m_HealthStyle;
		}
	}

	// Token: 0x17001524 RID: 5412
	// (get) Token: 0x0600B29B RID: 45723 RVA: 0x0042A408 File Offset: 0x00428608
	public GUIStyle AmmoStyle
	{
		get
		{
			if (this.m_AmmoStyle == null)
			{
				this.m_AmmoStyle = new GUIStyle("Label");
				this.m_AmmoStyle.font = this.BigFont;
				this.m_AmmoStyle.alignment = TextAnchor.MiddleRight;
				this.m_AmmoStyle.fontSize = 28;
				this.m_AmmoStyle.wordWrap = false;
			}
			return this.m_AmmoStyle;
		}
	}

	// Token: 0x17001525 RID: 5413
	// (get) Token: 0x0600B29C RID: 45724 RVA: 0x0042A470 File Offset: 0x00428670
	public GUIStyle AmmoStyleSmall
	{
		get
		{
			if (this.m_AmmoStyleSmall == null)
			{
				this.m_AmmoStyleSmall = new GUIStyle("Label");
				this.m_AmmoStyleSmall.font = this.SmallFont;
				this.m_AmmoStyleSmall.alignment = TextAnchor.UpperLeft;
				this.m_AmmoStyleSmall.fontSize = 15;
				this.m_AmmoStyleSmall.wordWrap = false;
			}
			return this.m_AmmoStyleSmall;
		}
	}

	// Token: 0x0600B29D RID: 45725 RVA: 0x0042A4D6 File Offset: 0x004286D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Audio = this.m_Player.transform.GetComponent<AudioSource>();
	}

	// Token: 0x0600B29E RID: 45726 RVA: 0x0042A4FF File Offset: 0x004286FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B29F RID: 45727 RVA: 0x0042A51B File Offset: 0x0042871B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x17001526 RID: 5414
	// (get) Token: 0x0600B2A0 RID: 45728 RVA: 0x0042A538 File Offset: 0x00428738
	public string FormattedHealth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			this.m_FormattedHealth = this.m_Player.Health.Get() * this.HealthMultiplier;
			if (this.m_FormattedHealth < 1f)
			{
				this.m_FormattedHealth = (this.m_Player.Dead.Active ? Mathf.Min(this.m_FormattedHealth, 0f) : 1f);
			}
			if (this.m_Player.Dead.Active && this.m_FormattedHealth > 0f)
			{
				this.m_FormattedHealth = 0f;
			}
			return ((int)this.m_FormattedHealth).ToString();
		}
	}

	// Token: 0x0600B2A1 RID: 45729 RVA: 0x0042A5DC File Offset: 0x004287DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.m_CurrentAmmoOffset = Mathf.SmoothStep(this.m_CurrentAmmoOffset, this.m_TargetAmmoOffset, Time.deltaTime * 10f);
		this.m_CurrentHealthOffset = Mathf.SmoothStep(this.m_CurrentHealthOffset, this.m_TargetHealthOffset, Time.deltaTime * 10f);
		if (this.m_Player.CurrentWeaponIndex.Get() == 0 || this.m_Player.CurrentWeaponType.Get() == 2)
		{
			this.m_TargetAmmoOffset = 200f;
		}
		else
		{
			this.m_TargetAmmoOffset = 10f;
		}
		if (this.m_Player.Dead.Active)
		{
			this.HealthColor = Color.black;
		}
		else if (this.m_Player.Health.Get() < this.HealthLowLevel)
		{
			this.HealthColor = Color.Lerp(Color.white, this.HealthLowColor, vp_MathUtility.Sinus(6f, 0.1f, 0f) * 5f + 0.5f);
			if (this.HealthLowSound != null && Time.time >= this.m_NextAllowedPlayHealthLowSoundTime)
			{
				this.m_NextAllowedPlayHealthLowSoundTime = Time.time + this.HealthLowSoundInterval;
				this.m_Audio.pitch = 1f;
				this.m_Audio.PlayOneShot(this.HealthLowSound);
			}
		}
		else
		{
			this.HealthColor = Color.white;
		}
		if (this.m_Player.CurrentWeaponAmmoCount.Get() < 1 && this.m_Player.CurrentWeaponType.Get() != 3)
		{
			this.AmmoColor = Color.Lerp(Color.white, this.AmmoLowColor, vp_MathUtility.Sinus(8f, 0.1f, 0f) * 5f + 0.5f);
			return;
		}
		this.AmmoColor = Color.white;
	}

	// Token: 0x0600B2A2 RID: 45730 RVA: 0x0042A7B9 File Offset: 0x004289B9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGUI()
	{
		if (!this.ShowHUD)
		{
			return;
		}
		this.DrawHealth();
		this.DrawAmmo();
		this.DrawText();
	}

	// Token: 0x0600B2A3 RID: 45731 RVA: 0x0042A7D8 File Offset: 0x004289D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawHealth()
	{
		this.DrawLabel("", new Vector2(this.m_CurrentHealthOffset, (float)(Screen.height - 68)), new Vector2(80f + this.m_HealthWidth, 52f), this.AmmoStyle, Color.white, this.m_TranspBlack, null);
		if (this.HealthIcon != null)
		{
			this.DrawLabel("", new Vector2(this.m_CurrentHealthOffset + 10f, (float)(Screen.height - 58)), new Vector2(32f, 32f), this.AmmoStyle, Color.white, this.HealthColor, this.HealthIcon);
		}
		this.DrawLabel(this.FormattedHealth, new Vector2(this.m_CurrentHealthOffset - 18f - (45f - this.m_HealthWidth), (float)Screen.height - this.BigFontOffset), new Vector2(110f, 60f), this.HealthStyle, this.HealthColor, Color.clear, null);
		this.DrawLabel("%", new Vector2(this.m_CurrentHealthOffset + 50f + this.m_HealthWidth, (float)Screen.height - this.SmallFontOffset), new Vector2(110f, 60f), this.AmmoStyleSmall, this.HealthColor, Color.clear, null);
		GUI.color = Color.white;
	}

	// Token: 0x0600B2A4 RID: 45732 RVA: 0x0042A938 File Offset: 0x00428B38
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawAmmo()
	{
		if (this.m_Player.CurrentWeaponType.Get() == 3)
		{
			this.DrawLabel("", new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 93f - this.AmmoStyle.CalcSize(new GUIContent(this.m_Player.CurrentWeaponAmmoCount.Get().ToString())).x, (float)(Screen.height - 68)), new Vector2(200f, 52f), this.AmmoStyle, this.AmmoColor, this.m_TranspBlack, null);
			if (this.m_Player.CurrentAmmoIcon.Get() != null)
			{
				this.DrawLabel("", new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 83f - this.AmmoStyle.CalcSize(new GUIContent(this.m_Player.CurrentWeaponAmmoCount.Get().ToString())).x, (float)(Screen.height - 58)), new Vector2(32f, 32f), this.AmmoStyle, Color.white, this.AmmoColor, this.m_Player.CurrentAmmoIcon.Get());
			}
			this.DrawLabel((this.m_Player.CurrentWeaponAmmoCount.Get() + this.m_Player.CurrentWeaponClipCount.Get()).ToString(), new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 145f, (float)Screen.height - this.BigFontOffset), new Vector2(110f, 60f), this.AmmoStyle, this.AmmoColor, Color.clear, null);
			return;
		}
		this.DrawLabel("", new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 115f - this.AmmoStyle.CalcSize(new GUIContent(this.m_Player.CurrentWeaponAmmoCount.Get().ToString())).x, (float)(Screen.height - 68)), new Vector2(200f, 52f), this.AmmoStyle, this.AmmoColor, this.m_TranspBlack, null);
		if (this.m_Player.CurrentAmmoIcon.Get() != null)
		{
			this.DrawLabel("", new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 105f - this.AmmoStyle.CalcSize(new GUIContent(this.m_Player.CurrentWeaponAmmoCount.Get().ToString())).x, (float)(Screen.height - 58)), new Vector2(32f, 32f), this.AmmoStyle, Color.white, this.AmmoColor, this.m_Player.CurrentAmmoIcon.Get());
		}
		this.DrawLabel(this.m_Player.CurrentWeaponAmmoCount.Get().ToString(), new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 177f, (float)Screen.height - this.BigFontOffset), new Vector2(110f, 60f), this.AmmoStyle, this.AmmoColor, Color.clear, null);
		this.DrawLabel("/ " + this.m_Player.CurrentWeaponClipCount.Get().ToString(), new Vector2(this.m_CurrentAmmoOffset + (float)Screen.width - 60f, (float)Screen.height - this.SmallFontOffset), new Vector2(110f, 60f), this.AmmoStyleSmall, this.AmmoColor, Color.clear, null);
	}

	// Token: 0x0600B2A5 RID: 45733 RVA: 0x0042AD28 File Offset: 0x00428F28
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawText()
	{
		if (this.m_PickupMessage == null)
		{
			return;
		}
		if (this.m_MessageColor.a < 0.01f)
		{
			return;
		}
		this.m_MessageColor = Color.Lerp(this.m_MessageColor, this.m_InvisibleColor, Time.deltaTime * 0.4f);
		GUI.color = this.m_MessageColor;
		GUI.Box(new Rect(200f, 150f, (float)(Screen.width - 400), (float)(Screen.height - 400)), this.m_PickupMessage, this.MessageStyle);
		GUI.color = Color.white;
	}

	// Token: 0x0600B2A6 RID: 45734 RVA: 0x0042ADC0 File Offset: 0x00428FC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HUDText(string message)
	{
		this.m_MessageColor = Color.white;
		this.m_PickupMessage = message;
	}

	// Token: 0x0600B2A7 RID: 45735 RVA: 0x0042ADD4 File Offset: 0x00428FD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawLabel(string text, Vector2 position, Vector2 scale, GUIStyle textStyle, Color textColor, Color bgColor, Texture texture)
	{
		if (texture == null)
		{
			texture = this.Background;
		}
		if (scale.x == 0f)
		{
			scale.x = textStyle.CalcSize(new GUIContent(text)).x;
		}
		if (scale.y == 0f)
		{
			scale.y = textStyle.CalcSize(new GUIContent(text)).y;
		}
		this.m_DrawLabelRect.x = (this.m_DrawPos.x = position.x);
		this.m_DrawLabelRect.y = (this.m_DrawPos.y = position.y);
		this.m_DrawLabelRect.width = (this.m_DrawSize.x = scale.x);
		this.m_DrawLabelRect.height = (this.m_DrawSize.y = scale.y);
		if (bgColor != Color.clear)
		{
			GUI.color = bgColor;
			if (texture != null)
			{
				GUI.DrawTexture(this.m_DrawLabelRect, texture);
			}
		}
		GUI.color = textColor;
		GUI.Label(this.m_DrawLabelRect, text, textStyle);
		GUI.color = Color.white;
		this.m_DrawPos.x = this.m_DrawPos.x + this.m_DrawSize.x;
		this.m_DrawPos.y = this.m_DrawPos.y + this.m_DrawSize.y;
	}

	// Token: 0x0600B2A8 RID: 45736 RVA: 0x0042AF3A File Offset: 0x0042913A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_SetWeapon()
	{
		this.m_TargetAmmoOffset = 200f;
	}

	// Token: 0x0600B2A9 RID: 45737 RVA: 0x0042AF47 File Offset: 0x00429147
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_SetWeapon()
	{
		this.m_TargetAmmoOffset = 10f;
	}

	// Token: 0x0600B2AA RID: 45738 RVA: 0x0042AF54 File Offset: 0x00429154
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_CurrentHealthOffset = -200f;
		this.m_TargetHealthOffset = 0f;
		this.HealthColor = Color.white;
	}

	// Token: 0x0400861A RID: 34330
	public bool ShowHUD = true;

	// Token: 0x0400861B RID: 34331
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400861C RID: 34332
	public Font BigFont;

	// Token: 0x0400861D RID: 34333
	public Font SmallFont;

	// Token: 0x0400861E RID: 34334
	public Font MessageFont;

	// Token: 0x0400861F RID: 34335
	public float BigFontOffset = 69f;

	// Token: 0x04008620 RID: 34336
	public float SmallFontOffset = 56f;

	// Token: 0x04008621 RID: 34337
	public Texture Background;

	// Token: 0x04008622 RID: 34338
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawPos = Vector2.zero;

	// Token: 0x04008623 RID: 34339
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawSize = Vector2.zero;

	// Token: 0x04008624 RID: 34340
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawLabelRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04008625 RID: 34341
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawShadowRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04008626 RID: 34342
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetHealthOffset;

	// Token: 0x04008627 RID: 34343
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHealthOffset;

	// Token: 0x04008628 RID: 34344
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetAmmoOffset = 200f;

	// Token: 0x04008629 RID: 34345
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAmmoOffset = 200f;

	// Token: 0x0400862A RID: 34346
	public Texture2D HealthIcon;

	// Token: 0x0400862B RID: 34347
	public float HealthMultiplier = 10f;

	// Token: 0x0400862C RID: 34348
	public Color HealthColor = Color.white;

	// Token: 0x0400862D RID: 34349
	public float HealthLowLevel = 2.5f;

	// Token: 0x0400862E RID: 34350
	public Color HealthLowColor = new Color(0.75f, 0f, 0f, 1f);

	// Token: 0x0400862F RID: 34351
	public AudioClip HealthLowSound;

	// Token: 0x04008630 RID: 34352
	public float HealthLowSoundInterval = 1f;

	// Token: 0x04008631 RID: 34353
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FormattedHealth;

	// Token: 0x04008632 RID: 34354
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedPlayHealthLowSoundTime;

	// Token: 0x04008633 RID: 34355
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008634 RID: 34356
	public Color AmmoColor = Color.white;

	// Token: 0x04008635 RID: 34357
	public Color AmmoLowColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x04008636 RID: 34358
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_PickupMessage = "";

	// Token: 0x04008637 RID: 34359
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_MessageColor = new Color(1f, 1f, 1f, 2f);

	// Token: 0x04008638 RID: 34360
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_InvisibleColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04008639 RID: 34361
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspBlack = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x0400863A RID: 34362
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspWhite = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400863B RID: 34363
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static GUIStyle m_MessageStyle;

	// Token: 0x0400863C RID: 34364
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_HealthStyle;

	// Token: 0x0400863D RID: 34365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyle;

	// Token: 0x0400863E RID: 34366
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyleSmall;
}
