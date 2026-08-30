using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000999 RID: 2457
[Preserve]
public class ObjectiveRandomGotoNPC : ObjectiveRandomGoto
{
	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06004866 RID: 18534 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NeedsNPCSetPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004867 RID: 18535 RVA: 0x001C57B1 File Offset: 0x001C39B1
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return this.GetPosition(ownerNPC) != Vector3.zero;
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x001C57C4 File Offset: 0x001C39C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 GetPosition(EntityNPC ownerNPC)
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.Location))
		{
			base.OwnerQuest.Position = this.position;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.Location, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.TreasurePoint))
		{
			this.positionSet = true;
			base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.Location, base.OwnerQuest.Position);
			base.CurrentValue = 2;
			return this.position;
		}
		float num = 50f;
		if (this.Value != null && this.Value != "")
		{
			if (!StringParsers.TryParseFloat(this.Value, out num))
			{
				if (this.Value.Contains("-"))
				{
					string[] array = this.Value.Split('-', StringSplitOptions.None);
					float num2 = StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any);
					float num3 = StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any);
					num = GameManager.Instance.World.GetGameRandom().RandomFloat * (num3 - num2) + num2;
				}
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Vector3i vector3i = ObjectiveRandomGoto.CalculateRandomPoint(ownerNPC.entityId, num, base.OwnerQuest.ID, false, this.biomeFilterType, this.biomeFilter);
			if (!GameManager.Instance.World.CheckForLevelNearbyHeights((float)vector3i.x, (float)vector3i.z, 5) || GameManager.Instance.World.GetWaterAt((float)vector3i.x, (float)vector3i.z))
			{
				return Vector3.zero;
			}
			World world = GameManager.Instance.World;
			if (vector3i.y > 0 && world.IsPositionInBounds(vector3i) && !world.IsPositionWithinPOI(vector3i, 5))
			{
				base.FinalizePoint(vector3i.x, vector3i.y, vector3i.z);
				return this.position;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(ownerNPC.entityId, num, 1, base.OwnerQuest.QuestCode, 0, -1, 0, false), false);
			base.CurrentValue = 1;
		}
		return Vector3.zero;
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x001C5A10 File Offset: 0x001C3C10
	public override BaseObjective Clone()
	{
		ObjectiveRandomGotoNPC objectiveRandomGotoNPC = new ObjectiveRandomGotoNPC();
		this.CopyValues(objectiveRandomGotoNPC);
		objectiveRandomGotoNPC.position = this.position;
		objectiveRandomGotoNPC.positionSet = this.positionSet;
		objectiveRandomGotoNPC.completionDistance = this.completionDistance;
		objectiveRandomGotoNPC.biomeFilter = this.biomeFilter;
		objectiveRandomGotoNPC.biomeFilterType = this.biomeFilterType;
		return objectiveRandomGotoNPC;
	}
}
