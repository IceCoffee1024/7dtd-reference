using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027F RID: 639
[Preserve]
public class ConsoleCmdSaveDataManagerInfo : ConsoleCmdAbstract
{
	// Token: 0x060012C3 RID: 4803 RVA: 0x00074EF0 File Offset: 0x000730F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sdminfo" };
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x00074F00 File Offset: 0x00073100
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "SaveDataManager Information";
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x00074F07 File Offset: 0x00073107
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "sdminfo";
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x00074F10 File Offset: 0x00073110
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ISaveDataManager saveDataManager = SaveDataUtils.SaveDataManager;
		this.AppendSaveDataManagerInfo(stringBuilder, saveDataManager);
		IPlatformSaveGameProvider saveGameProvider = PlatformManager.MultiPlatform.SaveGameProvider;
		this.AppendSaveGameProviderInfo(stringBuilder, saveGameProvider);
		Log.Out(stringBuilder.TrimEnd().ToString());
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00074F54 File Offset: 0x00073154
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

	// Token: 0x060012C9 RID: 4809 RVA: 0x00074F90 File Offset: 0x00073190
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

	// Token: 0x060012CA RID: 4810 RVA: 0x00075018 File Offset: 0x00073218
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
