using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

// Token: 0x020004AC RID: 1196
[Preserve]
public class EntityFallingTree : Entity
{
	// Token: 0x060025D4 RID: 9684 RVA: 0x000E74F3 File Offset: 0x000E56F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.treeRB = base.GetComponent<Rigidbody>();
		this.treeRB.useGravity = !this.isEntityRemote;
		this.treeRB.isKinematic = this.isEntityRemote;
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000E752C File Offset: 0x000E572C
	public Vector3i GetBlockPos()
	{
		return this.treeBlockPos;
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000E7534 File Offset: 0x000E5734
	public Vector3 GetFallTreeDir()
	{
		return this.fallTreeDir;
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000E753C File Offset: 0x000E573C
	public void SetBlockPos(Vector3i _blockPos, Vector3 _fallTreeDir)
	{
		this.treeBlockPos = _blockPos;
		this.fallTreeDir = _fallTreeDir;
		if (!this.isEntityRemote)
		{
			base.SetAirBorne(true);
		}
		Chunk chunk = (Chunk)this.world.GetChunkFromWorldPos(this.treeBlockPos);
		if (chunk == null)
		{
			return;
		}
		this.treeBV = chunk.GetBlock(World.toBlock(_blockPos));
		if (DecoManager.Instance.IsEnabled && this.treeBV.Block.IsDistantDecoration)
		{
			this.treeTransform = DecoManager.Instance.GetDecorationTransform(this.treeBlockPos, true);
		}
		else
		{
			BlockEntityData blockEntity = chunk.GetBlockEntity(this.treeBlockPos);
			if (blockEntity != null && blockEntity.bHasTransform)
			{
				this.treeTransform = blockEntity.transform;
				blockEntity.transform = null;
				blockEntity.bHasTransform = false;
			}
		}
		this.collHeight = 3f;
		if (this.treeTransform)
		{
			foreach (Collider collider in this.treeTransform.GetComponentsInChildren<Collider>())
			{
				collider.enabled = false;
				CapsuleCollider capsuleCollider = collider as CapsuleCollider;
				if (capsuleCollider != null)
				{
					this.collHeight = Utils.FastMax(this.collHeight, capsuleCollider.height);
				}
			}
		}
		this.collHeight *= 0.9f;
	}

	// Token: 0x060025D8 RID: 9688 RVA: 0x000E766D File Offset: 0x000E586D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnCollisionEnter(Collision collision)
	{
		this.Collide(collision);
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x000E766D File Offset: 0x000E586D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnCollisionStay(Collision collision)
	{
		this.Collide(collision);
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x000E7678 File Offset: 0x000E5878
	[PublicizedFrom(EAccessModifier.Private)]
	public void Collide(Collision collision)
	{
		if (this.isEntityRemote)
		{
			return;
		}
		if (collision.contactCount == 0)
		{
			return;
		}
		float magnitude = collision.relativeVelocity.magnitude;
		if (magnitude > 1f)
		{
			this.collidedWith(collision.gameObject.transform);
		}
		if (magnitude > 0.2f && collision.impulse.magnitude / this.treeRB.mass > 1.5f)
		{
			Vector3 vector = base.transform.position;
			float num = -1f;
			for (int i = 0; i < collision.contactCount; i++)
			{
				ContactPoint contact = collision.GetContact(i);
				float magnitude2 = contact.impulse.magnitude;
				if (magnitude2 > num)
				{
					num = magnitude2;
					vector = contact.point;
				}
			}
			Manager.BroadcastPlay(this, "treefallimpact", false, 1f);
			ParticleEffect particleEffect = new ParticleEffect("treefall", vector + Origin.position, base.transform.rotation * Quaternion.AngleAxis(90f, Vector3.forward), 1f, Color.white, null, null, 1f, "");
			GameManager.Instance.SpawnParticleEffectServer(particleEffect, this.entityId, false, false);
		}
	}

	// Token: 0x060025DB RID: 9691 RVA: 0x000E77B0 File Offset: 0x000E59B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void collidedWith(Transform _other)
	{
		if (this.timeToEnableDamage > 0f)
		{
			return;
		}
		Transform transform = _other;
		string text = _other.tag;
		if (text.StartsWith("E_BP_"))
		{
			transform = GameUtils.GetHitRootTransform(text, transform);
			text = transform.tag;
		}
		if (text.StartsWith("E_"))
		{
			Entity component = transform.GetComponent<Entity>();
			if (component && !component.IsDead() && this.treeCanDamageEntity(component))
			{
				this.hitEntities.Add(component.entityId);
				int num = (int)(this.treeRB.mass * 0.35999998f);
				base.StartCoroutine(this.onEntityDamageLater(component, num));
			}
		}
	}

	// Token: 0x060025DC RID: 9692 RVA: 0x000E7850 File Offset: 0x000E5A50
	[PublicizedFrom(EAccessModifier.Private)]
	public bool treeCanDamageEntity(Entity _entity)
	{
		return !this.hitEntities.Contains(_entity.entityId) && !(_entity is EntityPlayer) && !(_entity is EntitySupplyCrate);
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x000E787D File Offset: 0x000E5A7D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator onEntityDamageLater(Entity _entity, int _damage)
	{
		yield return new WaitForSeconds(0.05f);
		if (!_entity.IsDead() && _damage > 10)
		{
			_entity.DamageEntity(new DamageSource(EnumDamageSource.External, EnumDamageTypes.Crushing), _damage, false, 1f);
		}
		yield break;
	}

	// Token: 0x060025DE RID: 9694 RVA: 0x000E7894 File Offset: 0x000E5A94
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateMesh()
	{
		if (!this.isEntityRemote && this.world.GetBlock(this.treeBlockPos).type == this.treeBV.type)
		{
			this.world.SetBlockRPC(this.treeBlockPos, BlockValue.Air);
		}
		if (this.treeBV.isair)
		{
			if (this.treeTransform)
			{
				global::UnityEngine.Object.Destroy(this.treeTransform.gameObject);
				this.treeTransform = null;
			}
			return;
		}
		Transform transform = base.transform;
		Vector3 position = this.treeTransform.position;
		this.SetPosition(position + Origin.position, true);
		this.SetRotation(this.treeTransform.eulerAngles);
		transform.SetPositionAndRotation(position, this.treeTransform.rotation);
		this.treeTransform.SetParent(transform, false);
		this.treeTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		Transform transform2 = this.treeTransform.Find("rootBall");
		if (transform2)
		{
			transform2.gameObject.SetActive(true);
			transform2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
		}
		this.treeRB.useGravity = !this.isEntityRemote;
		this.treeRB.isKinematic = this.isEntityRemote;
		this.treeRB.position = position;
		this.treeRB.rotation = this.treeTransform.rotation;
		Block block = this.treeBV.Block;
		BlockShapeModelEntity blockShapeModelEntity = block.shape as BlockShapeModelEntity;
		float num = ((blockShapeModelEntity != null) ? blockShapeModelEntity.modelOffset.y : 0f);
		RaycastHit raycastHit;
		if (Physics.SphereCast(new Ray(transform.position + 3f * Vector3.up, Vector3.down), 0.25f, out raycastHit, 5f, -538750989))
		{
			num = transform.position.y - raycastHit.point.y;
		}
		transform.gameObject.layer = 23;
		CapsuleCollider component = transform.GetComponent<CapsuleCollider>();
		component.height = this.collHeight;
		component.center = new Vector3(0f, this.collHeight * 0.5f - num, 0f);
		component.enabled = true;
		this.treeRB.mass = (15f + 7f * this.collHeight) * 5f;
		this.treeRB.centerOfMass = new Vector3(0f, this.collHeight * 0.3f - num, 0f);
		if (!this.isEntityRemote)
		{
			this.treeRB.velocity = Vector3.zero;
			this.treeRB.angularVelocity = Vector3.zero;
			this.treeRB.solverIterations = 10;
			this.treeRB.solverVelocityIterations = 3;
			this.treeRB.AddForceAtPosition(this.fallTreeDir * ((80f + this.collHeight * 8f) * 5f), transform.position + Vector3.up * (this.collHeight * 0.65f - num), ForceMode.Impulse);
			block.SpawnDestroyParticleEffect(this.world, this.treeBV, this.treeBlockPos, this.world.GetLightBrightness(this.treeBlockPos), block.GetColorForSide(this.treeBV, BlockFace.Top), -1);
			this.lifetime = 3f;
			this.timeToEnableDamage = 1.5f;
		}
		this.rendererList.Clear();
		foreach (MeshRenderer meshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
		{
			this.rendererList.Add(meshRenderer);
		}
	}

	// Token: 0x060025DF RID: 9695 RVA: 0x000E7C40 File Offset: 0x000E5E40
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (this.isEntityRemote)
		{
			return;
		}
		this.timeToEnableDamage -= 0.05f;
		if (this.lifetime > 0f)
		{
			this.lifetime -= 0.05f;
			return;
		}
		if (this.timeToRemoveTree < 0f)
		{
			if (this.treeRB.angularVelocity.sqrMagnitude < 0.1f && this.treeRB.velocity.sqrMagnitude < 0.1f)
			{
				this.timeToRemoveTree = 1f;
				this.targetFade = 0f;
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<EntityFallingTree.NetPackageTreeFade>().Setup(this), false, -1, -1, -1, null, 192, false);
			}
		}
		else
		{
			this.timeToRemoveTree -= 0.05f;
			if (this.timeToRemoveTree < 0f)
			{
				this.DestroyTree();
			}
		}
		if (this.isMeshCreated && base.transform.position.y + Origin.position.y < 1f)
		{
			this.DestroyTree();
		}
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x000E7D64 File Offset: 0x000E5F64
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyTree()
	{
		this.SetDead();
		if (!this.isEntityRemote && this.world.GetBlock(this.treeBlockPos).type == this.treeBV.type)
		{
			this.world.SetBlockRPC(this.treeBlockPos, BlockValue.Air);
		}
		if (this.treeTransform != null)
		{
			global::UnityEngine.Object.Destroy(this.treeTransform.gameObject);
			this.treeTransform = null;
		}
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWith(Entity _other)
	{
		return false;
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsQRotationUsed()
	{
		return true;
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000E7DE8 File Offset: 0x000E5FE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTransform()
	{
		if (!this.isMeshCreated && this.treeTransform)
		{
			this.isMeshCreated = true;
			this.CreateMesh();
		}
		float deltaTime = Time.deltaTime;
		this.fade = Mathf.MoveTowards(this.fade, this.targetFade, deltaTime);
		if (this.fade < 1f)
		{
			float num = this.fade;
			for (int i = 0; i < this.rendererList.Count; i++)
			{
				Renderer renderer = this.rendererList[i];
				if (renderer)
				{
					if (this.fade < 0.5f && renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
					{
						renderer.gameObject.SetActive(false);
					}
					renderer.GetMaterials(this.mats);
					for (int j = 0; j < this.mats.Count; j++)
					{
						if (num < 1f)
						{
							this.mats[j].EnableKeyword("ENABLE_FADEOUT");
						}
						else
						{
							this.mats[j].DisableKeyword("ENABLE_FADEOUT");
						}
						this.mats[j].SetFloat("_FadeOut", num);
					}
				}
			}
			this.mats.Clear();
		}
		Transform transform = base.transform;
		Vector3 vector = transform.position;
		if (this.isEntityRemote)
		{
			float num2 = deltaTime * 20f;
			vector = Vector3.Lerp(vector, this.targetPos - Origin.position, num2);
			Quaternion quaternion = Quaternion.Slerp(transform.rotation, this.targetQRot, num2);
			transform.SetPositionAndRotation(vector, quaternion);
			return;
		}
		this.SetPosition(vector + Origin.position, true);
		this.SetRotation(transform.eulerAngles);
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsSavedToFile()
	{
		return false;
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x000E7FA2 File Offset: 0x000E61A2
	public override void OnEntityUnload()
	{
		if (this.treeTransform != null)
		{
			global::UnityEngine.Object.Destroy(this.treeTransform.gameObject);
			this.treeTransform = null;
		}
		base.OnEntityUnload();
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x000E7FCF File Offset: 0x000E61CF
	public override void MarkToUnload()
	{
		base.MarkToUnload();
		if (!this.isEntityRemote)
		{
			this.world.SetBlockRPC(this.treeBlockPos, BlockValue.Air);
		}
	}

	// Token: 0x04001C20 RID: 7200
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cMassScale = 5f;

	// Token: 0x04001C21 RID: 7201
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i treeBlockPos;

	// Token: 0x04001C22 RID: 7202
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockValue treeBV;

	// Token: 0x04001C23 RID: 7203
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 fallTreeDir;

	// Token: 0x04001C24 RID: 7204
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform treeTransform;

	// Token: 0x04001C25 RID: 7205
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody treeRB;

	// Token: 0x04001C26 RID: 7206
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<MeshRenderer> rendererList = new List<MeshRenderer>();

	// Token: 0x04001C27 RID: 7207
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float collHeight;

	// Token: 0x04001C28 RID: 7208
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isMeshCreated;

	// Token: 0x04001C29 RID: 7209
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeToRemoveTree = -1f;

	// Token: 0x04001C2A RID: 7210
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeToEnableDamage;

	// Token: 0x04001C2B RID: 7211
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<int> hitEntities = new List<int>();

	// Token: 0x04001C2C RID: 7212
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fade = 1f;

	// Token: 0x04001C2D RID: 7213
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetFade = 1f;

	// Token: 0x04001C2E RID: 7214
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Material> mats = new List<Material>();

	// Token: 0x020004AD RID: 1197
	[Preserve]
	public class NetPackageTreeFade : NetPackage
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override NetPackageDirection PackageDirection
		{
			get
			{
				return NetPackageDirection.ToClient;
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000E8051 File Offset: 0x000E6251
		public EntityFallingTree.NetPackageTreeFade Setup(Entity entity)
		{
			this.entityId = entity.entityId;
			return this;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000E8060 File Offset: 0x000E6260
		public override void read(PooledBinaryReader _reader)
		{
			this.entityId = _reader.ReadInt32();
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000E806E File Offset: 0x000E626E
		public override void write(PooledBinaryWriter _writer)
		{
			base.write(_writer);
			_writer.Write(this.entityId);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000E8084 File Offset: 0x000E6284
		public override void ProcessPackage(World _world, GameManager _callbacks)
		{
			if (_world == null)
			{
				return;
			}
			EntityFallingTree entityFallingTree = _world.GetEntity(this.entityId) as EntityFallingTree;
			if (entityFallingTree != null)
			{
				entityFallingTree.targetFade = 0f;
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override int GetLength()
		{
			return 4;
		}

		// Token: 0x04001C2F RID: 7215
		[PublicizedFrom(EAccessModifier.Private)]
		public int entityId;
	}
}
