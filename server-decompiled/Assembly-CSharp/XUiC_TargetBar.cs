using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001080 RID: 4224
[Preserve]
public class XUiC_TargetBar : XUiController
{
	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x0600863E RID: 34366 RVA: 0x00332921 File Offset: 0x00330B21
	// (set) Token: 0x0600863F RID: 34367 RVA: 0x00332929 File Offset: 0x00330B29
	public EntityAlive Target { get; set; }

	// Token: 0x06008640 RID: 34368 RVA: 0x002DBCDB File Offset: 0x002D9EDB
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06008641 RID: 34369 RVA: 0x00332934 File Offset: 0x00330B34
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

	// Token: 0x06008642 RID: 34370 RVA: 0x00332C3C File Offset: 0x00330E3C
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

	// Token: 0x06008643 RID: 34371 RVA: 0x00332FB7 File Offset: 0x003311B7
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

	// Token: 0x040063BB RID: 25531
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastValue;

	// Token: 0x040063BC RID: 25532
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x040063BD RID: 25533
	[PublicizedFrom(EAccessModifier.Private)]
	public float noTargetFadeTime;

	// Token: 0x040063BE RID: 25534
	[PublicizedFrom(EAccessModifier.Private)]
	public float noTargetFadeTimeMax = 3f;

	// Token: 0x040063BF RID: 25535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TargetBar.EVisibility visibility;

	// Token: 0x040063C0 RID: 25536
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultBossIcon = "ui_game_symbol_twitch_boss_bar_default";

	// Token: 0x040063C1 RID: 25537
	[PublicizedFrom(EAccessModifier.Private)]
	public GameEventManager gameEventManager;

	// Token: 0x040063C2 RID: 25538
	[PublicizedFrom(EAccessModifier.Private)]
	public BossGroup CurrentBossGroup;

	// Token: 0x040063C3 RID: 25539
	[PublicizedFrom(EAccessModifier.Private)]
	public float checkSandboxOption;

	// Token: 0x040063C4 RID: 25540
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sandboxForceShow;

	// Token: 0x040063C5 RID: 25541
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statcurrentFormatterInt = new CachedStringFormatterInt();

	// Token: 0x040063C6 RID: 25542
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> statcurrentWMaxFormatterAOfB = new CachedStringFormatter<int, int>((int _i, int _i1) => string.Format("{0}/{1}", _i, _i1));

	// Token: 0x040063C7 RID: 25543
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat statfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x02001081 RID: 4225
	public enum EVisibility
	{
		// Token: 0x040063C9 RID: 25545
		Never,
		// Token: 0x040063CA RID: 25546
		GodMode,
		// Token: 0x040063CB RID: 25547
		Always,
		// Token: 0x040063CC RID: 25548
		Boss
	}
}
