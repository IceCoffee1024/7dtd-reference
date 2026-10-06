using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000224 RID: 548
[Preserve]
public class ConsoleCmdGive : ConsoleCmdAbstract
{
	// Token: 0x06001089 RID: 4233 RVA: 0x00069638 File Offset: 0x00067838
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "give an item to a player (entity id or name)";
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0006963F File Offset: 0x0006783F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give an item to a player by dropping it in front of that player\nUsage:\n   give <name / entity id> <item name> <amount>\n   give <name / entity id> <item name> <amount> <quality>\nEither pass the full name of a player or his entity id (given by e.g. \"lpi\").\nItem name has to be the exact name of an item as listed by \"listitems\".\nAmount is the number of instances of this item to drop (as a single stack).\nQuality is the quality of the dropped items for items that have a quality.";
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x00069646 File Offset: 0x00067846
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[]
		{
			"give",
			string.Empty
		};
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x00069660 File Offset: 0x00067860
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 3 && _params.Count != 4)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Wrong number of arguments, expected 3 or 4, found {0}.", _params.Count));
			return;
		}
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[0], true, false);
		if (clientInfo == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity id not found.");
			return;
		}
		ItemValue itemValue = ItemClass.GetItem(_params[1], true);
		if (itemValue.type == ItemValue.None.type)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Item not found.");
			return;
		}
		itemValue = new ItemValue(itemValue.type, true);
		int num;
		if (!int.TryParse(_params[2], out num) || num <= 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Amount is not an integer or not greater than zero.");
			return;
		}
		ushort num2 = 6;
		if (_params.Count == 4 && (!ushort.TryParse(_params[3], out num2) || num2 > 6))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Quality is not an integer or not greater than zero.");
			return;
		}
		if (ItemClass.list[itemValue.type].HasSubItems)
		{
			for (int i = 0; i < itemValue.ModificationCount; i++)
			{
				ItemValue modification = itemValue.GetModification(i);
				modification.Quality = num2;
				itemValue.SetModification(i, modification);
			}
		}
		else if (ItemClass.list[itemValue.type].HasQuality)
		{
			itemValue.Quality = num2;
		}
		EntityPlayer entityPlayer = GameManager.Instance.World.Players.dict[clientInfo.entityId];
		ItemStack itemStack = new ItemStack(itemValue, num);
		GameManager.Instance.ItemDropServer(itemStack, entityPlayer.GetPosition(), Vector3.zero, -1, 60f, false);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Dropped item");
	}
}
