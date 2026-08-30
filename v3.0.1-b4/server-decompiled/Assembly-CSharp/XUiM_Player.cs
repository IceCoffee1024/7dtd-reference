using System;
using UnityEngine;

// Token: 0x02001112 RID: 4370
public class XUiM_Player : XUiModel
{
	// Token: 0x060089E7 RID: 35303 RVA: 0x0034A24D File Offset: 0x0034844D
	public static int GetLevel(EntityPlayer _player)
	{
		return _player.Progression.GetLevel();
	}

	// Token: 0x060089E8 RID: 35304 RVA: 0x0034A25A File Offset: 0x0034845A
	public static float GetLevelPercent(EntityPlayer _player)
	{
		return _player.Progression.GetLevelProgressPercentage();
	}

	// Token: 0x060089E9 RID: 35305 RVA: 0x0034A267 File Offset: 0x00348467
	public static int GetXPToNextLevel(EntityPlayer _player)
	{
		return _player.Progression.ExpToNextLevel;
	}

	// Token: 0x060089EA RID: 35306 RVA: 0x0034A274 File Offset: 0x00348474
	public static float GetFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value;
	}

	// Token: 0x060089EB RID: 35307 RVA: 0x0034A286 File Offset: 0x00348486
	public static float GetModifiedCurrentFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value + _player.Buffs.GetCustomVar("$foodAmount");
	}

	// Token: 0x060089EC RID: 35308 RVA: 0x0034A2A9 File Offset: 0x003484A9
	public static float GetFoodPercent(EntityPlayer _player)
	{
		return 1f - _player.Stats.Food.Value / _player.Stats.Food.ModifiedMax;
	}

	// Token: 0x060089ED RID: 35309 RVA: 0x0034A2D2 File Offset: 0x003484D2
	public static int GetFoodMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Food.Max;
	}

	// Token: 0x060089EE RID: 35310 RVA: 0x0034A2E5 File Offset: 0x003484E5
	public static float GetWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value;
	}

	// Token: 0x060089EF RID: 35311 RVA: 0x0034A2F7 File Offset: 0x003484F7
	public static float GetModifiedCurrentWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value + _player.Buffs.GetCustomVar("$waterAmount");
	}

	// Token: 0x060089F0 RID: 35312 RVA: 0x0034A31A File Offset: 0x0034851A
	public static float GetWaterPercent(EntityPlayer _player)
	{
		return _player.Stats.Water.ValuePercentUI * 100f;
	}

	// Token: 0x060089F1 RID: 35313 RVA: 0x0034A332 File Offset: 0x00348532
	public static int GetWaterMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Water.Max;
	}

	// Token: 0x060089F2 RID: 35314 RVA: 0x0034A345 File Offset: 0x00348545
	public static string GetCoreTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_coretemp"), -1);
	}

	// Token: 0x060089F3 RID: 35315 RVA: 0x0034A35D File Offset: 0x0034855D
	public static string GetOutsideTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_outsidetemp"), -1);
	}

	// Token: 0x060089F4 RID: 35316 RVA: 0x0034A375 File Offset: 0x00348575
	public static int GetZombieKills(EntityPlayer _player)
	{
		return _player.KilledZombies;
	}

	// Token: 0x060089F5 RID: 35317 RVA: 0x0034A37D File Offset: 0x0034857D
	public static int GetPlayerKills(EntityPlayer _player)
	{
		return _player.KilledPlayers;
	}

	// Token: 0x060089F6 RID: 35318 RVA: 0x0034A385 File Offset: 0x00348585
	public static int GetDeaths(EntityPlayer _player)
	{
		return _player.Died;
	}

	// Token: 0x060089F7 RID: 35319 RVA: 0x0034A38D File Offset: 0x0034858D
	public static string GetKMTraveled(EntityPlayer _player)
	{
		return (_player.distanceWalked / 1000f).ToCultureInvariantString("0.00") + " KM";
	}

	// Token: 0x060089F8 RID: 35320 RVA: 0x0034A3AF File Offset: 0x003485AF
	public static int GetItemsCrafted(EntityPlayer _player)
	{
		return (int)_player.totalItemsCrafted;
	}

	// Token: 0x060089F9 RID: 35321 RVA: 0x0034A3B7 File Offset: 0x003485B7
	public static string GetLongestLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.longestLife) * 60f);
	}

	// Token: 0x060089FA RID: 35322 RVA: 0x0034A3CC File Offset: 0x003485CC
	public static string GetCurrentLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.currentLife) * 60f);
	}

	// Token: 0x060089FB RID: 35323 RVA: 0x0034A3E1 File Offset: 0x003485E1
	public static float GetHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Value;
	}

	// Token: 0x060089FC RID: 35324 RVA: 0x0034A3F3 File Offset: 0x003485F3
	public static float GetStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Value;
	}

	// Token: 0x060089FD RID: 35325 RVA: 0x0034A405 File Offset: 0x00348605
	public static float GetMaxHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Max;
	}

	// Token: 0x060089FE RID: 35326 RVA: 0x0034A417 File Offset: 0x00348617
	public static float GetMaxStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Max;
	}

	// Token: 0x060089FF RID: 35327 RVA: 0x0034A429 File Offset: 0x00348629
	public static bool GetHasFullHealth(EntityPlayer _player)
	{
		return Mathf.Approximately(_player.Stats.Health.Max, _player.Stats.Health.Value);
	}

	// Token: 0x06008A00 RID: 35328 RVA: 0x0034A450 File Offset: 0x00348650
	public static EntityPlayer GetPlayer()
	{
		return GameManager.Instance.World.GetPrimaryPlayer();
	}

	// Token: 0x06008A01 RID: 35329 RVA: 0x0034A461 File Offset: 0x00348661
	public static EntityPlayer GetPlayer(int _id)
	{
		if (GameManager.Instance != null && GameManager.Instance.World != null)
		{
			return GameManager.Instance.World.GetEntity(_id) as EntityPlayer;
		}
		return null;
	}

	// Token: 0x06008A02 RID: 35330 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CalcDisplayProtectionValues()
	{
	}

	// Token: 0x06008A03 RID: 35331 RVA: 0x0034A494 File Offset: 0x00348694
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
