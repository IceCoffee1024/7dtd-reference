using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public class NavObject
{
	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x0600381F RID: 14367 RVA: 0x001706A8 File Offset: 0x0016E8A8
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

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06003820 RID: 14368 RVA: 0x001706DF File Offset: 0x0016E8DF
	// (set) Token: 0x06003821 RID: 14369 RVA: 0x001706E7 File Offset: 0x0016E8E7
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

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06003822 RID: 14370 RVA: 0x001706F7 File Offset: 0x0016E8F7
	// (set) Token: 0x06003823 RID: 14371 RVA: 0x001706FF File Offset: 0x0016E8FF
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

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06003824 RID: 14372 RVA: 0x0017070F File Offset: 0x0016E90F
	// (set) Token: 0x06003825 RID: 14373 RVA: 0x00170717 File Offset: 0x0016E917
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

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06003826 RID: 14374 RVA: 0x0017074E File Offset: 0x0016E94E
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

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06003827 RID: 14375 RVA: 0x00170767 File Offset: 0x0016E967
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

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06003828 RID: 14376 RVA: 0x00170780 File Offset: 0x0016E980
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

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06003829 RID: 14377 RVA: 0x00170799 File Offset: 0x0016E999
	public bool HasOnScreen
	{
		get
		{
			return this.hasOnScreen;
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x0600382A RID: 14378 RVA: 0x001707A4 File Offset: 0x0016E9A4
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

	// Token: 0x0600382B RID: 14379 RVA: 0x001707FC File Offset: 0x0016E9FC
	public bool IsTrackedTransform(Transform transform)
	{
		return this.TrackType == NavObject.TrackTypes.Transform && this.trackedTransform == transform;
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x00170815 File Offset: 0x0016EA15
	public bool IsTrackedPosition(Vector3 position)
	{
		return this.TrackType == NavObject.TrackTypes.Position && this.trackedPosition == position;
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0017082E File Offset: 0x0016EA2E
	public bool IsTrackedEntity(Entity entity)
	{
		return this.TrackType == NavObject.TrackTypes.Entity && this.trackedEntity == entity;
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x00170848 File Offset: 0x0016EA48
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

	// Token: 0x0600382F RID: 14383 RVA: 0x00170934 File Offset: 0x0016EB34
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

	// Token: 0x06003830 RID: 14384 RVA: 0x00170BE3 File Offset: 0x0016EDE3
	public void AddNavObjectClass(NavObjectClass navClass)
	{
		if (!this.NavObjectClassList.Contains(navClass))
		{
			this.NavObjectClassList.Insert(0, navClass);
		}
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x00170C00 File Offset: 0x0016EE00
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

	// Token: 0x06003832 RID: 14386 RVA: 0x00170C2C File Offset: 0x0016EE2C
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

	// Token: 0x06003833 RID: 14387 RVA: 0x00170C8C File Offset: 0x0016EE8C
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

	// Token: 0x06003834 RID: 14388 RVA: 0x00170CE0 File Offset: 0x0016EEE0
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

	// Token: 0x06003835 RID: 14389 RVA: 0x00170D44 File Offset: 0x0016EF44
	public float GetMaxDistance(NavObjectSettings settings, EntityPlayer player)
	{
		if (this.TrackType == NavObject.TrackTypes.Entity && this.NavObjectClass.RequirementType == NavObjectClass.RequirementTypes.Tracking && settings.MaxDistance == -1f)
		{
			return EffectManager.GetValue(PassiveEffects.TrackDistance, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return settings.MaxDistance;
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x00170D9F File Offset: 0x0016EF9F
	public string GetSpriteName(NavObjectSettings settings)
	{
		if (!this.NavObjectClass.UseOverrideIcon)
		{
			return settings.SpriteName;
		}
		return this.OverrideSpriteName;
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x00170DBC File Offset: 0x0016EFBC
	public NavObject(string className)
	{
		this.Key = NavObject.nextKey++;
		this.SetupNavObjectClass(className);
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x00170E10 File Offset: 0x0016F010
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

	// Token: 0x06003839 RID: 14393 RVA: 0x00170E78 File Offset: 0x0016F078
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

	// Token: 0x0600383A RID: 14394 RVA: 0x00170F2C File Offset: 0x0016F12C
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

	// Token: 0x0600383B RID: 14395 RVA: 0x00170FB0 File Offset: 0x0016F1B0
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.CurrentCompassSettings.MaxScaleDistance;
		return Mathf.Lerp(this.CurrentCompassSettings.MinCompassIconScale, this.CurrentCompassSettings.MaxCompassIconScale, num);
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00170FF0 File Offset: 0x0016F1F0
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

	// Token: 0x04002DCF RID: 11727
	public static Vector3 InvalidPos = new Vector3(-99999f, -99999f, -99999f);

	// Token: 0x04002DD0 RID: 11728
	public List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DD1 RID: 11729
	public NavObjectClass NavObjectClass;

	// Token: 0x04002DD2 RID: 11730
	public NavObject.TrackTypes TrackType;

	// Token: 0x04002DD3 RID: 11731
	public bool IsActive = true;

	// Token: 0x04002DD4 RID: 11732
	public bool ForceDisabled;

	// Token: 0x04002DD5 RID: 11733
	public int EntityID;

	// Token: 0x04002DD6 RID: 11734
	public Entity OwnerEntity;

	// Token: 0x04002DD7 RID: 11735
	public string name;

	// Token: 0x04002DD8 RID: 11736
	public bool usingLocalizationId;

	// Token: 0x04002DD9 RID: 11737
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04002DDA RID: 11738
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform trackedTransform;

	// Token: 0x04002DDB RID: 11739
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 trackedPosition;

	// Token: 0x04002DDC RID: 11740
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity trackedEntity;

	// Token: 0x04002DDD RID: 11741
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOnScreen;

	// Token: 0x04002DDE RID: 11742
	[PublicizedFrom(EAccessModifier.Private)]
	public static int nextKey;

	// Token: 0x04002DDF RID: 11743
	public int Key;

	// Token: 0x04002DE0 RID: 11744
	public float ExtraData;

	// Token: 0x04002DE1 RID: 11745
	public bool IsTracked;

	// Token: 0x04002DE2 RID: 11746
	public bool hiddenOnCompass = true;

	// Token: 0x04002DE3 RID: 11747
	public string HiddenDisplayName;

	// Token: 0x04002DE4 RID: 11748
	public bool hiddenOnMap;

	// Token: 0x04002DE5 RID: 11749
	public string OverrideSpriteName = "";

	// Token: 0x04002DE6 RID: 11750
	public bool UseOverrideColor;

	// Token: 0x04002DE7 RID: 11751
	public bool UseOverrideFontColor;

	// Token: 0x04002DE8 RID: 11752
	public Color OverrideColor;

	// Token: 0x0200075A RID: 1882
	public enum TrackTypes
	{
		// Token: 0x04002DEA RID: 11754
		None,
		// Token: 0x04002DEB RID: 11755
		Transform,
		// Token: 0x04002DEC RID: 11756
		Position,
		// Token: 0x04002DED RID: 11757
		Entity
	}
}
