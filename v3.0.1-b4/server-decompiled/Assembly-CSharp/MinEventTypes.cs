using System;

// Token: 0x02000718 RID: 1816
public enum MinEventTypes
{
	// Token: 0x04002C42 RID: 11330
	onSelfBuffStart,
	// Token: 0x04002C43 RID: 11331
	onSelfBuffUpdate,
	// Token: 0x04002C44 RID: 11332
	onSelfBuffFinish,
	// Token: 0x04002C45 RID: 11333
	onSelfBuffRemove,
	// Token: 0x04002C46 RID: 11334
	onSelfBuffStack,
	// Token: 0x04002C47 RID: 11335
	onSelfProgressionUpdate,
	// Token: 0x04002C48 RID: 11336
	onSelfChallengeCompleteUpdate,
	// Token: 0x04002C49 RID: 11337
	onOtherDamagedSelf,
	// Token: 0x04002C4A RID: 11338
	onOtherAttackedSelf,
	// Token: 0x04002C4B RID: 11339
	onOtherHealedSelf,
	// Token: 0x04002C4C RID: 11340
	onSelfDamagedOther,
	// Token: 0x04002C4D RID: 11341
	onSelfAttackedOther,
	// Token: 0x04002C4E RID: 11342
	onSelfVehicleAttackedOther,
	// Token: 0x04002C4F RID: 11343
	onSelfHealedOther,
	// Token: 0x04002C50 RID: 11344
	onSelfExplosionAttackedOther,
	// Token: 0x04002C51 RID: 11345
	onSelfExplosionDamagedOther,
	// Token: 0x04002C52 RID: 11346
	onSelfExplosionMissEntity,
	// Token: 0x04002C53 RID: 11347
	onSelfDamagedSelf,
	// Token: 0x04002C54 RID: 11348
	onSelfHealedSelf,
	// Token: 0x04002C55 RID: 11349
	onSelfKilledOther,
	// Token: 0x04002C56 RID: 11350
	onOtherKilledSelf,
	// Token: 0x04002C57 RID: 11351
	onBlockKilledSelf,
	// Token: 0x04002C58 RID: 11352
	onSelfKilledSelf,
	// Token: 0x04002C59 RID: 11353
	onSelfDied,
	// Token: 0x04002C5A RID: 11354
	onSelfPrimaryActionStart,
	// Token: 0x04002C5B RID: 11355
	onSelfPrimaryActionRayHit,
	// Token: 0x04002C5C RID: 11356
	onSelfPrimaryActionRayMiss,
	// Token: 0x04002C5D RID: 11357
	onSelfPrimaryActionGrazeHit,
	// Token: 0x04002C5E RID: 11358
	onSelfPrimaryActionGrazeMiss,
	// Token: 0x04002C5F RID: 11359
	onSelfPrimaryActionEnd,
	// Token: 0x04002C60 RID: 11360
	onSelfPrimaryActionUpdate,
	// Token: 0x04002C61 RID: 11361
	onSelfPrimaryActionMissEntity,
	// Token: 0x04002C62 RID: 11362
	onSelfSecondaryActionStart,
	// Token: 0x04002C63 RID: 11363
	onSelfSecondaryActionRayHit,
	// Token: 0x04002C64 RID: 11364
	onSelfSecondaryActionRayMiss,
	// Token: 0x04002C65 RID: 11365
	onSelfSecondaryActionGrazeHit,
	// Token: 0x04002C66 RID: 11366
	onSelfSecondaryActionGrazeMiss,
	// Token: 0x04002C67 RID: 11367
	onSelfSecondaryActionEnd,
	// Token: 0x04002C68 RID: 11368
	onSelfSecondaryActionUpdate,
	// Token: 0x04002C69 RID: 11369
	onSelfSecondaryActionMissEntity,
	// Token: 0x04002C6A RID: 11370
	onSelfAction2Start,
	// Token: 0x04002C6B RID: 11371
	onSelfAction2Update,
	// Token: 0x04002C6C RID: 11372
	onSelfAction2End,
	// Token: 0x04002C6D RID: 11373
	onSelfRepairBlock,
	// Token: 0x04002C6E RID: 11374
	onSelfPlaceBlock,
	// Token: 0x04002C6F RID: 11375
	onSelfUpgradedBlock,
	// Token: 0x04002C70 RID: 11376
	onSelfDamagedBlock,
	// Token: 0x04002C71 RID: 11377
	onSelfDestroyedBlock,
	// Token: 0x04002C72 RID: 11378
	onSelfHarvestBlock,
	// Token: 0x04002C73 RID: 11379
	onSelfHarvestOther,
	// Token: 0x04002C74 RID: 11380
	onOtherEnteredRange,
	// Token: 0x04002C75 RID: 11381
	onOtherLeftRange,
	// Token: 0x04002C76 RID: 11382
	onSelfRangedBurstShotStart,
	// Token: 0x04002C77 RID: 11383
	onSelfRangedBurstShotEnd,
	// Token: 0x04002C78 RID: 11384
	onSelfEquipStart,
	// Token: 0x04002C79 RID: 11385
	onSelfEquipChanged,
	// Token: 0x04002C7A RID: 11386
	onSelfEquipUpdate,
	// Token: 0x04002C7B RID: 11387
	onSelfEquipStop,
	// Token: 0x04002C7C RID: 11388
	onReloadStart,
	// Token: 0x04002C7D RID: 11389
	onReloadUpdate,
	// Token: 0x04002C7E RID: 11390
	onReloadStop,
	// Token: 0x04002C7F RID: 11391
	onSelfFirstSpawn,
	// Token: 0x04002C80 RID: 11392
	onSelfRespawn,
	// Token: 0x04002C81 RID: 11393
	onSelfLeaveGame,
	// Token: 0x04002C82 RID: 11394
	onSelfEnteredGame,
	// Token: 0x04002C83 RID: 11395
	onSelfTeleported,
	// Token: 0x04002C84 RID: 11396
	onSelfJump,
	// Token: 0x04002C85 RID: 11397
	onSelfLandJump,
	// Token: 0x04002C86 RID: 11398
	onSelfRun,
	// Token: 0x04002C87 RID: 11399
	onSelfWalk,
	// Token: 0x04002C88 RID: 11400
	onSelfCrouch,
	// Token: 0x04002C89 RID: 11401
	onSelfStand,
	// Token: 0x04002C8A RID: 11402
	onSelfAimingGunStart,
	// Token: 0x04002C8B RID: 11403
	onSelfAimingGunStop,
	// Token: 0x04002C8C RID: 11404
	onSelfCrouchRun,
	// Token: 0x04002C8D RID: 11405
	onSelfCrouchWalk,
	// Token: 0x04002C8E RID: 11406
	onSelfSwimStart,
	// Token: 0x04002C8F RID: 11407
	onSelfSwimStop,
	// Token: 0x04002C90 RID: 11408
	onSelfSwimRun,
	// Token: 0x04002C91 RID: 11409
	onSelfSwimIdle,
	// Token: 0x04002C92 RID: 11410
	onSelfWaterSurface,
	// Token: 0x04002C93 RID: 11411
	onSelfWaterSubmerge,
	// Token: 0x04002C94 RID: 11412
	onSelfHoldingItemThrown,
	// Token: 0x04002C95 RID: 11413
	onSelfHoldingItemCreated,
	// Token: 0x04002C96 RID: 11414
	onSelfItemCrafted,
	// Token: 0x04002C97 RID: 11415
	onSelfItemRepaired,
	// Token: 0x04002C98 RID: 11416
	onSelfItemLooted,
	// Token: 0x04002C99 RID: 11417
	onSelfItemLost,
	// Token: 0x04002C9A RID: 11418
	onSelfItemGained,
	// Token: 0x04002C9B RID: 11419
	onSelfItemSold,
	// Token: 0x04002C9C RID: 11420
	onSelfItemBought,
	// Token: 0x04002C9D RID: 11421
	onSelfItemActivate,
	// Token: 0x04002C9E RID: 11422
	onSelfItemDeactivate,
	// Token: 0x04002C9F RID: 11423
	onSelfItemJammedUse,
	// Token: 0x04002CA0 RID: 11424
	onSelfChangedView,
	// Token: 0x04002CA1 RID: 11425
	onSelfFallImpact,
	// Token: 0x04002CA2 RID: 11426
	onProjectilePreImpact,
	// Token: 0x04002CA3 RID: 11427
	onProjectileImpact,
	// Token: 0x04002CA4 RID: 11428
	onPerkLevelChanged,
	// Token: 0x04002CA5 RID: 11429
	onSelfEnteredBiome,
	// Token: 0x04002CA6 RID: 11430
	onSelfLootContainer,
	// Token: 0x04002CA7 RID: 11431
	onSelfOpenLootContainer,
	// Token: 0x04002CA8 RID: 11432
	onSelfCloseLootContainer,
	// Token: 0x04002CA9 RID: 11433
	onSelfBiomeLootStageMaxEntered,
	// Token: 0x04002CAA RID: 11434
	onSelfBiomeLootStageMaxExited,
	// Token: 0x04002CAB RID: 11435
	onDismember,
	// Token: 0x04002CAC RID: 11436
	onCombatEntered,
	// Token: 0x04002CAD RID: 11437
	onTrackedEntityLost,
	// Token: 0x04002CAE RID: 11438
	onTreasureRadiusEntered,
	// Token: 0x04002CAF RID: 11439
	onTreasureRadiusExited,
	// Token: 0x04002CB0 RID: 11440
	onTreasureRadiusCompleted,
	// Token: 0x04002CB1 RID: 11441
	COUNT
}
