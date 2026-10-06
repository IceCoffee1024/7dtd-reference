using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005D8 RID: 1496
[Preserve]
public class ItemActionTerrainTool : ItemActionRanged
{
	// Token: 0x060030E0 RID: 12512 RVA: 0x0013AD48 File Offset: 0x00138F48
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateModel()
	{
		if (ItemActionTerrainTool.modelObj)
		{
			return;
		}
		ItemActionTerrainTool.modelObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Transform transform = ItemActionTerrainTool.modelObj.transform;
		global::UnityEngine.Object.Destroy(transform.GetComponent<Collider>());
		transform.SetParent(null);
		ItemActionTerrainTool.modelObj.layer = 0;
		ItemActionTerrainTool.modelObj.SetActive(false);
		ItemActionTerrainTool.modelMat = Resources.Load<Material>("Materials/TerrainSmoothing");
		ItemActionTerrainTool.modelObj.GetComponent<Renderer>().material = ItemActionTerrainTool.modelMat;
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x0013ADC1 File Offset: 0x00138FC1
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Mode"))
		{
			this.mode = EnumUtils.Parse<ItemActionTerrainTool.EnumMode>(_props.Values["Mode"], false);
		}
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x0013ADF8 File Offset: 0x00138FF8
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionTerrainTool.MyInventoryData(_invData, _indexInEntityOfAction, null);
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetInitialMeta(ItemValue _itemValue)
	{
		return 0;
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0013AE02 File Offset: 0x00139002
	public override void StartHolding(ItemActionData _actionData)
	{
		this.showSphere(_actionData);
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0013AE0B File Offset: 0x0013900B
	public override void StopHolding(ItemActionData _actionData)
	{
		base.StopHolding(_actionData);
		((ItemActionTerrainTool.MyInventoryData)_actionData).bActivated = false;
		this.hideSphere(_actionData);
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x0013AE28 File Offset: 0x00139028
	[PublicizedFrom(EAccessModifier.Private)]
	public void showSphere(ItemActionData _actionData)
	{
		this.CreateModel();
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		Ray lookRay = myInventoryData.invData.holdingEntity.GetLookRay();
		Transform transform = ItemActionTerrainTool.modelObj.transform;
		transform.SetPositionAndRotation(lookRay.origin + lookRay.direction * myInventoryData.sphereDistance - Origin.position, Quaternion.identity);
		transform.SetParent(myInventoryData.invData.holdingEntity.transform);
		transform.localScale = new Vector3(ItemActionTerrainTool.sphereRadius * 2f, ItemActionTerrainTool.sphereRadius * 2f, ItemActionTerrainTool.sphereRadius * 2f);
		ItemActionTerrainTool.modelObj.SetActive(true);
		ItemActionTerrainTool.modelObj.layer = 0;
		ItemActionTerrainTool.modelMat.color = new Color(0f, 0f, 0f, 0f);
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x0013AF0C File Offset: 0x0013910C
	[PublicizedFrom(EAccessModifier.Private)]
	public void hideSphere(ItemActionData _actionData)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		ItemActionTerrainTool.modelObj.SetActive(false);
		ItemActionTerrainTool.modelObj.transform.SetParent(myInventoryData.invData.model);
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x0013AF48 File Offset: 0x00139148
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		if (_bReleased)
		{
			myInventoryData.bActivated = false;
			GameManager.Instance.ItemActionEffectsServer(myInventoryData.invData.holdingEntity.entityId, myInventoryData.invData.slotIdx, myInventoryData.indexInEntityOfAction, 0, Vector3.zero, Vector3.zero, 0);
			return;
		}
		myInventoryData.bActivated = true;
		myInventoryData.activateTime = Time.time;
		GameManager.Instance.ItemActionEffectsServer(myInventoryData.invData.holdingEntity.entityId, myInventoryData.invData.slotIdx, myInventoryData.indexInEntityOfAction, 1, Vector3.zero, Vector3.zero, 0);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x0013AFE8 File Offset: 0x001391E8
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		switch (_firingState)
		{
		case 0:
			ItemActionTerrainTool.modelMat.color = new Color(0f, 0f, 0f, 0f);
			break;
		case 1:
			if (this.mode == ItemActionTerrainTool.EnumMode.Grow)
			{
				ItemActionTerrainTool.modelMat.color = new Color(0f, 1f, 0f, 0f);
				return;
			}
			ItemActionTerrainTool.modelMat.color = new Color(1f, 0f, 0f, 0f);
			return;
		case 2:
			break;
		default:
			return;
		}
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x0013B07D File Offset: 0x0013927D
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionTerrainTool.MyInventoryData)_actionData).bActivated;
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x0013B08C File Offset: 0x0013928C
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		if (!ItemActionTerrainTool.modelObj)
		{
			return;
		}
		Ray lookRay = entityPlayerLocal.GetLookRay();
		lookRay.origin += lookRay.direction.normalized * 0.1f;
		int num = 256;
		int num2 = 65536;
		if (!Voxel.RaycastOnVoxels(entityPlayerLocal.world, lookRay, 100f, num2, num, 0f))
		{
			ItemActionTerrainTool.modelObj.transform.position = lookRay.origin + lookRay.direction.normalized * 100f - Origin.position;
			if (InputUtils.AltKeyPressed)
			{
				ItemActionTerrainTool.blockValueSelected = BlockValue.Air;
			}
			return;
		}
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		myInventoryData.hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
		Vector3 pos = myInventoryData.hitInfo.hit.pos;
		ItemActionTerrainTool.modelObj.transform.position = Vector3.Lerp(ItemActionTerrainTool.modelObj.transform.position, pos - Origin.position, 0.5f);
		if (!myInventoryData.bActivated)
		{
			return;
		}
		if (Time.time - myInventoryData.lastHitTime > 0.2f)
		{
			myInventoryData.lastHitTime = Time.time;
			bool shiftKeyPressed = InputUtils.ShiftKeyPressed;
			if (this.mode == ItemActionTerrainTool.EnumMode.Grow)
			{
				int num3 = ((!shiftKeyPressed) ? 20 : 5);
				this.GrowTerrain(_actionData, pos, num3);
				return;
			}
			if (this.mode == ItemActionTerrainTool.EnumMode.Shrink)
			{
				if (!shiftKeyPressed)
				{
					this.GrowTerrain(_actionData, pos, -8);
					return;
				}
				this.RemoveTerrain(_actionData, pos, this.damage, null, true);
			}
		}
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x0013B247 File Offset: 0x00139447
	public override float GetRange(ItemActionData _actionData)
	{
		return 20f;
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x0013B250 File Offset: 0x00139450
	[PublicizedFrom(EAccessModifier.Private)]
	public int GrowTerrain(ItemActionData _actionData, Vector3 _worldPos, int _densityStep)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		World world = myInventoryData.invData.world;
		int num = Utils.Fastfloor(_worldPos.x - ItemActionTerrainTool.sphereRadius - 1f);
		int num2 = Utils.Fastfloor(_worldPos.x + ItemActionTerrainTool.sphereRadius + 1f);
		int num3 = Utils.Fastfloor(_worldPos.z - ItemActionTerrainTool.sphereRadius - 1f);
		int num4 = Utils.Fastfloor(_worldPos.z + ItemActionTerrainTool.sphereRadius + 1f);
		int num5 = Utils.FastClamp(Utils.Fastfloor(_worldPos.y - ItemActionTerrainTool.sphereRadius), 0, 255);
		int num6 = Utils.FastClamp(Utils.Fastfloor(_worldPos.y + ItemActionTerrainTool.sphereRadius), 0, 255);
		if (InputUtils.AltKeyPressed)
		{
			BlockValue blockValue = myInventoryData.hitInfo.hit.blockValue;
			if (!blockValue.Block.isMultiBlock)
			{
				ItemActionTerrainTool.blockValueSelected = blockValue;
			}
			return 0;
		}
		BlockValue blockValue2 = ItemActionTerrainTool.blockValueSelected;
		bool flag = !blockValue2.isair;
		if (!flag && _densityStep >= 0)
		{
			blockValue2 = myInventoryData.hitInfo.hit.blockValue;
			if (!blockValue2.Block.shape.IsTerrain())
			{
				return 0;
			}
		}
		bool flag2 = blockValue2.Block.shape.IsTerrain();
		int type = blockValue2.type;
		this.blockChanges.Clear();
		IChunk chunk = null;
		for (int i = num3; i <= num4; i++)
		{
			Vector3 vector;
			vector.z = (float)i;
			int num7 = World.toBlockXZ(i);
			for (int j = num; j <= num2; j++)
			{
				vector.x = (float)j;
				int num8 = World.toBlockXZ(j);
				world.GetChunkFromWorldPos(j, i, ref chunk);
				if (chunk != null)
				{
					int k = num5;
					while (k <= num6)
					{
						vector.y = (float)k;
						int num9 = _densityStep;
						float magnitude = (_worldPos - vector).magnitude;
						if (magnitude <= ItemActionTerrainTool.sphereRadius)
						{
							goto IL_0202;
						}
						if (magnitude - ItemActionTerrainTool.sphereRadius < 1f)
						{
							float num10 = 1f - (magnitude - ItemActionTerrainTool.sphereRadius) / 1f;
							num9 = (int)((float)num9 * num10);
							if (num9 != 0)
							{
								goto IL_0202;
							}
						}
						IL_04EE:
						k++;
						continue;
						IL_0202:
						BlockValue blockNoDamage = chunk.GetBlockNoDamage(num8, k, num7);
						bool isair = blockNoDamage.isair;
						if (!blockNoDamage.Block.shape.IsTerrain() && !isair)
						{
							goto IL_04EE;
						}
						int num11 = (int)world.GetDensity(j, k, i);
						BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
						blockChangeInfo.blockValueRef = new BlockValueRef(j, k, i);
						if (num9 > 0)
						{
							if (isair)
							{
								int num12 = Vector3i.AllDirections.Length;
								int l = 0;
								while (l < num12)
								{
									Vector3i vector3i;
									vector3i.x = j + Vector3i.AllDirections[l].x;
									vector3i.y = k + Vector3i.AllDirections[l].y;
									vector3i.z = i + Vector3i.AllDirections[l].z;
									if (world.GetBlock(vector3i).Block.shape.IsTerrain())
									{
										if (flag2)
										{
											num11 -= num9;
											if (num11 < 0)
											{
												num11 = -1;
												blockChangeInfo.blockValue.type = type;
												blockChangeInfo.bChangeBlockValue = true;
											}
											blockChangeInfo.density = (sbyte)num11;
											blockChangeInfo.bChangeDensity = true;
											this.blockChanges.Add(blockChangeInfo);
											break;
										}
										blockChangeInfo.blockValue.type = type;
										blockChangeInfo.bChangeBlockValue = true;
										this.blockChanges.Add(blockChangeInfo);
										break;
									}
									else
									{
										l++;
									}
								}
								goto IL_04EE;
							}
							if (flag2 && num11 > (int)MarchingCubes.DensityTerrain)
							{
								num11 -= num9;
								if (num11 < (int)MarchingCubes.DensityTerrain)
								{
									num11 = (int)MarchingCubes.DensityTerrain;
								}
								blockChangeInfo.density = (sbyte)num11;
								blockChangeInfo.bChangeDensity = true;
								if (flag)
								{
									blockChangeInfo.blockValue.type = type;
									blockChangeInfo.bChangeBlockValue = true;
								}
								this.blockChanges.Add(blockChangeInfo);
								goto IL_04EE;
							}
							goto IL_04EE;
						}
						else
						{
							if (!isair)
							{
								int num13 = Vector3i.AllDirections.Length;
								for (int m = 0; m < num13; m++)
								{
									Vector3i vector3i2;
									vector3i2.x = j + Vector3i.AllDirections[m].x;
									vector3i2.y = k + Vector3i.AllDirections[m].y;
									vector3i2.z = i + Vector3i.AllDirections[m].z;
									if (world.GetBlock(vector3i2).isair)
									{
										num11 -= num9;
										if (num11 > 0)
										{
											num11 = 1;
											blockChangeInfo.blockValue.type = 0;
											blockChangeInfo.bChangeBlockValue = true;
										}
										blockChangeInfo.density = (sbyte)num11;
										blockChangeInfo.bChangeDensity = true;
										this.blockChanges.Add(blockChangeInfo);
										break;
									}
								}
								goto IL_04EE;
							}
							if (num11 < (int)MarchingCubes.DensityAir)
							{
								num11 -= num9;
								if (num11 > (int)MarchingCubes.DensityAir)
								{
									num11 = (int)MarchingCubes.DensityAir;
								}
								blockChangeInfo.density = (sbyte)num11;
								blockChangeInfo.bChangeDensity = true;
								this.blockChanges.Add(blockChangeInfo);
								goto IL_04EE;
							}
							goto IL_04EE;
						}
					}
				}
			}
		}
		if (this.blockChanges.Count > 0)
		{
			BlockToolSelection.Instance.BeginUndo();
			myInventoryData.invData.world.SetBlocksRPC(this.blockChanges);
			BlockToolSelection.Instance.EndUndo(false);
		}
		return 0;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x0013B7B4 File Offset: 0x001399B4
	[PublicizedFrom(EAccessModifier.Private)]
	public int setTerrainOLD(ItemActionData _actionData, Vector3 _worldPos, float _damage = 0f, DamageMultiplier _damageMultiplier = null, bool _bChangeBlocks = true)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		int num = Utils.Fastfloor(_worldPos.x - ItemActionTerrainTool.sphereRadius);
		int num2 = Utils.Fastfloor(_worldPos.x + ItemActionTerrainTool.sphereRadius);
		int num3 = Utils.Fastfloor(_worldPos.y - ItemActionTerrainTool.sphereRadius);
		int num4 = Utils.Fastfloor(_worldPos.y + ItemActionTerrainTool.sphereRadius);
		int num5 = Utils.Fastfloor(_worldPos.z - ItemActionTerrainTool.sphereRadius);
		int num6 = Utils.Fastfloor(_worldPos.z + ItemActionTerrainTool.sphereRadius);
		this.blockChanges.Clear();
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				for (int k = num5; k <= num6; k++)
				{
					int num7 = 0;
					for (int l = 0; l < ItemActionTerrainTool.INNER_POINTS.Length; l++)
					{
						Vector3 vector = ItemActionTerrainTool.INNER_POINTS[l];
						if ((new Vector3((float)i + vector.x, (float)j + vector.y, (float)k + vector.z) - _worldPos).magnitude <= ItemActionTerrainTool.sphereRadius)
						{
							num7++;
						}
						if (l == 8)
						{
							if (num7 == 9)
							{
								num7 = ItemActionTerrainTool.INNER_POINTS.Length;
								break;
							}
							if (num7 == 0)
							{
								break;
							}
						}
					}
					if (num7 != 0)
					{
						Vector3i vector3i = new Vector3i(i, j, k);
						BlockValue block = myInventoryData.invData.world.GetBlock(vector3i);
						BlockValue blockValue = block;
						sbyte density = myInventoryData.invData.world.GetDensity(vector3i);
						sbyte b = density;
						if (num7 > ItemActionTerrainTool.INNER_POINTS.Length / 2 || block.Block.shape.IsTerrain())
						{
							blockValue.type = 1;
							b = (sbyte)((float)MarchingCubes.DensityTerrain * (float)(num7 - ItemActionTerrainTool.INNER_POINTS.Length / 2 - 1) / (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2));
						}
						else if (block.isair)
						{
							b = (sbyte)((float)MarchingCubes.DensityAir * (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2 - num7) / (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2));
							if (b >= 0)
							{
								b = -1;
							}
						}
						if (blockValue.type != block.type || b < density)
						{
							BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
							blockChangeInfo.blockValueRef = vector3i;
							blockChangeInfo.bChangeDensity = true;
							blockChangeInfo.density = b;
							if (blockValue.type != block.type)
							{
								blockChangeInfo.bChangeBlockValue = true;
								blockChangeInfo.blockValue = blockValue;
							}
							this.blockChanges.Add(blockChangeInfo);
						}
					}
				}
			}
		}
		myInventoryData.invData.world.SetBlocksRPC(this.blockChanges);
		return 0;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0013BA4C File Offset: 0x00139C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public int RemoveTerrain(ItemActionData _actionData, Vector3 _worldPos, float _damage = 0f, DamageMultiplier _damageMultiplier = null, bool _bChangeBlocks = true)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		int num = Utils.Fastfloor(_worldPos.x - ItemActionTerrainTool.sphereRadius);
		int num2 = Utils.Fastfloor(_worldPos.x + ItemActionTerrainTool.sphereRadius);
		int num3 = Utils.Fastfloor(_worldPos.y - ItemActionTerrainTool.sphereRadius);
		int num4 = Utils.Fastfloor(_worldPos.y + ItemActionTerrainTool.sphereRadius);
		int num5 = Utils.Fastfloor(_worldPos.z - ItemActionTerrainTool.sphereRadius);
		int num6 = Utils.Fastfloor(_worldPos.z + ItemActionTerrainTool.sphereRadius);
		this.blockChanges.Clear();
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				for (int k = num5; k <= num6; k++)
				{
					int num7 = 0;
					for (int l = 0; l < ItemActionTerrainTool.INNER_POINTS.Length; l++)
					{
						Vector3 vector = ItemActionTerrainTool.INNER_POINTS[l];
						if ((new Vector3((float)i + vector.x, (float)j + vector.y, (float)k + vector.z) - _worldPos).magnitude <= ItemActionTerrainTool.sphereRadius)
						{
							num7++;
						}
						if (l == 8)
						{
							if (num7 == 9)
							{
								num7 = ItemActionTerrainTool.INNER_POINTS.Length;
								break;
							}
							if (num7 == 0)
							{
								break;
							}
						}
					}
					if (num7 != 0)
					{
						Vector3i vector3i = new Vector3i(i, j, k);
						BlockValue block = myInventoryData.invData.world.GetBlock(vector3i);
						if (block.Block.shape.IsTerrain())
						{
							BlockValue blockValue = block;
							sbyte density = myInventoryData.invData.world.GetDensity(vector3i);
							sbyte b = density;
							if (num7 > ItemActionTerrainTool.INNER_POINTS.Length / 2)
							{
								blockValue = BlockValue.Air;
								b = (sbyte)((float)MarchingCubes.DensityAir * (float)(num7 - ItemActionTerrainTool.INNER_POINTS.Length / 2 - 1) / (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2));
								if (b <= 0)
								{
									b = 1;
								}
							}
							else if (!block.isair)
							{
								b = (sbyte)((float)MarchingCubes.DensityTerrain * (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2 - num7) / (float)(ItemActionTerrainTool.INNER_POINTS.Length / 2));
								if (b >= 0)
								{
									b = -1;
								}
							}
							if (blockValue.type != block.type || b > density)
							{
								BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
								blockChangeInfo.blockValueRef = vector3i;
								blockChangeInfo.bChangeDensity = true;
								blockChangeInfo.density = b;
								if (blockValue.type != block.type)
								{
									blockChangeInfo.bChangeBlockValue = true;
									blockChangeInfo.blockValue = blockValue;
								}
								this.blockChanges.Add(blockChangeInfo);
							}
						}
					}
				}
			}
		}
		if (this.blockChanges.Count > 0)
		{
			BlockToolSelection.Instance.BeginUndo();
			myInventoryData.invData.world.SetBlocksRPC(this.blockChanges);
			BlockToolSelection.Instance.EndUndo(false);
		}
		return 0;
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsEditingTool()
	{
		return true;
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x0013BD11 File Offset: 0x00139F11
	public override string GetStat(ItemActionData _data)
	{
		if (!ItemActionTerrainTool.blockValueSelected.isair)
		{
			return ItemActionTerrainTool.blockValueSelected.Block.GetLocalizedBlockName();
		}
		return "-";
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsStatChanged()
	{
		return true;
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x0013BD34 File Offset: 0x00139F34
	public override bool ConsumeScrollWheel(ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		ItemActionTerrainTool.MyInventoryData myInventoryData = (ItemActionTerrainTool.MyInventoryData)_actionData;
		if (_playerInput.Run && ItemActionTerrainTool.modelObj)
		{
			float num = _scrollWheelInput * 1f + _scrollWheelInput * ItemActionTerrainTool.sphereRadius * 0.5f;
			ItemActionTerrainTool.sphereRadius = Utils.FastClamp(ItemActionTerrainTool.sphereRadius + num, 0.5f, 30f);
			ItemActionTerrainTool.modelObj.transform.localScale = new Vector3(ItemActionTerrainTool.sphereRadius * 2f, ItemActionTerrainTool.sphereRadius * 2f, ItemActionTerrainTool.sphereRadius * 2f);
			return true;
		}
		return false;
	}

	// Token: 0x040025BB RID: 9659
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTransparency = 0f;

	// Token: 0x040025BC RID: 9660
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadiusMin = 0.5f;

	// Token: 0x040025BD RID: 9661
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadiusMax = 30f;

	// Token: 0x040025BE RID: 9662
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadiusStep = 1f;

	// Token: 0x040025BF RID: 9663
	[PublicizedFrom(EAccessModifier.Private)]
	public float damage;

	// Token: 0x040025C0 RID: 9664
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionTerrainTool.EnumMode mode;

	// Token: 0x040025C1 RID: 9665
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject modelObj;

	// Token: 0x040025C2 RID: 9666
	[PublicizedFrom(EAccessModifier.Private)]
	public static Material modelMat;

	// Token: 0x040025C3 RID: 9667
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sphereRadius = 2f;

	// Token: 0x040025C4 RID: 9668
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValue blockValueSelected = BlockValue.Air;

	// Token: 0x040025C5 RID: 9669
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3[] INNER_POINTS = new Vector3[]
	{
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(1f, 1f, 0f),
		new Vector3(0f, 1f, 1f),
		new Vector3(1f, 0f, 1f),
		new Vector3(1f, 1f, 1f),
		new Vector3(0.25f, 0.25f, 0.25f),
		new Vector3(0.75f, 0.25f, 0.25f),
		new Vector3(0.25f, 0.75f, 0.25f),
		new Vector3(0.25f, 0.25f, 0.75f),
		new Vector3(0.75f, 0.75f, 0.25f),
		new Vector3(0.25f, 0.75f, 0.75f),
		new Vector3(0.75f, 0.25f, 0.75f),
		new Vector3(0.75f, 0.75f, 0.75f)
	};

	// Token: 0x040025C6 RID: 9670
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3[] INNER_POINTS_XZ = new Vector3[]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(1f, 0f, 1f),
		new Vector3(0.25f, 0f, 0.25f),
		new Vector3(0.75f, 0f, 0.25f),
		new Vector3(0.25f, 0f, 0.75f),
		new Vector3(0.75f, 0f, 0.75f)
	};

	// Token: 0x040025C7 RID: 9671
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

	// Token: 0x020005D9 RID: 1497
	public enum EnumMode
	{
		// Token: 0x040025C9 RID: 9673
		Grow,
		// Token: 0x040025CA RID: 9674
		Shrink
	}

	// Token: 0x020005DA RID: 1498
	[PublicizedFrom(EAccessModifier.Protected)]
	public class MyInventoryData : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x060030F6 RID: 12534 RVA: 0x0013C0C3 File Offset: 0x0013A2C3
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction, string _particleTransform)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025CB RID: 9675
		public float activateTime;

		// Token: 0x040025CC RID: 9676
		public float lastHitTime;

		// Token: 0x040025CD RID: 9677
		public bool bActivated;

		// Token: 0x040025CE RID: 9678
		public float sphereDistance = 5f;
	}
}
