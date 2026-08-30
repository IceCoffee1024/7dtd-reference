using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002C1 RID: 705
[Preserve]
public class ConsoleCmdListDLC : ConsoleCmdAbstract
{
	// Token: 0x06001467 RID: 5223 RVA: 0x0007BEA2 File Offset: 0x0007A0A2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listdlc", "dlcs" };
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001468 RID: 5224 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06001469 RID: 5225 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x0007BEBA File Offset: 0x0007A0BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List the available DLC and their current entitlement status.";
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x0007BEC1 File Offset: 0x0007A0C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "List DLCs and their entitlement states.\nUsage:\n   listdlc\n   dlcs\n";
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x0007BEC8 File Offset: 0x0007A0C8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		ConsoleCmdListDLC.ExecuteList();
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x0007BED0 File Offset: 0x0007A0D0
	public static void ExecuteList()
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("DLC List:");
		foreach (object obj in Enum.GetValues(typeof(EntitlementSetEnum)))
		{
			EntitlementSetEnum entitlementSetEnum = (EntitlementSetEnum)obj;
			if (entitlementSetEnum != EntitlementSetEnum.None)
			{
				ValueTuple<bool, bool> valueTuple = EntitlementManager.Instance.CheckOverride(entitlementSetEnum);
				bool item = valueTuple.Item1;
				bool item2 = valueTuple.Item2;
				bool flag = EntitlementManager.Instance.HasEntitlement(entitlementSetEnum);
				bool flag2 = EntitlementManager.Instance.IsAvailableOnPlatform(entitlementSetEnum);
				bool flag3 = EntitlementManager.Instance.IsEntitlementPurchasable(entitlementSetEnum);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("  {0}: {1} => Entitlement State: {2}, Available on Platform: {3}, Purchasable: {4}, Override State: {5}", new object[]
				{
					(int)entitlementSetEnum,
					entitlementSetEnum,
					flag,
					flag2,
					flag3,
					item ? item2.ToString() : "Not Overridden"
				}));
			}
		}
	}
}
