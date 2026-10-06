using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001090 RID: 4240
[Preserve]
public class XUiC_TargetBar : XUiController
{
	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x06008680 RID: 34432 RVA: 0x00336259 File Offset: 0x00334459
	// (set) Token: 0x06008681 RID: 34433 RVA: 0x00336261 File Offset: 0x00334461
	public EntityAlive Target { get; set; }

	// Token: 0x06008682 RID: 34434 RVA: 0x002DF077 File Offset: 0x002DD277
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06008683 RID: 34435 RVA: 0x0033626C File Offset: 0x0033446C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.deltaTime = _dt;
		this.checkSandboxOption -= this.deltaTime;
		if (this.checkSandboxOption <= 0f)
		{
			this.checkSandboxOption = 3f;
			this.sandboxForceShow = SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.ShowHealthBars);
		}
		if (this.gameEventManager == null)
		{
			this.gameEventManager = GameEventManager.Current;
		}
		if (!this.xui.playerUI.entityPlayer.IsAlive())
		{
			this.viewComponent.IsVisible = false;
			return;
		}
		if (this.gameEventManager.CurrentBossGroup != null)
		{
			this.CurrentBossGroup = this.gameEventManager.CurrentBossGroup;
			this.Target = this.CurrentBossGroup.BossEntity;
			bool flag = false;
			if (this.Target != null && this.Target.IsAlive())
			{
				flag = true;
			}
			if (this.CurrentBossGroup.MinionCount != 0)
			{
				flag = true;
			}
			if (flag)
			{
				this.viewComponent.IsVisible = true;
				this.noTargetFadeTime = 0f;
			}
			else if (this.noTargetFadeTime >= this.noTargetFadeTimeMax)
			{
				this.Target = null;
				this.viewComponent.IsVisible = false;
				this.CurrentBossGroup.BossEntity = null;
			}
			else
			{
				this.noTargetFadeTime += Time.deltaTime;
			}
		}
		else
		{
			if (this.CurrentBossGroup != null)
			{
				this.Target = null;
				this.CurrentBossGroup = null;
			}
			if (!this.sandboxForceShow)
			{
				if (this.visibility == XUiC_TargetBar.EVisibility.Never)
				{
					this.viewComponent.IsVisible = false;
					return;
				}
				if (this.visibility == XUiC_TargetBar.EVisibility.GodMode && !this.xui.playerUI.entityPlayer.IsGodMode.Value)
				{
					this.viewComponent.IsVisible = false;
					return;
				}
			}
			bool flag2 = false;
			WorldRayHitInfo hitInfo = this.xui.playerUI.entityPlayer.HitInfo;
			if (hitInfo.bHitValid && hitInfo.transform && hitInfo.tag.StartsWith("E_", StringComparison.Ordinal))
			{
				Transform hitRootTransform = GameUtils.GetHitRootTransform(hitInfo.tag, hitInfo.transform);
				EntityAlive entityAlive = null;
				if (hitRootTransform != null)
				{
					entityAlive = hitRootTransform.GetComponent<EntityAlive>();
				}
				if (entityAlive != null && entityAlive.IsAlive())
				{
					flag2 = true;
					this.Target = entityAlive;
				}
			}
			if (this.Target == null)
			{
				this.viewComponent.IsVisible = false;
				this.noTargetFadeTime = this.noTargetFadeTimeMax;
				return;
			}
			if (flag2)
			{
				this.viewComponent.IsVisible = true;
				this.noTargetFadeTime = 0f;
			}
			else if (this.noTargetFadeTime >= this.noTargetFadeTimeMax)
			{
				this.Target = null;
				this.viewComponent.IsVisible = false;
			}
			else
			{
				this.noTargetFadeTime += Time.deltaTime;
			}
			if (this.Target != null && (this.Target.IsDead() || this.Target.Health == 0))
			{
				this.Target = null;
				this.viewComponent.IsVisible = false;
				this.noTargetFadeTime = this.noTargetFadeTimeMax;
			}
		}
		base.RefreshBindings();
		this.IsDirty = false;
	}

	// Token: 0x06008684 RID: 34436 RVA: 0x00336574 File Offset: 0x00334774
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1324577848U)
		{
			if (num <= 973435292U)
			{
				if (num != 17711523U)
				{
					if (num == 973435292U)
					{
						if (bindingName == "isboss")
						{
							if (this.gameEventManager == null)
							{
								value = "false";
								return true;
							}
							value = (this.CurrentBossGroup != null).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "isnotboss")
				{
					if (this.gameEventManager == null)
					{
						value = "false";
						return true;
					}
					value = (this.CurrentBossGroup == null).ToString();
					return true;
				}
			}
			else if (num != 1080943954U)
			{
				if (num == 1324577848U)
				{
					if (bindingName == "currentwithmax")
					{
						if (this.Target == null)
						{
							value = "";
							return true;
						}
						value = this.statcurrentWMaxFormatterAOfB.Format(this.Target.IsAlive() ? ((int)this.Target.Stats.Health.Value) : 0, (int)this.Target.Stats.Health.Max);
						return true;
					}
				}
			}
			else if (bindingName == "minioncount")
			{
				if (this.gameEventManager == null || this.CurrentBossGroup == null)
				{
					value = "";
					return true;
				}
				value = this.CurrentBossGroup.MinionCount.ToString();
				return true;
			}
		}
		else if (num <= 2984927816U)
		{
			if (num != 2369371622U)
			{
				if (num == 2984927816U)
				{
					if (bindingName == "fill")
					{
						if (this.Target == null)
						{
							value = "0";
							return true;
						}
						float num2 = (float)this.Target.Health / (float)this.Target.GetMaxHealth();
						float num3 = Math.Max(this.lastValue, 0f) * 1.01f;
						value = this.statfillFormatter.Format(num3);
						this.lastValue = Mathf.Lerp(this.lastValue, num2, this.deltaTime * 3f);
						return true;
					}
				}
			}
			else if (bindingName == "name")
			{
				if (this.CurrentBossGroup != null)
				{
					value = this.CurrentBossGroup.BossName;
				}
				else if (this.Target == null)
				{
					value = "";
				}
				else
				{
					EntityPlayer entityPlayer = this.Target as EntityPlayer;
					value = ((entityPlayer != null) ? entityPlayer.PlayerDisplayName : Localization.Get(EntityClass.list[this.Target.entityClass].entityClassName, false, null));
				}
				return true;
			}
		}
		else if (num != 3647323098U)
		{
			if (num == 3767448944U)
			{
				if (bindingName == "boss_sprite")
				{
					if (this.gameEventManager == null || this.CurrentBossGroup == null)
					{
						value = "";
						return true;
					}
					value = ((this.CurrentBossGroup.BossIcon == "") ? this.defaultBossIcon : this.CurrentBossGroup.BossIcon);
					return true;
				}
			}
		}
		else if (bindingName == "current")
		{
			if (this.Target == null)
			{
				value = "";
				return true;
			}
			value = this.statcurrentFormatterInt.Format(this.Target.Health);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008685 RID: 34437 RVA: 0x003368EF File Offset: 0x00334AEF
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "visibility")
		{
			this.visibility = EnumUtils.Parse<XUiC_TargetBar.EVisibility>(_value, true);
			return true;
		}
		if (!(_name == "default_boss_icon"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.defaultBossIcon = _value;
		return true;
	}

	// Token: 0x040063FB RID: 25595
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastValue;

	// Token: 0x040063FC RID: 25596
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x040063FD RID: 25597
	[PublicizedFrom(EAccessModifier.Private)]
	public float noTargetFadeTime;

	// Token: 0x040063FE RID: 25598
	[PublicizedFrom(EAccessModifier.Private)]
	public float noTargetFadeTimeMax = 3f;

	// Token: 0x040063FF RID: 25599
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TargetBar.EVisibility visibility;

	// Token: 0x04006400 RID: 25600
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultBossIcon = "ui_game_symbol_twitch_boss_bar_default";

	// Token: 0x04006401 RID: 25601
	[PublicizedFrom(EAccessModifier.Private)]
	public GameEventManager gameEventManager;

	// Token: 0x04006402 RID: 25602
	[PublicizedFrom(EAccessModifier.Private)]
	public BossGroup CurrentBossGroup;

	// Token: 0x04006403 RID: 25603
	[PublicizedFrom(EAccessModifier.Private)]
	public float checkSandboxOption;

	// Token: 0x04006404 RID: 25604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sandboxForceShow;

	// Token: 0x04006405 RID: 25605
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statcurrentFormatterInt = new CachedStringFormatterInt();

	// Token: 0x04006406 RID: 25606
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> statcurrentWMaxFormatterAOfB = new CachedStringFormatter<int, int>((int _i, int _i1) => string.Format("{0}/{1}", _i, _i1));

	// Token: 0x04006407 RID: 25607
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat statfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x02001091 RID: 4241
	public enum EVisibility
	{
		// Token: 0x04006409 RID: 25609
		Never,
		// Token: 0x0400640A RID: 25610
		GodMode,
		// Token: 0x0400640B RID: 25611
		Always,
		// Token: 0x0400640C RID: 25612
		Boss
	}
}
