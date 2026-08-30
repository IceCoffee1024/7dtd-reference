using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

// Token: 0x020001AA RID: 426
[Preserve]
public class BlockShapeModelEntity : BlockShapeInvisible
{
	// Token: 0x06000CD3 RID: 3283 RVA: 0x0005091A File Offset: 0x0004EB1A
	public BlockShapeModelEntity()
	{
		this.IsRotatable = true;
		this.IsNotifyOnLoadUnload = true;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0005093C File Offset: 0x0004EB3C
	public override void Init(Block _block)
	{
		base.Init(_block);
		this.modelNameWithPath = _block.Properties.GetValue("Model");
		if (this.modelNameWithPath == null)
		{
			throw new Exception("No model specified on block with name " + _block.GetBlockName());
		}
		if (this.modelNameWithPath.Length > 0)
		{
			_block.Properties.ParseInt(EntityClass.PropCensor, ref this.censorMode);
			if (this.censorMode != 0 && GameManager.Instance && GameManager.Instance.IsGoreCensored())
			{
				if (this.modelNameWithPath.Contains("@"))
				{
					this.modelNameWithPath = this.modelNameWithPath.Replace(".", "_CGore.");
				}
				else if (!this.modelNameWithPath.Contains("."))
				{
					this.modelNameWithPath += "_CGore";
				}
			}
		}
		this.modelName = GameIO.GetFilenameFromPathWithoutExtension(this.modelNameWithPath);
		this.modelOffset = new Vector3(0f, 0.5f, 0f);
		_block.Properties.ParseVec("ModelOffset", ref this.modelOffset);
		_block.Properties.ParseFloat("LODCullScale", ref this.LODCullScale);
		_block.Properties.ParseInt("SymType", ref this.SymmetryType);
		string text;
		if (_block.Properties.Values.TryGetValue(BlockShapeModelEntity.PropDamagedMesh, out text))
		{
			string[] array = text.Split(',', StringSplitOptions.None);
			if (array.Length >= 2)
			{
				this.damageStates = new List<BlockShapeModelEntity.DamageState>();
				for (int i = 0; i < array.Length - 1; i += 2)
				{
					BlockShapeModelEntity.DamageState damageState;
					damageState.objName = array[i].Trim();
					damageState.health = float.Parse(array[i + 1]);
					this.damageStates.Add(damageState);
				}
			}
		}
		GameObjectPool.Instance.AddPooledObject(this.modelName, new GameObjectPool.LoadCallback(this.PoolLoadCallback), new GameObjectPool.CreateCallback(this.PoolCreateOnceToAllCallBack), new GameObjectPool.CreateCallback(this.PoolCreateCallBack));
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x00050B33 File Offset: 0x0004ED33
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform PoolLoadCallback()
	{
		Transform prefab = this.getPrefab();
		if (prefab == null)
		{
			throw new Exception("Model '" + this.modelNameWithPath + "' not found on block with name " + this.block.GetBlockName());
		}
		return prefab;
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00050B6C File Offset: 0x0004ED6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PoolCreateOnceToAllCallBack(GameObject obj)
	{
		Collider component = obj.transform.GetComponent<Collider>();
		if (component != null)
		{
			if (component is BoxCollider)
			{
				Vector3 center = ((BoxCollider)component).center;
				Vector3 size = ((BoxCollider)component).size;
				this.bounds = BoundsUtils.BoundsForMinMax(center.x - size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f, center.x + size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f);
				this.boundsArr[0] = this.bounds;
				this.isCustomBounds = true;
				return;
			}
			if (component is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = component as CapsuleCollider;
				Vector3 center2 = capsuleCollider.center;
				Vector3 vector = new Vector3(capsuleCollider.radius * 2f, capsuleCollider.height, capsuleCollider.radius * 2f);
				this.bounds = BoundsUtils.BoundsForMinMax(center2.x - vector.x / 2f, center2.y - vector.y / 2f, center2.z - vector.z / 2f, center2.x + vector.x / 2f, center2.y + vector.y / 2f, center2.z + vector.z / 2f);
				this.boundsArr[0] = this.bounds;
				this.isCustomBounds = true;
			}
		}
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00050D2C File Offset: 0x0004EF2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PoolCreateCallBack(GameObject obj)
	{
		Transform transform = obj.transform;
		LODGroup lodgroup = transform.GetComponent<LODGroup>();
		if (lodgroup)
		{
			LODFadeMode fadeMode = lodgroup.fadeMode;
			if (fadeMode == LODFadeMode.SpeedTree)
			{
				return;
			}
			if (fadeMode == LODFadeMode.None)
			{
				lodgroup.fadeMode = LODFadeMode.CrossFade;
				lodgroup.animateCrossFading = true;
			}
			if (fadeMode == LODFadeMode.CrossFade)
			{
				lodgroup.animateCrossFading = true;
			}
			LOD[] lods = lodgroup.GetLODs();
			int num = lods.Length - 1;
			float num2 = lodgroup.size;
			if (num2 < 0.4f)
			{
				num2 *= 3.8f;
				if (num2 < 1f)
				{
					num2 = 1f;
				}
			}
			else if (num2 < 0.65f)
			{
				num2 *= 2.5f;
			}
			else if (num2 < 0.95f)
			{
				num2 *= 1.5f;
			}
			else if (num2 >= 1.45f)
			{
				if (num2 < 2.5f)
				{
					num2 *= 0.83f;
				}
				else if (num2 < 6.2f)
				{
					num2 *= 0.64f;
				}
				else
				{
					num2 *= 0.45f;
				}
			}
			float num3 = num2 * 0.02f * this.LODCullScale;
			if (num3 > 0.1f)
			{
				num3 = 0.1f;
			}
			lods[num].screenRelativeTransitionHeight = num3;
			if (num > 0)
			{
				float num4 = num3;
				for (int i = num - 1; i >= 0; i--)
				{
					float num5 = lods[i].screenRelativeTransitionHeight;
					if (num5 - 0.025f <= num4)
					{
						num5 = num4 + 0.025f;
						lods[i].screenRelativeTransitionHeight = num5;
					}
					num4 = num5;
				}
			}
			lodgroup.SetLODs(lods);
			if (num >= 2 && GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowDistance) <= 2)
			{
				foreach (Renderer renderer in lods[num].renderers)
				{
					if (renderer)
					{
						renderer.shadowCastingMode = ShadowCastingMode.Off;
					}
				}
				return;
			}
		}
		else if (transform.childCount == 0)
		{
			MeshRenderer component = obj.GetComponent<MeshRenderer>();
			if (component)
			{
				LOD lod;
				lod.screenRelativeTransitionHeight = 0.025f;
				lod.renderers = new Renderer[] { component };
				lod.fadeTransitionWidth = 0f;
				lodgroup = obj.AddComponent<LODGroup>();
				lodgroup.fadeMode = LODFadeMode.CrossFade;
				lodgroup.animateCrossFading = true;
				lodgroup.SetLODs(new LOD[] { lod });
			}
		}
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00050F70 File Offset: 0x0004F170
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform getPrefab()
	{
		Transform transform = DataLoader.LoadAsset<Transform>(this.modelNameWithPath, false);
		if (transform == null)
		{
			Log.Error("Model '{0}' not found on block with name {1}", new object[]
			{
				this.modelNameWithPath,
				this.block.GetBlockName()
			});
			transform = DataLoader.LoadAsset<Transform>("@:Entities/Misc/block_missingPrefab.prefab", false);
			if (transform == null)
			{
				return null;
			}
		}
		string filenameFromPathWithoutExtension = GameIO.GetFilenameFromPathWithoutExtension(this.modelNameWithPath);
		if (transform.name != filenameFromPathWithoutExtension)
		{
			Log.Error("Model has a wrong name '{0}'. Maybe check upper/lower case mismatch on block with name {1}?", new object[]
			{
				filenameFromPathWithoutExtension,
				this.block.GetBlockName()
			});
		}
		return transform;
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00051010 File Offset: 0x0004F210
	public Transform CloneModel(BlockValue _blockValue, Transform _parent)
	{
		Transform transform = global::UnityEngine.Object.Instantiate<Transform>(this.getPrefab());
		transform.parent = _parent;
		Block block = _blockValue.Block;
		if (block.tintColor.a > 0f)
		{
			UpdateLight.SetTintColor(transform, block.tintColor);
		}
		Quaternion rotation = this.GetRotation(_blockValue);
		Vector3 rotatedOffset = this.GetRotatedOffset(block, rotation);
		transform.localPosition = rotatedOffset + new Vector3(0f, -0.5f, 0f);
		transform.localRotation = rotation;
		return transform;
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00051090 File Offset: 0x0004F290
	public static Vector3 GetOffsetForMultiblock(Vector3 modelOffset, Vector3i multiBlockSize)
	{
		Vector3 vector = modelOffset;
		Vector3 zero = Vector3.zero;
		zero.y = -0.5f;
		if ((multiBlockSize.x & 1) == 0)
		{
			zero.x = -0.5f;
		}
		if ((multiBlockSize.z & 1) == 0)
		{
			zero.z = -0.5f;
		}
		vector += zero;
		vector.y += 0.5f;
		return vector;
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x000510F8 File Offset: 0x0004F2F8
	public Vector3 GetRotatedOffset(Block block, Quaternion rot)
	{
		Vector3 vector = rot * this.modelOffset;
		Vector3 vector2 = Vector3.zero;
		vector2.y = -0.5f;
		if (block.isMultiBlock)
		{
			if ((block.multiBlockPos.dim.x & 1) == 0)
			{
				vector2.x = -0.5f;
			}
			if ((block.multiBlockPos.dim.z & 1) == 0)
			{
				vector2.z = -0.5f;
			}
		}
		vector2 = rot * vector2;
		vector += vector2;
		vector.y += 0.5f;
		return vector;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0005118D File Offset: 0x0004F38D
	public override Quaternion GetRotation(BlockValue _blockValue)
	{
		return BlockShapeNew.GetRotationStatic((int)_blockValue.rotation);
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0005119C File Offset: 0x0004F39C
	public override Bounds[] GetBounds(BlockValue _blockValue)
	{
		if (!this.isCustomBounds)
		{
			return base.GetBounds(_blockValue);
		}
		Quaternion rotation = this.GetRotation(_blockValue);
		Vector3 vector = rotation * this.bounds.min + this.modelOffset;
		Vector3 vector2 = rotation * this.bounds.max + this.modelOffset;
		this.boundsArr[0].min = new Vector3((vector2.x > vector.x) ? vector.x : vector2.x, (vector2.y > vector.y) ? vector.y : vector2.y, (vector2.z > vector.z) ? vector.z : vector2.z) + new Vector3(0.5f, 0f, 0.5f);
		this.boundsArr[0].max = new Vector3((vector2.x < vector.x) ? vector.x : vector2.x, (vector2.y < vector.y) ? vector.y : vector2.y, (vector2.z < vector.z) ? vector.z : vector2.z) + new Vector3(0.5f, 0f, 0.5f);
		return this.boundsArr;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00051308 File Offset: 0x0004F508
	public override BlockValue RotateY(bool _bLeft, BlockValue _blockValue, int _rotCount)
	{
		if (_bLeft)
		{
			_rotCount = -_rotCount;
		}
		int rotation = (int)_blockValue.rotation;
		if (rotation >= 24)
		{
			_blockValue.rotation = (byte)(((rotation - 24 + _rotCount) & 3) + 24);
		}
		else
		{
			int num = 90 * _rotCount;
			_blockValue.rotation = (byte)BlockShapeNew.ConvertRotationFree(rotation, Quaternion.AngleAxis((float)num, Vector3.up), false);
		}
		return _blockValue;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0005135F File Offset: 0x0004F55F
	public override byte Rotate(bool _bLeft, int _rotation)
	{
		_rotation += (_bLeft ? (-1) : 1);
		if (_rotation > 10)
		{
			_rotation = 0;
		}
		if (_rotation < 0)
		{
			_rotation = 10;
		}
		return (byte)_rotation;
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00051380 File Offset: 0x0004F580
	public override BlockValue MirrorY(bool _bAlongX, BlockValue _blockValue)
	{
		if (!_bAlongX)
		{
			switch (_blockValue.rotation)
			{
			case 0:
				_blockValue.rotation = 2;
				break;
			case 1:
				_blockValue.rotation = 1;
				break;
			case 2:
				_blockValue.rotation = 0;
				break;
			case 3:
				_blockValue.rotation = 3;
				break;
			case 4:
				_blockValue.rotation = 7;
				break;
			case 5:
				_blockValue.rotation = 6;
				break;
			case 6:
				_blockValue.rotation = 5;
				break;
			case 7:
				_blockValue.rotation = 4;
				break;
			case 8:
				_blockValue.rotation = 8;
				break;
			case 9:
				_blockValue.rotation = 9;
				break;
			case 10:
				_blockValue.rotation = 10;
				break;
			case 11:
				_blockValue.rotation = 11;
				break;
			}
		}
		else
		{
			switch (_blockValue.rotation)
			{
			case 0:
				_blockValue.rotation = 0;
				break;
			case 1:
				_blockValue.rotation = 3;
				break;
			case 2:
				_blockValue.rotation = 2;
				break;
			case 3:
				_blockValue.rotation = 1;
				break;
			case 4:
				_blockValue.rotation = 7;
				break;
			case 5:
				_blockValue.rotation = 6;
				break;
			case 6:
				_blockValue.rotation = 5;
				break;
			case 7:
				_blockValue.rotation = 4;
				break;
			case 8:
				_blockValue.rotation = 8;
				break;
			case 9:
				_blockValue.rotation = 11;
				break;
			case 10:
				_blockValue.rotation = 10;
				break;
			case 11:
				_blockValue.rotation = 9;
				break;
			}
		}
		return _blockValue;
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00051530 File Offset: 0x0004F730
	public override void OnBlockValueChanged(WorldBase _world, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _blockPos, _oldBlockValue, _newBlockValue);
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_blockPos.x, _blockPos.y, _blockPos.z);
		if (chunk == null)
		{
			return;
		}
		BlockEntityData blockEntity = chunk.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return;
		}
		Block block = _newBlockValue.Block;
		if (_newBlockValue.rotation != _oldBlockValue.rotation)
		{
			blockEntity.transform.localRotation = block.shape.GetRotation(_newBlockValue);
		}
		blockEntity.blockValue = _newBlockValue;
		if (this.damageStates != null)
		{
			if (this.GetDamageStateIndex(_oldBlockValue) != this.GetDamageStateIndex(_newBlockValue))
			{
				this.UpdateDamageState(_oldBlockValue, _newBlockValue, blockEntity, true);
				return;
			}
		}
		else
		{
			int num = Mathf.Min(_newBlockValue.damage, block.MaxDamage) - 1;
			blockEntity.SetMaterialValue("_Damage", (float)num);
		}
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0005160C File Offset: 0x0004F80C
	public override void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockAdded(world, _chunk, _blockPos, _blockValue);
		_chunk.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
		{
			bNeedsTemperature = true
		});
		this.registerSleepers(_blockPos, _blockValue);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00051644 File Offset: 0x0004F844
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
		_chunk.RemoveEntityBlockStub(_blockPos);
		if (GameManager.Instance.IsEditMode() && _blockValue.Block.IsSleeperBlock)
		{
			Prefab.TransientSleeperBlockIncrement(_blockPos, -1);
			SleeperVolumeToolManager.UnRegisterSleeperBlock(_blockPos);
		}
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00051680 File Offset: 0x0004F880
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_blockPos);
		if (chunk == null)
		{
			return;
		}
		chunk.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
		{
			bNeedsTemperature = true
		});
		this.registerSleepers(_blockPos, _blockValue);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x000516D0 File Offset: 0x0004F8D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerSleepers(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (GameManager.Instance.IsEditMode() && _blockValue.Block.IsSleeperBlock)
		{
			Prefab.TransientSleeperBlockIncrement(_blockPos, 1);
			ThreadManager.AddSingleTaskMainThread("OnBlockAddedOrLoaded.RegisterSleeperBlock", delegate
			{
				SleeperVolumeToolManager.RegisterSleeperBlock(_blockValue, this.CloneModel(_blockValue, null), _blockPos);
			});
		}
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x00051738 File Offset: 0x0004F938
	public override void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformBeforeActivated(_world, _blockPos, _blockValue, _ebcd);
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (this.damageStates != null)
		{
			this.UpdateDamageState(_blockValue, _blockValue, _ebcd, true);
		}
		else
		{
			int num = (int)(10f * (float)_blockValue.damage) / _blockValue.Block.MaxDamage;
			_ebcd.SetMaterialValue("_Damage", (float)num);
		}
		if (this.block.tintColor.a > 0f)
		{
			_ebcd.SetMaterialColor("_Color", this.block.tintColor);
			return;
		}
		if (this.block.defaultTintColor.a > 0f)
		{
			_ebcd.SetMaterialColor("_Color", this.block.defaultTintColor);
		}
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000517F4 File Offset: 0x0004F9F4
	public override bool UseRepairDamageState(BlockValue _blockValue)
	{
		return this.damageStates.Count > 1 && this.GetDamageStateIndex(_blockValue) == this.damageStates.Count - 1;
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x00051820 File Offset: 0x0004FA20
	public void UpdateDamageState(BlockValue _oldBlockValue, BlockValue _newBlockValue, BlockEntityData _data, bool bPlayEffects = true)
	{
		int damageStateIndex = this.GetDamageStateIndex(_oldBlockValue);
		int damageStateIndex2 = this.GetDamageStateIndex(_newBlockValue);
		bool flag = damageStateIndex2 > damageStateIndex;
		if (flag)
		{
			Transform transform = _data.transform.Find("FX");
			if (transform)
			{
				AudioPlayer componentInChildren = transform.GetComponentInChildren<AudioPlayer>();
				if (componentInChildren)
				{
					componentInChildren.Play();
				}
				ParticleSystem componentInChildren2 = transform.GetComponentInChildren<ParticleSystem>();
				if (componentInChildren2)
				{
					componentInChildren2.Emit(10);
				}
			}
		}
		for (int i = 0; i < this.damageStates.Count; i++)
		{
			BlockShapeModelEntity.DamageState damageState = this.damageStates[i];
			if (!(damageState.objName == "-"))
			{
				GameObject gameObject = _data.transform.Find(damageState.objName).gameObject;
				gameObject.SetActive(i == damageStateIndex2);
				if (i == damageStateIndex2 && flag)
				{
					AudioSource component = gameObject.GetComponent<AudioSource>();
					if (component != null)
					{
						component.PlayDelayed(0.15f);
					}
					AudioPlayer component2 = gameObject.GetComponent<AudioPlayer>();
					if (component2 != null)
					{
						component2.Play();
					}
					ParticleSystem component3 = gameObject.GetComponent<ParticleSystem>();
					if (component3)
					{
						component3.Emit(10);
					}
				}
			}
		}
		UpdateLightOnAllMaterials component4 = _data.transform.GetComponent<UpdateLightOnAllMaterials>();
		if (component4 != null)
		{
			component4.Reset();
		}
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00051978 File Offset: 0x0004FB78
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetDamageStateIndex(BlockValue _blockValue)
	{
		float num = (float)(_blockValue.Block.MaxDamage - _blockValue.damage);
		int num2 = this.damageStates.Count - 1;
		for (int i = 0; i < num2; i++)
		{
			if (num > this.damageStates[i + 1].health)
			{
				return i;
			}
		}
		return num2;
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000519CD File Offset: 0x0004FBCD
	public float GetNextDamageStateDownHealth(BlockValue _blockValue)
	{
		return this.damageStates[Utils.FastMin(this.GetDamageStateCount() - 1, this.GetDamageStateIndex(_blockValue) + 1)].health;
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x000519F5 File Offset: 0x0004FBF5
	public float GetNextDamageStateUpHealth(BlockValue _blockValue)
	{
		return this.damageStates[Utils.FastMax(0, this.GetDamageStateIndex(_blockValue) - 1)].health;
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x00051A16 File Offset: 0x0004FC16
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetDamageStateCount()
	{
		return this.damageStates.Count;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00051A23 File Offset: 0x0004FC23
	public bool IsObstructionForDamageState(BlockValue _blockValue)
	{
		if (this.damageStates.Count > 0)
		{
			this.GetDamageStateIndex(_blockValue);
			List<BlockShapeModelEntity.DamageState> list = this.damageStates;
			return list[list.Count - 1].health > 1f;
		}
		return true;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00051A5C File Offset: 0x0004FC5C
	public override float GetStepHeight(BlockValue _blockValue, BlockFace crossingFace)
	{
		if (this.isCustomBounds && _blockValue.Block.IsCollideMovement)
		{
			return this.boundsArr[0].size.y;
		}
		return base.GetStepHeight(_blockValue, crossingFace);
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00051A94 File Offset: 0x0004FC94
	public override void CalculateCollisionHash(IncrementalHash calcHash)
	{
		int num = 1;
		calcHash.AppendDataNoAlloc(num);
		if (this.damageStates != null)
		{
			foreach (BlockShapeModelEntity.DamageState damageState in this.damageStates)
			{
				calcHash.AppendData(Encoding.UTF8.GetBytes(damageState.objName ?? ""));
				calcHash.AppendDataNoAlloc(damageState.health);
			}
		}
	}

	// Token: 0x04000B60 RID: 2912
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropDamagedMesh = "MeshDamage";

	// Token: 0x04000B61 RID: 2913
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cMissingPrefabEntityPath = "@:Entities/Misc/block_missingPrefab.prefab";

	// Token: 0x04000B62 RID: 2914
	public string modelName;

	// Token: 0x04000B63 RID: 2915
	public Vector3 modelOffset;

	// Token: 0x04000B64 RID: 2916
	public int censorMode;

	// Token: 0x04000B65 RID: 2917
	[PublicizedFrom(EAccessModifier.Protected)]
	public string modelNameWithPath;

	// Token: 0x04000B66 RID: 2918
	[PublicizedFrom(EAccessModifier.Private)]
	public float LODCullScale = 1f;

	// Token: 0x04000B67 RID: 2919
	[PublicizedFrom(EAccessModifier.Private)]
	public new Bounds bounds;

	// Token: 0x04000B68 RID: 2920
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCustomBounds;

	// Token: 0x04000B69 RID: 2921
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockShapeModelEntity.DamageState> damageStates;

	// Token: 0x020001AB RID: 427
	[PublicizedFrom(EAccessModifier.Private)]
	public struct DamageState
	{
		// Token: 0x04000B6A RID: 2922
		public string objName;

		// Token: 0x04000B6B RID: 2923
		public float health;
	}
}
