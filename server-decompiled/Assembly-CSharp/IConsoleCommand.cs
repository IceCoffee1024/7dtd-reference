using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002C2 RID: 706
[Preserve]
public interface IConsoleCommand
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x0600146F RID: 5231
	bool IsExecuteOnClient { get; }

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06001470 RID: 5232
	int DefaultPermissionLevel { get; }

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06001471 RID: 5233
	bool AllowedInMainMenu { get; }

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06001472 RID: 5234
	DeviceFlag AllowedDeviceTypes { get; }

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06001473 RID: 5235
	DeviceFlag AllowedDeviceTypesClient { get; }

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06001474 RID: 5236 RVA: 0x0007BFE8 File Offset: 0x0007A1E8
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

	// Token: 0x06001475 RID: 5237
	string[] GetCommands();

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001476 RID: 5238
	string PrimaryCommand { get; }

	// Token: 0x06001477 RID: 5239
	string GetDescription();

	// Token: 0x06001478 RID: 5240
	string GetHelp();

	// Token: 0x06001479 RID: 5241
	void Execute(List<string> _params, CommandSenderInfo _senderInfo);
}
