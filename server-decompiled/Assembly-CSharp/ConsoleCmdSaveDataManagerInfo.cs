using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000281 RID: 641
[Preserve]
public class ConsoleCmdSaveDataManagerInfo : ConsoleCmdAbstract
{
	// Token: 0x060012C7 RID: 4807 RVA: 0x000750C0 File Offset: 0x000732C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sdminfo" };
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x000750D0 File Offset: 0x000732D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "SaveDataManager Information";
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x000750D7 File Offset: 0x000732D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "sdminfo";
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x000750E0 File Offset: 0x000732E0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ISaveDataManager saveDataManager = SaveDataUtils.SaveDataManager;
		this.AppendSaveDataManagerInfo(stringBuilder, saveDataManager);
		IPlatformSaveGameProvider saveGameProvider = PlatformManager.MultiPlatform.SaveGameProvider;
		this.AppendSaveGameProviderInfo(stringBuilder, saveGameProvider);
		Log.Out(stringBuilder.TrimEnd().ToString());
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x00075124 File Offset: 0x00073324
	[PublicizedFrom(EAccessModifier.Private)]
	public void AppendSaveDataManagerInfo(StringBuilder builder, ISaveDataManager saveDataManager)
	{
		if (saveDataManager == null)
		{
			builder.AppendLine("No ISaveDataManager available.");
			return;
		}
		builder.AppendLine("Save Data Manager Info:");
		builder.AppendLine(string.Format("\tWrite Mode: {0}", saveDataManager.GetWriteMode()));
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x00075160 File Offset: 0x00073360
	[PublicizedFrom(EAccessModifier.Private)]
	public void AppendSaveGameProviderInfo(StringBuilder builder, IPlatformSaveGameProvider saveGameProvider)
	{
		if (saveGameProvider == null)
		{
			builder.AppendLine("No IPlatformSaveGameProvider available.");
			return;
		}
		builder.AppendLine("Save Game Provider Info:");
		builder.AppendLine(string.Format("\tStatus: {0}", saveGameProvider.Status));
		builder.AppendLine(string.Format("\tShould Backup: {0}", saveGameProvider.ShouldBackup()));
		builder.AppendLine(string.Format("\tShould Commit: {0}", saveGameProvider.ShouldCommit()));
		this.AppendSaveGameProviderSizeInfo(builder, saveGameProvider);
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x000751E8 File Offset: 0x000733E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void AppendSaveGameProviderSizeInfo(StringBuilder builder, IPlatformSaveGameProvider saveGameProvider)
	{
		bool flag = saveGameProvider.ShouldLimitSize();
		builder.AppendLine(string.Format("\tShould Limit Size: {0}", flag));
		if (!flag)
		{
			return;
		}
		saveGameProvider.UpdateSizes();
		SaveDataSizes sizes = saveGameProvider.GetSizes();
		builder.AppendLine("\tUsed Size      : " + sizes.Used.FormatSize(true));
		builder.AppendLine("\tRemaining Size : " + sizes.Remaining.FormatSize(true));
		builder.AppendLine("\tTotal Size     : " + sizes.Total.FormatSize(true));
	}
}
