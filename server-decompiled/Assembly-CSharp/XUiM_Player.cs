using System;
using UnityEngine;

// Token: 0x02001153 RID: 4435
public class XUiM_Player : XUiModel
{
	// Token: 0x06008B98 RID: 35736 RVA: 0x00354F39 File Offset: 0x00353139
	public static int GetLevel(EntityPlayer _player)
	{
		return _player.Progression.GetLevel();
	}

	// Token: 0x06008B99 RID: 35737 RVA: 0x00354F46 File Offset: 0x00353146
	public static float GetLevelPercent(EntityPlayer _player)
	{
		return _player.Progression.GetLevelProgressPercentage();
	}

	// Token: 0x06008B9A RID: 35738 RVA: 0x00354F53 File Offset: 0x00353153
	public static int GetXPToNextLevel(EntityPlayer _player)
	{
		return _player.Progression.ExpToNextLevel;
	}

	// Token: 0x06008B9B RID: 35739 RVA: 0x00354F60 File Offset: 0x00353160
	public static float GetFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value;
	}

	// Token: 0x06008B9C RID: 35740 RVA: 0x00354F72 File Offset: 0x00353172
	public static float GetModifiedCurrentFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value + _player.Buffs.GetCustomVar("$foodAmount");
	}

	// Token: 0x06008B9D RID: 35741 RVA: 0x00354F95 File Offset: 0x00353195
	public static float GetFoodPercent(EntityPlayer _player)
	{
		return 1f - _player.Stats.Food.Value / _player.Stats.Food.ModifiedMax;
	}

	// Token: 0x06008B9E RID: 35742 RVA: 0x00354FBE File Offset: 0x003531BE
	public static int GetFoodMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Food.Max;
	}

	// Token: 0x06008B9F RID: 35743 RVA: 0x00354FD1 File Offset: 0x003531D1
	public static float GetWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value;
	}

	// Token: 0x06008BA0 RID: 35744 RVA: 0x00354FE3 File Offset: 0x003531E3
	public static float GetModifiedCurrentWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value + _player.Buffs.GetCustomVar("$waterAmount");
	}

	// Token: 0x06008BA1 RID: 35745 RVA: 0x00355006 File Offset: 0x00353206
	public static float GetWaterPercent(EntityPlayer _player)
	{
		return _player.Stats.Water.ValuePercentUI * 100f;
	}

	// Token: 0x06008BA2 RID: 35746 RVA: 0x0035501E File Offset: 0x0035321E
	public static int GetWaterMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Water.Max;
	}

	// Token: 0x06008BA3 RID: 35747 RVA: 0x00355031 File Offset: 0x00353231
	public static string GetCoreTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_coretemp"), -1);
	}

	// Token: 0x06008BA4 RID: 35748 RVA: 0x00355049 File Offset: 0x00353249
	public static string GetOutsideTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_outsidetemp"), -1);
	}

	// Token: 0x06008BA5 RID: 35749 RVA: 0x00355061 File Offset: 0x00353261
	public static int GetZombieKills(EntityPlayer _player)
	{
		return _player.KilledZombies;
	}

	// Token: 0x06008BA6 RID: 35750 RVA: 0x00355069 File Offset: 0x00353269
	public static int GetPlayerKills(EntityPlayer _player)
	{
		return _player.KilledPlayers;
	}

	// Token: 0x06008BA7 RID: 35751 RVA: 0x00355071 File Offset: 0x00353271
	public static int GetDeaths(EntityPlayer _player)
	{
		return _player.Died;
	}

	// Token: 0x06008BA8 RID: 35752 RVA: 0x00355079 File Offset: 0x00353279
	public static string GetKMTraveled(EntityPlayer _player)
	{
		return (_player.distanceWalked / 1000f).ToCultureInvariantString("0.00") + " KM";
	}

	// Token: 0x06008BA9 RID: 35753 RVA: 0x0035509B File Offset: 0x0035329B
	public static int GetItemsCrafted(EntityPlayer _player)
	{
		return (int)_player.totalItemsCrafted;
	}

	// Token: 0x06008BAA RID: 35754 RVA: 0x003550A3 File Offset: 0x003532A3
	public static string GetLongestLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.longestLife) * 60f);
	}

	// Token: 0x06008BAB RID: 35755 RVA: 0x003550B8 File Offset: 0x003532B8
	public static string GetCurrentLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.currentLife) * 60f);
	}

	// Token: 0x06008BAC RID: 35756 RVA: 0x003550CD File Offset: 0x003532CD
	public static float GetHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Value;
	}

	// Token: 0x06008BAD RID: 35757 RVA: 0x003550DF File Offset: 0x003532DF
	public static float GetStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Value;
	}

	// Token: 0x06008BAE RID: 35758 RVA: 0x003550F1 File Offset: 0x003532F1
	public static float GetMaxHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Max;
	}

	// Token: 0x06008BAF RID: 35759 RVA: 0x00355103 File Offset: 0x00353303
	public static float GetMaxStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Max;
	}

	// Token: 0x06008BB0 RID: 35760 RVA: 0x00355115 File Offset: 0x00353315
	public static bool GetHasFullHealth(EntityPlayer _player)
	{
		return Mathf.Approximately(_player.Stats.Health.Max, _player.Stats.Health.Value);
	}

	// Token: 0x06008BB1 RID: 35761 RVA: 0x0035513C File Offset: 0x0035333C
	public static EntityPlayer GetPlayer()
	{
		return GameManager.Instance.World.GetPrimaryPlayer();
	}

	// Token: 0x06008BB2 RID: 35762 RVA: 0x0035514D File Offset: 0x0035334D
	public static EntityPlayer GetPlayer(int _id)
	{
		if (GameManager.Instance != null && GameManager.Instance.World != null)
		{
			return GameManager.Instance.World.GetEntity(_id) as EntityPlayer;
		}
		return null;
	}

	// Token: 0x06008BB3 RID: 35763 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CalcDisplayProtectionValues()
	{
	}

	// Token: 0x06008BB4 RID: 35764 RVA: 0x00355180 File Offset: 0x00353380
	public static string GetStatValue(PassiveEffects _effect, EntityPlayer _player, DisplayInfoEntry _entry, FastTags<TagGroup.Global> _overrideMovementTag)
	{
		FastTags<TagGroup.Global> fastTags = _player.generalTags;
		if (_entry.TagsSet)
		{
			fastTags = _entry.Tags;
		}
		if (_overrideMovementTag.IsEmpty)
		{
			fastTags |= EntityAlive.MovementTagRunning;
		}
		else
		{
			fastTags |= _overrideMovementTag;
		}
		float num = EffectManager.GetValue(_effect, null, 0f, _player, null, fastTags, true, true, true, true, true, 1, true, true);
		if (_entry.DisplayType == DisplayInfoEntry.DisplayTypes.Percent)
		{
			num *= 100f;
			num = Mathf.Floor(num);
			if (_entry.ShowInverted)
			{
				num -= 100f;
			}
			return num.ToString("0") + "%";
		}
		if (_entry.DisplayType == DisplayInfoEntry.DisplayTypes.Time)
		{
			return XUiM_PlayerBuffs.GetCVarValueAsTimeString(num);
		}
		if (_entry.DisplayType == DisplayInfoEntry.DisplayTypes.Integer)
		{
			num = Mathf.Floor(num);
		}
		else
		{
			num *= 100f;
			num = Mathf.Floor(num);
			num /= 100f;
		}
		if (_entry.ShowInverted)
		{
			num -= 1f;
		}
		return num.ToString("0.##");
	}
}
