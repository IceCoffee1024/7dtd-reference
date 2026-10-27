using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class MotionSensorController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000B42F6 File Offset: 0x000B24F6
	// (set) Token: 0x06001D93 RID: 7571 RVA: 0x000B42FE File Offset: 0x000B24FE
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x06001D94 RID: 7572 RVA: 0x000B4307 File Offset: 0x000B2507
	public void OnDestroy()
	{
		this.Cleanup();
		if (this.ConeMaterial != null)
		{
			global::UnityEngine.Object.Destroy(this.ConeMaterial);
		}
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x000B4328 File Offset: 0x000B2528
	public void Init(DynamicProperties _properties)
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		if (_properties.Values.ContainsKey("MaxDistance"))
		{
			this.maxDistance = StringParsers.ParseFloat(_properties.Values["MaxDistance"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.maxDistance = 16f;
		}
		if (_properties.Values.ContainsKey("YawRange"))
		{
			float num = StringParsers.ParseFloat(_properties.Values["YawRange"], 0, -1, NumberStyles.Any);
			num *= 0.5f;
			this.yawRange = new Vector2(-num, num);
		}
		else
		{
			this.yawRange = new Vector2(-22.5f, 22.5f);
		}
		if (_properties.Values.ContainsKey("PitchRange"))
		{
			float num2 = StringParsers.ParseFloat(_properties.Values["PitchRange"], 0, -1, NumberStyles.Any);
			num2 *= 0.5f;
			this.pitchRange = new Vector2(-num2, num2);
		}
		else
		{
			this.pitchRange = new Vector2(-22.5f, 22.5f);
		}
		if (_properties.Values.ContainsKey("FallAsleepTime"))
		{
			this.fallAsleepTimeMax = StringParsers.ParseFloat(_properties.Values["FallAsleepTime"], 0, -1, NumberStyles.Any);
		}
		this.Cone.localScale = new Vector3(this.Cone.localScale.x * (this.yawRange.y / 45f) * (this.maxDistance / 4f), this.Cone.localScale.y * (this.pitchRange.y / 45f) * (this.maxDistance / 4f), this.Cone.localScale.z * (this.maxDistance / 4f));
		this.Cone.gameObject.SetActive(false);
		WireManager.Instance.AddPulseObject(this.Cone.gameObject);
		this.targetingBounds = this.Cone.GetComponent<MeshRenderer>().bounds;
		this.YawController.BaseRotation = new Vector3(-90f, 0f, 0f);
		this.PitchController.BaseRotation = new Vector3(0f, 0f, 0f);
		if (this.Cone != null)
		{
			MeshRenderer component = this.Cone.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					this.ConeMaterial = component.material;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
					return;
				}
				if (component.sharedMaterial != null)
				{
					this.ConeMaterial = component.sharedMaterial;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
			}
		}
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x000B45FC File Offset: 0x000B27FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.TileEntity == null)
		{
			return;
		}
		if (!this.TileEntity.IsPowered || this.UserAccessingId != -1)
		{
			if (this.UserAccessingId != -1)
			{
				this.YawController.Yaw = this.TileEntity.CenteredYaw;
				this.YawController.UpdateYaw();
				this.PitchController.Pitch = this.TileEntity.CenteredPitch;
				this.PitchController.UpdatePitch();
				return;
			}
			if (!this.TileEntity.IsPowered)
			{
				if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
				{
					this.YawController.Yaw = this.TileEntity.CenteredYaw;
					this.YawController.SetYaw();
				}
				if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
				{
					this.PitchController.Pitch = this.TileEntity.CenteredPitch;
					this.PitchController.SetPitch();
				}
			}
			return;
		}
		else
		{
			if (this.TileEntity.IsPowered)
			{
				bool flag = this.hasTarget();
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					PowerTrigger powerTrigger = (PowerTrigger)this.TileEntity.GetPowerItem();
					if (flag)
					{
						this.TileEntity.IsTriggered = true;
					}
				}
				this.YawController.UpdateYaw();
				this.PitchController.UpdatePitch();
				this.UpdateEmissionColor(flag);
				return;
			}
			this.UpdateEmissionColor(false);
			return;
		}
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x000B475C File Offset: 0x000B295C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEmissionColor(bool isTriggered)
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].material != componentsInChildren[i].sharedMaterial)
				{
					componentsInChildren[i].material = new Material(componentsInChildren[i].sharedMaterial);
				}
				if (this.TileEntity.IsPowered)
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", isTriggered ? Color.green : Color.red);
				}
				else
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.black);
				}
				componentsInChildren[i].sharedMaterial = componentsInChildren[i].material;
			}
		}
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x000B4814 File Offset: 0x000B2A14
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasTarget()
	{
		List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityAlive), new Bounds(this.TileEntity.ToWorldPos().ToVector3(), Vector3.one * (this.maxDistance * 2f)), new List<Entity>());
		if (entitiesInBounds.Count > 0)
		{
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				if (!this.shouldIgnoreTarget(entitiesInBounds[i]))
				{
					Vector3 zero = Vector3.zero;
					float centeredYaw = this.TileEntity.CenteredYaw;
					float centeredPitch = this.TileEntity.CenteredPitch;
					if (this.trackTarget(entitiesInBounds[i], ref centeredYaw, ref centeredPitch, out zero))
					{
						Ray ray = new Ray(this.Cone.transform.position + Origin.position, (zero - this.Cone.transform.position).normalized);
						if (Voxel.Raycast(GameManager.Instance.World, ray, this.maxDistance, -538750989, 8, 0.1f) && Voxel.voxelRayHitInfo.tag.StartsWith("E_"))
						{
							if (Voxel.voxelRayHitInfo.tag == "E_Vehicle")
							{
								EntityVehicle entityVehicle = EntityVehicle.FindCollisionEntity(Voxel.voxelRayHitInfo.transform);
								if (entityVehicle != null && entityVehicle.IsAttached(entitiesInBounds[i]))
								{
									this.YawController.Yaw = centeredYaw;
									this.PitchController.Pitch = centeredPitch;
									return true;
								}
							}
							else
							{
								Transform hitRootTransform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, Voxel.voxelRayHitInfo.transform);
								if (!(hitRootTransform == null) && hitRootTransform.GetComponent<Entity>() == entitiesInBounds[i])
								{
									this.YawController.Yaw = centeredYaw;
									this.PitchController.Pitch = centeredPitch;
									return true;
								}
							}
						}
					}
				}
			}
		}
		this.YawController.Yaw = this.TileEntity.CenteredYaw;
		this.PitchController.Pitch = this.TileEntity.CenteredPitch;
		return false;
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000B4A40 File Offset: 0x000B2C40
	[PublicizedFrom(EAccessModifier.Private)]
	public bool trackTarget(Entity _target, ref float _yaw, ref float _pitch, out Vector3 _targetPos)
	{
		Vector3 vector = _target.getHeadPosition();
		if (vector == Vector3.zero)
		{
			vector = _target.position;
		}
		Vector3 vector2 = (_target.position + vector) * 0.5f;
		_targetPos = Vector3.Lerp(vector2, vector, 0.75f);
		EntityAlive entityAlive = _target as EntityAlive;
		if (entityAlive && entityAlive.GetWalkType() == 21)
		{
			_targetPos = vector2;
		}
		_targetPos -= Origin.position;
		Vector3 normalized = (_targetPos - this.YawController.transform.position).normalized;
		Vector3 normalized2 = (_targetPos - this.PitchController.transform.position).normalized;
		float num = Quaternion.LookRotation(normalized).eulerAngles.y - base.transform.rotation.eulerAngles.y;
		float num2 = Quaternion.LookRotation(normalized2).eulerAngles.x - base.transform.rotation.z;
		if (num > 180f)
		{
			num -= 360f;
		}
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		float num3 = this.TileEntity.CenteredYaw % 360f;
		float num4 = this.TileEntity.CenteredPitch % 360f;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		if (num4 > 180f)
		{
			num4 -= 360f;
		}
		if (num < num3 + this.yawRange.x || num > num3 + this.yawRange.y || num2 < num4 + this.pitchRange.x || num2 > num4 + this.pitchRange.y)
		{
			if (this.fallAsleepTime >= this.fallAsleepTimeMax)
			{
				this.YawController.Yaw = this.TileEntity.CenteredYaw;
				this.PitchController.Pitch = this.TileEntity.CenteredPitch;
				this.fallAsleepTime = 0f;
			}
			else
			{
				this.fallAsleepTime += Time.deltaTime;
			}
			return false;
		}
		_yaw = num;
		_pitch = num2;
		return true;
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000B4C8C File Offset: 0x000B2E8C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldIgnoreTarget(Entity _target)
	{
		if (Vector3.Dot(_target.position - this.TileEntity.ToWorldPos().ToVector3(), this.Cone.transform.forward) > 0f)
		{
			return true;
		}
		if (!_target.IsAlive())
		{
			return true;
		}
		if (_target is EntityVehicle)
		{
			Entity attachedMainEntity = (_target as EntityVehicle).AttachedMainEntity;
			if (attachedMainEntity == null)
			{
				return true;
			}
			_target = attachedMainEntity;
		}
		if (_target is EntityPlayer)
		{
			bool flag = false;
			bool flag2 = false;
			PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
			if (persistentPlayerList != null && persistentPlayerList.EntityToPlayerMap.ContainsKey(_target.entityId) && this.TileEntity.IsOwner(persistentPlayerList.EntityToPlayerMap[_target.entityId].PrimaryId))
			{
				flag = true;
			}
			if (!flag)
			{
				PersistentPlayerData playerData = persistentPlayerList.GetPlayerData(this.TileEntity.GetOwner());
				if (playerData != null && persistentPlayerList.EntityToPlayerMap.ContainsKey(_target.entityId))
				{
					PersistentPlayerData persistentPlayerData = persistentPlayerList.EntityToPlayerMap[_target.entityId];
					if (playerData.IsAlly(persistentPlayerData))
					{
						flag2 = true;
					}
				}
			}
			if (flag && !this.TileEntity.TargetSelf)
			{
				return true;
			}
			if (flag2 && !this.TileEntity.TargetAllies)
			{
				return true;
			}
			if (!flag && !flag2 && !this.TileEntity.TargetStrangers)
			{
				return true;
			}
		}
		if (_target is EntityNPC)
		{
			if (!this.TileEntity.TargetStrangers)
			{
				return true;
			}
			if (_target is EntityDrone)
			{
				return true;
			}
		}
		return (_target is EntityEnemy && !this.TileEntity.TargetZombies) || (_target is EntityAnimal && !_target.EntityClass.bIsEnemyEntity);
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000B4E33 File Offset: 0x000B3033
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x000B4E41 File Offset: 0x000B3041
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x000B4E4F File Offset: 0x000B304F
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000B4E5C File Offset: 0x000B305C
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x000B4E69 File Offset: 0x000B3069
	public Transform GetCameraTransform()
	{
		return this.Cone;
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x000B4E71 File Offset: 0x000B3071
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x000B4E7A File Offset: 0x000B307A
	public void Cleanup()
	{
		if (this.Cone != null && WireManager.HasInstance)
		{
			WireManager.Instance.RemovePulseObject(this.Cone.gameObject);
		}
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000B4EA7 File Offset: 0x000B30A7
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000B4EC8 File Offset: 0x000B30C8
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000B4ED0 File Offset: 0x000B30D0
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000B4EF1 File Offset: 0x000B30F1
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x000B4F13 File Offset: 0x000B3113
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool HasLaser()
	{
		return false;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x000AF7B9 File Offset: 0x000AD9B9
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserActive(bool _active)
	{
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool GetLaserActive()
	{
		return false;
	}

	// Token: 0x040013B6 RID: 5046
	public AutoTurretYawLerp YawController;

	// Token: 0x040013B7 RID: 5047
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040013B8 RID: 5048
	public Transform Cone;

	// Token: 0x040013B9 RID: 5049
	public Material ConeMaterial;

	// Token: 0x040013BA RID: 5050
	public Color ConeColor;

	// Token: 0x040013BB RID: 5051
	public bool IsOn;

	// Token: 0x040013BC RID: 5052
	public TileEntityPoweredTrigger TileEntity;

	// Token: 0x040013BE RID: 5054
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float baseConeYaw = 45f;

	// Token: 0x040013BF RID: 5055
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float baseConePitch = 45f;

	// Token: 0x040013C0 RID: 5056
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float baseConeDistance = 4f;

	// Token: 0x040013C1 RID: 5057
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxDistance;

	// Token: 0x040013C2 RID: 5058
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 yawRange;

	// Token: 0x040013C3 RID: 5059
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 pitchRange;

	// Token: 0x040013C4 RID: 5060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Bounds targetingBounds;

	// Token: 0x040013C5 RID: 5061
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fallAsleepTimeMax = 1f;

	// Token: 0x040013C6 RID: 5062
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fallAsleepTime;

	// Token: 0x040013C7 RID: 5063
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;
}
