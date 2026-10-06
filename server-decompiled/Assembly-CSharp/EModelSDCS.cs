using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200051E RID: 1310
[Preserve]
public class EModelSDCS : EModelPlayer
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x0010FC6F File Offset: 0x0010DE6F
	public bool isMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.archetype == null || this.archetype.Sex == "Male";
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x0010FC90 File Offset: 0x0010DE90
	public override Transform NeckTransform
	{
		get
		{
			if (!base.IsFPV)
			{
				return this.boneCatalog["Neck"];
			}
			return this.boneCatalogFP["Neck"];
		}
	}

	// Token: 0x06002AD6 RID: 10966 RVA: 0x0010FCBB File Offset: 0x0010DEBB
	public void Awake()
	{
		this.playerEntity = base.transform.GetComponent<EntityPlayerLocal>();
		if (this.playerEntity == null)
		{
			this.playerEntity = base.transform.GetComponent<EntityPlayer>();
		}
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x0010FCF0 File Offset: 0x0010DEF0
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		this.entity = _entity;
		this.assets = _assets;
		this.entityClass = EntityClass.list[this.entity.entityClass];
		this.archetype = this.playerEntity.playerProfile.CreateTempArchetype();
		this.ragdollChance = this.entityClass.RagdollOnDeathChance;
		this.bHasRagdoll = this.entityClass.HasRagdoll;
		this.modelTransformParent = EModelBase.FindModel(base.transform);
		base.IsFPV = this.entity is EntityPlayerLocal;
		this.createModel(_world, this.entityClass);
		this.createAvatarController(EntityClass.list[this.entity.entityClass]);
		XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x0010FDBC File Offset: 0x0010DFBC
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnDestroy()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x0010FDCF File Offset: 0x0010DFCF
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment playerEquipment)
	{
		if (playerEquipment.Equipment == this.playerEntity.equipment)
		{
			this.UpdateEquipment();
		}
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x0010FDEC File Offset: 0x0010DFEC
	public void UpdateEquipment()
	{
		Animator animator = this.avatarController.GetAnimator();
		if (animator && (base.IsFPV || animator.enabled))
		{
			this.GenerateMeshes();
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002ADB RID: 10971 RVA: 0x0010FE24 File Offset: 0x0010E024
	public Transform HeadTransformFP
	{
		get
		{
			if (this.headTransFP == null && this.boneCatalogFP.ContainsKey("Head"))
			{
				this.headTransFP = this.boneCatalogFP["Head"];
			}
			return this.headTransFP;
		}
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x0010FE64 File Offset: 0x0010E064
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (base.IsFPV && this.HeadTransformFP != null)
		{
			foreach (Material material in this.ClipMaterialsFP)
			{
				material.SetVector("_ClipCenter", this.HeadTransformFP.position);
			}
		}
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x0010FEE8 File Offset: 0x0010E0E8
	public override void SwitchModelAndView(bool _bFPV, bool _isMale)
	{
		base.IsFPV = _bFPV;
		this.playerEntity.IsMale = this.isMale;
		this.GenerateMeshes();
		base.SwitchModelAndView(base.IsFPV, _isMale);
		this.meshTransform = this.modelTransform.FindInChildren("Spine1");
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x0010FF38 File Offset: 0x0010E138
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createModel(World _world, EntityClass _ec)
	{
		if (this.modelTransformParent == null)
		{
			return;
		}
		if (this.playerModelTransform != null)
		{
			global::UnityEngine.Object.Destroy(this.playerModelTransform.gameObject);
		}
		this.playerModelTransform = this.GenerateMeshes();
		this.playerModelTransform.name = (this.modelName = "player_" + this.archetype.Sex + "Ragdoll");
		this.playerModelTransform.tag = "E_BP_Body";
		this.playerModelTransform.SetParent(this.modelTransformParent, false);
		this.playerModelTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		this.playerModelTransform.gameObject.GetOrAddComponent<AnimationEventBridge>();
		this.updateLightScript = this.playerModelTransform.gameObject.GetOrAddComponent<UpdateLightOnPlayers>();
		this.updateLightScript.IsDynamicObject = true;
		EntityAlive entityAlive = this.entity as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.ReassignEquipmentTransforms();
		}
		this.baseRig.transform.FindInChilds("Origin", false).tag = "E_BP_BipedRoot";
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x0011004C File Offset: 0x0010E24C
	public Transform GenerateMeshes()
	{
		SDCSUtils.CreateVizTP(this.archetype, ref this.baseRig, ref this.boneCatalog, this.playerEntity, base.IsFPV);
		if (this.playerEntity as EntityPlayerLocal != null)
		{
			SDCSUtils.CreateVizFP(this.archetype, ref this.baseRigFP, ref this.boneCatalogFP, this.playerEntity, base.IsFPV);
		}
		base.ClothSimInit();
		return this.baseRig.transform;
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x001100C3 File Offset: 0x0010E2C3
	public override Transform GetHeadTransform()
	{
		if (this.headT == null && this.boneCatalog.ContainsKey("Head"))
		{
			this.headT = this.boneCatalog["Head"];
		}
		return this.headT;
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x00110101 File Offset: 0x0010E301
	public override Transform GetPelvisTransform()
	{
		if (this.bipedPelvisTransform == null && this.boneCatalog.ContainsKey("Hips"))
		{
			this.bipedPelvisTransform = this.boneCatalog["Hips"];
		}
		return this.bipedPelvisTransform;
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x0011013F File Offset: 0x0010E33F
	public Archetype Archetype
	{
		get
		{
			return this.archetype;
		}
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x00110147 File Offset: 0x0010E347
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetRace(string value)
	{
		this.archetype.Race = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x00110174 File Offset: 0x0010E374
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetVariant(int value)
	{
		this.archetype.Variant = (int)((byte)value);
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x001101A2 File Offset: 0x0010E3A2
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSex(bool value)
	{
		this.archetype.IsMale = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x001101D0 File Offset: 0x0010E3D0
	public override void SetVisible(bool _bVisible, bool _isKeepColliders = false)
	{
		bool visible = base.visible;
		if (_bVisible != visible)
		{
			SDCSUtils.SetVisible(this.baseRig, _bVisible);
		}
		base.SetVisible(_bVisible, _isKeepColliders);
	}

	// Token: 0x040020CB RID: 8395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityClass entityClass;

	// Token: 0x040020CC RID: 8396
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayer playerEntity;

	// Token: 0x040020CD RID: 8397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRig;

	// Token: 0x040020CE RID: 8398
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalog;

	// Token: 0x040020CF RID: 8399
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRigFP;

	// Token: 0x040020D0 RID: 8400
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalogFP;

	// Token: 0x040020D1 RID: 8401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform playerModelTransform;

	// Token: 0x040020D2 RID: 8402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headT;

	// Token: 0x040020D3 RID: 8403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headTransFP;

	// Token: 0x040020D4 RID: 8404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UpdateLightOnPlayers updateLightScript;

	// Token: 0x040020D5 RID: 8405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Archetype archetype;

	// Token: 0x040020D6 RID: 8406
	public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

	// Token: 0x040020D7 RID: 8407
	public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

	// Token: 0x040020D8 RID: 8408
	public List<Material> ClipMaterialsFP = new List<Material>();
}
