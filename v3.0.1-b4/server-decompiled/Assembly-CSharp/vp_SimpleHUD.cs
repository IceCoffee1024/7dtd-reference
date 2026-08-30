using System;
using UnityEngine;

// Token: 0x02001602 RID: 5634
public class vp_SimpleHUD : MonoBehaviour
{
	// Token: 0x170014A5 RID: 5285
	// (get) Token: 0x0600B109 RID: 45321 RVA: 0x0042132A File Offset: 0x0041F52A
	public float m_HealthWidth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HealthStyle.CalcSize(new GUIContent(this.FormattedHealth)).x;
		}
	}

	// Token: 0x170014A6 RID: 5286
	// (get) Token: 0x0600B10A RID: 45322 RVA: 0x00421347 File Offset: 0x0041F547
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

	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x0600B10B RID: 45323 RVA: 0x00421384 File Offset: 0x0041F584
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

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x0600B10C RID: 45324 RVA: 0x004213EC File Offset: 0x0041F5EC
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

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x0600B10D RID: 45325 RVA: 0x00421454 File Offset: 0x0041F654
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

	// Token: 0x0600B10E RID: 45326 RVA: 0x004214BA File Offset: 0x0041F6BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Audio = this.m_Player.transform.GetComponent<AudioSource>();
	}

	// Token: 0x0600B10F RID: 45327 RVA: 0x004214E3 File Offset: 0x0041F6E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B110 RID: 45328 RVA: 0x004214FF File Offset: 0x0041F6FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x0600B111 RID: 45329 RVA: 0x0042151C File Offset: 0x0041F71C
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

	// Token: 0x0600B112 RID: 45330 RVA: 0x004215C0 File Offset: 0x0041F7C0
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

	// Token: 0x0600B113 RID: 45331 RVA: 0x0042179D File Offset: 0x0041F99D
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

	// Token: 0x0600B114 RID: 45332 RVA: 0x004217BC File Offset: 0x0041F9BC
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

	// Token: 0x0600B115 RID: 45333 RVA: 0x0042191C File Offset: 0x0041FB1C
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

	// Token: 0x0600B116 RID: 45334 RVA: 0x00421D0C File Offset: 0x0041FF0C
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

	// Token: 0x0600B117 RID: 45335 RVA: 0x00421DA4 File Offset: 0x0041FFA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HUDText(string message)
	{
		this.m_MessageColor = Color.white;
		this.m_PickupMessage = message;
	}

	// Token: 0x0600B118 RID: 45336 RVA: 0x00421DB8 File Offset: 0x0041FFB8
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

	// Token: 0x0600B119 RID: 45337 RVA: 0x00421F1E File Offset: 0x0042011E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_SetWeapon()
	{
		this.m_TargetAmmoOffset = 200f;
	}

	// Token: 0x0600B11A RID: 45338 RVA: 0x00421F2B File Offset: 0x0042012B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_SetWeapon()
	{
		this.m_TargetAmmoOffset = 10f;
	}

	// Token: 0x0600B11B RID: 45339 RVA: 0x00421F38 File Offset: 0x00420138
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_CurrentHealthOffset = -200f;
		this.m_TargetHealthOffset = 0f;
		this.HealthColor = Color.white;
	}

	// Token: 0x040084EB RID: 34027
	public bool ShowHUD = true;

	// Token: 0x040084EC RID: 34028
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040084ED RID: 34029
	public Font BigFont;

	// Token: 0x040084EE RID: 34030
	public Font SmallFont;

	// Token: 0x040084EF RID: 34031
	public Font MessageFont;

	// Token: 0x040084F0 RID: 34032
	public float BigFontOffset = 69f;

	// Token: 0x040084F1 RID: 34033
	public float SmallFontOffset = 56f;

	// Token: 0x040084F2 RID: 34034
	public Texture Background;

	// Token: 0x040084F3 RID: 34035
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawPos = Vector2.zero;

	// Token: 0x040084F4 RID: 34036
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawSize = Vector2.zero;

	// Token: 0x040084F5 RID: 34037
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawLabelRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040084F6 RID: 34038
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawShadowRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040084F7 RID: 34039
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetHealthOffset;

	// Token: 0x040084F8 RID: 34040
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHealthOffset;

	// Token: 0x040084F9 RID: 34041
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetAmmoOffset = 200f;

	// Token: 0x040084FA RID: 34042
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAmmoOffset = 200f;

	// Token: 0x040084FB RID: 34043
	public Texture2D HealthIcon;

	// Token: 0x040084FC RID: 34044
	public float HealthMultiplier = 10f;

	// Token: 0x040084FD RID: 34045
	public Color HealthColor = Color.white;

	// Token: 0x040084FE RID: 34046
	public float HealthLowLevel = 2.5f;

	// Token: 0x040084FF RID: 34047
	public Color HealthLowColor = new Color(0.75f, 0f, 0f, 1f);

	// Token: 0x04008500 RID: 34048
	public AudioClip HealthLowSound;

	// Token: 0x04008501 RID: 34049
	public float HealthLowSoundInterval = 1f;

	// Token: 0x04008502 RID: 34050
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FormattedHealth;

	// Token: 0x04008503 RID: 34051
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedPlayHealthLowSoundTime;

	// Token: 0x04008504 RID: 34052
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008505 RID: 34053
	public Color AmmoColor = Color.white;

	// Token: 0x04008506 RID: 34054
	public Color AmmoLowColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x04008507 RID: 34055
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_PickupMessage = "";

	// Token: 0x04008508 RID: 34056
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_MessageColor = new Color(1f, 1f, 1f, 2f);

	// Token: 0x04008509 RID: 34057
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_InvisibleColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400850A RID: 34058
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspBlack = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x0400850B RID: 34059
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspWhite = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400850C RID: 34060
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static GUIStyle m_MessageStyle;

	// Token: 0x0400850D RID: 34061
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_HealthStyle;

	// Token: 0x0400850E RID: 34062
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyle;

	// Token: 0x0400850F RID: 34063
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyleSmall;
}
