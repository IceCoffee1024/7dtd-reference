using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002CB RID: 715
[Preserve]
public interface IConsoleCommand
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06001482 RID: 5250
	bool IsExecuteOnClient { get; }

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06001483 RID: 5251
	int DefaultPermissionLevel { get; }

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06001484 RID: 5252
	bool AllowedInMainMenu { get; }

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06001485 RID: 5253
	DeviceFlag AllowedDeviceTypes { get; }

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001486 RID: 5254
	DeviceFlag AllowedDeviceTypesClient { get; }

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001487 RID: 5255 RVA: 0x0007CAC4 File Offset: 0x0007ACC4
	bool CanExecuteForDevice
	{
		get
		{
			if (!Submission.Enabled)
			{
				return true;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				return this.AllowedDeviceTypesClient.IsCurrent();
			}
			return this.AllowedDeviceTypes.IsCurrent();
		}
	}

	// Token: 0x06001488 RID: 5256
	string[] GetCommands();

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06001489 RID: 5257
	string PrimaryCommand { get; }

	// Token: 0x0600148A RID: 5258
	string GetDescription();

	// Token: 0x0600148B RID: 5259
	string GetHelp();

	// Token: 0x0600148C RID: 5260
	void Execute(List<string> _params, CommandSenderInfo _senderInfo);
}
