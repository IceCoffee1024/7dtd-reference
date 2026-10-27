using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000520 RID: 1312
[Preserve]
public class EModelSDCS : EModelPlayer
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002ADB RID: 10971 RVA: 0x001101E3 File Offset: 0x0010E3E3
	public bool isMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.archetype == null || this.archetype.Sex == "Male";
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002ADC RID: 10972 RVA: 0x00110204 File Offset: 0x0010E404
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

	// Token: 0x06002ADD RID: 10973 RVA: 0x0011022F File Offset: 0x0010E42F
	public void Awake()
	{
		this.playerEntity = base.transform.GetComponent<EntityPlayerLocal>();
		if (this.playerEntity == null)
		{
			this.playerEntity = base.transform.GetComponent<EntityPlayer>();
		}
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x00110264 File Offset: 0x0010E464
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

	// Token: 0x06002ADF RID: 10975 RVA: 0x00110330 File Offset: 0x0010E530
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnDestroy()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x00110343 File Offset: 0x0010E543
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment playerEquipment)
	{
		if (playerEquipment.Equipment == this.playerEntity.equipment)
		{
			this.UpdateEquipment();
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x00110360 File Offset: 0x0010E560
	public void UpdateEquipment()
	{
		Animator animator = this.avatarController.GetAnimator();
		if (animator && (base.IsFPV || animator.enabled))
		{
			this.GenerateMeshes();
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x00110398 File Offset: 0x0010E598
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

	// Token: 0x06002AE3 RID: 10979 RVA: 0x001103D8 File Offset: 0x0010E5D8
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

	// Token: 0x06002AE4 RID: 10980 RVA: 0x0011045C File Offset: 0x0010E65C
	public override void SwitchModelAndView(bool _bFPV, bool _isMale)
	{
		base.IsFPV = _bFPV;
		this.playerEntity.IsMale = this.isMale;
		this.GenerateMeshes();
		base.SwitchModelAndView(base.IsFPV, _isMale);
		this.meshTransform = this.modelTransform.FindInChildren("Spine1");
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x001104AC File Offset: 0x0010E6AC
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

	// Token: 0x06002AE6 RID: 10982 RVA: 0x001105C0 File Offset: 0x0010E7C0
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

	// Token: 0x06002AE7 RID: 10983 RVA: 0x00110637 File Offset: 0x0010E837
	public override Transform GetHeadTransform()
	{
		if (this.headT == null && this.boneCatalog.ContainsKey("Head"))
		{
			this.headT = this.boneCatalog["Head"];
		}
		return this.headT;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x00110675 File Offset: 0x0010E875
	public override Transform GetPelvisTransform()
	{
		if (this.bipedPelvisTransform == null && this.boneCatalog.ContainsKey("Hips"))
		{
			this.bipedPelvisTransform = this.boneCatalog["Hips"];
		}
		return this.bipedPelvisTransform;
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x001106B3 File Offset: 0x0010E8B3
	public Archetype Archetype
	{
		get
		{
			return this.archetype;
		}
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x001106BB File Offset: 0x0010E8BB
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetRace(string value)
	{
		this.archetype.Race = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x001106E8 File Offset: 0x0010E8E8
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetVariant(int value)
	{
		this.archetype.Variant = (int)((byte)value);
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x00110716 File Offset: 0x0010E916
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSex(bool value)
	{
		this.archetype.IsMale = value;
		if (SDCSUtils.BasePartsExist(this.archetype))
		{
			this.SwitchModelAndView(base.IsFPV, this.isMale);
		}
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x00110744 File Offset: 0x0010E944
	public override void SetVisible(bool _bVisible, bool _isKeepColliders = false)
	{
		bool visible = base.visible;
		if (_bVisible != visible)
		{
			SDCSUtils.SetVisible(this.baseRig, _bVisible);
		}
		base.SetVisible(_bVisible, _isKeepColliders);
	}

	// Token: 0x040020DC RID: 8412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityClass entityClass;

	// Token: 0x040020DD RID: 8413
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayer playerEntity;

	// Token: 0x040020DE RID: 8414
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRig;

	// Token: 0x040020DF RID: 8415
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalog;

	// Token: 0x040020E0 RID: 8416
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject baseRigFP;

	// Token: 0x040020E1 RID: 8417
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SDCSUtils.TransformCatalog boneCatalogFP;

	// Token: 0x040020E2 RID: 8418
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform playerModelTransform;

	// Token: 0x040020E3 RID: 8419
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headT;

	// Token: 0x040020E4 RID: 8420
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headTransFP;

	// Token: 0x040020E5 RID: 8421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UpdateLightOnPlayers updateLightScript;

	// Token: 0x040020E6 RID: 8422
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Archetype archetype;

	// Token: 0x040020E7 RID: 8423
	public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

	// Token: 0x040020E8 RID: 8424
	public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

	// Token: 0x040020E9 RID: 8425
	public List<Material> ClipMaterialsFP = new List<Material>();
}
