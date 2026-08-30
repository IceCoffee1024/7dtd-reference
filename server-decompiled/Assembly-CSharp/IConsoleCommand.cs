using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002C6 RID: 710
[Preserve]
public interface IConsoleCommand
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06001481 RID: 5249
	bool IsExecuteOnClient { get; }

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06001482 RID: 5250
	int DefaultPermissionLevel { get; }

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06001483 RID: 5251
	bool AllowedInMainMenu { get; }

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06001484 RID: 5252
	DeviceFlag AllowedDeviceTypes { get; }

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06001485 RID: 5253
	DeviceFlag AllowedDeviceTypesClient { get; }

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001486 RID: 5254 RVA: 0x0007C640 File Offset: 0x0007A840
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

	// Token: 0x06001487 RID: 5255
	string[] GetCommands();

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001488 RID: 5256
	string PrimaryCommand { get; }

	// Token: 0x06001489 RID: 5257
	string GetDescription();

	// Token: 0x0600148A RID: 5258
	string GetHelp();

	// Token: 0x0600148B RID: 5259
	void Execute(List<string> _params, CommandSenderInfo _senderInfo);
}
