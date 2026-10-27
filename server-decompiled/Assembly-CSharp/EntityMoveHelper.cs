using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class EntityMoveHelper
{
	// Token: 0x06002A26 RID: 10790 RVA: 0x00108C10 File Offset: 0x00106E10
	public EntityMoveHelper(EntityAlive _entity)
	{
		this.entity = _entity;
		this.random = _entity.rand;
		this.moveToPos = _entity.position;
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x00108C64 File Offset: 0x00106E64
	public void SetMoveTo(Vector3 _pos, bool _canBreakBlocks)
	{
		this.moveToPos = _pos;
		this.moveSpeed = this.entity.GetMoveSpeedAggro();
		this.focusTicks = 0;
		this.isTempMove = false;
		this.CanBreakBlocks = _canBreakBlocks;
		this.isClimb = false;
		this.IsActive = true;
		this.expiryTicks = 10;
		this.ResetStuckCheck();
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x00108CBC File Offset: 0x00106EBC
	public void SetMoveTo(Vector3 _pos, bool _canBreakBlocks, float _speed)
	{
		this.moveToPos = _pos;
		this.moveSpeed = _speed;
		this.focusTicks = 0;
		this.isTempMove = false;
		this.CanBreakBlocks = _canBreakBlocks;
		this.isClimb = false;
		this.IsActive = true;
		this.expiryTicks = 10;
		this.ResetStuckCheck();
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x00108D08 File Offset: 0x00106F08
	public void SetMoveTo(PathEntity path, float _speed, bool _canBreakBlocks)
	{
		PathPoint currentPoint = path.CurrentPoint;
		Vector3 vector = this.moveToPos;
		this.moveToPos = currentPoint.AdjustedPositionForEntity(this.entity);
		this.CanBreakBlocks = _canBreakBlocks;
		bool flag = true;
		if (this.IsActive)
		{
			if ((this.moveToPos - vector).sqrMagnitude < 0.010000001f)
			{
				flag = false;
			}
		}
		else
		{
			this.moveToDir = this.entity.rotation.y;
		}
		if (flag)
		{
			this.focusTicks = 0;
			this.isTempMove = false;
			this.ResetStuckCheck();
		}
		this.hasNextPos = false;
		PathPoint nextPoint = path.NextPoint;
		if (nextPoint != null)
		{
			this.hasNextPos = true;
			this.nextMoveToPos = nextPoint.AdjustedPositionForEntity(this.entity);
		}
		this.moveSpeed = _speed;
		this.isClimb = false;
		this.expiryTicks = 40;
		this.IsActive = true;
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x00108DDA File Offset: 0x00106FDA
	public void Stop()
	{
		this.StopMove();
		this.entity.getNavigator().clearPath();
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x00108DF4 File Offset: 0x00106FF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void StopMove()
	{
		this.IsActive = false;
		if (!this.entity.Jumping || this.entity.isSwimming)
		{
			this.entity.SetMoveForward(0f);
			this.entity.SetRotationAndStopTurning(this.entity.rotation);
		}
		this.expiryTicks = 0;
		this.ClearBlocked();
		this.BlockedEntity = null;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x00108E5C File Offset: 0x0010705C
	public void SetFocusPos(Vector3 _pos)
	{
		this.focusPos = _pos;
		this.focusTicks = 5;
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x00108E6C File Offset: 0x0010706C
	public void UpdateMoveHelper()
	{
		this.destroyRefreshTicks--;
		float num = 0f;
		float num2 = 0f;
		bool flag = true;
		Vector3 position = this.entity.position;
		float num3 = MathUtils.NormalizeAxis(this.entity.rotation.y);
		float num4 = 0f;
		float num5 = 1f;
		if (this.IsActive)
		{
			int num6 = this.expiryTicks - 1;
			this.expiryTicks = num6;
			if (num6 <= 0)
			{
				this.StopMove();
			}
			else
			{
				this.ccHeight = this.entity.m_characterController.GetHeight();
				this.ccRadius = this.entity.m_characterController.GetRadius();
				Vector3 vector = this.moveToPos;
				if (this.isTempMove)
				{
					if (this.BlockedFlags == 0)
					{
						this.isTempMove = false;
						this.ResetStuckCheck();
					}
					else
					{
						vector = this.tempMoveToPos;
					}
				}
				bool jumping = this.entity.Jumping;
				bool flag2 = jumping || this.entity.isSwimming;
				bool flag3 = jumping && !this.entity.isSwimming;
				float num7 = vector.x - position.x;
				float num8 = vector.z - position.z;
				float num9 = num7 * num7 + num8 * num8;
				float num10 = vector.y - (position.y + 0.05f);
				bool flag4 = this.entity.IsInElevator();
				this.isClimb = false;
				if (flag4 && this.entity.bCanClimbLadders && num9 < 0.10890001f && num10 > 0.1f && !jumping)
				{
					this.isClimb = true;
				}
				else if (num9 <= 0.0009f && num10 > -0.25f && num10 < Utils.FastMax(0.25f, this.ccHeight - 0.3f) && !this.isTempMove)
				{
					this.StopMove();
					goto IL_0D91;
				}
				AvatarController avatarController = this.entity.emodel.avatarController;
				if (avatarController.IsRootMotionForced())
				{
					this.entity.SetMoveForwardWithModifiers(this.moveSpeed, 1f, 0f, false);
					this.ResetStuckCheck();
					this.ClearTempMove();
					this.ClearBlocked();
					return;
				}
				if ((!flag2 && !this.isDigging && !avatarController.IsAnimationWithMotionRunning()) || this.entity.sleepingOrWakingUp || !this.entity.bodyDamage.CurrentStun.CanMove() || this.entity.emodel.IsRagdollActive)
				{
					this.entity.SetMoveForward(0f);
					this.ResetStuckCheck();
					this.ClearBlocked();
					flag = false;
				}
				else
				{
					num = this.moveToPos.x - position.x;
					num2 = this.moveToPos.z - position.z;
					float num11 = this.moveToPos.y - (position.y + 0.05f);
					float num12 = num * num + num2 * num2;
					if (EntityMoveHelper.AllowZombieDigging && num11 < -1.1f && num12 <= 0.010000001f && !flag3 && this.entity.onGround)
					{
						this.DigStart(20);
					}
					if (this.isDigging)
					{
						this.DigUpdate();
						flag = false;
					}
					else
					{
						float num13 = Mathf.Atan2(num, num2) * 57.29578f;
						if (flag3)
						{
							this.moveToDir = num13;
						}
						else
						{
							this.moveToDir = Mathf.MoveTowardsAngle(this.moveToDir, num13, 13f);
						}
						this.entity.emodel.ClearLookAt();
						if (this.hasNextPos || num12 >= 0.0225f)
						{
							if (flag3)
							{
								num3 = this.jumpYaw;
							}
							else
							{
								float num14 = num;
								float num15 = num2;
								if (this.hasNextPos && num12 <= 2.25f)
								{
									float num16 = Mathf.Sqrt(num12) / 1.5f;
									num14 = Utils.FastLerp(this.nextMoveToPos.x, this.moveToPos.x, num16) - position.x;
									num15 = Utils.FastLerp(this.nextMoveToPos.z, this.moveToPos.z, num16) - position.z;
								}
								if (this.focusTicks > 0)
								{
									this.focusTicks--;
									num14 = this.focusPos.x - position.x;
									num15 = this.focusPos.z - position.z;
								}
								if (num14 * num14 + num15 * num15 > 0.0001f)
								{
									num3 = Mathf.Atan2(num14, num15) * 57.29578f;
								}
							}
						}
						float num17 = Utils.FastAbs(Utils.DeltaAngle(num13, this.moveToDir));
						if (this.IsUnreachableAbove && !this.entity.IsRunning)
						{
							num5 = 1.3f;
						}
						if (num4 == 0f)
						{
							float num18 = num17 - 15f;
							if (num18 > 0f)
							{
								num5 *= 1f - Utils.FastMin(num18 / 30f, 0.8f);
							}
						}
						if (num5 > 0.5f)
						{
							if (this.BlockedTime > 0.1f)
							{
								num5 = 0.5f;
							}
							if (this.focusTicks > 0)
							{
								num5 = 0.45f;
							}
						}
						if (flag4 && !this.entity.onGround)
						{
							num5 = 0.5f;
						}
						if (this.entity.hasBeenAttackedTime > 0 && this.entity.painResistPercent < 1f)
						{
							num5 = 0.1f;
						}
						if (!this.hasNextPos && !this.isTempMove && !jumping && num9 < 0.36f && num5 > 0.1f)
						{
							float num19 = num5 * Mathf.Sqrt(num9) / 0.6f;
							if (num19 < 0.1f)
							{
								num19 = 0.1f;
							}
							num5 = num19;
						}
						bool isBreakingBlocks = this.entity.IsBreakingBlocks;
						if (isBreakingBlocks)
						{
							num5 = 0.03f;
						}
						if (num5 > 0f)
						{
							float num20 = num7;
							float num21 = num8;
							float num22 = 0.02f * num5;
							float num23 = 1f;
							if (!this.isTempMove)
							{
								if (this.SideStepAngle != 0f)
								{
									float num24 = (this.moveToDir + this.SideStepAngle) * 0.017453292f;
									num20 = Mathf.Sin(num24);
									num21 = Mathf.Cos(num24);
									num22 = 0.025f;
									num23 = 0.06f;
									this.moveToPos = Vector3.MoveTowards(this.moveToPos, position, 0.010000001f);
								}
								else if (num9 > 0.42249995f)
								{
									float num25 = this.moveToDir * 0.017453292f;
									num20 = Mathf.Sin(num25);
									num21 = Mathf.Cos(num25);
								}
							}
							this.entity.MakeMotionMoveToward(num20, num21, num22, num23);
							if (flag4)
							{
								Vector3 vector2 = new Vector3(num7, num10, num8).normalized;
								float num26 = Mathf.Pow(this.moveSpeed, 0.4f);
								if (num10 > 0.1f)
								{
									num26 *= 0.7f;
								}
								else if (num10 < -0.1f)
								{
									num26 *= 1.4f;
								}
								vector2 *= num26 * 0.1f;
								this.entity.motion = vector2;
							}
						}
						if (flag3)
						{
							flag = false;
						}
						else
						{
							if (this.entity.isSwimming && this.entity.swimStrokeRate.x > 0f)
							{
								this.swimStrokeDelayTicks--;
								if (this.swimStrokeDelayTicks <= 0)
								{
									this.swimStrokeDelayTicks = (int)(20f / this.random.RandomRange(this.entity.swimStrokeRate.x, this.entity.swimStrokeRate.y));
									this.StartSwimStroke();
									this.swimStrokeDelayTicks += 3;
								}
							}
							if (isBreakingBlocks || num17 > 60f || num5 == 0f)
							{
								this.moveToTicks = 0;
							}
							else
							{
								num6 = this.moveToTicks + 1;
								this.moveToTicks = num6;
								if (num6 > 6)
								{
									this.moveToTicks = 0;
									float num27 = Mathf.Sqrt(num7 * num7 + num10 * num10 + num8 * num8);
									float num28 = this.moveToDistance - num27;
									if (num28 < 0.021f)
									{
										if (num28 < -0.01f)
										{
											this.moveToDistance = num27;
										}
										num6 = this.moveToFailCnt + 1;
										this.moveToFailCnt = num6;
										if (num6 >= 3 && !AIDirector.debugFreezePos)
										{
											bool flag5 = EntityMoveHelper.AllowZombieDigging && num11 < -1.1f && num12 <= 0.64000005f;
											if (flag5 && this.entity.onGround && this.random.RandomFloat < 0.6f)
											{
												this.DigStart(80);
												flag = false;
												goto IL_0D91;
											}
											this.CheckAreaBlocked();
											if (this.BlockedFlags > 0)
											{
												if (this.random.RandomFloat < 0.7f)
												{
													this.DamageScale = 6f;
													this.obstacleCheckTickDelay = 40;
												}
												else
												{
													this.StartJump(false, 0.5f + this.random.RandomFloat * 0.4f, 1.3f);
												}
												flag = false;
												goto IL_0D91;
											}
											if (flag5)
											{
												flag = false;
												goto IL_0D91;
											}
											if (this.random.RandomFloat > 0.5f)
											{
												if (this.entity.Attack(false))
												{
													this.entity.Attack(true);
												}
											}
											else
											{
												this.StartJump(false, 0.7f + this.random.RandomFloat * 0.8f, 1.4f);
											}
											flag = false;
											goto IL_0D91;
										}
									}
									else
									{
										this.moveToDistance = num27;
										if (num28 >= 0.07f)
										{
											this.moveToFailCnt = 0;
										}
									}
								}
							}
							if (!this.entity.onGround && !this.entity.isSwimming && !flag4 && !this.isClimb && (num11 < -0.5f || num11 > 0.5f))
							{
								this.BlockedTime = 0f;
								this.BlockedEntity = null;
							}
							else
							{
								num6 = this.obstacleCheckTickDelay - 1;
								this.obstacleCheckTickDelay = num6;
								if (num6 <= 0)
								{
									this.obstacleCheckTickDelay = 4;
									this.BlockedEntity = null;
									this.BlockedFlags = 0;
									this.BlockedFlagsAfterCrouch = 0;
									this.blockedDistSq = float.MaxValue;
									if (this.isClimb)
									{
										this.CheckBlockedUp(position);
										this.BlockedFlagsAfterCrouch = this.BlockedFlags;
									}
									else if (num17 < 10f)
									{
										this.CheckEntityBlocked(position, this.moveToPos);
										this.CheckWorldBlocked();
										this.BlockedFlagsAfterCrouch = ((this.entity.crouchType == 0) ? this.BlockedFlags : (this.BlockedFlags & -3));
										if (this.BlockedFlagsAfterCrouch > 0)
										{
											this.obstacleCheckTickDelay = 12;
											this.ResetStuckCheck();
										}
										this.SideStepAngle = 0f;
										if (!this.IsUnreachableAbove && this.hasNextPos && (this.BlockedFlagsAfterCrouch > 0 || this.BlockedEntity))
										{
											this.SideStepAngle = this.CalcObstacleSideStep();
											if (this.SideStepAngle != 0f)
											{
												this.isTempMove = false;
												this.BlockedEntity = null;
												this.ClearBlocked();
											}
										}
										if (this.BlockedEntity)
										{
											if (this.BlockedFlagsAfterCrouch == 0 || this.blockedEntityDistSq < this.blockedDistSq)
											{
												this.moveToTicks = 0;
												if (this.random.RandomFloat < 0.1f)
												{
													if (this.BlockedEntity.moveHelper != null && this.BlockedEntity.moveHelper.BlockedFlags > 0)
													{
														this.StartJump(false, 0.7f, this.BlockedEntity.height * 0.8f);
													}
												}
												else
												{
													this.Push(this.BlockedEntity);
												}
											}
										}
										else if ((this.BlockedFlags > 0 || !this.hasNextPos) && EntityMoveHelper.AllowZombieDigging && num11 < -1.5f && num12 >= 2.25f && this.entity.onGround)
										{
											float num29 = Mathf.Sqrt(num12 + num11 * num11) + 0.001f;
											if (num11 / num29 < -0.86f)
											{
												this.DigStart(160);
											}
										}
									}
								}
							}
							if (this.BlockedFlagsAfterCrouch > 0)
							{
								this.BlockedTime += 0.05f;
							}
							else
							{
								this.BlockedTime = 0f;
							}
							if (this.CanOpenDoors)
							{
								this.CheckForDoorAndOpen();
							}
							if (this.entity.CanEntityJump() && !this.isClimb && !flag2)
							{
								float num30 = 0f;
								float num31 = 0.9f;
								if (this.BlockedTime > 0.1f && this.BlockedFlags == 1)
								{
									num30 = 0.5f + this.random.RandomFloat * 0.3f;
								}
								else if (num11 > 0.9f && num12 <= 0.16000001f && this.random.RandomFloat < 0.1f)
								{
									num30 = 0.05f + this.random.RandomFloat * 0.2f;
									num31 = 1f;
								}
								if (this.IsUnreachableSideJump && num17 < 25f)
								{
									PathEntity path = this.entity.navigator.getPath();
									if (path == null || path.NodeCountRemaining() <= 1)
									{
										Vector3 vector3 = this.entity.position + this.entity.GetForwardVector() * 0.2f;
										vector3.y += 0.4f;
										RaycastHit raycastHit;
										if (!Physics.Raycast(vector3 - Origin.position, Vector3.down, out raycastHit, 3.4f, 1082195968) || raycastHit.distance > 2.2f)
										{
											num30 = this.entity.jumpMaxDistance;
											num31 = this.UnreachablePos.y - this.entity.position.y;
										}
									}
								}
								if (num30 > 0f)
								{
									if (!this.CheckJumpBlocked(position, num12))
									{
										this.StartJump(true, num30, num31);
										if (this.IsUnreachableSideJump)
										{
											this.UnreachablePercent += 0.1f;
											this.IsDestroyAreaTryUnreachable = true;
										}
									}
									this.IsUnreachableSideJump = false;
								}
							}
						}
					}
				}
			}
		}
		else
		{
			this.moveSpeed = 0f;
		}
		IL_0D91:
		if (flag)
		{
			this.entity.CalcStrafeYawOffset(num, num2, ref num3, ref num4);
		}
		if (this.IsActive)
		{
			this.entity.SetMoveForwardWithModifiers(this.moveSpeed, num5, num4, this.isClimb);
		}
		if (this.IsActive || !this.entity.IsSeekYaw())
		{
			this.entity.SeekYaw(num3, 0f, 30f);
		}
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x00109C70 File Offset: 0x00107E70
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckForDoorAndOpen()
	{
		PathEntity path = this.entity.navigator.getPath();
		if (path != null && !path.isFinished())
		{
			PathPoint currentPoint = path.CurrentPoint;
			if (currentPoint != null)
			{
				Vector3i blockPos = currentPoint.GetBlockPos();
				Block block = this.entity.world.GetBlock(blockPos).Block;
				if (block.HasTag(BlockTags.Door) && block is BlockCompositeTileEntity)
				{
					TileEntityComposite tileEntityComposite = this.entity.world.GetTileEntity(blockPos) as TileEntityComposite;
					TEFeatureDoor tefeatureDoor;
					if (tileEntityComposite != null && tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor) && !tefeatureDoor.IsOpen())
					{
						bool flag = false;
						TEFeatureLockable tefeatureLockable;
						if (tileEntityComposite.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable))
						{
							flag = tefeatureLockable.IsLocked();
						}
						if (!flag)
						{
							tefeatureDoor.SetOpen(true, true);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x00109D38 File Offset: 0x00107F38
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckJumpBlocked(Vector3 position, float moveToDistXZSq)
	{
		Vector3i vector3i = new Vector3i(Utils.Fastfloor(position.x), Utils.Fastfloor(position.y + 2.35f), Utils.Fastfloor(position.z));
		BlockValue block = this.entity.world.GetBlock(vector3i);
		bool flag = block.Block.IsMovementBlocked(this.entity.world, vector3i, block, BlockFace.None);
		if (flag)
		{
			Vector3 vector = position - Origin.position;
			Vector3 vector2 = Vector3.up;
			if (moveToDistXZSq > 0.25f)
			{
				Vector3 normalized = (this.moveToPos - position).normalized;
				vector2 = Vector3.Slerp(vector2, normalized, 0.28f);
			}
			vector.y += 0.5f;
			float num = this.ccRadius - 0.05f;
			float num2 = this.entity.physicsBaseHeight + 0.88f - 0.5f - num;
			RaycastHit raycastHit;
			flag = Physics.SphereCast(vector, num, vector2, out raycastHit, num2, 1082195968);
		}
		return flag;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x00109E3C File Offset: 0x0010803C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckWorldBlocked()
	{
		this.BlockedFlags = 0;
		this.DamageScale = 1f;
		Vector3 headPosition = this.entity.getHeadPosition();
		headPosition.x = this.entity.position.x * 0.5f + headPosition.x * 0.5f;
		headPosition.z = this.entity.position.z * 0.5f + headPosition.z * 0.5f;
		headPosition.y = this.entity.position.y;
		Vector3 vector = this.moveToPos;
		if (this.ccHeight < 1f)
		{
			float num = Utils.FastMax(this.ccHeight, 0.7f);
			headPosition.y += num - 0.125f;
			vector.y = headPosition.y + 1f;
			headPosition.y += 0.3f;
			this.CheckBlocked(headPosition, vector, 1, false, this.HitInfo2);
			headPosition.y -= 0.3f;
			vector.y = headPosition.y;
			this.CheckBlocked(headPosition, vector, 0, true, this.HitInfo);
			if (this.BlockedFlags == 2)
			{
				this.HitInfo.CopyFrom(this.HitInfo2);
				if (this.entity.crouchType != 0 || this.entity.physicsHeight < 1f)
				{
					this.isTempMove = false;
					return;
				}
			}
			else if (this.BlockedFlags == 3)
			{
				this.SelectBestHit();
				return;
			}
		}
		else
		{
			float num2 = Utils.FastClamp(this.ccHeight, 1.225f, 1.5f);
			headPosition.y += num2;
			vector.y = Utils.FastMax(headPosition.y, this.moveToPos.y + 0.125f + 0.3f);
			this.CheckBlocked(headPosition, vector, 1, false, this.HitInfo2);
			Vector3 vector2 = headPosition;
			vector2.y = this.entity.position.y + this.entity.stepHeight + 0.125f;
			vector.y = vector2.y;
			this.CheckBlocked(vector2, vector, 0, true, this.HitInfo);
			if ((this.BlockedFlags & 2) > 0)
			{
				if (this.BlockedFlags == 2)
				{
					this.HitInfo.CopyFrom(this.HitInfo2);
					if (this.entity.crouchType != 0)
					{
						this.isTempMove = false;
						return;
					}
				}
				else
				{
					this.SelectBestHit();
				}
				return;
			}
			if (this.BlockedFlags > 0)
			{
				vector.y = headPosition.y + 1f;
				this.CheckBlocked(headPosition, vector, 2, false, this.HitInfo2);
				if ((this.BlockedFlags & 4) > 0 && (this.HitInfo.hit.blockPos.x != Utils.Fastfloor(this.entity.position.x) || this.HitInfo.hit.blockPos.z != Utils.Fastfloor(this.entity.position.z)))
				{
					this.SelectBestHit();
					BlockValue blockValue = this.HitInfo.hit.blockValue;
					float num3 = (float)(blockValue.Block.MaxDamage - blockValue.damage);
					BlockValue blockValue2 = this.HitInfo2.hit.blockValue;
					if ((float)(blockValue2.Block.MaxDamage - blockValue2.damage) < num3 * 0.7f)
					{
						this.HitInfo.CopyFrom(this.HitInfo2);
					}
				}
			}
		}
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x0010A1B0 File Offset: 0x001083B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectBestHit()
	{
		BlockValue blockValue = this.HitInfo.hit.blockValue;
		float num = (float)(blockValue.Block.MaxDamage - blockValue.damage);
		BlockValue blockValue2 = this.HitInfo2.hit.blockValue;
		if ((float)(blockValue2.Block.MaxDamage - blockValue2.damage) < num * 0.7f)
		{
			this.HitInfo.CopyFrom(this.HitInfo2);
		}
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x0010A224 File Offset: 0x00108424
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckBlocked(Vector3 pos, Vector3 endPos, int baseY, bool checkSlope, WorldRayHitInfo hitInfo)
	{
		endPos.y -= 0.01f;
		Vector3 vector = endPos - pos;
		float num = vector.magnitude + 0.001f;
		vector *= 1f / num;
		Ray ray = new Ray(pos - vector * 0.375f, vector);
		if (num > this.ccRadius + 0.35f)
		{
			num = this.ccRadius + 0.35f;
			if (this.isTempMove)
			{
				num += 0.4f;
			}
		}
		if (vector.y >= 0.2f)
		{
			num += 0.21f;
		}
		if (Voxel.Raycast(this.entity.world, ray, num - 0.125f + 0.375f, 1082195968, 128, 0.125f))
		{
			if (checkSlope && this.BlockedFlags == 0 && Voxel.phyxRaycastHit.normal.y > 0.643f)
			{
				Vector2 vector2;
				vector2.x = Voxel.phyxRaycastHit.normal.x;
				vector2.y = Voxel.phyxRaycastHit.normal.z;
				vector2.Normalize();
				Vector2 vector3;
				vector3.x = vector.x;
				vector3.y = vector.z;
				vector3.Normalize();
				if (vector3.x * vector2.x + vector3.y * vector2.y < -0.7f)
				{
					return;
				}
			}
			if (Voxel.voxelRayHitInfo.hit.blockValue.Block is BlockDamage)
			{
				return;
			}
			hitInfo.CopyFrom(Voxel.voxelRayHitInfo);
			this.BlockedFlags |= 1 << baseY;
			Vector3 vector4 = pos - hitInfo.hit.pos;
			float sqrMagnitude = vector4.sqrMagnitude;
			if (sqrMagnitude < this.blockedDistSq)
			{
				this.blockedDistSq = sqrMagnitude;
				float num2 = 1f / Mathf.Sqrt(sqrMagnitude);
				float num3 = this.ccRadius + 0.4f;
				this.tempMoveToPos = vector4 * (num2 * num3) + hitInfo.hit.pos;
				this.tempMoveToPos.y = Mathf.MoveTowards(this.tempMoveToPos.y, this.moveToPos.y, 1f);
				this.isTempMove = true;
			}
		}
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x0010A478 File Offset: 0x00108678
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckBlockedUp(Vector3 pos)
	{
		this.BlockedFlags = 0;
		Vector3 headPosition = this.entity.getHeadPosition();
		headPosition.x = pos.x;
		headPosition.z = pos.z;
		headPosition.y -= 0.625f;
		Ray ray = new Ray(headPosition, Vector3.up);
		if (Voxel.Raycast(this.entity.world, ray, 1f, 1082195968, 128, 0.125f))
		{
			if (Voxel.voxelRayHitInfo.hit.blockValue.Block is BlockDamage)
			{
				return;
			}
			this.HitInfo.CopyFrom(Voxel.voxelRayHitInfo);
			this.BlockedFlags = 4;
			float sqrMagnitude = (pos - this.HitInfo.hit.pos).sqrMagnitude;
			if (sqrMagnitude < this.blockedDistSq)
			{
				this.blockedDistSq = sqrMagnitude;
				this.obstacleCheckTickDelay = 12;
				this.ResetStuckCheck();
			}
		}
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x0010A56C File Offset: 0x0010876C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckAreaBlocked()
	{
		this.BlockedFlags = 0;
		Vector3 headPosition = this.entity.getHeadPosition();
		headPosition.y = this.entity.position.y;
		Vector3 vector = this.moveToPos - headPosition;
		float num = Mathf.Atan2(vector.x, vector.z);
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Cos(num);
		vector.Normalize();
		Vector3 vector2 = headPosition + vector * 0.575f;
		for (float num4 = this.ccHeight - 0.125f; num4 > 0.225f; num4 -= 0.25f)
		{
			for (int i = 0; i < 3; i++)
			{
				float num5 = EntityMoveHelper.checkEdgeXs[i];
				float num6 = num5 * num3;
				float num7 = num5 * -num2;
				Vector3 vector3 = headPosition;
				vector3.x += num6;
				vector3.y += num4;
				vector3.z += num7;
				Vector3 vector4 = vector2;
				vector4.x += num6;
				vector4.y += num4;
				vector4.z += num7;
				this.CheckBlocked(vector3, vector4, 0, false, this.HitInfo);
				if (this.BlockedFlags > 0)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06002A35 RID: 10805 RVA: 0x0010A6AC File Offset: 0x001088AC
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcObstacleSideStep()
	{
		Vector3 headPosition = this.entity.getHeadPosition();
		headPosition.y = this.entity.position.y;
		Vector3 vector = this.moveToPos - headPosition;
		if (vector.y >= 0.6f)
		{
			return 0f;
		}
		float num = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
		if (num <= this.ccRadius + 0.05f)
		{
			return 0f;
		}
		Vector2 vector2 = new Vector2(vector.x / num, vector.z / num);
		headPosition.x -= vector2.x * 0.2f;
		headPosition.z -= vector2.y * 0.2f;
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (this.CalcObstacleSideStepArc(headPosition, num2, 8f, 20f, 10f) == 0f && this.CalcObstacleSideStepArc(headPosition, num2, -8f, -20f, -10f) == 0f)
		{
			return 0f;
		}
		float num3 = this.CalcObstacleSideStepArc(headPosition, num2, -48f, -20f, 11f);
		float num4 = this.CalcObstacleSideStepArc(headPosition, num2, 48f, 20f, -11f);
		if (Utils.FastAbs(num3) < num4)
		{
			if (num3 <= -48f)
			{
				return 0f;
			}
			if (num3 == 0f)
			{
				num3 = -20f;
			}
			return num3 - 50f;
		}
		else
		{
			if (num4 >= 48f)
			{
				return 0f;
			}
			if (num4 == 0f)
			{
				num4 = 20f;
			}
			return num4 + 50f;
		}
	}

	// Token: 0x06002A36 RID: 10806 RVA: 0x0010A85C File Offset: 0x00108A5C
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcObstacleSideStepArc(Vector3 startPos, float angleRad, float dirMin, float dirMax, float dirStep)
	{
		float num = this.ccRadius + 0.45f;
		Vector3 vector = startPos;
		Vector3 vector2;
		vector2.y = 0f;
		float num2 = dirMin;
		int num3 = (int)Utils.FastAbs((dirMax - dirMin) / dirStep) + 1;
		for (int i = 0; i < num3; i++)
		{
			float num4 = num2 * 0.017453292f;
			float num5 = angleRad + num4;
			vector2.x = Mathf.Sin(num5);
			vector2.z = Mathf.Cos(num5);
			float num6 = num / Mathf.Cos(num4);
			for (float num7 = this.ccHeight - 0.1f; num7 > 0.3f; num7 -= 0.9f)
			{
				vector.y = startPos.y + num7;
				RaycastHit raycastHit;
				if (Physics.SphereCast(vector - Origin.position, 0.1f, vector2, out raycastHit, num6, 1082720256))
				{
					return num2;
				}
			}
			num2 += dirStep;
		}
		return 0f;
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x0010A944 File Offset: 0x00108B44
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckEntityBlocked(Vector3 pos, Vector3 endPos)
	{
		Vector3 vector = endPos - pos;
		pos.y += 0.7f;
		RaycastHit raycastHit;
		if (Physics.SphereCast(pos - Origin.position, 0.15f, vector, out raycastHit, 0.8f, 524288))
		{
			Transform transform = raycastHit.transform;
			if (transform)
			{
				Transform transform2 = transform.parent.Find("GameObject");
				if (transform2)
				{
					EntityAlive component = transform2.GetComponent<EntityAlive>();
					if (component && component != this.entity)
					{
						float sqrMagnitude = (this.entity.position - component.position).sqrMagnitude;
						float num = this.ccRadius + component.m_characterController.GetRadius() + 0.16f + 0.25f;
						if (sqrMagnitude < num * num)
						{
							this.BlockedEntity = component;
							this.blockedEntityDistSq = sqrMagnitude;
						}
					}
				}
			}
		}
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x0010AA38 File Offset: 0x00108C38
	public void StartJump(bool calcYaw, float distance = 0f, float heightDiff = 0f)
	{
		if (!this.entity.Jumping && (this.entity.onGround || this.entity.IsInElevator()) && !this.entity.Electrocuted)
		{
			this.JumpToPos = this.moveToPos;
			if (!calcYaw)
			{
				this.jumpYaw = this.entity.rotation.y;
			}
			else
			{
				float num = this.moveToPos.x - this.entity.position.x;
				float num2 = this.moveToPos.z - this.entity.position.z;
				this.jumpYaw = Mathf.Atan2(num, num2) * 57.29578f;
			}
			this.entity.Jumping = true;
			this.entity.SetJumpDistance(distance, heightDiff);
			this.ClearBlocked();
		}
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x0010AB18 File Offset: 0x00108D18
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartSwimStroke()
	{
		if (!this.entity.Jumping)
		{
			this.JumpToPos = this.moveToPos;
			float num = this.moveToPos.x - this.entity.position.x;
			float num2 = this.moveToPos.z - this.entity.position.z;
			this.jumpYaw = Mathf.Atan2(num, num2) * 57.29578f;
			this.entity.Jumping = true;
			this.entity.SetSwimValues((float)this.swimStrokeDelayTicks, this.moveToPos - this.entity.position);
		}
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x0010ABC4 File Offset: 0x00108DC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Push(EntityAlive blockerEntity)
	{
		Vector3 normalized = (blockerEntity.position - this.entity.position).normalized;
		this.damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing, normalized);
		float massKg = EntityClass.list[this.entity.entityClass].MassKg;
		this.damageResponse.StunDuration = 0f;
		this.damageResponse.Strength = (int)(massKg * 0.05f);
		blockerEntity.DoRagdoll(in this.damageResponse);
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x0010AC50 File Offset: 0x00108E50
	[PublicizedFrom(EAccessModifier.Private)]
	public void AttackPush(EntityAlive blockerEntity)
	{
		Vector3 normalized = (blockerEntity.position - this.entity.position).normalized;
		this.damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing, normalized);
		ItemActionAttackData itemActionAttackData = this.entity.inventory.holdingItemData.actionData[0] as ItemActionAttackData;
		if (itemActionAttackData != null)
		{
			itemActionAttackData.hitDelegate = new ItemActionAttackData.HitDelegate(this.GetAttackHitInfo);
			if (this.entity.Attack(false))
			{
				this.entity.Attack(true);
			}
		}
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x0010ACE0 File Offset: 0x00108EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldRayHitInfo GetAttackHitInfo(out float damageMpy)
	{
		if (this.BlockedEntity)
		{
			float massKg = EntityClass.list[this.entity.entityClass].MassKg;
			if (this.random.RandomFloat < 0.3f)
			{
				this.damageResponse.StunDuration = 0.5f;
				this.damageResponse.Strength = (int)(massKg * 0.4f);
			}
			else
			{
				this.damageResponse.StunDuration = 0f;
				this.damageResponse.Strength = (int)(massKg * 0.2f);
			}
			this.BlockedEntity.DoRagdoll(in this.damageResponse);
		}
		damageMpy = 0f;
		return null;
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x0010AD8C File Offset: 0x00108F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DigStart(int forTicks)
	{
		this.digStartPos = this.entity.position;
		if (this.isDigging)
		{
			this.digForTicks = Utils.FastMax(this.digForTicks, (float)forTicks);
			return;
		}
		if (!this.CanBreakBlocks)
		{
			return;
		}
		this.digForTicks = (float)forTicks;
		this.digTicks = 0f;
		this.digActionTicks = 18f;
		this.digAttacked = false;
		this.digForwardCount = 0f;
		AvatarController avatarController = this.entity.emodel.avatarController;
		avatarController.CancelEvent("EndTrigger");
		avatarController.TriggerEvent("DigStartTrigger");
		this.isDigging = true;
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x0010AE2C File Offset: 0x0010902C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DigUpdate()
	{
		float num = this.digForTicks - 1f;
		this.digForTicks = num;
		if (num <= 0f)
		{
			this.DigStop();
			return;
		}
		this.entity.SetMoveForward(0f);
		if (this.entity.world.IsDark())
		{
			this.expiryTicks = 5;
		}
		this.digTicks += 1f;
		if (this.digTicks < this.digActionTicks)
		{
			return;
		}
		if (!this.entity.emodel.avatarController.IsAnimationDigRunning())
		{
			this.isDigging = false;
			return;
		}
		if ((this.entity.position - this.digStartPos).sqrMagnitude >= 0.25f)
		{
			this.DigStop();
			return;
		}
		if (!this.digAttacked)
		{
			this.entity.emodel.avatarController.TriggerEvent("DigTrigger");
			this.digTicks = 0f;
			this.digActionTicks = 4f;
			this.digAttacked = true;
			return;
		}
		this.digActionTicks = 14f;
		this.digAttacked = false;
		Vector3 position = this.entity.position;
		position.y += 0.6f;
		Vector3 vector;
		float num2;
		if (this.digForwardCount > 0f)
		{
			this.digForwardCount -= 1f;
			vector = this.entity.GetForwardVector();
			num2 = 1.1f;
			this.entity.SeekYaw(this.entity.rotation.y + (this.random.RandomFloat * 2f - 1f) * 120f, 0f, 120f);
		}
		else
		{
			position.x += (this.random.RandomFloat - 0.5f) * 0.3f;
			position.z += (this.random.RandomFloat - 0.5f) * 0.3f;
			vector = this.moveToPos - position;
			num2 = 1.4000001f;
		}
		Ray ray = new Ray(position, vector);
		if (Voxel.Raycast(this.entity.world, ray, num2, 1082195968, 128, 0.15f))
		{
			WorldRayHitInfo voxelRayHitInfo = Voxel.voxelRayHitInfo;
			DamageMultiplier damageMultiplier = new DamageMultiplier();
			List<string> list = null;
			ItemActionAttack.AttackHitInfo attackHitInfo = new ItemActionAttack.AttackHitInfo();
			attackHitInfo.hardnessScale = 1f;
			float num3 = 1f;
			ItemActionAttack itemActionAttack = this.entity.inventory.holdingItem.Actions[0] as ItemActionAttack;
			if (itemActionAttack != null)
			{
				num3 = itemActionAttack.GetDamageBlock(this.entity.inventory.holdingItemData.actionData[0].invData.itemValue, BlockValue.Air, null, 0);
			}
			ItemActionAttack.Hit(voxelRayHitInfo, this.entity.entityId, EnumDamageTypes.Bashing, num3, num3, 1f, 1f, 0f, 0.05f, "organic", damageMultiplier, list, attackHitInfo, 1, 0, 0f, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, -1, null, false, false, true, null);
			return;
		}
		if (this.digForwardCount == 0f)
		{
			this.digForwardCount = 2f;
			return;
		}
		this.digForwardCount = 0f;
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x0010B14A File Offset: 0x0010934A
	[PublicizedFrom(EAccessModifier.Private)]
	public void DigStop()
	{
		if (this.isDigging)
		{
			this.isDigging = false;
			this.entity.emodel.avatarController.TriggerEvent("EndTrigger");
		}
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x0010B178 File Offset: 0x00109378
	public float CalcBlockedDistanceSq()
	{
		Vector3 pos = this.HitInfo.hit.pos;
		Vector3 position = this.entity.position;
		float num = pos.x - position.x;
		float num2 = pos.z - position.z;
		return num * num + num2 * num2;
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x0010B1C3 File Offset: 0x001093C3
	public void ClearBlocked()
	{
		this.BlockedFlags = 0;
		this.BlockedFlagsAfterCrouch = 0;
		this.BlockedTime = 0f;
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x0010B1DE File Offset: 0x001093DE
	public void ClearTempMove()
	{
		this.isTempMove = false;
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x0010B1E7 File Offset: 0x001093E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetStuckCheck()
	{
		this.SideStepAngle = 0f;
		this.moveToTicks = 0;
		this.moveToFailCnt = 0;
		if (this.isTempMove)
		{
			this.moveToDistance = this.CalcTempMoveDist();
			return;
		}
		this.moveToDistance = this.CalcMoveDist();
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x0010B224 File Offset: 0x00109424
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcMoveDist()
	{
		Vector3 position = this.entity.position;
		float num = this.moveToPos.x - position.x;
		float num2 = this.moveToPos.z - position.z;
		float num3 = this.moveToPos.y - position.y;
		return Mathf.Sqrt(num * num + num3 * num3 + num2 * num2);
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x0010B284 File Offset: 0x00109484
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcTempMoveDist()
	{
		Vector3 position = this.entity.position;
		float num = this.tempMoveToPos.x - position.x;
		float num2 = this.tempMoveToPos.z - position.z;
		float num3 = this.tempMoveToPos.y - position.y;
		return Mathf.Sqrt(num * num + num3 * num3 + num2 * num2);
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x0010B2E4 File Offset: 0x001094E4
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 CalcBlockCenterXZ(Vector3 pos)
	{
		pos.x = (float)Utils.Fastfloor(pos.x) + 0.5f;
		pos.z = (float)Utils.Fastfloor(pos.z) + 0.5f;
		return pos;
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x0010B31C File Offset: 0x0010951C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 CalcBlockCenter(Vector3 pos)
	{
		pos.x = (float)Utils.Fastfloor(pos.x) + 0.5f;
		pos.y = (float)Utils.Fastfloor(pos.y) + 0.5f;
		pos.z = (float)Utils.Fastfloor(pos.z) + 0.5f;
		return pos;
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x0010B378 File Offset: 0x00109578
	public void CalcIfUnreachablePos()
	{
		this.IsUnreachableSideJump = false;
		if (this.entity.Jumping)
		{
			return;
		}
		this.IsUnreachableAbove = false;
		this.IsUnreachableSide = false;
		PathEntity path = this.entity.navigator.getPath();
		if (path != null)
		{
			Vector3 toPos = path.toPos;
			Vector3 rawEndPos = path.rawEndPos;
			float num = rawEndPos.x - toPos.x;
			float num2 = rawEndPos.z - toPos.z;
			float num3 = num * num + num2 * num2;
			float num4 = toPos.y - rawEndPos.y;
			if (num4 > this.ccHeight + 0.8f && num3 < 25f)
			{
				this.IsUnreachableAbove = true;
				this.UnreachablePos = rawEndPos;
			}
			if (num4 >= -1.5f && num3 >= 1.44f)
			{
				this.IsUnreachableSide = true;
				this.UnreachablePos = rawEndPos;
				float num5 = this.entity.jumpMaxDistance;
				if (num5 > 0f && num4 < 0.5f + num5 * 0.5f)
				{
					num5 += 3.4f;
					if (num3 <= num5 * num5)
					{
						this.IsUnreachableSideJump = true;
					}
				}
			}
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x0010B488 File Offset: 0x00109688
	public bool IsMoveToAbove()
	{
		return this.moveToPos.y - this.entity.position.y > 1.9f;
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x0010B4B0 File Offset: 0x001096B0
	public bool FindExistingDestroyPos(ref Vector3 destroyPos)
	{
		if (this.GetExistingDestroyPos(ref destroyPos))
		{
			return true;
		}
		this.entity.world.GetEntitiesAround(EntityFlags.AISmelling, destroyPos, 20f, EntityMoveHelper.entityTempList);
		int count = EntityMoveHelper.entityTempList.Count;
		if (count > 1)
		{
			int num = this.random.RandomRange(count);
			for (int i = 0; i < count; i++)
			{
				EntityAlive entityAlive = (EntityAlive)EntityMoveHelper.entityTempList[(i + num) % count];
				if (entityAlive != this.entity && entityAlive.moveHelper != null && entityAlive.moveHelper.GetExistingDestroyPos(ref destroyPos))
				{
					EntityMoveHelper.entityTempList.Clear();
					return true;
				}
			}
			EntityMoveHelper.entityTempList.Clear();
		}
		return false;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x0010B564 File Offset: 0x00109764
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GetExistingDestroyPos(ref Vector3 destroyPos)
	{
		if (this.destroyRefreshTicks > 0 && this.destroyPosition.y > 0f)
		{
			ChunkCluster chunkCache = this.entity.world.ChunkCache;
			Vector3i vector3i = World.worldToBlockPos(this.destroyPosition);
			BlockValue block = chunkCache.GetBlock(vector3i);
			Block block2 = block.Block;
			if (block2.IsMovementBlocked(this.entity.world, vector3i, block, BlockFace.None) && block2.StabilitySupport)
			{
				destroyPos = this.destroyPosition;
				return true;
			}
			this.destroyPosition.y = 0f;
		}
		return false;
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x0010B5F8 File Offset: 0x001097F8
	public bool FindDestroyPos(ref Vector3 destroyPos, int destroyRadius, bool isLookFar)
	{
		this.destroyPosition.y = 0f;
		if (this.SearchForDestroyPos(ref destroyPos, destroyRadius, isLookFar))
		{
			this.destroyRefreshTicks = 500;
			this.destroyPosition = destroyPos;
			return true;
		}
		return false;
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x0010B630 File Offset: 0x00109830
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SearchForDestroyPos(ref Vector3 destroyPos, int destroyRadius, bool isLookFar)
	{
		int num = int.MaxValue;
		Vector3i vector3i = Vector3i.zero;
		World world = this.entity.world;
		ChunkCluster chunkCache = world.ChunkCache;
		Vector3i vector3i2 = World.worldToBlockPos(destroyPos);
		int num2 = 0;
		int num3 = 1;
		if (isLookFar)
		{
			num2 = this.random.RandomRange(destroyRadius / 2, destroyRadius);
			num3 = -1;
			vector3i2.y -= 2;
		}
		int num4 = this.random.RandomRange(0, 4);
		int num5 = 0;
		vector3i2.y = Utils.FastMax(2, vector3i2.y);
		BlockValue[] array = new BlockValue[7];
		IChunk chunk = null;
		while (num2 >= 0 && num2 <= destroyRadius)
		{
			int num6 = num2 * 2;
			for (int i = 0; i < 4; i++)
			{
				EntityMoveHelper.DestroyData destroyData = EntityMoveHelper.destroyData[i + num4];
				int num7 = destroyData.offsetX * num2;
				int num8 = destroyData.offsetZ * num2;
				Vector3i vector3i3;
				vector3i3.x = vector3i2.x + num7;
				vector3i3.z = vector3i2.z + num8;
				int num9 = 0;
				do
				{
					world.GetChunkFromWorldPos(vector3i3.x, vector3i3.z, ref chunk);
					if (chunk != null)
					{
						chunk.GetBlockColumn(World.toBlockXZ(vector3i3.x), vector3i2.y + -2, World.toBlockXZ(vector3i3.z), array);
						for (int j = -2; j <= 2; j++)
						{
							int num10 = j - -2;
							BlockValue blockValue = array[num10 + 1];
							if (!blockValue.isair)
							{
								Block block = blockValue.Block;
								if (block.StabilitySupport)
								{
									BlockValue blockValue2 = array[num10];
									if (!blockValue2.isair)
									{
										Block block2 = blockValue2.Block;
										if (block2.StabilitySupport)
										{
											vector3i3.y = vector3i2.y + j;
											int num11 = 0;
											int num12 = 0;
											int num13 = block2.MaxDamagePlusDowngrades - (blockValue2.damage & -128);
											if (block2.shape.IsTerrain())
											{
												num13 *= 51;
												num11++;
											}
											if (block.shape.IsTerrain())
											{
												num13 *= 2;
												num11++;
											}
											if (num11 == 0)
											{
												BlockValue blockValue3 = array[num10 + 2];
												if (!blockValue3.isair && blockValue3.Block.StabilitySupport)
												{
													num12++;
													num13 /= 2;
													if (num10 < 4)
													{
														BlockValue blockValue4 = array[num10 + 3];
														if (!blockValue4.isair)
														{
															num12++;
															num13 /= 4;
														}
													}
												}
											}
											if (num13 < num && (num5 == 0 || num11 < 2) && this.IsABlockSideOpen(vector3i3, ref chunk))
											{
												num5 += num12;
												num = num13;
												vector3i = vector3i3;
											}
										}
									}
								}
							}
						}
					}
					vector3i3.x += destroyData.stepX;
					vector3i3.z += destroyData.stepZ;
				}
				while (++num9 < num6);
				if (num2 == 0)
				{
					break;
				}
			}
			if (num5 >= 2 && num2 >= 5)
			{
				break;
			}
			num2 += num3;
		}
		if (num > 999999)
		{
			return false;
		}
		destroyPos = vector3i.ToVector3CenterXZ();
		return true;
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x0010B93C File Offset: 0x00109B3C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsABlockSideOpen(Vector3i _checkPos, ref IChunk _chunk)
	{
		World world = this.entity.world;
		Vector3i vector3i = _checkPos;
		for (int i = 0; i < 8; i += 2)
		{
			vector3i.x = _checkPos.x + EntityMoveHelper.blockOpenOffsets[i];
			vector3i.z = _checkPos.z + EntityMoveHelper.blockOpenOffsets[i + 1];
			world.GetChunkFromWorldPos(vector3i.x, vector3i.z, ref _chunk);
			if (_chunk != null)
			{
				BlockValue blockNoDamage = _chunk.GetBlockNoDamage(World.toBlockXZ(vector3i.x), vector3i.y, World.toBlockXZ(vector3i.z));
				if (!blockNoDamage.Block.IsMovementBlocked(world, vector3i, blockNoDamage, BlockFace.None))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0400200E RID: 8206
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDoneXZDistSq = 0.0009f;

	// Token: 0x0400200F RID: 8207
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCheckBlockedDist = 0.35f;

	// Token: 0x04002010 RID: 8208
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCheckBlockedRadius = 0.125f;

	// Token: 0x04002011 RID: 8209
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCheckSidestepDist = 0.35f;

	// Token: 0x04002012 RID: 8210
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCheckSidestepRadius = 0.1f;

	// Token: 0x04002013 RID: 8211
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTempMoveDist = 0.4f;

	// Token: 0x04002014 RID: 8212
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cYawNextDist = 1.5f;

	// Token: 0x04002015 RID: 8213
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMoveDirectDist = 0.65f;

	// Token: 0x04002016 RID: 8214
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMoveSlowDist = 0.6f;

	// Token: 0x04002017 RID: 8215
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDigXZDistSq = 0.010000001f;

	// Token: 0x04002018 RID: 8216
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDigDiagonalXZDistSq = 2.25f;

	// Token: 0x04002019 RID: 8217
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDigAngleCos = 0.86f;

	// Token: 0x0400201A RID: 8218
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cJumpUpXZDistSq = 0.16000001f;

	// Token: 0x0400201B RID: 8219
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLadderXZDistSq = 0.10890001f;

	// Token: 0x0400201C RID: 8220
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cUnreachJumpMin = 1.2f;

	// Token: 0x0400201D RID: 8221
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollisionMask = 1082195968;

	// Token: 0x0400201E RID: 8222
	public bool IsActive;

	// Token: 0x0400201F RID: 8223
	public bool CanBreakBlocks;

	// Token: 0x04002020 RID: 8224
	public bool CanOpenDoors;

	// Token: 0x04002021 RID: 8225
	public Vector3 JumpToPos;

	// Token: 0x04002022 RID: 8226
	public EntityAlive BlockedEntity;

	// Token: 0x04002023 RID: 8227
	public int BlockedFlags;

	// Token: 0x04002024 RID: 8228
	public int BlockedFlagsAfterCrouch;

	// Token: 0x04002025 RID: 8229
	public float BlockedTime;

	// Token: 0x04002026 RID: 8230
	public WorldRayHitInfo HitInfo = new WorldRayHitInfo();

	// Token: 0x04002027 RID: 8231
	public WorldRayHitInfo HitInfo2 = new WorldRayHitInfo();

	// Token: 0x04002028 RID: 8232
	public float DamageScale;

	// Token: 0x04002029 RID: 8233
	public bool IsUnreachableAbove;

	// Token: 0x0400202A RID: 8234
	public bool IsUnreachableSide;

	// Token: 0x0400202B RID: 8235
	public bool IsUnreachableSideJump;

	// Token: 0x0400202C RID: 8236
	public Vector3 UnreachablePos;

	// Token: 0x0400202D RID: 8237
	public float SideStepAngle;

	// Token: 0x0400202E RID: 8238
	public float UnreachablePercent;

	// Token: 0x0400202F RID: 8239
	public bool IsDestroyAreaTryUnreachable;

	// Token: 0x04002030 RID: 8240
	public bool IsDestroyArea;

	// Token: 0x04002031 RID: 8241
	[PublicizedFrom(EAccessModifier.Private)]
	public DamageResponse damageResponse = DamageResponse.New(false);

	// Token: 0x04002032 RID: 8242
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entity;

	// Token: 0x04002033 RID: 8243
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom random;

	// Token: 0x04002034 RID: 8244
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 moveToPos;

	// Token: 0x04002035 RID: 8245
	[PublicizedFrom(EAccessModifier.Private)]
	public float moveToDistance;

	// Token: 0x04002036 RID: 8246
	[PublicizedFrom(EAccessModifier.Private)]
	public int moveToTicks;

	// Token: 0x04002037 RID: 8247
	[PublicizedFrom(EAccessModifier.Private)]
	public int moveToFailCnt;

	// Token: 0x04002038 RID: 8248
	[PublicizedFrom(EAccessModifier.Private)]
	public float moveToDir;

	// Token: 0x04002039 RID: 8249
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 focusPos;

	// Token: 0x0400203A RID: 8250
	[PublicizedFrom(EAccessModifier.Private)]
	public int focusTicks;

	// Token: 0x0400203B RID: 8251
	[PublicizedFrom(EAccessModifier.Private)]
	public int obstacleCheckTickDelay;

	// Token: 0x0400203C RID: 8252
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasNextPos;

	// Token: 0x0400203D RID: 8253
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 nextMoveToPos;

	// Token: 0x0400203E RID: 8254
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 tempMoveToPos;

	// Token: 0x0400203F RID: 8255
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTempMove;

	// Token: 0x04002040 RID: 8256
	[PublicizedFrom(EAccessModifier.Private)]
	public float blockedDistSq;

	// Token: 0x04002041 RID: 8257
	[PublicizedFrom(EAccessModifier.Private)]
	public float blockedEntityDistSq;

	// Token: 0x04002042 RID: 8258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDigging;

	// Token: 0x04002043 RID: 8259
	[PublicizedFrom(EAccessModifier.Private)]
	public float moveSpeed;

	// Token: 0x04002044 RID: 8260
	[PublicizedFrom(EAccessModifier.Private)]
	public int expiryTicks;

	// Token: 0x04002045 RID: 8261
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClimb;

	// Token: 0x04002046 RID: 8262
	[PublicizedFrom(EAccessModifier.Private)]
	public float jumpYaw;

	// Token: 0x04002047 RID: 8263
	[PublicizedFrom(EAccessModifier.Private)]
	public int swimStrokeDelayTicks;

	// Token: 0x04002048 RID: 8264
	[PublicizedFrom(EAccessModifier.Private)]
	public float ccRadius;

	// Token: 0x04002049 RID: 8265
	[PublicizedFrom(EAccessModifier.Private)]
	public float ccHeight;

	// Token: 0x0400204A RID: 8266
	public static bool AllowZombieDigging = true;

	// Token: 0x0400204B RID: 8267
	[PublicizedFrom(EAccessModifier.Private)]
	public static float[] checkEdgeXs = new float[] { 0f, -0.25f, 0.25f };

	// Token: 0x0400204C RID: 8268
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDigMovedDist = 0.5f;

	// Token: 0x0400204D RID: 8269
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 digStartPos;

	// Token: 0x0400204E RID: 8270
	[PublicizedFrom(EAccessModifier.Private)]
	public float digForTicks;

	// Token: 0x0400204F RID: 8271
	[PublicizedFrom(EAccessModifier.Private)]
	public float digTicks;

	// Token: 0x04002050 RID: 8272
	[PublicizedFrom(EAccessModifier.Private)]
	public float digActionTicks;

	// Token: 0x04002051 RID: 8273
	[PublicizedFrom(EAccessModifier.Private)]
	public bool digAttacked;

	// Token: 0x04002052 RID: 8274
	[PublicizedFrom(EAccessModifier.Private)]
	public float digForwardCount;

	// Token: 0x04002053 RID: 8275
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDestroyRefreshAfter = 25;

	// Token: 0x04002054 RID: 8276
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDestroyOtherAIDist = 20f;

	// Token: 0x04002055 RID: 8277
	[PublicizedFrom(EAccessModifier.Private)]
	public static EntityMoveHelper.DestroyData[] destroyData = new EntityMoveHelper.DestroyData[]
	{
		new EntityMoveHelper.DestroyData(-1, 1, 1, 0),
		new EntityMoveHelper.DestroyData(1, 1, 0, -1),
		new EntityMoveHelper.DestroyData(1, -1, -1, 0),
		new EntityMoveHelper.DestroyData(-1, -1, 0, 1),
		new EntityMoveHelper.DestroyData(-1, 1, 1, 0),
		new EntityMoveHelper.DestroyData(1, 1, 0, -1),
		new EntityMoveHelper.DestroyData(1, -1, -1, 0)
	};

	// Token: 0x04002056 RID: 8278
	[PublicizedFrom(EAccessModifier.Private)]
	public int destroyRefreshTicks;

	// Token: 0x04002057 RID: 8279
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 destroyPosition;

	// Token: 0x04002058 RID: 8280
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> entityTempList = new List<Entity>();

	// Token: 0x04002059 RID: 8281
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] blockOpenOffsets = new int[] { -1, 0, 1, 0, 0, 1, 0, -1 };

	// Token: 0x02000511 RID: 1297
	[PublicizedFrom(EAccessModifier.Private)]
	public struct DestroyData
	{
		// Token: 0x06002A50 RID: 10832 RVA: 0x0010BAB5 File Offset: 0x00109CB5
		public DestroyData(int _offsetX, int _offsetZ, int _stepX, int _stepZ)
		{
			this.offsetX = _offsetX;
			this.offsetZ = _offsetZ;
			this.stepX = _stepX;
			this.stepZ = _stepZ;
		}

		// Token: 0x0400205A RID: 8282
		public int offsetX;

		// Token: 0x0400205B RID: 8283
		public int offsetZ;

		// Token: 0x0400205C RID: 8284
		public int stepX;

		// Token: 0x0400205D RID: 8285
		public int stepZ;
	}
}
