using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020001E0 RID: 480
public abstract class ConsoleCmdAbstract : IConsoleCommand
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int DefaultPermissionLevel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00060491 File Offset: 0x0005E691
	public virtual DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00060491 File Offset: 0x0005E691
	public virtual DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;
		}
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0000640C File Offset: 0x0000460C
	public ConsoleCmdAbstract()
	{
	}

	// Token: 0x06000EB8 RID: 3768
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract string[] getCommands();

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00060494 File Offset: 0x0005E694
	public virtual string[] GetCommands()
	{
		string[] array;
		if ((array = this.commandNamesCache) == null)
		{
			array = (this.commandNamesCache = this.getCommands());
		}
		return array;
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000EBA RID: 3770 RVA: 0x000604BC File Offset: 0x0005E6BC
	public virtual string PrimaryCommand
	{
		get
		{
			string text;
			if ((text = this.primaryCommand) == null)
			{
				text = (this.primaryCommand = this.GetCommands()[0]);
			}
			return text;
		}
	}

	// Token: 0x06000EBB RID: 3771
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract string getDescription();

	// Token: 0x06000EBC RID: 3772 RVA: 0x000604E4 File Offset: 0x0005E6E4
	public virtual string GetDescription()
	{
		string text;
		if ((text = this.commandDescriptionCache) == null)
		{
			text = (this.commandDescriptionCache = this.getDescription());
		}
		return text;
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x000200DA File Offset: 0x0001E2DA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string getHelp()
	{
		return null;
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0006050C File Offset: 0x0005E70C
	public virtual string GetHelp()
	{
		string text;
		if ((text = this.commandHelpCache) == null)
		{
			text = (this.commandHelpCache = this.getHelp());
		}
		return text;
	}

	// Token: 0x06000EBF RID: 3775
	public abstract void Execute(List<string> _params, CommandSenderInfo _senderInfo);

	// Token: 0x04000C80 RID: 3200
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] commandNamesCache;

	// Token: 0x04000C81 RID: 3201
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandDescriptionCache;

	// Token: 0x04000C82 RID: 3202
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandHelpCache;

	// Token: 0x04000C83 RID: 3203
	[PublicizedFrom(EAccessModifier.Private)]
	public string primaryCommand;
}
