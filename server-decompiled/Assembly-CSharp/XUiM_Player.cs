using System;
using UnityEngine;

// Token: 0x02001143 RID: 4419
public class XUiM_Player : XUiModel
{
	// Token: 0x06008B44 RID: 35652 RVA: 0x00352E89 File Offset: 0x00351089
	public static int GetLevel(EntityPlayer _player)
	{
		return _player.Progression.GetLevel();
	}

	// Token: 0x06008B45 RID: 35653 RVA: 0x00352E96 File Offset: 0x00351096
	public static float GetLevelPercent(EntityPlayer _player)
	{
		return _player.Progression.GetLevelProgressPercentage();
	}

	// Token: 0x06008B46 RID: 35654 RVA: 0x00352EA3 File Offset: 0x003510A3
	public static int GetXPToNextLevel(EntityPlayer _player)
	{
		return _player.Progression.ExpToNextLevel;
	}

	// Token: 0x06008B47 RID: 35655 RVA: 0x00352EB0 File Offset: 0x003510B0
	public static float GetFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value;
	}

	// Token: 0x06008B48 RID: 35656 RVA: 0x00352EC2 File Offset: 0x003510C2
	public static float GetModifiedCurrentFood(EntityPlayer _player)
	{
		return _player.Stats.Food.Value + _player.Buffs.GetCustomVar("$foodAmount");
	}

	// Token: 0x06008B49 RID: 35657 RVA: 0x00352EE5 File Offset: 0x003510E5
	public static float GetFoodPercent(EntityPlayer _player)
	{
		return 1f - _player.Stats.Food.Value / _player.Stats.Food.ModifiedMax;
	}

	// Token: 0x06008B4A RID: 35658 RVA: 0x00352F0E File Offset: 0x0035110E
	public static int GetFoodMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Food.Max;
	}

	// Token: 0x06008B4B RID: 35659 RVA: 0x00352F21 File Offset: 0x00351121
	public static float GetWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value;
	}

	// Token: 0x06008B4C RID: 35660 RVA: 0x00352F33 File Offset: 0x00351133
	public static float GetModifiedCurrentWater(EntityPlayer _player)
	{
		return _player.Stats.Water.Value + _player.Buffs.GetCustomVar("$waterAmount");
	}

	// Token: 0x06008B4D RID: 35661 RVA: 0x00352F56 File Offset: 0x00351156
	public static float GetWaterPercent(EntityPlayer _player)
	{
		return _player.Stats.Water.ValuePercentUI * 100f;
	}

	// Token: 0x06008B4E RID: 35662 RVA: 0x00352F6E File Offset: 0x0035116E
	public static int GetWaterMax(EntityPlayer _player)
	{
		return (int)_player.Stats.Water.Max;
	}

	// Token: 0x06008B4F RID: 35663 RVA: 0x00352F81 File Offset: 0x00351181
	public static string GetCoreTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_coretemp"), -1);
	}

	// Token: 0x06008B50 RID: 35664 RVA: 0x00352F99 File Offset: 0x00351199
	public static string GetOutsideTemp(EntityPlayer _player)
	{
		return ValueDisplayFormatters.Temperature(_player.Buffs.GetCustomVar("_outsidetemp"), -1);
	}

	// Token: 0x06008B51 RID: 35665 RVA: 0x00352FB1 File Offset: 0x003511B1
	public static int GetZombieKills(EntityPlayer _player)
	{
		return _player.KilledZombies;
	}

	// Token: 0x06008B52 RID: 35666 RVA: 0x00352FB9 File Offset: 0x003511B9
	public static int GetPlayerKills(EntityPlayer _player)
	{
		return _player.KilledPlayers;
	}

	// Token: 0x06008B53 RID: 35667 RVA: 0x00352FC1 File Offset: 0x003511C1
	public static int GetDeaths(EntityPlayer _player)
	{
		return _player.Died;
	}

	// Token: 0x06008B54 RID: 35668 RVA: 0x00352FC9 File Offset: 0x003511C9
	public static string GetKMTraveled(EntityPlayer _player)
	{
		return (_player.distanceWalked / 1000f).ToCultureInvariantString("0.00") + " KM";
	}

	// Token: 0x06008B55 RID: 35669 RVA: 0x00352FEB File Offset: 0x003511EB
	public static int GetItemsCrafted(EntityPlayer _player)
	{
		return (int)_player.totalItemsCrafted;
	}

	// Token: 0x06008B56 RID: 35670 RVA: 0x00352FF3 File Offset: 0x003511F3
	public static string GetLongestLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.longestLife) * 60f);
	}

	// Token: 0x06008B57 RID: 35671 RVA: 0x00353008 File Offset: 0x00351208
	public static string GetCurrentLife(EntityPlayer _player)
	{
		return XUiM_PlayerBuffs.GetTimeString((float)((int)_player.currentLife) * 60f);
	}

	// Token: 0x06008B58 RID: 35672 RVA: 0x0035301D File Offset: 0x0035121D
	public static float GetHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Value;
	}

	// Token: 0x06008B59 RID: 35673 RVA: 0x0035302F File Offset: 0x0035122F
	public static float GetStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Value;
	}

	// Token: 0x06008B5A RID: 35674 RVA: 0x00353041 File Offset: 0x00351241
	public static float GetMaxHealth(EntityPlayer _player)
	{
		return _player.Stats.Health.Max;
	}

	// Token: 0x06008B5B RID: 35675 RVA: 0x00353053 File Offset: 0x00351253
	public static float GetMaxStamina(EntityPlayer _player)
	{
		return _player.Stats.Stamina.Max;
	}

	// Token: 0x06008B5C RID: 35676 RVA: 0x00353065 File Offset: 0x00351265
	public static bool GetHasFullHealth(EntityPlayer _player)
	{
		return Mathf.Approximately(_player.Stats.Health.Max, _player.Stats.Health.Value);
	}

	// Token: 0x06008B5D RID: 35677 RVA: 0x0035308C File Offset: 0x0035128C
	public static EntityPlayer GetPlayer()
	{
		return GameManager.Instance.World.GetPrimaryPlayer();
	}

	// Token: 0x06008B5E RID: 35678 RVA: 0x0035309D File Offset: 0x0035129D
	public static EntityPlayer GetPlayer(int _id)
	{
		if (GameManager.Instance != null && GameManager.Instance.World != null)
		{
			return GameManager.Instance.World.GetEntity(_id) as EntityPlayer;
		}
		return null;
	}

	// Token: 0x06008B5F RID: 35679 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CalcDisplayProtectionValues()
	{
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x003530D0 File Offset: 0x003512D0
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
