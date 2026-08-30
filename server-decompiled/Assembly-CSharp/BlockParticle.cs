using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000132 RID: 306
[Preserve]
public class BlockParticle : Block
{
	// Token: 0x06000847 RID: 2119 RVA: 0x0003ADBC File Offset: 0x00038FBC
	public BlockParticle()
	{
		base.IsNotifyOnLoadUnload = true;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0003ADCC File Offset: 0x00038FCC
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("ParticleName"))
		{
			this.particleName = base.Properties.Values["ParticleName"];
			ParticleEffect.LoadAsset(this.particleName);
		}
		if (base.Properties.Values.ContainsKey("ParticleOffset"))
		{
			this.offset = StringParsers.ParseVector3(base.Properties.Values["ParticleOffset"], 0, -1);
		}
		if (GameManager.IsDedicatedServer && this.particleName != null && this.particleName.Length > 0)
		{
			if (BlockParticle.particleLights == null)
			{
				BlockParticle.particleLights = new Dictionary<int, List<Light>>();
			}
			this.particleId = ParticleEffect.ToId(this.particleName);
			if (!BlockParticle.particleLights.ContainsKey(this.particleId))
			{
				BlockParticle.particleLights.Add(this.particleId, new List<Light>());
				Transform dynamicTransform = ParticleEffect.GetDynamicTransform(this.particleId);
				if (dynamicTransform != null)
				{
					Light[] componentsInChildren = dynamicTransform.GetComponentsInChildren<Light>();
					if (componentsInChildren != null)
					{
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							BlockParticle.particleLights[this.particleId].Add(componentsInChildren[i]);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0003AF09 File Offset: 0x00039109
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
		this.removeParticles(_world, _blockPos.x, _blockPos.y, _blockPos.z, _blockValue);
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0003AF31 File Offset: 0x00039131
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (!_chunk.NeedsDecoration)
		{
			this.checkParticles(_world, _blockPos, _blockValue);
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0003AF54 File Offset: 0x00039154
	public override void OnNeighborBlockChange(WorldBase world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		Transform blockParticleEffect;
		if (_myBlockPos == _blockPosThatChanged + Vector3i.up && _newNeighborBlockValue.Block.shape.IsTerrain() && _myBlockValue.Block.IsTerrainDecoration && this.particleName != null && (blockParticleEffect = world.GetGameManager().GetBlockParticleEffect(_myBlockPos)) != null)
		{
			float num = 0f;
			if (_myBlockPos.y > 0)
			{
				sbyte density = world.GetDensity(_myBlockPos.x, _myBlockPos.y, _myBlockPos.z);
				sbyte density2 = world.GetDensity(_myBlockPos.x, _myBlockPos.y - 1, _myBlockPos.z);
				num = MarchingCubes.GetDecorationOffsetY(density, density2);
			}
			blockParticleEffect.localPosition = new Vector3((float)_myBlockPos.x, (float)_myBlockPos.y + num, (float)_myBlockPos.z) + this.getParticleOffset(_myBlockValue);
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0003B038 File Offset: 0x00039238
	public override void OnBlockValueChanged(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		Transform blockParticleEffect;
		if (_oldBlockValue.rotation != _newBlockValue.rotation && this.particleName != null && (blockParticleEffect = world.GetGameManager().GetBlockParticleEffect(_blockPos)) != null)
		{
			Vector3 particleOffset = this.getParticleOffset(_oldBlockValue);
			Vector3 particleOffset2 = this.getParticleOffset(_newBlockValue);
			blockParticleEffect.localPosition -= particleOffset;
			blockParticleEffect.localPosition += particleOffset2;
			blockParticleEffect.localRotation = this.shape.GetRotation(_newBlockValue);
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0003B0CC File Offset: 0x000392CC
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if (this.particleName != null)
		{
			if (GameManager.IsDedicatedServer && BlockParticle.particleLights.ContainsKey(this.particleId))
			{
				List<Light> list = BlockParticle.particleLights[this.particleId];
				if (list.Count > 0)
				{
					Vector3 vector;
					vector.x = (float)_blockPos.x;
					vector.y = (float)_blockPos.y;
					vector.z = (float)_blockPos.z;
					this.dediLights = new List<Light>();
					for (int i = 0; i < list.Count; i++)
					{
						Light light = global::UnityEngine.Object.Instantiate<Light>(list[i]);
						Transform transform = light.transform;
						transform.position = vector + this.getParticleOffset(_blockValue) - Origin.position;
						transform.parent = BlockParticle.hierarchyParentT;
						this.dediLights.Add(light);
						LightManager.RegisterLight(light);
					}
				}
			}
			this.checkParticles(_world, _blockPos, _blockValue);
		}
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0003B1C5 File Offset: 0x000393C5
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
		this.removeParticles(_world, _blockPos.x, _blockPos.y, _blockPos.z, _blockValue);
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003B1EC File Offset: 0x000393EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 getParticleOffset(BlockValue _blockValue)
	{
		return this.shape.GetRotation(_blockValue) * (this.offset - new Vector3(0.5f, 0.5f, 0.5f)) + new Vector3(0.5f, 0.5f, 0.5f);
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0003B242 File Offset: 0x00039442
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void checkParticles(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (this.particleName != null && !_world.GetGameManager().HasBlockParticleEffect(_blockPos))
		{
			this.addParticles(_world, _blockPos.x, _blockPos.y, _blockPos.z, _blockValue);
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0003B274 File Offset: 0x00039474
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void addParticles(WorldBase _world, int _x, int _y, int _z, BlockValue _blockValue)
	{
		if (this.particleName == null || this.particleName == "")
		{
			return;
		}
		float num = 0f;
		if (_y > 0 && _blockValue.Block.IsTerrainDecoration && _world.GetBlock(_x, _y - 1, _z).Block.shape.IsTerrain())
		{
			sbyte density = _world.GetDensity(_x, _y, _z);
			sbyte density2 = _world.GetDensity(_x, _y - 1, _z);
			num = MarchingCubes.GetDecorationOffsetY(density, density2);
		}
		_world.GetGameManager().SpawnBlockParticleEffect(new Vector3i(_x, _y, _z), new ParticleEffect(this.particleName, new Vector3((float)_x, (float)_y + num, (float)_z) + this.getParticleOffset(_blockValue), this.shape.GetRotation(_blockValue), 1f, Color.white));
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003B344 File Offset: 0x00039544
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void removeParticles(WorldBase _world, int _x, int _y, int _z, BlockValue _blockValue)
	{
		if (GameManager.IsDedicatedServer && this.dediLights != null)
		{
			for (int i = 0; i < this.dediLights.Count; i++)
			{
				LightManager.UnRegisterLight(this.dediLights[i].transform.position + Origin.position, this.dediLights[i].range);
				global::UnityEngine.Object.Destroy(this.dediLights[i]);
			}
			this.dediLights.Clear();
		}
		_world.GetGameManager().RemoveBlockParticleEffect(new Vector3i(_x, _y, _z));
	}

	// Token: 0x0400094C RID: 2380
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleName;

	// Token: 0x0400094D RID: 2381
	[PublicizedFrom(EAccessModifier.Private)]
	public int particleId;

	// Token: 0x0400094E RID: 2382
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> dediLights;

	// Token: 0x0400094F RID: 2383
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 offset;

	// Token: 0x04000950 RID: 2384
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform hierarchyParentT = new GameObject("BlockParticleLights").transform;

	// Token: 0x04000951 RID: 2385
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, List<Light>> particleLights;
}
