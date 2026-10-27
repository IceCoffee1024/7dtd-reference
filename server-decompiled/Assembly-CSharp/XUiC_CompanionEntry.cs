using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F16 RID: 3862
[Preserve]
public class XUiC_CompanionEntry : XUiController
{
	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x0600786B RID: 30827 RVA: 0x002D9FFF File Offset: 0x002D81FF
	// (set) Token: 0x0600786C RID: 30828 RVA: 0x002DA007 File Offset: 0x002D8207
	public EntityAlive Companion { get; set; }

	// Token: 0x0600786D RID: 30829 RVA: 0x002DA010 File Offset: 0x002D8210
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

	// Token: 0x0600786E RID: 30830 RVA: 0x002DA15C File Offset: 0x002D835C
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

	// Token: 0x0600786F RID: 30831 RVA: 0x002DA2B5 File Offset: 0x002D84B5
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

	// Token: 0x06007870 RID: 30832 RVA: 0x002DA2DC File Offset: 0x002D84DC
	public bool HasChanged()
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		float magnitude = (this.Companion.GetPosition() - entityPlayer.GetPosition()).magnitude;
		bool flag = this.oldValue != magnitude;
		this.oldValue = magnitude;
		this.distance = magnitude;
		return flag;
	}

	// Token: 0x06007871 RID: 30833 RVA: 0x002DA334 File Offset: 0x002D8534
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

	// Token: 0x06007872 RID: 30834 RVA: 0x002DA4EC File Offset: 0x002D86EC
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

	// Token: 0x06007873 RID: 30835 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007874 RID: 30836 RVA: 0x002DA98A File Offset: 0x002D8B8A
	public override void OnClose()
	{
		base.OnClose();
		this.SetCompanion(null);
	}

	// Token: 0x06007875 RID: 30837 RVA: 0x002DA99C File Offset: 0x002D8B9C
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

	// Token: 0x04005950 RID: 22864
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealth;

	// Token: 0x04005951 RID: 22865
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barHealthModifiedMax;

	// Token: 0x04005952 RID: 22866
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStamina;

	// Token: 0x04005953 RID: 22867
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] barStaminaModifiedMax;

	// Token: 0x04005954 RID: 22868
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite arrowContent;

	// Token: 0x04005955 RID: 22869
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastHealthValue;

	// Token: 0x04005956 RID: 22870
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastStaminaValue;

	// Token: 0x04005957 RID: 22871
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance;

	// Token: 0x04005958 RID: 22872
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x04005959 RID: 22873
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;

	// Token: 0x0400595A RID: 22874
	[PublicizedFrom(EAccessModifier.Private)]
	public float arrowRotation;

	// Token: 0x0400595B RID: 22875
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastArrowRotation;

	// Token: 0x0400595C RID: 22876
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldValue;

	// Token: 0x0400595D RID: 22877
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt healthcurrentFormatter = new CachedStringFormatterInt();

	// Token: 0x0400595E RID: 22878
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> healthcurrentWMaxFormatter = new CachedStringFormatter<int, int>((int _i, int _i2) => string.Format("{0}/{1}", _i, _i2));

	// Token: 0x0400595F RID: 22879
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat healthfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005960 RID: 22880
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> distanceFormatter = new CachedStringFormatter<float>(delegate(float _f)
	{
		if (_f > 1000f)
		{
			return (_f / 1000f).ToCultureInvariantString("0.0") + " KM";
		}
		return _f.ToCultureInvariantString("0.0") + " M";
	});

	// Token: 0x04005961 RID: 22881
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005962 RID: 22882
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor arrowcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
