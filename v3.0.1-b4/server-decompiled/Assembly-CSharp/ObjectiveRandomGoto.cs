using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000997 RID: 2455
[Preserve]
public class ObjectiveRandomGoto : BaseObjective
{
	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x06004852 RID: 18514 RVA: 0x001C0119 File Offset: 0x001BE319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			if (base.CurrentValue != 3)
			{
				return BaseObjective.ObjectiveValueTypes.Distance;
			}
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x001C4DCE File Offset: 0x001C2FCE
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
		this.SetupIcon();
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06004854 RID: 18516 RVA: 0x001BE8E8 File Offset: 0x001BCAE8
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x001BE8F6 File Offset: 0x001BCAF6
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		this.StatusText = "";
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06004856 RID: 18518 RVA: 0x001C4DE8 File Offset: 0x001C2FE8
	public override string StatusText
	{
		get
		{
			if (base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
			{
				if (!this.positionSet)
				{
					return "--";
				}
				return ValueDisplayFormatters.Distance(this.distance);
			}
			else
			{
				if (base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
				{
					return "";
				}
				if (base.ObjectiveState == BaseObjective.ObjectiveStates.Failed)
				{
					return Localization.Get("failed", false, null);
				}
				return Localization.Get("completed", false, null);
			}
		}
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x001C4E52 File Offset: 0x001C3052
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.Location, this.NavObjectName, -1);
	}

	// Token: 0x06004858 RID: 18520 RVA: 0x001C4E72 File Offset: 0x001C3072
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetupIcon()
	{
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x001C4E80 File Offset: 0x001C3080
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetPosition()
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
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.Location, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
		float num = 50f;
		float num2 = 50f;
		if (this.Value != null && this.Value != "")
		{
			if (StringParsers.TryParseFloat(this.Value, out this.distance))
			{
				num2 = (num = this.distance);
			}
			else if (this.Value.Contains("-"))
			{
				string[] array = this.Value.Split('-', StringSplitOptions.None);
				num = StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any);
				num2 = StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any);
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.distance = GameManager.Instance.World.GetGameRandom().RandomFloat * (num2 - num) + num;
			Vector3i vector3i = ObjectiveRandomGoto.CalculateRandomPoint(ownerPlayer.entityId, this.distance, base.OwnerQuest.ID, false, this.biomeFilterType, this.biomeFilter);
			if (!GameManager.Instance.World.CheckForLevelNearbyHeights((float)vector3i.x, (float)vector3i.z, 5) || GameManager.Instance.World.GetWaterAt((float)vector3i.x, (float)vector3i.z))
			{
				return Vector3.zero;
			}
			World world = GameManager.Instance.World;
			if (vector3i.y > 0 && world.IsPositionInBounds(vector3i) && !world.IsPositionWithinPOI(vector3i, 5))
			{
				this.FinalizePoint(vector3i.x, vector3i.y, vector3i.z);
				return this.position;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(ownerPlayer.entityId, this.distance, 1, base.OwnerQuest.QuestCode, 0, -1, 0, false), false);
			base.CurrentValue = 1;
		}
		return Vector3.zero;
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x001C50F0 File Offset: 0x001C32F0
	public static Vector3i CalculateRandomPoint(int entityID, float distance, string questID, bool canBeWithinPOI = false, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "")
	{
		World world = GameManager.Instance.World;
		EntityAlive entityAlive = world.GetEntity(entityID) as EntityAlive;
		Vector3 vector = new Vector3(world.GetGameRandom().RandomFloat * 2f + -1f, 0f, world.GetGameRandom().RandomFloat * 2f + -1f);
		vector.Normalize();
		Vector3 vector2 = entityAlive.position + vector * distance;
		int num = (int)vector2.x;
		int num2 = (int)vector2.z;
		int num3 = (int)world.GetHeightAt(vector2.x, vector2.z);
		Vector3i vector3i = new Vector3i(num, num3, num2);
		Vector3 vector3 = new Vector3((float)vector3i.x, (float)vector3i.y, (float)vector3i.z);
		if (!world.IsPositionInBounds(vector3) || (entityAlive is EntityPlayer && !world.CanPlaceBlockAt(vector3i, GameManager.Instance.GetPersistentLocalPlayer(), false)) || (!canBeWithinPOI && world.IsPositionWithinPOI(vector3, 2)))
		{
			return new Vector3i(0, -99999, 0);
		}
		if (!world.CheckForLevelNearbyHeights(vector2.x, vector2.z, 5) || world.GetWaterAt(vector2.x, vector2.z))
		{
			return new Vector3i(0, -99999, 0);
		}
		if (biomeFilterType != BiomeFilterTypes.AnyBiome)
		{
			string[] array = null;
			BiomeDefinition biomeAt = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)vector2.x, (int)vector2.z);
			if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
			{
				if (biomeAt.m_sBiomeName != biomeFilter)
				{
					return new Vector3i(0, -99999, 0);
				}
			}
			else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
			{
				if (array == null)
				{
					array = biomeFilter.Split(',', StringSplitOptions.None);
				}
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (biomeAt.m_sBiomeName == array[i])
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return new Vector3i(0, -99999, 0);
				}
			}
			else if (biomeFilterType == BiomeFilterTypes.SameBiome)
			{
				BiomeDefinition biomeAt2 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)entityAlive.position.x, (int)entityAlive.position.z);
				if (biomeAt != biomeAt2)
				{
					return new Vector3i(0, -99999, 0);
				}
			}
		}
		return vector3i;
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x001C5344 File Offset: 0x001C3544
	public static Vector3i CalculateRandomPositionFromFlatAreas(int entityID, float minDistance, float maxDistance, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "")
	{
		World world = GameManager.Instance.World;
		EntityAlive entityAlive = world.GetEntity(entityID) as EntityAlive;
		if (biomeFilterType == BiomeFilterTypes.SameBiome)
		{
			biomeFilter = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)entityAlive.position.x, (int)entityAlive.position.z).m_sBiomeName;
		}
		List<FlatArea> areasWithinRange = GameManager.Instance.World.FlatAreaManager.GetAreasWithinRange(entityAlive.position, minDistance, maxDistance, eFlatAreaSizeFilter.All, biomeFilterType, biomeFilter.Split(',', StringSplitOptions.None), ChunkProtectionLevel.NearLandClaim);
		if (areasWithinRange.Count > 0)
		{
			return Vector3i.FromVector3Rounded(areasWithinRange[world.GetGameRandom().RandomRange(areasWithinRange.Count)].GetRandomPosition(1f));
		}
		return new Vector3i(0, -99999, 0);
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x001C5414 File Offset: 0x001C3614
	public void FinalizePoint(int x, int y, int z)
	{
		this.position = new Vector3((float)x, (float)y, (float)z);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.Location, this.position);
		base.OwnerQuest.Position = this.position;
		this.positionSet = true;
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.Location, this.NavObjectName, -1);
		base.CurrentValue = 2;
	}

	// Token: 0x0600485E RID: 18526 RVA: 0x001C5478 File Offset: 0x001C3678
	public override void Update(float deltaTime)
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			if (!this.positionSet && base.CurrentValue != 1)
			{
				this.GetPosition() != Vector3.zero;
				this.OnStart();
			}
			switch (base.CurrentValue)
			{
			case 0:
				this.GetPosition() != Vector3.zero;
				return;
			case 1:
				break;
			case 2:
			{
				Entity ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
				if (base.OwnerQuest.NavObject != null && base.OwnerQuest.NavObject.TrackedPosition != this.position)
				{
					base.OwnerQuest.NavObject.TrackedPosition = this.position;
				}
				Vector3 vector = ownerPlayer.position;
				this.distance = Vector3.Distance(vector, this.position);
				if (this.distance < this.completionDistance && base.OwnerQuest.CheckRequirements())
				{
					base.CurrentValue = 3;
					this.Refresh();
					return;
				}
				break;
			}
			case 3:
			{
				if (this.completeWithinRange)
				{
					QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
					return;
				}
				Entity ownerPlayer2 = base.OwnerQuest.OwnerJournal.OwnerPlayer;
				if (base.OwnerQuest.NavObject != null && base.OwnerQuest.NavObject.TrackedPosition != this.position)
				{
					base.OwnerQuest.NavObject.TrackedPosition = this.position;
				}
				Vector3 vector2 = ownerPlayer2.position;
				this.distance = Vector3.Distance(vector2, this.position);
				if (this.distance > this.completionDistance)
				{
					base.CurrentValue = 2;
					this.Refresh();
				}
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnStart()
	{
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x001C562C File Offset: 0x001C382C
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 3;
		base.Complete = flag;
		if (base.Complete)
		{
			base.ObjectiveState = BaseObjective.ObjectiveStates.Complete;
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, this.PlayObjectiveComplete, null);
			base.OwnerQuest.RemoveMapObject();
			this.RemoveHooks();
		}
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x001C5680 File Offset: 0x001C3880
	public override BaseObjective Clone()
	{
		ObjectiveRandomGoto objectiveRandomGoto = new ObjectiveRandomGoto();
		this.CopyValues(objectiveRandomGoto);
		objectiveRandomGoto.position = this.position;
		objectiveRandomGoto.positionSet = this.positionSet;
		objectiveRandomGoto.completionDistance = this.completionDistance;
		objectiveRandomGoto.biomeFilter = this.biomeFilter;
		objectiveRandomGoto.biomeFilterType = this.biomeFilterType;
		return objectiveRandomGoto;
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x001C56D7 File Offset: 0x001C38D7
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		this.FinalizePoint((int)pos.x, (int)pos.y, (int)pos.z);
		return true;
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x001C56F8 File Offset: 0x001C38F8
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveRandomGoto.PropDistance, ref this.Value);
		properties.ParseFloat(ObjectiveRandomGoto.PropCompletionDistance, ref this.completionDistance);
		properties.ParseEnum<BiomeFilterTypes>(ObjectiveRandomGoto.PropBiomeFilterType, ref this.biomeFilterType);
		properties.ParseString(ObjectiveRandomGoto.PropBiomeFilter, ref this.biomeFilter);
	}

	// Token: 0x04003978 RID: 14712
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool positionSet;

	// Token: 0x04003979 RID: 14713
	[PublicizedFrom(EAccessModifier.Protected)]
	public float distance;

	// Token: 0x0400397A RID: 14714
	[PublicizedFrom(EAccessModifier.Protected)]
	public float completionDistance = 10f;

	// Token: 0x0400397B RID: 14715
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x0400397C RID: 14716
	[PublicizedFrom(EAccessModifier.Protected)]
	public string icon = "ui_game_symbol_quest";

	// Token: 0x0400397D RID: 14717
	[PublicizedFrom(EAccessModifier.Protected)]
	public BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome;

	// Token: 0x0400397E RID: 14718
	[PublicizedFrom(EAccessModifier.Protected)]
	public string biomeFilter = "";

	// Token: 0x0400397F RID: 14719
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;

	// Token: 0x04003980 RID: 14720
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool completeWithinRange = true;

	// Token: 0x04003981 RID: 14721
	public static string PropDistance = "distance";

	// Token: 0x04003982 RID: 14722
	public static string PropCompletionDistance = "completion_distance";

	// Token: 0x04003983 RID: 14723
	public static string PropBiomeFilterType = "biome_filter_type";

	// Token: 0x04003984 RID: 14724
	public static string PropBiomeFilter = "biome_filter";

	// Token: 0x02000998 RID: 2456
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum GotoStates
	{
		// Token: 0x04003986 RID: 14726
		NoPosition,
		// Token: 0x04003987 RID: 14727
		WaitingForPoint,
		// Token: 0x04003988 RID: 14728
		TryComplete,
		// Token: 0x04003989 RID: 14729
		Completed
	}
}
