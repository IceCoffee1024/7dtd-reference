using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200074F RID: 1871
public class NavObject
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x060037C5 RID: 14277 RVA: 0x0016EDAC File Offset: 0x0016CFAC
	public string DisplayName
	{
		get
		{
			if (this.usingLocalizationId)
			{
				if (string.IsNullOrEmpty(this.localizedName))
				{
					this.localizedName = Localization.Get(this.name, false, null);
				}
				return this.localizedName;
			}
			return this.name;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x060037C6 RID: 14278 RVA: 0x0016EDE3 File Offset: 0x0016CFE3
	// (set) Token: 0x060037C7 RID: 14279 RVA: 0x0016EDEB File Offset: 0x0016CFEB
	public Transform TrackedTransform
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.trackedTransform;
		}
		set
		{
			this.trackedTransform = value;
			this.TrackType = NavObject.TrackTypes.Transform;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x060037C8 RID: 14280 RVA: 0x0016EDFB File Offset: 0x0016CFFB
	// (set) Token: 0x060037C9 RID: 14281 RVA: 0x0016EE03 File Offset: 0x0016D003
	public Vector3 TrackedPosition
	{
		get
		{
			return this.trackedPosition;
		}
		set
		{
			this.trackedPosition = value;
			this.TrackType = NavObject.TrackTypes.Position;
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x060037CA RID: 14282 RVA: 0x0016EE13 File Offset: 0x0016D013
	// (set) Token: 0x060037CB RID: 14283 RVA: 0x0016EE1B File Offset: 0x0016D01B
	public Entity TrackedEntity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.trackedEntity;
		}
		set
		{
			this.trackedEntity = value;
			this.EntityID = (this.trackedEntity ? this.trackedEntity.entityId : (-1));
			this.TrackType = NavObject.TrackTypes.Entity;
			this.SetupEntityOptions();
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x060037CC RID: 14284 RVA: 0x0016EE52 File Offset: 0x0016D052
	public NavObjectMapSettings CurrentMapSettings
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			NavObjectClass navObjectClass = this.NavObjectClass;
			if (navObjectClass == null)
			{
				return null;
			}
			return navObjectClass.GetMapSettings(this.IsActive);
		}
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x060037CD RID: 14285 RVA: 0x0016EE6B File Offset: 0x0016D06B
	public NavObjectCompassSettings CurrentCompassSettings
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			NavObjectClass navObjectClass = this.NavObjectClass;
			if (navObjectClass == null)
			{
				return null;
			}
			return navObjectClass.GetCompassSettings(this.IsActive);
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x060037CE RID: 14286 RVA: 0x0016EE84 File Offset: 0x0016D084
	public NavObjectScreenSettings CurrentScreenSettings
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			NavObjectClass navObjectClass = this.NavObjectClass;
			if (navObjectClass == null)
			{
				return null;
			}
			return navObjectClass.GetOnScreenSettings(this.IsActive);
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x060037CF RID: 14287 RVA: 0x0016EE9D File Offset: 0x0016D09D
	public bool HasOnScreen
	{
		get
		{
			return this.hasOnScreen;
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x060037D0 RID: 14288 RVA: 0x0016EEA8 File Offset: 0x0016D0A8
	public Vector3 Rotation
	{
		get
		{
			if (!this.CurrentMapSettings.UseRotation || this.TrackType != NavObject.TrackTypes.Entity)
			{
				return Vector3.zero;
			}
			if (this.trackedEntity.AttachedToEntity != null)
			{
				return this.trackedEntity.AttachedToEntity.rotation;
			}
			return this.trackedEntity.rotation;
		}
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x0016EF00 File Offset: 0x0016D100
	public bool IsTrackedTransform(Transform transform)
	{
		return this.TrackType == NavObject.TrackTypes.Transform && this.trackedTransform == transform;
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0016EF19 File Offset: 0x0016D119
	public bool IsTrackedPosition(Vector3 position)
	{
		return this.TrackType == NavObject.TrackTypes.Position && this.trackedPosition == position;
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0016EF32 File Offset: 0x0016D132
	public bool IsTrackedEntity(Entity entity)
	{
		return this.TrackType == NavObject.TrackTypes.Entity && this.trackedEntity == entity;
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x0016EF4C File Offset: 0x0016D14C
	public bool IsValidPlayer(EntityPlayerLocal player, NavObjectClass navObjectClass)
	{
		if (this.ForceDisabled)
		{
			return false;
		}
		bool flag = true;
		if (this.TrackType == NavObject.TrackTypes.Entity)
		{
			flag = this.IsValidEntity(player, this.TrackedEntity, navObjectClass);
		}
		switch (navObjectClass.RequirementType)
		{
		case NavObjectClass.RequirementTypes.CVar:
			return player.GetCVar(navObjectClass.RequirementName) > 0f && flag;
		case NavObjectClass.RequirementTypes.QuestBounds:
		case NavObjectClass.RequirementTypes.Tracking:
			return flag;
		case NavObjectClass.RequirementTypes.NoTag:
			return !NavObjectManager.Instance.HasNavObjectTag(navObjectClass.RequirementName) && flag;
		case NavObjectClass.RequirementTypes.IsOwner:
			return this.OwnerEntity == player && flag;
		case NavObjectClass.RequirementTypes.MinimumTreasureRadius:
		{
			float num = this.ExtraData;
			num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, num, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			num = Mathf.Clamp(num, 0f, num);
			return num == 0f;
		}
		}
		return flag;
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x0016F038 File Offset: 0x0016D238
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsValidEntity(EntityPlayerLocal player, Entity entity, NavObjectClass navObjectClass)
	{
		if (entity == null)
		{
			return true;
		}
		if (player == null)
		{
			return true;
		}
		EntityAlive entityAlive = entity as EntityAlive;
		if (entityAlive)
		{
			if (navObjectClass.RequirementType == NavObjectClass.RequirementTypes.None)
			{
				return entityAlive.IsAlive() && !entityAlive.IsSleeperPassive;
			}
			if (!entityAlive.IsAlive() || entityAlive.IsSleeperPassive)
			{
				return false;
			}
			switch (navObjectClass.RequirementType)
			{
			case NavObjectClass.RequirementTypes.CVar:
				return entityAlive.GetCVar(navObjectClass.RequirementName) > 0f;
			case NavObjectClass.RequirementTypes.QuestBounds:
				if (player.QuestJournal.ActiveQuest != null && entityAlive.IsSleeper)
				{
					Vector3 position = entity.position;
					position.y = position.z;
					if (player.ZombieCompassBounds.Contains(position))
					{
						return true;
					}
				}
				return false;
			case NavObjectClass.RequirementTypes.Tracking:
				return EffectManager.GetValue(PassiveEffects.Tracking, null, 0f, player, null, entity.EntityTags, true, true, true, true, true, 1, true, false) > 0f;
			case NavObjectClass.RequirementTypes.InParty:
				return player.Party != null && player.Party.MemberList.Contains(entity as EntityPlayer) && entity != player && !(entity as EntityPlayer).IsSpectator && (player.AttachedToEntity == null || player.AttachedToEntity != entity.AttachedToEntity);
			case NavObjectClass.RequirementTypes.IsAlly:
				return entity as EntityPlayer != null && (entity as EntityPlayer).IsFriendOfLocalPlayer && entity != player && !(entity as EntityPlayer).IsSpectator;
			case NavObjectClass.RequirementTypes.IsPlayer:
				return entity == player;
			case NavObjectClass.RequirementTypes.IsVehicleOwner:
				return (entity as EntityVehicle != null && (entity as EntityVehicle).HasOwnedEntity(player.entityId)) || (entity as EntityTurret != null && (entity as EntityTurret).belongsPlayerId == player.entityId);
			case NavObjectClass.RequirementTypes.NoActiveQuests:
				return entity as EntityNPC == null || player.QuestJournal.FindReadyForTurnInQuestByGiver(entity.entityId) == null;
			case NavObjectClass.RequirementTypes.IsTwitchSpawnedSelf:
				return entity.spawnById == player.entityId && !string.IsNullOrEmpty(entity.spawnByName);
			case NavObjectClass.RequirementTypes.IsTwitchSpawnedOther:
				return entity.spawnById > 0 && entity.spawnById != player.entityId && !string.IsNullOrEmpty(entity.spawnByName);
			}
		}
		else
		{
			NavObjectClass.RequirementTypes requirementType = navObjectClass.RequirementType;
			if (requirementType == NavObjectClass.RequirementTypes.IsTwitchSpawnedSelf)
			{
				return entity.spawnById == player.entityId;
			}
			if (requirementType == NavObjectClass.RequirementTypes.IsTwitchSpawnedOther)
			{
				return entity.spawnById > 0 && entity.spawnById != player.entityId;
			}
		}
		return true;
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x0016F2E7 File Offset: 0x0016D4E7
	public void AddNavObjectClass(NavObjectClass navClass)
	{
		if (!this.NavObjectClassList.Contains(navClass))
		{
			this.NavObjectClassList.Insert(0, navClass);
		}
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0016F304 File Offset: 0x0016D504
	public bool RemoveNavObjectClass(NavObjectClass navClass)
	{
		this.NavObjectClassList.Remove(navClass);
		if (this.NavObjectClassList.Count == 0)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this);
			return true;
		}
		return false;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0016F330 File Offset: 0x0016D530
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupEntityOptions()
	{
		if (this.TrackType == NavObject.TrackTypes.Entity && this.NavObjectClass != null && this.NavObjectClass.RequirementType == NavObjectClass.RequirementTypes.Tracking)
		{
			this.OverrideSpriteName = ((this.TrackedEntity.GetTrackerIcon() == null) ? "" : this.TrackedEntity.GetTrackerIcon());
			return;
		}
		this.OverrideSpriteName = "";
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0016F390 File Offset: 0x0016D590
	public bool IsValid()
	{
		if (this.TrackType == NavObject.TrackTypes.Transform && this.TrackedTransform == null)
		{
			this.TrackType = NavObject.TrackTypes.None;
		}
		else if (this.TrackType == NavObject.TrackTypes.Entity && this.TrackedEntity == null)
		{
			this.TrackType = NavObject.TrackTypes.None;
		}
		return this.TrackType > NavObject.TrackTypes.None;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x0016F3E4 File Offset: 0x0016D5E4
	public Vector3 GetPosition()
	{
		switch (this.TrackType)
		{
		case NavObject.TrackTypes.Transform:
			return this.trackedTransform.position;
		case NavObject.TrackTypes.Position:
			return this.trackedPosition - Origin.position;
		case NavObject.TrackTypes.Entity:
			return this.trackedEntity.position - Origin.position;
		default:
			return NavObject.InvalidPos;
		}
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x0016F448 File Offset: 0x0016D648
	public float GetMaxDistance(NavObjectSettings settings, EntityPlayer player)
	{
		if (this.TrackType == NavObject.TrackTypes.Entity && this.NavObjectClass.RequirementType == NavObjectClass.RequirementTypes.Tracking && settings.MaxDistance == -1f)
		{
			return EffectManager.GetValue(PassiveEffects.TrackDistance, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return settings.MaxDistance;
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0016F4A3 File Offset: 0x0016D6A3
	public string GetSpriteName(NavObjectSettings settings)
	{
		if (!this.NavObjectClass.UseOverrideIcon)
		{
			return settings.SpriteName;
		}
		return this.OverrideSpriteName;
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0016F4C0 File Offset: 0x0016D6C0
	public NavObject(string className)
	{
		this.Key = NavObject.nextKey++;
		this.SetupNavObjectClass(className);
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0016F514 File Offset: 0x0016D714
	public void Reset(string className)
	{
		this.UseOverrideColor = false;
		this.OverrideColor = Color.white;
		this.SetupNavObjectClass(className);
		this.trackedPosition = NavObject.InvalidPos;
		this.trackedTransform = null;
		this.trackedEntity = null;
		this.OwnerEntity = null;
		this.name = "";
		this.ForceDisabled = false;
		this.usingLocalizationId = false;
		this.TrackType = NavObject.TrackTypes.None;
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0016F57C File Offset: 0x0016D77C
	public void SetupNavObjectClass(string className)
	{
		this.NavObjectClassList.Clear();
		this.hasOnScreen = false;
		if (className.Contains(","))
		{
			string[] array = className.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				NavObjectClass navObjectClass = NavObjectClass.GetNavObjectClass(array[i]);
				if (navObjectClass != null)
				{
					if (navObjectClass.OnScreenSettings != null || navObjectClass.InactiveOnScreenSettings != null)
					{
						this.hasOnScreen = true;
					}
					this.NavObjectClassList.Add(navObjectClass);
				}
			}
		}
		else
		{
			NavObjectClass navObjectClass2 = NavObjectClass.GetNavObjectClass(className);
			if (navObjectClass2.OnScreenSettings != null || navObjectClass2.InactiveOnScreenSettings != null)
			{
				this.hasOnScreen = true;
			}
			this.NavObjectClassList.Add(navObjectClass2);
		}
		this.NavObjectClass = this.NavObjectClassList[0];
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0016F630 File Offset: 0x0016D830
	public void HandleActiveNavClass(EntityPlayerLocal localPlayer)
	{
		if (this.NavObjectClassList != null && this.NavObjectClassList.Count > 0)
		{
			for (int i = 0; i < this.NavObjectClassList.Count; i++)
			{
				if (this.IsValidPlayer(localPlayer, this.NavObjectClassList[i]))
				{
					if (this.NavObjectClass != this.NavObjectClassList[i])
					{
						this.NavObjectClass = this.NavObjectClassList[i];
						this.SetupEntityOptions();
					}
					return;
				}
			}
			this.NavObjectClass = null;
		}
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x0016F6B4 File Offset: 0x0016D8B4
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.CurrentCompassSettings.MaxScaleDistance;
		return Mathf.Lerp(this.CurrentCompassSettings.MinCompassIconScale, this.CurrentCompassSettings.MaxCompassIconScale, num);
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x0016F6F4 File Offset: 0x0016D8F4
	public override string ToString()
	{
		string text = "";
		if (this.TrackType == NavObject.TrackTypes.Transform)
		{
			text = ((this.TrackedTransform != null) ? this.TrackedTransform.name : "none");
		}
		else if (this.TrackType == NavObject.TrackTypes.Entity)
		{
			text = ((this.TrackedEntity != null) ? this.TrackedEntity.GetDebugName() : "none");
		}
		string text2 = "{0} #{1}, {2}, {3}, {4}, {5}, {6}";
		object[] array = new object[7];
		array[0] = this.name;
		array[1] = this.NavObjectClassList.Count;
		array[2] = ((this.NavObjectClass != null) ? this.NavObjectClass.NavObjectClassName : "null");
		int num = 3;
		NavObjectClass navObjectClass = this.NavObjectClass;
		array[num] = ((navObjectClass != null) ? new NavObjectClass.RequirementTypes?(navObjectClass.RequirementType) : null);
		array[4] = this.TrackType;
		array[5] = text;
		array[6] = this.GetPosition();
		return string.Format(text2, array);
	}

	// Token: 0x04002DBA RID: 11706
	public static Vector3 InvalidPos = new Vector3(-99999f, -99999f, -99999f);

	// Token: 0x04002DBB RID: 11707
	public List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DBC RID: 11708
	public NavObjectClass NavObjectClass;

	// Token: 0x04002DBD RID: 11709
	public NavObject.TrackTypes TrackType;

	// Token: 0x04002DBE RID: 11710
	public bool IsActive = true;

	// Token: 0x04002DBF RID: 11711
	public bool ForceDisabled;

	// Token: 0x04002DC0 RID: 11712
	public int EntityID;

	// Token: 0x04002DC1 RID: 11713
	public Entity OwnerEntity;

	// Token: 0x04002DC2 RID: 11714
	public string name;

	// Token: 0x04002DC3 RID: 11715
	public bool usingLocalizationId;

	// Token: 0x04002DC4 RID: 11716
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04002DC5 RID: 11717
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform trackedTransform;

	// Token: 0x04002DC6 RID: 11718
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 trackedPosition;

	// Token: 0x04002DC7 RID: 11719
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity trackedEntity;

	// Token: 0x04002DC8 RID: 11720
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOnScreen;

	// Token: 0x04002DC9 RID: 11721
	[PublicizedFrom(EAccessModifier.Private)]
	public static int nextKey;

	// Token: 0x04002DCA RID: 11722
	public int Key;

	// Token: 0x04002DCB RID: 11723
	public float ExtraData;

	// Token: 0x04002DCC RID: 11724
	public bool IsTracked;

	// Token: 0x04002DCD RID: 11725
	public bool hiddenOnCompass = true;

	// Token: 0x04002DCE RID: 11726
	public string HiddenDisplayName;

	// Token: 0x04002DCF RID: 11727
	public bool hiddenOnMap;

	// Token: 0x04002DD0 RID: 11728
	public string OverrideSpriteName = "";

	// Token: 0x04002DD1 RID: 11729
	public bool UseOverrideColor;

	// Token: 0x04002DD2 RID: 11730
	public bool UseOverrideFontColor;

	// Token: 0x04002DD3 RID: 11731
	public Color OverrideColor;

	// Token: 0x02000750 RID: 1872
	public enum TrackTypes
	{
		// Token: 0x04002DD5 RID: 11733
		None,
		// Token: 0x04002DD6 RID: 11734
		Transform,
		// Token: 0x04002DD7 RID: 11735
		Position,
		// Token: 0x04002DD8 RID: 11736
		Entity
	}
}
