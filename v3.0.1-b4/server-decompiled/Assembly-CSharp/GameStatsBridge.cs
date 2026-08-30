using System;

// Token: 0x020011EB RID: 4587
public static class GameStatsBridge
{
	// Token: 0x06009252 RID: 37458 RVA: 0x003750F8 File Offset: 0x003732F8
	public static void Init()
	{
		GameStats.OnChangedDelegates -= GameStatsBridge.GameStats_OnChangedDelegates;
		GameStats.OnChangedDelegates += GameStatsBridge.GameStats_OnChangedDelegates;
		GameStatsBridge.UpdateStaticFields(EnumGameStats.XPMultiplier, GameStats.GetInt(EnumGameStats.XPMultiplier));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.GlobalGSModifier, GameStats.GetInt(EnumGameStats.GlobalGSModifier));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.BiomeGSModifier, GameStats.GetInt(EnumGameStats.BiomeGSModifier));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.GlobalLSModifier, GameStats.GetInt(EnumGameStats.GlobalLSModifier));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.BiomeLSModifier, GameStats.GetInt(EnumGameStats.BiomeLSModifier));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.BlockDamageAI, GameStats.GetInt(EnumGameStats.BlockDamageAI));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.BlockDamageAIBM, GameStats.GetInt(EnumGameStats.BlockDamageAIBM));
		GameStatsBridge.UpdateStaticFields(EnumGameStats.LootAbundance, GameStats.GetInt(EnumGameStats.LootAbundance));
	}

	// Token: 0x06009253 RID: 37459 RVA: 0x003751BF File Offset: 0x003733BF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void GameStats_OnChangedDelegates(EnumGameStats _gameState, object _newValue)
	{
		GameStatsBridge.UpdateStaticFields(_gameState, _newValue);
	}

	// Token: 0x06009254 RID: 37460 RVA: 0x003751C8 File Offset: 0x003733C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ToFloatPercent(object _value)
	{
		if (_value == null)
		{
			return 0f;
		}
		float num;
		try
		{
			num = Convert.ToSingle(_value) / 100f;
		}
		catch (Exception ex)
		{
			Log.Warning(string.Format("GameStatsBridge: unable to convert {0} ({1}) to float: {2}", _value.GetType(), _value, ex.Message));
			num = 0f;
		}
		return num;
	}

	// Token: 0x06009255 RID: 37461 RVA: 0x00375224 File Offset: 0x00373424
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateStaticFields(EnumGameStats _gameState, object _newValue)
	{
		if (_gameState != EnumGameStats.XPMultiplier)
		{
			switch (_gameState)
			{
			case EnumGameStats.BlockDamageAI:
				ItemActionAttack.EntityBlockDamagePercent = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			case EnumGameStats.BlockDamageAIBM:
				ItemActionAttack.BMBlockDamagePercent = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			case EnumGameStats.LootAbundance:
				LootContainer.GlobalCountModifier = GameStatsBridge.ToFloatPercent(_newValue);
				break;
			case EnumGameStats.LootRespawnDays:
				break;
			case EnumGameStats.GlobalGSModifier:
				EntityPlayer.GlobalGameStageModifier = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			case EnumGameStats.BiomeGSModifier:
				EntityPlayer.BiomeGameStageModifier = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			case EnumGameStats.GlobalLSModifier:
				EntityPlayer.GlobalLootStageModifier = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			case EnumGameStats.BiomeLSModifier:
				EntityPlayer.BiomeLootStageModifier = GameStatsBridge.ToFloatPercent(_newValue);
				return;
			default:
				return;
			}
			return;
		}
		Progression.XPGain = GameStatsBridge.ToFloatPercent(_newValue);
	}
}
