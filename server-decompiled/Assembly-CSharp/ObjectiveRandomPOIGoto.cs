using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AB RID: 2475
[Preserve]
public class ObjectiveRandomPOIGoto : ObjectiveGoto
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x060048F2 RID: 18674 RVA: 0x001C826C File Offset: 0x001C646C
	// (set) Token: 0x060048F1 RID: 18673 RVA: 0x001C8263 File Offset: 0x001C6463
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

	// Token: 0x060048F3 RID: 18675 RVA: 0x001C158C File Offset: 0x001BF78C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x001C15AE File Offset: 0x001BF7AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x060048F5 RID: 18677 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NeedsNPCSetPosition
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x060048F6 RID: 18678 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x001C3BD4 File Offset: 0x001C1DD4
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return this.GetPosition(ownerNPC, player, usedPOILocations, entityIDforQuests) != Vector3.zero;
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x001C828E File Offset: 0x001C648E
	public override void AddHooks()
	{
		base.AddHooks();
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.POIPosition, this.NavObjectName, -1);
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001C82A9 File Offset: 0x001C64A9
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

	// Token: 0x060048FA RID: 18682 RVA: 0x001C82D2 File Offset: 0x001C64D2
	public override void SetPosition(Vector3 POIPosition, Vector3 POISize)
	{
		this.SetDistanceOffset(POISize);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POIPosition, POIPosition);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.POISize, POISize);
		base.OwnerQuest.Position = POIPosition;
		this.position = POIPosition;
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x001C8308 File Offset: 0x001C6508
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

	// Token: 0x060048FC RID: 18684 RVA: 0x001C870C File Offset: 0x001C690C
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

	// Token: 0x060048FD RID: 18685 RVA: 0x001C8764 File Offset: 0x001C6964
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

	// Token: 0x060048FE RID: 18686 RVA: 0x001C8946 File Offset: 0x001C6B46
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseInt(ObjectiveRandomPOIGoto.PropPOITier, ref this.poiTier);
	}

	// Token: 0x04003A1B RID: 14875
	[PublicizedFrom(EAccessModifier.Private)]
	public int poiTier = -1;

	// Token: 0x04003A1C RID: 14876
	public static string PropPOITier = "poi_tier";
}
