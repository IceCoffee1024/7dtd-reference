using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000222 RID: 546
[Preserve]
public class ConsoleCmdGetSandboxOptions : ConsoleCmdAbstract
{
	// Token: 0x06001072 RID: 4210 RVA: 0x00067D18 File Offset: 0x00065F18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getsandboxoptions", "gso" };
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x00067D30 File Offset: 0x00065F30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets the current game's Sandbox Options";
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x00067D37 File Offset: 0x00065F37
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return ("\r\n\t\t\t|Usage:\r\n\t\t\t|  " + this.PrimaryCommand + " [show all]\r\n\t\t\t|Shows the currently loaded game's Sandbox Options.\r\n\t\t\t|By default only shows those that have non-default values, but can show all options by specifying the optional argument with \"true\".\r\n\t\t\t").Unindent(true);
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x00067D54 File Offset: 0x00065F54
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool flag = _params.Count > 0 && StringParsers.ParseBool(_params[0], 0, -1, true);
		ConsoleCmdGetSandboxOptions.LogOptions(GameStats.GetString(EnumGameStats.SandboxCode), flag, ConsoleCmdGetSandboxOptions.ELogType.RemoteConsole);
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00067D8C File Offset: 0x00065F8C
	public static void LogOptions(string _code, bool _showAll, ConsoleCmdGetSandboxOptions.ELogType _logTarget)
	{
		Action<string> action;
		switch (_logTarget)
		{
		case ConsoleCmdGetSandboxOptions.ELogType.LogOnly:
			action = new Action<string>(ConsoleCmdGetSandboxOptions.<LogOptions>g__PrintLog|7_0);
			break;
		case ConsoleCmdGetSandboxOptions.ELogType.Console:
			action = new Action<string>(ConsoleCmdGetSandboxOptions.<LogOptions>g__PrintConsole|7_1);
			break;
		case ConsoleCmdGetSandboxOptions.ELogType.RemoteConsole:
			action = new Action<string>(ConsoleCmdGetSandboxOptions.<LogOptions>g__PrintRemoteConsole|7_2);
			break;
		default:
			throw new ArgumentOutOfRangeException("_logTarget", _logTarget, null);
		}
		Action<string> action2 = action;
		if (_code == null)
		{
			_code = "";
		}
		action2("Sandbox Code: " + _code);
		action2("Sandbox Options:");
		SandboxOptionManager sandboxOptionManager = SandboxOptionManager.Current;
		SandboxOptionPreset sandboxOptionPreset = new SandboxOptionPreset();
		if (!sandboxOptionManager.LoadOptionsFromCode(_code, sandboxOptionPreset))
		{
			action2("  - Invalid code -");
			return;
		}
		foreach (KeyValuePair<string, List<BaseSandboxOption>> keyValuePair in sandboxOptionManager.OptionsByCategory.dict)
		{
			string text;
			List<BaseSandboxOption> list;
			keyValuePair.Deconstruct(out text, out list);
			string text2 = text;
			List<BaseSandboxOption> list2 = list;
			bool flag = false;
			foreach (BaseSandboxOption baseSandboxOption in list2)
			{
				int defaultIndex = baseSandboxOption.GetDefaultIndex();
				int num = defaultIndex;
				if (sandboxOptionPreset.PresetValues.TryGetValue(baseSandboxOption.Option, out num) || _showAll)
				{
					if (!flag)
					{
						flag = true;
						action2("*** " + text2.ToUpper() + " ***");
					}
					string valueTextFromIndex = baseSandboxOption.GetValueTextFromIndex(num, null);
					string valueTextFromIndex2 = baseSandboxOption.GetValueTextFromIndex(defaultIndex, null);
					action2(string.Format("Option {0}: {1}/{2} (default: {3}/{4})", new object[]
					{
						baseSandboxOption.Option.ToStringCached<global::SandboxOptions.SandboxOptions>(),
						num,
						valueTextFromIndex,
						defaultIndex,
						valueTextFromIndex2
					}));
				}
			}
		}
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x00067F90 File Offset: 0x00066190
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <LogOptions>g__PrintLog|7_0(string _line)
	{
		Log.WriteLine(_line);
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x00067F98 File Offset: 0x00066198
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <LogOptions>g__PrintConsole|7_1(string _line)
	{
		Log.Out(_line);
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x00067FA0 File Offset: 0x000661A0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <LogOptions>g__PrintRemoteConsole|7_2(string _line)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(_line);
	}

	// Token: 0x02000223 RID: 547
	public enum ELogType
	{
		// Token: 0x04000CB2 RID: 3250
		LogOnly,
		// Token: 0x04000CB3 RID: 3251
		Console,
		// Token: 0x04000CB4 RID: 3252
		RemoteConsole
	}
}
