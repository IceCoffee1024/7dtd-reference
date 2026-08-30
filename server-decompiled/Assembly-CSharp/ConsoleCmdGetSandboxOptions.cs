using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x0200021D RID: 541
[Preserve]
public class ConsoleCmdGetSandboxOptions : ConsoleCmdAbstract
{
	// Token: 0x06001070 RID: 4208 RVA: 0x00067600 File Offset: 0x00065800
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getsandboxoptions", "gso" };
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x00067618 File Offset: 0x00065818
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets the current game's Sandbox Options";
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0006761F File Offset: 0x0006581F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return ("\n\t\t\t|Usage:\n\t\t\t|  " + this.PrimaryCommand + " [show all]\n\t\t\t|Shows the currently loaded game's Sandbox Options.\n\t\t\t|By default only shows those that have non-default values, but can show all options by specifying the optional argument with \"true\".\n\t\t\t").Unindent(true);
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x0006763C File Offset: 0x0006583C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool flag = _params.Count > 0 && StringParsers.ParseBool(_params[0], 0, -1, true);
		ConsoleCmdGetSandboxOptions.LogOptions(GameStats.GetString(EnumGameStats.SandboxCode), flag, true);
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x00067674 File Offset: 0x00065874
	public static void LogOptions(string _code, bool _showAll, bool _logToConsole)
	{
		Action<string> action = (_logToConsole ? new Action<string>(ConsoleCmdGetSandboxOptions.<LogOptions>g__PrintConsole|6_1) : new Action<string>(ConsoleCmdGetSandboxOptions.<LogOptions>g__PrintLog|6_0));
		if (_code == null)
		{
			_code = "";
		}
		action("Sandbox Code: " + _code);
		action("Sandbox Options:");
		SandboxOptionManager sandboxOptionManager = SandboxOptionManager.Current;
		SandboxOptionPreset sandboxOptionPreset = new SandboxOptionPreset();
		if (!sandboxOptionManager.LoadOptionsFromCode(_code, sandboxOptionPreset))
		{
			action("  - Invalid code -");
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
				if (_showAll || sandboxOptionPreset.PresetValues.TryGetValue(baseSandboxOption.Option, out num))
				{
					if (!flag)
					{
						flag = true;
						action("*** " + text2.ToUpper() + " ***");
					}
					string valueTextFromIndex = baseSandboxOption.GetValueTextFromIndex(num);
					string valueTextFromIndex2 = baseSandboxOption.GetValueTextFromIndex(defaultIndex);
					action(string.Format("Option {0}: {1}/{2} (default: {3}/{4})", new object[]
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

	// Token: 0x06001077 RID: 4215 RVA: 0x00067840 File Offset: 0x00065A40
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <LogOptions>g__PrintLog|6_0(string _line)
	{
		Log.WriteLine(_line);
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x00067848 File Offset: 0x00065A48
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <LogOptions>g__PrintConsole|6_1(string _line)
	{
		Log.Out(_line);
	}
}
