using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x020011B2 RID: 4530
public class EntitlementManager
{
	// Token: 0x17001184 RID: 4484
	// (get) Token: 0x06009092 RID: 37010 RVA: 0x00366D68 File Offset: 0x00364F68
	public static EntitlementManager Instance
	{
		get
		{
			if (EntitlementManager._instance == null)
			{
				EntitlementManager._instance = new EntitlementManager();
				if (PlatformManager.MultiPlatform.EntitlementValidators != null)
				{
					PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _)
					{
						object obj = EntitlementManager.lockObj;
						lock (obj)
						{
							EntitlementManager._instance.entitlementValidators = PlatformManager.MultiPlatform.EntitlementValidators;
						}
					};
				}
			}
			return EntitlementManager._instance;
		}
	}

	// Token: 0x06009093 RID: 37011 RVA: 0x00366DC8 File Offset: 0x00364FC8
	public bool HasEntitlement(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.HasEntitlement(entitlementSet);
	}

	// Token: 0x06009094 RID: 37012 RVA: 0x00366DE4 File Offset: 0x00364FE4
	public bool HasEntitlement(EntitlementSetEnum _set)
	{
		ValueTuple<bool, bool> valueTuple = this.CheckOverride(_set);
		bool item = valueTuple.Item1;
		bool item2 = valueTuple.Item2;
		if (item)
		{
			return item2;
		}
		if (_set == EntitlementSetEnum.None)
		{
			return true;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			using (IEnumerator<IEntitlementValidator> enumerator = this.entitlementValidators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasEntitlement(_set))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06009095 RID: 37013 RVA: 0x00366E84 File Offset: 0x00365084
	public bool IsAvailableOnPlatform(object _addressableKey)
	{
		EntitlementSetEnum setForAsset = this.GetSetForAsset(_addressableKey);
		return this.IsAvailableOnPlatform(setForAsset);
	}

	// Token: 0x06009096 RID: 37014 RVA: 0x00366EA0 File Offset: 0x003650A0
	public bool IsAvailableOnPlatform(EntitlementSetEnum _set)
	{
		ValueTuple<bool, bool> valueTuple = this.CheckOverride(_set);
		bool item = valueTuple.Item1;
		bool item2 = valueTuple.Item2;
		if (item)
		{
			return item2;
		}
		if (_set == EntitlementSetEnum.None)
		{
			return true;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			using (IEnumerator<IEntitlementValidator> enumerator = this.entitlementValidators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsAvailableOnPlatform(_set))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06009097 RID: 37015 RVA: 0x00366F40 File Offset: 0x00365140
	public unsafe EntitlementSetEnum GetSetForAsset(object addressableKey)
	{
		string text = addressableKey as string;
		if (text == null)
		{
			EntitlementSetEnum entitlementSetEnum;
			if (EntitlementAddressablesMaps.AddressablesKeyMap.TryGetValue(addressableKey, out entitlementSetEnum))
			{
				return entitlementSetEnum;
			}
			return EntitlementSetEnum.None;
		}
		else
		{
			if (string.IsNullOrEmpty(text))
			{
				return EntitlementSetEnum.None;
			}
			StringSpan stringSpan = text.ToLowerInvariant().Trim();
			if (stringSpan.Length >= 2 && *stringSpan[0] == 64 && *stringSpan[1] == 58)
			{
				stringSpan = stringSpan.Slice(2);
			}
			while (stringSpan.Length > 0)
			{
				EntitlementSetEnum entitlementSetEnum2;
				if (EntitlementAddressablesMaps.AddressablesStringMap.TryGetValue(stringSpan, out entitlementSetEnum2))
				{
					return entitlementSetEnum2;
				}
				int num = stringSpan.LastIndexOf('/');
				if (num == -1)
				{
					break;
				}
				stringSpan = stringSpan.Slice(0, num);
			}
			return EntitlementSetEnum.None;
		}
	}

	// Token: 0x06009098 RID: 37016 RVA: 0x00366FF0 File Offset: 0x003651F0
	public bool IsEntitlementPurchasable(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.IsEntitlementPurchasable(entitlementSet);
	}

	// Token: 0x06009099 RID: 37017 RVA: 0x0036700C File Offset: 0x0036520C
	public bool IsEntitlementPurchasable(EntitlementSetEnum _set)
	{
		if (_set == EntitlementSetEnum.None)
		{
			return true;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			using (IEnumerator<IEntitlementValidator> enumerator = this.entitlementValidators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsEntitlementPurchasable(_set))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600909A RID: 37018 RVA: 0x0036708C File Offset: 0x0036528C
	public unsafe EntitlementSetEnum GetEntitlementSet(object addressableKey)
	{
		string text = addressableKey as string;
		if (text == null)
		{
			EntitlementSetEnum entitlementSetEnum;
			if (EntitlementAddressablesMaps.AddressablesKeyMap.TryGetValue(addressableKey, out entitlementSetEnum))
			{
				return entitlementSetEnum;
			}
			return EntitlementSetEnum.None;
		}
		else
		{
			if (string.IsNullOrEmpty(text))
			{
				return EntitlementSetEnum.None;
			}
			StringSpan stringSpan = text.ToLowerInvariant().Trim();
			if (stringSpan.Length >= 2 && *stringSpan[0] == 64 && *stringSpan[1] == 58)
			{
				stringSpan = stringSpan.Slice(2);
			}
			while (stringSpan.Length > 0)
			{
				EntitlementSetEnum entitlementSetEnum2;
				if (EntitlementAddressablesMaps.AddressablesStringMap.TryGetValue(stringSpan, out entitlementSetEnum2))
				{
					return entitlementSetEnum2;
				}
				int num = stringSpan.LastIndexOf('/');
				if (num == -1)
				{
					break;
				}
				stringSpan = stringSpan.Slice(0, num);
			}
			return EntitlementSetEnum.None;
		}
	}

	// Token: 0x0600909B RID: 37019 RVA: 0x0036713C File Offset: 0x0036533C
	public void OpenStore(EntitlementSetEnum _set, Action<EntitlementSetEnum> _onPurchased)
	{
		if (_set == EntitlementSetEnum.None)
		{
			return;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			using (IEnumerator<IEntitlementValidator> enumerator = this.entitlementValidators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.OpenStore(_set, _onPurchased))
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600909C RID: 37020 RVA: 0x003671B8 File Offset: 0x003653B8
	public string GetEntitlementSetId(EntitlementSetEnum entitlementSet)
	{
		if (entitlementSet == EntitlementSetEnum.None)
		{
			return null;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			foreach (IEntitlementValidator entitlementValidator in this.entitlementValidators)
			{
				string entitlementSetId = entitlementValidator.GetEntitlementSetId(entitlementSet);
				if (entitlementSetId != null)
				{
					return entitlementSetId;
				}
			}
		}
		return null;
	}

	// Token: 0x0600909D RID: 37021 RVA: 0x0036723C File Offset: 0x0036543C
	public DateTime? GetAcquiredDate(EntitlementSetEnum entitlementSet)
	{
		if (entitlementSet == EntitlementSetEnum.None)
		{
			DateTime? dateTime = null;
			return dateTime;
		}
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			foreach (IEntitlementValidator entitlementValidator in this.entitlementValidators)
			{
				DateTime? dateTime = entitlementValidator.GetAcquiredDate(entitlementSet);
				if (dateTime != null)
				{
					DateTime valueOrDefault = dateTime.GetValueOrDefault();
					return new DateTime?(valueOrDefault);
				}
			}
		}
		return null;
	}

	// Token: 0x0600909E RID: 37022 RVA: 0x003672E4 File Offset: 0x003654E4
	public bool IsFetchingEntitlements()
	{
		object obj = EntitlementManager.lockObj;
		lock (obj)
		{
			if (this.entitlementValidators.Any<IEntitlementValidator>((IEntitlementValidator validator) => validator.IsFetchingEntitlements()))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600909F RID: 37023 RVA: 0x00367350 File Offset: 0x00365550
	[return: TupleElementNames(new string[] { "hasOverride", "overrideValue" })]
	public ValueTuple<bool, bool> CheckOverride(EntitlementSetEnum _set)
	{
		return new ValueTuple<bool, bool>(false, false);
	}

	// Token: 0x04006B07 RID: 27399
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x04006B08 RID: 27400
	[PublicizedFrom(EAccessModifier.Private)]
	public static EntitlementManager _instance;

	// Token: 0x04006B09 RID: 27401
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<IEntitlementValidator> entitlementValidators = new List<IEntitlementValidator>();
}
