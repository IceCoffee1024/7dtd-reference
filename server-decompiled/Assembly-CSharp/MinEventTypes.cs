using System;

// Token: 0x02000722 RID: 1826
public enum MinEventTypes
{
	// Token: 0x04002CB7 RID: 11447
	onSelfBuffStart,
	// Token: 0x04002CB8 RID: 11448
	onSelfBuffUpdate,
	// Token: 0x04002CB9 RID: 11449
	onSelfBuffFinish,
	// Token: 0x04002CBA RID: 11450
	onSelfBuffRemove,
	// Token: 0x04002CBB RID: 11451
	onSelfBuffStack,
	// Token: 0x04002CBC RID: 11452
	onSelfProgressionUpdate,
	// Token: 0x04002CBD RID: 11453
	onSelfChallengeCompleteUpdate,
	// Token: 0x04002CBE RID: 11454
	onOtherDamagedSelf,
	// Token: 0x04002CBF RID: 11455
	onOtherAttackedSelf,
	// Token: 0x04002CC0 RID: 11456
	onOtherHealedSelf,
	// Token: 0x04002CC1 RID: 11457
	onSelfDamagedOther,
	// Token: 0x04002CC2 RID: 11458
	onSelfAttackedOther,
	// Token: 0x04002CC3 RID: 11459
	onSelfVehicleAttackedOther,
	// Token: 0x04002CC4 RID: 11460
	onSelfHealedOther,
	// Token: 0x04002CC5 RID: 11461
	onSelfExplosionAttackedOther,
	// Token: 0x04002CC6 RID: 11462
	onSelfExplosionDamagedOther,
	// Token: 0x04002CC7 RID: 11463
	onSelfExplosionMissEntity,
	// Token: 0x04002CC8 RID: 11464
	onSelfDamagedSelf,
	// Token: 0x04002CC9 RID: 11465
	onSelfHealedSelf,
	// Token: 0x04002CCA RID: 11466
	onSelfKilledOther,
	// Token: 0x04002CCB RID: 11467
	onOtherKilledSelf,
	// Token: 0x04002CCC RID: 11468
	onBlockKilledSelf,
	// Token: 0x04002CCD RID: 11469
	onSelfKilledSelf,
	// Token: 0x04002CCE RID: 11470
	onSelfDied,
	// Token: 0x04002CCF RID: 11471
	onSelfPrimaryActionStart,
	// Token: 0x04002CD0 RID: 11472
	onSelfPrimaryActionRayHit,
	// Token: 0x04002CD1 RID: 11473
	onSelfPrimaryActionRayMiss,
	// Token: 0x04002CD2 RID: 11474
	onSelfPrimaryActionGrazeHit,
	// Token: 0x04002CD3 RID: 11475
	onSelfPrimaryActionGrazeMiss,
	// Token: 0x04002CD4 RID: 11476
	onSelfPrimaryActionEnd,
	// Token: 0x04002CD5 RID: 11477
	onSelfPrimaryActionUpdate,
	// Token: 0x04002CD6 RID: 11478
	onSelfPrimaryActionMissEntity,
	// Token: 0x04002CD7 RID: 11479
	onSelfSecondaryActionStart,
	// Token: 0x04002CD8 RID: 11480
	onSelfSecondaryActionRayHit,
	// Token: 0x04002CD9 RID: 11481
	onSelfSecondaryActionRayMiss,
	// Token: 0x04002CDA RID: 11482
	onSelfSecondaryActionGrazeHit,
	// Token: 0x04002CDB RID: 11483
	onSelfSecondaryActionGrazeMiss,
	// Token: 0x04002CDC RID: 11484
	onSelfSecondaryActionEnd,
	// Token: 0x04002CDD RID: 11485
	onSelfSecondaryActionUpdate,
	// Token: 0x04002CDE RID: 11486
	onSelfSecondaryActionMissEntity,
	// Token: 0x04002CDF RID: 11487
	onSelfAction2Start,
	// Token: 0x04002CE0 RID: 11488
	onSelfAction2Update,
	// Token: 0x04002CE1 RID: 11489
	onSelfAction2End,
	// Token: 0x04002CE2 RID: 11490
	onSelfRepairBlock,
	// Token: 0x04002CE3 RID: 11491
	onSelfPlaceBlock,
	// Token: 0x04002CE4 RID: 11492
	onSelfUpgradedBlock,
	// Token: 0x04002CE5 RID: 11493
	onSelfDamagedBlock,
	// Token: 0x04002CE6 RID: 11494
	onSelfDestroyedBlock,
	// Token: 0x04002CE7 RID: 11495
	onSelfHarvestBlock,
	// Token: 0x04002CE8 RID: 11496
	onSelfHarvestOther,
	// Token: 0x04002CE9 RID: 11497
	onOtherEnteredRange,
	// Token: 0x04002CEA RID: 11498
	onOtherLeftRange,
	// Token: 0x04002CEB RID: 11499
	onSelfRangedBurstShotStart,
	// Token: 0x04002CEC RID: 11500
	onSelfRangedBurstShotEnd,
	// Token: 0x04002CED RID: 11501
	onSelfEquipStart,
	// Token: 0x04002CEE RID: 11502
	onSelfEquipChanged,
	// Token: 0x04002CEF RID: 11503
	onSelfEquipUpdate,
	// Token: 0x04002CF0 RID: 11504
	onSelfEquipStop,
	// Token: 0x04002CF1 RID: 11505
	onReloadStart,
	// Token: 0x04002CF2 RID: 11506
	onReloadUpdate,
	// Token: 0x04002CF3 RID: 11507
	onReloadStop,
	// Token: 0x04002CF4 RID: 11508
	onSelfFirstSpawn,
	// Token: 0x04002CF5 RID: 11509
	onSelfRespawn,
	// Token: 0x04002CF6 RID: 11510
	onSelfLeaveGame,
	// Token: 0x04002CF7 RID: 11511
	onSelfEnteredGame,
	// Token: 0x04002CF8 RID: 11512
	onSelfTeleported,
	// Token: 0x04002CF9 RID: 11513
	onSelfJump,
	// Token: 0x04002CFA RID: 11514
	onSelfLandJump,
	// Token: 0x04002CFB RID: 11515
	onSelfRun,
	// Token: 0x04002CFC RID: 11516
	onSelfWalk,
	// Token: 0x04002CFD RID: 11517
	onSelfCrouch,
	// Token: 0x04002CFE RID: 11518
	onSelfStand,
	// Token: 0x04002CFF RID: 11519
	onSelfAimingGunStart,
	// Token: 0x04002D00 RID: 11520
	onSelfAimingGunStop,
	// Token: 0x04002D01 RID: 11521
	onSelfCrouchRun,
	// Token: 0x04002D02 RID: 11522
	onSelfCrouchWalk,
	// Token: 0x04002D03 RID: 11523
	onSelfSwimStart,
	// Token: 0x04002D04 RID: 11524
	onSelfSwimStop,
	// Token: 0x04002D05 RID: 11525
	onSelfSwimRun,
	// Token: 0x04002D06 RID: 11526
	onSelfSwimIdle,
	// Token: 0x04002D07 RID: 11527
	onSelfWaterSurface,
	// Token: 0x04002D08 RID: 11528
	onSelfWaterSubmerge,
	// Token: 0x04002D09 RID: 11529
	onSelfHoldingItemThrown,
	// Token: 0x04002D0A RID: 11530
	onSelfHoldingItemCreated,
	// Token: 0x04002D0B RID: 11531
	onSelfItemCrafted,
	// Token: 0x04002D0C RID: 11532
	onSelfItemRepaired,
	// Token: 0x04002D0D RID: 11533
	onSelfItemLooted,
	// Token: 0x04002D0E RID: 11534
	onSelfItemLost,
	// Token: 0x04002D0F RID: 11535
	onSelfItemGained,
	// Token: 0x04002D10 RID: 11536
	onSelfItemSold,
	// Token: 0x04002D11 RID: 11537
	onSelfItemBought,
	// Token: 0x04002D12 RID: 11538
	onSelfItemActivate,
	// Token: 0x04002D13 RID: 11539
	onSelfItemDeactivate,
	// Token: 0x04002D14 RID: 11540
	onSelfItemJammedUse,
	// Token: 0x04002D15 RID: 11541
	onSelfChangedView,
	// Token: 0x04002D16 RID: 11542
	onSelfFallImpact,
	// Token: 0x04002D17 RID: 11543
	onProjectilePreImpact,
	// Token: 0x04002D18 RID: 11544
	onProjectileImpact,
	// Token: 0x04002D19 RID: 11545
	onPerkLevelChanged,
	// Token: 0x04002D1A RID: 11546
	onSelfEnteredBiome,
	// Token: 0x04002D1B RID: 11547
	onSelfLootContainer,
	// Token: 0x04002D1C RID: 11548
	onSelfOpenLootContainer,
	// Token: 0x04002D1D RID: 11549
	onSelfCloseLootContainer,
	// Token: 0x04002D1E RID: 11550
	onSelfBiomeLootStageMaxEntered,
	// Token: 0x04002D1F RID: 11551
	onSelfBiomeLootStageMaxExited,
	// Token: 0x04002D20 RID: 11552
	onDismember,
	// Token: 0x04002D21 RID: 11553
	onCombatEntered,
	// Token: 0x04002D22 RID: 11554
	onTrackedEntityLost,
	// Token: 0x04002D23 RID: 11555
	onTreasureRadiusEntered,
	// Token: 0x04002D24 RID: 11556
	onTreasureRadiusExited,
	// Token: 0x04002D25 RID: 11557
	onTreasureRadiusCompleted,
	// Token: 0x04002D26 RID: 11558
	onSelfPickupOther,
	// Token: 0x04002D27 RID: 11559
	onSelfDroppedOther,
	// Token: 0x04002D28 RID: 11560
	onSelfPickedUpByOther,
	// Token: 0x04002D29 RID: 11561
	COUNT
}
