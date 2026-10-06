using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x02000214 RID: 532
[Preserve]
public class ConsoleCmdExportCurrentConfigs : ConsoleCmdAbstract
{
	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x00066D5E File Offset: 0x00064F5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Exports the current game config XMLs";
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x00066D65 File Offset: 0x00064F65
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Exports all game config XMLs as they are currently used (including applied\npatches from mods) to the folder \"Configs\" in the save folder of the game.\nIf run from the main menu it exports the XUi configs for the menu, if run\nfrom a game session will export all others.";
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x00066D6C File Offset: 0x00064F6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "exportcurrentconfigs" };
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x00066D7C File Offset: 0x00064F7C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No game started, exporting XUi menu and rwgmixer configs");
			string text = Path.Combine(GameIO.GetApplicationTempPath(), "ExportedConfigs");
			if (SdDirectory.Exists(text))
			{
				SdDirectory.Delete(text, true);
			}
			Thread.Sleep(50);
			SdDirectory.CreateDirectory(text);
			string[] array = new string[] { "rwgmixer", "loadingscreen", "XUi_Menu/styles", "XUi_Menu/templates", "XUi_Menu/windows", "XUi_Menu/xui" };
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				XmlFile xml = null;
				ThreadManager.RunCoroutineSync(XmlPatcher.LoadAndPatchConfig(text2, delegate(XmlFile _file)
				{
					xml = _file;
				}));
				string text3 = text + "/" + text2 + ".xml";
				if (text2.IndexOf('/') >= 0)
				{
					string directoryName = Path.GetDirectoryName(text3);
					if (!SdDirectory.Exists(directoryName))
					{
						SdDirectory.CreateDirectory(directoryName);
					}
				}
				xml.SerializeToFile(text3, false, null);
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Configs exports to " + text);
			GameIO.OpenExplorer(text);
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
			ConsoleCmdExportCurrentConfigs.DumpLoadedXmls(Path.Combine(GameIO.GetApplicationTempPath(), "SavesLocal", @string, "ConfigsDump"));
			return;
		}
		string text4 = Path.Combine(GameIO.GetSaveGameDir(), "ConfigsDump");
		if (!GameIO.IsRoamingUserDataPath(text4))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Patched XMLs are automatically dumped on game start to a ConfigsDump subdirectory of the save game.");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("In this case you can find the folder at: " + text4);
			GameIO.OpenExplorer(text4);
			return;
		}
		string string2 = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		string string3 = GamePrefs.GetString(EnumGamePrefs.GameName);
		ConsoleCmdExportCurrentConfigs.DumpLoadedXmls(Path.Combine(GameIO.GetApplicationTempPath(), string2, string3, "ConfigsDump"));
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x00066F4D File Offset: 0x0006514D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DumpLoadedXmls(string tempPath)
	{
		if (SdDirectory.Exists(tempPath))
		{
			SdDirectory.Delete(tempPath, true);
		}
		SdDirectory.CreateDirectory(tempPath);
		WorldStaticData.SaveXmlsToFolder(tempPath);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Configs exports to " + tempPath);
		GameIO.OpenExplorer(tempPath);
	}
}
