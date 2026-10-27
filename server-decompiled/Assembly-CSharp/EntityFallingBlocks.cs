using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

// Token: 0x020004B7 RID: 1207
[Preserve]
public class EntityFallingBlocks : Entity
{
	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x060025E6 RID: 9702 RVA: 0x0002F260 File Offset: 0x0002D460
	public override Entity.EnumPositionUpdateMovementType positionUpdateMovementType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return Entity.EnumPositionUpdateMovementType.Instant;
		}
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x000E7B28 File Offset: 0x000E5D28
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.yOffset = 0.15f;
		Transform transform = base.transform;
		this.rigidBody = transform.GetComponent<Rigidbody>();
		if (this.isEntityRemote)
		{
			global::UnityEngine.Object.Destroy(this.rigidBody);
		}
		this.boxCollider = base.transform.GetComponent<BoxCollider>();
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x000E7B80 File Offset: 0x000E5D80
	public override void InitLocation(Vector3 _pos, Vector3 _rot)
	{
		base.InitLocation(_pos, _rot);
		Vector2i vector2i = World.toChunkXZ(_pos);
		this.chunkKey = WorldChunkCache.MakeChunkKey(vector2i.x, vector2i.y);
		if (!EntityFallingBlocks.fallingBlocksByChunk.ContainsKey(this.chunkKey))
		{
			EntityFallingBlocks.fallingBlocksByChunk[this.chunkKey] = new List<EntityFallingBlocks>();
		}
		EntityFallingBlocks.fallingBlocksByChunk[this.chunkKey].Add(this);
		if (!this.isEntityRemote)
		{
			this.rigidBody.position = _pos - Origin.position;
			this.rigidBody.rotation = Quaternion.Euler(_rot);
		}
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x000E7C1F File Offset: 0x000E5E1F
	public BlockValue[] GetBlockValues()
	{
		return this.blockValues;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000E7C27 File Offset: 0x000E5E27
	public Vector3i[] GetBlockPositions()
	{
		return this.blockPositions;
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x000E7C2F File Offset: 0x000E5E2F
	public void SetBlockGroupData(Vector3i[] _blockPositions, BlockValue[] _blockValues)
	{
		this.blockPositions = _blockPositions;
		this.blockValues = _blockValues;
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x000E7C3F File Offset: 0x000E5E3F
	public TextureFullArray[] GetTextureFullArrays()
	{
		return this.textureFullArrays;
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x000E7C47 File Offset: 0x000E5E47
	public void SetTextureFullArrays(TextureFullArray[] _textureFullArrays)
	{
		this.textureFullArrays = _textureFullArrays;
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x000E7C50 File Offset: 0x000E5E50
	public void SetStartVelocity(Vector3 _vel, float _angularVel)
	{
		this.startVel = _vel;
		this.startAngularVel = _angularVel;
	}

	// Token: 0x060025EF RID: 9711 RVA: 0x000E7C60 File Offset: 0x000E5E60
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (!this.isMeshCreated && !GameManager.IsDedicatedServer)
		{
			this.CreateMesh();
			if (this.meshRenderer)
			{
				this.isMeshCreated = true;
				this.meshRenderer.enabled = true;
			}
		}
		if (this.boxColliders.Count > 0 && !this.boxColliders[0].enabled)
		{
			foreach (BoxCollider boxCollider in this.boxColliders)
			{
				boxCollider.enabled = true;
			}
			this.massKg = 0f;
			foreach (BlockValue blockValue in this.blockValues)
			{
				Block block = blockValue.Block;
				this.massKg += Utils.FastMin(block.blockMaterial.Hardness.Value * (float)block.blockMaterial.Mass.Value, 10f) * 8f;
			}
			if (!this.isEntityRemote)
			{
				this.rigidBody.mass = Utils.FastMax(10f, this.massKg);
				this.rigidBody.velocity = this.startVel;
				this.rigidBody.angularVelocity = this.rand.RandomOnUnitSphere * this.startAngularVel;
			}
		}
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x000E7DDC File Offset: 0x000E5FDC
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (this.IsDead())
		{
			return;
		}
		this.fallTimeInTicks++;
		Block block = this.blockValues[0].Block;
		if (this.isEntityRemote)
		{
			return;
		}
		Vector3 velocity = this.rigidBody.velocity;
		if ((this.fallTimeInTicks & 1) == 0)
		{
			List<Entity> entitiesInBounds = this.world.GetEntitiesInBounds(this, BoundsUtils.ExpandBounds(BoundsUtils.ExpandDirectional(this.boundingBox, this.motion), 0f, 0.2f, 0f));
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				Entity entity = entitiesInBounds[i];
				int entityId = entity.entityId;
				int num;
				this.entityHits.TryGetValue(entityId, out num);
				if (num < 3 && entity.CanCollideWith(this) && this.position.y >= entity.getHeadPosition().y && velocity.y < -0.8f)
				{
					float num2 = (float)((int)Utils.FastMin(this.massKg * velocity.y * -0.05f, 40f));
					num2 = EffectManager.GetValue(PassiveEffects.FallingBlockDamage, null, num2, entity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
					int num3 = (int)num2;
					entity.DamageEntity(DamageSource.fallingBlock, num3, false, 1f);
					if (num3 >= 1)
					{
						num++;
						this.entityHits[entityId] = num;
					}
					Log.Warning("{0} EntityFallingBlocks {1} hit {2}, vel {3}, for {4}", new object[]
					{
						GameManager.frameCount,
						this,
						entity,
						velocity,
						num3
					});
				}
			}
		}
		bool flag = false;
		Transform transform = base.transform;
		if (this.fallTimeInTicks < 60 || velocity.sqrMagnitude > 0.0625f)
		{
			this.notMovingCount = 0;
		}
		else
		{
			int num4 = this.notMovingCount + 1;
			this.notMovingCount = num4;
			if (num4 > 3)
			{
				Vector3i vector3i = World.worldToBlockPos(this.position + Vector3.down);
				if (!this.world.GetBlock(vector3i).isair && this.world.GetStability(vector3i) > 0 && GamePrefs.GetBool(EnumGamePrefs.OptionsStabSpawnBlocksOnGround))
				{
					if (block.HasItemsToDropForEvent(EnumDropEvent.Fall))
					{
						float num5 = 1f;
						List<Block.SItemDropProb> list = block.itemsToDrop[EnumDropEvent.Fall];
						if (list.Count > 0)
						{
							num5 = list[0].prob;
						}
						block.DropItemsOnEvent(this.world, this.blockValues[0], EnumDropEvent.Fall, num5, base.GetPosition(), new Vector3(1.5f, 0f, 1.5f), Constants.cItemExplosionLifetime, -1, false);
					}
					else if (this.fallTimeInTicks < 16)
					{
						block.DropItemsOnEvent(this.world, this.blockValues[0], EnumDropEvent.Destroy, 0.7f, base.GetPosition(), new Vector3(1.5f, 0f, 1.5f), Constants.cItemExplosionLifetime, -1, false);
					}
				}
				flag = true;
			}
		}
		if (this.fallTimeInTicks > 300)
		{
			flag = true;
		}
		if (transform.position.y + Origin.position.y < 2f)
		{
			flag = true;
		}
		if (flag)
		{
			this.SetDead();
		}
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x000E8132 File Offset: 0x000E6332
	public override void SetDead()
	{
		EntityFallingBlocks.fallingBlocksByChunk[this.chunkKey].Remove(this);
		base.SetDead();
	}

	// Token: 0x060025F2 RID: 9714 RVA: 0x000E8154 File Offset: 0x000E6354
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateMesh()
	{
		int num = this.blockValues[0].ToItemType();
		ItemClass forId = ItemClass.GetForId(num);
		if (num == 0 || forId == null)
		{
			Log.Error("EntityFallingBlocks failed id {0}, type", new object[] { num });
			this.SetDead();
			return;
		}
		Transform transform = base.transform;
		int meshIndex = (int)this.blockValues[0].Block.MeshIndex;
		Vector3i vector3i = World.worldToBlockPos(this.blockPositions[0]);
		byte b;
		byte b2;
		this.world.GetSunAndBlockColors(vector3i, out b, out b2);
		VoxelMesh voxelMesh = VoxelMesh.Create(meshIndex, MeshDescription.meshes[meshIndex].meshType, 1);
		VoxelMesh[] array = new VoxelMesh[MeshDescription.meshes.Length];
		array[meshIndex] = voxelMesh;
		for (int i = 0; i < this.blockValues.Length; i++)
		{
			BlockValue blockValue = this.blockValues[i];
			Vector3i vector3i2 = World.worldToBlockPos(this.blockPositions[i]);
			if (!(blockValue.Block.shape is BlockShapeTerrain))
			{
				blockValue.Block.shape.renderFull(vector3i2, blockValue, vector3i2 - vector3i + EntityFallingBlocks.renderOffsetV, null, new LightingAround(b, b2, 0), this.textureFullArrays[i], array, BlockShape.MeshPurpose.Local);
				int count = voxelMesh.m_Vertices.Count;
			}
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(transform, false);
		gameObject.AddComponent<UpdateLightOnChunkMesh>();
		gameObject.name = "Block_" + this.blockValues[0].type.ToString();
		Transform transform2 = gameObject.transform;
		MeshFilter meshFilter;
		MeshRenderer meshRenderer;
		VoxelMesh.CreateMeshFilter(meshIndex, 0, gameObject, "Item", false, out meshFilter, out meshRenderer);
		if (meshFilter != null)
		{
			voxelMesh.CopyToMesh(meshFilter, meshRenderer, 0, null);
			for (int j = 0; j < this.blockValues.Length; j++)
			{
				BlockValue[] array2 = this.blockValues;
				Vector3i vector3i3 = World.worldToBlockPos(this.blockPositions[j]);
				GameObject gameObject2 = new GameObject(string.Format("blockCollider{0}", j));
				gameObject2.transform.SetParent(transform, false);
				gameObject2.transform.localPosition = vector3i3 - vector3i;
				BoxCollider boxCollider = gameObject2.AddComponent<BoxCollider>();
				boxCollider.size = new Vector3(0.9f, 0.9f, 0.9f);
				boxCollider.material = this.boxCollider.material;
				this.boxColliders.Add(boxCollider);
			}
		}
		if (!transform2)
		{
			Log.Warning("EntityFallingBlock failed id {0}, mesh", new object[] { num });
			this.SetDead();
			return;
		}
		this.meshRenderer = transform2.GetComponentInChildren<Renderer>();
		this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		Collider[] componentsInChildren = transform2.GetComponentsInChildren<Collider>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].enabled = false;
		}
		Animator[] componentsInChildren2 = transform2.GetComponentsInChildren<Animator>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			componentsInChildren2[k].enabled = false;
		}
		Utils.SetColliderLayerRecursively(transform.gameObject, 13);
	}

	// Token: 0x060025F3 RID: 9715 RVA: 0x000E849C File Offset: 0x000E669C
	public override void VisiblityCheck(float _distanceSqr, bool _masterIsZooming)
	{
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = _distanceSqr < (float)(_masterIsZooming ? 14400 : 10000);
		}
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x000E77C7 File Offset: 0x000E59C7
	public override bool CanCollideWith(Entity _other)
	{
		return _other is EntityAlive;
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x000E84CC File Offset: 0x000E66CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTransform()
	{
		Transform transform = base.transform;
		if (this.isEntityRemote)
		{
			if (this.targetPos.sqrMagnitude > 0f && (this.targetPos - this.interpEndPos).sqrMagnitude > 0.0001f)
			{
				this.interpStartPos = transform.position + Origin.position;
				this.interpEndPos = this.targetPos;
				this.interpTime = 0.1f;
			}
			Vector3 vector = this.targetPos;
			float deltaTime = Time.deltaTime;
			if (this.interpTime > 0f)
			{
				this.interpTime -= deltaTime;
				float num = 1f - this.interpTime / 0.1f;
				vector = Vector3.Lerp(this.interpStartPos, this.interpEndPos, num);
			}
			Quaternion quaternion = Quaternion.Slerp(transform.rotation, this.qrotation, deltaTime * 20f);
			transform.SetPositionAndRotation(vector - Origin.position, quaternion);
			return;
		}
		this.SetPosition(transform.position + Origin.position, true);
		this.SetRotation(transform.eulerAngles);
		this.qrotation = transform.rotation;
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x000E85F8 File Offset: 0x000E67F8
	public void OnContactEvent()
	{
		if (this.isGroundHit || this.isEntityRemote)
		{
			return;
		}
		Vector3i blockPosition = base.GetBlockPosition();
		blockPosition.y--;
		BlockValue block = this.world.GetBlock(blockPosition);
		if (block.isair)
		{
			return;
		}
		this.isGroundHit = true;
		float lightBrightness = this.world.GetLightBrightness(blockPosition);
		Color colorForSide = block.Block.GetColorForSide(block, BlockFace.Top);
		string text = "impact_stone_on_" + block.Block.blockMaterial.SurfaceCategory;
		this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(text, base.GetPosition(), Quaternion.identity, lightBrightness, colorForSide, this.blockValues[0].Block.blockMaterial.SurfaceCategory + "hit" + block.Block.blockMaterial.SurfaceCategory, null, 1f, ""), this.entityId, false, false);
	}

	// Token: 0x060025F7 RID: 9719 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsQRotationUsed()
	{
		return true;
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x000E86ED File Offset: 0x000E68ED
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		if (this.meshRenderer != null)
		{
			global::UnityEngine.Object.Destroy(this.meshRenderer.material);
		}
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x000E8714 File Offset: 0x000E6914
	public static void ClearFallingBlocksForChunks(HashSetLong chunks)
	{
		List<EntityFallingBlocks> list = new List<EntityFallingBlocks>();
		foreach (long num in chunks)
		{
			if (EntityFallingBlocks.fallingBlocksByChunk.ContainsKey(num))
			{
				foreach (EntityFallingBlocks entityFallingBlocks in EntityFallingBlocks.fallingBlocksByChunk[num])
				{
					list.Add(entityFallingBlocks);
				}
			}
		}
		foreach (EntityFallingBlocks entityFallingBlocks2 in list)
		{
			entityFallingBlocks2.SetDead();
		}
	}

	// Token: 0x04001C37 RID: 7223
	public static bool Enabled = false;

	// Token: 0x04001C38 RID: 7224
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector3 renderOffsetV = new Vector3(-0.5f, -0.5f, -0.5f);

	// Token: 0x04001C39 RID: 7225
	public static int MaxGroupSize = 3;

	// Token: 0x04001C3A RID: 7226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxHitsPerEntity = 3;

	// Token: 0x04001C3B RID: 7227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMinDamageToCountHit = 1;

	// Token: 0x04001C3C RID: 7228
	public Transform prefabParticleOnFallT;

	// Token: 0x04001C3D RID: 7229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer meshRenderer;

	// Token: 0x04001C3E RID: 7230
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody rigidBody;

	// Token: 0x04001C3F RID: 7231
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockValue[] blockValues;

	// Token: 0x04001C40 RID: 7232
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i[] blockPositions;

	// Token: 0x04001C41 RID: 7233
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float massKg;

	// Token: 0x04001C42 RID: 7234
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextureFullArray[] textureFullArrays;

	// Token: 0x04001C43 RID: 7235
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isMeshCreated;

	// Token: 0x04001C44 RID: 7236
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int fallTimeInTicks;

	// Token: 0x04001C45 RID: 7237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, int> entityHits = new Dictionary<int, int>(8);

	// Token: 0x04001C46 RID: 7238
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lastTimeStartParticleSpawned;

	// Token: 0x04001C47 RID: 7239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lastTimeEndParticleSpawned;

	// Token: 0x04001C48 RID: 7240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int notMovingCount;

	// Token: 0x04001C49 RID: 7241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGroundHit;

	// Token: 0x04001C4A RID: 7242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BoxCollider boxCollider;

	// Token: 0x04001C4B RID: 7243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<BoxCollider> boxColliders = new List<BoxCollider>();

	// Token: 0x04001C4C RID: 7244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long chunkKey;

	// Token: 0x04001C4D RID: 7245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<long, List<EntityFallingBlocks>> fallingBlocksByChunk = new Dictionary<long, List<EntityFallingBlocks>>();

	// Token: 0x04001C4E RID: 7246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startVel;

	// Token: 0x04001C4F RID: 7247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startAngularVel = 0.5f;

	// Token: 0x04001C50 RID: 7248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 interpStartPos;

	// Token: 0x04001C51 RID: 7249
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 interpEndPos;

	// Token: 0x04001C52 RID: 7250
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float interpTime;
}
