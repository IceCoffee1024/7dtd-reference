using System;
using UnityEngine;

// Token: 0x0200164E RID: 5710
public class vp_SimpleHUD : MonoBehaviour
{
	// Token: 0x1700152D RID: 5421
	// (get) Token: 0x0600B2F0 RID: 45808 RVA: 0x0042C74A File Offset: 0x0042A94A
	public float m_HealthWidth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HealthStyle.CalcSize(new GUIContent(this.FormattedHealth)).x;
		}
	}

	// Token: 0x1700152E RID: 5422
	// (get) Token: 0x0600B2F1 RID: 45809 RVA: 0x0042C767 File Offset: 0x0042A967
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

	// Token: 0x1700152F RID: 5423
	// (get) Token: 0x0600B2F2 RID: 45810 RVA: 0x0042C7A4 File Offset: 0x0042A9A4
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

	// Token: 0x17001530 RID: 5424
	// (get) Token: 0x0600B2F3 RID: 45811 RVA: 0x0042C80C File Offset: 0x0042AA0C
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

	// Token: 0x17001531 RID: 5425
	// (get) Token: 0x0600B2F4 RID: 45812 RVA: 0x0042C874 File Offset: 0x0042AA74
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

	// Token: 0x0600B2F5 RID: 45813 RVA: 0x0042C8DA File Offset: 0x0042AADA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Audio = this.m_Player.transform.GetComponent<AudioSource>();
	}

	// Token: 0x0600B2F6 RID: 45814 RVA: 0x0042C903 File Offset: 0x0042AB03
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B2F7 RID: 45815 RVA: 0x0042C91F File Offset: 0x0042AB1F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x17001532 RID: 5426
	// (get) Token: 0x0600B2F8 RID: 45816 RVA: 0x0042C93C File Offset: 0x0042AB3C
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

	// Token: 0x0600B2F9 RID: 45817 RVA: 0x0042C9E0 File Offset: 0x0042ABE0
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

	// Token: 0x0600B2FA RID: 45818 RVA: 0x0042CBBD File Offset: 0x0042ADBD
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

	// Token: 0x0600B2FB RID: 45819 RVA: 0x0042CBDC File Offset: 0x0042ADDC
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

	// Token: 0x0600B2FC RID: 45820 RVA: 0x0042CD3C File Offset: 0x0042AF3C
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

	// Token: 0x0600B2FD RID: 45821 RVA: 0x0042D12C File Offset: 0x0042B32C
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

	// Token: 0x0600B2FE RID: 45822 RVA: 0x0042D1C4 File Offset: 0x0042B3C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HUDText(string message)
	{
		this.m_MessageColor = Color.white;
		this.m_PickupMessage = message;
	}

	// Token: 0x0600B2FF RID: 45823 RVA: 0x0042D1D8 File Offset: 0x0042B3D8
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

	// Token: 0x0600B300 RID: 45824 RVA: 0x0042D33E File Offset: 0x0042B53E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_SetWeapon()
	{
		this.m_TargetAmmoOffset = 200f;
	}

	// Token: 0x0600B301 RID: 45825 RVA: 0x0042D34B File Offset: 0x0042B54B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_SetWeapon()
	{
		this.m_TargetAmmoOffset = 10f;
	}

	// Token: 0x0600B302 RID: 45826 RVA: 0x0042D358 File Offset: 0x0042B558
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_CurrentHealthOffset = -200f;
		this.m_TargetHealthOffset = 0f;
		this.HealthColor = Color.white;
	}

	// Token: 0x0400865B RID: 34395
	public bool ShowHUD = true;

	// Token: 0x0400865C RID: 34396
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400865D RID: 34397
	public Font BigFont;

	// Token: 0x0400865E RID: 34398
	public Font SmallFont;

	// Token: 0x0400865F RID: 34399
	public Font MessageFont;

	// Token: 0x04008660 RID: 34400
	public float BigFontOffset = 69f;

	// Token: 0x04008661 RID: 34401
	public float SmallFontOffset = 56f;

	// Token: 0x04008662 RID: 34402
	public Texture Background;

	// Token: 0x04008663 RID: 34403
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawPos = Vector2.zero;

	// Token: 0x04008664 RID: 34404
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawSize = Vector2.zero;

	// Token: 0x04008665 RID: 34405
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawLabelRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04008666 RID: 34406
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawShadowRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04008667 RID: 34407
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetHealthOffset;

	// Token: 0x04008668 RID: 34408
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHealthOffset;

	// Token: 0x04008669 RID: 34409
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetAmmoOffset = 200f;

	// Token: 0x0400866A RID: 34410
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAmmoOffset = 200f;

	// Token: 0x0400866B RID: 34411
	public Texture2D HealthIcon;

	// Token: 0x0400866C RID: 34412
	public float HealthMultiplier = 10f;

	// Token: 0x0400866D RID: 34413
	public Color HealthColor = Color.white;

	// Token: 0x0400866E RID: 34414
	public float HealthLowLevel = 2.5f;

	// Token: 0x0400866F RID: 34415
	public Color HealthLowColor = new Color(0.75f, 0f, 0f, 1f);

	// Token: 0x04008670 RID: 34416
	public AudioClip HealthLowSound;

	// Token: 0x04008671 RID: 34417
	public float HealthLowSoundInterval = 1f;

	// Token: 0x04008672 RID: 34418
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FormattedHealth;

	// Token: 0x04008673 RID: 34419
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedPlayHealthLowSoundTime;

	// Token: 0x04008674 RID: 34420
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008675 RID: 34421
	public Color AmmoColor = Color.white;

	// Token: 0x04008676 RID: 34422
	public Color AmmoLowColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x04008677 RID: 34423
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_PickupMessage = "";

	// Token: 0x04008678 RID: 34424
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_MessageColor = new Color(1f, 1f, 1f, 2f);

	// Token: 0x04008679 RID: 34425
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_InvisibleColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400867A RID: 34426
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspBlack = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x0400867B RID: 34427
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspWhite = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400867C RID: 34428
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static GUIStyle m_MessageStyle;

	// Token: 0x0400867D RID: 34429
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_HealthStyle;

	// Token: 0x0400867E RID: 34430
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyle;

	// Token: 0x0400867F RID: 34431
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyleSmall;
}
