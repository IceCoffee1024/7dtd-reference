using System;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005A6 RID: 1446
[Preserve]
public class ItemActionConnectPower : ItemAction
{
	// Token: 0x06002F77 RID: 12151 RVA: 0x0012C747 File Offset: 0x0012A947
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionConnectPower.ConnectPowerData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x0012C750 File Offset: 0x0012A950
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("WireOffset"))
		{
			this.wireOffset = StringParsers.ParseVector3(_props.Values["WireOffset"], 0, -1);
		}
		if (_props.Values.ContainsKey("MaxWireLength"))
		{
			this.maxWireLength = StringParsers.ParseSInt32(_props.Values["MaxWireLength"], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x0012C7C4 File Offset: 0x0012A9C4
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		ItemActionConnectPower.ConnectPowerData connectPowerData = (ItemActionConnectPower.ConnectPowerData)_data;
		connectPowerData.HasStartPoint = false;
		if (connectPowerData.wireNode != null)
		{
			WireManager.Instance.RemoveActiveWire(connectPowerData.wireNode);
			global::UnityEngine.Object.Destroy(connectPowerData.wireNode.gameObject);
			connectPowerData.wireNode = null;
		}
		if (connectPowerData.invData.world.GetTileEntity(connectPowerData.startPoint) is TileEntityPowered)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, connectPowerData.invData.holdingEntity.entityId), false, -1, -1, -1, null, 192, false);
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, connectPowerData.invData.holdingEntity.entityId), false);
			}
		}
		if (!(_data.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		((ItemActionConnectPower.ConnectPowerData)_data).playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.PowerInfo, null, true);
		WireManager.Instance.ToggleAllWirePulse(false);
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x0012C8E8 File Offset: 0x0012AAE8
	public override void StartHolding(ItemActionData _data)
	{
		base.StartHolding(_data);
		if (!(_data.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		((ItemActionConnectPower.ConnectPowerData)_data).playerUI = LocalPlayerUI.GetUIForPlayer(_data.invData.holdingEntity as EntityPlayerLocal);
		WireManager.Instance.ToggleAllWirePulse(true);
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x0012C93A File Offset: 0x0012AB3A
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		_actionData.lastUseTime = Time.time;
		((ItemActionConnectPower.ConnectPowerData)_actionData).StartLink = true;
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x0012C96C File Offset: 0x0012AB6C
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionConnectPower.ConnectPowerData connectPowerData = (ItemActionConnectPower.ConnectPowerData)_actionData;
		return connectPowerData.StartLink && Time.time - connectPowerData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[connectPowerData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x0012C9C4 File Offset: 0x0012ABC4
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionConnectPower.ConnectPowerData connectPowerData = (ItemActionConnectPower.ConnectPowerData)_actionData;
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)_actionData.invData.holdingEntity).HitInfo;
		Vector3i blockPos = hitInfo.hit.blockPos;
		bool flag = true;
		if (connectPowerData.invData.holdingEntity is EntityPlayerLocal && connectPowerData.playerUI == null)
		{
			connectPowerData.playerUI = LocalPlayerUI.GetUIForPlayer(connectPowerData.invData.holdingEntity as EntityPlayerLocal);
		}
		if (connectPowerData.playerUI != null && !connectPowerData.invData.world.CanPlaceBlockAt(blockPos, connectPowerData.invData.world.gameManager.GetPersistentLocalPlayer(), false))
		{
			connectPowerData.isFriendly = false;
			connectPowerData.playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.PowerInfo, null, true);
			return;
		}
		connectPowerData.isFriendly = true;
		if (hitInfo.bHitValid)
		{
			int num = (int)(Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance);
			if (hitInfo.hit.distanceSq <= (float)num)
			{
				BlockValue block = _actionData.invData.world.GetBlock(blockPos);
				BlockPowered blockPowered = block.Block as BlockPowered;
				if (blockPowered != null)
				{
					if (connectPowerData.playerUI != null)
					{
						Color color = Color.grey;
						int num2 = blockPowered.RequiredPower;
						if (blockPowered.isMultiBlock && block.ischild)
						{
							connectPowerData.playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.PowerInfo, null, true);
							return;
						}
						Vector3i vector3i = blockPos;
						ChunkCluster chunkCache = _actionData.invData.world.ChunkCache;
						if (chunkCache != null)
						{
							Chunk chunk = (Chunk)chunkCache.GetChunkSync(World.toChunkXZ(vector3i.x), vector3i.y, World.toChunkXZ(vector3i.z));
							if (chunk != null)
							{
								TileEntityPowered tileEntityPowered = chunk.GetTileEntity(World.toBlock(vector3i)) as TileEntityPowered;
								if (tileEntityPowered != null)
								{
									color = (tileEntityPowered.IsPowered ? Color.yellow : Color.grey);
									num2 = tileEntityPowered.PowerUsed;
								}
								else
								{
									color = Color.grey;
								}
							}
						}
						connectPowerData.playerUI.nguiWindowManager.SetLabel(EnumNGUIWindow.PowerInfo, string.Format("{0}W", num2), new Color?(color), true);
					}
					flag = false;
				}
			}
		}
		if (flag && connectPowerData.playerUI != null)
		{
			connectPowerData.playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.PowerInfo, null, true);
		}
		if (connectPowerData.HasStartPoint)
		{
			if (connectPowerData.wireNode == null)
			{
				return;
			}
			float num3 = Vector3.Distance(connectPowerData.startPoint.ToVector3(), _actionData.invData.holdingEntity.position);
			if (num3 < (float)(this.maxWireLength - 5))
			{
				connectPowerData.inRange = true;
				connectPowerData.wireNode.wireColor = new Color(0f, 0f, 0f, 0f);
			}
			if (num3 > (float)(this.maxWireLength - 5))
			{
				connectPowerData.inRange = false;
				connectPowerData.wireNode.wireColor = Color.red;
			}
			if (num3 > (float)this.maxWireLength)
			{
				connectPowerData.HasStartPoint = false;
				if (connectPowerData.wireNode != null)
				{
					WireManager.Instance.RemoveActiveWire(connectPowerData.wireNode);
					global::UnityEngine.Object.Destroy(connectPowerData.wireNode.gameObject);
					connectPowerData.wireNode = null;
				}
				if (!(connectPowerData.invData.world.GetChunkFromWorldPos(connectPowerData.startPoint) is Chunk))
				{
					return;
				}
				if (connectPowerData.invData.world.GetTileEntity(connectPowerData.startPoint) is TileEntityPowered)
				{
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false, -1, -1, -1, null, 192, false);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false);
					}
				}
				_actionData.invData.holdingEntity.RightArmAnimationUse = true;
				connectPowerData.invData.holdingEntity.PlayOneShot("ui_denied", false, false, false, null, 1f);
			}
		}
		if (!connectPowerData.StartLink || Time.time - connectPowerData.lastUseTime < AnimationDelayData.AnimationDelay[connectPowerData.invData.item.HoldType.Value].RayCast)
		{
			return;
		}
		connectPowerData.StartLink = false;
		ItemActionConnectPower.ConnectPowerData connectPowerData2 = (ItemActionConnectPower.ConnectPowerData)_actionData;
		ItemInventoryData invData = _actionData.invData;
		Vector3i lastBlockPos = hitInfo.lastBlockPos;
		if (!hitInfo.bHitValid || hitInfo.tag.StartsWith("E_"))
		{
			connectPowerData2.HasStartPoint = false;
			return;
		}
		if (connectPowerData.invData.itemValue.MaxUseTimes > 0 && connectPowerData.invData.itemValue.UseTimes >= (float)connectPowerData.invData.itemValue.MaxUseTimes)
		{
			EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
			base.HandleJamSound(connectPowerData.invData.itemValue, this.item, entityPlayerLocal);
			return;
		}
		if (connectPowerData.invData.itemValue.MaxUseTimes > 0)
		{
			_actionData.invData.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, _actionData.invData.itemValue, 1f, invData.holdingEntity, null, (_actionData.invData.itemValue.ItemClass != null) ? _actionData.invData.itemValue.ItemClass.ItemTags : FastTags<TagGroup.Global>.none, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
			base.HandleItemBreak(_actionData);
		}
		if (connectPowerData2.HasStartPoint)
		{
			if (connectPowerData2.startPoint == hitInfo.hit.blockPos || !connectPowerData2.inRange)
			{
				return;
			}
			if (Vector3.Distance(connectPowerData.startPoint.ToVector3(), hitInfo.hit.blockPos.ToVector3()) > (float)this.maxWireLength)
			{
				return;
			}
			TileEntityPowered poweredBlock = this.GetPoweredBlock(invData);
			if (poweredBlock != null)
			{
				TileEntityPowered poweredBlock2 = this.GetPoweredBlock(connectPowerData2.startPoint);
				if (poweredBlock2 != null)
				{
					if (!poweredBlock.CanHaveParent(poweredBlock2))
					{
						GameManager.ShowTooltip(_actionData.invData.holdingEntity as EntityPlayerLocal, Localization.Get("ttCantHaveParent", false, null), false, false, 0f);
						invData.holdingEntity.PlayOneShot("ui_denied", false, false, false, null, 1f);
						return;
					}
					if (poweredBlock2.ChildCount > 8)
					{
						GameManager.ShowTooltip(_actionData.invData.holdingEntity as EntityPlayerLocal, Localization.Get("ttWireLimit", false, null), false, false, 0f);
						invData.holdingEntity.PlayOneShot("ui_denied", false, false, false, null, 1f);
						return;
					}
					poweredBlock.SetParentWithWireTool(poweredBlock2, invData.holdingEntity.entityId);
					_actionData.invData.holdingEntity.RightArmAnimationUse = true;
					connectPowerData2.HasStartPoint = false;
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false, -1, -1, -1, null, 192, false);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false);
					}
					EntityAlive holdingEntity = _actionData.invData.holdingEntity;
					string text = "wire_tool_" + (poweredBlock2.IsPowered ? "sparks" : "dust");
					Transform handTransform = this.GetHandTransform(holdingEntity);
					GameManager.Instance.SpawnParticleEffectServer(new ParticleEffect(text, handTransform.position + Origin.position, handTransform.rotation, holdingEntity.GetLightBrightness(), Color.white), invData.holdingEntity.entityId, false, false);
					if (connectPowerData.wireNode != null)
					{
						WireManager.Instance.RemoveActiveWire(connectPowerData.wireNode);
						global::UnityEngine.Object.Destroy(connectPowerData.wireNode.gameObject);
						connectPowerData.wireNode = null;
					}
					this.DecreaseDurability(connectPowerData);
					return;
				}
			}
		}
		else
		{
			TileEntityPowered poweredBlock3 = this.GetPoweredBlock(invData);
			if (poweredBlock3 != null)
			{
				_actionData.invData.holdingEntity.RightArmAnimationUse = true;
				connectPowerData2.startPoint = hitInfo.hit.blockPos;
				connectPowerData2.HasStartPoint = true;
				EntityAlive holdingEntity2 = _actionData.invData.holdingEntity;
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.AddWire, connectPowerData2.startPoint, holdingEntity2.entityId), false, -1, -1, -1, null, 192, false);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.AddWire, connectPowerData2.startPoint, holdingEntity2.entityId), false);
				}
				Manager.BroadcastPlay(poweredBlock3.ToWorldPos().ToVector3(), poweredBlock3.IsPowered ? "wire_live_connect" : "wire_dead_connect", 0f);
				Transform handTransform2 = this.GetHandTransform(holdingEntity2);
				if (handTransform2 != null)
				{
					Transform transform = handTransform2.FindInChilds("wire_mesh", false);
					if (transform == null)
					{
						return;
					}
					if (connectPowerData2.wireNode != null)
					{
						WireManager.Instance.RemoveActiveWire(connectPowerData2.wireNode);
						global::UnityEngine.Object.Destroy(connectPowerData2.wireNode.gameObject);
						connectPowerData2.wireNode = null;
					}
					WireNode component = ((GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode"))).GetComponent<WireNode>();
					component.LocalPosition = hitInfo.hit.blockPos.ToVector3() - Origin.position;
					component.localOffset = poweredBlock3.GetWireOffset();
					WireNode wireNode = component;
					wireNode.localOffset.x = wireNode.localOffset.x + 0.5f;
					WireNode wireNode2 = component;
					wireNode2.localOffset.y = wireNode2.localOffset.y + 0.5f;
					WireNode wireNode3 = component;
					wireNode3.localOffset.z = wireNode3.localOffset.z + 0.5f;
					component.Source = transform.gameObject;
					component.sourceOffset = this.wireOffset;
					component.TogglePulse(false);
					component.SetPulseSpeed(360f);
					connectPowerData2.wireNode = component;
					WireManager.Instance.AddActiveWire(component);
					string text2 = "wire_tool_" + (poweredBlock3.IsPowered ? "sparks" : "dust");
					GameManager.Instance.SpawnParticleEffectServer(new ParticleEffect(text2, handTransform2.position + Origin.position, handTransform2.rotation, holdingEntity2.GetLightBrightness(), Color.white), invData.holdingEntity.entityId, false, false);
				}
			}
		}
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x0012D428 File Offset: 0x0012B628
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform GetHandTransform(EntityAlive holdingEntity)
	{
		Transform transform = holdingEntity.RootTransform.Find("Graphics").FindInChilds(holdingEntity.GetRightHandTransformName(), true);
		Transform transform2;
		if (transform != null && transform.childCount > 0)
		{
			transform2 = transform;
		}
		else
		{
			Transform transform3 = holdingEntity.RootTransform.Find("Camera").FindInChilds(holdingEntity.GetRightHandTransformName(), true);
			if (transform3 != null && transform3.childCount > 0)
			{
				transform2 = transform3;
			}
			else
			{
				transform2 = holdingEntity.emodel.GetRightHandTransform();
			}
		}
		return transform2;
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x0012D4AC File Offset: 0x0012B6AC
	[PublicizedFrom(EAccessModifier.Internal)]
	public void CheckForWireRemoveNeeded(EntityAlive _player, Vector3i _blockPos)
	{
		ItemActionConnectPower.ConnectPowerData connectPowerData = (ItemActionConnectPower.ConnectPowerData)_player.inventory.holdingItemData.actionData[1];
		if (connectPowerData.HasStartPoint && connectPowerData.startPoint == _blockPos)
		{
			this.DisconnectWire(connectPowerData);
		}
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x0012D4F4 File Offset: 0x0012B6F4
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)_actionData.invData.holdingEntity).HitInfo;
		if (hitInfo.bHitValid && (_actionData as ItemActionConnectPower.ConnectPowerData).isFriendly)
		{
			int num = (int)(Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance);
			if (hitInfo.hit.distanceSq <= (float)num)
			{
				Vector3i blockPos = hitInfo.hit.blockPos;
				Block block = _actionData.invData.world.GetBlock(blockPos).Block;
				if (block is BlockPowered)
				{
					return ItemClass.EnumCrosshairType.PowerItem;
				}
				if (block is BlockPowerSource)
				{
					return ItemClass.EnumCrosshairType.PowerSource;
				}
			}
		}
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x0012D588 File Offset: 0x0012B788
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered GetPoweredBlock(ItemInventoryData data)
	{
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)data.holdingEntity).HitInfo;
		Block block = data.world.GetBlock(hitInfo.hit.blockPos).Block;
		if (!(block is BlockPowered) && !(block is BlockPowerSource))
		{
			return null;
		}
		Vector3i blockPos = hitInfo.hit.blockPos;
		ChunkCluster chunkCache = data.world.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkSync(World.toChunkXZ(blockPos.x), blockPos.y, World.toChunkXZ(blockPos.z));
		if (chunk == null)
		{
			return null;
		}
		TileEntity tileEntity = chunk.GetTileEntity(World.toBlock(blockPos));
		if (tileEntity == null)
		{
			if (block is BlockPowered)
			{
				tileEntity = (block as BlockPowered).CreateTileEntity(chunk);
			}
			else if (block is BlockPowerSource)
			{
				tileEntity = (block as BlockPowerSource).CreateTileEntity(chunk);
			}
			tileEntity.localChunkPos = World.toBlock(blockPos);
			BlockEntityData blockEntity = chunk.GetBlockEntity(blockPos);
			if (blockEntity != null)
			{
				((TileEntityPowered)tileEntity).BlockTransform = blockEntity.transform;
			}
			((TileEntityPowered)tileEntity).InitializePowerData();
			chunk.AddTileEntity(tileEntity);
		}
		return tileEntity as TileEntityPowered;
	}

	// Token: 0x06002F82 RID: 12162 RVA: 0x0012D6B8 File Offset: 0x0012B8B8
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered GetPoweredBlock(Vector3i tileEntityPos)
	{
		World world = GameManager.Instance.World;
		Block block = world.GetBlock(tileEntityPos).Block;
		if (!(block is BlockPowered) && !(block is BlockPowerSource))
		{
			return null;
		}
		Chunk chunk = world.GetChunkFromWorldPos(tileEntityPos.x, tileEntityPos.y, tileEntityPos.z) as Chunk;
		if (chunk == null)
		{
			return null;
		}
		TileEntity tileEntity = chunk.GetTileEntity(World.toBlock(tileEntityPos));
		if (tileEntity == null)
		{
			if (block is BlockPowered)
			{
				tileEntity = (block as BlockPowered).CreateTileEntity(chunk);
			}
			else if (block is BlockPowerSource)
			{
				tileEntity = (block as BlockPowerSource).CreateTileEntity(chunk);
			}
			tileEntity.localChunkPos = World.toBlock(tileEntityPos);
			BlockEntityData blockEntity = chunk.GetBlockEntity(tileEntityPos);
			if (blockEntity != null)
			{
				((TileEntityPowered)tileEntity).BlockTransform = blockEntity.transform;
			}
			((TileEntityPowered)tileEntity).InitializePowerData();
			chunk.AddTileEntity(tileEntity);
		}
		return tileEntity as TileEntityPowered;
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x0012D7A0 File Offset: 0x0012B9A0
	public bool DisconnectWire(ItemActionConnectPower.ConnectPowerData _actionData)
	{
		if (!_actionData.HasStartPoint)
		{
			return false;
		}
		_actionData.HasStartPoint = false;
		if (_actionData.wireNode != null)
		{
			WireManager.Instance.RemoveActiveWire(_actionData.wireNode);
			global::UnityEngine.Object.Destroy(_actionData.wireNode.gameObject);
			_actionData.wireNode = null;
		}
		if (!(_actionData.invData.world.GetChunkFromWorldPos(_actionData.startPoint) is Chunk))
		{
			return false;
		}
		TileEntityPowered tileEntityPowered = _actionData.invData.world.GetTileEntity(_actionData.startPoint) as TileEntityPowered;
		if (tileEntityPowered != null)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false, -1, -1, -1, null, 192, false);
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(NetPackageWireToolActions.WireActions.RemoveWire, Vector3i.zero, _actionData.invData.holdingEntity.entityId), false);
			}
			Manager.BroadcastPlay(tileEntityPowered.ToWorldPos().ToVector3(), tileEntityPowered.IsPowered ? "wire_live_break" : "wire_dead_break", 0f);
			EntityAlive holdingEntity = _actionData.invData.holdingEntity;
			string text = "wire_tool_" + (tileEntityPowered.IsPowered ? "sparks" : "dust");
			Transform handTransform = this.GetHandTransform(holdingEntity);
			GameManager.Instance.SpawnParticleEffectServer(new ParticleEffect(text, handTransform.position + Origin.position, handTransform.rotation, holdingEntity.GetLightBrightness(), Color.white), holdingEntity.entityId, false, false);
		}
		_actionData.invData.holdingEntity.RightArmAnimationAttack = true;
		_actionData.invData.holdingEntity.PlayOneShot("ui_denied", false, false, false, null, 1f);
		return true;
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x0012D978 File Offset: 0x0012BB78
	[PublicizedFrom(EAccessModifier.Private)]
	public void DecreaseDurability(ItemActionConnectPower.ConnectPowerData _actionData)
	{
		if (_actionData.invData.itemValue.MaxUseTimes > 0)
		{
			if (_actionData.invData.itemValue.UseTimes + 1f < (float)_actionData.invData.itemValue.MaxUseTimes)
			{
				_actionData.invData.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, _actionData.invData.itemValue, 1f, _actionData.invData.holdingEntity, null, _actionData.invData.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				base.HandleItemBreak(_actionData);
				return;
			}
			_actionData.invData.holdingEntity.inventory.DecHoldingItem(1);
		}
	}

	// Token: 0x040024A0 RID: 9376
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 wireOffset = Vector3.zero;

	// Token: 0x040024A1 RID: 9377
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxWireLength = 15;

	// Token: 0x020005A7 RID: 1447
	public class ConnectPowerData : ItemActionAttackData
	{
		// Token: 0x06002F86 RID: 12166 RVA: 0x0012BE18 File Offset: 0x0012A018
		public ConnectPowerData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024A2 RID: 9378
		public bool StartLink;

		// Token: 0x040024A3 RID: 9379
		public bool HasStartPoint;

		// Token: 0x040024A4 RID: 9380
		public LocalPlayerUI playerUI;

		// Token: 0x040024A5 RID: 9381
		public Vector3i startPoint;

		// Token: 0x040024A6 RID: 9382
		public bool inRange;

		// Token: 0x040024A7 RID: 9383
		public bool isFriendly;

		// Token: 0x040024A8 RID: 9384
		public WireNode wireNode;
	}
}
