using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000513 RID: 1299
[Preserve]
public class EModelSDCS : EModelPlayer
{
	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x0010E36F File Offset: 0x0010C56F
	public bool isMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.archetype == null || this.archetype.Sex == "Male";
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x0010E390 File Offset: 0x0010C590
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

	// Token: 0x06002AB7 RID: 10935 RVA: 0x0010E3BB File Offset: 0x0010C5BB
	public void Awake()
	{
		this.playerEntity = base.transform.GetComponent<EntityPlayerLocal>();
		if (this.playerEntity == null)
		{
			this.playerEntity = base.transform.GetComponent<EntityPlayer>();
		}
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x0010E3F0 File Offset: 0x0010C5F0
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

	// Token: 0x06002AB9 RID: 10937 RVA: 0x0010E4BC File Offset: 0x0010C6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnDestroy()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x0010E4CF File Offset: 0x0010C6CF
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment playerEquipment)
	{
		if (playerEquipment.Equipment == this.playerEntity.equipment)
		{
			this.UpdateEquipment();
		}
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x0010E4EC File Offset: 0x0010C6EC
	public void UpdateEquipment()
	{
		Animator animator = this.avatarController.GetAnimator();
		if (animator && (base.IsFPV || animator.enabled))
		{
			this.GenerateMeshes();
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06002ABC RID: 10940 RVA: 0x0010E524 File Offset: 0x0010C724
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

	// Token: 0x06002ABD RID: 10941 RVA: 0x0010E564 File Offset: 0x0010C764
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

	// Token: 0x06002ABE RID: 10942 RVA: 0x0010E5E8 File Offset: 0x0010C7E8
	public override void SwitchModelAndView(bool _bFPV, bool _isMale)
	{
		base.IsFPV = _bFPV;
		this.playerEntity.IsMale = this.isMale;
		this.GenerateMeshes();
		base.SwitchModelAndView(base.IsFPV, _isMale);
		this.meshTransform = this.modelTransform.FindInChildren("Spine1");
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x0010E638 File Offset: 0x0010C838
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

	// Token: 0x06002AC0 RID: 10944 RVA: 0x0010E74C File Offset: 0x0010C94C
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

	// Token: 0x06002AC1 RID: 10945 RVA: 0x0010E7C3 File Offset: 0x0010C9C3
	public override Transform GetHeadTransform()
	{
		if (this.headT == null && this.boneCatalog.ContainsKey("Head"))
		{
			this.headT = this.boneCatalog["Head"];
		}
		return this.headT;
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x0010E801 File Offset: 0x0010CA01
	public override Transform GetPelvisTransform()
	{
		if (this.bipedPelvisTransform == null && this.boneCatalog.ContainsKey("Hips"))
		{
			this.bipedPelvisTransform = this.boneCatalog["Hips"];
		}
		return this.bipedPelvisTransform;
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x0010E83F File Offset: 0x0010CA3F
	public Archetype Archetype
	{
		get
		{
			return this.archetype;
		}
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x0010E847 File Offset: 0x0010CA47
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetRace(string value)
	{
		this.archetype.Race = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x0010E874 File Offset: 0x0010CA74
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetVariant(int value)
	{
		this.archetype.Variant = (int)((byte)value);
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x0010E8A2 File Offset: 0x0010CAA2
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSex(bool value)
	{
		this.archetype.IsMale = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x0010E8D0 File Offset: 0x0010CAD0
	public override void SetVisible(bool _bVisible, bool _isKeepColliders = false)
	{
		bool visible = base.visible;
		if (_bVisible != visible)
		{
			SDCSUtils.SetVisible(this.baseRig, _bVisible);
		}
		base.SetVisible(_bVisible, _isKeepColliders);
	}

	// Token: 0x04002094 RID: 8340
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityClass entityClass;

	// Token: 0x04002095 RID: 8341
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayer playerEntity;

	// Token: 0x04002096 RID: 8342
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRig;

	// Token: 0x04002097 RID: 8343
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalog;

	// Token: 0x04002098 RID: 8344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRigFP;

	// Token: 0x04002099 RID: 8345
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalogFP;

	// Token: 0x0400209A RID: 8346
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform playerModelTransform;

	// Token: 0x0400209B RID: 8347
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headT;

	// Token: 0x0400209C RID: 8348
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headTransFP;

	// Token: 0x0400209D RID: 8349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UpdateLightOnPlayers updateLightScript;

	// Token: 0x0400209E RID: 8350
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Archetype archetype;

	// Token: 0x0400209F RID: 8351
	public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

	// Token: 0x040020A0 RID: 8352
	public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

	// Token: 0x040020A1 RID: 8353
	public List<Material> ClipMaterialsFP = new List<Material>();
}
