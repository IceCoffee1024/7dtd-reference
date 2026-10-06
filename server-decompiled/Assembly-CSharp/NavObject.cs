using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000751 RID: 1873
public class NavObject
{
	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06003808 RID: 14344 RVA: 0x0016F710 File Offset: 0x0016D910
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

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06003809 RID: 14345 RVA: 0x0016F747 File Offset: 0x0016D947
	// (set) Token: 0x0600380A RID: 14346 RVA: 0x0016F74F File Offset: 0x0016D94F
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

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x0600380B RID: 14347 RVA: 0x0016F75F File Offset: 0x0016D95F
	// (set) Token: 0x0600380C RID: 14348 RVA: 0x0016F767 File Offset: 0x0016D967
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

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x0600380D RID: 14349 RVA: 0x0016F777 File Offset: 0x0016D977
	// (set) Token: 0x0600380E RID: 14350 RVA: 0x0016F77F File Offset: 0x0016D97F
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

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x0600380F RID: 14351 RVA: 0x0016F7B6 File Offset: 0x0016D9B6
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

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06003810 RID: 14352 RVA: 0x0016F7CF File Offset: 0x0016D9CF
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

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06003811 RID: 14353 RVA: 0x0016F7E8 File Offset: 0x0016D9E8
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

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06003812 RID: 14354 RVA: 0x0016F801 File Offset: 0x0016DA01
	public bool HasOnScreen
	{
		get
		{
			return this.hasOnScreen;
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06003813 RID: 14355 RVA: 0x0016F80C File Offset: 0x0016DA0C
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

	// Token: 0x06003814 RID: 14356 RVA: 0x0016F864 File Offset: 0x0016DA64
	public bool IsTrackedTransform(Transform transform)
	{
		return this.TrackType == NavObject.TrackTypes.Transform && this.trackedTransform == transform;
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0016F87D File Offset: 0x0016DA7D
	public bool IsTrackedPosition(Vector3 position)
	{
		return this.TrackType == NavObject.TrackTypes.Position && this.trackedPosition == position;
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x0016F896 File Offset: 0x0016DA96
	public bool IsTrackedEntity(Entity entity)
	{
		return this.TrackType == NavObject.TrackTypes.Entity && this.trackedEntity == entity;
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x0016F8B0 File Offset: 0x0016DAB0
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

	// Token: 0x06003818 RID: 14360 RVA: 0x0016F99C File Offset: 0x0016DB9C
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

	// Token: 0x06003819 RID: 14361 RVA: 0x0016FC4B File Offset: 0x0016DE4B
	public void AddNavObjectClass(NavObjectClass navClass)
	{
		if (!this.NavObjectClassList.Contains(navClass))
		{
			this.NavObjectClassList.Insert(0, navClass);
		}
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x0016FC68 File Offset: 0x0016DE68
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

	// Token: 0x0600381B RID: 14363 RVA: 0x0016FC94 File Offset: 0x0016DE94
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

	// Token: 0x0600381C RID: 14364 RVA: 0x0016FCF4 File Offset: 0x0016DEF4
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

	// Token: 0x0600381D RID: 14365 RVA: 0x0016FD48 File Offset: 0x0016DF48
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

	// Token: 0x0600381E RID: 14366 RVA: 0x0016FDAC File Offset: 0x0016DFAC
	public float GetMaxDistance(NavObjectSettings settings, EntityPlayer player)
	{
		if (this.TrackType == NavObject.TrackTypes.Entity && this.NavObjectClass.RequirementType == NavObjectClass.RequirementTypes.Tracking && settings.MaxDistance == -1f)
		{
			return EffectManager.GetValue(PassiveEffects.TrackDistance, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return settings.MaxDistance;
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x0016FE07 File Offset: 0x0016E007
	public string GetSpriteName(NavObjectSettings settings)
	{
		if (!this.NavObjectClass.UseOverrideIcon)
		{
			return settings.SpriteName;
		}
		return this.OverrideSpriteName;
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x0016FE24 File Offset: 0x0016E024
	public NavObject(string className)
	{
		this.Key = NavObject.nextKey++;
		this.SetupNavObjectClass(className);
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x0016FE78 File Offset: 0x0016E078
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

	// Token: 0x06003822 RID: 14370 RVA: 0x0016FEE0 File Offset: 0x0016E0E0
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

	// Token: 0x06003823 RID: 14371 RVA: 0x0016FF94 File Offset: 0x0016E194
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

	// Token: 0x06003824 RID: 14372 RVA: 0x00170018 File Offset: 0x0016E218
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.CurrentCompassSettings.MaxScaleDistance;
		return Mathf.Lerp(this.CurrentCompassSettings.MinCompassIconScale, this.CurrentCompassSettings.MaxCompassIconScale, num);
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x00170058 File Offset: 0x0016E258
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

	// Token: 0x04002DB3 RID: 11699
	public static Vector3 InvalidPos = new Vector3(-99999f, -99999f, -99999f);

	// Token: 0x04002DB4 RID: 11700
	public List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DB5 RID: 11701
	public NavObjectClass NavObjectClass;

	// Token: 0x04002DB6 RID: 11702
	public NavObject.TrackTypes TrackType;

	// Token: 0x04002DB7 RID: 11703
	public bool IsActive = true;

	// Token: 0x04002DB8 RID: 11704
	public bool ForceDisabled;

	// Token: 0x04002DB9 RID: 11705
	public int EntityID;

	// Token: 0x04002DBA RID: 11706
	public Entity OwnerEntity;

	// Token: 0x04002DBB RID: 11707
	public string name;

	// Token: 0x04002DBC RID: 11708
	public bool usingLocalizationId;

	// Token: 0x04002DBD RID: 11709
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04002DBE RID: 11710
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform trackedTransform;

	// Token: 0x04002DBF RID: 11711
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 trackedPosition;

	// Token: 0x04002DC0 RID: 11712
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity trackedEntity;

	// Token: 0x04002DC1 RID: 11713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOnScreen;

	// Token: 0x04002DC2 RID: 11714
	[PublicizedFrom(EAccessModifier.Private)]
	public static int nextKey;

	// Token: 0x04002DC3 RID: 11715
	public int Key;

	// Token: 0x04002DC4 RID: 11716
	public float ExtraData;

	// Token: 0x04002DC5 RID: 11717
	public bool IsTracked;

	// Token: 0x04002DC6 RID: 11718
	public bool hiddenOnCompass = true;

	// Token: 0x04002DC7 RID: 11719
	public string HiddenDisplayName;

	// Token: 0x04002DC8 RID: 11720
	public bool hiddenOnMap;

	// Token: 0x04002DC9 RID: 11721
	public string OverrideSpriteName = "";

	// Token: 0x04002DCA RID: 11722
	public bool UseOverrideColor;

	// Token: 0x04002DCB RID: 11723
	public bool UseOverrideFontColor;

	// Token: 0x04002DCC RID: 11724
	public Color OverrideColor;

	// Token: 0x02000752 RID: 1874
	public enum TrackTypes
	{
		// Token: 0x04002DCE RID: 11726
		None,
		// Token: 0x04002DCF RID: 11727
		Transform,
		// Token: 0x04002DD0 RID: 11728
		Position,
		// Token: 0x04002DD1 RID: 11729
		Entity
	}
}
