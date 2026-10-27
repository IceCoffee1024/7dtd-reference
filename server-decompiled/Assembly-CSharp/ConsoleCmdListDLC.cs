using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002CA RID: 714
[Preserve]
public class ConsoleCmdListDLC : ConsoleCmdAbstract
{
	// Token: 0x0600147A RID: 5242 RVA: 0x0007C97E File Offset: 0x0007AB7E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listdlc", "dlcs" };
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600147B RID: 5243 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600147C RID: 5244 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x0007C996 File Offset: 0x0007AB96
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List the available DLC and their current entitlement status.";
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0007C99D File Offset: 0x0007AB9D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "List DLCs and their entitlement states.\nUsage:\n   listdlc\n   dlcs\n";
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0007C9A4 File Offset: 0x0007ABA4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		ConsoleCmdListDLC.ExecuteList();
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0007C9AC File Offset: 0x0007ABAC
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
