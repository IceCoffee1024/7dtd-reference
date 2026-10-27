using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x020011C2 RID: 4546
public class EntitlementManager
{
	// Token: 0x1700118F RID: 4495
	// (get) Token: 0x060090E8 RID: 37096 RVA: 0x00368F7C File Offset: 0x0036717C
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

	// Token: 0x060090E9 RID: 37097 RVA: 0x00368FDC File Offset: 0x003671DC
	public bool HasEntitlement(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.HasEntitlement(entitlementSet);
	}

	// Token: 0x060090EA RID: 37098 RVA: 0x00368FF8 File Offset: 0x003671F8
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

	// Token: 0x060090EB RID: 37099 RVA: 0x00369098 File Offset: 0x00367298
	public bool IsAvailableOnPlatform(object _addressableKey)
	{
		EntitlementSetEnum setForAsset = this.GetSetForAsset(_addressableKey);
		return this.IsAvailableOnPlatform(setForAsset);
	}

	// Token: 0x060090EC RID: 37100 RVA: 0x003690B4 File Offset: 0x003672B4
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

	// Token: 0x060090ED RID: 37101 RVA: 0x00369154 File Offset: 0x00367354
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

	// Token: 0x060090EE RID: 37102 RVA: 0x00369204 File Offset: 0x00367404
	public bool IsEntitlementPurchasable(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.IsEntitlementPurchasable(entitlementSet);
	}

	// Token: 0x060090EF RID: 37103 RVA: 0x00369220 File Offset: 0x00367420
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

	// Token: 0x060090F0 RID: 37104 RVA: 0x003692A0 File Offset: 0x003674A0
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

	// Token: 0x060090F1 RID: 37105 RVA: 0x00369350 File Offset: 0x00367550
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

	// Token: 0x060090F2 RID: 37106 RVA: 0x003693CC File Offset: 0x003675CC
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

	// Token: 0x060090F3 RID: 37107 RVA: 0x00369450 File Offset: 0x00367650
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

	// Token: 0x060090F4 RID: 37108 RVA: 0x003694F8 File Offset: 0x003676F8
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

	// Token: 0x060090F5 RID: 37109 RVA: 0x00369564 File Offset: 0x00367764
	[return: TupleElementNames(new string[] { "hasOverride", "overrideValue" })]
	public ValueTuple<bool, bool> CheckOverride(EntitlementSetEnum _set)
	{
		return new ValueTuple<bool, bool>(false, false);
	}

	// Token: 0x04006B1F RID: 27423
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x04006B20 RID: 27424
	[PublicizedFrom(EAccessModifier.Private)]
	public static EntitlementManager _instance;

	// Token: 0x04006B21 RID: 27425
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<IEntitlementValidator> entitlementValidators = new List<IEntitlementValidator>();
}
