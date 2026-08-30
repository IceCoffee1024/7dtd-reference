using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class ItemValue : IEquatable<ItemValue>
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x00122648 File Offset: 0x00120848
	public static ItemValue None
	{
		get
		{
			return new ItemValue(0, false);
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x00122651 File Offset: 0x00120851
	public int MaxUseTimes
	{
		get
		{
			return ItemValue.ModMaxUseTimes(this.MaxUseTimesBase, this);
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0012265F File Offset: 0x0012085F
	public int MaxUseTimesUI
	{
		get
		{
			return this.MaxUseTimesBase;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x00122668 File Offset: 0x00120868
	public int MaxUseTimesBase
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			ItemClass itemClass = this.ItemClass;
			return (int)EffectManager.GetValue(PassiveEffects.DegradationMax, this, 0f, null, null, (itemClass != null) ? itemClass.ItemTags : FastTags<TagGroup.Global>.none, true, true, true, true, true, 1, true, false);
		}
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x001226A4 File Offset: 0x001208A4
	public static int ModMaxUseTimes(int _value, ItemValue _iv)
	{
		if (_value <= 0)
		{
			return _value;
		}
		float num;
		if (!_iv.TryGetMetadata("DurabilityModifier", out num))
		{
			return _value;
		}
		_value = Mathf.RoundToInt((float)_value * num);
		_value = Mathf.Max(_value, 1);
		return _value;
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06002DBA RID: 11706 RVA: 0x001226E0 File Offset: 0x001208E0
	// (set) Token: 0x06002DBB RID: 11707 RVA: 0x00122703 File Offset: 0x00120903
	public float MaxDurabilityModifier
	{
		get
		{
			float num;
			if (!this.TryGetMetadata("DurabilityModifier", out num))
			{
				return 1f;
			}
			return num;
		}
		set
		{
			if (value == 1f)
			{
				this.RemoveMetaData("DurabilityModifier");
				return;
			}
			this.SetMetadata("DurabilityModifier", value);
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06002DBC RID: 11708 RVA: 0x00122728 File Offset: 0x00120928
	public float PercentUsesLeft
	{
		get
		{
			int maxUseTimes = this.MaxUseTimes;
			if (maxUseTimes > 0)
			{
				return 1f - Utils.FastClamp01(this.UseTimes / (float)maxUseTimes);
			}
			return 1f;
		}
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x0012275C File Offset: 0x0012095C
	public bool TryGetMetadata(string key, out int value)
	{
		object obj;
		if (!this.TryGetMetadata(key, out obj, TypedMetadataValue.TypeTag.Integer))
		{
			value = 0;
			return false;
		}
		value = (int)obj;
		return true;
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x00122784 File Offset: 0x00120984
	public bool TryGetMetadata(string key, out float value)
	{
		object obj;
		if (!this.TryGetMetadata(key, out obj, TypedMetadataValue.TypeTag.Float))
		{
			value = 0f;
			return false;
		}
		value = (float)obj;
		return true;
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x001227B0 File Offset: 0x001209B0
	public bool TryGetMetadata(string key, out string value)
	{
		object obj;
		if (!this.TryGetMetadata(key, out obj, TypedMetadataValue.TypeTag.String))
		{
			value = null;
			return false;
		}
		value = (string)obj;
		return true;
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x001227D8 File Offset: 0x001209D8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetMetadata(string key, out object value, TypedMetadataValue.TypeTag typeTag = TypedMetadataValue.TypeTag.None)
	{
		if (this.Metadata == null)
		{
			value = null;
			return false;
		}
		TypedMetadataValue typedMetadataValue;
		if (!this.Metadata.TryGetValue(key, out typedMetadataValue))
		{
			value = null;
			return false;
		}
		if (typeTag != TypedMetadataValue.TypeTag.None && typedMetadataValue.GetTypeTag() != typeTag)
		{
			value = null;
			return false;
		}
		value = typedMetadataValue.GetValue();
		return true;
	}

	// Token: 0x06002DC1 RID: 11713 RVA: 0x00122824 File Offset: 0x00120A24
	public bool HasMetadata(string key, TypedMetadataValue.TypeTag typeTag = TypedMetadataValue.TypeTag.None)
	{
		if (this.Metadata == null)
		{
			return false;
		}
		if (typeTag == TypedMetadataValue.TypeTag.None)
		{
			return this.Metadata.ContainsKey(key);
		}
		TypedMetadataValue typedMetadataValue;
		return this.Metadata.TryGetValue(key, out typedMetadataValue) && typedMetadataValue.GetTypeTag() == typeTag;
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x00122868 File Offset: 0x00120A68
	public object GetMetadata(string key)
	{
		if (this.Metadata == null)
		{
			return false;
		}
		TypedMetadataValue typedMetadataValue;
		if (this.Metadata.TryGetValue(key, out typedMetadataValue))
		{
			return typedMetadataValue.GetValue();
		}
		return null;
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x0012289C File Offset: 0x00120A9C
	public void SetMetadata(string key, int value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.Integer);
	}

	// Token: 0x06002DC4 RID: 11716 RVA: 0x001228AC File Offset: 0x00120AAC
	public void SetMetadata(string key, float value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.Float);
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x001228BC File Offset: 0x00120ABC
	public void SetMetadata(string key, string value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.String);
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x001228C7 File Offset: 0x00120AC7
	public void SetMetadata(string key, object value, string typeTag)
	{
		this.SetMetadata(key, value, TypedMetadataValue.StringToTag(typeTag));
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x001228D8 File Offset: 0x00120AD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMetadata(string key, object value, TypedMetadataValue.TypeTag typeTag)
	{
		if (this.Metadata == null)
		{
			this.Metadata = new Dictionary<string, TypedMetadataValue>();
		}
		TypedMetadataValue typedMetadataValue;
		if (this.Metadata.TryGetValue(key, out typedMetadataValue))
		{
			if (!typedMetadataValue.SetValue(value))
			{
				Log.Warning(string.Format("Can not update Metadata value '{0}' on ItemValue, type of value '{1}' ({2}) does not match existing TypeTag ({3}). From: {4}", new object[]
				{
					key,
					value,
					value.GetType().Name,
					typedMetadataValue.GetTypeTag(),
					StackTraceUtility.ExtractStackTrace()
				}));
				return;
			}
		}
		else
		{
			TypedMetadataValue typedMetadataValue2;
			if (TypedMetadataValue.TryCreate(value, typeTag, out typedMetadataValue2))
			{
				this.Metadata.Add(key, typedMetadataValue2);
				return;
			}
			Log.Warning(string.Format("Can not set Metadata key '{0}' on ItemValue, type of value '{1}' ({2}) does not match TypeTag ({3}). From: {4}", new object[]
			{
				key,
				value,
				value.GetType().Name,
				typeTag,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x001229AC File Offset: 0x00120BAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMetadata(string key, TypedMetadataValue tmv)
	{
		if (this.Metadata == null)
		{
			this.Metadata = new Dictionary<string, TypedMetadataValue>();
		}
		TypedMetadataValue typedMetadataValue;
		if (this.Metadata.TryGetValue(key, out typedMetadataValue))
		{
			object value = tmv.GetValue();
			if (!typedMetadataValue.SetValue(value))
			{
				Log.Warning(string.Format("Can not update Metadata value '{0}' on ItemValue, type of value '{1}' ({2}) does not match existing TypeTag ({3}). From: {4}", new object[]
				{
					key,
					value,
					value.GetType().Name,
					typedMetadataValue.GetTypeTag(),
					StackTraceUtility.ExtractStackTrace()
				}));
				return;
			}
		}
		else
		{
			this.Metadata.Add(key, tmv);
		}
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x00122A3C File Offset: 0x00120C3C
	public bool RemoveMetaData(string name)
	{
		bool flag = false;
		if (this.Metadata != null)
		{
			flag = this.Metadata.Remove(name);
		}
		return flag;
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06002DCA RID: 11722 RVA: 0x00122A64 File Offset: 0x00120C64
	public bool HasQuality
	{
		get
		{
			ItemClass itemClass = this.ItemClass;
			return itemClass != null && (itemClass.HasQuality || itemClass is ItemClassModifier);
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x00122A90 File Offset: 0x00120C90
	public bool HasModSlots
	{
		get
		{
			return this.Modifications.Length != 0;
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06002DCC RID: 11724 RVA: 0x00122A9C File Offset: 0x00120C9C
	public bool IsMod
	{
		get
		{
			ItemClass itemClass = this.ItemClass;
			return itemClass != null && itemClass is ItemClassModifier;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06002DCD RID: 11725 RVA: 0x00122AC0 File Offset: 0x00120CC0
	public bool IsShapeHelperBlock
	{
		get
		{
			ItemClassBlock itemClassBlock = this.ItemClass as ItemClassBlock;
			return itemClassBlock != null && itemClassBlock.GetBlock().SelectAlternates;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06002DCE RID: 11726 RVA: 0x00122AEC File Offset: 0x00120CEC
	public ItemClass ItemClass
	{
		get
		{
			if (this.type < 0 || ItemClass.list == null || this.type >= ItemClass.list.Length)
			{
				return null;
			}
			ItemClass itemClass = ItemClass.list[this.type];
			if (itemClass is ItemClassQuest)
			{
				return ItemClassQuest.GetItemQuestById(this.Seed);
			}
			return itemClass;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06002DCF RID: 11727 RVA: 0x00122B3C File Offset: 0x00120D3C
	public ItemClass ItemClassOrMissing
	{
		get
		{
			ItemClass itemClass = this.ItemClass;
			if (itemClass != null)
			{
				return itemClass;
			}
			return ItemClass.MissingItem;
		}
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x00122B5A File Offset: 0x00120D5A
	public ItemValue()
	{
		this.Modifications = ItemValue.emptyItemValueArray;
		this.CosmeticMods = ItemValue.emptyItemValueArray;
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x00122B78 File Offset: 0x00120D78
	public ItemValue(int _type, bool _bCreateDefaultParts = false)
		: this(_type, 1, 6, _bCreateDefaultParts, null, 1f)
	{
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x00122B8C File Offset: 0x00120D8C
	public ItemValue(int _type, int minQuality, int maxQuality, bool _bCreateDefaultModItems = false, string[] modsToInstall = null, float modInstallDescendingChance = 1f)
	{
		this.type = _type;
		this.Modifications = ItemValue.emptyItemValueArray;
		this.CosmeticMods = ItemValue.emptyItemValueArray;
		if (this.type == 0)
		{
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		this.Seed = (ushort)((utcNow - ItemValue.baseDate).Seconds + utcNow.Millisecond + this.type);
		if (!ThreadManager.IsMainThread())
		{
			return;
		}
		ItemClass itemClass = this.ItemClass;
		if (itemClass == null)
		{
			return;
		}
		GameRandom gameRandom = null;
		if (itemClass.HasQuality)
		{
			if (minQuality == maxQuality)
			{
				this.Quality = (ushort)minQuality;
			}
			else
			{
				gameRandom = GameRandomManager.Instance.CreateGameRandom((int)this.Seed);
				this.Quality = (ushort)gameRandom.RandomRange(minQuality, maxQuality + 1);
			}
		}
		if (itemClass is ItemClassModifier)
		{
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return;
		}
		if (itemClass.Stacknumber.Value > 1)
		{
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return;
		}
		this.Modifications = new ItemValue[this.CalcModSlotCount()];
		this.CosmeticMods = new ItemValue[itemClass.HasAnyTags(ItemClassModifier.CosmeticItemTags) ? 1 : 0];
		if (_bCreateDefaultModItems)
		{
			if (gameRandom == null)
			{
				gameRandom = GameRandomManager.Instance.CreateGameRandom((int)this.Seed);
			}
			this.createDefaultModItems(itemClass, gameRandom, modsToInstall, modInstallDescendingChance);
		}
		GameRandomManager.Instance.FreeGameRandom(gameRandom);
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x00122CCC File Offset: 0x00120ECC
	public void Clear()
	{
		this.type = 0;
		this.UseTimes = 0f;
		this.Quality = 0;
		this.Meta = 0;
		this.Seed = 0;
		this.Stats = null;
		this.Modifications = new ItemValue[0];
		this.CosmeticMods = new ItemValue[0];
		this.Metadata = null;
		this.SelectedAmmoTypeIndex = 0;
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x00122D30 File Offset: 0x00120F30
	[PublicizedFrom(EAccessModifier.Private)]
	public int CalcModSlotCount()
	{
		int num = Utils.FastMax(0, (int)(this.Quality - 1));
		return Utils.FastMin(255, (int)EffectManager.GetValue(PassiveEffects.ModSlots, this, (float)num, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, false, false));
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x00122D74 File Offset: 0x00120F74
	[PublicizedFrom(EAccessModifier.Private)]
	public void createDefaultModItems(ItemClass ic, GameRandom random, string[] modsToInstall, float modInstallDescendingChance)
	{
		FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.none;
		bool flag = false;
		bool flag2 = false;
		if (modsToInstall != null && modsToInstall.Length != 0)
		{
			float num = modInstallDescendingChance;
			if (!ic.ItemTags.IsEmpty)
			{
				int num2 = 0;
				for (int i = 0; i < modsToInstall.Length; i++)
				{
					ItemClassModifier itemClassModifier = ItemClass.GetItemClass(modsToInstall[i], true) as ItemClassModifier;
					if (itemClassModifier == null)
					{
						itemClassModifier = ItemClassModifier.GetDesiredItemModWithAnyTags(ic.ItemTags, fastTags, FastTags<TagGroup.Global>.Parse(modsToInstall[i]), random);
					}
					if (itemClassModifier != null)
					{
						if (itemClassModifier.HasAnyTags(ItemClassModifier.CosmeticModTypes))
						{
							flag = true;
							if (!flag2 && random.RandomFloat <= modInstallDescendingChance)
							{
								this.CosmeticMods[0] = new ItemValue(itemClassModifier.Id, false);
								fastTags |= itemClassModifier.ItemTags;
								flag2 = true;
								Log.Warning("ItemValue createDefaultModItems cosmetic {0}", new object[] { this.CosmeticMods[0] });
							}
						}
						else if (num2 < this.Modifications.Length && random.RandomFloat <= num)
						{
							this.Modifications[num2] = new ItemValue(itemClassModifier.Id, false);
							fastTags |= itemClassModifier.ItemTags;
							num2++;
							num *= 0.5f;
						}
					}
				}
				for (int j = num2; j < this.Modifications.Length; j++)
				{
					this.Modifications[j] = ItemValue.None;
				}
			}
		}
		if (!flag && !ic.HasAnyTags(ItemValue.noPreinstallCosmeticItemTags))
		{
			for (int k = 0; k < this.CosmeticMods.Length; k++)
			{
				ItemClassModifier cosmeticItemMod = ItemClassModifier.GetCosmeticItemMod(ic.ItemTags, fastTags, random);
				if (cosmeticItemMod != null)
				{
					this.CosmeticMods[k] = new ItemValue(cosmeticItemMod.Id, false);
					fastTags |= cosmeticItemMod.ItemTags;
				}
				else
				{
					this.CosmeticMods[k] = ItemValue.None;
				}
			}
		}
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x00122F3C File Offset: 0x0012113C
	public void ModifyValue(EntityAlive _entity, ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _originalValue, ref float _perc_value, FastTags<TagGroup.Global> _tags, bool _useMods = true, bool _useDurability = false)
	{
		if (_originalItemValue == null || !_originalItemValue.Equals(this))
		{
			int num = MinEventParams.CachedEventParam.Seed;
			if (_entity != null)
			{
				num = _entity.MinEventContext.Seed;
			}
			ItemClass itemClass = this.ItemClass;
			if (itemClass != null)
			{
				if (itemClass.Actions != null && itemClass.Actions.Length != 0 && itemClass.Actions[0] is ItemActionRanged)
				{
					string[] magazineItemNames = (itemClass.Actions[0] as ItemActionRanged).MagazineItemNames;
					if (magazineItemNames != null)
					{
						ItemClass itemClass2 = ItemClass.GetItemClass(magazineItemNames[(int)this.SelectedAmmoTypeIndex], false);
						if (itemClass2 != null && itemClass2.Effects != null)
						{
							itemClass2.Effects.ModifyValue(_entity, _passiveEffect, ref _originalValue, ref _perc_value, 0f, _tags, 1);
						}
					}
				}
				if (itemClass.Effects != null)
				{
					ItemValue itemValue = MinEventParams.CachedEventParam.ItemValue;
					ItemValue itemValue2 = ((_entity != null) ? _entity.MinEventContext.ItemValue : null);
					MinEventParams.CachedEventParam.Seed = (int)((byte)this.Seed + ((this.Seed != 0) ? _passiveEffect : PassiveEffects.None));
					MinEventParams.CachedEventParam.ItemValue = this;
					if (_entity != null)
					{
						_entity.MinEventContext.Seed = MinEventParams.CachedEventParam.Seed;
						_entity.MinEventContext.ItemValue = this;
					}
					float num2 = _originalValue;
					itemClass.Effects.ModifyValue(_entity, _passiveEffect, ref _originalValue, ref _perc_value, (float)this.Quality, _tags, 1);
					if (_useDurability)
					{
						if (_passiveEffect != PassiveEffects.PhysicalDamageResist)
						{
							if (_passiveEffect != PassiveEffects.ElementalDamageResist)
							{
								if (_passiveEffect == PassiveEffects.BuffResistance)
								{
									if (this.PercentUsesLeft < 0.5f)
									{
										float num3 = _originalValue - num2;
										_originalValue = num2 + num3 * this.PercentUsesLeft * 2f;
									}
								}
							}
							else if (this.PercentUsesLeft < 0.5f)
							{
								float num4 = _originalValue - num2;
								_originalValue = num2 + num4 * this.PercentUsesLeft * 2f;
							}
						}
						else if (this.PercentUsesLeft < 0.5f)
						{
							float num5 = _originalValue - num2;
							_originalValue = num2 + num5 * this.PercentUsesLeft * 2f;
						}
					}
					MinEventParams.CachedEventParam.ItemValue = itemValue;
					if (_entity != null)
					{
						_entity.MinEventContext.ItemValue = itemValue2;
					}
				}
			}
			if (this.Stats != null)
			{
				this.StatModifyValue(_passiveEffect, ref _originalValue, false);
			}
			if (_useMods)
			{
				for (int i = 0; i < this.CosmeticMods.Length; i++)
				{
					if (this.CosmeticMods[i] != null && this.CosmeticMods[i].ItemClass is ItemClassModifier)
					{
						this.CosmeticMods[i].ModifyValue(_entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags, true, false);
					}
				}
				for (int j = 0; j < this.Modifications.Length; j++)
				{
					if (this.Modifications[j] != null && this.Modifications[j].ItemClass is ItemClassModifier)
					{
						this.Modifications[j].ModifyValue(_entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags, true, false);
					}
				}
			}
			MinEventParams.CachedEventParam.Seed = num;
			if (_entity != null)
			{
				_entity.MinEventContext.Seed = num;
			}
		}
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x0012322C File Offset: 0x0012142C
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _entity, ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _originalValue, ref float _perc_value, FastTags<TagGroup.Global> _tags)
	{
		if (_originalItemValue == null || !_originalItemValue.Equals(this))
		{
			ItemClass itemClass = this.ItemClass;
			if (itemClass != null)
			{
				if (itemClass.Actions != null && itemClass.Actions.Length != 0 && itemClass.Actions[0] is ItemActionRanged)
				{
					string[] magazineItemNames = (itemClass.Actions[0] as ItemActionRanged).MagazineItemNames;
					if (magazineItemNames != null)
					{
						ItemClass.GetItem(magazineItemNames[(int)this.SelectedAmmoTypeIndex], false).GetModifiedValueData(_modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType.Ammo, _entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags);
					}
				}
				if (itemClass.Effects != null)
				{
					itemClass.Effects.GetModifiedValueData(_modValueSources, _sourceType, _entity, _passiveEffect, ref _originalValue, ref _perc_value, (float)this.Quality, _tags, 1);
				}
			}
			for (int i = 0; i < this.CosmeticMods.Length; i++)
			{
				if (this.CosmeticMods[i] != null && this.CosmeticMods[i].ItemClass is ItemClassModifier)
				{
					this.CosmeticMods[i].GetModifiedValueData(_modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType.CosmeticMod, _entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags);
				}
			}
			for (int j = 0; j < this.Modifications.Length; j++)
			{
				if (this.Modifications[j] != null && this.Modifications[j].ItemClass is ItemClassModifier)
				{
					this.Modifications[j].GetModifiedValueData(_modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType.Mod, _entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags);
				}
			}
		}
	}

	// Token: 0x06002DD8 RID: 11736 RVA: 0x00123368 File Offset: 0x00121568
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		ItemClass itemClass = this.ItemClass;
		if (itemClass != null)
		{
			if (itemClass is ItemClassModifier && itemClass.Effects != null)
			{
				itemClass.Effects.FireEvent(_eventType, _eventParms);
				return;
			}
			if (itemClass.Actions != null && itemClass.Actions.Length != 0 && itemClass.Actions[0] is ItemActionRanged)
			{
				string[] magazineItemNames = (itemClass.Actions[0] as ItemActionRanged).MagazineItemNames;
				if (magazineItemNames != null)
				{
					ItemClass.GetItem(magazineItemNames[(int)this.SelectedAmmoTypeIndex], false).FireEvent(_eventType, _eventParms);
				}
			}
			itemClass.FireEvent(_eventType, _eventParms);
		}
		if (!this.HasQuality)
		{
			return;
		}
		for (int i = 0; i < this.Modifications.Length; i++)
		{
			if (this.Modifications[i] != null)
			{
				this.Modifications[i].FireEvent(_eventType, _eventParms);
			}
		}
		for (int j = 0; j < this.CosmeticMods.Length; j++)
		{
			if (this.CosmeticMods[j] != null)
			{
				this.CosmeticMods[j].FireEvent(_eventType, _eventParms);
			}
		}
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x00123454 File Offset: 0x00121654
	public ItemValue Clone()
	{
		ItemValue itemValue = new ItemValue(this.type, false);
		itemValue.Meta = this.Meta;
		itemValue.UseTimes = this.UseTimes;
		itemValue.Quality = this.Quality;
		itemValue.SelectedAmmoTypeIndex = this.SelectedAmmoTypeIndex;
		if (this.Stats != null)
		{
			itemValue.Stats = new ItemValue.Stat[this.Stats.Length];
			Array.Copy(this.Stats, itemValue.Stats, this.Stats.Length);
		}
		this.CloneModsTo(itemValue);
		if (this.Metadata != null)
		{
			itemValue.Metadata = new Dictionary<string, TypedMetadataValue>();
			foreach (KeyValuePair<string, TypedMetadataValue> keyValuePair in this.Metadata)
			{
				Dictionary<string, TypedMetadataValue> metadata = itemValue.Metadata;
				string key = keyValuePair.Key;
				TypedMetadataValue value = keyValuePair.Value;
				metadata.Add(key, (value != null) ? value.Clone() : null);
			}
		}
		this.CloneCosmeticModsTo(itemValue);
		itemValue.Activated = this.Activated;
		if (itemValue.type == 0)
		{
			this.Seed = 0;
		}
		itemValue.Seed = this.Seed;
		itemValue.TextureFullArray = this.TextureFullArray;
		return itemValue;
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x0012358C File Offset: 0x0012178C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloneModsTo(ItemValue _iv)
	{
		int num = this.Modifications.Length;
		_iv.Modifications = new ItemValue[num];
		for (int i = 0; i < num; i++)
		{
			ItemValue[] modifications = _iv.Modifications;
			int num2 = i;
			ItemValue itemValue = this.Modifications[i];
			modifications[num2] = ((itemValue != null) ? itemValue.Clone() : null);
		}
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x001235D8 File Offset: 0x001217D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloneCosmeticModsTo(ItemValue _iv)
	{
		int num = this.CosmeticMods.Length;
		_iv.CosmeticMods = new ItemValue[num];
		for (int i = 0; i < num; i++)
		{
			ItemValue[] cosmeticMods = _iv.CosmeticMods;
			int num2 = i;
			ItemValue itemValue = this.CosmeticMods[i];
			cosmeticMods[num2] = ((itemValue != null) ? itemValue.Clone() : null);
		}
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x00123622 File Offset: 0x00121822
	public bool IsEmpty()
	{
		return this.type == 0;
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x00123630 File Offset: 0x00121830
	public BlockValue ToBlockValue(bool _allowAlternates = false)
	{
		if (this.type >= Block.ItemsStartHere)
		{
			return BlockValue.Air;
		}
		BlockValue blockValue = default(BlockValue);
		blockValue.type = this.type;
		if (_allowAlternates && blockValue.Block.SelectAlternates)
		{
			return blockValue.Block.GetAltBlockValue(this.Meta);
		}
		return blockValue;
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x0012368C File Offset: 0x0012188C
	public void MergeBest(ItemValue _iv)
	{
		int maxUseTimes = this.MaxUseTimes;
		int maxUseTimes2 = _iv.MaxUseTimes;
		float num = (float)maxUseTimes - this.UseTimes;
		float num2 = (float)maxUseTimes2 - _iv.UseTimes;
		if (ItemAction.RepairType == ItemAction.RepairTypes.Both || ItemAction.RepairType == ItemAction.RepairTypes.CombineOnly)
		{
			int num3 = Utils.FastMax(maxUseTimes, maxUseTimes2);
			float num4 = num + num2;
			this.UseTimes = Utils.FastMax(0f, (float)num3 - num4);
		}
		else
		{
			bool flag = false;
			if (_iv.Quality == this.Quality)
			{
				flag = num2 > num;
				if (flag)
				{
					this.MaxDurabilityModifier = _iv.MaxDurabilityModifier;
				}
			}
			else if (_iv.Quality > this.Quality)
			{
				flag = true;
			}
			if (flag)
			{
				this.UseTimes = Utils.FastMax(0f, (float)maxUseTimes2 - num2);
			}
		}
		if (_iv.Quality > this.Quality)
		{
			this.Quality = _iv.Quality;
			this.MaxDurabilityModifier = _iv.MaxDurabilityModifier;
		}
		this.MergeBestStats(_iv);
		if (_iv.HasMods())
		{
			_iv.CloneModsTo(this);
		}
		if (_iv.HasCosmetics())
		{
			_iv.CloneCosmeticModsTo(this);
		}
		int num5 = this.CalcModSlotCount();
		Array.Resize<ItemValue>(ref this.Modifications, num5);
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x001237A8 File Offset: 0x001219A8
	public void InitStats(int _count)
	{
		this.Stats = new ItemValue.Stat[_count];
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x001237B6 File Offset: 0x001219B6
	public void ClearStats()
	{
		this.Stats = null;
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x001237C0 File Offset: 0x001219C0
	public void RemoveUnusedStats()
	{
		if (this.Stats != null)
		{
			int num = 0;
			for (int i = 0; i < this.Stats.Length; i++)
			{
				if (this.Stats[i].value != 0)
				{
					num++;
				}
			}
			if (num == 0)
			{
				this.ClearStats();
				return;
			}
			if (num < this.Stats.Length)
			{
				ItemValue.Stat[] stats = this.Stats;
				this.Stats = new ItemValue.Stat[num];
				int num2 = 0;
				for (int j = 0; j < stats.Length; j++)
				{
					if (stats[j].value != 0)
					{
						this.Stats[num2++] = stats[j];
					}
				}
			}
		}
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x00123867 File Offset: 0x00121A67
	public bool HasStats()
	{
		return this.Stats != null;
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x00123874 File Offset: 0x00121A74
	public bool HasAnyBoostedStats()
	{
		if (this.Stats != null)
		{
			for (int i = 0; i < this.Stats.Length; i++)
			{
				if (this.Stats[i].isBoosted)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x001238B4 File Offset: 0x00121AB4
	public float GetStatPercent(PassiveEffects _type, bool _onlyBoosted)
	{
		float num = 1f;
		if (this.Stats != null)
		{
			this.StatModifyValue(_type, ref num, _onlyBoosted);
		}
		return num;
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x001238DA File Offset: 0x00121ADA
	public void SetStat(int _index, PassiveEffects _type, int _base, int _added)
	{
		this.Stats[_index] = new ItemValue.Stat(_type, _base, _added);
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x001238F4 File Offset: 0x00121AF4
	public void MergeBestStats(ItemValue _iv)
	{
		if (_iv.Stats == null)
		{
			return;
		}
		if (this.Stats == null)
		{
			this.Stats = new ItemValue.Stat[_iv.Stats.Length];
			Array.Copy(_iv.Stats, this.Stats, _iv.Stats.Length);
			return;
		}
		List<ItemValue.Stat> list = new List<ItemValue.Stat>(this.Stats);
		for (int i = 0; i < _iv.Stats.Length; i++)
		{
			ItemValue.Stat stat = _iv.Stats[i];
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				ItemValue.Stat stat2 = list[j];
				if (stat.type == stat2.type)
				{
					bool flag2 = ItemValue.IsStatLowerBetter(stat2.type);
					if (stat.value != stat2.value && stat.value > stat2.value != flag2)
					{
						stat2.value = stat.value;
						stat2.isBoosted = stat.isBoosted;
						list[j] = stat2;
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(stat);
			}
		}
		this.Stats = list.ToArray();
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x00123A12 File Offset: 0x00121C12
	public static bool IsStatLowerBetter(PassiveEffects _type)
	{
		return _type == PassiveEffects.StaminaLoss || _type == PassiveEffects.TargetArmor;
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x00123A24 File Offset: 0x00121C24
	public void StatModifyValue(PassiveEffects _passiveEffect, ref float _originalValue, bool onlyBoosted = false)
	{
		for (int i = 0; i < this.Stats.Length; i++)
		{
			ItemValue.Stat stat = this.Stats[i];
			if (stat.type == _passiveEffect)
			{
				int num = (int)((onlyBoosted && !stat.isBoosted) ? 0 : stat.value);
				float num2 = 1f + (float)num * 0.005f;
				_originalValue *= num2;
				return;
			}
		}
	}

	// Token: 0x06002DE9 RID: 11753 RVA: 0x000027FC File Offset: 0x000009FC
	public void ReadOld(BinaryReader _br)
	{
	}

	// Token: 0x06002DEA RID: 11754 RVA: 0x00123A88 File Offset: 0x00121C88
	public static ItemValue ReadOrNull(BinaryReader _br)
	{
		byte b = _br.ReadByte();
		if (b == 0)
		{
			return null;
		}
		ItemValue itemValue = new ItemValue();
		itemValue.ReadData(_br, (int)b);
		return itemValue;
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x00123AB0 File Offset: 0x00121CB0
	public void Read(BinaryReader _br)
	{
		byte b = _br.ReadByte();
		if (b == 0)
		{
			this.type = 0;
			return;
		}
		this.ReadData(_br, (int)b);
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x00123AD8 File Offset: 0x00121CD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadData(BinaryReader _br, int version)
	{
		int num = 0;
		if (version >= 8)
		{
			num = (int)_br.ReadByte();
		}
		this.type = (int)_br.ReadUInt16();
		if ((num & 1) > 0)
		{
			this.type += Block.ItemsStartHere;
		}
		if (version < 8 && this.type >= 32768)
		{
			this.type += 32768;
		}
		ItemClass itemClass = this.ItemClass;
		if (version > 5)
		{
			this.UseTimes = _br.ReadSingle();
		}
		else
		{
			this.UseTimes = (float)_br.ReadUInt16();
		}
		this.Quality = _br.ReadUInt16();
		if (itemClass != null && itemClass.QualityMin > 0)
		{
			this.Quality = (ushort)((byte)Utils.FastMax((int)this.Quality, (int)itemClass.QualityMin));
		}
		this.Meta = (int)_br.ReadUInt16();
		if (this.Meta >= 65535)
		{
			this.Meta = -1;
		}
		if (version > 6)
		{
			int num2 = (int)_br.ReadByte();
			for (int i = 0; i < num2; i++)
			{
				string text = _br.ReadString();
				TypedMetadataValue typedMetadataValue = TypedMetadataValue.Read(_br);
				this.SetMetadata(text, typedMetadataValue);
			}
		}
		if ((num & 2) > 0)
		{
			int num3 = (int)_br.ReadByte();
			this.Stats = new ItemValue.Stat[num3];
			for (int j = 0; j < num3; j++)
			{
				PassiveEffects passiveEffects = (PassiveEffects)_br.ReadByte();
				int num4 = (int)_br.ReadInt16();
				int num5 = (int)_br.ReadInt16();
				this.Stats[j] = new ItemValue.Stat(passiveEffects, num4, num5);
			}
			this.RemoveUnusedStats();
		}
		if ((version > 4 || this.HasQuality) && !(itemClass is ItemClassModifier))
		{
			byte b = _br.ReadByte();
			this.Modifications = new ItemValue[(int)b];
			if (b != 0)
			{
				for (int k = 0; k < (int)b; k++)
				{
					if (_br.ReadBoolean())
					{
						this.Modifications[k] = new ItemValue();
						this.Modifications[k].Read(_br);
					}
					else
					{
						this.Modifications[k] = ItemValue.None;
					}
				}
			}
			b = _br.ReadByte();
			this.CosmeticMods = new ItemValue[(int)b];
			if (b != 0)
			{
				for (int l = 0; l < (int)b; l++)
				{
					if (_br.ReadBoolean())
					{
						this.CosmeticMods[l] = new ItemValue();
						this.CosmeticMods[l].Read(_br);
					}
					else
					{
						this.CosmeticMods[l] = ItemValue.None;
					}
				}
			}
		}
		if (version > 1)
		{
			this.Activated = _br.ReadByte();
		}
		if (version > 2)
		{
			this.SelectedAmmoTypeIndex = _br.ReadByte();
		}
		if (version > 3)
		{
			this.Seed = _br.ReadUInt16();
			if (this.type == 0)
			{
				this.Seed = 0;
			}
		}
		if (version > 8)
		{
			if (_br.ReadBoolean())
			{
				this.TextureFullArray.Read(_br, 1);
				return;
			}
			this.TextureFullArray.Fill(0L);
		}
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x00123D80 File Offset: 0x00121F80
	public void AdjustForSandboxOptions()
	{
		if (!EntityPlayerLocal.PermaDegrationOn)
		{
			this.RemoveMetaData("DurabilityModifier");
		}
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x00123D95 File Offset: 0x00121F95
	public static void Write(ItemValue _iv, BinaryWriter _bw)
	{
		if (_iv == null)
		{
			_bw.Write(0);
			return;
		}
		_iv.Write(_bw);
	}

	// Token: 0x06002DEF RID: 11759 RVA: 0x00123DAC File Offset: 0x00121FAC
	public void Write(BinaryWriter _bw)
	{
		if (this.IsEmpty())
		{
			_bw.Write(0);
			return;
		}
		_bw.Write(9);
		int num = this.type;
		byte b = 0;
		if (this.type >= Block.ItemsStartHere)
		{
			b = 1;
			num -= Block.ItemsStartHere;
		}
		if (this.Stats != null)
		{
			b |= 2;
		}
		_bw.Write(b);
		_bw.Write((ushort)num);
		_bw.Write(this.UseTimes);
		_bw.Write(this.Quality);
		_bw.Write((ushort)this.Meta);
		int num2 = ((this.Metadata != null) ? this.Metadata.Count : 0);
		_bw.Write((byte)num2);
		if (this.Metadata != null)
		{
			foreach (string text in this.Metadata.Keys)
			{
				TypedMetadataValue typedMetadataValue = this.Metadata[text];
				if (((typedMetadataValue != null) ? typedMetadataValue.GetValue() : null) != null)
				{
					_bw.Write(text);
					TypedMetadataValue.Write(this.Metadata[text], _bw);
				}
			}
		}
		if (this.Stats != null)
		{
			int num3 = this.Stats.Length;
			_bw.Write((byte)num3);
			for (int i = 0; i < num3; i++)
			{
				ItemValue.Stat stat = this.Stats[i];
				_bw.Write((byte)stat.type);
				short num4 = ((!stat.isBoosted) ? stat.value : 0);
				short num5 = (stat.isBoosted ? stat.value : 0);
				_bw.Write(num4);
				_bw.Write(num5);
			}
		}
		if (!(this.ItemClass is ItemClassModifier))
		{
			_bw.Write((byte)this.Modifications.Length);
			for (int j = 0; j < this.Modifications.Length; j++)
			{
				bool flag = this.Modifications[j] != null && !this.Modifications[j].IsEmpty();
				_bw.Write(flag);
				if (flag)
				{
					this.Modifications[j].Write(_bw);
				}
			}
			_bw.Write((byte)this.CosmeticMods.Length);
			for (int k = 0; k < this.CosmeticMods.Length; k++)
			{
				bool flag2 = this.CosmeticMods[k] != null && !this.CosmeticMods[k].IsEmpty();
				_bw.Write(flag2);
				if (flag2)
				{
					this.CosmeticMods[k].Write(_bw);
				}
			}
		}
		_bw.Write(this.Activated);
		_bw.Write(this.SelectedAmmoTypeIndex);
		if (this.type == 0)
		{
			this.Seed = 0;
		}
		_bw.Write(this.Seed);
		if (this.TextureFullArray.IsDefault)
		{
			_bw.Write(false);
		}
		else
		{
			_bw.Write(true);
			this.TextureFullArray.Write(_bw);
		}
		ItemClass itemClass = ItemClass.list[this.type];
		if (itemClass == null)
		{
			if (this.type != 0)
			{
				Log.Error("No ItemClass entry for type " + this.type.ToString());
				return;
			}
		}
		else
		{
			NameIdMapping nameIdMapping;
			if (itemClass.IsBlock())
			{
				nameIdMapping = Block.nameIdMapping;
			}
			else
			{
				nameIdMapping = ItemClass.nameIdMapping;
			}
			if (nameIdMapping != null)
			{
				nameIdMapping.AddMapping(this.type, itemClass.Name, false);
			}
		}
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x001240EC File Offset: 0x001222EC
	public bool Equals(ItemValue _other)
	{
		return _other != null && (_other.type == this.type && _other.UseTimes == this.UseTimes && _other.Meta == this.Meta && _other.Seed == this.Seed && _other.Quality == this.Quality && _other.SelectedAmmoTypeIndex == this.SelectedAmmoTypeIndex && _other.Activated == this.Activated && ItemValue.Equals(_other.Metadata, this.Metadata) && ItemValue.Equals(_other.Stats, this.Stats) && ItemValue.Equals(_other.CosmeticMods, this.CosmeticMods)) && ItemValue.Equals(_other.Modifications, this.Modifications);
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x001241B5 File Offset: 0x001223B5
	public override bool Equals(object _other)
	{
		return _other is ItemValue && this.Equals((ItemValue)_other);
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x001241D0 File Offset: 0x001223D0
	public bool EqualsExceptUseTimesAndAmmo(ItemValue _other)
	{
		if (_other == null)
		{
			return false;
		}
		bool flag = this.ItemClass != null && this.ItemClass.Actions != null && this.ItemClass.Actions.Length != 0 && this.ItemClass.Actions[0] is ItemActionRanged;
		return _other.type == this.type && (flag || _other.Meta == this.Meta) && _other.Seed == this.Seed && _other.Quality == this.Quality && _other.SelectedAmmoTypeIndex == this.SelectedAmmoTypeIndex && _other.Activated == this.Activated && ItemValue.Equals(_other.Metadata, this.Metadata) && ItemValue.Equals(_other.Stats, this.Stats) && ItemValue.Equals(_other.CosmeticMods, this.CosmeticMods) && ItemValue.Equals(_other.Modifications, this.Modifications);
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x001242C8 File Offset: 0x001224C8
	public bool EqualsForMerging(ItemValue _other)
	{
		if (_other == null)
		{
			return false;
		}
		bool flag = ItemAction.RepairType == ItemAction.RepairTypes.Both || ItemAction.RepairType == ItemAction.RepairTypes.CombineOnly;
		return _other.type == this.type && (!flag || this.UseTimes + _other.UseTimes == 0f || ((float)this.MaxUseTimes == this.UseTimes && (float)_other.MaxUseTimes == _other.UseTimes)) && ItemValue.Equals(_other.Stats, this.Stats);
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x00124344 File Offset: 0x00122544
	public static bool Equals(ItemValue[] _a, ItemValue[] _b)
	{
		if (_a == null && _b == null)
		{
			return true;
		}
		if (_a == null || _b == null)
		{
			return false;
		}
		if (_a.Length != _b.Length)
		{
			return false;
		}
		if (_a.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < _a.Length; i++)
		{
			if (_a[i] != null || _b[i] != null)
			{
				if (_a[i] == null || _b[i] == null)
				{
					return false;
				}
				if (!_a[i].Equals(_b[i]))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x001243A5 File Offset: 0x001225A5
	public static bool Equals(ItemValue.Stat[] _a, ItemValue.Stat[] _b)
	{
		return (_a == null && _b == null) || (_a != null && _b != null && _a.Length == _b.Length && (_a.Length == 0 || _a.SequenceEqual<ItemValue.Stat>(_b)));
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x001243CE File Offset: 0x001225CE
	public static bool Equals(object[] _a, object[] _b)
	{
		return _a is ItemValue[] && _b is ItemValue[] && ItemValue.Equals((ItemValue[])_a, (ItemValue[])_b);
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x001243F4 File Offset: 0x001225F4
	public static bool Equals(Dictionary<string, TypedMetadataValue> _a, Dictionary<string, TypedMetadataValue> _b)
	{
		if (_a == null && _b == null)
		{
			return true;
		}
		if (_a == null || _b == null)
		{
			return false;
		}
		if (_a.Count != _b.Count)
		{
			return false;
		}
		if (_a.Count == 0)
		{
			return true;
		}
		foreach (string text in _a.Keys)
		{
			if (!_b.ContainsKey(text))
			{
				return false;
			}
			if (!_a[text].Equals(_b[text]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x00124494 File Offset: 0x00122694
	public override int GetHashCode()
	{
		return this.type;
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x0012449C File Offset: 0x0012269C
	public int GetItemId()
	{
		return this.type - Block.ItemsStartHere;
	}

	// Token: 0x06002DFA RID: 11770 RVA: 0x001244AA File Offset: 0x001226AA
	public int GetItemOrBlockId()
	{
		if (this.type < Block.ItemsStartHere)
		{
			return this.type;
		}
		return this.type - Block.ItemsStartHere;
	}

	// Token: 0x06002DFB RID: 11771 RVA: 0x001244CC File Offset: 0x001226CC
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			(this.type >= Block.ItemsStartHere) ? ("item=" + (this.type - Block.ItemsStartHere).ToString()) : ("block=" + this.type.ToString()),
			" m=",
			this.Meta.ToString(),
			" ut=",
			this.UseTimes.ToString()
		});
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x00124558 File Offset: 0x00122758
	public string GetPropertyOverride(string _propertyName, string _originalValue)
	{
		if (this.Modifications.Length == 0 && this.CosmeticMods.Length == 0)
		{
			return _originalValue;
		}
		string text = "";
		string itemName = this.ItemClass.GetItemName();
		for (int i = 0; i < this.Modifications.Length; i++)
		{
			ItemValue itemValue = this.Modifications[i];
			if (itemValue != null)
			{
				ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
				if (itemClassModifier != null && itemClassModifier.GetPropertyOverride(_propertyName, itemName, ref text))
				{
					return text;
				}
			}
		}
		text = "";
		for (int j = 0; j < this.CosmeticMods.Length; j++)
		{
			ItemValue itemValue2 = this.CosmeticMods[j];
			if (itemValue2 != null)
			{
				ItemClassModifier itemClassModifier2 = itemValue2.ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && itemClassModifier2.GetPropertyOverride(_propertyName, itemName, ref text))
				{
					return text;
				}
			}
		}
		return _originalValue;
	}

	// Token: 0x06002DFD RID: 11773 RVA: 0x00124618 File Offset: 0x00122818
	public bool HasCosmetics()
	{
		if (this.CosmeticMods != null)
		{
			for (int i = 0; i < this.CosmeticMods.Length; i++)
			{
				ItemValue itemValue = this.CosmeticMods[i];
				if (itemValue != null && !itemValue.IsEmpty())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x00124658 File Offset: 0x00122858
	public bool HasMods()
	{
		if (this.Modifications != null)
		{
			for (int i = 0; i < this.Modifications.Length; i++)
			{
				ItemValue itemValue = this.Modifications[i];
				if (itemValue != null && !itemValue.IsEmpty())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04002392 RID: 9106
	[PublicizedFrom(EAccessModifier.Private)]
	public const int EmptySaveVersion = 0;

	// Token: 0x04002393 RID: 9107
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 9;

	// Token: 0x04002394 RID: 9108
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte MaxModifications = 255;

	// Token: 0x04002395 RID: 9109
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ItemValue[] emptyItemValueArray = new ItemValue[0];

	// Token: 0x04002396 RID: 9110
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> noPreinstallCosmeticItemTags = FastTags<TagGroup.Global>.Parse("weapon,tool,armor");

	// Token: 0x04002397 RID: 9111
	public int type;

	// Token: 0x04002398 RID: 9112
	public byte Activated;

	// Token: 0x04002399 RID: 9113
	public byte SelectedAmmoTypeIndex;

	// Token: 0x0400239A RID: 9114
	public float UseTimes;

	// Token: 0x0400239B RID: 9115
	public int Meta;

	// Token: 0x0400239C RID: 9116
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, TypedMetadataValue> Metadata;

	// Token: 0x0400239D RID: 9117
	public ushort Quality;

	// Token: 0x0400239E RID: 9118
	public ItemValue.Stat[] Stats;

	// Token: 0x0400239F RID: 9119
	public ItemValue[] Modifications;

	// Token: 0x040023A0 RID: 9120
	public ItemValue[] CosmeticMods;

	// Token: 0x040023A1 RID: 9121
	public ushort Seed;

	// Token: 0x040023A2 RID: 9122
	public TextureFullArray TextureFullArray;

	// Token: 0x040023A3 RID: 9123
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDurabilityMetaName = "DurabilityModifier";

	// Token: 0x040023A4 RID: 9124
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime baseDate = new DateTime(2013, 10, 1);

	// Token: 0x040023A5 RID: 9125
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsItem = 1;

	// Token: 0x040023A6 RID: 9126
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsStats = 2;

	// Token: 0x0200057A RID: 1402
	public struct Stat
	{
		// Token: 0x06002E00 RID: 11776 RVA: 0x001246C5 File Offset: 0x001228C5
		public Stat(PassiveEffects _type, int _base, int _added)
		{
			this.type = _type;
			this.isBoosted = _added != 0;
			this.value = (short)(_base + _added);
		}

		// Token: 0x040023A7 RID: 9127
		public PassiveEffects type;

		// Token: 0x040023A8 RID: 9128
		public bool isBoosted;

		// Token: 0x040023A9 RID: 9129
		public short value;
	}
}
