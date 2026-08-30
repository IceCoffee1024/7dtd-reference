using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000971 RID: 2417
[Preserve]
public class QuestActionTreasureChest : BaseQuestAction
{
	// Token: 0x060046C5 RID: 18117 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x001BED58 File Offset: 0x001BCF58
	public override void PerformAction(Quest ownerQuest)
	{
		World world = GameManager.Instance.World;
		EntityPlayer ownerPlayer = ownerQuest.OwnerJournal.OwnerPlayer;
		float num = ((this.Value == "" || this.Value == null) ? 50f : StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any));
		GameRandom gameRandom = world.GetGameRandom();
		Vector3 vector = new Vector3(-1f + 2f * gameRandom.RandomFloat, 0f, -1f + 2f * gameRandom.RandomFloat);
		vector.Normalize();
		Vector3 vector2 = ownerPlayer.position + vector * num;
		int num2 = (int)vector2.x;
		int num3 = (int)vector2.z;
		int num4 = (int)(world.GetHeight(num2, num3) - 3);
		BlockValue blockValue = new BlockValue
		{
			type = 372
		};
		Vector3i vector3i = new Vector3i(num2, num4, num3);
		world.SetBlockRPC(vector3i, blockValue, sbyte.MaxValue);
		ownerQuest.DataVariables.Add("treasurecontainer", string.Format("{0},{1},{2}", num2, num4, num3));
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x001BEE84 File Offset: 0x001BD084
	public override BaseQuestAction Clone()
	{
		QuestActionTreasureChest questActionTreasureChest = new QuestActionTreasureChest();
		base.CopyValues(questActionTreasureChest);
		return questActionTreasureChest;
	}
}
