using System;

// Token: 0x0200064D RID: 1613
public enum PassiveEffects : byte
{
	// Token: 0x04002968 RID: 10600
	None,
	// Token: 0x04002969 RID: 10601
	EntityDamage,
	// Token: 0x0400296A RID: 10602
	EntityHeal,
	// Token: 0x0400296B RID: 10603
	BlockDamage,
	// Token: 0x0400296C RID: 10604
	BlockRepairAmount,
	// Token: 0x0400296D RID: 10605
	DamageModifier,
	// Token: 0x0400296E RID: 10606
	LandClaimDamageModifier,
	// Token: 0x0400296F RID: 10607
	DegradationPerUse,
	// Token: 0x04002970 RID: 10608
	DegradationMax,
	// Token: 0x04002971 RID: 10609
	MagazineSize,
	// Token: 0x04002972 RID: 10610
	DamageFalloffRange,
	// Token: 0x04002973 RID: 10611
	MaxRange,
	// Token: 0x04002974 RID: 10612
	BlockRange,
	// Token: 0x04002975 RID: 10613
	WeaponHandling,
	// Token: 0x04002976 RID: 10614
	IncrementalSpreadMultiplier,
	// Token: 0x04002977 RID: 10615
	BurstRoundCount,
	// Token: 0x04002978 RID: 10616
	RoundRayCount,
	// Token: 0x04002979 RID: 10617
	RoundsPerMinute,
	// Token: 0x0400297A RID: 10618
	AttacksPerMinute,
	// Token: 0x0400297B RID: 10619
	ExplosionBlockDamage,
	// Token: 0x0400297C RID: 10620
	ExplosionEntityDamage,
	// Token: 0x0400297D RID: 10621
	ExplosionRadius,
	// Token: 0x0400297E RID: 10622
	ExplosionIncomingDamage,
	// Token: 0x0400297F RID: 10623
	ModSlots,
	// Token: 0x04002980 RID: 10624
	ModPowerBonus,
	// Token: 0x04002981 RID: 10625
	SpreadMultiplierHip,
	// Token: 0x04002982 RID: 10626
	SpreadMultiplierAiming,
	// Token: 0x04002983 RID: 10627
	SpreadMultiplierRunning,
	// Token: 0x04002984 RID: 10628
	SpreadMultiplierWalking,
	// Token: 0x04002985 RID: 10629
	SpreadMultiplierCrouching,
	// Token: 0x04002986 RID: 10630
	SpreadMultiplierIdle,
	// Token: 0x04002987 RID: 10631
	SpreadDegreesVertical,
	// Token: 0x04002988 RID: 10632
	SpreadDegreesHorizontal,
	// Token: 0x04002989 RID: 10633
	KickDegreesVerticalMin,
	// Token: 0x0400298A RID: 10634
	KickDegreesHorizontalMin,
	// Token: 0x0400298B RID: 10635
	KickDegreesVerticalMax,
	// Token: 0x0400298C RID: 10636
	KickDegreesHorizontalMax,
	// Token: 0x0400298D RID: 10637
	SphereCastRadius,
	// Token: 0x0400298E RID: 10638
	GrazeDamageMultiplier,
	// Token: 0x0400298F RID: 10639
	GrazeStaminaMultiplier,
	// Token: 0x04002990 RID: 10640
	GeneralDamageResist,
	// Token: 0x04002991 RID: 10641
	PhysicalDamageResist,
	// Token: 0x04002992 RID: 10642
	PhysicalDamageResistMax,
	// Token: 0x04002993 RID: 10643
	ElementalDamageResist,
	// Token: 0x04002994 RID: 10644
	ElementalDamageResistMax,
	// Token: 0x04002995 RID: 10645
	HypothermalResist,
	// Token: 0x04002996 RID: 10646
	HyperthermalResist,
	// Token: 0x04002997 RID: 10647
	FallDamageReduction,
	// Token: 0x04002998 RID: 10648
	Tier,
	// Token: 0x04002999 RID: 10649
	LightIntensity,
	// Token: 0x0400299A RID: 10650
	VehicleFuelMaxPer,
	// Token: 0x0400299B RID: 10651
	VehicleFuelUsePer,
	// Token: 0x0400299C RID: 10652
	VehicleVelocityMaxPer,
	// Token: 0x0400299D RID: 10653
	VehicleMotorTorquePer,
	// Token: 0x0400299E RID: 10654
	VehicleCarryCapacity,
	// Token: 0x0400299F RID: 10655
	VehicleEntityDamage,
	// Token: 0x040029A0 RID: 10656
	VehicleBlockDamage,
	// Token: 0x040029A1 RID: 10657
	VehicleSelfDamage,
	// Token: 0x040029A2 RID: 10658
	VehicleStrongSelfDamage,
	// Token: 0x040029A3 RID: 10659
	VehicleSeats,
	// Token: 0x040029A4 RID: 10660
	VehicleTankSize,
	// Token: 0x040029A5 RID: 10661
	VehicleHopStrength,
	// Token: 0x040029A6 RID: 10662
	VehiclePlayerStaminaDrainRate,
	// Token: 0x040029A7 RID: 10663
	BatteryMaxLoadInVolts,
	// Token: 0x040029A8 RID: 10664
	BatteryDischargeTimeInMinutes,
	// Token: 0x040029A9 RID: 10665
	DistractionResistance,
	// Token: 0x040029AA RID: 10666
	DistractionRadius,
	// Token: 0x040029AB RID: 10667
	DistractionLifetime,
	// Token: 0x040029AC RID: 10668
	DistractionStrength,
	// Token: 0x040029AD RID: 10669
	DistractionEatTicks,
	// Token: 0x040029AE RID: 10670
	ProjectileGravity,
	// Token: 0x040029AF RID: 10671
	ProjectileVelocity,
	// Token: 0x040029B0 RID: 10672
	ProjectileStickChance,
	// Token: 0x040029B1 RID: 10673
	RecipeTagUnlocked,
	// Token: 0x040029B2 RID: 10674
	JunkTurretActiveRange,
	// Token: 0x040029B3 RID: 10675
	JunkTurretActiveCount,
	// Token: 0x040029B4 RID: 10676
	EconomicValue,
	// Token: 0x040029B5 RID: 10677
	LockPickTime,
	// Token: 0x040029B6 RID: 10678
	LockPickBreakChance,
	// Token: 0x040029B7 RID: 10679
	LootProb,
	// Token: 0x040029B8 RID: 10680
	LootDropProb,
	// Token: 0x040029B9 RID: 10681
	LootQuantity,
	// Token: 0x040029BA RID: 10682
	Tracking,
	// Token: 0x040029BB RID: 10683
	AttributeLevel,
	// Token: 0x040029BC RID: 10684
	SkillLevel,
	// Token: 0x040029BD RID: 10685
	PerkLevel,
	// Token: 0x040029BE RID: 10686
	SkillExpGain,
	// Token: 0x040029BF RID: 10687
	PlayerExpGain,
	// Token: 0x040029C0 RID: 10688
	NoiseMultiplier,
	// Token: 0x040029C1 RID: 10689
	LightMultiplier,
	// Token: 0x040029C2 RID: 10690
	CraftingTime,
	// Token: 0x040029C3 RID: 10691
	CraftingTier,
	// Token: 0x040029C4 RID: 10692
	CraftingOutputCount,
	// Token: 0x040029C5 RID: 10693
	ActiveCraftingSlots,
	// Token: 0x040029C6 RID: 10694
	CraftingSlots,
	// Token: 0x040029C7 RID: 10695
	CraftingSmeltTime,
	// Token: 0x040029C8 RID: 10696
	ExpDeficitMaxPercentage,
	// Token: 0x040029C9 RID: 10697
	ExpDeficitPerDeathPercentage,
	// Token: 0x040029CA RID: 10698
	CarryCapacity,
	// Token: 0x040029CB RID: 10699
	BagSize,
	// Token: 0x040029CC RID: 10700
	RepairTime,
	// Token: 0x040029CD RID: 10701
	RepairAmount,
	// Token: 0x040029CE RID: 10702
	ReloadSpeedMultiplier,
	// Token: 0x040029CF RID: 10703
	WaterRegenRate,
	// Token: 0x040029D0 RID: 10704
	HealthMax,
	// Token: 0x040029D1 RID: 10705
	HealthChangeOT,
	// Token: 0x040029D2 RID: 10706
	HealthGain,
	// Token: 0x040029D3 RID: 10707
	HealthLoss,
	// Token: 0x040029D4 RID: 10708
	HealthMaxBlockage,
	// Token: 0x040029D5 RID: 10709
	StaminaMax,
	// Token: 0x040029D6 RID: 10710
	StaminaChangeOT,
	// Token: 0x040029D7 RID: 10711
	StaminaGain,
	// Token: 0x040029D8 RID: 10712
	StaminaLoss,
	// Token: 0x040029D9 RID: 10713
	StaminaMaxBlockage,
	// Token: 0x040029DA RID: 10714
	FoodMax,
	// Token: 0x040029DB RID: 10715
	FoodChangeOT,
	// Token: 0x040029DC RID: 10716
	FoodGain,
	// Token: 0x040029DD RID: 10717
	FoodLoss,
	// Token: 0x040029DE RID: 10718
	FoodLossPerHealthPointLost,
	// Token: 0x040029DF RID: 10719
	FoodLossPerStaminaPointGained,
	// Token: 0x040029E0 RID: 10720
	FoodLossPerHealthPointGained,
	// Token: 0x040029E1 RID: 10721
	FoodMaxBlockage,
	// Token: 0x040029E2 RID: 10722
	WaterMax,
	// Token: 0x040029E3 RID: 10723
	WaterChangeOT,
	// Token: 0x040029E4 RID: 10724
	WaterGain,
	// Token: 0x040029E5 RID: 10725
	WaterLoss,
	// Token: 0x040029E6 RID: 10726
	WaterLossPerHealthPointGained,
	// Token: 0x040029E7 RID: 10727
	WaterLossPerStaminaPointGained,
	// Token: 0x040029E8 RID: 10728
	WaterMaxBlockage,
	// Token: 0x040029E9 RID: 10729
	CoreTempChangeOT,
	// Token: 0x040029EA RID: 10730
	CoreTempGain,
	// Token: 0x040029EB RID: 10731
	CoreTempLoss,
	// Token: 0x040029EC RID: 10732
	JumpStrength,
	// Token: 0x040029ED RID: 10733
	WalkSpeed,
	// Token: 0x040029EE RID: 10734
	RunSpeed,
	// Token: 0x040029EF RID: 10735
	CrouchSpeed,
	// Token: 0x040029F0 RID: 10736
	Mobility,
	// Token: 0x040029F1 RID: 10737
	LandMineImmunity,
	// Token: 0x040029F2 RID: 10738
	ScavengingTime,
	// Token: 0x040029F3 RID: 10739
	ScavengingTier,
	// Token: 0x040029F4 RID: 10740
	ScavengingItemCount,
	// Token: 0x040029F5 RID: 10741
	HarvestCount,
	// Token: 0x040029F6 RID: 10742
	ScrappingTime,
	// Token: 0x040029F7 RID: 10743
	DismemberSelfChance,
	// Token: 0x040029F8 RID: 10744
	DismemberChance,
	// Token: 0x040029F9 RID: 10745
	TreasureRadius,
	// Token: 0x040029FA RID: 10746
	TreasureBlocksPerReduction,
	// Token: 0x040029FB RID: 10747
	BreathHoldDuration,
	// Token: 0x040029FC RID: 10748
	BarteringBuying,
	// Token: 0x040029FD RID: 10749
	BarteringSelling,
	// Token: 0x040029FE RID: 10750
	SecretStash,
	// Token: 0x040029FF RID: 10751
	LootTier,
	// Token: 0x04002A00 RID: 10752
	HeatGain,
	// Token: 0x04002A01 RID: 10753
	MovementFactorMultiplier,
	// Token: 0x04002A02 RID: 10754
	QuestBonusItemReward,
	// Token: 0x04002A03 RID: 10755
	QuestRewardOptionCount,
	// Token: 0x04002A04 RID: 10756
	QuestRewardChoiceCount,
	// Token: 0x04002A05 RID: 10757
	GameStage,
	// Token: 0x04002A06 RID: 10758
	TraderStage,
	// Token: 0x04002A07 RID: 10759
	LootStage,
	// Token: 0x04002A08 RID: 10760
	LootStageMax,
	// Token: 0x04002A09 RID: 10761
	DamageBonus,
	// Token: 0x04002A0A RID: 10762
	InternalDamageModifier,
	// Token: 0x04002A0B RID: 10763
	TargetArmor,
	// Token: 0x04002A0C RID: 10764
	FallingBlockDamage,
	// Token: 0x04002A0D RID: 10765
	SilenceBlockSteps,
	// Token: 0x04002A0E RID: 10766
	TrackDistance,
	// Token: 0x04002A0F RID: 10767
	HealthSteal,
	// Token: 0x04002A10 RID: 10768
	TurretWakeUp,
	// Token: 0x04002A11 RID: 10769
	ElectricalTrapXP,
	// Token: 0x04002A12 RID: 10770
	TrapDoorTriggerDelay,
	// Token: 0x04002A13 RID: 10771
	LandMineTriggerDelay,
	// Token: 0x04002A14 RID: 10772
	TrapIncomingDamage,
	// Token: 0x04002A15 RID: 10773
	BlockPickup,
	// Token: 0x04002A16 RID: 10774
	HeadShotOnly,
	// Token: 0x04002A17 RID: 10775
	NegateDamageSelf,
	// Token: 0x04002A18 RID: 10776
	NegateDamageOther,
	// Token: 0x04002A19 RID: 10777
	DisableItem,
	// Token: 0x04002A1A RID: 10778
	DisableLoot,
	// Token: 0x04002A1B RID: 10779
	NoVehicle,
	// Token: 0x04002A1C RID: 10780
	FlipControls,
	// Token: 0x04002A1D RID: 10781
	CelebrationKill,
	// Token: 0x04002A1E RID: 10782
	ShuffledBackpack,
	// Token: 0x04002A1F RID: 10783
	EnemySearchDuration,
	// Token: 0x04002A20 RID: 10784
	TwitchViewerPointRate,
	// Token: 0x04002A21 RID: 10785
	TwitchSpawnMultiplier,
	// Token: 0x04002A22 RID: 10786
	TwitchAddCooldown,
	// Token: 0x04002A23 RID: 10787
	TwitchAddPimpPot,
	// Token: 0x04002A24 RID: 10788
	InfiniteAmmo,
	// Token: 0x04002A25 RID: 10789
	NoTimeDisplay,
	// Token: 0x04002A26 RID: 10790
	DisableGameEventNotify,
	// Token: 0x04002A27 RID: 10791
	NoTrader,
	// Token: 0x04002A28 RID: 10792
	DisableMovement,
	// Token: 0x04002A29 RID: 10793
	ExperienceGain,
	// Token: 0x04002A2A RID: 10794
	CriticalChance,
	// Token: 0x04002A2B RID: 10795
	BuffProcChance,
	// Token: 0x04002A2C RID: 10796
	BuffBlink,
	// Token: 0x04002A2D RID: 10797
	BuffResistance,
	// Token: 0x04002A2E RID: 10798
	CraftingIngredientCount,
	// Token: 0x04002A2F RID: 10799
	EntityPenetrationCount,
	// Token: 0x04002A30 RID: 10800
	BlockPenetrationFactor,
	// Token: 0x04002A31 RID: 10801
	JunkDroneModCooldown,
	// Token: 0x04002A32 RID: 10802
	HeadshotDamageModifier,
	// Token: 0x04002A33 RID: 10803
	Count
}
