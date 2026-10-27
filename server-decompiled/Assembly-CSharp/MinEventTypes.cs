using System;

// Token: 0x0200072C RID: 1836
public enum MinEventTypes
{
	// Token: 0x04002CCC RID: 11468
	onSelfBuffStart,
	// Token: 0x04002CCD RID: 11469
	onSelfBuffUpdate,
	// Token: 0x04002CCE RID: 11470
	onSelfBuffFinish,
	// Token: 0x04002CCF RID: 11471
	onSelfBuffRemove,
	// Token: 0x04002CD0 RID: 11472
	onSelfBuffStack,
	// Token: 0x04002CD1 RID: 11473
	onSelfProgressionUpdate,
	// Token: 0x04002CD2 RID: 11474
	onSelfChallengeCompleteUpdate,
	// Token: 0x04002CD3 RID: 11475
	onOtherDamagedSelf,
	// Token: 0x04002CD4 RID: 11476
	onOtherAttackedSelf,
	// Token: 0x04002CD5 RID: 11477
	onOtherHealedSelf,
	// Token: 0x04002CD6 RID: 11478
	onSelfDamagedOther,
	// Token: 0x04002CD7 RID: 11479
	onSelfAttackedOther,
	// Token: 0x04002CD8 RID: 11480
	onSelfVehicleAttackedOther,
	// Token: 0x04002CD9 RID: 11481
	onSelfHealedOther,
	// Token: 0x04002CDA RID: 11482
	onSelfExplosionAttackedOther,
	// Token: 0x04002CDB RID: 11483
	onSelfExplosionDamagedOther,
	// Token: 0x04002CDC RID: 11484
	onSelfExplosionMissEntity,
	// Token: 0x04002CDD RID: 11485
	onSelfDamagedSelf,
	// Token: 0x04002CDE RID: 11486
	onSelfHealedSelf,
	// Token: 0x04002CDF RID: 11487
	onSelfKilledOther,
	// Token: 0x04002CE0 RID: 11488
	onOtherKilledSelf,
	// Token: 0x04002CE1 RID: 11489
	onBlockKilledSelf,
	// Token: 0x04002CE2 RID: 11490
	onSelfKilledSelf,
	// Token: 0x04002CE3 RID: 11491
	onSelfDied,
	// Token: 0x04002CE4 RID: 11492
	onSelfPrimaryActionStart,
	// Token: 0x04002CE5 RID: 11493
	onSelfPrimaryActionRayHit,
	// Token: 0x04002CE6 RID: 11494
	onSelfPrimaryActionRayMiss,
	// Token: 0x04002CE7 RID: 11495
	onSelfPrimaryActionGrazeHit,
	// Token: 0x04002CE8 RID: 11496
	onSelfPrimaryActionGrazeMiss,
	// Token: 0x04002CE9 RID: 11497
	onSelfPrimaryActionEnd,
	// Token: 0x04002CEA RID: 11498
	onSelfPrimaryActionUpdate,
	// Token: 0x04002CEB RID: 11499
	onSelfPrimaryActionMissEntity,
	// Token: 0x04002CEC RID: 11500
	onSelfSecondaryActionStart,
	// Token: 0x04002CED RID: 11501
	onSelfSecondaryActionRayHit,
	// Token: 0x04002CEE RID: 11502
	onSelfSecondaryActionRayMiss,
	// Token: 0x04002CEF RID: 11503
	onSelfSecondaryActionGrazeHit,
	// Token: 0x04002CF0 RID: 11504
	onSelfSecondaryActionGrazeMiss,
	// Token: 0x04002CF1 RID: 11505
	onSelfSecondaryActionEnd,
	// Token: 0x04002CF2 RID: 11506
	onSelfSecondaryActionUpdate,
	// Token: 0x04002CF3 RID: 11507
	onSelfSecondaryActionMissEntity,
	// Token: 0x04002CF4 RID: 11508
	onSelfAction2Start,
	// Token: 0x04002CF5 RID: 11509
	onSelfAction2Update,
	// Token: 0x04002CF6 RID: 11510
	onSelfAction2End,
	// Token: 0x04002CF7 RID: 11511
	onSelfRepairBlock,
	// Token: 0x04002CF8 RID: 11512
	onSelfPlaceBlock,
	// Token: 0x04002CF9 RID: 11513
	onSelfUpgradedBlock,
	// Token: 0x04002CFA RID: 11514
	onSelfDamagedBlock,
	// Token: 0x04002CFB RID: 11515
	onSelfDestroyedBlock,
	// Token: 0x04002CFC RID: 11516
	onSelfHarvestBlock,
	// Token: 0x04002CFD RID: 11517
	onSelfHarvestOther,
	// Token: 0x04002CFE RID: 11518
	onOtherEnteredRange,
	// Token: 0x04002CFF RID: 11519
	onOtherLeftRange,
	// Token: 0x04002D00 RID: 11520
	onSelfRangedBurstShotStart,
	// Token: 0x04002D01 RID: 11521
	onSelfRangedBurstShotEnd,
	// Token: 0x04002D02 RID: 11522
	onSelfEquipStart,
	// Token: 0x04002D03 RID: 11523
	onSelfEquipChanged,
	// Token: 0x04002D04 RID: 11524
	onSelfEquipUpdate,
	// Token: 0x04002D05 RID: 11525
	onSelfEquipStop,
	// Token: 0x04002D06 RID: 11526
	onReloadStart,
	// Token: 0x04002D07 RID: 11527
	onReloadUpdate,
	// Token: 0x04002D08 RID: 11528
	onReloadStop,
	// Token: 0x04002D09 RID: 11529
	onSelfFirstSpawn,
	// Token: 0x04002D0A RID: 11530
	onSelfRespawn,
	// Token: 0x04002D0B RID: 11531
	onSelfLeaveGame,
	// Token: 0x04002D0C RID: 11532
	onSelfEnteredGame,
	// Token: 0x04002D0D RID: 11533
	onSelfTeleported,
	// Token: 0x04002D0E RID: 11534
	onSelfJump,
	// Token: 0x04002D0F RID: 11535
	onSelfLandJump,
	// Token: 0x04002D10 RID: 11536
	onSelfRun,
	// Token: 0x04002D11 RID: 11537
	onSelfWalk,
	// Token: 0x04002D12 RID: 11538
	onSelfCrouch,
	// Token: 0x04002D13 RID: 11539
	onSelfStand,
	// Token: 0x04002D14 RID: 11540
	onSelfAimingGunStart,
	// Token: 0x04002D15 RID: 11541
	onSelfAimingGunStop,
	// Token: 0x04002D16 RID: 11542
	onSelfCrouchRun,
	// Token: 0x04002D17 RID: 11543
	onSelfCrouchWalk,
	// Token: 0x04002D18 RID: 11544
	onSelfSwimStart,
	// Token: 0x04002D19 RID: 11545
	onSelfSwimStop,
	// Token: 0x04002D1A RID: 11546
	onSelfSwimRun,
	// Token: 0x04002D1B RID: 11547
	onSelfSwimIdle,
	// Token: 0x04002D1C RID: 11548
	onSelfWaterSurface,
	// Token: 0x04002D1D RID: 11549
	onSelfWaterSubmerge,
	// Token: 0x04002D1E RID: 11550
	onSelfHoldingItemThrown,
	// Token: 0x04002D1F RID: 11551
	onSelfHoldingItemCreated,
	// Token: 0x04002D20 RID: 11552
	onSelfItemCrafted,
	// Token: 0x04002D21 RID: 11553
	onSelfItemRepaired,
	// Token: 0x04002D22 RID: 11554
	onSelfItemLooted,
	// Token: 0x04002D23 RID: 11555
	onSelfItemLost,
	// Token: 0x04002D24 RID: 11556
	onSelfItemGained,
	// Token: 0x04002D25 RID: 11557
	onSelfItemSold,
	// Token: 0x04002D26 RID: 11558
	onSelfItemBought,
	// Token: 0x04002D27 RID: 11559
	onSelfItemActivate,
	// Token: 0x04002D28 RID: 11560
	onSelfItemDeactivate,
	// Token: 0x04002D29 RID: 11561
	onSelfItemJammedUse,
	// Token: 0x04002D2A RID: 11562
	onSelfChangedView,
	// Token: 0x04002D2B RID: 11563
	onSelfFallImpact,
	// Token: 0x04002D2C RID: 11564
	onProjectilePreImpact,
	// Token: 0x04002D2D RID: 11565
	onProjectileImpact,
	// Token: 0x04002D2E RID: 11566
	onPerkLevelChanged,
	// Token: 0x04002D2F RID: 11567
	onSelfEnteredBiome,
	// Token: 0x04002D30 RID: 11568
	onSelfLootContainer,
	// Token: 0x04002D31 RID: 11569
	onSelfOpenLootContainer,
	// Token: 0x04002D32 RID: 11570
	onSelfCloseLootContainer,
	// Token: 0x04002D33 RID: 11571
	onSelfBiomeLootStageMaxEntered,
	// Token: 0x04002D34 RID: 11572
	onSelfBiomeLootStageMaxExited,
	// Token: 0x04002D35 RID: 11573
	onDismember,
	// Token: 0x04002D36 RID: 11574
	onCombatEntered,
	// Token: 0x04002D37 RID: 11575
	onTrackedEntityLost,
	// Token: 0x04002D38 RID: 11576
	onTreasureRadiusEntered,
	// Token: 0x04002D39 RID: 11577
	onTreasureRadiusExited,
	// Token: 0x04002D3A RID: 11578
	onTreasureRadiusCompleted,
	// Token: 0x04002D3B RID: 11579
	onSelfPickupOther,
	// Token: 0x04002D3C RID: 11580
	onSelfDroppedOther,
	// Token: 0x04002D3D RID: 11581
	onSelfPickedUpByOther,
	// Token: 0x04002D3E RID: 11582
	COUNT
}
