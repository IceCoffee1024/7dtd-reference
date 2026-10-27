using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FAB RID: 4011
[Preserve]
public class XUiC_MapStats : XUiController
{
	// Token: 0x06007E15 RID: 32277 RVA: 0x002FEC34 File Offset: 0x002FCE34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1527894770U)
		{
			if (num <= 281380209U)
			{
				if (num != 85644157U)
				{
					if (num != 195109979U)
					{
						if (num == 281380209U)
						{
							if (bindingName == "maptemperature")
							{
								value = "";
								if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
								{
									value = this.tempFormatter.Format(WeatherManager.Instance.GetCurrentTemperatureValue());
								}
								return true;
							}
						}
					}
					else if (bindingName == "mapwind")
					{
						value = "";
						if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
						{
							value = this.mapwindFormatter.Format(Mathf.RoundToInt(WeatherManager.GetWindSpeed()));
						}
						return true;
					}
				}
				else if (bindingName == "showtime")
				{
					value = "true";
					if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
					{
						value = (EffectManager.GetValue(PassiveEffects.NoTimeDisplay, null, 0f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 0f && SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.ShowDayTime)).ToString();
					}
					return true;
				}
			}
			else if (num != 304163417U)
			{
				if (num != 871453238U)
				{
					if (num == 1527894770U)
					{
						if (bindingName == "maptradertimetitle")
						{
							value = TraderInfo.GetNextTraderText();
							return true;
						}
					}
				}
				else if (bindingName == "mapdaytimetitle")
				{
					value = Localization.Get("xuiDayTime", false, null);
					return true;
				}
			}
			else if (bindingName == "playercoretemp")
			{
				value = "";
				if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
				{
					value = XUiM_Player.GetCoreTemp(this.xui.playerUI.entityPlayer);
				}
				return true;
			}
		}
		else if (num <= 1972321830U)
		{
			if (num != 1600623488U)
			{
				if (num != 1921418308U)
				{
					if (num == 1972321830U)
					{
						if (bindingName == "mapelevationtitle")
						{
							value = Localization.Get("xuiElevation", false, null);
							return true;
						}
					}
				}
				else if (bindingName == "maptradertime")
				{
					value = TraderInfo.GetNextTraderTime();
					return true;
				}
			}
			else if (bindingName == "mapdaytime")
			{
				value = "";
				if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
				{
					value = this.mapdaytimeFormatter.Format(GameManager.Instance.World.worldTime);
				}
				return true;
			}
		}
		else if (num != 2762345723U)
		{
			if (num != 3729109520U)
			{
				if (num == 4025935093U)
				{
					if (bindingName == "playercoretemptitle")
					{
						value = Localization.Get("xuiFeelsLike", false, null);
						return true;
					}
				}
			}
			else if (bindingName == "mapelevation")
			{
				value = "";
				if (XUi.IsGameRunning() && this.xui.playerUI.entityPlayer != null)
				{
					int num2 = Mathf.RoundToInt(this.xui.playerUI.entityPlayer.GetPosition().y - WeatherManager.SeaLevel());
					value = this.levelFormatter.Format(num2);
				}
				return true;
			}
		}
		else if (bindingName == "mapwindtitle")
		{
			value = Localization.Get("xuiWind", false, null);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007E16 RID: 32278 RVA: 0x002AF555 File Offset: 0x002AD755
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x04005D8F RID: 23951
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<ulong> mapdaytimeFormatter = new CachedStringFormatter<ulong>((ulong _worldTime) => ValueDisplayFormatters.WorldTime(_worldTime, "{0}/{1:00}:{2:00}"));

	// Token: 0x04005D90 RID: 23952
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> tempFormatter = new CachedStringFormatter<float>((float _f) => ValueDisplayFormatters.Temperature(_f, -1));

	// Token: 0x04005D91 RID: 23953
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt mapwindFormatter = new CachedStringFormatterInt();

	// Token: 0x04005D92 RID: 23954
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> levelFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString("+0;-#"));
}
