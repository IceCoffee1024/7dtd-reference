using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021B RID: 539
[Preserve]
public class ConsoleCmdForceEventDate : ConsoleCmdAbstract
{
	// Token: 0x0600103E RID: 4158 RVA: 0x00067588 File Offset: 0x00065788
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ForceEventDate" };
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600103F RID: 4159 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06001040 RID: 4160 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x00067598 File Offset: 0x00065798
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Specify date for testing event dates";
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x000675A0 File Offset: 0x000657A0
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
