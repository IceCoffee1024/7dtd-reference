using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC2 RID: 4034
[Preserve]
public class XUiC_PartyEntry : XUiController
{
	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06007EC4 RID: 32452 RVA: 0x00302E86 File Offset: 0x00301086
	// (set) Token: 0x06007EC5 RID: 32453 RVA: 0x00302E8E File Offset: 0x0030108E
	public EntityPlayer Player { get; set; }

	// Token: 0x06007EC6 RID: 32454 RVA: 0x00302E98 File Offset: 0x00301098
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		XUiController[] array = base.GetChildrenById("BarHealth", null);
		if (array != null)
		{
			this.barHealth = new XUiV_Sprite[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.barHealth[i] = (XUiV_Sprite)array[i].ViewComponent;
			}
		}
		array = base.GetChildrenById("BarHealthModifiedMax", null);
		if (array != null)
		{
			this.barHealthModifiedMax = new XUiV_Sprite[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				this.barHealthModifiedMax[j] = (XUiV_Sprite)array[j].ViewComponent;
			}
		}
		array = base.GetChildrenById("BarStamina", null);
		if (array != null)
		{
			this.barStamina = new XUiV_Sprite[array.Length];
			for (int k = 0; k < array.Length; k++)
			{
				this.barStamina[k] = (XUiV_Sprite)array[k].ViewComponent;
			}
		}
		array = base.GetChildrenById("BarStaminaModifiedMax", null);
		if (array != null)
		{
			this.barStaminaModifiedMax = new XUiV_Sprite[array.Length];
			for (int l = 0; l < array.Length; l++)
			{
				this.barStaminaModifiedMax[l] = (XUiV_Sprite)array[l].ViewComponent;
			}
		}
		XUiController childById = base.GetChildById("arrowContent");
		if (childById != null)
		{
			this.arrowContent = (XUiV_Sprite)childById.ViewComponent;
		}
		XUiController xuiController = base.GetChildById("icon1");
		if (xuiController != null)
		{
			this.iconSprite1 = (XUiV_Sprite)xuiController.ViewComponent;
			this.defaultIconColor = this.iconSprite1.Color;
			this.iconSpriteSize = new Vector2((float)this.iconSprite1.Size.x, (float)this.iconSprite1.Size.y);
		}
		xuiController = base.GetChildById("icon2");
		if (xuiController != null)
		{
			this.iconSprite2 = (XUiV_Sprite)xuiController.ViewComponent;
		}
	}

	// Token: 0x06007EC7 RID: 32455 RVA: 0x00303068 File Offset: 0x00301268
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.Player == null || !XUi.IsGameRunning())
		{
			return;
		}
		this.RefreshFill();
		this.updateVoiceState();
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 0.5f;
			if (this.HasChanged() || this.IsDirty)
			{
				if (this.IsDirty)
				{
					base.RefreshBindings();
					this.IsDirty = false;
				}
				else
				{
					base.RefreshBindings();
				}
			}
		}
		if (this.Player != null && this.arrowContent != null)
		{
			this.arrowRotation = this.ReturnRotation(this.xui.playerUI.entityPlayer, this.Player);
			if (this.lastArrowRotation < 15f && this.arrowRotation > 345f)
			{
				this.lastArrowRotation = this.arrowRotation;
			}
			else if (this.lastArrowRotation > 345f && this.arrowRotation < 15f)
			{
				this.lastArrowRotation = this.arrowRotation;
			}
			else
			{
				this.lastArrowRotation = Mathf.Lerp(this.lastArrowRotation, this.arrowRotation, _dt * 3f);
			}
			this.arrowContent.UiTransform.localEulerAngles = new Vector3(0f, 0f, this.lastArrowRotation - 180f);
		}
		if (this.Player != null)
		{
			if (this.Player.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.TempDisabledEnding)
			{
				float num = Mathf.PingPong(Time.time, 0.5f);
				float num2 = 1f;
				if (num > 0.25f)
				{
					num2 = 1f + num - 0.25f;
				}
				if (this.Player.Party.Leader == this.Player)
				{
					this.iconSprite2.Color = Color.Lerp(this.defaultIconColor, this.iconBlinkColor, num * 4f);
					this.iconSprite2.Size = new Vector2i(this.iconSpriteSize * num2);
					return;
				}
				this.iconSprite1.Color = Color.Lerp(this.defaultIconColor, this.iconBlinkColor, num * 4f);
				this.iconSprite1.Size = new Vector2i(this.iconSpriteSize * num2);
				return;
			}
			else
			{
				this.iconSprite1.Color = this.defaultIconColor;
				this.iconSprite2.Color = this.defaultIconColor;
			}
		}
	}

	// Token: 0x06007EC8 RID: 32456 RVA: 0x003032D0 File Offset: 0x003014D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateVoiceState()
	{
		IPartyVoice.EVoiceMemberState playerVoiceState = VoiceHelpers.GetPlayerVoiceState(this.Player, true);
		if (playerVoiceState != this.voiceState)
		{
			this.voiceState = playerVoiceState;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007EC9 RID: 32457 RVA: 0x00303301 File Offset: 0x00301501
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetPlayer(EntityPlayer player)
	{
		this.Player = player;
		if (this.Player == null)
		{
			base.RefreshBindings();
			return;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007ECA RID: 32458 RVA: 0x00303328 File Offset: 0x00301528
	public bool HasChanged()
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		float magnitude = (this.Player.GetPosition() - entityPlayer.GetPosition()).magnitude;
		bool flag = this.oldValue != magnitude || this.Player.TwitchEnabled != this.oldTwitch || this.Player.TwitchSafe != this.oldSafe || this.Player.TwitchActionsEnabled != this.oldTwitchActions;
		this.oldValue = magnitude;
		this.distance = magnitude;
		this.oldTwitch = this.Player.TwitchEnabled;
		this.oldSafe = this.Player.TwitchSafe;
		this.oldTwitchActions = this.Player.TwitchActionsEnabled;
		return flag;
	}

	// Token: 0x06007ECB RID: 32459 RVA: 0x003033F0 File Offset: 0x003015F0
	public void RefreshFill()
	{
		if (this.Player == null)
		{
			return;
		}
		float num = Time.deltaTime * 3f;
		if (this.barHealth != null)
		{
			float valuePercentUI = this.Player.Stats.Health.ValuePercentUI;
			float num2 = Math.Max(this.lastHealthValue, 0f) * 1.01f;
			this.lastHealthValue = Mathf.Lerp(this.lastHealthValue, valuePercentUI, num);
			for (int i = 0; i < this.barHealth.Length; i++)
			{
				this.barHealth[i].Fill = num2;
			}
		}
		if (this.barHealthModifiedMax != null)
		{
			for (int j = 0; j < this.barHealthModifiedMax.Length; j++)
			{
				this.barHealthModifiedMax[j].Fill = this.Player.Stats.Health.ModifiedMax / this.Player.Stats.Health.Max;
			}
		}
		if (this.barStamina != null)
		{
			float valuePercentUI2 = this.Player.Stats.Stamina.ValuePercentUI;
			float num3 = Math.Max(this.lastStaminaValue, 0f) * 1.01f;
			this.lastStaminaValue = Mathf.Lerp(this.lastStaminaValue, valuePercentUI2, num);
			for (int k = 0; k < this.barStamina.Length; k++)
			{
				this.barStamina[k].Fill = num3;
			}
		}
		if (this.barStaminaModifiedMax != null)
		{
			for (int l = 0; l < this.barStaminaModifiedMax.Length; l++)
			{
				this.barStaminaModifiedMax[l].Fill = this.Player.Stats.Stamina.ModifiedMax / this.Player.Stats.Stamina.Max;
			}
		}
	}

	// Token: 0x06007ECC RID: 32460 RVA: 0x003035A8 File Offset: 0x003017A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1810125418U)
		{
			if (num <= 833193568U)
			{
				if (num <= 679423209U)
				{
					if (num != 365230134U)
					{
						if (num == 679423209U)
						{
							if (bindingName == "arrowcolor")
							{
								if (this.Player == null)
								{
									value = "255,255,255,255";
									return true;
								}
								int num2 = this.Player.Party.MemberList.IndexOf(this.Player);
								Color32 color = Constants.TrackedFriendColors[num2 % Constants.TrackedFriendColors.Length];
								value = this.arrowcolorFormatter.Format(color);
								return true;
							}
						}
					}
					else if (bindingName == "healthfill")
					{
						if (this.Player == null)
						{
							value = "0";
							return true;
						}
						float valuePercentUI = this.Player.Stats.Health.ValuePercentUI;
						value = this.healthfillFormatter.Format(valuePercentUI);
						return true;
					}
				}
				else if (num != 783488098U)
				{
					if (num == 833193568U)
					{
						if (bindingName == "healthcurrent")
						{
							if (this.Player == null)
							{
								value = "";
								return true;
							}
							value = this.healthcurrentFormatter.Format(this.Player.Health);
							return true;
						}
					}
				}
				else if (bindingName == "distance")
				{
					if (this.Player == null)
					{
						value = "";
						return true;
					}
					value = this.distanceFormatter.Format(this.distance);
					return true;
				}
			}
			else if (num <= 954351718U)
			{
				if (num != 937574099U)
				{
					if (num == 954351718U)
					{
						if (bindingName == "icon2")
						{
							if (this.Player == null || GameStats.GetBool(EnumGameStats.AutoParty))
							{
								value = "";
								return true;
							}
							if (!this.Player.IsDead() && !this.Player.IsPartyLead())
							{
								value = "";
							}
							else if (this.Player.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
							{
								value = this.twitchDisabledIcon;
							}
							else if (this.Player.TwitchSafe)
							{
								value = this.twitchSafeIcon;
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "icon1")
				{
					if (this.Player == null || GameStats.GetBool(EnumGameStats.AutoParty))
					{
						value = "";
						return true;
					}
					if (this.Player.IsDead())
					{
						value = this.deathIcon;
					}
					else if (this.Player.IsPartyLead())
					{
						value = this.leaderIcon;
					}
					else if (this.Player.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
					{
						value = this.twitchDisabledIcon;
					}
					else if (this.Player.TwitchSafe)
					{
						value = this.twitchSafeIcon;
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 1339791005U)
			{
				if (num != 1766231739U)
				{
					if (num == 1810125418U)
					{
						if (bindingName == "healthmodifiedmax")
						{
							if (this.Player == null || this.xui.playerUI.entityPlayer.IsDead())
							{
								value = "0";
								return true;
							}
							value = (this.Player.Stats.Health.ModifiedMax / this.Player.Stats.Health.Max).ToCultureInvariantString();
							return true;
						}
					}
				}
				else if (bindingName == "voiceactive")
				{
					value = (this.voiceState == IPartyVoice.EVoiceMemberState.VoiceActive).ToString();
					return true;
				}
			}
			else if (bindingName == "showarrow")
			{
				if (this.Player == null)
				{
					value = "false";
					return true;
				}
				value = this.Player.IsAlive().ToString();
				return true;
			}
		}
		else if (num <= 2535347189U)
		{
			if (num <= 2369371622U)
			{
				if (num != 1926871326U)
				{
					if (num == 2369371622U)
					{
						if (bindingName == "name")
						{
							if (this.Player == null)
							{
								value = "";
								return true;
							}
							value = GameUtils.SafeStringFormat(this.Player.PlayerDisplayName);
							return true;
						}
					}
				}
				else if (bindingName == "healthcolor")
				{
					if (this.Player == null)
					{
						value = "255,255,255,255";
						return true;
					}
					value = (this.Player.TwitchEnabled ? this.twitchHealthColor : this.defaultHealthColor);
					return true;
				}
			}
			else if (num != 2529325253U)
			{
				if (num == 2535347189U)
				{
					if (bindingName == "distancecolor")
					{
						Color32 color2 = Color.white;
						if (this.Player == null)
						{
							value = "255,255,255,255";
							return true;
						}
						if (this.distance > 100f)
						{
							color2 = Color.grey;
						}
						value = this.itemicontintcolorFormatter.Format(color2);
						return true;
					}
				}
			}
			else if (bindingName == "voicevisible")
			{
				value = (this.voiceState > IPartyVoice.EVoiceMemberState.Disabled).ToString();
				return true;
			}
		}
		else if (num <= 2966955437U)
		{
			if (num != 2916622580U)
			{
				if (num == 2966955437U)
				{
					if (bindingName == "showicon2")
					{
						if (this.Player == null)
						{
							value = "false";
							return true;
						}
						value = ((this.Player.IsPartyLead() || this.Player.IsDead()) && (this.Player.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled || this.Player.TwitchSafe) && this.Player.HasTwitchMember()).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "showicon1")
			{
				if (this.Player == null)
				{
					value = "false";
					return true;
				}
				value = (this.Player.IsPartyLead() || this.Player.IsDead() || ((this.Player.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled || this.Player.TwitchSafe) && this.Player.HasTwitchMember())).ToString();
				return true;
			}
		}
		else if (num != 3004043574U)
		{
			if (num != 3388762708U)
			{
				if (num == 3910386971U)
				{
					if (bindingName == "partyvisible")
					{
						if (this.Player == null || this.xui.playerUI.entityPlayer.IsDead())
						{
							value = "false";
							return true;
						}
						value = "true";
						return true;
					}
				}
			}
			else if (bindingName == "voicemuted")
			{
				value = (this.voiceState == IPartyVoice.EVoiceMemberState.Muted).ToString();
				return true;
			}
		}
		else if (bindingName == "healthcurrentwithmax")
		{
			if (this.Player == null)
			{
				value = "";
				return true;
			}
			value = this.healthcurrentWMaxFormatter.Format(this.Player.Health, this.Player.GetMaxHealth());
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007ECD RID: 32461 RVA: 0x00303D38 File Offset: 0x00301F38
	public XUiC_PartyEntry()
	{
		this.oldTwitch = false;
	}

	// Token: 0x06007ECE RID: 32462 RVA: 0x00303E30 File Offset: 0x00302030
	public override bool ParseAttribute(string name, string value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 1003795756U)
		{
			if (num != 547217411U)
			{
				if (num != 933825086U)
				{
					if (num == 1003795756U)
					{
						if (name == "leader_icon")
						{
							this.leaderIcon = value;
							return true;
						}
					}
				}
				else if (name == "twitch_icon")
				{
					this.twitchActiveIcon = value;
					return true;
				}
			}
			else if (name == "death_icon")
			{
				this.deathIcon = value;
				return true;
			}
		}
		else if (num <= 3319615829U)
		{
			if (num != 2710568369U)
			{
				if (num == 3319615829U)
				{
					if (name == "twitch_health_color")
					{
						this.twitchHealthColor = value;
						return true;
					}
				}
			}
			else if (name == "default_health_color")
			{
				this.defaultHealthColor = value;
				return true;
			}
		}
		else if (num != 3463996801U)
		{
			if (num == 4202079582U)
			{
				if (name == "twitch_safe_icon")
				{
					this.twitchSafeIcon = value;
					return true;
				}
			}
		}
		else if (name == "twitch_disabled_icon")
		{
			this.twitchDisabledIcon = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x06007ECF RID: 32463 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007ED0 RID: 32464 RVA: 0x00303F53 File Offset: 0x00302153
	public override void OnClose()
	{
		base.OnClose();
		this.SetPlayer(null);
	}

	// Token: 0x06007ED1 RID: 32465 RVA: 0x00303F64 File Offset: 0x00302164
	[PublicizedFrom(EAccessModifier.Private)]
	public float ReturnRotation(EntityAlive _self, EntityAlive _other)
	{
		Transform transform = _self.transform;
		Vector3 forward = transform.forward;
		Vector2 vector = new Vector2(forward.x, forward.z);
		Vector3 normalized = (transform.position - _other.transform.position).normalized;
		Vector2 vector2 = new Vector2(normalized.x, normalized.z);
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		float num = Vector2.Angle(vector, vector2);
		if (vector3.z < 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	// Token: 0x04005E05 RID: 24069
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ForcedRefreshTime = 0.5f;

	// Token: 0x04005E06 RID: 24070
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastHealthValue;

	// Token: 0x04005E07 RID: 24071
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastStaminaValue;

	// Token: 0x04005E09 RID: 24073
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite arrowContent;

	// Token: 0x04005E0A RID: 24074
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealth;

	// Token: 0x04005E0B RID: 24075
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealthModifiedMax;

	// Token: 0x04005E0C RID: 24076
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStamina;

	// Token: 0x04005E0D RID: 24077
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStaminaModifiedMax;

	// Token: 0x04005E0E RID: 24078
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance;

	// Token: 0x04005E0F RID: 24079
	public string defaultHealthColor = "255,0,0,128";

	// Token: 0x04005E10 RID: 24080
	public string twitchHealthColor = "100,65,165,128";

	// Token: 0x04005E11 RID: 24081
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite iconSprite1;

	// Token: 0x04005E12 RID: 24082
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite iconSprite2;

	// Token: 0x04005E13 RID: 24083
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultIconColor;

	// Token: 0x04005E14 RID: 24084
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 iconSpriteSize;

	// Token: 0x04005E15 RID: 24085
	[PublicizedFrom(EAccessModifier.Private)]
	public Color iconBlinkColor = new Color32(byte.MaxValue, 180, 0, byte.MaxValue);

	// Token: 0x04005E16 RID: 24086
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;

	// Token: 0x04005E17 RID: 24087
	[PublicizedFrom(EAccessModifier.Private)]
	public float arrowRotation;

	// Token: 0x04005E18 RID: 24088
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastArrowRotation;

	// Token: 0x04005E19 RID: 24089
	[PublicizedFrom(EAccessModifier.Private)]
	public IPartyVoice.EVoiceMemberState voiceState;

	// Token: 0x04005E1A RID: 24090
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldValue;

	// Token: 0x04005E1B RID: 24091
	[PublicizedFrom(EAccessModifier.Private)]
	public bool oldTwitch;

	// Token: 0x04005E1C RID: 24092
	[PublicizedFrom(EAccessModifier.Private)]
	public bool oldSafe;

	// Token: 0x04005E1D RID: 24093
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer.TwitchActionsStates oldTwitchActions;

	// Token: 0x04005E1E RID: 24094
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt healthcurrentFormatter = new CachedStringFormatterInt();

	// Token: 0x04005E1F RID: 24095
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> healthcurrentWMaxFormatter = new CachedStringFormatter<int, int>((int _i, int _i2) => string.Format("{0}/{1}", _i, _i2));

	// Token: 0x04005E20 RID: 24096
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat healthfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005E21 RID: 24097
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> distanceFormatter = new CachedStringFormatter<float>(new Func<float, string>(ValueDisplayFormatters.Distance));

	// Token: 0x04005E22 RID: 24098
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005E23 RID: 24099
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor arrowcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005E24 RID: 24100
	public string deathIcon = "ui_game_symbol_death";

	// Token: 0x04005E25 RID: 24101
	public string leaderIcon = "server_favorite";

	// Token: 0x04005E26 RID: 24102
	public string twitchActiveIcon = "ui_game_symbol_twitch_actions";

	// Token: 0x04005E27 RID: 24103
	public string twitchDisabledIcon = "ui_game_symbol_twitch_action_disabled";

	// Token: 0x04005E28 RID: 24104
	public string twitchSafeIcon = "ui_game_symbol_brick";
}
