using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000588 RID: 1416
public class ItemValue : IEquatable<ItemValue>
{
	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06002E28 RID: 11816 RVA: 0x001252CA File Offset: 0x001234CA
	public static ItemValue None
	{
		get
		{
			return new ItemValue(0, false);
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06002E29 RID: 11817 RVA: 0x001252D3 File Offset: 0x001234D3
	// (set) Token: 0x06002E2A RID: 11818 RVA: 0x001252DB File Offset: 0x001234DB
	public int type
	{
		get
		{
			return this.typeField;
		}
		set
		{
			if (this.typeField == value)
			{
				return;
			}
			this.typeField = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06002E2B RID: 11819 RVA: 0x001252F4 File Offset: 0x001234F4
	// (set) Token: 0x06002E2C RID: 11820 RVA: 0x001252FC File Offset: 0x001234FC
	public byte Flags
	{
		get
		{
			return this.flags;
		}
		set
		{
			if (this.flags == value)
			{
				return;
			}
			this.flags = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06002E2D RID: 11821 RVA: 0x00125315 File Offset: 0x00123515
	// (set) Token: 0x06002E2E RID: 11822 RVA: 0x0012531D File Offset: 0x0012351D
	public byte SelectedAmmoTypeIndex
	{
		get
		{
			return this.selectedAmmoTypeIndex;
		}
		set
		{
			if (this.selectedAmmoTypeIndex == value)
			{
				return;
			}
			this.selectedAmmoTypeIndex = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06002E2F RID: 11823 RVA: 0x00125336 File Offset: 0x00123536
	// (set) Token: 0x06002E30 RID: 11824 RVA: 0x0012533E File Offset: 0x0012353E
	public float UseTimes
	{
		get
		{
			return this.useTimes;
		}
		set
		{
			if (this.useTimes == value)
			{
				return;
			}
			this.useTimes = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06002E31 RID: 11825 RVA: 0x00125357 File Offset: 0x00123557
	// (set) Token: 0x06002E32 RID: 11826 RVA: 0x0012535F File Offset: 0x0012355F
	public int Meta
	{
		get
		{
			return this.meta;
		}
		set
		{
			if (this.meta == value)
			{
				return;
			}
			this.meta = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06002E33 RID: 11827 RVA: 0x00125378 File Offset: 0x00123578
	// (set) Token: 0x06002E34 RID: 11828 RVA: 0x00125380 File Offset: 0x00123580
	public ushort Quality
	{
		get
		{
			return this.quality;
		}
		set
		{
			if (this.quality == value)
			{
				return;
			}
			this.quality = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06002E35 RID: 11829 RVA: 0x00125399 File Offset: 0x00123599
	// (set) Token: 0x06002E36 RID: 11830 RVA: 0x001253A1 File Offset: 0x001235A1
	public ItemValue.Stat[] Stats
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06002E37 RID: 11831 RVA: 0x001253AA File Offset: 0x001235AA
	public int ModificationCount
	{
		get
		{
			if (this.modifications == null)
			{
				return 0;
			}
			return this.modifications.Length;
		}
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x001253BE File Offset: 0x001235BE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ItemValue GetModification(int _index)
	{
		return this.modifications[_index];
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06002E39 RID: 11833 RVA: 0x001253C8 File Offset: 0x001235C8
	public int CosmeticModCount
	{
		get
		{
			if (this.cosmeticMods == null)
			{
				return 0;
			}
			return this.cosmeticMods.Length;
		}
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x001253DC File Offset: 0x001235DC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ItemValue GetCosmeticMod(int _index)
	{
		return this.cosmeticMods[_index];
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x001253E6 File Offset: 0x001235E6
	public void SetModification(int _index, ItemValue _mod)
	{
		this.modifications[_index] = _mod;
		this.NotifyChanged();
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x001253F7 File Offset: 0x001235F7
	public void SetCosmeticMod(int _index, ItemValue _mod)
	{
		this.cosmeticMods[_index] = _mod;
		this.NotifyChanged();
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x00125408 File Offset: 0x00123608
	public void SetTextureFull(int _index, long _value)
	{
		if (this.textureFullArray[_index] == _value)
		{
			return;
		}
		this.textureFullArray[_index] = _value;
		this.NotifyChanged();
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x0012542D File Offset: 0x0012362D
	public void ResizeCosmeticMods(int _length)
	{
		if (_length == 0)
		{
			this.cosmeticMods = null;
			this.NotifyChanged();
			return;
		}
		Array.Resize<ItemValue>(ref this.cosmeticMods, _length);
		this.NotifyChanged();
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06002E3F RID: 11839 RVA: 0x00125452 File Offset: 0x00123652
	// (set) Token: 0x06002E40 RID: 11840 RVA: 0x0012545A File Offset: 0x0012365A
	public ushort Seed
	{
		get
		{
			return this.seed;
		}
		set
		{
			if (this.seed == value)
			{
				return;
			}
			this.seed = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06002E41 RID: 11841 RVA: 0x00125473 File Offset: 0x00123673
	// (set) Token: 0x06002E42 RID: 11842 RVA: 0x0012547B File Offset: 0x0012367B
	public TextureFullArray TextureFullArray
	{
		get
		{
			return this.textureFullArray;
		}
		set
		{
			if (this.textureFullArray == value)
			{
				return;
			}
			this.textureFullArray = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06002E43 RID: 11843 RVA: 0x00125499 File Offset: 0x00123699
	// (set) Token: 0x06002E44 RID: 11844 RVA: 0x001254A6 File Offset: 0x001236A6
	public bool Activated
	{
		get
		{
			return (this.Flags & 1) > 0;
		}
		set
		{
			this.Flags = (byte)(value ? ((int)(this.Flags | 1)) : ((int)this.Flags & -2));
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06002E45 RID: 11845 RVA: 0x001254C5 File Offset: 0x001236C5
	public bool WasCombined
	{
		get
		{
			return (this.Flags & 2) > 0;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06002E46 RID: 11846 RVA: 0x001254D2 File Offset: 0x001236D2
	public int MaxUseTimes
	{
		get
		{
			return ItemValue.ModMaxUseTimes(this.MaxUseTimesBase, this);
		}
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06002E47 RID: 11847 RVA: 0x001254E0 File Offset: 0x001236E0
	public int MaxUseTimesUI
	{
		get
		{
			return this.MaxUseTimesBase;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06002E48 RID: 11848 RVA: 0x001254E8 File Offset: 0x001236E8
	public int MaxUseTimesBase
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			ItemClass itemClass = this.ItemClass;
			return (int)EffectManager.GetValue(PassiveEffects.DegradationMax, this, 0f, null, null, (itemClass != null) ? itemClass.ItemTags : FastTags<TagGroup.Global>.none, true, true, true, true, true, 1, true, false);
		}
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x00125524 File Offset: 0x00123724
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

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06002E4A RID: 11850 RVA: 0x00125560 File Offset: 0x00123760
	// (set) Token: 0x06002E4B RID: 11851 RVA: 0x00125583 File Offset: 0x00123783
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

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06002E4C RID: 11852 RVA: 0x001255A8 File Offset: 0x001237A8
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

	// Token: 0x06002E4D RID: 11853 RVA: 0x001255DC File Offset: 0x001237DC
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

	// Token: 0x06002E4E RID: 11854 RVA: 0x00125604 File Offset: 0x00123804
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

	// Token: 0x06002E4F RID: 11855 RVA: 0x00125630 File Offset: 0x00123830
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

	// Token: 0x06002E50 RID: 11856 RVA: 0x00125658 File Offset: 0x00123858
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

	// Token: 0x06002E51 RID: 11857 RVA: 0x001256A4 File Offset: 0x001238A4
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

	// Token: 0x06002E52 RID: 11858 RVA: 0x001256E8 File Offset: 0x001238E8
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

	// Token: 0x06002E53 RID: 11859 RVA: 0x0012571C File Offset: 0x0012391C
	public void SetMetadata(string key, int value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.Integer);
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x0012572C File Offset: 0x0012392C
	public void SetMetadata(string key, float value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.Float);
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x0012573C File Offset: 0x0012393C
	public void SetMetadata(string key, string value)
	{
		this.SetMetadata(key, value, TypedMetadataValue.TypeTag.String);
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x00125747 File Offset: 0x00123947
	public void SetMetadata(string key, object value, string typeTag)
	{
		this.SetMetadata(key, value, TypedMetadataValue.StringToTag(typeTag));
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x00125758 File Offset: 0x00123958
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

	// Token: 0x06002E58 RID: 11864 RVA: 0x0012582C File Offset: 0x00123A2C
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

	// Token: 0x06002E59 RID: 11865 RVA: 0x001258BC File Offset: 0x00123ABC
	public bool RemoveMetaData(string name)
	{
		bool flag = false;
		if (this.Metadata != null)
		{
			flag = this.Metadata.Remove(name);
		}
		return flag;
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06002E5A RID: 11866 RVA: 0x001258E4 File Offset: 0x00123AE4
	public bool HasQuality
	{
		get
		{
			ItemClass itemClass = this.ItemClass;
			return itemClass != null && (itemClass.HasQuality || itemClass is ItemClassModifier);
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06002E5B RID: 11867 RVA: 0x00125910 File Offset: 0x00123B10
	public bool HasModSlots
	{
		get
		{
			return this.ModificationCount > 0;
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06002E5C RID: 11868 RVA: 0x0012591C File Offset: 0x00123B1C
	public bool IsMod
	{
		get
		{
			ItemClass itemClass = this.ItemClass;
			return itemClass != null && itemClass is ItemClassModifier;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06002E5D RID: 11869 RVA: 0x00125940 File Offset: 0x00123B40
	public bool IsShapeHelperBlock
	{
		get
		{
			ItemClassBlock itemClassBlock = this.ItemClass as ItemClassBlock;
			return itemClassBlock != null && itemClassBlock.GetBlock().SelectAlternates;
		}
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06002E5E RID: 11870 RVA: 0x0012596C File Offset: 0x00123B6C
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

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06002E5F RID: 11871 RVA: 0x001259BC File Offset: 0x00123BBC
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

	// Token: 0x06002E60 RID: 11872 RVA: 0x0000640C File Offset: 0x0000460C
	public ItemValue()
	{
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x001259DA File Offset: 0x00123BDA
	public ItemValue(int _type, bool _bCreateDefaultParts = false)
		: this(_type, 1, 6, _bCreateDefaultParts, null, 1f)
	{
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x001259EC File Offset: 0x00123BEC
	public ItemValue(int _type, int minQuality, int maxQuality, bool _bCreateDefaultModItems = false, string[] modsToInstall = null, float modInstallDescendingChance = 1f)
	{
		this.type = _type;
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
		if (itemClass.MaxCount > 1)
		{
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return;
		}
		int num = this.CalcModSlotCount();
		this.modifications = ((num > 0) ? new ItemValue[num] : null);
		this.cosmeticMods = (itemClass.HasAnyTags(ItemClassModifier.CosmeticItemTags) ? new ItemValue[1] : null);
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

	// Token: 0x06002E63 RID: 11875 RVA: 0x00125B1A File Offset: 0x00123D1A
	public void BindItemStack(ItemStack _owner)
	{
		this.owner = _owner;
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x00125B23 File Offset: 0x00123D23
	[PublicizedFrom(EAccessModifier.Private)]
	public void NotifyChanged()
	{
		ItemStack itemStack = this.owner;
		if (itemStack == null)
		{
			return;
		}
		itemStack.OnValueChanged();
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x00125B35 File Offset: 0x00123D35
	public void Clear()
	{
		this.ResetFields();
		this.NotifyChanged();
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x00125B44 File Offset: 0x00123D44
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetFields()
	{
		this.typeField = 0;
		this.flags = 0;
		this.useTimes = 0f;
		this.quality = 0;
		this.meta = 0;
		this.seed = 0;
		this.selectedAmmoTypeIndex = 0;
		this.textureFullArray = default(TextureFullArray);
		this.Stats = null;
		this.modifications = null;
		this.cosmeticMods = null;
		this.Metadata = null;
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x00125BB0 File Offset: 0x00123DB0
	[PublicizedFrom(EAccessModifier.Private)]
	public int CalcModSlotCount()
	{
		int num = Utils.FastMax(0, (int)(this.Quality - 1));
		return Utils.FastMin(255, (int)EffectManager.GetValue(PassiveEffects.ModSlots, this, (float)num, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, false, false));
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x00125BF4 File Offset: 0x00123DF4
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
								this.cosmeticMods[0] = new ItemValue(itemClassModifier.Id, false);
								fastTags |= itemClassModifier.ItemTags;
								flag2 = true;
								Log.Warning("ItemValue createDefaultModItems cosmetic {0}", new object[] { this.GetCosmeticMod(0) });
							}
						}
						else if (num2 < this.ModificationCount && random.RandomFloat <= num)
						{
							this.modifications[num2] = new ItemValue(itemClassModifier.Id, false);
							fastTags |= itemClassModifier.ItemTags;
							num2++;
							num *= 0.5f;
						}
					}
				}
				for (int j = num2; j < this.ModificationCount; j++)
				{
					this.modifications[j] = ItemValue.None;
				}
			}
		}
		if (!flag && !ic.HasAnyTags(ItemValue.noPreinstallCosmeticItemTags))
		{
			for (int k = 0; k < this.CosmeticModCount; k++)
			{
				ItemClassModifier cosmeticItemMod = ItemClassModifier.GetCosmeticItemMod(ic.ItemTags, fastTags, random);
				if (cosmeticItemMod != null)
				{
					this.cosmeticMods[k] = new ItemValue(cosmeticItemMod.Id, false);
					fastTags |= cosmeticItemMod.ItemTags;
				}
				else
				{
					this.cosmeticMods[k] = ItemValue.None;
				}
			}
		}
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x00125DB4 File Offset: 0x00123FB4
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
				for (int i = 0; i < this.CosmeticModCount; i++)
				{
					ItemValue cosmeticMod = this.GetCosmeticMod(i);
					if (cosmeticMod != null && cosmeticMod.ItemClass is ItemClassModifier)
					{
						cosmeticMod.ModifyValue(_entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags, true, false);
					}
				}
				for (int j = 0; j < this.ModificationCount; j++)
				{
					ItemValue modification = this.GetModification(j);
					if (modification != null && modification.ItemClass is ItemClassModifier)
					{
						modification.ModifyValue(_entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags, true, false);
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

	// Token: 0x06002E6A RID: 11882 RVA: 0x0012608C File Offset: 0x0012428C
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
			for (int i = 0; i < this.CosmeticModCount; i++)
			{
				ItemValue cosmeticMod = this.GetCosmeticMod(i);
				if (cosmeticMod != null && cosmeticMod.ItemClass is ItemClassModifier)
				{
					cosmeticMod.GetModifiedValueData(_modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType.CosmeticMod, _entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags);
				}
			}
			for (int j = 0; j < this.ModificationCount; j++)
			{
				ItemValue modification = this.GetModification(j);
				if (modification != null && modification.ItemClass is ItemClassModifier)
				{
					modification.GetModifiedValueData(_modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType.Mod, _entity, _originalItemValue, _passiveEffect, ref _originalValue, ref _perc_value, _tags);
				}
			}
		}
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x001261B4 File Offset: 0x001243B4
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
		for (int i = 0; i < this.ModificationCount; i++)
		{
			ItemValue modification = this.GetModification(i);
			if (modification != null)
			{
				modification.FireEvent(_eventType, _eventParms);
			}
		}
		for (int j = 0; j < this.CosmeticModCount; j++)
		{
			ItemValue cosmeticMod = this.GetCosmeticMod(j);
			if (cosmeticMod != null)
			{
				cosmeticMod.FireEvent(_eventType, _eventParms);
			}
		}
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x00126298 File Offset: 0x00124498
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
		itemValue.modifications = this.modifications.CloneItemValueArray();
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
		itemValue.cosmeticMods = this.cosmeticMods.CloneItemValueArray();
		itemValue.Flags = this.Flags;
		if (itemValue.type == 0)
		{
			this.Seed = 0;
		}
		itemValue.Seed = this.Seed;
		itemValue.TextureFullArray = this.TextureFullArray;
		return itemValue;
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x001263E4 File Offset: 0x001245E4
	public bool IsEmpty()
	{
		return this.type == 0;
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x001263F0 File Offset: 0x001245F0
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

	// Token: 0x06002E6F RID: 11887 RVA: 0x0012644C File Offset: 0x0012464C
	public void MergeBest(ItemValue _iv)
	{
		this.Flags |= 2;
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
			this.modifications = _iv.modifications.CloneItemValueArray();
		}
		if (_iv.HasCosmetics())
		{
			this.cosmeticMods = _iv.cosmeticMods.CloneItemValueArray();
		}
		int num5 = this.CalcModSlotCount();
		if (num5 == 0)
		{
			this.modifications = null;
			return;
		}
		Array.Resize<ItemValue>(ref this.modifications, num5);
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x00126597 File Offset: 0x00124797
	public void InitStats(int _count)
	{
		this.Stats = new ItemValue.Stat[_count];
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x001265A5 File Offset: 0x001247A5
	public void ClearStats()
	{
		this.Stats = null;
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x001265B0 File Offset: 0x001247B0
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

	// Token: 0x06002E73 RID: 11891 RVA: 0x00126657 File Offset: 0x00124857
	public bool HasStats()
	{
		return this.Stats != null;
	}

	// Token: 0x06002E74 RID: 11892 RVA: 0x00126664 File Offset: 0x00124864
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

	// Token: 0x06002E75 RID: 11893 RVA: 0x001266A4 File Offset: 0x001248A4
	public float GetStatPercent(PassiveEffects _type, bool _onlyBoosted)
	{
		float num = 1f;
		if (this.Stats != null)
		{
			this.StatModifyValue(_type, ref num, _onlyBoosted);
		}
		return num;
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x001266CA File Offset: 0x001248CA
	public void SetStat(int _index, PassiveEffects _type, int _base, int _added)
	{
		this.Stats[_index] = new ItemValue.Stat(_type, _base, _added);
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x001266E4 File Offset: 0x001248E4
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

	// Token: 0x06002E78 RID: 11896 RVA: 0x00126802 File Offset: 0x00124A02
	public static bool IsStatLowerBetter(PassiveEffects _type)
	{
		return _type == PassiveEffects.StaminaLoss || _type == PassiveEffects.TargetArmor;
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x00126814 File Offset: 0x00124A14
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

	// Token: 0x06002E7A RID: 11898 RVA: 0x000027FC File Offset: 0x000009FC
	public void ReadOld(PooledBinaryReader _br)
	{
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x00126878 File Offset: 0x00124A78
	public static ItemValue ReadOrNull(PooledBinaryReader _br)
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

	// Token: 0x06002E7C RID: 11900 RVA: 0x001268A0 File Offset: 0x00124AA0
	public void Read(PooledBinaryReader _br)
	{
		this.ResetFields();
		byte b = _br.ReadByte();
		if (b != 0)
		{
			this.ReadData(_br, (int)b);
		}
		this.NotifyChanged();
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x001268CC File Offset: 0x00124ACC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadData(PooledBinaryReader _br, int version)
	{
		int num = 0;
		if (version >= 8)
		{
			num = (int)_br.ReadByte();
		}
		this.typeField = (int)_br.ReadUInt16();
		if ((num & 1) > 0)
		{
			this.typeField += Block.ItemsStartHere;
		}
		if (version < 8 && this.typeField >= 32768)
		{
			this.typeField += 32768;
		}
		ItemClass itemClass = this.ItemClass;
		if (version > 5)
		{
			this.useTimes = _br.ReadSingle();
		}
		else
		{
			this.useTimes = (float)_br.ReadUInt16();
		}
		this.quality = _br.ReadUInt16();
		if (itemClass != null && itemClass.QualityMin > 0)
		{
			this.quality = (ushort)((byte)Utils.FastMax((int)this.quality, (int)itemClass.QualityMin));
		}
		this.meta = (int)_br.ReadUInt16();
		if (this.meta >= 65535)
		{
			this.meta = -1;
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
			this.modifications = ((b > 0) ? new ItemValue[(int)b] : null);
			if (b != 0)
			{
				for (int k = 0; k < (int)b; k++)
				{
					if (_br.ReadBoolean())
					{
						this.modifications[k] = new ItemValue();
						this.modifications[k].Read(_br);
					}
					else
					{
						this.modifications[k] = ItemValue.None;
					}
				}
			}
			b = _br.ReadByte();
			this.cosmeticMods = ((b > 0) ? new ItemValue[(int)b] : null);
			if (b != 0)
			{
				for (int l = 0; l < (int)b; l++)
				{
					if (_br.ReadBoolean())
					{
						this.cosmeticMods[l] = new ItemValue();
						this.cosmeticMods[l].Read(_br);
					}
					else
					{
						this.cosmeticMods[l] = ItemValue.None;
					}
				}
			}
		}
		this.flags = _br.ReadByte();
		if (version > 2)
		{
			this.selectedAmmoTypeIndex = _br.ReadByte();
		}
		if (version > 3)
		{
			this.seed = _br.ReadUInt16();
			if (this.typeField == 0)
			{
				this.seed = 0;
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

	// Token: 0x06002E7E RID: 11902 RVA: 0x00126B88 File Offset: 0x00124D88
	public void AdjustForSandboxOptions()
	{
		if (!EntityPlayerLocal.PermaDegrationOn)
		{
			this.RemoveMetaData("DurabilityModifier");
		}
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x00126B9D File Offset: 0x00124D9D
	public static void Write(ItemValue _iv, PooledBinaryWriter _bw)
	{
		if (_iv == null)
		{
			_bw.Write(0);
			return;
		}
		_iv.Write(_bw);
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x00126BB4 File Offset: 0x00124DB4
	public void Write(PooledBinaryWriter _bw)
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
			_bw.Write((byte)this.ModificationCount);
			for (int j = 0; j < this.ModificationCount; j++)
			{
				ItemValue modification = this.GetModification(j);
				bool flag = modification != null && !modification.IsEmpty();
				_bw.Write(flag);
				if (flag)
				{
					modification.Write(_bw);
				}
			}
			_bw.Write((byte)this.CosmeticModCount);
			for (int k = 0; k < this.CosmeticModCount; k++)
			{
				ItemValue cosmeticMod = this.GetCosmeticMod(k);
				bool flag2 = cosmeticMod != null && !cosmeticMod.IsEmpty();
				_bw.Write(flag2);
				if (flag2)
				{
					cosmeticMod.Write(_bw);
				}
			}
		}
		_bw.Write(this.Flags);
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

	// Token: 0x06002E81 RID: 11905 RVA: 0x00126EE0 File Offset: 0x001250E0
	public bool Equals(ItemValue _other)
	{
		return _other != null && (_other.type == this.type && _other.UseTimes == this.UseTimes && _other.Meta == this.Meta && _other.Seed == this.Seed && _other.Quality == this.Quality && _other.SelectedAmmoTypeIndex == this.SelectedAmmoTypeIndex && _other.Activated == this.Activated && ItemValue.Equals(_other.Metadata, this.Metadata) && ItemValue.Equals(_other.Stats, this.Stats) && ItemValue.Equals(_other.cosmeticMods, this.cosmeticMods)) && ItemValue.Equals(_other.modifications, this.modifications);
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x00126FA9 File Offset: 0x001251A9
	public override bool Equals(object _other)
	{
		return _other is ItemValue && this.Equals((ItemValue)_other);
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x00126FC4 File Offset: 0x001251C4
	public bool EqualsExceptUseTimesAndMeta(ItemValue _other)
	{
		return _other != null && (_other.type == this.type && _other.Seed == this.Seed && _other.Quality == this.Quality && _other.SelectedAmmoTypeIndex == this.SelectedAmmoTypeIndex && _other.Activated == this.Activated && ItemValue.Equals(_other.Metadata, this.Metadata) && ItemValue.Equals(_other.Stats, this.Stats) && ItemValue.Equals(_other.cosmeticMods, this.cosmeticMods)) && ItemValue.Equals(_other.modifications, this.modifications);
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x0012706C File Offset: 0x0012526C
	public bool EqualsForMerging(ItemValue _other)
	{
		if (_other == null)
		{
			return false;
		}
		bool flag = ItemAction.RepairType == ItemAction.RepairTypes.Both || ItemAction.RepairType == ItemAction.RepairTypes.CombineOnly;
		return _other.type == this.type && (!flag || this.UseTimes + _other.UseTimes == 0f || ((float)this.MaxUseTimes == this.UseTimes && (float)_other.MaxUseTimes == _other.UseTimes)) && ItemValue.Equals(_other.Stats, this.Stats);
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x001270E8 File Offset: 0x001252E8
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

	// Token: 0x06002E86 RID: 11910 RVA: 0x00127149 File Offset: 0x00125349
	public static bool Equals(ItemValue.Stat[] _a, ItemValue.Stat[] _b)
	{
		return (_a == null && _b == null) || (_a != null && _b != null && _a.Length == _b.Length && (_a.Length == 0 || _a.SequenceEqual<ItemValue.Stat>(_b)));
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x00127172 File Offset: 0x00125372
	public static bool Equals(object[] _a, object[] _b)
	{
		return _a is ItemValue[] && _b is ItemValue[] && ItemValue.Equals((ItemValue[])_a, (ItemValue[])_b);
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x00127198 File Offset: 0x00125398
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

	// Token: 0x06002E89 RID: 11913 RVA: 0x00127238 File Offset: 0x00125438
	public override int GetHashCode()
	{
		return this.type;
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x00127240 File Offset: 0x00125440
	public int GetItemId()
	{
		return this.type - Block.ItemsStartHere;
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x0012724E File Offset: 0x0012544E
	public int GetItemOrBlockId()
	{
		if (this.type < Block.ItemsStartHere)
		{
			return this.type;
		}
		return this.type - Block.ItemsStartHere;
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x00127270 File Offset: 0x00125470
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

	// Token: 0x06002E8D RID: 11917 RVA: 0x00127304 File Offset: 0x00125504
	public string GetPropertyOverride(string _propertyName, string _originalValue)
	{
		if (this.ModificationCount == 0 && this.CosmeticModCount == 0)
		{
			return _originalValue;
		}
		string text = "";
		string itemName = this.ItemClass.GetItemName();
		for (int i = 0; i < this.ModificationCount; i++)
		{
			ItemValue modification = this.GetModification(i);
			if (modification != null)
			{
				ItemClassModifier itemClassModifier = modification.ItemClass as ItemClassModifier;
				if (itemClassModifier != null && itemClassModifier.GetPropertyOverride(_propertyName, itemName, ref text))
				{
					return text;
				}
			}
		}
		text = "";
		for (int j = 0; j < this.CosmeticModCount; j++)
		{
			ItemValue cosmeticMod = this.GetCosmeticMod(j);
			if (cosmeticMod != null)
			{
				ItemClassModifier itemClassModifier2 = cosmeticMod.ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && itemClassModifier2.GetPropertyOverride(_propertyName, itemName, ref text))
				{
					return text;
				}
			}
		}
		return _originalValue;
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x001273BC File Offset: 0x001255BC
	public bool HasCosmetics()
	{
		for (int i = 0; i < this.CosmeticModCount; i++)
		{
			ItemValue cosmeticMod = this.GetCosmeticMod(i);
			if (cosmeticMod != null && !cosmeticMod.IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x001273F0 File Offset: 0x001255F0
	public bool HasMods()
	{
		for (int i = 0; i < this.ModificationCount; i++)
		{
			ItemValue modification = this.GetModification(i);
			if (modification != null && !modification.IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040023E1 RID: 9185
	[PublicizedFrom(EAccessModifier.Private)]
	public const int EmptySaveVersion = 0;

	// Token: 0x040023E2 RID: 9186
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 9;

	// Token: 0x040023E3 RID: 9187
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte MaxModifications = 255;

	// Token: 0x040023E4 RID: 9188
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> noPreinstallCosmeticItemTags = FastTags<TagGroup.Global>.Parse("weapon,tool,armor");

	// Token: 0x040023E5 RID: 9189
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsActivated = 1;

	// Token: 0x040023E6 RID: 9190
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsWasCombined = 2;

	// Token: 0x040023E7 RID: 9191
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack owner;

	// Token: 0x040023E8 RID: 9192
	[PublicizedFrom(EAccessModifier.Private)]
	public int typeField;

	// Token: 0x040023E9 RID: 9193
	[PublicizedFrom(EAccessModifier.Private)]
	public byte flags;

	// Token: 0x040023EA RID: 9194
	[PublicizedFrom(EAccessModifier.Private)]
	public byte selectedAmmoTypeIndex;

	// Token: 0x040023EB RID: 9195
	[PublicizedFrom(EAccessModifier.Private)]
	public float useTimes;

	// Token: 0x040023EC RID: 9196
	[PublicizedFrom(EAccessModifier.Private)]
	public int meta;

	// Token: 0x040023ED RID: 9197
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort quality;

	// Token: 0x040023EE RID: 9198
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, TypedMetadataValue> Metadata;

	// Token: 0x040023F0 RID: 9200
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] modifications;

	// Token: 0x040023F1 RID: 9201
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] cosmeticMods;

	// Token: 0x040023F2 RID: 9202
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort seed;

	// Token: 0x040023F3 RID: 9203
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray textureFullArray;

	// Token: 0x040023F4 RID: 9204
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDurabilityMetaName = "DurabilityModifier";

	// Token: 0x040023F5 RID: 9205
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime baseDate = new DateTime(2013, 10, 1);

	// Token: 0x040023F6 RID: 9206
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cStreamFlagItem = 1;

	// Token: 0x040023F7 RID: 9207
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cStreamFlagStats = 2;

	// Token: 0x02000589 RID: 1417
	[StructLayout(LayoutKind.Auto)]
	public struct Stat
	{
		// Token: 0x06002E91 RID: 11921 RVA: 0x00127447 File Offset: 0x00125647
		public Stat(PassiveEffects _type, int _base, int _added)
		{
			this.type = _type;
			this.isBoosted = _added != 0;
			this.value = (short)(_base + _added);
		}

		// Token: 0x040023F8 RID: 9208
		public PassiveEffects type;

		// Token: 0x040023F9 RID: 9209
		public bool isBoosted;

		// Token: 0x040023FA RID: 9210
		public short value;
	}
}
