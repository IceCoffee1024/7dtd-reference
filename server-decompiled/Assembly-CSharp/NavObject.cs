using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000745 RID: 1861
public class NavObject
{
	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x0600376F RID: 14191 RVA: 0x0016CB90 File Offset: 0x0016AD90
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

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06003770 RID: 14192 RVA: 0x0016CBC7 File Offset: 0x0016ADC7
	// (set) Token: 0x06003771 RID: 14193 RVA: 0x0016CBCF File Offset: 0x0016ADCF
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

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06003772 RID: 14194 RVA: 0x0016CBDF File Offset: 0x0016ADDF
	// (set) Token: 0x06003773 RID: 14195 RVA: 0x0016CBE7 File Offset: 0x0016ADE7
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

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06003774 RID: 14196 RVA: 0x0016CBF7 File Offset: 0x0016ADF7
	// (set) Token: 0x06003775 RID: 14197 RVA: 0x0016CBFF File Offset: 0x0016ADFF
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

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x0016CC36 File Offset: 0x0016AE36
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

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06003777 RID: 14199 RVA: 0x0016CC4F File Offset: 0x0016AE4F
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

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06003778 RID: 14200 RVA: 0x0016CC68 File Offset: 0x0016AE68
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

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06003779 RID: 14201 RVA: 0x0016CC81 File Offset: 0x0016AE81
	public bool HasOnScreen
	{
		get
		{
			return this.hasOnScreen;
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x0600377A RID: 14202 RVA: 0x0016CC8C File Offset: 0x0016AE8C
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

	// Token: 0x0600377B RID: 14203 RVA: 0x0016CCE4 File Offset: 0x0016AEE4
	public bool IsTrackedTransform(Transform transform)
	{
		return this.TrackType == NavObject.TrackTypes.Transform && this.trackedTransform == transform;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0016CCFD File Offset: 0x0016AEFD
	public bool IsTrackedPosition(Vector3 position)
	{
		return this.TrackType == NavObject.TrackTypes.Position && this.trackedPosition == position;
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0016CD16 File Offset: 0x0016AF16
	public bool IsTrackedEntity(Entity entity)
	{
		return this.TrackType == NavObject.TrackTypes.Entity && this.trackedEntity == entity;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0016CD30 File Offset: 0x0016AF30
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

	// Token: 0x0600377F RID: 14207 RVA: 0x0016CE1C File Offset: 0x0016B01C
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

	// Token: 0x06003780 RID: 14208 RVA: 0x0016D0CB File Offset: 0x0016B2CB
	public void AddNavObjectClass(NavObjectClass navClass)
	{
		if (!this.NavObjectClassList.Contains(navClass))
		{
			this.NavObjectClassList.Insert(0, navClass);
		}
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x0016D0E8 File Offset: 0x0016B2E8
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

	// Token: 0x06003782 RID: 14210 RVA: 0x0016D114 File Offset: 0x0016B314
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

	// Token: 0x06003783 RID: 14211 RVA: 0x0016D174 File Offset: 0x0016B374
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

	// Token: 0x06003784 RID: 14212 RVA: 0x0016D1C8 File Offset: 0x0016B3C8
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

	// Token: 0x06003785 RID: 14213 RVA: 0x0016D22C File Offset: 0x0016B42C
	public float GetMaxDistance(NavObjectSettings settings, EntityPlayer player)
	{
		if (this.TrackType == NavObject.TrackTypes.Entity && this.NavObjectClass.RequirementType == NavObjectClass.RequirementTypes.Tracking && settings.MaxDistance == -1f)
		{
			return EffectManager.GetValue(PassiveEffects.TrackDistance, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return settings.MaxDistance;
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x0016D287 File Offset: 0x0016B487
	public string GetSpriteName(NavObjectSettings settings)
	{
		if (!this.NavObjectClass.UseOverrideIcon)
		{
			return settings.SpriteName;
		}
		return this.OverrideSpriteName;
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x0016D2A4 File Offset: 0x0016B4A4
	public NavObject(string className)
	{
		this.Key = NavObject.nextKey++;
		this.SetupNavObjectClass(className);
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x0016D2F8 File Offset: 0x0016B4F8
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

	// Token: 0x06003789 RID: 14217 RVA: 0x0016D360 File Offset: 0x0016B560
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

	// Token: 0x0600378A RID: 14218 RVA: 0x0016D414 File Offset: 0x0016B614
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

	// Token: 0x0600378B RID: 14219 RVA: 0x0016D498 File Offset: 0x0016B698
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.CurrentCompassSettings.MaxScaleDistance;
		return Mathf.Lerp(this.CurrentCompassSettings.MinCompassIconScale, this.CurrentCompassSettings.MaxCompassIconScale, num);
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0016D4D8 File Offset: 0x0016B6D8
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

	// Token: 0x04002D42 RID: 11586
	public static Vector3 InvalidPos = new Vector3(-99999f, -99999f, -99999f);

	// Token: 0x04002D43 RID: 11587
	public List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002D44 RID: 11588
	public NavObjectClass NavObjectClass;

	// Token: 0x04002D45 RID: 11589
	public NavObject.TrackTypes TrackType;

	// Token: 0x04002D46 RID: 11590
	public bool IsActive = true;

	// Token: 0x04002D47 RID: 11591
	public bool ForceDisabled;

	// Token: 0x04002D48 RID: 11592
	public int EntityID;

	// Token: 0x04002D49 RID: 11593
	public Entity OwnerEntity;

	// Token: 0x04002D4A RID: 11594
	public string name;

	// Token: 0x04002D4B RID: 11595
	public bool usingLocalizationId;

	// Token: 0x04002D4C RID: 11596
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x04002D4D RID: 11597
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform trackedTransform;

	// Token: 0x04002D4E RID: 11598
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 trackedPosition;

	// Token: 0x04002D4F RID: 11599
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity trackedEntity;

	// Token: 0x04002D50 RID: 11600
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOnScreen;

	// Token: 0x04002D51 RID: 11601
	[PublicizedFrom(EAccessModifier.Private)]
	public static int nextKey;

	// Token: 0x04002D52 RID: 11602
	public int Key;

	// Token: 0x04002D53 RID: 11603
	public float ExtraData;

	// Token: 0x04002D54 RID: 11604
	public bool IsTracked;

	// Token: 0x04002D55 RID: 11605
	public bool hiddenOnCompass = true;

	// Token: 0x04002D56 RID: 11606
	public string HiddenDisplayName;

	// Token: 0x04002D57 RID: 11607
	public bool hiddenOnMap;

	// Token: 0x04002D58 RID: 11608
	public string OverrideSpriteName = "";

	// Token: 0x04002D59 RID: 11609
	public bool UseOverrideColor;

	// Token: 0x04002D5A RID: 11610
	public bool UseOverrideFontColor;

	// Token: 0x04002D5B RID: 11611
	public Color OverrideColor;

	// Token: 0x02000746 RID: 1862
	public enum TrackTypes
	{
		// Token: 0x04002D5D RID: 11613
		None,
		// Token: 0x04002D5E RID: 11614
		Transform,
		// Token: 0x04002D5F RID: 11615
		Position,
		// Token: 0x04002D60 RID: 11616
		Entity
	}
}
