using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02001144 RID: 4420
public class XUiM_PlayerBuffs : XUiModel
{
	// Token: 0x06008B62 RID: 35682 RVA: 0x003531C0 File Offset: 0x003513C0
	public static List<string> GetInfoFromBuffNotification(EntityUINotification _notification, BuffValue _overridenBuff, XUi _xui)
	{
		List<string> list = new List<string>();
		string buffDisplayInfo = XUiM_PlayerBuffs.GetBuffDisplayInfo(_notification, null);
		if (buffDisplayInfo != null)
		{
			list.Add(XUiM_PlayerBuffs.StringFormatHandler(buffDisplayInfo, Localization.Get("xuiBuffStatDuration", false, null)));
		}
		BuffValue buff = _notification.Buff;
		BuffClass buffClass = buff.BuffClass;
		bool flag;
		if (buffClass == null)
		{
			flag = null != null;
		}
		else
		{
			MinEffectController effects = buffClass.Effects;
			flag = ((effects != null) ? effects.EffectGroups : null) != null;
		}
		if (flag && buff.BuffClass.Effects.EffectGroups.Count > 0)
		{
			for (int i = 0; i < buff.BuffClass.Effects.EffectGroups.Count; i++)
			{
				for (int j = 0; j < buff.BuffClass.Effects.EffectGroups[i].PassiveEffects.Count; j++)
				{
					PassiveEffect passiveEffect = buff.BuffClass.Effects.EffectGroups[i].PassiveEffects[j];
					if (passiveEffect != null)
					{
						passiveEffect.AddColoredInfoStrings(ref list, -1f);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06008B63 RID: 35683 RVA: 0x003532C0 File Offset: 0x003514C0
	public static string GetInfoFromBuff(EntityPlayerLocal _localPlayer, EntityUINotification _notification, BuffValue _overridenBuff)
	{
		StringBuilder stringBuilder = new StringBuilder();
		BuffValue buff = _notification.Buff;
		List<string> list = new List<string>();
		BuffClass buffClass = buff.BuffClass;
		bool flag;
		if (buffClass == null)
		{
			flag = null != null;
		}
		else
		{
			MinEffectController effects = buffClass.Effects;
			flag = ((effects != null) ? effects.EffectGroups : null) != null;
		}
		if (flag && buff.BuffClass.Effects.EffectGroups.Count > 0)
		{
			for (int i = 0; i < buff.BuffClass.Effects.EffectGroups.Count; i++)
			{
				for (int j = 0; j < buff.BuffClass.Effects.EffectGroups[i].PassiveEffects.Count; j++)
				{
					PassiveEffect passiveEffect = buff.BuffClass.Effects.EffectGroups[i].PassiveEffects[j];
					if (passiveEffect != null)
					{
						passiveEffect.AddColoredInfoStrings(ref list, buff.DurationInSeconds);
					}
				}
			}
		}
		string text = Utils.ColorToHex(new Color32(222, 206, 163, byte.MaxValue));
		for (int k = 0; k < list.Count; k++)
		{
			stringBuilder.Append(list[k]);
		}
		return stringBuilder.ToString().Replace("REPLACE_COLOR", text);
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x00353400 File Offset: 0x00351600
	public static string GetTimeString(float _currentTime)
	{
		int num = (int)Math.Floor((double)(_currentTime / 3600f));
		int num2 = (int)Math.Floor((double)((_currentTime - (float)(num * 3600)) / 60f));
		int num3 = (int)Math.Floor((double)(_currentTime % 60f));
		if (num3 == 0 && num2 == 0 && num == 0)
		{
			return "<1" + Localization.Get("timeAbbreviationMinutes", false, null);
		}
		return string.Format("{0}{1}{2}", (num > 0) ? string.Format("{0}{1} ", num, Localization.Get("timeAbbreviationHours", false, null)) : "", (num2 > 0) ? string.Format("{0}{1} ", num2, Localization.Get("timeAbbreviationMinutes", false, null)) : "", (num3 > 0) ? string.Format("{0}{1} ", num3, Localization.Get("timeAbbreviationSeconds", false, null)) : "");
	}

	// Token: 0x06008B65 RID: 35685 RVA: 0x003534E2 File Offset: 0x003516E2
	public static string GetBuffTimerDurationString(float _duration)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)Mathf.FloorToInt(_duration * 20f));
	}

	// Token: 0x06008B66 RID: 35686 RVA: 0x003534F6 File Offset: 0x003516F6
	public static string GetBuffTimerTimeLeftString(float _duration, float _maxDuration)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)((_maxDuration - _duration) * 20f)));
	}

	// Token: 0x06008B67 RID: 35687 RVA: 0x00353508 File Offset: 0x00351708
	public static string GetBuffTimeLeftString(BuffValue _buff)
	{
		if (_buff.BuffClass == null)
		{
			return "";
		}
		if (_buff.BuffClass.DurationMax <= 0f)
		{
			return "";
		}
		int num = (int)(_buff.BuffClass.DurationMax * (float)((_buff.BuffClass.StackType == BuffEffectStackTypes.Duration) ? _buff.StackEffectMultiplier : 1) - _buff.DurationInSeconds + 0.9f);
		int num2 = num / 60;
		int num3 = num2 / 60;
		if (num3 > 0)
		{
			return string.Format("{0}H", num3);
		}
		if (num2 > 0)
		{
			return string.Format("{0}M", num2);
		}
		return string.Format("{0}S", num);
	}

	// Token: 0x06008B68 RID: 35688 RVA: 0x003535B4 File Offset: 0x003517B4
	public static string FormatWorldTimeString(int _duration)
	{
		int num = _duration / 24000;
		_duration -= num * 24000;
		if (num > 0)
		{
			if (_duration >= 23000)
			{
				return string.Format("{0}.9 {1}", num, Localization.Get("xuiBuffStatDays", false, null));
			}
			return string.Format("{0}.{1} {2}", num, (int)Mathf.Floor((float)_duration / 24000f * 10f + 0.5f), Localization.Get("xuiBuffStatDays", false, null));
		}
		else
		{
			int num2 = _duration / 1000;
			_duration -= num2 * 1000;
			if (_duration >= 900)
			{
				return string.Format("{0}.9 {1}", num2, Localization.Get("xuiBuffStatHours", false, null));
			}
			return string.Format("{0}.{1} {2}", num2, (int)Mathf.Floor((float)_duration / 1000f * 10f + 0.5f), Localization.Get("xuiBuffStatHours", false, null));
		}
	}

	// Token: 0x06008B69 RID: 35689 RVA: 0x003536AC File Offset: 0x003518AC
	public static string GetBuffDisplayInfo(EntityUINotification _notification, BuffValue _overridenBuff = null)
	{
		if (_notification.Buff != null)
		{
			BuffValue buff = _notification.Buff;
			string buffTimeLeftString = XUiM_PlayerBuffs.GetBuffTimeLeftString(buff);
			if (buffTimeLeftString != null && buff.BuffClass.DurationMax != 0f)
			{
				return buffTimeLeftString;
			}
			if (_notification.DisplayMode == EnumEntityUINotificationDisplayMode.IconPlusCurrentValue)
			{
				string units = _notification.Units;
				if (units == "%")
				{
					return ((int)(_notification.CurrentValue * 100f)).ToString() + "%";
				}
				if (units == "°")
				{
					return ValueDisplayFormatters.Temperature(_notification.CurrentValue, -1);
				}
				if (!(units == "cvar"))
				{
					if (units == "duration")
					{
						return XUiM_PlayerBuffs.GetCVarValueAsTimeString(_notification.Buff.BuffClass.DurationMax - _notification.Buff.DurationInSeconds);
					}
					if (_notification.Buff.BuffClass.DisplayValueKey == null)
					{
						return ((int)_notification.CurrentValue).ToString();
					}
					if (_notification.Buff.BuffClass.DisplayValueFormat == BuffClass.CVarDisplayFormat.Time)
					{
						return string.Format(Localization.Get(_notification.Buff.BuffClass.DisplayValueKey, false, null), XUiM_PlayerBuffs.GetCVarValueAsTimeString(_notification.CurrentValue));
					}
					return string.Format(Localization.Get(_notification.Buff.BuffClass.DisplayValueKey, false, null), _notification.CurrentValue);
				}
				else
				{
					BuffClass buffClass = _notification.Buff.BuffClass;
					if (buffClass.DisplayValueKey != null)
					{
						string text = Localization.Get(buffClass.DisplayValueKey, false, null);
						BuffClass.CVarDisplayFormat displayValueFormat = buffClass.DisplayValueFormat;
						if (displayValueFormat == BuffClass.CVarDisplayFormat.Time)
						{
							return string.Format(text, XUiM_PlayerBuffs.GetCVarValueAsTimeString(_notification.CurrentValue));
						}
						if (displayValueFormat == BuffClass.CVarDisplayFormat.Degrees)
						{
							return string.Format(text, ValueDisplayFormatters.Temperature(_notification.CurrentValue, -1));
						}
						return string.Format(text, _notification.CurrentValue);
					}
					else
					{
						if (buffClass.DisplayValueFormat == BuffClass.CVarDisplayFormat.Time)
						{
							return XUiM_PlayerBuffs.GetCVarValueAsTimeString(_notification.CurrentValue);
						}
						return ((int)_notification.CurrentValue).ToString();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x06008B6A RID: 35690 RVA: 0x003538AC File Offset: 0x00351AAC
	public static string GetCVarValueAsTimeString(float _cvarValue)
	{
		if (_cvarValue == 0f)
		{
			return "";
		}
		if (XUiM_PlayerBuffs.hourAbbrev == null)
		{
			XUiM_PlayerBuffs.hourAbbrev = Localization.Get("timeAbbreviationHours", false, null);
			XUiM_PlayerBuffs.minuteAbbrev = Localization.Get("timeAbbreviationMinutes", false, null);
			XUiM_PlayerBuffs.secondAbbrev = Localization.Get("timeAbbreviationSeconds", false, null);
		}
		int num = (int)Math.Floor((double)(_cvarValue / 3600f));
		int num2 = (int)Math.Floor((double)((_cvarValue - (float)(num * 3600)) / 60f));
		int num3 = (int)Math.Floor((double)(_cvarValue % 60f));
		if (num > 0)
		{
			if (num >= 5 || num2 == 0)
			{
				return string.Format("{0}{1}", num, XUiM_PlayerBuffs.hourAbbrev);
			}
			return string.Format("{0}{1} {2}{3}", new object[]
			{
				num,
				XUiM_PlayerBuffs.hourAbbrev,
				num2,
				XUiM_PlayerBuffs.minuteAbbrev
			});
		}
		else
		{
			if (num2 <= 0)
			{
				return string.Format("{0}{1}", num3, XUiM_PlayerBuffs.secondAbbrev);
			}
			if (num2 >= 5 || num3 == 0)
			{
				return string.Format("{0}{1}", num2, XUiM_PlayerBuffs.minuteAbbrev);
			}
			return string.Format("{0}{1} {2}{3}", new object[]
			{
				num2,
				XUiM_PlayerBuffs.minuteAbbrev,
				num3,
				XUiM_PlayerBuffs.secondAbbrev
			});
		}
	}

	// Token: 0x06008B6B RID: 35691 RVA: 0x003539F4 File Offset: 0x00351BF4
	public static string ConvertToTimeString(float _timeSeconds)
	{
		if (_timeSeconds == 0f)
		{
			return "";
		}
		if (XUiM_PlayerBuffs.hourAbbrev == null)
		{
			XUiM_PlayerBuffs.hourAbbrev = Localization.Get("timeAbbreviationHours", false, null);
			XUiM_PlayerBuffs.minuteAbbrev = Localization.Get("timeAbbreviationMinutes", false, null);
			XUiM_PlayerBuffs.secondAbbrev = Localization.Get("timeAbbreviationSeconds", false, null);
		}
		int num = (int)Math.Floor((double)(_timeSeconds / 3600f));
		int num2 = (int)Math.Floor((double)((_timeSeconds - (float)(num * 3600)) / 60f));
		int num3 = (int)Math.Floor((double)(_timeSeconds % 60f));
		if (num > 0)
		{
			if (num2 == 0)
			{
				return string.Format("{0}{1}", num, XUiM_PlayerBuffs.hourAbbrev);
			}
			return string.Format("{0}{1} {2}{3}", new object[]
			{
				num,
				XUiM_PlayerBuffs.hourAbbrev,
				num2,
				XUiM_PlayerBuffs.minuteAbbrev
			});
		}
		else
		{
			if (num2 <= 0)
			{
				return string.Format("{0}{1}", num3, XUiM_PlayerBuffs.secondAbbrev);
			}
			if (num3 == 0)
			{
				return string.Format("{0}{1}", num2, XUiM_PlayerBuffs.minuteAbbrev);
			}
			return string.Format("{0}{1} {2}{3}", new object[]
			{
				num2,
				XUiM_PlayerBuffs.minuteAbbrev,
				num3,
				XUiM_PlayerBuffs.secondAbbrev
			});
		}
	}

	// Token: 0x06008B6C RID: 35692 RVA: 0x001289D2 File Offset: 0x00126BD2
	[PublicizedFrom(EAccessModifier.Private)]
	public static string StringFormatHandler(string _title, object _value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", _title, _value);
	}

	// Token: 0x04006754 RID: 26452
	[PublicizedFrom(EAccessModifier.Private)]
	public static string minuteAbbrev;

	// Token: 0x04006755 RID: 26453
	[PublicizedFrom(EAccessModifier.Private)]
	public static string secondAbbrev;

	// Token: 0x04006756 RID: 26454
	[PublicizedFrom(EAccessModifier.Private)]
	public static string hourAbbrev;
}
