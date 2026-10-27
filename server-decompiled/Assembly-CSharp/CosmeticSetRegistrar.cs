using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011B8 RID: 4536
public static class CosmeticSetRegistrar
{
	// Token: 0x060090AC RID: 37036 RVA: 0x00367464 File Offset: 0x00365664
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RegisterAddressablePrefixes()
	{
		IDictionary<string, EntitlementSetEnum> addressablesStringMap = EntitlementAddressablesMaps.AddressablesStringMap;
		foreach (ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]> valueTuple in CosmeticSets.All)
		{
			EntitlementSetEnum entitlementSetEnum;
			if (addressablesStringMap.TryGetValue(valueTuple.Item2, out entitlementSetEnum))
			{
				if (entitlementSetEnum != valueTuple.Item1)
				{
					Log.Warning(string.Format("[Cosmetics] '{0}' is hardcoded to {1} in ", valueTuple.Item2, entitlementSetEnum) + string.Format("EntitlementAddressableMappings.cs but {0} claims it. Keeping {1}.", valueTuple.Item1, entitlementSetEnum));
				}
			}
			else
			{
				addressablesStringMap.Add(valueTuple.Item2, valueTuple.Item1);
			}
		}
	}

	// Token: 0x060090AD RID: 37037 RVA: 0x00367500 File Offset: 0x00365700
	public static void RegisterCosmeticMappings()
	{
		ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>[] all = CosmeticSets.All;
		for (int i = 0; i < all.Length; i++)
		{
			foreach (ValueTuple<string, int> valueTuple in all[i].Item3)
			{
				int num;
				string text;
				if (Equipment.CosmeticMappingStringID.TryGetValue(valueTuple.Item1, out num))
				{
					if (num != valueTuple.Item2)
					{
						Log.Error(string.Format("[Cosmetics] '{0}' is mapped to id {1}, generated data says {2}. Keeping {3}.", new object[] { valueTuple.Item1, num, valueTuple.Item2, num }));
					}
				}
				else if (Equipment.CosmeticMappingIDString.TryGetValue(valueTuple.Item2, out text))
				{
					Log.Error(string.Format("[Cosmetics] id {0} already used by '{1}'; skipping '{2}'.", valueTuple.Item2, text, valueTuple.Item1));
				}
				else
				{
					Equipment.AddCosmeticMapping(valueTuple.Item2, valueTuple.Item1);
				}
			}
		}
	}
}
