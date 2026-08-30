using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02001180 RID: 4480
public class EntitlementManager
{
	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06008F0E RID: 36622 RVA: 0x0035DAC8 File Offset: 0x0035BCC8
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

	// Token: 0x06008F0F RID: 36623 RVA: 0x0035DB28 File Offset: 0x0035BD28
	public bool HasEntitlement(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.HasEntitlement(entitlementSet);
	}

	// Token: 0x06008F10 RID: 36624 RVA: 0x0035DB44 File Offset: 0x0035BD44
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

	// Token: 0x06008F11 RID: 36625 RVA: 0x0035DBE4 File Offset: 0x0035BDE4
	public bool IsAvailableOnPlatform(object _addressableKey)
	{
		EntitlementSetEnum setForAsset = this.GetSetForAsset(_addressableKey);
		return this.IsAvailableOnPlatform(setForAsset);
	}

	// Token: 0x06008F12 RID: 36626 RVA: 0x0035DC00 File Offset: 0x0035BE00
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

	// Token: 0x06008F13 RID: 36627 RVA: 0x0035DCA0 File Offset: 0x0035BEA0
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

	// Token: 0x06008F14 RID: 36628 RVA: 0x0035DD50 File Offset: 0x0035BF50
	public bool IsEntitlementPurchasable(object _addressableKey)
	{
		EntitlementSetEnum entitlementSet = this.GetEntitlementSet(_addressableKey);
		return this.IsEntitlementPurchasable(entitlementSet);
	}

	// Token: 0x06008F15 RID: 36629 RVA: 0x0035DD6C File Offset: 0x0035BF6C
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

	// Token: 0x06008F16 RID: 36630 RVA: 0x0035DDEC File Offset: 0x0035BFEC
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

	// Token: 0x06008F17 RID: 36631 RVA: 0x0035DE9C File Offset: 0x0035C09C
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

	// Token: 0x06008F18 RID: 36632 RVA: 0x0035DF18 File Offset: 0x0035C118
	[return: TupleElementNames(new string[] { "hasOverride", "overrideValue" })]
	public ValueTuple<bool, bool> CheckOverride(EntitlementSetEnum _set)
	{
		return new ValueTuple<bool, bool>(false, false);
	}

	// Token: 0x040069CF RID: 27087
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x040069D0 RID: 27088
	[PublicizedFrom(EAccessModifier.Private)]
	public static EntitlementManager _instance;

	// Token: 0x040069D1 RID: 27089
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<IEntitlementValidator> entitlementValidators = new List<IEntitlementValidator>();
}
