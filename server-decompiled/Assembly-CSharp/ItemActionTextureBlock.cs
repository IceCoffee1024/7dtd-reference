using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using GUI_2;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005DA RID: 1498
[Preserve]
public class ItemActionTextureBlock : ItemActionRanged
{
	// Token: 0x060030B0 RID: 12464 RVA: 0x0013B5FA File Offset: 0x001397FA
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionTextureBlock.ItemActionTextureBlockData(_invData, _indexInEntityOfAction, "Muzzle/Particle1");
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x0013B608 File Offset: 0x00139808
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("RemoveTexture"))
		{
			this.bRemoveTexture = StringParsers.ParseBool(_props.Values["RemoveTexture"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("DefaultTextureID"))
		{
			this.DefaultTextureID = Convert.ToInt32(_props.Values["DefaultTextureID"]);
		}
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x0013B67C File Offset: 0x0013987C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int getUserData(ItemActionData _actionData)
	{
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_actionData;
		int textureID = (int)BlockTextureData.list[itemActionTextureBlockData.idx].TextureID;
		Color color;
		if (textureID == 0)
		{
			color = Color.gray;
		}
		else
		{
			color = MeshDescription.meshes[0].textureAtlas.uvMapping[textureID].color;
		}
		return ((int)(color.r * 255f) & 255) | (((int)(color.g * 255f) << 8) & 65280) | (((int)(color.b * 255f) << 16) & 16711680);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x0013B70C File Offset: 0x0013990C
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		base.ItemActionEffects(_gameManager, _actionData, _firingState, _startPos, _direction, _userData);
		if (_firingState != 0 && _actionData.invData.model != null)
		{
			ParticleSystem[] componentsInChildren = _actionData.invData.model.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer component = componentsInChildren[i].GetComponent<Renderer>();
				if (component != null)
				{
					component.material.SetColor("_Color", new Color32((byte)(_userData & 255), (byte)((_userData >> 8) & 255), (byte)((_userData >> 16) & 255), byte.MaxValue));
				}
			}
		}
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x0013B7B1 File Offset: 0x001399B1
	public override void StartHolding(ItemActionData _data)
	{
		base.StartHolding(_data);
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_data;
		itemActionTextureBlockData.idx = itemActionTextureBlockData.invData.itemValue.Meta;
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ConsumeScrollWheel(ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x0013B7D8 File Offset: 0x001399D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool checkAmmo(ItemActionData _actionData)
	{
		if (this.InfiniteAmmo || GameStats.GetInt(EnumGameStats.GameModeId) == 2 || GameStats.GetInt(EnumGameStats.GameModeId) == 8)
		{
			return true;
		}
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		return holdingEntity.bag.GetItemCount(this.currentMagazineItem, -1, -1, true) > 0 || holdingEntity.inventory.GetItemCount(this.currentMagazineItem, false, -1, -1, true) > 0;
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x0013B840 File Offset: 0x00139A40
	[PublicizedFrom(EAccessModifier.Private)]
	public bool decreaseAmmo(ItemActionData _actionData)
	{
		if (this.InfiniteAmmo)
		{
			return true;
		}
		if (GameStats.GetInt(EnumGameStats.GameModeId) == 2 || GameStats.GetInt(EnumGameStats.GameModeId) == 8)
		{
			return true;
		}
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_actionData;
		int num = (int)BlockTextureData.list[itemActionTextureBlockData.idx].PaintCost;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue itemValue = this.currentMagazineItem;
		int itemCount = holdingEntity.bag.GetItemCount(itemValue, -1, -1, true);
		int itemCount2 = holdingEntity.inventory.GetItemCount(itemValue, false, -1, -1, true);
		if (itemCount + itemCount2 >= num)
		{
			num -= holdingEntity.bag.DecItem(itemValue, num, false, null);
			if (num > 0)
			{
				holdingEntity.inventory.DecItem(itemValue, num, false, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ConsumeAmmo(ItemActionData _actionData)
	{
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x0013B8E8 File Offset: 0x00139AE8
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		base.OnHoldingUpdate(_actionData);
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_actionData;
		if (itemActionTextureBlockData.bReplacePaintNextTime && Time.time - itemActionTextureBlockData.lastTimeReplacePaintShown > 5f)
		{
			itemActionTextureBlockData.lastTimeReplacePaintShown = Time.time;
			GameManager.ShowTooltip(GameManager.Instance.World.GetLocalPlayers()[0], Localization.Get("ttPaintedTextureReplaced", false, null), false, false, 0f);
		}
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x0013B958 File Offset: 0x00139B58
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemValue holdingItemItemValue = _actionData.invData.holdingEntity.inventory.holdingItemItemValue;
		this.currentMagazineItem = ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false);
		if ((double)_actionData.invData.holdingEntity.speedForward > 0.009)
		{
			this.rayCastDelay = AnimationDelayData.AnimationDelay[_actionData.invData.item.HoldType.Value].RayCastMoving;
		}
		else
		{
			this.rayCastDelay = AnimationDelayData.AnimationDelay[_actionData.invData.item.HoldType.Value].RayCast;
		}
		base.ExecuteAction(_actionData, _bReleased);
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x0013BA0E File Offset: 0x00139C0E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntity)
	{
		hitEntity = true;
		GameManager.Instance.StartCoroutine(this.fireShotLater(_shotIdx, _actionData));
		return Vector3.zero;
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x0013BA2B File Offset: 0x00139C2B
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x0013BA40 File Offset: 0x00139C40
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkBlockCanBeChanged(World _world, Vector3i _blockPos, PersistentPlayerData lpRelative)
	{
		return _world.CanPlaceBlockAt(_blockPos, lpRelative, false);
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x0013BA4C File Offset: 0x00139C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkBlockCanBePainted(World _world, Vector3i _blockPos, BlockValue _blockValue, PersistentPlayerData _lpRelative)
	{
		if (_blockValue.isair)
		{
			return false;
		}
		Block block = _blockValue.Block;
		return block.shape is BlockShapeNew && block.MeshIndex == 0 && this.checkBlockCanBeChanged(_world, _blockPos, _lpRelative);
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x0013BA90 File Offset: 0x00139C90
	[PublicizedFrom(EAccessModifier.Private)]
	public void getParentBlock(ref BlockValue _blockValue, ref Vector3i _blockPos, ChunkCluster _cc)
	{
		Block block = _blockValue.Block;
		if (_blockValue.ischild)
		{
			Log.Warning("Trying to paint multiblock block: " + _blockValue.Block.GetBlockName());
			_blockPos = block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			_blockValue = _cc.GetBlock(_blockPos);
		}
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x0013BAF8 File Offset: 0x00139CF8
	[PublicizedFrom(EAccessModifier.Private)]
	public int getCurrentPaintIdx(ChunkCluster _cc, Vector3i _blockPos, BlockFace _blockFace, BlockValue _blockValue, int _channel)
	{
		int blockFaceTexture = _cc.GetBlockFaceTexture(_blockPos, _blockFace, _channel);
		if (blockFaceTexture != 0)
		{
			return blockFaceTexture;
		}
		string text;
		return GameUtils.FindPaintIdForBlockFace(_blockValue, _blockFace, out text, _channel);
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x0013BB24 File Offset: 0x00139D24
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionTextureBlock.EPaintResult paintFace(ChunkCluster _cc, int _entityId, ItemActionTextureBlock.ItemActionTextureBlockData _actionData, Vector3i _blockPos, BlockFace _blockFace, BlockValue _blockValue, ItemActionTextureBlock.ChannelMask _channelMask)
	{
		ItemActionTextureBlock.EPaintResult epaintResult = ItemActionTextureBlock.EPaintResult.SamePaint;
		for (int i = 0; i < 1; i++)
		{
			if (_channelMask.IncludesChannel(i))
			{
				int currentPaintIdx = this.getCurrentPaintIdx(_cc, _blockPos, _blockFace, _blockValue, i);
				if (_actionData.idx != currentPaintIdx)
				{
					if (!this.decreaseAmmo(_actionData))
					{
						return ItemActionTextureBlock.EPaintResult.NoPaintAvailable;
					}
					GameManager.Instance.SetBlockTextureServer(_blockPos, _blockFace, _actionData.idx, _entityId, (byte)i);
					epaintResult = ItemActionTextureBlock.EPaintResult.Painted;
				}
			}
		}
		return epaintResult;
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x0013BB8C File Offset: 0x00139D8C
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionTextureBlock.EPaintResult paintBlock(World _world, ChunkCluster _cc, int _entityId, ItemActionTextureBlock.ItemActionTextureBlockData _actionData, Vector3i _blockPos, BlockFace _blockFace, BlockValue _blockValue, PersistentPlayerData _lpRelative, ItemActionTextureBlock.ChannelMask _channelMask)
	{
		this.getParentBlock(ref _blockValue, ref _blockPos, _cc);
		if (!this.checkBlockCanBePainted(_world, _blockPos, _blockValue, _lpRelative))
		{
			return ItemActionTextureBlock.EPaintResult.CanNotPaint;
		}
		if (BlockToolSelection.Instance.SelectionActive)
		{
			BoundsInt boundsInt = new BoundsInt(BlockToolSelection.Instance.SelectionMin, BlockToolSelection.Instance.SelectionSize);
			if (!boundsInt.Contains(_blockPos))
			{
				return ItemActionTextureBlock.EPaintResult.CanNotPaint;
			}
		}
		if (!_actionData.bPaintAllSides)
		{
			return this.paintFace(_cc, _entityId, _actionData, _blockPos, _blockFace, _blockValue, _channelMask);
		}
		int num = 0;
		for (int i = 0; i <= 5; i++)
		{
			_blockFace = (BlockFace)i;
			ItemActionTextureBlock.EPaintResult epaintResult = this.paintFace(_cc, _entityId, _actionData, _blockPos, _blockFace, _blockValue, _channelMask);
			if (epaintResult == ItemActionTextureBlock.EPaintResult.NoPaintAvailable)
			{
				return epaintResult;
			}
			if (epaintResult == ItemActionTextureBlock.EPaintResult.Painted)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return ItemActionTextureBlock.EPaintResult.SamePaint;
		}
		return ItemActionTextureBlock.EPaintResult.Painted;
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x0013BC50 File Offset: 0x00139E50
	[PublicizedFrom(EAccessModifier.Private)]
	public void floodFill(World _world, ChunkCluster _cc, int _entityId, ItemActionTextureBlock.ItemActionTextureBlockData _actionData, PersistentPlayerData _lpRelative, int _sourcePaint, Vector3 _hitPosition, Vector3 _hitFaceNormal, Vector3 _dir1, Vector3 _dir2, int _channel)
	{
		this.visitedPositions.Clear();
		this.visitedRays.Clear();
		this.positionsToCheck.Clear();
		this.positionsToCheck.Push(new Vector2i(0, 0));
		while (this.positionsToCheck.Count > 0)
		{
			Vector2i vector2i = this.positionsToCheck.Pop();
			if (!this.visitedRays.ContainsKey(vector2i))
			{
				this.visitedRays.Add(vector2i, true);
				Vector3 vector = _hitPosition + _hitFaceNormal * 0.2f + (float)vector2i.x * _dir1 + (float)vector2i.y * _dir2;
				Vector3 vector2 = -_hitFaceNormal * 0.3f;
				float magnitude = vector2.magnitude;
				if (Voxel.Raycast(_world, new Ray(vector, vector2), magnitude, -555528205, 69, 0f))
				{
					this.worldRayHitInfo.CopyFrom(Voxel.voxelRayHitInfo);
					BlockValue blockValue = this.worldRayHitInfo.hit.blockValue;
					Vector3i blockPos = this.worldRayHitInfo.hit.blockPos;
					bool flag2;
					bool flag;
					if (this.worldRayHitInfo.hitTriangleIdx >= 0 && (!(flag = this.visitedPositions.TryGetValue(blockPos, out flag2)) || flag2))
					{
						if (!flag)
						{
							Vector3 vector3;
							Vector3 normalized;
							BlockFace blockFaceFromHitInfo = GameUtils.GetBlockFaceFromHitInfo(blockPos, blockValue, this.worldRayHitInfo.hitCollider, this.worldRayHitInfo.hitTriangleIdx, out vector3, out normalized);
							if (blockFaceFromHitInfo == BlockFace.None)
							{
								continue;
							}
							normalized = normalized.normalized;
							if ((double)(normalized - _hitFaceNormal).sqrMagnitude > 0.01)
							{
								continue;
							}
							if (this.getCurrentPaintIdx(_cc, blockPos, blockFaceFromHitInfo, blockValue, _channel) != _sourcePaint)
							{
								this.visitedPositions.Add(blockPos, false);
								continue;
							}
							ItemActionTextureBlock.EPaintResult epaintResult = this.paintBlock(_world, _cc, _entityId, _actionData, blockPos, blockFaceFromHitInfo, blockValue, _lpRelative, new ItemActionTextureBlock.ChannelMask(_channel));
							if (epaintResult == ItemActionTextureBlock.EPaintResult.CanNotPaint || epaintResult == ItemActionTextureBlock.EPaintResult.NoPaintAvailable)
							{
								this.visitedPositions.Add(blockPos, false);
								continue;
							}
							this.visitedPositions.Add(blockPos, true);
						}
						this.positionsToCheck.Push(vector2i + Vector2i.down);
						this.positionsToCheck.Push(vector2i + Vector2i.up);
						this.positionsToCheck.Push(vector2i + Vector2i.left);
						this.positionsToCheck.Push(vector2i + Vector2i.right);
					}
				}
			}
		}
		this.visitedPositions.Clear();
		this.visitedRays.Clear();
		this.positionsToCheck.Clear();
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x0013BEE7 File Offset: 0x0013A0E7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator fireShotLater(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData)
	{
		yield return new WaitForSeconds(this.rayCastDelay);
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		PersistentPlayerData playerDataFromEntityID = GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(holdingEntity.entityId);
		Vector3 vector = holdingEntity.GetLookVector((_actionData.muzzle != null) ? _actionData.muzzle.forward : Vector3.zero);
		Vector3i blockPos;
		BlockValue blockValue;
		BlockFace blockFaceFromHitInfo;
		WorldRayHitInfo worldRayHitInfo;
		if (this.getHitBlockFace(_actionData, out blockPos, out blockValue, out blockFaceFromHitInfo, out worldRayHitInfo) == -1 || worldRayHitInfo == null || !worldRayHitInfo.bHitValid)
		{
			yield break;
		}
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_actionData;
		ItemInventoryData invData = itemActionTextureBlockData.invData;
		if (this.bRemoveTexture)
		{
			itemActionTextureBlockData.idx = 0;
		}
		World world = GameManager.Instance.World;
		ChunkCluster chunkCache = world.ChunkCache;
		if (chunkCache == null)
		{
			yield break;
		}
		BlockToolSelection.Instance.BeginUndo();
		if (!itemActionTextureBlockData.bReplacePaintNextTime)
		{
			switch (itemActionTextureBlockData.paintMode)
			{
			case ItemActionTextureBlock.EnumPaintMode.Single:
				this.paintBlock(world, chunkCache, holdingEntity.entityId, itemActionTextureBlockData, blockPos, blockFaceFromHitInfo, blockValue, playerDataFromEntityID, itemActionTextureBlockData.channelMask);
				break;
			case ItemActionTextureBlock.EnumPaintMode.Multiple:
			case ItemActionTextureBlock.EnumPaintMode.Spray:
			{
				float num = ((itemActionTextureBlockData.paintMode == ItemActionTextureBlock.EnumPaintMode.Spray) ? 7.5f : 1.25f);
				if (worldRayHitInfo.hitTriangleIdx != -1)
				{
					Vector3 vector2;
					Vector3 normalFromHitInfo = GameUtils.GetNormalFromHitInfo(blockPos, worldRayHitInfo.hitCollider, worldRayHitInfo.hitTriangleIdx, out vector2);
					Vector3 normalized = normalFromHitInfo.normalized;
					Vector3 vector3;
					Vector3 vector4;
					if (Utils.FastAbs(normalized.x) >= Utils.FastAbs(normalized.y) && Utils.FastAbs(normalized.x) >= Utils.FastAbs(normalized.z))
					{
						vector3 = Vector3.up;
						vector4 = Vector3.forward;
					}
					else if (Utils.FastAbs(normalized.y) >= Utils.FastAbs(normalized.x) && Utils.FastAbs(normalized.y) >= Utils.FastAbs(normalized.z))
					{
						vector3 = Vector3.right;
						vector4 = Vector3.forward;
					}
					else
					{
						vector3 = Vector3.right;
						vector4 = Vector3.up;
					}
					vector2 = ItemActionTextureBlock.ProjectVectorOnPlane(normalized, vector3);
					vector3 = vector2.normalized;
					vector2 = ItemActionTextureBlock.ProjectVectorOnPlane(normalized, vector4);
					vector4 = vector2.normalized;
					Vector3 pos = worldRayHitInfo.hit.pos;
					Vector3 origin = worldRayHitInfo.ray.origin;
					for (float num2 = -num; num2 <= num; num2 += 0.5f)
					{
						for (float num3 = -num; num3 <= num; num3 += 0.5f)
						{
							vector = pos + num2 * vector3 + num3 * vector4 - origin;
							int num4 = 69;
							if (Voxel.Raycast(world, new Ray(origin, vector), this.Range, -555528205, num4, 0f))
							{
								WorldRayHitInfo worldRayHitInfo2 = Voxel.voxelRayHitInfo.Clone();
								blockValue = worldRayHitInfo2.hit.blockValue;
								blockPos = worldRayHitInfo2.hit.blockPos;
								blockFaceFromHitInfo = GameUtils.GetBlockFaceFromHitInfo(blockPos, blockValue, worldRayHitInfo2.hitCollider, worldRayHitInfo2.hitTriangleIdx, out vector2, out normalFromHitInfo);
								if (blockFaceFromHitInfo != BlockFace.None)
								{
									this.paintBlock(world, chunkCache, holdingEntity.entityId, itemActionTextureBlockData, blockPos, blockFaceFromHitInfo, blockValue, playerDataFromEntityID, itemActionTextureBlockData.channelMask);
								}
							}
						}
					}
				}
				break;
			}
			case ItemActionTextureBlock.EnumPaintMode.Fill:
			{
				Vector3 vector5;
				Vector3 vector2 = GameUtils.GetNormalFromHitInfo(blockPos, worldRayHitInfo.hitCollider, worldRayHitInfo.hitTriangleIdx, out vector5);
				Vector3 normalized2 = vector2.normalized;
				Vector3 vector6;
				Vector3 vector7;
				if (Utils.FastAbs(normalized2.x) >= Utils.FastAbs(normalized2.y) && Utils.FastAbs(normalized2.x) >= Utils.FastAbs(normalized2.z))
				{
					vector6 = Vector3.up;
					vector7 = Vector3.forward;
				}
				else if (Utils.FastAbs(normalized2.y) >= Utils.FastAbs(normalized2.x) && Utils.FastAbs(normalized2.y) >= Utils.FastAbs(normalized2.z))
				{
					vector6 = Vector3.right;
					vector7 = Vector3.forward;
				}
				else
				{
					vector6 = Vector3.right;
					vector7 = Vector3.up;
				}
				vector2 = ItemActionTextureBlock.ProjectVectorOnPlane(normalized2, vector6);
				vector6 = vector2.normalized * 0.3f;
				vector2 = ItemActionTextureBlock.ProjectVectorOnPlane(normalized2, vector7);
				vector7 = vector2.normalized * 0.3f;
				for (int i = 0; i < 1; i++)
				{
					if (itemActionTextureBlockData.channelMask.IncludesChannel(i))
					{
						int num5 = chunkCache.GetBlockFaceTexture(blockPos, blockFaceFromHitInfo, i);
						if (itemActionTextureBlockData.idx != num5)
						{
							if (num5 == 0)
							{
								string text;
								num5 = GameUtils.FindPaintIdForBlockFace(blockValue, blockFaceFromHitInfo, out text, i);
							}
							if (itemActionTextureBlockData.idx != num5)
							{
								this.floodFill(world, chunkCache, holdingEntity.entityId, itemActionTextureBlockData, playerDataFromEntityID, num5, worldRayHitInfo.hit.pos, normalized2, vector6, vector7, i);
							}
						}
					}
				}
				break;
			}
			}
			BlockToolSelection.Instance.EndUndo(false);
			yield break;
		}
		itemActionTextureBlockData.bReplacePaintNextTime = false;
		if (!this.checkBlockCanBeChanged(world, blockPos, playerDataFromEntityID))
		{
			yield break;
		}
		for (int j = 0; j < 1; j++)
		{
			if (itemActionTextureBlockData.channelMask.IncludesChannel(j))
			{
				int num6 = chunkCache.GetBlockFaceTexture(blockPos, blockFaceFromHitInfo, j);
				if (itemActionTextureBlockData.idx != num6)
				{
					if (num6 == 0)
					{
						string text;
						num6 = GameUtils.FindPaintIdForBlockFace(blockValue, blockFaceFromHitInfo, out text, j);
					}
					if (num6 != itemActionTextureBlockData.idx)
					{
						BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
						if (blockToolSelection == null || !blockToolSelection.SelectionActive)
						{
							this.replacePaintInCurrentPrefab(blockPos, blockFaceFromHitInfo, num6, itemActionTextureBlockData.idx, j);
						}
						else
						{
							this.replacePaintInCurrentSelection(blockPos, blockFaceFromHitInfo, num6, itemActionTextureBlockData.idx, j);
						}
					}
				}
			}
		}
		BlockToolSelection.Instance.EndUndo(false);
		yield break;
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x0013BF00 File Offset: 0x0013A100
	[PublicizedFrom(EAccessModifier.Private)]
	public int getHitBlockFace(ItemActionRanged.ItemActionDataRanged _actionData, out Vector3i blockPos, out BlockValue bv, out BlockFace blockFace, out WorldRayHitInfo hitInfo)
	{
		int num;
		using (ItemActionTextureBlock.s_MarkerGetHitBlockFace.Auto())
		{
			bv = BlockValue.Air;
			blockFace = BlockFace.None;
			hitInfo = null;
			blockPos = Vector3i.zero;
			hitInfo = this.GetExecuteActionTarget(_actionData);
			if (hitInfo == null || !hitInfo.bHitValid || hitInfo.tag == null || !GameUtils.IsBlockOrTerrain(hitInfo.tag))
			{
				num = -1;
			}
			else
			{
				ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
				if (chunkCache == null)
				{
					num = -1;
				}
				else
				{
					bv = hitInfo.hit.blockValue;
					blockPos = hitInfo.hit.blockPos;
					Block block = bv.Block;
					if (bv.ischild)
					{
						blockPos = block.multiBlockPos.GetParentPos(blockPos, bv);
						bv = chunkCache.GetBlock(blockPos);
					}
					if (bv.Block.MeshIndex != 0)
					{
						num = -1;
					}
					else
					{
						blockFace = BlockFace.Top;
						BlockShapeNew blockShapeNew = bv.Block.shape as BlockShapeNew;
						if (blockShapeNew != null)
						{
							Vector3 vector;
							Vector3 vector2;
							blockFace = GameUtils.GetBlockFaceFromHitInfo(blockPos, bv, hitInfo.hitCollider, hitInfo.hitTriangleIdx, out vector, out vector2);
						}
						if (blockFace == BlockFace.None)
						{
							num = -1;
						}
						else
						{
							ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = _actionData as ItemActionTextureBlock.ItemActionTextureBlockData;
							if (itemActionTextureBlockData != null)
							{
								using (ItemActionTextureBlock.s_MarkerAutoChannel.Auto())
								{
									if (itemActionTextureBlockData.bAutoChannel && blockShapeNew != null)
									{
										int visualMeshChannelFromHitInfo = blockShapeNew.GetVisualMeshChannelFromHitInfo(blockPos, bv, blockFace, hitInfo);
										if (visualMeshChannelFromHitInfo < 0)
										{
											return -1;
										}
										itemActionTextureBlockData.channelMask.SetExclusiveChannel(visualMeshChannelFromHitInfo);
										return chunkCache.GetBlockFaceTexture(blockPos, blockFace, visualMeshChannelFromHitInfo);
									}
								}
								ItemActionTextureBlock.ChannelMask channelMask = itemActionTextureBlockData.channelMask;
								for (int i = 0; i < 1; i++)
								{
									if (channelMask.IncludesChannel(i))
									{
										int blockFaceTexture = chunkCache.GetBlockFaceTexture(blockPos, blockFace, i);
										if (blockFaceTexture != -1)
										{
											return blockFaceTexture;
										}
									}
								}
								num = -1;
							}
							else
							{
								num = chunkCache.GetBlockFaceTexture(blockPos, blockFace, 0);
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060030C6 RID: 12486 RVA: 0x0013C188 File Offset: 0x0013A388
	public void CopyTextureFromWorld(ItemActionTextureBlock.ItemActionTextureBlockData _actionData)
	{
		if (!(_actionData.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		Vector3i vector3i;
		BlockValue blockValue;
		BlockFace blockFace;
		WorldRayHitInfo worldRayHitInfo;
		int num = this.getHitBlockFace(_actionData, out vector3i, out blockValue, out blockFace, out worldRayHitInfo);
		if (num == -1)
		{
			return;
		}
		if (num == 0)
		{
			for (int i = 0; i < 1; i++)
			{
				if (_actionData.channelMask.IncludesChannel(i))
				{
					string text;
					num = GameUtils.FindPaintIdForBlockFace(blockValue, blockFace, out text, i);
					if (num != 0)
					{
						break;
					}
				}
			}
		}
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		BlockTextureData blockTextureData = BlockTextureData.list[num];
		if (blockTextureData != null && !blockTextureData.GetLocked(entityPlayerLocal))
		{
			_actionData.idx = num;
			_actionData.invData.itemValue.Meta = num;
			_actionData.invData.itemValue = _actionData.invData.itemValue;
			return;
		}
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
		GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("ttPaintTextureIsLocked", false, null), false, false, 0f);
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x0013C284 File Offset: 0x0013A484
	public void CopyBlockFromWorld(ItemActionRanged.ItemActionDataRanged _actionData)
	{
		if (!(_actionData.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		WorldRayHitInfo executeActionTarget = this.GetExecuteActionTarget(_actionData);
		if (executeActionTarget == null || !executeActionTarget.bHitValid || executeActionTarget.tag == null || !GameUtils.IsBlockOrTerrain(executeActionTarget.tag))
		{
			return;
		}
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		BlockValue blockValue = executeActionTarget.hit.blockValue;
		Vector3i vector3i = executeActionTarget.hit.blockPos;
		Block block = blockValue.Block;
		if (blockValue.ischild)
		{
			vector3i = block.multiBlockPos.GetParentPos(vector3i, blockValue);
			blockValue = chunkCache.GetBlock(vector3i);
		}
		if (blockValue.Block.MeshIndex != 0)
		{
			return;
		}
		ItemValue itemValue = executeActionTarget.hit.blockValue.ToItemValue();
		itemValue.TextureFullArray = chunkCache.GetTextureFullArray(vector3i);
		ItemStack itemStack = new ItemStack(itemValue, 99);
		_actionData.invData.holdingEntity.inventory.AddItem(itemStack);
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x001383F9 File Offset: 0x001365F9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void onHoldingEntityFired(ItemActionData _actionData)
	{
		if (_actionData.indexInEntityOfAction == 0)
		{
			_actionData.invData.holdingEntity.RightArmAnimationUse = true;
			return;
		}
		_actionData.invData.holdingEntity.RightArmAnimationAttack = true;
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x0013C374 File Offset: 0x0013A574
	[PublicizedFrom(EAccessModifier.Private)]
	public void replacePaintInCurrentPrefab(Vector3i _blockPos, BlockFace _blockFace, int _searchPaintId, int _replacePaintId, int _channel)
	{
		World world = GameManager.Instance.World;
		DynamicPrefabDecorator dynamicPrefabDecorator = world.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator == null)
		{
			return;
		}
		PrefabInstance prefabFromWorldPosInside = dynamicPrefabDecorator.GetPrefabFromWorldPosInside(_blockPos.x, _blockPos.z);
		if (prefabFromWorldPosInside == null)
		{
			return;
		}
		for (int i = prefabFromWorldPosInside.boundingBoxPosition.x; i <= prefabFromWorldPosInside.boundingBoxPosition.x + prefabFromWorldPosInside.boundingBoxSize.x; i++)
		{
			for (int j = prefabFromWorldPosInside.boundingBoxPosition.z; j <= prefabFromWorldPosInside.boundingBoxPosition.z + prefabFromWorldPosInside.boundingBoxSize.z; j++)
			{
				for (int k = 0; k < 256; k++)
				{
					BlockValue block = world.GetBlock(i, k, j);
					if (!block.isair)
					{
						long num = world.GetTexture(i, k, j, _channel);
						bool flag = false;
						for (int l = 0; l < 6; l++)
						{
							int num2 = (int)((num >> l * 8) & 255L);
							if (num2 == 0)
							{
								string text;
								num2 = GameUtils.FindPaintIdForBlockFace(block, (BlockFace)l, out text, _channel);
							}
							if (num2 == _searchPaintId)
							{
								num &= ~(255L << l * 8);
								num |= (long)_replacePaintId << l * 8;
								flag = true;
							}
						}
						if (flag)
						{
							world.SetTexture(i, k, j, num, _channel);
						}
					}
				}
			}
		}
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x0013C4DC File Offset: 0x0013A6DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void replacePaintInCurrentSelection(Vector3i _blockPos, BlockFace _blockFace, int _searchPaintId, int _replacePaintId, int _channel)
	{
		BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
		if (blockToolSelection == null)
		{
			return;
		}
		World world = GameManager.Instance.World;
		Vector3i selectionMin = blockToolSelection.SelectionMin;
		for (int i = selectionMin.x; i < selectionMin.x + blockToolSelection.SelectionSize.x; i++)
		{
			for (int j = selectionMin.z; j < selectionMin.z + blockToolSelection.SelectionSize.z; j++)
			{
				for (int k = selectionMin.y; k < selectionMin.y + blockToolSelection.SelectionSize.y; k++)
				{
					BlockValue block = world.GetBlock(i, k, j);
					if (!block.isair)
					{
						long num = world.GetTexture(i, k, j, _channel);
						bool flag = false;
						for (int l = 0; l < 6; l++)
						{
							int num2 = (int)((num >> l * 8) & 255L);
							if (num2 == 0)
							{
								string text;
								num2 = GameUtils.FindPaintIdForBlockFace(block, (BlockFace)l, out text, _channel);
							}
							if (num2 == _searchPaintId)
							{
								num &= ~(255L << l * 8);
								num |= (long)_replacePaintId << l * 8;
								flag = true;
							}
						}
						if (flag)
						{
							world.SetTexture(i, k, j, num, _channel);
						}
					}
				}
			}
		}
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x00010E62 File Offset: 0x0000F062
	public override EnumCameraShake GetCameraShakeType(ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool ShowAmmoInUI()
	{
		return true;
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x0013C634 File Offset: 0x0013A834
	public override void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)_epl.inventory.holdingItemData.actionData[1];
		_xuiRadialWindow.ResetRadialEntries();
		object obj = GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled);
		_xuiRadialWindow.CreateRadialEntry(0, "ui_game_symbol_paint_bucket", "UIAtlas", "", Localization.Get("xuiMaterials", false, null), false);
		_xuiRadialWindow.CreateRadialEntry(1, "ui_game_symbol_paint_brush", "UIAtlas", "", Localization.Get("xuiPaintBrush", false, null), itemActionTextureBlockData.paintMode == ItemActionTextureBlock.EnumPaintMode.Single);
		_xuiRadialWindow.CreateRadialEntry(2, "ui_game_symbol_paint_roller", "UIAtlas", "", Localization.Get("xuiPaintRoller", false, null), itemActionTextureBlockData.paintMode == ItemActionTextureBlock.EnumPaintMode.Multiple);
		_xuiRadialWindow.CreateRadialEntry(8, "ui_game_symbol_flood_fill", "UIAtlas", "", Localization.Get("xuiPaintFill", false, null), itemActionTextureBlockData.paintMode == ItemActionTextureBlock.EnumPaintMode.Fill);
		object obj2 = obj;
		if (obj2 != null)
		{
			_xuiRadialWindow.CreateRadialEntry(3, "ui_game_symbol_paint_spraygun", "UIAtlas", "", Localization.Get("xuiSprayGun", false, null), itemActionTextureBlockData.paintMode == ItemActionTextureBlock.EnumPaintMode.Spray);
		}
		_xuiRadialWindow.CreateRadialEntry(4, "ui_game_symbol_paint_allsides", "UIAtlas", "", Localization.Get("xuiPaintAllSides", false, null), itemActionTextureBlockData.bPaintAllSides);
		_xuiRadialWindow.CreateRadialEntry(5, "ui_game_symbol_paint_eyedropper", "UIAtlas", "", Localization.Get("xuiTexturePicker", false, null), false);
		if (obj2 != null)
		{
			_xuiRadialWindow.CreateRadialEntry(6, "ui_game_symbol_paint_copy_block", "UIAtlas", "", Localization.Get("xuiCopyBlock", false, null), false);
			_xuiRadialWindow.CreateRadialEntry(7, "ui_game_symbol_book", "UIAtlas", "", Localization.Get("xuiReplacePaint", false, null), itemActionTextureBlockData.bReplacePaintNextTime);
		}
		_xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonNorth, new XUiC_Radial.CommandHandlerDelegate(this.handleRadialCommand), new XUiC_Radial.RadialContextHoldingSlotIndex(_epl.inventory.holdingItemIdx), -1, false, new XUiC_Radial.RadialStillValidDelegate(XUiC_Radial.RadialValidSameHoldingSlotIndex));
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x0013C814 File Offset: 0x0013AA14
	[PublicizedFrom(EAccessModifier.Private)]
	public new void handleRadialCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		ItemClass holdingItem = entityPlayer.inventory.holdingItem;
		ItemInventoryData holdingItemData = entityPlayer.inventory.holdingItemData;
		if (!(holdingItem.Actions[0] is ItemActionTextureBlock) || !(holdingItem.Actions[1] is ItemActionTextureBlock))
		{
			return;
		}
		ItemActionTextureBlock itemActionTextureBlock = (ItemActionTextureBlock)holdingItem.Actions[0];
		ItemActionTextureBlock itemActionTextureBlock2 = (ItemActionTextureBlock)holdingItem.Actions[1];
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData = (ItemActionTextureBlock.ItemActionTextureBlockData)holdingItemData.actionData[0];
		ItemActionTextureBlock.ItemActionTextureBlockData itemActionTextureBlockData2 = (ItemActionTextureBlock.ItemActionTextureBlockData)holdingItemData.actionData[1];
		if (_commandIndex != 0 && _commandIndex != 7)
		{
			itemActionTextureBlockData2.bReplacePaintNextTime = false;
		}
		switch (_commandIndex)
		{
		case 0:
			_sender.xui.playerUI.windowManager.Open("materials", true);
			return;
		case 1:
			itemActionTextureBlockData.paintMode = (itemActionTextureBlockData2.paintMode = ItemActionTextureBlock.EnumPaintMode.Single);
			return;
		case 2:
			itemActionTextureBlockData.paintMode = (itemActionTextureBlockData2.paintMode = ItemActionTextureBlock.EnumPaintMode.Multiple);
			return;
		case 3:
			itemActionTextureBlockData.paintMode = (itemActionTextureBlockData2.paintMode = ItemActionTextureBlock.EnumPaintMode.Spray);
			return;
		case 4:
			itemActionTextureBlockData.bPaintAllSides = (itemActionTextureBlockData2.bPaintAllSides = !itemActionTextureBlockData2.bPaintAllSides);
			return;
		case 5:
			itemActionTextureBlock.CopyTextureFromWorld(itemActionTextureBlockData);
			itemActionTextureBlock2.CopyTextureFromWorld(itemActionTextureBlockData2);
			return;
		case 6:
			itemActionTextureBlock.CopyBlockFromWorld(itemActionTextureBlockData);
			itemActionTextureBlock2.CopyBlockFromWorld(itemActionTextureBlockData2);
			return;
		case 7:
			itemActionTextureBlockData2.bReplacePaintNextTime = !itemActionTextureBlockData2.bReplacePaintNextTime;
			return;
		case 8:
			itemActionTextureBlockData.paintMode = (itemActionTextureBlockData2.paintMode = ItemActionTextureBlock.EnumPaintMode.Fill);
			return;
		case 9:
			itemActionTextureBlockData.bAutoChannel = (itemActionTextureBlockData2.bAutoChannel = !itemActionTextureBlockData2.bAutoChannel);
			return;
		default:
		{
			int num = _commandIndex - 10;
			if (num >= 0 && num < 1)
			{
				itemActionTextureBlockData.bAutoChannel = (itemActionTextureBlockData2.bAutoChannel = false);
				if (InputUtils.ShiftKeyPressed)
				{
					itemActionTextureBlockData2.channelMask.ToggleChannel(num);
				}
				else
				{
					itemActionTextureBlockData2.channelMask.SetExclusiveChannel(num);
				}
				itemActionTextureBlockData.channelMask = itemActionTextureBlockData2.channelMask;
			}
			return;
		}
		}
	}

	// Token: 0x040025D5 RID: 9685
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_MarkerGetHitBlockFace = new ProfilerMarker("ItemActionTextureBlock.getHitBlockFace");

	// Token: 0x040025D6 RID: 9686
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_MarkerAutoChannel = new ProfilerMarker("ItemActionTextureBlock.autoChannel");

	// Token: 0x040025D7 RID: 9687
	[PublicizedFrom(EAccessModifier.Private)]
	public float rayCastDelay;

	// Token: 0x040025D8 RID: 9688
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bRemoveTexture;

	// Token: 0x040025D9 RID: 9689
	public int DefaultTextureID = 1;

	// Token: 0x040025DA RID: 9690
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Vector3i, bool> visitedPositions = new Dictionary<Vector3i, bool>();

	// Token: 0x040025DB RID: 9691
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Vector2i, bool> visitedRays = new Dictionary<Vector2i, bool>();

	// Token: 0x040025DC RID: 9692
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Stack<Vector2i> positionsToCheck = new Stack<Vector2i>();

	// Token: 0x040025DD RID: 9693
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly WorldRayHitInfo worldRayHitInfo = new WorldRayHitInfo();

	// Token: 0x040025DE RID: 9694
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue currentMagazineItem;

	// Token: 0x020005DB RID: 1499
	public enum EnumPaintMode
	{
		// Token: 0x040025E0 RID: 9696
		Single,
		// Token: 0x040025E1 RID: 9697
		Multiple,
		// Token: 0x040025E2 RID: 9698
		Spray,
		// Token: 0x040025E3 RID: 9699
		Fill
	}

	// Token: 0x020005DC RID: 1500
	public struct ChannelMask
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x0013CA7D File Offset: 0x0013AC7D
		public ChannelMask(int channel)
		{
			this.channelMask = 1 << channel;
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x0013CA8B File Offset: 0x0013AC8B
		public bool IncludesChannel(int _channel)
		{
			return (this.channelMask & (1 << _channel)) != 0;
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x0013CA9D File Offset: 0x0013AC9D
		public void ToggleChannel(int _channel)
		{
			if (this.channelMask != 1 << _channel)
			{
				this.channelMask ^= 1 << _channel;
			}
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x0013CA7D File Offset: 0x0013AC7D
		public void SetExclusiveChannel(int _channel)
		{
			this.channelMask = 1 << _channel;
		}

		// Token: 0x040025E4 RID: 9700
		[PublicizedFrom(EAccessModifier.Private)]
		public int channelMask;
	}

	// Token: 0x020005DD RID: 1501
	public class ItemActionTextureBlockData : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x060030D5 RID: 12501 RVA: 0x0013CAC0 File Offset: 0x0013ACC0
		public ItemActionTextureBlockData(ItemInventoryData _invData, int _indexInEntityOfAction, string _particleTransform)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025E5 RID: 9701
		public int idx = 1;

		// Token: 0x040025E6 RID: 9702
		public bool bAutoChannel;

		// Token: 0x040025E7 RID: 9703
		public ItemActionTextureBlock.ChannelMask channelMask = new ItemActionTextureBlock.ChannelMask(0);

		// Token: 0x040025E8 RID: 9704
		public ItemActionTextureBlock.EnumPaintMode paintMode;

		// Token: 0x040025E9 RID: 9705
		public bool bReplacePaintNextTime;

		// Token: 0x040025EA RID: 9706
		public bool bPaintAllSides;

		// Token: 0x040025EB RID: 9707
		public float lastTimeReplacePaintShown;
	}

	// Token: 0x020005DE RID: 1502
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EPaintResult
	{
		// Token: 0x040025ED RID: 9709
		CanNotPaint,
		// Token: 0x040025EE RID: 9710
		Painted,
		// Token: 0x040025EF RID: 9711
		SamePaint,
		// Token: 0x040025F0 RID: 9712
		NoPaintAvailable
	}
}
