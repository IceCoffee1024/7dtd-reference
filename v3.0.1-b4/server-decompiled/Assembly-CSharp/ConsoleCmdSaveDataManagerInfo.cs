using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000278 RID: 632
[Preserve]
public class ConsoleCmdSaveDataManagerInfo : ConsoleCmdAbstract
{
	// Token: 0x060012B4 RID: 4788 RVA: 0x000745E0 File Offset: 0x000727E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sdminfo" };
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x000745F0 File Offset: 0x000727F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "SaveDataManager Information";
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000745F7 File Offset: 0x000727F7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "sdminfo";
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x00074600 File Offset: 0x00072800
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ISaveDataManager saveDataManager = SaveDataUtils.SaveDataManager;
		this.AppendSaveDataManagerInfo(stringBuilder, saveDataManager);
		IPlatformSaveGameProvider saveGameProvider = PlatformManager.MultiPlatform.SaveGameProvider;
		this.AppendSaveGameProviderInfo(stringBuilder, saveGameProvider);
		Log.Out(stringBuilder.TrimEnd().ToString());
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x00074644 File Offset: 0x00072844
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

	// Token: 0x060012BA RID: 4794 RVA: 0x00074680 File Offset: 0x00072880
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

	// Token: 0x060012BB RID: 4795 RVA: 0x00074708 File Offset: 0x00072908
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
