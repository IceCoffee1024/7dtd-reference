using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009BB RID: 2491
[Preserve]
public class ObjectiveRandomPOIGoto : ObjectiveGoto
{
	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x060048CE RID: 18638 RVA: 0x001C9CA8 File Offset: 0x001C7EA8
	// (set) Token: 0x060048CD RID: 18637 RVA: 0x001C9C9F File Offset: 0x001C7E9F
	public int POITier
	{
		get
		{
			if (this.poiTier != -1)
			{
				return this.poiTier;
			}
			return (int)base.OwnerQuest.QuestClass.DifficultyTier;
		}
		set
		{
			this.poiTier = value;
		}
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x001C2CA3 File Offset: 0x001C0EA3
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x001C2CC5 File Offset: 0x001C0EC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x060048D1 RID: 18641 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NeedsNPCSetPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x060048D2 RID: 18642 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x001C5488 File Offset: 0x001C3688
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return this.GetPosition(ownerNPC, player, usedPOILocations, entityIDforQuests) != Vector3.zero;
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x001C9CCA File Offset: 0x001C7ECA
	public override void AddHooks()
	{
		base.AddHooks();
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.POIPosition, this.NavObjectName, -1);
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x001C9CE5 File Offset: 0x001C7EE5
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDistanceOffset(Vector3 POISize)
	{
		if (POISize.x > POISize.z)
		{
			this.distanceOffset = POISize.x;
			return;
		}
		this.distanceOffset = POISize.z;
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x001C9D0E File Offset: 0x001C7F0E
	public override void SetPosition(Vector3 POIPosition, Vector3 POISize)
	{
		this.SetDistanceOffset(POISize);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POIPosition, POIPosition);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POISize, POISize);
		base.OwnerQuest.Position = POIPosition;
		this.position = POIPosition;
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x001C9D44 File Offset: 0x001C7F44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetPosition(EntityNPC ownerNPC = null, EntityPlayer entityPlayer = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		int num = ((ownerNPC == null) ? (-1) : ownerNPC.entityId);
		int num2 = ((entityPlayer == null) ? (-1) : entityPlayer.entityId);
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
		{
			Vector3 vector;
			base.OwnerQuest.GetPositionData(out vector, Quest.PositionDataTypes.POISize);
			Vector2 vector2 = new Vector2(this.position.x + vector.x / 2f, this.position.z + vector.z / 2f);
			int num3 = (int)vector2.x;
			int num4 = (int)vector2.y;
			int num5 = (int)GameManager.Instance.World.GetHeightAt(vector2.x, vector2.y);
			this.position = new Vector3((float)num3, (float)num5, (float)num4);
			base.OwnerQuest.Position = this.position;
			this.SetDistanceOffset(vector);
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.POIPosition, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		EntityAlive entityAlive = entityPlayer;
		if (entityAlive == null)
		{
			entityAlive = ((ownerNPC == null) ? base.OwnerQuest.OwnerJournal.OwnerPlayer : ownerNPC);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			PrefabInstance prefabInstance;
			if (ownerNPC != null)
			{
				prefabInstance = DynamicPrefabDecorator.GetRandomPOINearTrader(ownerNPC as EntityTrader, base.OwnerQuest.QuestTags, (byte)this.POITier, usedPOILocations, entityIDforQuests, this.biomeFilterType, this.biomeFilter);
			}
			else
			{
				prefabInstance = DynamicPrefabDecorator.GetRandomPOINearWorldPos(new Vector2(entityAlive.position.x, entityAlive.position.z), 1000, 4000000, base.OwnerQuest.QuestTags, (byte)this.POITier, usedPOILocations, entityIDforQuests, this.biomeFilterType, this.biomeFilter);
			}
			if (prefabInstance == null)
			{
				return Vector3.zero;
			}
			if (prefabInstance != null)
			{
				Vector2 vector3 = new Vector2((float)prefabInstance.boundingBoxPosition.x + (float)prefabInstance.boundingBoxSize.x / 2f, (float)prefabInstance.boundingBoxPosition.z + (float)prefabInstance.boundingBoxSize.z / 2f);
				if (vector3.x == -0.1f && vector3.y == -0.1f)
				{
					Log.Error("ObjectiveRandomGoto: No POI found.");
					return Vector3.zero;
				}
				int num6 = (int)vector3.x;
				int num7 = (int)GameManager.Instance.World.GetHeightAt(vector3.x, vector3.y);
				int num8 = (int)vector3.y;
				this.position = new Vector3((float)num6, (float)num7, (float)num8);
				if (GameManager.Instance.World.IsPositionInBounds(this.position))
				{
					base.OwnerQuest.Position = this.position;
					base.FinalizePoint(new Vector3((float)prefabInstance.boundingBoxPosition.x, (float)prefabInstance.boundingBoxPosition.y, (float)prefabInstance.boundingBoxPosition.z), new Vector3((float)prefabInstance.boundingBoxSize.x, (float)prefabInstance.boundingBoxSize.y, (float)prefabInstance.boundingBoxSize.z));
					base.OwnerQuest.QuestPrefab = prefabInstance;
					base.OwnerQuest.DataVariables.Add("POIName", Localization.Get(base.OwnerQuest.QuestPrefab.location.Name, false, null));
					base.OwnerQuest.DataVariables.Add("POINameKey", base.OwnerQuest.QuestPrefab.location.Name);
					if (usedPOILocations != null)
					{
						usedPOILocations.Add(new Vector2((float)prefabInstance.boundingBoxPosition.x, (float)prefabInstance.boundingBoxPosition.z));
					}
					return this.position;
				}
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestGotoPoint>().Setup(num, num2, base.OwnerQuest.QuestTags, base.OwnerQuest.QuestCode, NetPackageQuestGotoPoint.QuestGotoTypes.RandomPOI, (byte)this.POITier, 0, -1, 0f, 0f, 0f, -1f, this.biomeFilterType, this.biomeFilter), false);
			base.CurrentValue = 1;
		}
		return Vector3.zero;
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x001CA170 File Offset: 0x001C8370
	public override BaseObjective Clone()
	{
		ObjectiveRandomPOIGoto objectiveRandomPOIGoto = new ObjectiveRandomPOIGoto();
		this.CopyValues(objectiveRandomPOIGoto);
		objectiveRandomPOIGoto.poiTier = this.poiTier;
		objectiveRandomPOIGoto.distance = this.distance;
		objectiveRandomPOIGoto.biomeFilterType = this.biomeFilterType;
		objectiveRandomPOIGoto.biomeFilter = this.biomeFilter;
		objectiveRandomPOIGoto.locationName = this.locationName;
		return objectiveRandomPOIGoto;
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x001CA1C8 File Offset: 0x001C83C8
	public override string ParseBinding(string bindingName)
	{
		string id = this.ID;
		string value = this.Value;
		if (!(bindingName == "name"))
		{
			if (!(bindingName == "distance"))
			{
				if (bindingName == "direction")
				{
					if (base.OwnerQuest.QuestGiverID != -1)
					{
						EntityNPC entityNPC = GameManager.Instance.World.GetEntity(base.OwnerQuest.QuestGiverID) as EntityNPC;
						if (entityNPC != null)
						{
							this.position.y = 0f;
							Vector3 position = entityNPC.position;
							position.y = 0f;
							return ValueDisplayFormatters.Direction(GameUtils.GetDirByNormal(new Vector2(this.position.x - position.x, this.position.z - position.z)), false);
						}
					}
				}
			}
			else if (base.OwnerQuest.QuestGiverID != -1)
			{
				EntityNPC entityNPC2 = GameManager.Instance.World.GetEntity(base.OwnerQuest.QuestGiverID) as EntityNPC;
				if (entityNPC2 != null)
				{
					Vector3 position2 = entityNPC2.position;
					this.currentDistance = Vector3.Distance(position2, this.position);
					return ValueDisplayFormatters.Distance(this.currentDistance);
				}
			}
			return "";
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (base.OwnerQuest.DataVariables.ContainsKey("POIName"))
			{
				return base.OwnerQuest.DataVariables["POIName"];
			}
			if (base.OwnerQuest.QuestPrefab == null)
			{
				return "";
			}
			return Localization.Get(base.OwnerQuest.QuestPrefab.location.Name, false, null);
		}
		else
		{
			if (!base.OwnerQuest.DataVariables.ContainsKey("POIName"))
			{
				return "";
			}
			return base.OwnerQuest.DataVariables["POIName"];
		}
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x001CA3AA File Offset: 0x001C85AA
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseInt(ObjectiveRandomPOIGoto.PropPOITier, ref this.poiTier);
	}

	// Token: 0x04003A38 RID: 14904
	[PublicizedFrom(EAccessModifier.Private)]
	public int poiTier = -1;

	// Token: 0x04003A39 RID: 14905
	public static string PropPOITier = "poi_tier";
}
