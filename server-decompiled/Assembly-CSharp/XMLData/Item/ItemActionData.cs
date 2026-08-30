using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001643 RID: 5699
	[Preserve]
	public class ItemActionData : IXMLData
	{
		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x0600B2D6 RID: 45782 RVA: 0x0042ABA7 File Offset: 0x00428DA7
		// (set) Token: 0x0600B2D7 RID: 45783 RVA: 0x0042ABAF File Offset: 0x00428DAF
		public DataItem<float> Delay
		{
			get
			{
				return this.pDelay;
			}
			set
			{
				this.pDelay = value;
			}
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x0600B2D8 RID: 45784 RVA: 0x0042ABB8 File Offset: 0x00428DB8
		// (set) Token: 0x0600B2D9 RID: 45785 RVA: 0x0042ABC0 File Offset: 0x00428DC0
		public DataItem<float> Range
		{
			get
			{
				return this.pRange;
			}
			set
			{
				this.pRange = value;
			}
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x0600B2DA RID: 45786 RVA: 0x0042ABC9 File Offset: 0x00428DC9
		// (set) Token: 0x0600B2DB RID: 45787 RVA: 0x0042ABD1 File Offset: 0x00428DD1
		public DataItem<string> SoundStart
		{
			get
			{
				return this.pSoundStart;
			}
			set
			{
				this.pSoundStart = value;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x0600B2DC RID: 45788 RVA: 0x0042ABDA File Offset: 0x00428DDA
		// (set) Token: 0x0600B2DD RID: 45789 RVA: 0x0042ABE2 File Offset: 0x00428DE2
		public DataItem<string> SoundRepeat
		{
			get
			{
				return this.pSoundRepeat;
			}
			set
			{
				this.pSoundRepeat = value;
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x0600B2DE RID: 45790 RVA: 0x0042ABEB File Offset: 0x00428DEB
		// (set) Token: 0x0600B2DF RID: 45791 RVA: 0x0042ABF3 File Offset: 0x00428DF3
		public DataItem<string> SoundEnd
		{
			get
			{
				return this.pSoundEnd;
			}
			set
			{
				this.pSoundEnd = value;
			}
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x0600B2E0 RID: 45792 RVA: 0x0042ABFC File Offset: 0x00428DFC
		// (set) Token: 0x0600B2E1 RID: 45793 RVA: 0x0042AC04 File Offset: 0x00428E04
		public DataItem<string> SoundEmpty
		{
			get
			{
				return this.pSoundEmpty;
			}
			set
			{
				this.pSoundEmpty = value;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x0600B2E2 RID: 45794 RVA: 0x0042AC0D File Offset: 0x00428E0D
		// (set) Token: 0x0600B2E3 RID: 45795 RVA: 0x0042AC15 File Offset: 0x00428E15
		public DataItem<string> SoundReload
		{
			get
			{
				return this.pSoundReload;
			}
			set
			{
				this.pSoundReload = value;
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x0600B2E4 RID: 45796 RVA: 0x0042AC1E File Offset: 0x00428E1E
		// (set) Token: 0x0600B2E5 RID: 45797 RVA: 0x0042AC26 File Offset: 0x00428E26
		public DataItem<string> SoundWarning
		{
			get
			{
				return this.pSoundWarning;
			}
			set
			{
				this.pSoundWarning = value;
			}
		}

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x0600B2E6 RID: 45798 RVA: 0x0042AC2F File Offset: 0x00428E2F
		// (set) Token: 0x0600B2E7 RID: 45799 RVA: 0x0042AC37 File Offset: 0x00428E37
		public DataItem<string> StaminaUsage
		{
			get
			{
				return this.pStaminaUsage;
			}
			set
			{
				this.pStaminaUsage = value;
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x0600B2E8 RID: 45800 RVA: 0x0042AC40 File Offset: 0x00428E40
		// (set) Token: 0x0600B2E9 RID: 45801 RVA: 0x0042AC48 File Offset: 0x00428E48
		public DataItem<string> UseTime
		{
			get
			{
				return this.pUseTime;
			}
			set
			{
				this.pUseTime = value;
			}
		}

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x0600B2EA RID: 45802 RVA: 0x0042AC51 File Offset: 0x00428E51
		// (set) Token: 0x0600B2EB RID: 45803 RVA: 0x0042AC59 File Offset: 0x00428E59
		public DataItem<string> FocusedBlockname1
		{
			get
			{
				return this.pFocusedBlockname1;
			}
			set
			{
				this.pFocusedBlockname1 = value;
			}
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x0600B2EC RID: 45804 RVA: 0x0042AC62 File Offset: 0x00428E62
		// (set) Token: 0x0600B2ED RID: 45805 RVA: 0x0042AC6A File Offset: 0x00428E6A
		public DataItem<string> FocusedBlockname2
		{
			get
			{
				return this.pFocusedBlockname2;
			}
			set
			{
				this.pFocusedBlockname2 = value;
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x0600B2EE RID: 45806 RVA: 0x0042AC73 File Offset: 0x00428E73
		// (set) Token: 0x0600B2EF RID: 45807 RVA: 0x0042AC7B File Offset: 0x00428E7B
		public DataItem<string> FocusedBlockname3
		{
			get
			{
				return this.pFocusedBlockname3;
			}
			set
			{
				this.pFocusedBlockname3 = value;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x0600B2F0 RID: 45808 RVA: 0x0042AC84 File Offset: 0x00428E84
		// (set) Token: 0x0600B2F1 RID: 45809 RVA: 0x0042AC8C File Offset: 0x00428E8C
		public DataItem<string> FocusedBlockname4
		{
			get
			{
				return this.pFocusedBlockname4;
			}
			set
			{
				this.pFocusedBlockname4 = value;
			}
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x0600B2F2 RID: 45810 RVA: 0x0042AC95 File Offset: 0x00428E95
		// (set) Token: 0x0600B2F3 RID: 45811 RVA: 0x0042AC9D File Offset: 0x00428E9D
		public DataItem<string> FocusedBlockname5
		{
			get
			{
				return this.pFocusedBlockname5;
			}
			set
			{
				this.pFocusedBlockname5 = value;
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x0600B2F4 RID: 45812 RVA: 0x0042ACA6 File Offset: 0x00428EA6
		// (set) Token: 0x0600B2F5 RID: 45813 RVA: 0x0042ACAE File Offset: 0x00428EAE
		public DataItem<string> FocusedBlockname6
		{
			get
			{
				return this.pFocusedBlockname6;
			}
			set
			{
				this.pFocusedBlockname6 = value;
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600B2F6 RID: 45814 RVA: 0x0042ACB7 File Offset: 0x00428EB7
		// (set) Token: 0x0600B2F7 RID: 45815 RVA: 0x0042ACBF File Offset: 0x00428EBF
		public DataItem<string> FocusedBlockname7
		{
			get
			{
				return this.pFocusedBlockname7;
			}
			set
			{
				this.pFocusedBlockname7 = value;
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x0600B2F8 RID: 45816 RVA: 0x0042ACC8 File Offset: 0x00428EC8
		// (set) Token: 0x0600B2F9 RID: 45817 RVA: 0x0042ACD0 File Offset: 0x00428ED0
		public DataItem<string> FocusedBlockname8
		{
			get
			{
				return this.pFocusedBlockname8;
			}
			set
			{
				this.pFocusedBlockname8 = value;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x0600B2FA RID: 45818 RVA: 0x0042ACD9 File Offset: 0x00428ED9
		// (set) Token: 0x0600B2FB RID: 45819 RVA: 0x0042ACE1 File Offset: 0x00428EE1
		public DataItem<string> FocusedBlockname9
		{
			get
			{
				return this.pFocusedBlockname9;
			}
			set
			{
				this.pFocusedBlockname9 = value;
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x0600B2FC RID: 45820 RVA: 0x0042ACEA File Offset: 0x00428EEA
		// (set) Token: 0x0600B2FD RID: 45821 RVA: 0x0042ACF2 File Offset: 0x00428EF2
		public DataItem<string> ChangeItemTo
		{
			get
			{
				return this.pChangeItemTo;
			}
			set
			{
				this.pChangeItemTo = value;
			}
		}

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600B2FE RID: 45822 RVA: 0x0042ACFB File Offset: 0x00428EFB
		// (set) Token: 0x0600B2FF RID: 45823 RVA: 0x0042AD03 File Offset: 0x00428F03
		public DataItem<string> ChangeBlockTo
		{
			get
			{
				return this.pChangeBlockTo;
			}
			set
			{
				this.pChangeBlockTo = value;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600B300 RID: 45824 RVA: 0x0042AD0C File Offset: 0x00428F0C
		// (set) Token: 0x0600B301 RID: 45825 RVA: 0x0042AD14 File Offset: 0x00428F14
		public DataItem<string> DoBlockAction
		{
			get
			{
				return this.pDoBlockAction;
			}
			set
			{
				this.pDoBlockAction = value;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600B302 RID: 45826 RVA: 0x0042AD1D File Offset: 0x00428F1D
		// (set) Token: 0x0600B303 RID: 45827 RVA: 0x0042AD25 File Offset: 0x00428F25
		public DataItem<float> GainHealth
		{
			get
			{
				return this.pGainHealth;
			}
			set
			{
				this.pGainHealth = value;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600B304 RID: 45828 RVA: 0x0042AD2E File Offset: 0x00428F2E
		// (set) Token: 0x0600B305 RID: 45829 RVA: 0x0042AD36 File Offset: 0x00428F36
		public DataItem<float> GainFood
		{
			get
			{
				return this.pGainFood;
			}
			set
			{
				this.pGainFood = value;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600B306 RID: 45830 RVA: 0x0042AD3F File Offset: 0x00428F3F
		// (set) Token: 0x0600B307 RID: 45831 RVA: 0x0042AD47 File Offset: 0x00428F47
		public DataItem<float> GainWater
		{
			get
			{
				return this.pGainWater;
			}
			set
			{
				this.pGainWater = value;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x0600B308 RID: 45832 RVA: 0x0042AD50 File Offset: 0x00428F50
		// (set) Token: 0x0600B309 RID: 45833 RVA: 0x0042AD58 File Offset: 0x00428F58
		public DataItem<float> GainStamina
		{
			get
			{
				return this.pGainStamina;
			}
			set
			{
				this.pGainStamina = value;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x0600B30A RID: 45834 RVA: 0x0042AD61 File Offset: 0x00428F61
		// (set) Token: 0x0600B30B RID: 45835 RVA: 0x0042AD69 File Offset: 0x00428F69
		public DataItem<float> GainSickness
		{
			get
			{
				return this.pGainSickness;
			}
			set
			{
				this.pGainSickness = value;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x0600B30C RID: 45836 RVA: 0x0042AD72 File Offset: 0x00428F72
		// (set) Token: 0x0600B30D RID: 45837 RVA: 0x0042AD7A File Offset: 0x00428F7A
		public DataItem<float> GainWellness
		{
			get
			{
				return this.pGainWellness;
			}
			set
			{
				this.pGainWellness = value;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x0600B30E RID: 45838 RVA: 0x0042AD83 File Offset: 0x00428F83
		// (set) Token: 0x0600B30F RID: 45839 RVA: 0x0042AD8B File Offset: 0x00428F8B
		public DataItem<string> Buff
		{
			get
			{
				return this.pBuff;
			}
			set
			{
				this.pBuff = value;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x0600B310 RID: 45840 RVA: 0x0042AD94 File Offset: 0x00428F94
		// (set) Token: 0x0600B311 RID: 45841 RVA: 0x0042AD9C File Offset: 0x00428F9C
		public DataItem<string> BuffChance
		{
			get
			{
				return this.pBuffChance;
			}
			set
			{
				this.pBuffChance = value;
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600B312 RID: 45842 RVA: 0x0042ADA5 File Offset: 0x00428FA5
		// (set) Token: 0x0600B313 RID: 45843 RVA: 0x0042ADAD File Offset: 0x00428FAD
		public DataItem<string> Debuff
		{
			get
			{
				return this.pDebuff;
			}
			set
			{
				this.pDebuff = value;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600B314 RID: 45844 RVA: 0x0042ADB6 File Offset: 0x00428FB6
		// (set) Token: 0x0600B315 RID: 45845 RVA: 0x0042ADBE File Offset: 0x00428FBE
		public DataItem<string> CreateItem
		{
			get
			{
				return this.pCreateItem;
			}
			set
			{
				this.pCreateItem = value;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x0600B316 RID: 45846 RVA: 0x0042ADC7 File Offset: 0x00428FC7
		// (set) Token: 0x0600B317 RID: 45847 RVA: 0x0042ADCF File Offset: 0x00428FCF
		public DataItem<int> ConditionRaycastBlock
		{
			get
			{
				return this.pConditionRaycastBlock;
			}
			set
			{
				this.pConditionRaycastBlock = value;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600B318 RID: 45848 RVA: 0x0042ADD8 File Offset: 0x00428FD8
		// (set) Token: 0x0600B319 RID: 45849 RVA: 0x0042ADE0 File Offset: 0x00428FE0
		public DataItem<int> GainGas
		{
			get
			{
				return this.pGainGas;
			}
			set
			{
				this.pGainGas = value;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600B31A RID: 45850 RVA: 0x0042ADE9 File Offset: 0x00428FE9
		// (set) Token: 0x0600B31B RID: 45851 RVA: 0x0042ADF1 File Offset: 0x00428FF1
		public DataItem<bool> Consume
		{
			get
			{
				return this.pConsume;
			}
			set
			{
				this.pConsume = value;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x0600B31C RID: 45852 RVA: 0x0042ADFA File Offset: 0x00428FFA
		// (set) Token: 0x0600B31D RID: 45853 RVA: 0x0042AE02 File Offset: 0x00429002
		public DataItem<string> Blockname
		{
			get
			{
				return this.pBlockname;
			}
			set
			{
				this.pBlockname = value;
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x0600B31E RID: 45854 RVA: 0x0042AE0B File Offset: 0x0042900B
		// (set) Token: 0x0600B31F RID: 45855 RVA: 0x0042AE13 File Offset: 0x00429013
		public DataItem<float> ThrowStrengthDefault
		{
			get
			{
				return this.pThrowStrengthDefault;
			}
			set
			{
				this.pThrowStrengthDefault = value;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x0600B320 RID: 45856 RVA: 0x0042AE1C File Offset: 0x0042901C
		// (set) Token: 0x0600B321 RID: 45857 RVA: 0x0042AE24 File Offset: 0x00429024
		public DataItem<float> ThrowStrengthMax
		{
			get
			{
				return this.pThrowStrengthMax;
			}
			set
			{
				this.pThrowStrengthMax = value;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600B322 RID: 45858 RVA: 0x0042AE2D File Offset: 0x0042902D
		// (set) Token: 0x0600B323 RID: 45859 RVA: 0x0042AE35 File Offset: 0x00429035
		public DataItem<float> MaxStrainTime
		{
			get
			{
				return this.pMaxStrainTime;
			}
			set
			{
				this.pMaxStrainTime = value;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x0600B324 RID: 45860 RVA: 0x0042AE3E File Offset: 0x0042903E
		// (set) Token: 0x0600B325 RID: 45861 RVA: 0x0042AE46 File Offset: 0x00429046
		public DataItem<int> MagazineSize
		{
			get
			{
				return this.pMagazineSize;
			}
			set
			{
				this.pMagazineSize = value;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x0600B326 RID: 45862 RVA: 0x0042AE4F File Offset: 0x0042904F
		// (set) Token: 0x0600B327 RID: 45863 RVA: 0x0042AE57 File Offset: 0x00429057
		public DataItem<string> MagazineItem
		{
			get
			{
				return this.pMagazineItem;
			}
			set
			{
				this.pMagazineItem = value;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x0600B328 RID: 45864 RVA: 0x0042AE60 File Offset: 0x00429060
		// (set) Token: 0x0600B329 RID: 45865 RVA: 0x0042AE68 File Offset: 0x00429068
		public DataItem<float> ReloadTime
		{
			get
			{
				return this.pReloadTime;
			}
			set
			{
				this.pReloadTime = value;
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x0600B32A RID: 45866 RVA: 0x0042AE71 File Offset: 0x00429071
		// (set) Token: 0x0600B32B RID: 45867 RVA: 0x0042AE79 File Offset: 0x00429079
		public DataItem<string> BulletIcon
		{
			get
			{
				return this.pBulletIcon;
			}
			set
			{
				this.pBulletIcon = value;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x0600B32C RID: 45868 RVA: 0x0042AE82 File Offset: 0x00429082
		// (set) Token: 0x0600B32D RID: 45869 RVA: 0x0042AE8A File Offset: 0x0042908A
		public DataItem<int> RaysPerShot
		{
			get
			{
				return this.pRaysPerShot;
			}
			set
			{
				this.pRaysPerShot = value;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600B32E RID: 45870 RVA: 0x0042AE93 File Offset: 0x00429093
		// (set) Token: 0x0600B32F RID: 45871 RVA: 0x0042AE9B File Offset: 0x0042909B
		public DataItem<float> RaysSpread
		{
			get
			{
				return this.pRaysSpread;
			}
			set
			{
				this.pRaysSpread = value;
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600B330 RID: 45872 RVA: 0x0042AEA4 File Offset: 0x004290A4
		// (set) Token: 0x0600B331 RID: 45873 RVA: 0x0042AEAC File Offset: 0x004290AC
		public DataItem<float> Sphere
		{
			get
			{
				return this.pSphere;
			}
			set
			{
				this.pSphere = value;
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600B332 RID: 45874 RVA: 0x0042AEB5 File Offset: 0x004290B5
		// (set) Token: 0x0600B333 RID: 45875 RVA: 0x0042AEBD File Offset: 0x004290BD
		public DataItem<int> CrosshairMinDistance
		{
			get
			{
				return this.pCrosshairMinDistance;
			}
			set
			{
				this.pCrosshairMinDistance = value;
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x0600B334 RID: 45876 RVA: 0x0042AEC6 File Offset: 0x004290C6
		// (set) Token: 0x0600B335 RID: 45877 RVA: 0x0042AECE File Offset: 0x004290CE
		public DataItem<int> CrosshairMaxDistance
		{
			get
			{
				return this.pCrosshairMaxDistance;
			}
			set
			{
				this.pCrosshairMaxDistance = value;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x0600B336 RID: 45878 RVA: 0x0042AED7 File Offset: 0x004290D7
		// (set) Token: 0x0600B337 RID: 45879 RVA: 0x0042AEDF File Offset: 0x004290DF
		public DataItem<int> DamageEntity
		{
			get
			{
				return this.pDamageEntity;
			}
			set
			{
				this.pDamageEntity = value;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x0600B338 RID: 45880 RVA: 0x0042AEE8 File Offset: 0x004290E8
		// (set) Token: 0x0600B339 RID: 45881 RVA: 0x0042AEF0 File Offset: 0x004290F0
		public DataItem<float> DamageBlock
		{
			get
			{
				return this.pDamageBlock;
			}
			set
			{
				this.pDamageBlock = value;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x0600B33A RID: 45882 RVA: 0x0042AEF9 File Offset: 0x004290F9
		// (set) Token: 0x0600B33B RID: 45883 RVA: 0x0042AF01 File Offset: 0x00429101
		public DataItem<string> ParticlesMuzzleFire
		{
			get
			{
				return this.pParticlesMuzzleFire;
			}
			set
			{
				this.pParticlesMuzzleFire = value;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x0600B33C RID: 45884 RVA: 0x0042AF0A File Offset: 0x0042910A
		// (set) Token: 0x0600B33D RID: 45885 RVA: 0x0042AF12 File Offset: 0x00429112
		public DataItem<string> ParticlesMuzzleSmoke
		{
			get
			{
				return this.pParticlesMuzzleSmoke;
			}
			set
			{
				this.pParticlesMuzzleSmoke = value;
			}
		}

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600B33E RID: 45886 RVA: 0x0042AF1B File Offset: 0x0042911B
		// (set) Token: 0x0600B33F RID: 45887 RVA: 0x0042AF23 File Offset: 0x00429123
		public DataItem<float> BlockRange
		{
			get
			{
				return this.pBlockRange;
			}
			set
			{
				this.pBlockRange = value;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600B340 RID: 45888 RVA: 0x0042AF2C File Offset: 0x0042912C
		// (set) Token: 0x0600B341 RID: 45889 RVA: 0x0042AF34 File Offset: 0x00429134
		public DataItem<bool> AutoFire
		{
			get
			{
				return this.pAutoFire;
			}
			set
			{
				this.pAutoFire = value;
			}
		}

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x0600B342 RID: 45890 RVA: 0x0042AF3D File Offset: 0x0042913D
		// (set) Token: 0x0600B343 RID: 45891 RVA: 0x0042AF45 File Offset: 0x00429145
		public DataItem<float> HordeMeterRate
		{
			get
			{
				return this.pHordeMeterRate;
			}
			set
			{
				this.pHordeMeterRate = value;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x0600B344 RID: 45892 RVA: 0x0042AF4E File Offset: 0x0042914E
		// (set) Token: 0x0600B345 RID: 45893 RVA: 0x0042AF56 File Offset: 0x00429156
		public DataItem<float> HordeMeterDistance
		{
			get
			{
				return this.pHordeMeterDistance;
			}
			set
			{
				this.pHordeMeterDistance = value;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x0600B346 RID: 45894 RVA: 0x0042AF5F File Offset: 0x0042915F
		// (set) Token: 0x0600B347 RID: 45895 RVA: 0x0042AF67 File Offset: 0x00429167
		public DataItem<string> HitmaskOverride
		{
			get
			{
				return this.pHitmaskOverride;
			}
			set
			{
				this.pHitmaskOverride = value;
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x0600B348 RID: 45896 RVA: 0x0042AF70 File Offset: 0x00429170
		// (set) Token: 0x0600B349 RID: 45897 RVA: 0x0042AF78 File Offset: 0x00429178
		public DataItem<bool> SingleMagazineUsage
		{
			get
			{
				return this.pSingleMagazineUsage;
			}
			set
			{
				this.pSingleMagazineUsage = value;
			}
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x0600B34A RID: 45898 RVA: 0x0042AF81 File Offset: 0x00429181
		// (set) Token: 0x0600B34B RID: 45899 RVA: 0x0042AF89 File Offset: 0x00429189
		public DataItem<string> BulletMaterial
		{
			get
			{
				return this.pBulletMaterial;
			}
			set
			{
				this.pBulletMaterial = value;
			}
		}

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x0600B34C RID: 45900 RVA: 0x0042AF92 File Offset: 0x00429192
		// (set) Token: 0x0600B34D RID: 45901 RVA: 0x0042AF9A File Offset: 0x0042919A
		public DataItem<bool> InfiniteAmmo
		{
			get
			{
				return this.pInfiniteAmmo;
			}
			set
			{
				this.pInfiniteAmmo = value;
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600B34E RID: 45902 RVA: 0x0042AFA3 File Offset: 0x004291A3
		// (set) Token: 0x0600B34F RID: 45903 RVA: 0x0042AFAB File Offset: 0x004291AB
		public DataItem<float> ZoomMaxOut
		{
			get
			{
				return this.pZoomMaxOut;
			}
			set
			{
				this.pZoomMaxOut = value;
			}
		}

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x0600B350 RID: 45904 RVA: 0x0042AFB4 File Offset: 0x004291B4
		// (set) Token: 0x0600B351 RID: 45905 RVA: 0x0042AFBC File Offset: 0x004291BC
		public DataItem<float> ZoomMaxIn
		{
			get
			{
				return this.pZoomMaxIn;
			}
			set
			{
				this.pZoomMaxIn = value;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x0600B352 RID: 45906 RVA: 0x0042AFC5 File Offset: 0x004291C5
		// (set) Token: 0x0600B353 RID: 45907 RVA: 0x0042AFCD File Offset: 0x004291CD
		public DataItem<string> ZoomOverlay
		{
			get
			{
				return this.pZoomOverlay;
			}
			set
			{
				this.pZoomOverlay = value;
			}
		}

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x0600B354 RID: 45908 RVA: 0x0042AFD6 File Offset: 0x004291D6
		// (set) Token: 0x0600B355 RID: 45909 RVA: 0x0042AFDE File Offset: 0x004291DE
		public DataItem<int> Velocity
		{
			get
			{
				return this.pVelocity;
			}
			set
			{
				this.pVelocity = value;
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x0600B356 RID: 45910 RVA: 0x0042AFE7 File Offset: 0x004291E7
		// (set) Token: 0x0600B357 RID: 45911 RVA: 0x0042AFEF File Offset: 0x004291EF
		public DataItem<float> FlyTime
		{
			get
			{
				return this.pFlyTime;
			}
			set
			{
				this.pFlyTime = value;
			}
		}

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x0600B358 RID: 45912 RVA: 0x0042AFF8 File Offset: 0x004291F8
		// (set) Token: 0x0600B359 RID: 45913 RVA: 0x0042B000 File Offset: 0x00429200
		public DataItem<float> LifeTime
		{
			get
			{
				return this.pLifeTime;
			}
			set
			{
				this.pLifeTime = value;
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x0600B35A RID: 45914 RVA: 0x0042B009 File Offset: 0x00429209
		// (set) Token: 0x0600B35B RID: 45915 RVA: 0x0042B011 File Offset: 0x00429211
		public DataItem<float> CollisionRadius
		{
			get
			{
				return this.pCollisionRadius;
			}
			set
			{
				this.pCollisionRadius = value;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x0600B35C RID: 45916 RVA: 0x0042B01A File Offset: 0x0042921A
		// (set) Token: 0x0600B35D RID: 45917 RVA: 0x0042B022 File Offset: 0x00429222
		public DataItem<int> ProjectileInitialVelocity
		{
			get
			{
				return this.pProjectileInitialVelocity;
			}
			set
			{
				this.pProjectileInitialVelocity = value;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x0600B35E RID: 45918 RVA: 0x0042B02B File Offset: 0x0042922B
		// (set) Token: 0x0600B35F RID: 45919 RVA: 0x0042B033 File Offset: 0x00429233
		public DataItem<string> Fertileblock
		{
			get
			{
				return this.pFertileblock;
			}
			set
			{
				this.pFertileblock = value;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x0600B360 RID: 45920 RVA: 0x0042B03C File Offset: 0x0042923C
		// (set) Token: 0x0600B361 RID: 45921 RVA: 0x0042B044 File Offset: 0x00429244
		public DataItem<string> Adjacentblock
		{
			get
			{
				return this.pAdjacentblock;
			}
			set
			{
				this.pAdjacentblock = value;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x0600B362 RID: 45922 RVA: 0x0042B04D File Offset: 0x0042924D
		// (set) Token: 0x0600B363 RID: 45923 RVA: 0x0042B055 File Offset: 0x00429255
		public DataItem<int> RepairAmount
		{
			get
			{
				return this.pRepairAmount;
			}
			set
			{
				this.pRepairAmount = value;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x0600B364 RID: 45924 RVA: 0x0042B05E File Offset: 0x0042925E
		// (set) Token: 0x0600B365 RID: 45925 RVA: 0x0042B066 File Offset: 0x00429266
		public DataItem<int> UpgradeHitOffset
		{
			get
			{
				return this.pUpgradeHitOffset;
			}
			set
			{
				this.pUpgradeHitOffset = value;
			}
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x0600B366 RID: 45926 RVA: 0x0042B06F File Offset: 0x0042926F
		// (set) Token: 0x0600B367 RID: 45927 RVA: 0x0042B077 File Offset: 0x00429277
		public DataItem<string> AllowedUpgradeItems
		{
			get
			{
				return this.pAllowedUpgradeItems;
			}
			set
			{
				this.pAllowedUpgradeItems = value;
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x0600B368 RID: 45928 RVA: 0x0042B080 File Offset: 0x00429280
		// (set) Token: 0x0600B369 RID: 45929 RVA: 0x0042B088 File Offset: 0x00429288
		public DataItem<string> RestrictedUpgradeItems
		{
			get
			{
				return this.pRestrictedUpgradeItems;
			}
			set
			{
				this.pRestrictedUpgradeItems = value;
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x0600B36A RID: 45930 RVA: 0x0042B091 File Offset: 0x00429291
		// (set) Token: 0x0600B36B RID: 45931 RVA: 0x0042B099 File Offset: 0x00429299
		public DataItem<string> UpgradeActionSound
		{
			get
			{
				return this.pUpgradeActionSound;
			}
			set
			{
				this.pUpgradeActionSound = value;
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x0600B36C RID: 45932 RVA: 0x0042B0A2 File Offset: 0x004292A2
		// (set) Token: 0x0600B36D RID: 45933 RVA: 0x0042B0AA File Offset: 0x004292AA
		public DataItem<string> RepairActionSound
		{
			get
			{
				return this.pRepairActionSound;
			}
			set
			{
				this.pRepairActionSound = value;
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x0600B36E RID: 45934 RVA: 0x0042B0B3 File Offset: 0x004292B3
		// (set) Token: 0x0600B36F RID: 45935 RVA: 0x0042B0BB File Offset: 0x004292BB
		public DataItem<string> ReferenceItem
		{
			get
			{
				return this.pReferenceItem;
			}
			set
			{
				this.pReferenceItem = value;
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x0600B370 RID: 45936 RVA: 0x0042B0C4 File Offset: 0x004292C4
		// (set) Token: 0x0600B371 RID: 45937 RVA: 0x0042B0CC File Offset: 0x004292CC
		public DataItem<string> Mesh
		{
			get
			{
				return this.pMesh;
			}
			set
			{
				this.pMesh = value;
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x0600B372 RID: 45938 RVA: 0x0042B0D5 File Offset: 0x004292D5
		// (set) Token: 0x0600B373 RID: 45939 RVA: 0x0042B0DD File Offset: 0x004292DD
		public DataItem<int> ActionIdx
		{
			get
			{
				return this.pActionIdx;
			}
			set
			{
				this.pActionIdx = value;
			}
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x0600B374 RID: 45940 RVA: 0x0042B0E6 File Offset: 0x004292E6
		// (set) Token: 0x0600B375 RID: 45941 RVA: 0x0042B0EE File Offset: 0x004292EE
		public DataItem<string> Title
		{
			get
			{
				return this.pTitle;
			}
			set
			{
				this.pTitle = value;
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x0600B376 RID: 45942 RVA: 0x0042B0F7 File Offset: 0x004292F7
		// (set) Token: 0x0600B377 RID: 45943 RVA: 0x0042B0FF File Offset: 0x004292FF
		public DataItem<string> Description
		{
			get
			{
				return this.pDescription;
			}
			set
			{
				this.pDescription = value;
			}
		}

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x0600B378 RID: 45944 RVA: 0x0042B108 File Offset: 0x00429308
		// (set) Token: 0x0600B379 RID: 45945 RVA: 0x0042B110 File Offset: 0x00429310
		public DataItem<string> RecipesToLearn
		{
			get
			{
				return this.pRecipesToLearn;
			}
			set
			{
				this.pRecipesToLearn = value;
			}
		}

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x0600B37A RID: 45946 RVA: 0x0042B119 File Offset: 0x00429319
		// (set) Token: 0x0600B37B RID: 45947 RVA: 0x0042B121 File Offset: 0x00429321
		public DataItem<string> InstantiateOnLoad
		{
			get
			{
				return this.pInstantiateOnLoad;
			}
			set
			{
				this.pInstantiateOnLoad = value;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600B37C RID: 45948 RVA: 0x0042B12A File Offset: 0x0042932A
		// (set) Token: 0x0600B37D RID: 45949 RVA: 0x0042B132 File Offset: 0x00429332
		public DataItem<string> SoundDraw
		{
			get
			{
				return this.pSoundDraw;
			}
			set
			{
				this.pSoundDraw = value;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600B37E RID: 45950 RVA: 0x0042B13B File Offset: 0x0042933B
		// (set) Token: 0x0600B37F RID: 45951 RVA: 0x0042B143 File Offset: 0x00429343
		public DataItem<DamageBonusData> DamageBonus
		{
			get
			{
				return this.pDamageBonus;
			}
			set
			{
				this.pDamageBonus = value;
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600B380 RID: 45952 RVA: 0x0042B14C File Offset: 0x0042934C
		// (set) Token: 0x0600B381 RID: 45953 RVA: 0x0042B154 File Offset: 0x00429354
		public DataItem<ExplosionData> Explosion
		{
			get
			{
				return this.pExplosion;
			}
			set
			{
				this.pExplosion = value;
			}
		}

		// Token: 0x0600B382 RID: 45954 RVA: 0x0042B160 File Offset: 0x00429360
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			List<IDataItem> list = new List<IDataItem>();
			if (_recursive && this.pDamageBonus != null)
			{
				list.AddRange(this.pDamageBonus.Value.GetDisplayValues(true));
			}
			if (_recursive && this.pExplosion != null)
			{
				list.AddRange(this.pExplosion.Value.GetDisplayValues(true));
			}
			return list;
		}

		// Token: 0x04008651 RID: 34385
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDelay;

		// Token: 0x04008652 RID: 34386
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRange;

		// Token: 0x04008653 RID: 34387
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundStart;

		// Token: 0x04008654 RID: 34388
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundRepeat;

		// Token: 0x04008655 RID: 34389
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEnd;

		// Token: 0x04008656 RID: 34390
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEmpty;

		// Token: 0x04008657 RID: 34391
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundReload;

		// Token: 0x04008658 RID: 34392
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundWarning;

		// Token: 0x04008659 RID: 34393
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pStaminaUsage;

		// Token: 0x0400865A RID: 34394
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUseTime;

		// Token: 0x0400865B RID: 34395
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname1;

		// Token: 0x0400865C RID: 34396
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname2;

		// Token: 0x0400865D RID: 34397
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname3;

		// Token: 0x0400865E RID: 34398
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname4;

		// Token: 0x0400865F RID: 34399
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname5;

		// Token: 0x04008660 RID: 34400
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname6;

		// Token: 0x04008661 RID: 34401
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname7;

		// Token: 0x04008662 RID: 34402
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname8;

		// Token: 0x04008663 RID: 34403
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname9;

		// Token: 0x04008664 RID: 34404
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeItemTo;

		// Token: 0x04008665 RID: 34405
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeBlockTo;

		// Token: 0x04008666 RID: 34406
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDoBlockAction;

		// Token: 0x04008667 RID: 34407
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainHealth;

		// Token: 0x04008668 RID: 34408
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainFood;

		// Token: 0x04008669 RID: 34409
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWater;

		// Token: 0x0400866A RID: 34410
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainStamina;

		// Token: 0x0400866B RID: 34411
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainSickness;

		// Token: 0x0400866C RID: 34412
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWellness;

		// Token: 0x0400866D RID: 34413
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuff;

		// Token: 0x0400866E RID: 34414
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuffChance;

		// Token: 0x0400866F RID: 34415
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDebuff;

		// Token: 0x04008670 RID: 34416
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pCreateItem;

		// Token: 0x04008671 RID: 34417
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pConditionRaycastBlock;

		// Token: 0x04008672 RID: 34418
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pGainGas;

		// Token: 0x04008673 RID: 34419
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pConsume;

		// Token: 0x04008674 RID: 34420
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockname;

		// Token: 0x04008675 RID: 34421
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthDefault;

		// Token: 0x04008676 RID: 34422
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthMax;

		// Token: 0x04008677 RID: 34423
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMaxStrainTime;

		// Token: 0x04008678 RID: 34424
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pMagazineSize;

		// Token: 0x04008679 RID: 34425
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMagazineItem;

		// Token: 0x0400867A RID: 34426
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pReloadTime;

		// Token: 0x0400867B RID: 34427
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletIcon;

		// Token: 0x0400867C RID: 34428
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRaysPerShot;

		// Token: 0x0400867D RID: 34429
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRaysSpread;

		// Token: 0x0400867E RID: 34430
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSphere;

		// Token: 0x0400867F RID: 34431
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMinDistance;

		// Token: 0x04008680 RID: 34432
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMaxDistance;

		// Token: 0x04008681 RID: 34433
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pDamageEntity;

		// Token: 0x04008682 RID: 34434
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDamageBlock;

		// Token: 0x04008683 RID: 34435
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleFire;

		// Token: 0x04008684 RID: 34436
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleSmoke;

		// Token: 0x04008685 RID: 34437
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlockRange;

		// Token: 0x04008686 RID: 34438
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pAutoFire;

		// Token: 0x04008687 RID: 34439
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterRate;

		// Token: 0x04008688 RID: 34440
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterDistance;

		// Token: 0x04008689 RID: 34441
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pHitmaskOverride;

		// Token: 0x0400868A RID: 34442
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pSingleMagazineUsage;

		// Token: 0x0400868B RID: 34443
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletMaterial;

		// Token: 0x0400868C RID: 34444
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pInfiniteAmmo;

		// Token: 0x0400868D RID: 34445
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxOut;

		// Token: 0x0400868E RID: 34446
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxIn;

		// Token: 0x0400868F RID: 34447
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pZoomOverlay;

		// Token: 0x04008690 RID: 34448
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pVelocity;

		// Token: 0x04008691 RID: 34449
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pFlyTime;

		// Token: 0x04008692 RID: 34450
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLifeTime;

		// Token: 0x04008693 RID: 34451
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCollisionRadius;

		// Token: 0x04008694 RID: 34452
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pProjectileInitialVelocity;

		// Token: 0x04008695 RID: 34453
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFertileblock;

		// Token: 0x04008696 RID: 34454
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAdjacentblock;

		// Token: 0x04008697 RID: 34455
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRepairAmount;

		// Token: 0x04008698 RID: 34456
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pUpgradeHitOffset;

		// Token: 0x04008699 RID: 34457
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAllowedUpgradeItems;

		// Token: 0x0400869A RID: 34458
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRestrictedUpgradeItems;

		// Token: 0x0400869B RID: 34459
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUpgradeActionSound;

		// Token: 0x0400869C RID: 34460
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRepairActionSound;

		// Token: 0x0400869D RID: 34461
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pReferenceItem;

		// Token: 0x0400869E RID: 34462
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x0400869F RID: 34463
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pActionIdx;

		// Token: 0x040086A0 RID: 34464
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pTitle;

		// Token: 0x040086A1 RID: 34465
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x040086A2 RID: 34466
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRecipesToLearn;

		// Token: 0x040086A3 RID: 34467
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pInstantiateOnLoad;

		// Token: 0x040086A4 RID: 34468
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundDraw;

		// Token: 0x040086A5 RID: 34469
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<DamageBonusData> pDamageBonus;

		// Token: 0x040086A6 RID: 34470
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ExplosionData> pExplosion;

		// Token: 0x02001644 RID: 5700
		public static class Parser
		{
			// Token: 0x0600B384 RID: 45956 RVA: 0x0042B1C4 File Offset: 0x004293C4
			[PublicizedFrom(EAccessModifier.Private)]
			public static DataItem<string> ParseItem(string _string, PositionXmlElement _elem)
			{
				string text;
				try
				{
					text = stringParser.Parse(ParserUtils.ParseStringAttribute(_elem, "value", true, null));
				}
				catch (Exception ex)
				{
					throw new InvalidValueException(string.Concat(new string[]
					{
						"Could not parse attribute \"",
						_elem.Name,
						"\" value \"",
						ParserUtils.ParseStringAttribute(_elem, "value", true, null),
						"\""
					}), _elem.LineNumber, ex);
				}
				return new DataItem<string>(_string, text);
			}

			// Token: 0x0600B385 RID: 45957 RVA: 0x0042B248 File Offset: 0x00429448
			public static ItemAction Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "ItemAction");
				Type type = Type.GetType(typeof(ItemActionData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				ItemAction itemAction = (ItemAction)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing ItemAction", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!ItemActionData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing ItemAction", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 2050383924U)
						{
							if (num <= 973038343U)
							{
								if (num <= 477934085U)
								{
									if (num <= 264004213U)
									{
										if (num <= 91459346U)
										{
											if (num != 32426346U)
											{
												if (num == 91459346U)
												{
													if (text2 == "CrosshairMinDistance")
													{
														int num2;
														try
														{
															num2 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
														}
														catch (Exception ex)
														{
															throw new InvalidValueException(string.Concat(new string[]
															{
																"Could not parse attribute \"",
																positionXmlElement.Name,
																"\" value \"",
																ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
																"\""
															}), positionXmlElement.LineNumber, ex);
														}
														DataItem<int> dataItem = new DataItem<int>("CrosshairMinDistance", num2);
														itemAction.pCrosshairMinDistance = dataItem;
													}
												}
											}
											else if (text2 == "HitmaskOverride")
											{
												string text3;
												try
												{
													text3 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex2)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex2);
												}
												DataItem<string> dataItem2 = new DataItem<string>("HitmaskOverride", text3);
												itemAction.pHitmaskOverride = dataItem2;
											}
										}
										else if (num != 132142556U)
										{
											if (num != 248841283U)
											{
												if (num == 264004213U)
												{
													if (text2 == "ProjectileInitialVelocity")
													{
														int num3;
														try
														{
															num3 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
														}
														catch (Exception ex3)
														{
															throw new InvalidValueException(string.Concat(new string[]
															{
																"Could not parse attribute \"",
																positionXmlElement.Name,
																"\" value \"",
																ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
																"\""
															}), positionXmlElement.LineNumber, ex3);
														}
														DataItem<int> dataItem3 = new DataItem<int>("ProjectileInitialVelocity", num3);
														itemAction.pProjectileInitialVelocity = dataItem3;
													}
												}
											}
											else if (text2 == "RaysSpread")
											{
												float num4;
												try
												{
													num4 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex4)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex4);
												}
												DataItem<float> dataItem4 = new DataItem<float>("RaysSpread", num4);
												itemAction.pRaysSpread = dataItem4;
											}
										}
										else if (text2 == "Mesh")
										{
											string text4;
											try
											{
												text4 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex5)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex5);
											}
											DataItem<string> dataItem5 = new DataItem<string>("Mesh", text4);
											itemAction.pMesh = dataItem5;
										}
									}
									else if (num <= 388984571U)
									{
										if (num != 383713930U)
										{
											if (num == 388984571U)
											{
												if (text2 == "DamageEntity")
												{
													int num5;
													try
													{
														num5 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex6)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex6);
													}
													DataItem<int> dataItem6 = new DataItem<int>("DamageEntity", num5);
													itemAction.pDamageEntity = dataItem6;
												}
											}
										}
										else if (text2 == "AutoFire")
										{
											bool flag;
											try
											{
												flag = boolParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex7)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex7);
											}
											DataItem<bool> dataItem7 = new DataItem<bool>("AutoFire", flag);
											itemAction.pAutoFire = dataItem7;
										}
									}
									else if (num != 422257095U)
									{
										if (num != 441323083U)
										{
											if (num == 477934085U)
											{
												if (text2 == "MaxStrainTime")
												{
													float num6;
													try
													{
														num6 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex8)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex8);
													}
													DataItem<float> dataItem8 = new DataItem<float>("MaxStrainTime", num6);
													itemAction.pMaxStrainTime = dataItem8;
												}
											}
										}
										else if (text2 == "RaysPerShot")
										{
											int num7;
											try
											{
												num7 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex9)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex9);
											}
											DataItem<int> dataItem9 = new DataItem<int>("RaysPerShot", num7);
											itemAction.pRaysPerShot = dataItem9;
										}
									}
									else if (text2 == "GainGas")
									{
										int num8;
										try
										{
											num8 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex10)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex10);
										}
										DataItem<int> dataItem10 = new DataItem<int>("GainGas", num8);
										itemAction.pGainGas = dataItem10;
									}
								}
								else if (num <= 678358751U)
								{
									if (num <= 573162709U)
									{
										if (num != 550270072U)
										{
											if (num == 573162709U)
											{
												if (text2 == "SoundEmpty")
												{
													string text5;
													try
													{
														text5 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex11)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex11);
													}
													DataItem<string> dataItem11 = new DataItem<string>("SoundEmpty", text5);
													itemAction.pSoundEmpty = dataItem11;
												}
											}
										}
										else if (text2 == "Delay")
										{
											float num9;
											try
											{
												num9 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex12)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex12);
											}
											DataItem<float> dataItem12 = new DataItem<float>("Delay", num9);
											itemAction.pDelay = dataItem12;
										}
									}
									else if (num != 573416248U)
									{
										if (num != 617902505U)
										{
											if (num == 678358751U)
											{
												if (text2 == "GainStamina")
												{
													float num10;
													try
													{
														num10 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex13)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex13);
													}
													DataItem<float> dataItem13 = new DataItem<float>("GainStamina", num10);
													itemAction.pGainStamina = dataItem13;
												}
											}
										}
										else if (text2 == "Title")
										{
											string text6;
											try
											{
												text6 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex14)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex14);
											}
											DataItem<string> dataItem14 = new DataItem<string>("Title", text6);
											itemAction.pTitle = dataItem14;
										}
									}
									else if (text2 == "Sphere")
									{
										float num11;
										try
										{
											num11 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex15)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex15);
										}
										DataItem<float> dataItem15 = new DataItem<float>("Sphere", num11);
										itemAction.pSphere = dataItem15;
									}
								}
								else if (num <= 859861643U)
								{
									if (num != 738624775U)
									{
										if (num != 843848237U)
										{
											if (num == 859861643U)
											{
												if (text2 == "SoundRepeat")
												{
													string text7;
													try
													{
														text7 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex16)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex16);
													}
													DataItem<string> dataItem16 = new DataItem<string>("SoundRepeat", text7);
													itemAction.pSoundRepeat = dataItem16;
												}
											}
										}
										else if (text2 == "CollisionRadius")
										{
											float num12;
											try
											{
												num12 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex17)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex17);
											}
											DataItem<float> dataItem17 = new DataItem<float>("CollisionRadius", num12);
											itemAction.pCollisionRadius = dataItem17;
										}
									}
									else if (text2 == "ThrowStrengthDefault")
									{
										float num13;
										try
										{
											num13 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex18)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex18);
										}
										DataItem<float> dataItem18 = new DataItem<float>("ThrowStrengthDefault", num13);
										itemAction.pThrowStrengthDefault = dataItem18;
									}
								}
								else if (num != 883311634U)
								{
									if (num != 906214847U)
									{
										if (num == 973038343U)
										{
											if (text2 == "InfiniteAmmo")
											{
												bool flag2;
												try
												{
													flag2 = boolParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex19)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex19);
												}
												DataItem<bool> dataItem19 = new DataItem<bool>("InfiniteAmmo", flag2);
												itemAction.pInfiniteAmmo = dataItem19;
											}
										}
									}
									else if (text2 == "ChangeBlockTo")
									{
										string text8;
										try
										{
											text8 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex20)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex20);
										}
										DataItem<string> dataItem20 = new DataItem<string>("ChangeBlockTo", text8);
										itemAction.pChangeBlockTo = dataItem20;
									}
								}
								else if (text2 == "ZoomOverlay")
								{
									string text9;
									try
									{
										text9 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex21)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex21);
									}
									DataItem<string> dataItem21 = new DataItem<string>("ZoomOverlay", text9);
									itemAction.pZoomOverlay = dataItem21;
								}
							}
							else if (num <= 1725856265U)
							{
								if (num <= 1237474039U)
								{
									if (num <= 1173181999U)
									{
										if (num != 1157581142U)
										{
											if (num == 1173181999U)
											{
												if (text2 == "BlockRange")
												{
													float num14;
													try
													{
														num14 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex22)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex22);
													}
													DataItem<float> dataItem22 = new DataItem<float>("BlockRange", num14);
													itemAction.pBlockRange = dataItem22;
												}
											}
										}
										else if (text2 == "SingleMagazineUsage")
										{
											bool flag3;
											try
											{
												flag3 = boolParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex23)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex23);
											}
											DataItem<bool> dataItem23 = new DataItem<bool>("SingleMagazineUsage", flag3);
											itemAction.pSingleMagazineUsage = dataItem23;
										}
									}
									else if (num != 1180803064U)
									{
										if (num != 1221226493U)
										{
											if (num == 1237474039U)
											{
												if (text2 == "DamageBonus")
												{
													DamageBonusData damageBonusData = DamageBonusData.Parser.Parse(positionXmlElement, _updateLater);
													DataItem<DamageBonusData> dataItem24 = new DataItem<DamageBonusData>("DamageBonus", damageBonusData);
													itemAction.pDamageBonus = dataItem24;
												}
											}
										}
										else if (text2 == "RecipesToLearn")
										{
											string text10;
											try
											{
												text10 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex24)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex24);
											}
											DataItem<string> dataItem25 = new DataItem<string>("RecipesToLearn", text10);
											itemAction.pRecipesToLearn = dataItem25;
										}
									}
									else if (text2 == "SoundStart")
									{
										string text11;
										try
										{
											text11 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex25)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex25);
										}
										DataItem<string> dataItem26 = new DataItem<string>("SoundStart", text11);
										itemAction.pSoundStart = dataItem26;
									}
								}
								else if (num <= 1330610628U)
								{
									if (num != 1259223448U)
									{
										if (num != 1292404073U)
										{
											if (num == 1330610628U)
											{
												if (text2 == "CrosshairMaxDistance")
												{
													int num15;
													try
													{
														num15 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex26)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex26);
													}
													DataItem<int> dataItem27 = new DataItem<int>("CrosshairMaxDistance", num15);
													itemAction.pCrosshairMaxDistance = dataItem27;
												}
											}
										}
										else if (text2 == "SoundEnd")
										{
											string text12;
											try
											{
												text12 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex27)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex27);
											}
											DataItem<string> dataItem28 = new DataItem<string>("SoundEnd", text12);
											itemAction.pSoundEnd = dataItem28;
										}
									}
									else if (text2 == "ZoomMaxOut")
									{
										float num16;
										try
										{
											num16 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex28)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex28);
										}
										DataItem<float> dataItem29 = new DataItem<float>("ZoomMaxOut", num16);
										itemAction.pZoomMaxOut = dataItem29;
									}
								}
								else if (num != 1601156781U)
								{
									if (num != 1660679176U)
									{
										if (num == 1725856265U)
										{
											if (text2 == "Description")
											{
												string text13;
												try
												{
													text13 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex29)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex29);
												}
												DataItem<string> dataItem30 = new DataItem<string>("Description", text13);
												itemAction.pDescription = dataItem30;
											}
										}
									}
									else if (text2 == "BuffChance")
									{
										string text14;
										try
										{
											text14 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex30)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex30);
										}
										DataItem<string> dataItem31 = new DataItem<string>("BuffChance", text14);
										itemAction.pBuffChance = dataItem31;
									}
								}
								else if (text2 == "ReloadTime")
								{
									float num17;
									try
									{
										num17 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex31)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex31);
									}
									DataItem<float> dataItem32 = new DataItem<float>("ReloadTime", num17);
									itemAction.pReloadTime = dataItem32;
								}
							}
							else if (num <= 1811911295U)
							{
								if (num <= 1744800819U)
								{
									if (num != 1728023200U)
									{
										if (num == 1744800819U)
										{
											if (text2 == "FocusedBlockname5")
											{
												string text15;
												try
												{
													text15 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex32)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex32);
												}
												DataItem<string> dataItem33 = new DataItem<string>("FocusedBlockname5", text15);
												itemAction.pFocusedBlockname5 = dataItem33;
											}
										}
									}
									else if (text2 == "FocusedBlockname4")
									{
										string text16;
										try
										{
											text16 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex33)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex33);
										}
										DataItem<string> dataItem34 = new DataItem<string>("FocusedBlockname4", text16);
										itemAction.pFocusedBlockname4 = dataItem34;
									}
								}
								else if (num != 1761578438U)
								{
									if (num != 1778356057U)
									{
										if (num == 1811911295U)
										{
											if (text2 == "FocusedBlockname1")
											{
												string text17;
												try
												{
													text17 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex34)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex34);
												}
												DataItem<string> dataItem35 = new DataItem<string>("FocusedBlockname1", text17);
												itemAction.pFocusedBlockname1 = dataItem35;
											}
										}
									}
									else if (text2 == "FocusedBlockname7")
									{
										string text18;
										try
										{
											text18 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex35)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex35);
										}
										DataItem<string> dataItem36 = new DataItem<string>("FocusedBlockname7", text18);
										itemAction.pFocusedBlockname7 = dataItem36;
									}
								}
								else if (text2 == "FocusedBlockname6")
								{
									string text19;
									try
									{
										text19 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex36)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex36);
									}
									DataItem<string> dataItem37 = new DataItem<string>("FocusedBlockname6", text19);
									itemAction.pFocusedBlockname6 = dataItem37;
								}
							}
							else if (num <= 1929345774U)
							{
								if (num != 1828688914U)
								{
									if (num != 1845466533U)
									{
										if (num == 1929345774U)
										{
											if (text2 == "LifeTime")
											{
												float num18;
												try
												{
													num18 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex37)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex37);
												}
												DataItem<float> dataItem38 = new DataItem<float>("LifeTime", num18);
												itemAction.pLifeTime = dataItem38;
											}
										}
									}
									else if (text2 == "FocusedBlockname3")
									{
										string text20;
										try
										{
											text20 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex38)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex38);
										}
										DataItem<string> dataItem39 = new DataItem<string>("FocusedBlockname3", text20);
										itemAction.pFocusedBlockname3 = dataItem39;
									}
								}
								else if (text2 == "FocusedBlockname2")
								{
									string text21;
									try
									{
										text21 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex39)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex39);
									}
									DataItem<string> dataItem40 = new DataItem<string>("FocusedBlockname2", text21);
									itemAction.pFocusedBlockname2 = dataItem40;
								}
							}
							else if (num != 1929354628U)
							{
								if (num != 1946132247U)
								{
									if (num == 2050383924U)
									{
										if (text2 == "RestrictedUpgradeItems")
										{
											string text22;
											try
											{
												text22 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex40)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex40);
											}
											DataItem<string> dataItem41 = new DataItem<string>("RestrictedUpgradeItems", text22);
											itemAction.pRestrictedUpgradeItems = dataItem41;
										}
									}
								}
								else if (text2 == "FocusedBlockname9")
								{
									string text23;
									try
									{
										text23 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex41)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex41);
									}
									DataItem<string> dataItem42 = new DataItem<string>("FocusedBlockname9", text23);
									itemAction.pFocusedBlockname9 = dataItem42;
								}
							}
							else if (text2 == "FocusedBlockname8")
							{
								string text24;
								try
								{
									text24 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex42)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex42);
								}
								DataItem<string> dataItem43 = new DataItem<string>("FocusedBlockname8", text24);
								itemAction.pFocusedBlockname8 = dataItem43;
							}
						}
						else if (num <= 3213271394U)
						{
							if (num <= 2731875518U)
							{
								if (num <= 2292684416U)
								{
									if (num <= 2079371571U)
									{
										if (num != 2054944794U)
										{
											if (num == 2079371571U)
											{
												if (text2 == "Fertileblock")
												{
													string text25;
													try
													{
														text25 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
													}
													catch (Exception ex43)
													{
														throw new InvalidValueException(string.Concat(new string[]
														{
															"Could not parse attribute \"",
															positionXmlElement.Name,
															"\" value \"",
															ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
															"\""
														}), positionXmlElement.LineNumber, ex43);
													}
													DataItem<string> dataItem44 = new DataItem<string>("Fertileblock", text25);
													itemAction.pFertileblock = dataItem44;
												}
											}
										}
										else if (text2 == "ParticlesMuzzleSmoke")
										{
											string text26;
											try
											{
												text26 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex44)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex44);
											}
											DataItem<string> dataItem45 = new DataItem<string>("ParticlesMuzzleSmoke", text26);
											itemAction.pParticlesMuzzleSmoke = dataItem45;
										}
									}
									else if (num != 2205678605U)
									{
										if (num != 2214691755U)
										{
											if (num == 2292684416U)
											{
												if (text2 == "Explosion")
												{
													ExplosionData explosionData = ExplosionData.Parser.Parse(positionXmlElement, _updateLater);
													DataItem<ExplosionData> dataItem46 = new DataItem<ExplosionData>("Explosion", explosionData);
													itemAction.pExplosion = dataItem46;
												}
											}
										}
										else if (text2 == "GainSickness")
										{
											float num19;
											try
											{
												num19 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex45)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex45);
											}
											DataItem<float> dataItem47 = new DataItem<float>("GainSickness", num19);
											itemAction.pGainSickness = dataItem47;
										}
									}
									else if (text2 == "ParticlesMuzzleFire")
									{
										string text27;
										try
										{
											text27 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex46)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex46);
										}
										DataItem<string> dataItem48 = new DataItem<string>("ParticlesMuzzleFire", text27);
										itemAction.pParticlesMuzzleFire = dataItem48;
									}
								}
								else if (num <= 2391273097U)
								{
									if (num != 2296213333U)
									{
										if (num == 2391273097U)
										{
											if (text2 == "GainWater")
											{
												float num20;
												try
												{
													num20 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex47)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex47);
												}
												DataItem<float> dataItem49 = new DataItem<float>("GainWater", num20);
												itemAction.pGainWater = dataItem49;
											}
										}
									}
									else if (text2 == "UseTime")
									{
										string text28;
										try
										{
											text28 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex48)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex48);
										}
										DataItem<string> dataItem50 = new DataItem<string>("UseTime", text28);
										itemAction.pUseTime = dataItem50;
									}
								}
								else if (num != 2508081957U)
								{
									if (num != 2654093974U)
									{
										if (num == 2731875518U)
										{
											if (text2 == "ActionIdx")
											{
												int num21;
												try
												{
													num21 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex49)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex49);
												}
												DataItem<int> dataItem51 = new DataItem<int>("ActionIdx", num21);
												itemAction.pActionIdx = dataItem51;
											}
										}
									}
									else if (text2 == "SoundDraw")
									{
										string text29;
										try
										{
											text29 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex50)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex50);
										}
										DataItem<string> dataItem52 = new DataItem<string>("SoundDraw", text29);
										itemAction.pSoundDraw = dataItem52;
									}
								}
								else if (text2 == "RepairActionSound")
								{
									string text30;
									try
									{
										text30 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex51)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex51);
									}
									DataItem<string> dataItem53 = new DataItem<string>("RepairActionSound", text30);
									itemAction.pRepairActionSound = dataItem53;
								}
							}
							else if (num <= 2951397452U)
							{
								if (num <= 2776146097U)
								{
									if (num != 2735859570U)
									{
										if (num == 2776146097U)
										{
											if (text2 == "GainWellness")
											{
												float num22;
												try
												{
													num22 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex52)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex52);
												}
												DataItem<float> dataItem54 = new DataItem<float>("GainWellness", num22);
												itemAction.pGainWellness = dataItem54;
											}
										}
									}
									else if (text2 == "Range")
									{
										float num23;
										try
										{
											num23 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex53)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex53);
										}
										DataItem<float> dataItem55 = new DataItem<float>("Range", num23);
										itemAction.pRange = dataItem55;
									}
								}
								else if (num != 2889970889U)
								{
									if (num != 2916596296U)
									{
										if (num == 2951397452U)
										{
											if (text2 == "ThrowStrengthMax")
											{
												float num24;
												try
												{
													num24 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex54)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex54);
												}
												DataItem<float> dataItem56 = new DataItem<float>("ThrowStrengthMax", num24);
												itemAction.pThrowStrengthMax = dataItem56;
											}
										}
									}
									else if (text2 == "SoundWarning")
									{
										string text31;
										try
										{
											text31 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex55)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex55);
										}
										DataItem<string> dataItem57 = new DataItem<string>("SoundWarning", text31);
										itemAction.pSoundWarning = dataItem57;
									}
								}
								else if (text2 == "Blockname")
								{
									string text32;
									try
									{
										text32 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex56)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex56);
									}
									DataItem<string> dataItem58 = new DataItem<string>("Blockname", text32);
									itemAction.pBlockname = dataItem58;
								}
							}
							else if (num <= 3036802414U)
							{
								if (num != 2970340076U)
								{
									if (num != 3027266612U)
									{
										if (num == 3036802414U)
										{
											if (text2 == "GainFood")
											{
												float num25;
												try
												{
													num25 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex57)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex57);
												}
												DataItem<float> dataItem59 = new DataItem<float>("GainFood", num25);
												itemAction.pGainFood = dataItem59;
											}
										}
									}
									else if (text2 == "HordeMeterRate")
									{
										float num26;
										try
										{
											num26 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex58)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex58);
										}
										DataItem<float> dataItem60 = new DataItem<float>("HordeMeterRate", num26);
										itemAction.pHordeMeterRate = dataItem60;
									}
								}
								else if (text2 == "Buff")
								{
									string text33;
									try
									{
										text33 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex59)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex59);
									}
									DataItem<string> dataItem61 = new DataItem<string>("Buff", text33);
									itemAction.pBuff = dataItem61;
								}
							}
							else if (num != 3118731031U)
							{
								if (num != 3124789842U)
								{
									if (num == 3213271394U)
									{
										if (text2 == "MagazineSize")
										{
											int num27;
											try
											{
												num27 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex60)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex60);
											}
											DataItem<int> dataItem62 = new DataItem<int>("MagazineSize", num27);
											itemAction.pMagazineSize = dataItem62;
										}
									}
								}
								else if (text2 == "Velocity")
								{
									int num28;
									try
									{
										num28 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex61)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex61);
									}
									DataItem<int> dataItem63 = new DataItem<int>("Velocity", num28);
									itemAction.pVelocity = dataItem63;
								}
							}
							else if (text2 == "DoBlockAction")
							{
								itemAction.pDoBlockAction = ItemActionData.Parser.ParseItem("DoBlockAction", positionXmlElement);
							}
						}
						else if (num <= 3880205310U)
						{
							if (num <= 3448204316U)
							{
								if (num <= 3261865014U)
								{
									if (num != 3239575924U)
									{
										if (num == 3261865014U)
										{
											if (text2 == "ConditionRaycastBlock")
											{
												int num29;
												try
												{
													num29 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex62)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex62);
												}
												DataItem<int> dataItem64 = new DataItem<int>("ConditionRaycastBlock", num29);
												itemAction.pConditionRaycastBlock = dataItem64;
											}
										}
									}
									else if (text2 == "BulletMaterial")
									{
										string text34;
										try
										{
											text34 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex63)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex63);
										}
										DataItem<string> dataItem65 = new DataItem<string>("BulletMaterial", text34);
										itemAction.pBulletMaterial = dataItem65;
									}
								}
								else if (num != 3297724363U)
								{
									if (num != 3370626489U)
									{
										if (num == 3448204316U)
										{
											if (text2 == "GainHealth")
											{
												float num30;
												try
												{
													num30 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex64)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex64);
												}
												DataItem<float> dataItem66 = new DataItem<float>("GainHealth", num30);
												itemAction.pGainHealth = dataItem66;
											}
										}
									}
									else if (text2 == "SoundReload")
									{
										string text35;
										try
										{
											text35 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex65)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex65);
										}
										DataItem<string> dataItem67 = new DataItem<string>("SoundReload", text35);
										itemAction.pSoundReload = dataItem67;
									}
								}
								else if (text2 == "ZoomMaxIn")
								{
									float num31;
									try
									{
										num31 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex66)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex66);
									}
									DataItem<float> dataItem68 = new DataItem<float>("ZoomMaxIn", num31);
									itemAction.pZoomMaxIn = dataItem68;
								}
							}
							else if (num <= 3781194609U)
							{
								if (num != 3646476408U)
								{
									if (num != 3699885409U)
									{
										if (num == 3781194609U)
										{
											if (text2 == "ReferenceItem")
											{
												string text36;
												try
												{
													text36 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex67)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex67);
												}
												DataItem<string> dataItem69 = new DataItem<string>("ReferenceItem", text36);
												itemAction.pReferenceItem = dataItem69;
											}
										}
									}
									else if (text2 == "Consume")
									{
										bool flag4;
										try
										{
											flag4 = boolParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex68)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex68);
										}
										DataItem<bool> dataItem70 = new DataItem<bool>("Consume", flag4);
										itemAction.pConsume = dataItem70;
									}
								}
								else if (text2 == "RepairAmount")
								{
									int num32;
									try
									{
										num32 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex69)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex69);
									}
									DataItem<int> dataItem71 = new DataItem<int>("RepairAmount", num32);
									itemAction.pRepairAmount = dataItem71;
								}
							}
							else if (num != 3788546643U)
							{
								if (num != 3860496195U)
								{
									if (num == 3880205310U)
									{
										if (text2 == "CreateItem")
										{
											string text37;
											try
											{
												text37 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex70)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex70);
											}
											DataItem<string> dataItem72 = new DataItem<string>("CreateItem", text37);
											itemAction.pCreateItem = dataItem72;
										}
									}
								}
								else if (text2 == "ChangeItemTo")
								{
									string text38;
									try
									{
										text38 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex71)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex71);
									}
									DataItem<string> dataItem73 = new DataItem<string>("ChangeItemTo", text38);
									itemAction.pChangeItemTo = dataItem73;
								}
							}
							else if (text2 == "AllowedUpgradeItems")
							{
								string text39;
								try
								{
									text39 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex72)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex72);
								}
								DataItem<string> dataItem74 = new DataItem<string>("AllowedUpgradeItems", text39);
								itemAction.pAllowedUpgradeItems = dataItem74;
							}
						}
						else if (num <= 4005067140U)
						{
							if (num <= 3894590787U)
							{
								if (num != 3893059105U)
								{
									if (num == 3894590787U)
									{
										if (text2 == "FlyTime")
										{
											float num33;
											try
											{
												num33 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex73)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex73);
											}
											DataItem<float> dataItem75 = new DataItem<float>("FlyTime", num33);
											itemAction.pFlyTime = dataItem75;
										}
									}
								}
								else if (text2 == "StaminaUsage")
								{
									string text40;
									try
									{
										text40 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex74)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex74);
									}
									DataItem<string> dataItem76 = new DataItem<string>("StaminaUsage", text40);
									itemAction.pStaminaUsage = dataItem76;
								}
							}
							else if (num != 3922517809U)
							{
								if (num != 3965826732U)
								{
									if (num == 4005067140U)
									{
										if (text2 == "MagazineItem")
										{
											string text41;
											try
											{
												text41 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex75)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex75);
											}
											DataItem<string> dataItem77 = new DataItem<string>("MagazineItem", text41);
											itemAction.pMagazineItem = dataItem77;
										}
									}
								}
								else if (text2 == "UpgradeActionSound")
								{
									string text42;
									try
									{
										text42 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex76)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex76);
									}
									DataItem<string> dataItem78 = new DataItem<string>("UpgradeActionSound", text42);
									itemAction.pUpgradeActionSound = dataItem78;
								}
							}
							else if (text2 == "DamageBlock")
							{
								float num34;
								try
								{
									num34 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex77)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex77);
								}
								DataItem<float> dataItem79 = new DataItem<float>("DamageBlock", num34);
								itemAction.pDamageBlock = dataItem79;
							}
						}
						else if (num <= 4124220918U)
						{
							if (num != 4013882141U)
							{
								if (num != 4069726011U)
								{
									if (num == 4124220918U)
									{
										if (text2 == "InstantiateOnLoad")
										{
											string text43;
											try
											{
												text43 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex78)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex78);
											}
											DataItem<string> dataItem80 = new DataItem<string>("InstantiateOnLoad", text43);
											itemAction.pInstantiateOnLoad = dataItem80;
										}
									}
								}
								else if (text2 == "UpgradeHitOffset")
								{
									int num35;
									try
									{
										num35 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex79)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex79);
									}
									DataItem<int> dataItem81 = new DataItem<int>("UpgradeHitOffset", num35);
									itemAction.pUpgradeHitOffset = dataItem81;
								}
							}
							else if (text2 == "HordeMeterDistance")
							{
								float num36;
								try
								{
									num36 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex80)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex80);
								}
								DataItem<float> dataItem82 = new DataItem<float>("HordeMeterDistance", num36);
								itemAction.pHordeMeterDistance = dataItem82;
							}
						}
						else if (num != 4129641136U)
						{
							if (num != 4139408471U)
							{
								if (num == 4265352596U)
								{
									if (text2 == "BulletIcon")
									{
										string text44;
										try
										{
											text44 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex81)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex81);
										}
										DataItem<string> dataItem83 = new DataItem<string>("BulletIcon", text44);
										itemAction.pBulletIcon = dataItem83;
									}
								}
							}
							else if (text2 == "Debuff")
							{
								string text45;
								try
								{
									text45 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex82)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex82);
								}
								DataItem<string> dataItem84 = new DataItem<string>("Debuff", text45);
								itemAction.pDebuff = dataItem84;
							}
						}
						else if (text2 == "Adjacentblock")
						{
							string text46;
							try
							{
								text46 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
							}
							catch (Exception ex83)
							{
								throw new InvalidValueException(string.Concat(new string[]
								{
									"Could not parse attribute \"",
									positionXmlElement.Name,
									"\" value \"",
									ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
									"\""
								}), positionXmlElement.LineNumber, ex83);
							}
							DataItem<string> dataItem85 = new DataItem<string>("Adjacentblock", text46);
							itemAction.pAdjacentblock = dataItem85;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num37 = dictionary2[text2];
						dictionary2[text2] = num37 + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in ItemActionData.Parser.knownAttributesMultiplicity)
				{
					int num38 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num38 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num38 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num38.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return itemAction;
			}

			// Token: 0x040086A7 RID: 34471
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Delay",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Range",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundStart",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundRepeat",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundEnd",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundEmpty",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundReload",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundWarning",
					new Range<int>(true, 0, true, 1)
				},
				{
					"StaminaUsage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"UseTime",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname1",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname2",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname3",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname4",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname5",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname6",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname7",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname8",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FocusedBlockname9",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ChangeItemTo",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ChangeBlockTo",
					new Range<int>(true, 0, true, 1)
				},
				{
					"DoBlockAction",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainHealth",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainFood",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainWater",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainStamina",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainSickness",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainWellness",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Buff",
					new Range<int>(true, 0, true, 1)
				},
				{
					"BuffChance",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Debuff",
					new Range<int>(true, 0, true, 1)
				},
				{
					"CreateItem",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ConditionRaycastBlock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainGas",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Consume",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Blockname",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ThrowStrengthDefault",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ThrowStrengthMax",
					new Range<int>(true, 0, true, 1)
				},
				{
					"MaxStrainTime",
					new Range<int>(true, 0, true, 1)
				},
				{
					"MagazineSize",
					new Range<int>(true, 0, true, 1)
				},
				{
					"MagazineItem",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ReloadTime",
					new Range<int>(true, 0, true, 1)
				},
				{
					"BulletIcon",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RaysPerShot",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RaysSpread",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Sphere",
					new Range<int>(true, 0, true, 1)
				},
				{
					"CrosshairMinDistance",
					new Range<int>(true, 0, true, 1)
				},
				{
					"CrosshairMaxDistance",
					new Range<int>(true, 0, true, 1)
				},
				{
					"DamageEntity",
					new Range<int>(true, 0, true, 1)
				},
				{
					"DamageBlock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ParticlesMuzzleFire",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ParticlesMuzzleSmoke",
					new Range<int>(true, 0, true, 1)
				},
				{
					"BlockRange",
					new Range<int>(true, 0, true, 1)
				},
				{
					"AutoFire",
					new Range<int>(true, 0, true, 1)
				},
				{
					"HordeMeterRate",
					new Range<int>(true, 0, true, 1)
				},
				{
					"HordeMeterDistance",
					new Range<int>(true, 0, true, 1)
				},
				{
					"HitmaskOverride",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SingleMagazineUsage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"BulletMaterial",
					new Range<int>(true, 0, true, 1)
				},
				{
					"InfiniteAmmo",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ZoomMaxOut",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ZoomMaxIn",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ZoomOverlay",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Velocity",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FlyTime",
					new Range<int>(true, 0, true, 1)
				},
				{
					"LifeTime",
					new Range<int>(true, 0, true, 1)
				},
				{
					"CollisionRadius",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ProjectileInitialVelocity",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Fertileblock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Adjacentblock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RepairAmount",
					new Range<int>(true, 0, true, 1)
				},
				{
					"UpgradeHitOffset",
					new Range<int>(true, 0, true, 1)
				},
				{
					"AllowedUpgradeItems",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RestrictedUpgradeItems",
					new Range<int>(true, 0, true, 1)
				},
				{
					"UpgradeActionSound",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RepairActionSound",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ReferenceItem",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Mesh",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ActionIdx",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Title",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Description",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RecipesToLearn",
					new Range<int>(true, 0, true, 1)
				},
				{
					"InstantiateOnLoad",
					new Range<int>(true, 0, true, 1)
				},
				{
					"SoundDraw",
					new Range<int>(true, 0, true, 1)
				},
				{
					"DamageBonus",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Explosion",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
