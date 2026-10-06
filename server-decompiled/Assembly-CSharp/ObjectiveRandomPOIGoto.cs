using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009B1 RID: 2481
[Preserve]
public class ObjectiveRandomPOIGoto : ObjectiveGoto
{
	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x060048AB RID: 18603 RVA: 0x001C8BFC File Offset: 0x001C6DFC
	// (set) Token: 0x060048AA RID: 18602 RVA: 0x001C8BF3 File Offset: 0x001C6DF3
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

	// Token: 0x060048AC RID: 18604 RVA: 0x001C1C1F File Offset: 0x001BFE1F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
	}

	// Token: 0x060048AD RID: 18605 RVA: 0x001C1C41 File Offset: 0x001BFE41
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x060048AE RID: 18606 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NeedsNPCSetPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x060048AF RID: 18607 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x001C4404 File Offset: 0x001C2604
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return this.GetPosition(ownerNPC, player, usedPOILocations, entityIDforQuests) != Vector3.zero;
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x001C8C1E File Offset: 0x001C6E1E
	public override void AddHooks()
	{
		base.AddHooks();
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.POIPosition, this.NavObjectName, -1);
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x001C8C39 File Offset: 0x001C6E39
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

	// Token: 0x060048B3 RID: 18611 RVA: 0x001C8C62 File Offset: 0x001C6E62
	public override void SetPosition(Vector3 POIPosition, Vector3 POISize)
	{
		this.SetDistanceOffset(POISize);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POIPosition, POIPosition);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POISize, POISize);
		base.OwnerQuest.Position = POIPosition;
		this.position = POIPosition;
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x001C8C98 File Offset: 0x001C6E98
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

	// Token: 0x060048B5 RID: 18613 RVA: 0x001C909C File Offset: 0x001C729C
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

	// Token: 0x060048B6 RID: 18614 RVA: 0x001C90F4 File Offset: 0x001C72F4
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

	// Token: 0x060048B7 RID: 18615 RVA: 0x001C92D6 File Offset: 0x001C74D6
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseInt(ObjectiveRandomPOIGoto.PropPOITier, ref this.poiTier);
	}

	// Token: 0x04003A18 RID: 14872
	[PublicizedFrom(EAccessModifier.Private)]
	public int poiTier = -1;

	// Token: 0x04003A19 RID: 14873
	public static string PropPOITier = "poi_tier";
}
