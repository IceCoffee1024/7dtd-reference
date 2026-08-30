using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020001DB RID: 475
public abstract class ConsoleCmdAbstract : IConsoleCommand
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int DefaultPermissionLevel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public virtual DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public virtual DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0000640C File Offset: 0x0000460C
	public ConsoleCmdAbstract()
	{
	}

	// Token: 0x06000EB6 RID: 3766
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract string[] getCommands();

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0005FC98 File Offset: 0x0005DE98
	public virtual string[] GetCommands()
	{
		string[] array;
		if ((array = this.commandNamesCache) == null)
		{
			array = (this.commandNamesCache = this.getCommands());
		}
		return array;
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0005FCC0 File Offset: 0x0005DEC0
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

	// Token: 0x06000EB9 RID: 3769
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract string getDescription();

	// Token: 0x06000EBA RID: 3770 RVA: 0x0005FCE8 File Offset: 0x0005DEE8
	public virtual string GetDescription()
	{
		string text;
		if ((text = this.commandDescriptionCache) == null)
		{
			text = (this.commandDescriptionCache = this.getDescription());
		}
		return text;
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0001FF1E File Offset: 0x0001E11E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string getHelp()
	{
		return null;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0005FD10 File Offset: 0x0005DF10
	public virtual string GetHelp()
	{
		string text;
		if ((text = this.commandHelpCache) == null)
		{
			text = (this.commandHelpCache = this.getHelp());
		}
		return text;
	}

	// Token: 0x06000EBD RID: 3773
	public abstract void Execute(List<string> _params, CommandSenderInfo _senderInfo);

	// Token: 0x04000C78 RID: 3192
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] commandNamesCache;

	// Token: 0x04000C79 RID: 3193
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandDescriptionCache;

	// Token: 0x04000C7A RID: 3194
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandHelpCache;

	// Token: 0x04000C7B RID: 3195
	[PublicizedFrom(EAccessModifier.Private)]
	public string primaryCommand;
}
