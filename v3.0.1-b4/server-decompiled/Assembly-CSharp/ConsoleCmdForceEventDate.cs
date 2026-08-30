using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000216 RID: 534
[Preserve]
public class ConsoleCmdForceEventDate : ConsoleCmdAbstract
{
	// Token: 0x0600103C RID: 4156 RVA: 0x00066E80 File Offset: 0x00065080
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ForceEventDate" };
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600103D RID: 4157 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x0600103E RID: 4158 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x00066E90 File Offset: 0x00065090
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Specify date for testing event dates";
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x00066E98 File Offset: 0x00065098
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Current forced date: " + ((EventsFromXml.ForceTestDateTime == DateTime.MinValue) ? "-none-" : EventsFromXml.ForceTestDateTime.ToShortDateString()));
			return;
		}
		string text = _params[0];
		DateTime minValue;
		if (text == "now")
		{
			minValue = DateTime.MinValue;
		}
		else if (!EventsFromXml.TryParseDate(text, out minValue))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed parsing date argument, must be in the form 'mm/dd'");
			return;
		}
		EventsFromXml.ForceTestDateTime = minValue;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Forced date: " + minValue.ToShortDateString());
		foreach (KeyValuePair<string, EventsFromXml.EventDefinition> keyValuePair in EventsFromXml.Events)
		{
			string text2;
			EventsFromXml.EventDefinition eventDefinition;
			keyValuePair.Deconstruct(out text2, out eventDefinition);
			EventsFromXml.EventDefinition eventDefinition2 = eventDefinition;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Name={0}, Start={1}, End={2}, Active={3}", new object[] { eventDefinition2.Name, eventDefinition2.Start, eventDefinition2.End, eventDefinition2.Active }));
		}
	}
}
