using System;

// Token: 0x02000724 RID: 1828
public enum MinEventTypes
{
	// Token: 0x04002CB0 RID: 11440
	onSelfBuffStart,
	// Token: 0x04002CB1 RID: 11441
	onSelfBuffUpdate,
	// Token: 0x04002CB2 RID: 11442
	onSelfBuffFinish,
	// Token: 0x04002CB3 RID: 11443
	onSelfBuffRemove,
	// Token: 0x04002CB4 RID: 11444
	onSelfBuffStack,
	// Token: 0x04002CB5 RID: 11445
	onSelfProgressionUpdate,
	// Token: 0x04002CB6 RID: 11446
	onSelfChallengeCompleteUpdate,
	// Token: 0x04002CB7 RID: 11447
	onOtherDamagedSelf,
	// Token: 0x04002CB8 RID: 11448
	onOtherAttackedSelf,
	// Token: 0x04002CB9 RID: 11449
	onOtherHealedSelf,
	// Token: 0x04002CBA RID: 11450
	onSelfDamagedOther,
	// Token: 0x04002CBB RID: 11451
	onSelfAttackedOther,
	// Token: 0x04002CBC RID: 11452
	onSelfVehicleAttackedOther,
	// Token: 0x04002CBD RID: 11453
	onSelfHealedOther,
	// Token: 0x04002CBE RID: 11454
	onSelfExplosionAttackedOther,
	// Token: 0x04002CBF RID: 11455
	onSelfExplosionDamagedOther,
	// Token: 0x04002CC0 RID: 11456
	onSelfExplosionMissEntity,
	// Token: 0x04002CC1 RID: 11457
	onSelfDamagedSelf,
	// Token: 0x04002CC2 RID: 11458
	onSelfHealedSelf,
	// Token: 0x04002CC3 RID: 11459
	onSelfKilledOther,
	// Token: 0x04002CC4 RID: 11460
	onOtherKilledSelf,
	// Token: 0x04002CC5 RID: 11461
	onBlockKilledSelf,
	// Token: 0x04002CC6 RID: 11462
	onSelfKilledSelf,
	// Token: 0x04002CC7 RID: 11463
	onSelfDied,
	// Token: 0x04002CC8 RID: 11464
	onSelfPrimaryActionStart,
	// Token: 0x04002CC9 RID: 11465
	onSelfPrimaryActionRayHit,
	// Token: 0x04002CCA RID: 11466
	onSelfPrimaryActionRayMiss,
	// Token: 0x04002CCB RID: 11467
	onSelfPrimaryActionGrazeHit,
	// Token: 0x04002CCC RID: 11468
	onSelfPrimaryActionGrazeMiss,
	// Token: 0x04002CCD RID: 11469
	onSelfPrimaryActionEnd,
	// Token: 0x04002CCE RID: 11470
	onSelfPrimaryActionUpdate,
	// Token: 0x04002CCF RID: 11471
	onSelfPrimaryActionMissEntity,
	// Token: 0x04002CD0 RID: 11472
	onSelfSecondaryActionStart,
	// Token: 0x04002CD1 RID: 11473
	onSelfSecondaryActionRayHit,
	// Token: 0x04002CD2 RID: 11474
	onSelfSecondaryActionRayMiss,
	// Token: 0x04002CD3 RID: 11475
	onSelfSecondaryActionGrazeHit,
	// Token: 0x04002CD4 RID: 11476
	onSelfSecondaryActionGrazeMiss,
	// Token: 0x04002CD5 RID: 11477
	onSelfSecondaryActionEnd,
	// Token: 0x04002CD6 RID: 11478
	onSelfSecondaryActionUpdate,
	// Token: 0x04002CD7 RID: 11479
	onSelfSecondaryActionMissEntity,
	// Token: 0x04002CD8 RID: 11480
	onSelfAction2Start,
	// Token: 0x04002CD9 RID: 11481
	onSelfAction2Update,
	// Token: 0x04002CDA RID: 11482
	onSelfAction2End,
	// Token: 0x04002CDB RID: 11483
	onSelfRepairBlock,
	// Token: 0x04002CDC RID: 11484
	onSelfPlaceBlock,
	// Token: 0x04002CDD RID: 11485
	onSelfUpgradedBlock,
	// Token: 0x04002CDE RID: 11486
	onSelfDamagedBlock,
	// Token: 0x04002CDF RID: 11487
	onSelfDestroyedBlock,
	// Token: 0x04002CE0 RID: 11488
	onSelfHarvestBlock,
	// Token: 0x04002CE1 RID: 11489
	onSelfHarvestOther,
	// Token: 0x04002CE2 RID: 11490
	onOtherEnteredRange,
	// Token: 0x04002CE3 RID: 11491
	onOtherLeftRange,
	// Token: 0x04002CE4 RID: 11492
	onSelfRangedBurstShotStart,
	// Token: 0x04002CE5 RID: 11493
	onSelfRangedBurstShotEnd,
	// Token: 0x04002CE6 RID: 11494
	onSelfEquipStart,
	// Token: 0x04002CE7 RID: 11495
	onSelfEquipChanged,
	// Token: 0x04002CE8 RID: 11496
	onSelfEquipUpdate,
	// Token: 0x04002CE9 RID: 11497
	onSelfEquipStop,
	// Token: 0x04002CEA RID: 11498
	onReloadStart,
	// Token: 0x04002CEB RID: 11499
	onReloadUpdate,
	// Token: 0x04002CEC RID: 11500
	onReloadStop,
	// Token: 0x04002CED RID: 11501
	onSelfFirstSpawn,
	// Token: 0x04002CEE RID: 11502
	onSelfRespawn,
	// Token: 0x04002CEF RID: 11503
	onSelfLeaveGame,
	// Token: 0x04002CF0 RID: 11504
	onSelfEnteredGame,
	// Token: 0x04002CF1 RID: 11505
	onSelfTeleported,
	// Token: 0x04002CF2 RID: 11506
	onSelfJump,
	// Token: 0x04002CF3 RID: 11507
	onSelfLandJump,
	// Token: 0x04002CF4 RID: 11508
	onSelfRun,
	// Token: 0x04002CF5 RID: 11509
	onSelfWalk,
	// Token: 0x04002CF6 RID: 11510
	onSelfCrouch,
	// Token: 0x04002CF7 RID: 11511
	onSelfStand,
	// Token: 0x04002CF8 RID: 11512
	onSelfAimingGunStart,
	// Token: 0x04002CF9 RID: 11513
	onSelfAimingGunStop,
	// Token: 0x04002CFA RID: 11514
	onSelfCrouchRun,
	// Token: 0x04002CFB RID: 11515
	onSelfCrouchWalk,
	// Token: 0x04002CFC RID: 11516
	onSelfSwimStart,
	// Token: 0x04002CFD RID: 11517
	onSelfSwimStop,
	// Token: 0x04002CFE RID: 11518
	onSelfSwimRun,
	// Token: 0x04002CFF RID: 11519
	onSelfSwimIdle,
	// Token: 0x04002D00 RID: 11520
	onSelfWaterSurface,
	// Token: 0x04002D01 RID: 11521
	onSelfWaterSubmerge,
	// Token: 0x04002D02 RID: 11522
	onSelfHoldingItemThrown,
	// Token: 0x04002D03 RID: 11523
	onSelfHoldingItemCreated,
	// Token: 0x04002D04 RID: 11524
	onSelfItemCrafted,
	// Token: 0x04002D05 RID: 11525
	onSelfItemRepaired,
	// Token: 0x04002D06 RID: 11526
	onSelfItemLooted,
	// Token: 0x04002D07 RID: 11527
	onSelfItemLost,
	// Token: 0x04002D08 RID: 11528
	onSelfItemGained,
	// Token: 0x04002D09 RID: 11529
	onSelfItemSold,
	// Token: 0x04002D0A RID: 11530
	onSelfItemBought,
	// Token: 0x04002D0B RID: 11531
	onSelfItemActivate,
	// Token: 0x04002D0C RID: 11532
	onSelfItemDeactivate,
	// Token: 0x04002D0D RID: 11533
	onSelfItemJammedUse,
	// Token: 0x04002D0E RID: 11534
	onSelfChangedView,
	// Token: 0x04002D0F RID: 11535
	onSelfFallImpact,
	// Token: 0x04002D10 RID: 11536
	onProjectilePreImpact,
	// Token: 0x04002D11 RID: 11537
	onProjectileImpact,
	// Token: 0x04002D12 RID: 11538
	onPerkLevelChanged,
	// Token: 0x04002D13 RID: 11539
	onSelfEnteredBiome,
	// Token: 0x04002D14 RID: 11540
	onSelfLootContainer,
	// Token: 0x04002D15 RID: 11541
	onSelfOpenLootContainer,
	// Token: 0x04002D16 RID: 11542
	onSelfCloseLootContainer,
	// Token: 0x04002D17 RID: 11543
	onSelfBiomeLootStageMaxEntered,
	// Token: 0x04002D18 RID: 11544
	onSelfBiomeLootStageMaxExited,
	// Token: 0x04002D19 RID: 11545
	onDismember,
	// Token: 0x04002D1A RID: 11546
	onCombatEntered,
	// Token: 0x04002D1B RID: 11547
	onTrackedEntityLost,
	// Token: 0x04002D1C RID: 11548
	onTreasureRadiusEntered,
	// Token: 0x04002D1D RID: 11549
	onTreasureRadiusExited,
	// Token: 0x04002D1E RID: 11550
	onTreasureRadiusCompleted,
	// Token: 0x04002D1F RID: 11551
	onSelfPickupOther,
	// Token: 0x04002D20 RID: 11552
	onSelfDroppedOther,
	// Token: 0x04002D21 RID: 11553
	onSelfPickedUpByOther,
	// Token: 0x04002D22 RID: 11554
	COUNT
}
