using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000518 RID: 1304
[Preserve]
public class EModelSDCS : EModelPlayer
{
	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002ADF RID: 10975 RVA: 0x0010F3AB File Offset: 0x0010D5AB
	public bool isMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.archetype == null || this.archetype.Sex == "Male";
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x0010F3CC File Offset: 0x0010D5CC
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

	// Token: 0x06002AE1 RID: 10977 RVA: 0x0010F3F7 File Offset: 0x0010D5F7
	public void Awake()
	{
		this.playerEntity = base.transform.GetComponent<EntityPlayerLocal>();
		if (this.playerEntity == null)
		{
			this.playerEntity = base.transform.GetComponent<EntityPlayer>();
		}
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x0010F42C File Offset: 0x0010D62C
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

	// Token: 0x06002AE3 RID: 10979 RVA: 0x0010F4F8 File Offset: 0x0010D6F8
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnDestroy()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x0010F50B File Offset: 0x0010D70B
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment playerEquipment)
	{
		if (playerEquipment.Equipment == this.playerEntity.equipment)
		{
			this.UpdateEquipment();
		}
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x0010F528 File Offset: 0x0010D728
	public void UpdateEquipment()
	{
		Animator animator = this.avatarController.GetAnimator();
		if (animator && (base.IsFPV || animator.enabled))
		{
			this.GenerateMeshes();
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x0010F560 File Offset: 0x0010D760
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

	// Token: 0x06002AE7 RID: 10983 RVA: 0x0010F5A0 File Offset: 0x0010D7A0
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

	// Token: 0x06002AE8 RID: 10984 RVA: 0x0010F624 File Offset: 0x0010D824
	public override void SwitchModelAndView(bool _bFPV, bool _isMale)
	{
		base.IsFPV = _bFPV;
		this.playerEntity.IsMale = this.isMale;
		this.GenerateMeshes();
		base.SwitchModelAndView(base.IsFPV, _isMale);
		this.meshTransform = this.modelTransform.FindInChildren("Spine1");
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x0010F674 File Offset: 0x0010D874
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

	// Token: 0x06002AEA RID: 10986 RVA: 0x0010F788 File Offset: 0x0010D988
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

	// Token: 0x06002AEB RID: 10987 RVA: 0x0010F7FF File Offset: 0x0010D9FF
	public override Transform GetHeadTransform()
	{
		if (this.headT == null && this.boneCatalog.ContainsKey("Head"))
		{
			this.headT = this.boneCatalog["Head"];
		}
		return this.headT;
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x0010F83D File Offset: 0x0010DA3D
	public override Transform GetPelvisTransform()
	{
		if (this.bipedPelvisTransform == null && this.boneCatalog.ContainsKey("Hips"))
		{
			this.bipedPelvisTransform = this.boneCatalog["Hips"];
		}
		return this.bipedPelvisTransform;
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06002AED RID: 10989 RVA: 0x0010F87B File Offset: 0x0010DA7B
	public Archetype Archetype
	{
		get
		{
			return this.archetype;
		}
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x0010F883 File Offset: 0x0010DA83
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetRace(string value)
	{
		this.archetype.Race = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x0010F8B0 File Offset: 0x0010DAB0
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetVariant(int value)
	{
		this.archetype.Variant = (int)((byte)value);
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x0010F8DE File Offset: 0x0010DADE
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSex(bool value)
	{
		this.archetype.IsMale = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x0010F90C File Offset: 0x0010DB0C
	public override void SetVisible(bool _bVisible, bool _isKeepColliders = false)
	{
		bool visible = base.visible;
		if (_bVisible != visible)
		{
			SDCSUtils.SetVisible(this.baseRig, _bVisible);
		}
		base.SetVisible(_bVisible, _isKeepColliders);
	}

	// Token: 0x040020C3 RID: 8387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityClass entityClass;

	// Token: 0x040020C4 RID: 8388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayer playerEntity;

	// Token: 0x040020C5 RID: 8389
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRig;

	// Token: 0x040020C6 RID: 8390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalog;

	// Token: 0x040020C7 RID: 8391
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRigFP;

	// Token: 0x040020C8 RID: 8392
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalogFP;

	// Token: 0x040020C9 RID: 8393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform playerModelTransform;

	// Token: 0x040020CA RID: 8394
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headT;

	// Token: 0x040020CB RID: 8395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headTransFP;

	// Token: 0x040020CC RID: 8396
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UpdateLightOnPlayers updateLightScript;

	// Token: 0x040020CD RID: 8397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Archetype archetype;

	// Token: 0x040020CE RID: 8398
	public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

	// Token: 0x040020CF RID: 8399
	public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

	// Token: 0x040020D0 RID: 8400
	public List<Material> ClipMaterialsFP = new List<Material>();
}
