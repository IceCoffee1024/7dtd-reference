using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public class NetEntityDistributionEntry
{
	// Token: 0x060039CD RID: 14797 RVA: 0x00179040 File Offset: 0x00177240
	public NetEntityDistributionEntry(Entity _e, int _d, int _ticks, bool _isMotionSent)
	{
		this.updateCounter = 0;
		this.sendFullUpdateAfterTicks = 0;
		this.trackedPlayers = new HashSet<EntityPlayer>();
		this.trackedEntity = _e;
		this.trackingDistanceThreshold = Math.Min(46340, _d);
		this.updatTickCounter = _ticks;
		this.shouldSendMotionUpdates = _isMotionSent;
		this.encodedPos = NetEntityDistributionEntry.EncodePos(_e.position);
		this.encodedRot = NetEntityDistributionEntry.EncodeRot(_e.rotation);
		this.encodedOnGround = _e.onGround;
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x001790C8 File Offset: 0x001772C8
	public override bool Equals(object _other)
	{
		return _other is NetEntityDistributionEntry && ((NetEntityDistributionEntry)_other).trackedEntity.entityId == this.trackedEntity.entityId;
	}

	// Token: 0x060039CF RID: 14799 RVA: 0x001790F1 File Offset: 0x001772F1
	public override int GetHashCode()
	{
		return this.trackedEntity.entityId;
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x00179100 File Offset: 0x00177300
	public void SendToPlayers(NetPackage _packet, int _excludePlayer, bool _inRangeOnly = false, int _range = 192)
	{
		foreach (EntityPlayer entityPlayer in this.trackedPlayers)
		{
			if (entityPlayer.entityId != _excludePlayer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(_packet, false, entityPlayer.entityId, -1, _inRangeOnly ? this.trackedEntity.entityId : (-1), null, _range, false);
			}
		}
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x00179188 File Offset: 0x00177388
	public void sendPacketToTrackedPlayersAndTrackedEntity(NetPackage _packet, int _excludePlayer, bool _inRangeOnly = false)
	{
		this.SendToPlayers(_packet, _excludePlayer, _inRangeOnly, 192);
		if (this.trackedEntity is EntityPlayer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(_packet, false, this.trackedEntity.entityId, -1, _inRangeOnly ? this.trackedEntity.entityId : (-1), null, 192, false);
		}
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x001791E8 File Offset: 0x001773E8
	public void SendDestroyEntityToPlayers()
	{
		this.SendToPlayers(NetPackageManager.GetPackage<NetPackageEntityRemove>().Setup(this.trackedEntity.entityId, EnumRemoveEntityReason.Killed), -1, false, 192);
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x0017920D File Offset: 0x0017740D
	public void SendUnloadEntityToPlayers()
	{
		this.SendToPlayers(NetPackageManager.GetPackage<NetPackageEntityRemove>().Setup(this.trackedEntity.entityId, EnumRemoveEntityReason.Unloaded), -1, false, 192);
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x00179232 File Offset: 0x00177432
	public void Remove(EntityPlayer _e)
	{
		if (this.trackedPlayers.Contains(_e))
		{
			this.trackedPlayers.Remove(_e);
		}
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x00179250 File Offset: 0x00177450
	public void updatePlayerEntity(EntityPlayer _ep)
	{
		if (_ep == this.trackedEntity)
		{
			return;
		}
		float num = _ep.position.x - (float)(this.encodedPos.x / 32);
		float num2 = _ep.position.z - (float)(this.encodedPos.z / 32);
		if (num * num + num2 * num2 <= (float)(this.trackingDistanceThreshold * this.trackingDistanceThreshold))
		{
			if (!this.trackedPlayers.Contains(_ep))
			{
				this.trackedPlayers.Add(_ep);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this.getSpawnPacket(), false, _ep.entityId, -1, -1, null, 192, false);
				EntityAlive entityAlive = this.trackedEntity as EntityAlive;
				if (entityAlive)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup(entityAlive), false, _ep.entityId, -1, -1, null, 192, false);
					if (entityAlive is EntityPlayer)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerStats>().Setup(entityAlive), false, _ep.entityId, -1, -1, null, 192, false);
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerTwitchStats>().Setup(entityAlive), false, _ep.entityId, -1, -1, null, 192, false);
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerEquipment>().Setup(entityAlive), false, _ep.entityId, -1, -1, null, 192, false);
					}
				}
				EModelBase emodel = this.trackedEntity.emodel;
				if (emodel != null)
				{
					AvatarController avatarController = emodel.avatarController;
					if (avatarController != null)
					{
						avatarController.SyncAnimParameters(_ep.entityId);
					}
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntitySpeeds>().Setup(this.trackedEntity), false, _ep.entityId, -1, -1, null, 192, false);
				if (this.shouldSendMotionUpdates)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityVelocity>().Setup(this.trackedEntity.entityId, this.trackedEntity.motion, false), false, _ep.entityId, -1, -1, null, 192, false);
					return;
				}
			}
		}
		else if (this.trackedPlayers.Contains(_ep))
		{
			this.trackedPlayers.Remove(_ep);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityRemove>().Setup(this.trackedEntity.entityId, EnumRemoveEntityReason.Unloaded), false, _ep.entityId, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x001794D8 File Offset: 0x001776D8
	public void updatePlayerEntities(List<EntityPlayer> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			EntityPlayer entityPlayer = _list[i];
			this.updatePlayerEntity(entityPlayer);
		}
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x00179505 File Offset: 0x00177705
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackage getSpawnPacket()
	{
		return NetPackageManager.GetPackage<NetPackageEntitySpawn>().Setup(new EntityCreationData(this.trackedEntity, true));
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x00179520 File Offset: 0x00177720
	public void updatePlayerList(List<EntityPlayer> _playerList)
	{
		if (!this.firstUpdateDone || this.trackedEntity.GetDistanceSq(this.lastTrackedEntityPos) > 16f)
		{
			this.lastTrackedEntityPos = this.trackedEntity.position;
			this.firstUpdateDone = true;
			this.updatePlayerEntities(_playerList);
		}
		if (this.trackedEntity.usePhysicsMaster)
		{
			if (this.trackedEntity.isPhysicsMaster)
			{
				int num = this.updateCounter;
				this.updateCounter = num + 1;
				if (num % this.updatTickCounter == 0)
				{
					NetPackageEntityPhysics netPackageEntityPhysics = this.trackedEntity.PhysicsMasterSetupBroadcast();
					if (netPackageEntityPhysics != null)
					{
						this.SendToPlayers(netPackageEntityPhysics, -1, false, 192);
					}
				}
			}
			return;
		}
		this.sendFullUpdateAfterTicks++;
		bool flag = this.priorityLevel == 0 || this.trackedEntity.IsAirBorne();
		int num2 = ((this.priorityLevel == 0) ? 1 : 3);
		this.updateCounter++;
		if (!flag)
		{
			switch (this.priorityLevel)
			{
			case 1:
				flag = this.updateCounter % this.updatTickCounter == 0;
				num2 = 3;
				break;
			case 2:
				flag = this.updateCounter % 6 == 0;
				num2 = 6;
				break;
			case 3:
				flag = this.updateCounter % 10 == 0;
				num2 = 10;
				break;
			}
		}
		if (flag)
		{
			Vector3i vector3i = NetEntityDistributionEntry.EncodePos(this.trackedEntity.position);
			Vector3i vector3i2 = vector3i - this.encodedPos;
			bool flag2 = Utils.FastAbs((float)vector3i2.x) >= 2f || Utils.FastAbs((float)vector3i2.y) >= 2f || Utils.FastAbs((float)vector3i2.z) >= 2f || this.encodedOnGround != this.trackedEntity.onGround;
			Vector3i vector3i3 = NetEntityDistributionEntry.EncodeRot(this.trackedEntity.rotation);
			Vector3i vector3i4 = vector3i3 - this.encodedRot;
			bool flag3 = Utils.FastAbs((float)vector3i4.x) >= 2f || Utils.FastAbs((float)vector3i4.y) >= 2f || Utils.FastAbs((float)vector3i4.z) >= 2f;
			NetPackage netPackage = null;
			bool flag4 = false;
			if (this.trackedEntity.IsMovementReplicated)
			{
				if (this.updatTickCounter == 1)
				{
					this.sendFullUpdateAfterTicks = int.MaxValue;
					flag4 = true;
				}
				if (vector3i2.x < -256 || vector3i2.x >= 256 || vector3i2.y < -256 || vector3i2.y >= 256 || vector3i2.z < -256 || vector3i2.z >= 256)
				{
					this.sendFullUpdateAfterTicks = 0;
					netPackage = NetPackageManager.GetPackage<NetPackageEntityTeleport>().Setup(this.trackedEntity);
				}
				else if (vector3i2.x < -128 || vector3i2.x >= 128 || vector3i2.y < -128 || vector3i2.y >= 128 || vector3i2.z < -128 || vector3i2.z >= 128 || this.sendFullUpdateAfterTicks > 100)
				{
					this.sendFullUpdateAfterTicks = 0;
					netPackage = NetPackageManager.GetPackage<NetPackageEntityPosAndRot>().Setup(this.trackedEntity);
				}
				else if (flag2 && flag3)
				{
					netPackage = NetPackageManager.GetPackage<NetPackageEntityRelPosAndRot>().Setup(this.trackedEntity.entityId, vector3i2, vector3i3, this.trackedEntity.qrotation, this.trackedEntity.onGround, this.trackedEntity.IsQRotationUsed(), num2);
					flag4 = true;
				}
				else if (flag2)
				{
					netPackage = NetPackageManager.GetPackage<NetPackageEntityRelPosAndRot>().Setup(this.trackedEntity.entityId, vector3i2, vector3i3, this.trackedEntity.qrotation, this.trackedEntity.onGround, this.trackedEntity.IsQRotationUsed(), num2);
					flag4 = true;
				}
				else if (flag3)
				{
					netPackage = NetPackageManager.GetPackage<NetPackageEntityRotation>().Setup(this.trackedEntity.entityId, vector3i3, this.trackedEntity.qrotation, this.trackedEntity.IsQRotationUsed());
					flag4 = true;
				}
			}
			if (this.shouldSendMotionUpdates)
			{
				float sqrMagnitude = (this.trackedEntity.motion - this.lastTrackedEntityMotion).sqrMagnitude;
				if (sqrMagnitude > 0.040000003f || (sqrMagnitude > 0f && this.trackedEntity.motion.Equals(Vector3.zero)))
				{
					this.lastTrackedEntityMotion = this.trackedEntity.motion;
					this.SendToPlayers(NetPackageManager.GetPackage<NetPackageEntityVelocity>().Setup(this.trackedEntity.entityId, this.lastTrackedEntityMotion, false), -1, false, 192);
				}
			}
			if (netPackage != null)
			{
				this.SendToPlayers(netPackage, -1, flag4, this.trackingDistanceThreshold);
			}
			EntityAlive entityAlive = this.trackedEntity as EntityAlive;
			if (entityAlive != null && entityAlive.bEntityAliveFlagsChanged)
			{
				this.SendToPlayers(NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup(entityAlive), this.trackedEntity.entityId, false, 192);
				entityAlive.bEntityAliveFlagsChanged = false;
			}
			EntityPlayer entityPlayer = this.trackedEntity as EntityPlayer;
			if (entityPlayer != null && entityPlayer.bPlayerStatsChanged)
			{
				this.SendToPlayers(NetPackageManager.GetPackage<NetPackagePlayerStats>().Setup(entityAlive), this.trackedEntity.entityId, false, 192);
				entityAlive.bPlayerStatsChanged = false;
			}
			if (entityPlayer != null && entityPlayer.bPlayerTwitchChanged)
			{
				this.SendToPlayers(NetPackageManager.GetPackage<NetPackagePlayerTwitchStats>().Setup(entityAlive), this.trackedEntity.entityId, false, 192);
				entityAlive.bPlayerTwitchChanged = false;
			}
			if (entityPlayer != null && entityPlayer.bPlayerEquipmentChanged)
			{
				this.SendToPlayers(NetPackageManager.GetPackage<NetPackagePlayerEquipment>().Setup(entityAlive), this.trackedEntity.entityId, false, 192);
				entityAlive.bPlayerEquipmentChanged = false;
			}
			if (flag2)
			{
				this.encodedPos = vector3i;
				this.encodedOnGround = this.trackedEntity.onGround;
			}
			if (flag3)
			{
				this.encodedRot = vector3i3;
			}
		}
		this.trackedEntity.SetAirBorne(false);
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x00179AF8 File Offset: 0x00177CF8
	public void SendFullUpdateNextTick()
	{
		this.sendFullUpdateAfterTicks = 100;
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x00179B02 File Offset: 0x00177D02
	public static Vector3i EncodePos(Vector3 _pos)
	{
		return new Vector3i(_pos.x * 32f + 0.5f, _pos.y * 32f + 0.5f, _pos.z * 32f + 0.5f);
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x00179B3F File Offset: 0x00177D3F
	public static Vector3i EncodeRot(Vector3 _rot)
	{
		return new Vector3i(_rot * 256f / 360f);
	}

	// Token: 0x04002F71 RID: 12145
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxTrackingDistance = 46340;

	// Token: 0x04002F72 RID: 12146
	public Entity trackedEntity;

	// Token: 0x04002F73 RID: 12147
	public int updatTickCounter;

	// Token: 0x04002F74 RID: 12148
	public Vector3i encodedPos;

	// Token: 0x04002F75 RID: 12149
	public Vector3i encodedRot;

	// Token: 0x04002F76 RID: 12150
	public bool encodedOnGround;

	// Token: 0x04002F77 RID: 12151
	public Vector3 lastTrackedEntityMotion;

	// Token: 0x04002F78 RID: 12152
	public int updateCounter;

	// Token: 0x04002F79 RID: 12153
	public HashSet<EntityPlayer> trackedPlayers;

	// Token: 0x04002F7A RID: 12154
	[PublicizedFrom(EAccessModifier.Private)]
	public int trackingDistanceThreshold;

	// Token: 0x04002F7B RID: 12155
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastTrackedEntityPos;

	// Token: 0x04002F7C RID: 12156
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdateDone;

	// Token: 0x04002F7D RID: 12157
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldSendMotionUpdates;

	// Token: 0x04002F7E RID: 12158
	[PublicizedFrom(EAccessModifier.Private)]
	public int sendFullUpdateAfterTicks;

	// Token: 0x04002F7F RID: 12159
	public const int cFullUpdateAfterTicks = 100;

	// Token: 0x04002F80 RID: 12160
	public int priorityLevel = 1;
}
