using System;
using System.Collections.Generic;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000221 RID: 545
[Preserve]
public class ConsoleCmdGiveQuest : ConsoleCmdAbstract
{
	// Token: 0x06001090 RID: 4240 RVA: 0x0006922D File Offset: 0x0006742D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "givequest" };
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06001091 RID: 4241 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06001092 RID: 4242 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0006923D File Offset: 0x0006743D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gives a quest to the player or add to quest tier";
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x00069244 File Offset: 0x00067444
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "givequest commands:\ncomplete - complete all quests\ntieradd <points> - add the points to your quest tier (default is 10)\n<quest name> - gives you the quest";
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0006924C File Offset: 0x0006744C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Cannot execute givequest on dedicated server, please execute as a client");
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Available quests:");
			foreach (KeyValuePair<string, QuestClass> keyValuePair in QuestClass.s_Quests)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   " + keyValuePair.Key);
			}
			return;
		}
		QuestJournal questJournal = XUiM_Player.GetPlayer().QuestJournal;
		if (_params[0].EqualsCaseInsensitive("tieradd"))
		{
			int num = 10;
			if (_params.Count >= 2)
			{
				num = StringParsers.ParseSInt32(_params[1], 0, -1, NumberStyles.Integer);
			}
			questJournal.AddQuestFactionPoint(1, num);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Quest points {0}, tier {1}", new object[]
			{
				questJournal.GetQuestFactionPoints(1),
				questJournal.GetCurrentFactionTier(1, 0, false)
			});
			return;
		}
		if (_params[0].EqualsCaseInsensitive("complete"))
		{
			if (questJournal.quests.Count > 0)
			{
				for (int i = 0; i < questJournal.quests.Count; i++)
				{
					Quest quest = questJournal.quests[i];
					if (quest.CurrentState == Quest.QuestState.NotStarted || quest.CurrentState == Quest.QuestState.InProgress)
					{
						for (int j = 0; j < quest.Objectives.Count; j++)
						{
							BaseObjective baseObjective = quest.Objectives[j];
							if (baseObjective.ObjectiveState != BaseObjective.ObjectiveStates.Complete && !(baseObjective is ObjectiveReturnToNPC))
							{
								baseObjective.ChangeStatus(true);
							}
						}
						questJournal.AddPOIToTraderData((int)quest.QuestClass.DifficultyTier, quest.PositionData[Quest.PositionDataTypes.TraderPosition], quest.PositionData[Quest.PositionDataTypes.POIPosition]);
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Quest completed {0}", new object[] { quest.GetPOIName() });
					}
				}
			}
			return;
		}
		if (!QuestClass.s_Quests.ContainsKey(_params[0]))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Quest '{0}' does not exist!", new object[] { _params[0] });
			return;
		}
		string text = _params[0];
		foreach (KeyValuePair<string, QuestClass> keyValuePair2 in QuestClass.s_Quests)
		{
			if (keyValuePair2.Key.EqualsCaseInsensitive(text))
			{
				text = keyValuePair2.Key;
				break;
			}
		}
		Quest quest2 = QuestClass.CreateQuest(text);
		if (quest2 == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Quest '{0}' does not exist!", new object[] { _params[0] });
			return;
		}
		questJournal.AddQuest(quest2, true);
	}
}
