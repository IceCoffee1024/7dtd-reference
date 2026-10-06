using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F0B RID: 3851
[Preserve]
public class XUiC_CompanionEntry : XUiController
{
	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x06007845 RID: 30789 RVA: 0x002D8AA7 File Offset: 0x002D6CA7
	// (set) Token: 0x06007846 RID: 30790 RVA: 0x002D8AAF File Offset: 0x002D6CAF
	public EntityAlive Companion { get; set; }

	// Token: 0x06007847 RID: 30791 RVA: 0x002D8AB8 File Offset: 0x002D6CB8
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
	}

	// Token: 0x06007848 RID: 30792 RVA: 0x002D8C04 File Offset: 0x002D6E04
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.deltaTime = _dt;
		if (this.Companion == null || !XUi.IsGameRunning())
		{
			return;
		}
		this.RefreshFill();
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
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
		if (this.Companion != null && this.arrowContent != null)
		{
			this.arrowRotation = this.ReturnRotation(this.xui.playerUI.entityPlayer, this.Companion);
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
	}

	// Token: 0x06007849 RID: 30793 RVA: 0x002D8D5D File Offset: 0x002D6F5D
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetCompanion(EntityAlive entity)
	{
		this.Companion = entity;
		if (this.Companion == null)
		{
			base.RefreshBindings();
			return;
		}
		this.IsDirty = true;
	}

	// Token: 0x0600784A RID: 30794 RVA: 0x002D8D84 File Offset: 0x002D6F84
	public bool HasChanged()
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		float magnitude = (this.Companion.GetPosition() - entityPlayer.GetPosition()).magnitude;
		bool flag = this.oldValue != magnitude;
		this.oldValue = magnitude;
		this.distance = magnitude;
		return flag;
	}

	// Token: 0x0600784B RID: 30795 RVA: 0x002D8DDC File Offset: 0x002D6FDC
	public void RefreshFill()
	{
		if (this.Companion == null)
		{
			return;
		}
		float num = Time.deltaTime * 3f;
		if (this.barHealth != null)
		{
			float valuePercentUI = this.Companion.Stats.Health.ValuePercentUI;
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
				this.barHealthModifiedMax[j].Fill = this.Companion.Stats.Health.ModifiedMax / this.Companion.Stats.Health.Max;
			}
		}
		if (this.barStamina != null)
		{
			float valuePercentUI2 = this.Companion.Stats.Stamina.ValuePercentUI;
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
				this.barStaminaModifiedMax[l].Fill = this.Companion.Stats.Stamina.ModifiedMax / this.Companion.Stats.Stamina.Max;
			}
		}
	}

	// Token: 0x0600784C RID: 30796 RVA: 0x002D8F94 File Offset: 0x002D7194
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1339791005U)
		{
			if (num <= 783488098U)
			{
				if (num != 365230134U)
				{
					if (num != 679423209U)
					{
						if (num == 783488098U)
						{
							if (bindingName == "distance")
							{
								if (this.Companion == null)
								{
									value = "";
									return true;
								}
								value = this.distanceFormatter.Format(this.distance);
								return true;
							}
						}
					}
					else if (bindingName == "arrowcolor")
					{
						Color32 color = Color.white;
						if (this.Companion == null)
						{
							value = "255,255,255,255";
							return true;
						}
						int num2 = this.xui.playerUI.entityPlayer.Companions.IndexOf(this.Companion);
						color = Constants.TrackedFriendColors[num2 % Constants.TrackedFriendColors.Length];
						value = this.arrowcolorFormatter.Format(color);
						return true;
					}
				}
				else if (bindingName == "healthfill")
				{
					if (this.Companion == null)
					{
						value = "0";
						return true;
					}
					float valuePercentUI = this.Companion.Stats.Health.ValuePercentUI;
					value = this.healthfillFormatter.Format(valuePercentUI);
					return true;
				}
			}
			else if (num != 833193568U)
			{
				if (num != 1129104269U)
				{
					if (num == 1339791005U)
					{
						if (bindingName == "showarrow")
						{
							if (this.Companion == null)
							{
								value = "false";
								return true;
							}
							value = this.Companion.IsAlive().ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showicon")
				{
					if (this.Companion == null)
					{
						value = "false";
						return true;
					}
					value = this.Companion.IsDead().ToString();
					return true;
				}
			}
			else if (bindingName == "healthcurrent")
			{
				if (this.Companion == null)
				{
					value = "";
					return true;
				}
				value = this.healthcurrentFormatter.Format(this.Companion.Health);
				return true;
			}
		}
		else if (num <= 2535347189U)
		{
			if (num != 1810125418U)
			{
				if (num != 2369371622U)
				{
					if (num == 2535347189U)
					{
						if (bindingName == "distancecolor")
						{
							Color32 color2 = Color.white;
							if (this.Companion == null)
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
				else if (bindingName == "name")
				{
					if (this.Companion == null)
					{
						value = "";
						return true;
					}
					value = this.Companion.EntityName;
					return true;
				}
			}
			else if (bindingName == "healthmodifiedmax")
			{
				if (this.Companion == null || this.xui.playerUI.entityPlayer.IsDead())
				{
					value = "0";
					return true;
				}
				value = (this.Companion.Stats.Health.ModifiedMax / this.Companion.Stats.Health.Max).ToCultureInvariantString();
				return true;
			}
		}
		else if (num != 3004043574U)
		{
			if (num != 3862959600U)
			{
				if (num == 3910386971U)
				{
					if (bindingName == "partyvisible")
					{
						if (this.Companion == null)
						{
							value = "false";
							return true;
						}
						value = "true";
						return true;
					}
				}
			}
			else if (bindingName == "icon")
			{
				if (this.Companion == null || GameStats.GetBool(EnumGameStats.AutoParty))
				{
					value = "";
					return true;
				}
				if (this.Companion.IsDead())
				{
					value = "ui_game_symbol_death";
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (bindingName == "healthcurrentwithmax")
		{
			if (this.Companion == null)
			{
				value = "";
				return true;
			}
			value = this.healthcurrentWMaxFormatter.Format(this.Companion.Health, this.Companion.GetMaxHealth());
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600784D RID: 30797 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0600784E RID: 30798 RVA: 0x002D9432 File Offset: 0x002D7632
	public override void OnClose()
	{
		base.OnClose();
		this.SetCompanion(null);
	}

	// Token: 0x0600784F RID: 30799 RVA: 0x002D9444 File Offset: 0x002D7644
	[PublicizedFrom(EAccessModifier.Private)]
	public float ReturnRotation(EntityAlive _self, EntityAlive _other)
	{
		Vector2 vector = new Vector2(_self.transform.forward.x, _self.transform.forward.z);
		Vector3 normalized = (_self.transform.position - _other.transform.position).normalized;
		Vector2 vector2 = new Vector2(normalized.x, normalized.z);
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		float num = Vector2.Angle(vector, vector2);
		if (vector3.z < 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	// Token: 0x0400592B RID: 22827
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealth;

	// Token: 0x0400592C RID: 22828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealthModifiedMax;

	// Token: 0x0400592D RID: 22829
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStamina;

	// Token: 0x0400592E RID: 22830
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStaminaModifiedMax;

	// Token: 0x0400592F RID: 22831
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite arrowContent;

	// Token: 0x04005930 RID: 22832
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastHealthValue;

	// Token: 0x04005931 RID: 22833
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastStaminaValue;

	// Token: 0x04005932 RID: 22834
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance;

	// Token: 0x04005933 RID: 22835
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x04005934 RID: 22836
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;

	// Token: 0x04005935 RID: 22837
	[PublicizedFrom(EAccessModifier.Private)]
	public float arrowRotation;

	// Token: 0x04005936 RID: 22838
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastArrowRotation;

	// Token: 0x04005937 RID: 22839
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldValue;

	// Token: 0x04005938 RID: 22840
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt healthcurrentFormatter = new CachedStringFormatterInt();

	// Token: 0x04005939 RID: 22841
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> healthcurrentWMaxFormatter = new CachedStringFormatter<int, int>((int _i, int _i2) => string.Format("{0}/{1}", _i, _i2));

	// Token: 0x0400593A RID: 22842
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat healthfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400593B RID: 22843
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> distanceFormatter = new CachedStringFormatter<float>(delegate(float _f)
	{
		if (_f > 1000f)
		{
			return (_f / 1000f).ToCultureInvariantString("0.0") + " KM";
		}
		return _f.ToCultureInvariantString("0.0") + " M";
	});

	// Token: 0x0400593C RID: 22844
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400593D RID: 22845
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor arrowcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
