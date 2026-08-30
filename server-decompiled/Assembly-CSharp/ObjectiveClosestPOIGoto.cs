using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098E RID: 2446
[Preserve]
public class ObjectiveClosestPOIGoto : ObjectiveGoto
{
	// Token: 0x060047B3 RID: 18355 RVA: 0x001C158C File Offset: 0x001BF78C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x001C15A0 File Offset: 0x001BF7A0
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x001C15AE File Offset: 0x001BF7AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x001C15BB File Offset: 0x001BF7BB
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = 1)
	{
		return this.GetPosition(ownerNPC, player, usedPOILocations, -1) != Vector3.zero;
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x001C15D1 File Offset: 0x001BF7D1
	public override void SetPosition(Vector3 position, Vector3 size)
	{
		base.FinalizePoint(position, size);
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x001C15DC File Offset: 0x001BF7DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetPosition(EntityNPC ownerNPC = null, EntityPlayer entityPlayer = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		int num = ((ownerNPC == null) ? (-1) : ownerNPC.entityId);
		int num2 = ((entityPlayer == null) ? (-1) : entityPlayer.entityId);
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
		{
			base.OwnerQuest.Position = this.position;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.POIPosition, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		EntityAlive entityAlive = ((ownerNPC == null) ? base.OwnerQuest.OwnerJournal.OwnerPlayer : ownerNPC);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			string text = "";
			if (base.OwnerQuest != null && base.OwnerQuest.QuestClass != null)
			{
				text = base.OwnerQuest.QuestClass.UniqueKey;
			}
			PrefabInstance closestPOIToWorldPos = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetClosestPOIToWorldPos(base.OwnerQuest.QuestTags, new Vector2(entityAlive.position.x, entityAlive.position.z), null, -1, false, BiomeFilterTypes.SameBiome, "", text);
			if (closestPOIToWorldPos == null)
			{
				return Vector3.zero;
			}
			Vector2 vector = new Vector2((float)closestPOIToWorldPos.boundingBoxPosition.x + (float)closestPOIToWorldPos.boundingBoxSize.x / 2f, (float)closestPOIToWorldPos.boundingBoxPosition.z + (float)closestPOIToWorldPos.boundingBoxSize.z / 2f);
			if (vector.x == -0.1f && vector.y == -0.1f)
			{
				Log.Error("ObjectiveClosestPOIGoto: No POI found.");
				return Vector3.zero;
			}
			int num3 = (int)vector.x;
			int num4 = (int)entityAlive.position.y;
			int num5 = (int)vector.y;
			if (GameManager.Instance.World.IsPositionInBounds(this.position))
			{
				base.FinalizePoint(new Vector3((float)closestPOIToWorldPos.boundingBoxPosition.x, (float)closestPOIToWorldPos.boundingBoxPosition.y, (float)closestPOIToWorldPos.boundingBoxPosition.z), new Vector3((float)closestPOIToWorldPos.boundingBoxSize.x, (float)closestPOIToWorldPos.boundingBoxSize.y, (float)closestPOIToWorldPos.boundingBoxSize.z));
				this.position = new Vector3((float)num3, (float)num4, (float)num5);
				base.OwnerQuest.Position = this.position;
				return this.position;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestGotoPoint>().Setup(num, num2, base.OwnerQuest.QuestTags, base.OwnerQuest.QuestCode, NetPackageQuestGotoPoint.QuestGotoTypes.Closest, base.OwnerQuest.QuestClass.DifficultyTier, 0, -1, 0f, 0f, 0f, -1f, BiomeFilterTypes.SameBiome, ""), false);
			base.CurrentValue = 1;
		}
		return Vector3.zero;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x001C18AF File Offset: 0x001BFAAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_WaitingForServer()
	{
		if (this.positionSet)
		{
			base.CurrentValue = 2;
		}
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x001C18C0 File Offset: 0x001BFAC0
	public override BaseObjective Clone()
	{
		ObjectiveClosestPOIGoto objectiveClosestPOIGoto = new ObjectiveClosestPOIGoto();
		this.CopyValues(objectiveClosestPOIGoto);
		return objectiveClosestPOIGoto;
	}
}
