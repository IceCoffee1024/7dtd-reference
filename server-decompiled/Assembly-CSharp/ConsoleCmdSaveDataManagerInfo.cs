using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027C RID: 636
[Preserve]
public class ConsoleCmdSaveDataManagerInfo : ConsoleCmdAbstract
{
	// Token: 0x060012C6 RID: 4806 RVA: 0x00074C3C File Offset: 0x00072E3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sdminfo" };
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00074C4C File Offset: 0x00072E4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "SaveDataManager Information";
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00074C53 File Offset: 0x00072E53
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "sdminfo";
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x00074C5C File Offset: 0x00072E5C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ISaveDataManager saveDataManager = SaveDataUtils.SaveDataManager;
		this.AppendSaveDataManagerInfo(stringBuilder, saveDataManager);
		IPlatformSaveGameProvider saveGameProvider = PlatformManager.MultiPlatform.SaveGameProvider;
		this.AppendSaveGameProviderInfo(stringBuilder, saveGameProvider);
		Log.Out(stringBuilder.TrimEnd().ToString());
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x00074CA0 File Offset: 0x00072EA0
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

	// Token: 0x060012CC RID: 4812 RVA: 0x00074CDC File Offset: 0x00072EDC
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

	// Token: 0x060012CD RID: 4813 RVA: 0x00074D64 File Offset: 0x00072F64
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
