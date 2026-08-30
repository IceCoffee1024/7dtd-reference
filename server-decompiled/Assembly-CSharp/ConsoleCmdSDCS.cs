using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200027A RID: 634
[Preserve]
public class ConsoleCmdSDCS : ConsoleCmdAbstract
{
	// Token: 0x060012C2 RID: 4802 RVA: 0x000747EF File Offset: 0x000729EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Change a player's sex, race, and variant:\n  Usage:\n    sdcs                            : Show current archetype values\n    sdcs <sex|race|variant> <value> : Set the specified value\n  Examples :\n    sdcs sex <male|female>\n    sdcs race <white|black|asian|native>\n    sdcs variant <1|2|3|4>";
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x000747F6 File Offset: 0x000729F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sdcs" };
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x00074806 File Offset: 0x00072A06
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Control entity sex, race, and variant";
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x00074810 File Offset: 0x00072A10
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World.GetLocalPlayers().Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No local players found");
			return;
		}
		EModelSDCS component = GameManager.Instance.World.GetLocalPlayers()[0].GetComponent<EModelSDCS>();
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
			{
				"Current Archetype Values:\n  Sex: ",
				component.Archetype.Sex,
				"\n  Race: ",
				component.Archetype.Race,
				"\n  Variant: ",
				component.Archetype.Variant.ToString()
			}));
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		ConsoleCmdSDCS.cTypes cTypes;
		if (!Enum.TryParse<ConsoleCmdSDCS.cTypes>(_params[0], true, out cTypes))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid control type");
			return;
		}
		string text = _params[1];
		if (component == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No SDCS model found");
			return;
		}
		switch (cTypes)
		{
		case ConsoleCmdSDCS.cTypes.Sex:
		{
			ConsoleCmdSDCS.sTypes sTypes;
			if (!Enum.TryParse<ConsoleCmdSDCS.sTypes>(_params[1], true, out sTypes))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sex '" + _params[1] + "'");
				return;
			}
			bool flag = false;
			if (sTypes == ConsoleCmdSDCS.sTypes.Male)
			{
				flag = true;
			}
			component.SetSex(flag);
			return;
		}
		case ConsoleCmdSDCS.cTypes.Race:
		{
			ConsoleCmdSDCS.rTypes rTypes;
			if (!Enum.TryParse<ConsoleCmdSDCS.rTypes>(_params[1], true, out rTypes))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid race '" + _params[1] + "'");
				return;
			}
			component.SetRace(rTypes.ToString());
			return;
		}
		case ConsoleCmdSDCS.cTypes.Variant:
		{
			int num;
			if (!StringParsers.TryParseSInt32(_params[1], out num))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid variant number " + _params[1]);
				return;
			}
			if (num < 1 || num > 4)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Invalid variant number {0}", num));
				return;
			}
			component.SetVariant(num);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x0200027B RID: 635
	[PublicizedFrom(EAccessModifier.Private)]
	public enum cTypes
	{
		// Token: 0x04000D42 RID: 3394
		Sex,
		// Token: 0x04000D43 RID: 3395
		Race,
		// Token: 0x04000D44 RID: 3396
		Variant
	}

	// Token: 0x0200027C RID: 636
	[PublicizedFrom(EAccessModifier.Private)]
	public enum rTypes
	{
		// Token: 0x04000D46 RID: 3398
		White,
		// Token: 0x04000D47 RID: 3399
		Black,
		// Token: 0x04000D48 RID: 3400
		Asian,
		// Token: 0x04000D49 RID: 3401
		Native
	}

	// Token: 0x0200027D RID: 637
	[PublicizedFrom(EAccessModifier.Private)]
	public enum sTypes
	{
		// Token: 0x04000D4B RID: 3403
		Male,
		// Token: 0x04000D4C RID: 3404
		Female
	}
}
