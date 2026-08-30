using System;
using UnityEngine;

// Token: 0x0200161D RID: 5661
public class vp_SimpleHUD : MonoBehaviour
{
	// Token: 0x170014DE RID: 5342
	// (get) Token: 0x0600B206 RID: 45574 RVA: 0x0042666A File Offset: 0x0042486A
	public float m_HealthWidth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HealthStyle.CalcSize(new GUIContent(this.FormattedHealth)).x;
		}
	}

	// Token: 0x170014DF RID: 5343
	// (get) Token: 0x0600B207 RID: 45575 RVA: 0x00426687 File Offset: 0x00424887
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

	// Token: 0x170014E0 RID: 5344
	// (get) Token: 0x0600B208 RID: 45576 RVA: 0x004266C4 File Offset: 0x004248C4
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

	// Token: 0x170014E1 RID: 5345
	// (get) Token: 0x0600B209 RID: 45577 RVA: 0x0042672C File Offset: 0x0042492C
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

	// Token: 0x170014E2 RID: 5346
	// (get) Token: 0x0600B20A RID: 45578 RVA: 0x00426794 File Offset: 0x00424994
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

	// Token: 0x0600B20B RID: 45579 RVA: 0x004267FA File Offset: 0x004249FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Audio = this.m_Player.transform.GetComponent<AudioSource>();
	}

	// Token: 0x0600B20C RID: 45580 RVA: 0x00426823 File Offset: 0x00424A23
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B20D RID: 45581 RVA: 0x0042683F File Offset: 0x00424A3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x170014E3 RID: 5347
	// (get) Token: 0x0600B20E RID: 45582 RVA: 0x0042685C File Offset: 0x00424A5C
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

	// Token: 0x0600B20F RID: 45583 RVA: 0x00426900 File Offset: 0x00424B00
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

	// Token: 0x0600B210 RID: 45584 RVA: 0x00426ADD File Offset: 0x00424CDD
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

	// Token: 0x0600B211 RID: 45585 RVA: 0x00426AFC File Offset: 0x00424CFC
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

	// Token: 0x0600B212 RID: 45586 RVA: 0x00426C5C File Offset: 0x00424E5C
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

	// Token: 0x0600B213 RID: 45587 RVA: 0x0042704C File Offset: 0x0042524C
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

	// Token: 0x0600B214 RID: 45588 RVA: 0x004270E4 File Offset: 0x004252E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HUDText(string message)
	{
		this.m_MessageColor = Color.white;
		this.m_PickupMessage = message;
	}

	// Token: 0x0600B215 RID: 45589 RVA: 0x004270F8 File Offset: 0x004252F8
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

	// Token: 0x0600B216 RID: 45590 RVA: 0x0042725E File Offset: 0x0042545E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_SetWeapon()
	{
		this.m_TargetAmmoOffset = 200f;
	}

	// Token: 0x0600B217 RID: 45591 RVA: 0x0042726B File Offset: 0x0042546B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_SetWeapon()
	{
		this.m_TargetAmmoOffset = 10f;
	}

	// Token: 0x0600B218 RID: 45592 RVA: 0x00427278 File Offset: 0x00425478
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_CurrentHealthOffset = -200f;
		this.m_TargetHealthOffset = 0f;
		this.HealthColor = Color.white;
	}

	// Token: 0x040085C4 RID: 34244
	public bool ShowHUD = true;

	// Token: 0x040085C5 RID: 34245
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040085C6 RID: 34246
	public Font BigFont;

	// Token: 0x040085C7 RID: 34247
	public Font SmallFont;

	// Token: 0x040085C8 RID: 34248
	public Font MessageFont;

	// Token: 0x040085C9 RID: 34249
	public float BigFontOffset = 69f;

	// Token: 0x040085CA RID: 34250
	public float SmallFontOffset = 56f;

	// Token: 0x040085CB RID: 34251
	public Texture Background;

	// Token: 0x040085CC RID: 34252
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawPos = Vector2.zero;

	// Token: 0x040085CD RID: 34253
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_DrawSize = Vector2.zero;

	// Token: 0x040085CE RID: 34254
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawLabelRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040085CF RID: 34255
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_DrawShadowRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040085D0 RID: 34256
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetHealthOffset;

	// Token: 0x040085D1 RID: 34257
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHealthOffset;

	// Token: 0x040085D2 RID: 34258
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TargetAmmoOffset = 200f;

	// Token: 0x040085D3 RID: 34259
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAmmoOffset = 200f;

	// Token: 0x040085D4 RID: 34260
	public Texture2D HealthIcon;

	// Token: 0x040085D5 RID: 34261
	public float HealthMultiplier = 10f;

	// Token: 0x040085D6 RID: 34262
	public Color HealthColor = Color.white;

	// Token: 0x040085D7 RID: 34263
	public float HealthLowLevel = 2.5f;

	// Token: 0x040085D8 RID: 34264
	public Color HealthLowColor = new Color(0.75f, 0f, 0f, 1f);

	// Token: 0x040085D9 RID: 34265
	public AudioClip HealthLowSound;

	// Token: 0x040085DA RID: 34266
	public float HealthLowSoundInterval = 1f;

	// Token: 0x040085DB RID: 34267
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FormattedHealth;

	// Token: 0x040085DC RID: 34268
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedPlayHealthLowSoundTime;

	// Token: 0x040085DD RID: 34269
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040085DE RID: 34270
	public Color AmmoColor = Color.white;

	// Token: 0x040085DF RID: 34271
	public Color AmmoLowColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x040085E0 RID: 34272
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_PickupMessage = "";

	// Token: 0x040085E1 RID: 34273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_MessageColor = new Color(1f, 1f, 1f, 2f);

	// Token: 0x040085E2 RID: 34274
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_InvisibleColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040085E3 RID: 34275
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspBlack = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x040085E4 RID: 34276
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_TranspWhite = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x040085E5 RID: 34277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static GUIStyle m_MessageStyle;

	// Token: 0x040085E6 RID: 34278
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_HealthStyle;

	// Token: 0x040085E7 RID: 34279
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyle;

	// Token: 0x040085E8 RID: 34280
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIStyle m_AmmoStyleSmall;
}
