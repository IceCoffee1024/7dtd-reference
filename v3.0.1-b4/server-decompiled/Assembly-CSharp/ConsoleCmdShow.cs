using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000287 RID: 647
[Preserve]
public class ConsoleCmdShow : ConsoleCmdAbstract
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060012FD RID: 4861 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060012FE RID: 4862 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x000752FC File Offset: 0x000734FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "show" };
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0007530C File Offset: 0x0007350C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Shows custom layers of rendering.";
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x00075314 File Offset: 0x00073514
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Enable(ConsoleCmdShow.DebugView dView)
	{
		ConsoleCmdShow.Disable();
		ConsoleCmdShow.enabledKeyword = dView.key;
		Shader.EnableKeyword(ConsoleCmdShow.enabledKeyword);
		ConsoleCmdShow.savedShadowsOption = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowQuality);
		ConsoleCmdShow.savedSSAOOption = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxSSAO);
		ConsoleCmdShow.savedDOFOption = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxDOF);
		if (dView.disableShadows)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxShadowQuality, 0);
		}
		if (dView.disableSSAO)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxSSAO, false);
		}
		if (dView.disableDOF)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxDOF, false);
		}
		GameManager.Instance.ApplyAllOptions();
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x000753AC File Offset: 0x000735AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Disable()
	{
		if (ConsoleCmdShow.enabledKeyword.Length < 1)
		{
			return;
		}
		Shader.DisableKeyword(ConsoleCmdShow.enabledKeyword);
		ConsoleCmdShow.enabledKeyword = "";
		GamePrefs.Set(EnumGamePrefs.OptionsGfxShadowQuality, ConsoleCmdShow.savedShadowsOption);
		GamePrefs.Set(EnumGamePrefs.OptionsGfxSSAO, ConsoleCmdShow.savedSSAOOption);
		GamePrefs.Set(EnumGamePrefs.OptionsGfxDOF, ConsoleCmdShow.savedDOFOption);
		GameManager.Instance.ApplyAllOptions();
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00075412 File Offset: 0x00073612
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsEnabled(string key)
	{
		return ConsoleCmdShow.enabledKeyword.Length >= 1 && ConsoleCmdShow.enabledKeyword.EqualsCaseInsensitive(key);
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x00075430 File Offset: 0x00073630
	public static void Init()
	{
		for (int i = 0; i < ConsoleCmdShow.Commands.Length; i++)
		{
			Shader.DisableKeyword(ConsoleCmdShow.Commands[i].key);
		}
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00075460 File Offset: 0x00073660
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Switch(ConsoleCmdShow.DebugView dView)
	{
		if (ConsoleCmdShow.IsEnabled(dView.key))
		{
			ConsoleCmdShow.Disable();
			return;
		}
		ConsoleCmdShow.Enable(dView);
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0007547C File Offset: 0x0007367C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetDescription());
			return;
		}
		if (_params.Count == 1)
		{
			for (int i = 0; i < ConsoleCmdShow.Commands.Length; i++)
			{
				if (_params[0].EqualsCaseInsensitive(ConsoleCmdShow.Commands[i].cmd))
				{
					ConsoleCmdShow.Switch(ConsoleCmdShow.Commands[i]);
					return;
				}
			}
			if (_params[0].EqualsCaseInsensitive("none") || _params[0].EqualsCaseInsensitive("off"))
			{
				ConsoleCmdShow.Disable();
			}
			return;
		}
		if (_params.Count > 1)
		{
			StringParsers.ParseFloat(_params[1], 0, -1, NumberStyles.Any);
		}
		_params[0].EqualsCaseInsensitive("NA");
	}

	// Token: 0x04000D4D RID: 3405
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool DISABLE_SHADOWS = true;

	// Token: 0x04000D4E RID: 3406
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ENABLE_SHADOWS = false;

	// Token: 0x04000D4F RID: 3407
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool DISABLE_SSAO = true;

	// Token: 0x04000D50 RID: 3408
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ENABLE_SSAO = false;

	// Token: 0x04000D51 RID: 3409
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool DISABLE_DOF = true;

	// Token: 0x04000D52 RID: 3410
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ENABLE_DOF = false;

	// Token: 0x04000D53 RID: 3411
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConsoleCmdShow.DebugView[] Commands = new ConsoleCmdShow.DebugView[]
	{
		new ConsoleCmdShow.DebugView("blockAO", "SHOW_BLOCK_AO", ConsoleCmdShow.DISABLE_SHADOWS, ConsoleCmdShow.DISABLE_SSAO, ConsoleCmdShow.DISABLE_DOF),
		new ConsoleCmdShow.DebugView("occlusion", "SHOW_OCCLUSION", ConsoleCmdShow.DISABLE_SHADOWS, ConsoleCmdShow.DISABLE_SSAO, ConsoleCmdShow.DISABLE_DOF),
		new ConsoleCmdShow.DebugView("lighting", "SHOW_LIGHTING", ConsoleCmdShow.ENABLE_SHADOWS, ConsoleCmdShow.ENABLE_SSAO, ConsoleCmdShow.DISABLE_DOF),
		new ConsoleCmdShow.DebugView("normals", "SHOW_NORMALS", ConsoleCmdShow.DISABLE_SHADOWS, ConsoleCmdShow.DISABLE_SSAO, ConsoleCmdShow.DISABLE_DOF)
	};

	// Token: 0x04000D54 RID: 3412
	[PublicizedFrom(EAccessModifier.Private)]
	public static string enabledKeyword = "";

	// Token: 0x04000D55 RID: 3413
	[PublicizedFrom(EAccessModifier.Private)]
	public static int savedShadowsOption = -1;

	// Token: 0x04000D56 RID: 3414
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool savedSSAOOption = false;

	// Token: 0x04000D57 RID: 3415
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool savedDOFOption = false;

	// Token: 0x02000288 RID: 648
	public class DebugView
	{
		// Token: 0x06001309 RID: 4873 RVA: 0x0007561C File Offset: 0x0007381C
		public DebugView(string _cmd, string _key, bool _disableShadows, bool _disableSSAO, bool _disableDOF)
		{
			this.cmd = _cmd;
			this.key = _key;
			this.disableShadows = _disableShadows;
			this.disableSSAO = _disableSSAO;
			this.disableDOF = _disableDOF;
		}

		// Token: 0x04000D58 RID: 3416
		public string cmd;

		// Token: 0x04000D59 RID: 3417
		public string key;

		// Token: 0x04000D5A RID: 3418
		public bool disableShadows;

		// Token: 0x04000D5B RID: 3419
		public bool disableSSAO;

		// Token: 0x04000D5C RID: 3420
		public bool disableDOF;
	}
}
