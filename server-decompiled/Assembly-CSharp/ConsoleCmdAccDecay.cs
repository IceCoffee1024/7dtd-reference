using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E1 RID: 481
[Preserve]
public class ConsoleCmdAccDecay : ConsoleCmdAbstract
{
	// Token: 0x06000EC0 RID: 3776 RVA: 0x00060532 File Offset: 0x0005E732
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "AccDecay", "SetAccDecay", "SetAccuracyDecay", "sad" };
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0006055A File Offset: 0x0005E75A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Accuracy Decay for guns, show/hide/reset/<Decimal value>";
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00060561 File Offset: 0x0005E761
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "reset - apply the default settings for this command\n<Decimal Value> - Sets the decay constant for accuracy calculations using ItemActionRanged";
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x00060568 File Offset: 0x0005E768
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("ItemActionRanged.DecayConstant: {0}", ItemActionRanged.AccuracyUpdateDecayConstant));
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("ItemActionRanged.LogOldAccuracy: {0}", ItemActionRanged.LogOldAccuracy));
			return;
		}
		float num;
		if (float.TryParse(_params[0], out num))
		{
			ItemActionRanged.AccuracyUpdateDecayConstant = num;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Set ItemActionRanged.DecayConstant to {0} and reset old accuracy for checking", num));
			return;
		}
		if (_params[0].ToLower() == "reset")
		{
			ItemActionRanged.LogOldAccuracy = false;
			ItemActionRanged.AccuracyUpdateDecayConstant = 9.1f;
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("ItemActionRanged.DecayConstant: {0}", ItemActionRanged.AccuracyUpdateDecayConstant));
	}
}
