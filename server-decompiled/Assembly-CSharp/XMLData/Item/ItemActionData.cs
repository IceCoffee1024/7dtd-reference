using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001662 RID: 5730
	[Preserve]
	public class ItemActionData : IXMLData
	{
		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x0600B368 RID: 45928 RVA: 0x0042E883 File Offset: 0x0042CA83
		// (set) Token: 0x0600B369 RID: 45929 RVA: 0x0042E88B File Offset: 0x0042CA8B
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

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x0600B36A RID: 45930 RVA: 0x0042E894 File Offset: 0x0042CA94
		// (set) Token: 0x0600B36B RID: 45931 RVA: 0x0042E89C File Offset: 0x0042CA9C
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

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x0600B36C RID: 45932 RVA: 0x0042E8A5 File Offset: 0x0042CAA5
		// (set) Token: 0x0600B36D RID: 45933 RVA: 0x0042E8AD File Offset: 0x0042CAAD
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

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x0600B36E RID: 45934 RVA: 0x0042E8B6 File Offset: 0x0042CAB6
		// (set) Token: 0x0600B36F RID: 45935 RVA: 0x0042E8BE File Offset: 0x0042CABE
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

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x0600B370 RID: 45936 RVA: 0x0042E8C7 File Offset: 0x0042CAC7
		// (set) Token: 0x0600B371 RID: 45937 RVA: 0x0042E8CF File Offset: 0x0042CACF
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

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x0600B372 RID: 45938 RVA: 0x0042E8D8 File Offset: 0x0042CAD8
		// (set) Token: 0x0600B373 RID: 45939 RVA: 0x0042E8E0 File Offset: 0x0042CAE0
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

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x0600B374 RID: 45940 RVA: 0x0042E8E9 File Offset: 0x0042CAE9
		// (set) Token: 0x0600B375 RID: 45941 RVA: 0x0042E8F1 File Offset: 0x0042CAF1
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

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x0600B376 RID: 45942 RVA: 0x0042E8FA File Offset: 0x0042CAFA
		// (set) Token: 0x0600B377 RID: 45943 RVA: 0x0042E902 File Offset: 0x0042CB02
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

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x0600B378 RID: 45944 RVA: 0x0042E90B File Offset: 0x0042CB0B
		// (set) Token: 0x0600B379 RID: 45945 RVA: 0x0042E913 File Offset: 0x0042CB13
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

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x0600B37A RID: 45946 RVA: 0x0042E91C File Offset: 0x0042CB1C
		// (set) Token: 0x0600B37B RID: 45947 RVA: 0x0042E924 File Offset: 0x0042CB24
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

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x0600B37C RID: 45948 RVA: 0x0042E92D File Offset: 0x0042CB2D
		// (set) Token: 0x0600B37D RID: 45949 RVA: 0x0042E935 File Offset: 0x0042CB35
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

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x0600B37E RID: 45950 RVA: 0x0042E93E File Offset: 0x0042CB3E
		// (set) Token: 0x0600B37F RID: 45951 RVA: 0x0042E946 File Offset: 0x0042CB46
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

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x0600B380 RID: 45952 RVA: 0x0042E94F File Offset: 0x0042CB4F
		// (set) Token: 0x0600B381 RID: 45953 RVA: 0x0042E957 File Offset: 0x0042CB57
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

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x0600B382 RID: 45954 RVA: 0x0042E960 File Offset: 0x0042CB60
		// (set) Token: 0x0600B383 RID: 45955 RVA: 0x0042E968 File Offset: 0x0042CB68
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

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x0600B384 RID: 45956 RVA: 0x0042E971 File Offset: 0x0042CB71
		// (set) Token: 0x0600B385 RID: 45957 RVA: 0x0042E979 File Offset: 0x0042CB79
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

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x0600B386 RID: 45958 RVA: 0x0042E982 File Offset: 0x0042CB82
		// (set) Token: 0x0600B387 RID: 45959 RVA: 0x0042E98A File Offset: 0x0042CB8A
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

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600B388 RID: 45960 RVA: 0x0042E993 File Offset: 0x0042CB93
		// (set) Token: 0x0600B389 RID: 45961 RVA: 0x0042E99B File Offset: 0x0042CB9B
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

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600B38A RID: 45962 RVA: 0x0042E9A4 File Offset: 0x0042CBA4
		// (set) Token: 0x0600B38B RID: 45963 RVA: 0x0042E9AC File Offset: 0x0042CBAC
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

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600B38C RID: 45964 RVA: 0x0042E9B5 File Offset: 0x0042CBB5
		// (set) Token: 0x0600B38D RID: 45965 RVA: 0x0042E9BD File Offset: 0x0042CBBD
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

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x0600B38E RID: 45966 RVA: 0x0042E9C6 File Offset: 0x0042CBC6
		// (set) Token: 0x0600B38F RID: 45967 RVA: 0x0042E9CE File Offset: 0x0042CBCE
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

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600B390 RID: 45968 RVA: 0x0042E9D7 File Offset: 0x0042CBD7
		// (set) Token: 0x0600B391 RID: 45969 RVA: 0x0042E9DF File Offset: 0x0042CBDF
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

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600B392 RID: 45970 RVA: 0x0042E9E8 File Offset: 0x0042CBE8
		// (set) Token: 0x0600B393 RID: 45971 RVA: 0x0042E9F0 File Offset: 0x0042CBF0
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

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600B394 RID: 45972 RVA: 0x0042E9F9 File Offset: 0x0042CBF9
		// (set) Token: 0x0600B395 RID: 45973 RVA: 0x0042EA01 File Offset: 0x0042CC01
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

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x0600B396 RID: 45974 RVA: 0x0042EA0A File Offset: 0x0042CC0A
		// (set) Token: 0x0600B397 RID: 45975 RVA: 0x0042EA12 File Offset: 0x0042CC12
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

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600B398 RID: 45976 RVA: 0x0042EA1B File Offset: 0x0042CC1B
		// (set) Token: 0x0600B399 RID: 45977 RVA: 0x0042EA23 File Offset: 0x0042CC23
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

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x0600B39A RID: 45978 RVA: 0x0042EA2C File Offset: 0x0042CC2C
		// (set) Token: 0x0600B39B RID: 45979 RVA: 0x0042EA34 File Offset: 0x0042CC34
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

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x0600B39C RID: 45980 RVA: 0x0042EA3D File Offset: 0x0042CC3D
		// (set) Token: 0x0600B39D RID: 45981 RVA: 0x0042EA45 File Offset: 0x0042CC45
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

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x0600B39E RID: 45982 RVA: 0x0042EA4E File Offset: 0x0042CC4E
		// (set) Token: 0x0600B39F RID: 45983 RVA: 0x0042EA56 File Offset: 0x0042CC56
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

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x0600B3A0 RID: 45984 RVA: 0x0042EA5F File Offset: 0x0042CC5F
		// (set) Token: 0x0600B3A1 RID: 45985 RVA: 0x0042EA67 File Offset: 0x0042CC67
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

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x0600B3A2 RID: 45986 RVA: 0x0042EA70 File Offset: 0x0042CC70
		// (set) Token: 0x0600B3A3 RID: 45987 RVA: 0x0042EA78 File Offset: 0x0042CC78
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

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x0600B3A4 RID: 45988 RVA: 0x0042EA81 File Offset: 0x0042CC81
		// (set) Token: 0x0600B3A5 RID: 45989 RVA: 0x0042EA89 File Offset: 0x0042CC89
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

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x0600B3A6 RID: 45990 RVA: 0x0042EA92 File Offset: 0x0042CC92
		// (set) Token: 0x0600B3A7 RID: 45991 RVA: 0x0042EA9A File Offset: 0x0042CC9A
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

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x0600B3A8 RID: 45992 RVA: 0x0042EAA3 File Offset: 0x0042CCA3
		// (set) Token: 0x0600B3A9 RID: 45993 RVA: 0x0042EAAB File Offset: 0x0042CCAB
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

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x0600B3AA RID: 45994 RVA: 0x0042EAB4 File Offset: 0x0042CCB4
		// (set) Token: 0x0600B3AB RID: 45995 RVA: 0x0042EABC File Offset: 0x0042CCBC
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

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x0600B3AC RID: 45996 RVA: 0x0042EAC5 File Offset: 0x0042CCC5
		// (set) Token: 0x0600B3AD RID: 45997 RVA: 0x0042EACD File Offset: 0x0042CCCD
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

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600B3AE RID: 45998 RVA: 0x0042EAD6 File Offset: 0x0042CCD6
		// (set) Token: 0x0600B3AF RID: 45999 RVA: 0x0042EADE File Offset: 0x0042CCDE
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

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600B3B0 RID: 46000 RVA: 0x0042EAE7 File Offset: 0x0042CCE7
		// (set) Token: 0x0600B3B1 RID: 46001 RVA: 0x0042EAEF File Offset: 0x0042CCEF
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

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x0600B3B2 RID: 46002 RVA: 0x0042EAF8 File Offset: 0x0042CCF8
		// (set) Token: 0x0600B3B3 RID: 46003 RVA: 0x0042EB00 File Offset: 0x0042CD00
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

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x0600B3B4 RID: 46004 RVA: 0x0042EB09 File Offset: 0x0042CD09
		// (set) Token: 0x0600B3B5 RID: 46005 RVA: 0x0042EB11 File Offset: 0x0042CD11
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

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x0600B3B6 RID: 46006 RVA: 0x0042EB1A File Offset: 0x0042CD1A
		// (set) Token: 0x0600B3B7 RID: 46007 RVA: 0x0042EB22 File Offset: 0x0042CD22
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

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600B3B8 RID: 46008 RVA: 0x0042EB2B File Offset: 0x0042CD2B
		// (set) Token: 0x0600B3B9 RID: 46009 RVA: 0x0042EB33 File Offset: 0x0042CD33
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

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x0600B3BA RID: 46010 RVA: 0x0042EB3C File Offset: 0x0042CD3C
		// (set) Token: 0x0600B3BB RID: 46011 RVA: 0x0042EB44 File Offset: 0x0042CD44
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

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x0600B3BC RID: 46012 RVA: 0x0042EB4D File Offset: 0x0042CD4D
		// (set) Token: 0x0600B3BD RID: 46013 RVA: 0x0042EB55 File Offset: 0x0042CD55
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

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x0600B3BE RID: 46014 RVA: 0x0042EB5E File Offset: 0x0042CD5E
		// (set) Token: 0x0600B3BF RID: 46015 RVA: 0x0042EB66 File Offset: 0x0042CD66
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

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x0600B3C0 RID: 46016 RVA: 0x0042EB6F File Offset: 0x0042CD6F
		// (set) Token: 0x0600B3C1 RID: 46017 RVA: 0x0042EB77 File Offset: 0x0042CD77
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

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x0600B3C2 RID: 46018 RVA: 0x0042EB80 File Offset: 0x0042CD80
		// (set) Token: 0x0600B3C3 RID: 46019 RVA: 0x0042EB88 File Offset: 0x0042CD88
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

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x0600B3C4 RID: 46020 RVA: 0x0042EB91 File Offset: 0x0042CD91
		// (set) Token: 0x0600B3C5 RID: 46021 RVA: 0x0042EB99 File Offset: 0x0042CD99
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

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x0600B3C6 RID: 46022 RVA: 0x0042EBA2 File Offset: 0x0042CDA2
		// (set) Token: 0x0600B3C7 RID: 46023 RVA: 0x0042EBAA File Offset: 0x0042CDAA
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

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x0600B3C8 RID: 46024 RVA: 0x0042EBB3 File Offset: 0x0042CDB3
		// (set) Token: 0x0600B3C9 RID: 46025 RVA: 0x0042EBBB File Offset: 0x0042CDBB
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

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600B3CA RID: 46026 RVA: 0x0042EBC4 File Offset: 0x0042CDC4
		// (set) Token: 0x0600B3CB RID: 46027 RVA: 0x0042EBCC File Offset: 0x0042CDCC
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

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x0600B3CC RID: 46028 RVA: 0x0042EBD5 File Offset: 0x0042CDD5
		// (set) Token: 0x0600B3CD RID: 46029 RVA: 0x0042EBDD File Offset: 0x0042CDDD
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

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600B3CE RID: 46030 RVA: 0x0042EBE6 File Offset: 0x0042CDE6
		// (set) Token: 0x0600B3CF RID: 46031 RVA: 0x0042EBEE File Offset: 0x0042CDEE
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

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x0600B3D0 RID: 46032 RVA: 0x0042EBF7 File Offset: 0x0042CDF7
		// (set) Token: 0x0600B3D1 RID: 46033 RVA: 0x0042EBFF File Offset: 0x0042CDFF
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

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x0600B3D2 RID: 46034 RVA: 0x0042EC08 File Offset: 0x0042CE08
		// (set) Token: 0x0600B3D3 RID: 46035 RVA: 0x0042EC10 File Offset: 0x0042CE10
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

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x0600B3D4 RID: 46036 RVA: 0x0042EC19 File Offset: 0x0042CE19
		// (set) Token: 0x0600B3D5 RID: 46037 RVA: 0x0042EC21 File Offset: 0x0042CE21
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

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x0600B3D6 RID: 46038 RVA: 0x0042EC2A File Offset: 0x0042CE2A
		// (set) Token: 0x0600B3D7 RID: 46039 RVA: 0x0042EC32 File Offset: 0x0042CE32
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

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x0600B3D8 RID: 46040 RVA: 0x0042EC3B File Offset: 0x0042CE3B
		// (set) Token: 0x0600B3D9 RID: 46041 RVA: 0x0042EC43 File Offset: 0x0042CE43
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

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x0600B3DA RID: 46042 RVA: 0x0042EC4C File Offset: 0x0042CE4C
		// (set) Token: 0x0600B3DB RID: 46043 RVA: 0x0042EC54 File Offset: 0x0042CE54
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

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x0600B3DC RID: 46044 RVA: 0x0042EC5D File Offset: 0x0042CE5D
		// (set) Token: 0x0600B3DD RID: 46045 RVA: 0x0042EC65 File Offset: 0x0042CE65
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

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x0600B3DE RID: 46046 RVA: 0x0042EC6E File Offset: 0x0042CE6E
		// (set) Token: 0x0600B3DF RID: 46047 RVA: 0x0042EC76 File Offset: 0x0042CE76
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

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x0600B3E0 RID: 46048 RVA: 0x0042EC7F File Offset: 0x0042CE7F
		// (set) Token: 0x0600B3E1 RID: 46049 RVA: 0x0042EC87 File Offset: 0x0042CE87
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

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x0600B3E2 RID: 46050 RVA: 0x0042EC90 File Offset: 0x0042CE90
		// (set) Token: 0x0600B3E3 RID: 46051 RVA: 0x0042EC98 File Offset: 0x0042CE98
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

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x0600B3E4 RID: 46052 RVA: 0x0042ECA1 File Offset: 0x0042CEA1
		// (set) Token: 0x0600B3E5 RID: 46053 RVA: 0x0042ECA9 File Offset: 0x0042CEA9
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

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x0600B3E6 RID: 46054 RVA: 0x0042ECB2 File Offset: 0x0042CEB2
		// (set) Token: 0x0600B3E7 RID: 46055 RVA: 0x0042ECBA File Offset: 0x0042CEBA
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

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x0600B3E8 RID: 46056 RVA: 0x0042ECC3 File Offset: 0x0042CEC3
		// (set) Token: 0x0600B3E9 RID: 46057 RVA: 0x0042ECCB File Offset: 0x0042CECB
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

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x0600B3EA RID: 46058 RVA: 0x0042ECD4 File Offset: 0x0042CED4
		// (set) Token: 0x0600B3EB RID: 46059 RVA: 0x0042ECDC File Offset: 0x0042CEDC
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

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x0600B3EC RID: 46060 RVA: 0x0042ECE5 File Offset: 0x0042CEE5
		// (set) Token: 0x0600B3ED RID: 46061 RVA: 0x0042ECED File Offset: 0x0042CEED
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

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x0600B3EE RID: 46062 RVA: 0x0042ECF6 File Offset: 0x0042CEF6
		// (set) Token: 0x0600B3EF RID: 46063 RVA: 0x0042ECFE File Offset: 0x0042CEFE
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

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x0600B3F0 RID: 46064 RVA: 0x0042ED07 File Offset: 0x0042CF07
		// (set) Token: 0x0600B3F1 RID: 46065 RVA: 0x0042ED0F File Offset: 0x0042CF0F
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

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600B3F2 RID: 46066 RVA: 0x0042ED18 File Offset: 0x0042CF18
		// (set) Token: 0x0600B3F3 RID: 46067 RVA: 0x0042ED20 File Offset: 0x0042CF20
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

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x0600B3F4 RID: 46068 RVA: 0x0042ED29 File Offset: 0x0042CF29
		// (set) Token: 0x0600B3F5 RID: 46069 RVA: 0x0042ED31 File Offset: 0x0042CF31
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

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x0600B3F6 RID: 46070 RVA: 0x0042ED3A File Offset: 0x0042CF3A
		// (set) Token: 0x0600B3F7 RID: 46071 RVA: 0x0042ED42 File Offset: 0x0042CF42
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

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x0600B3F8 RID: 46072 RVA: 0x0042ED4B File Offset: 0x0042CF4B
		// (set) Token: 0x0600B3F9 RID: 46073 RVA: 0x0042ED53 File Offset: 0x0042CF53
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

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x0600B3FA RID: 46074 RVA: 0x0042ED5C File Offset: 0x0042CF5C
		// (set) Token: 0x0600B3FB RID: 46075 RVA: 0x0042ED64 File Offset: 0x0042CF64
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

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x0600B3FC RID: 46076 RVA: 0x0042ED6D File Offset: 0x0042CF6D
		// (set) Token: 0x0600B3FD RID: 46077 RVA: 0x0042ED75 File Offset: 0x0042CF75
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

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x0600B3FE RID: 46078 RVA: 0x0042ED7E File Offset: 0x0042CF7E
		// (set) Token: 0x0600B3FF RID: 46079 RVA: 0x0042ED86 File Offset: 0x0042CF86
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

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x0600B400 RID: 46080 RVA: 0x0042ED8F File Offset: 0x0042CF8F
		// (set) Token: 0x0600B401 RID: 46081 RVA: 0x0042ED97 File Offset: 0x0042CF97
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

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x0600B402 RID: 46082 RVA: 0x0042EDA0 File Offset: 0x0042CFA0
		// (set) Token: 0x0600B403 RID: 46083 RVA: 0x0042EDA8 File Offset: 0x0042CFA8
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

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x0600B404 RID: 46084 RVA: 0x0042EDB1 File Offset: 0x0042CFB1
		// (set) Token: 0x0600B405 RID: 46085 RVA: 0x0042EDB9 File Offset: 0x0042CFB9
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

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x0600B406 RID: 46086 RVA: 0x0042EDC2 File Offset: 0x0042CFC2
		// (set) Token: 0x0600B407 RID: 46087 RVA: 0x0042EDCA File Offset: 0x0042CFCA
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

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x0600B408 RID: 46088 RVA: 0x0042EDD3 File Offset: 0x0042CFD3
		// (set) Token: 0x0600B409 RID: 46089 RVA: 0x0042EDDB File Offset: 0x0042CFDB
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

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x0600B40A RID: 46090 RVA: 0x0042EDE4 File Offset: 0x0042CFE4
		// (set) Token: 0x0600B40B RID: 46091 RVA: 0x0042EDEC File Offset: 0x0042CFEC
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

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x0600B40C RID: 46092 RVA: 0x0042EDF5 File Offset: 0x0042CFF5
		// (set) Token: 0x0600B40D RID: 46093 RVA: 0x0042EDFD File Offset: 0x0042CFFD
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

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x0600B40E RID: 46094 RVA: 0x0042EE06 File Offset: 0x0042D006
		// (set) Token: 0x0600B40F RID: 46095 RVA: 0x0042EE0E File Offset: 0x0042D00E
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

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x0600B410 RID: 46096 RVA: 0x0042EE17 File Offset: 0x0042D017
		// (set) Token: 0x0600B411 RID: 46097 RVA: 0x0042EE1F File Offset: 0x0042D01F
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

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600B412 RID: 46098 RVA: 0x0042EE28 File Offset: 0x0042D028
		// (set) Token: 0x0600B413 RID: 46099 RVA: 0x0042EE30 File Offset: 0x0042D030
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

		// Token: 0x0600B414 RID: 46100 RVA: 0x0042EE3C File Offset: 0x0042D03C
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

		// Token: 0x040086A7 RID: 34471
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDelay;

		// Token: 0x040086A8 RID: 34472
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRange;

		// Token: 0x040086A9 RID: 34473
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundStart;

		// Token: 0x040086AA RID: 34474
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundRepeat;

		// Token: 0x040086AB RID: 34475
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEnd;

		// Token: 0x040086AC RID: 34476
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEmpty;

		// Token: 0x040086AD RID: 34477
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundReload;

		// Token: 0x040086AE RID: 34478
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundWarning;

		// Token: 0x040086AF RID: 34479
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pStaminaUsage;

		// Token: 0x040086B0 RID: 34480
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUseTime;

		// Token: 0x040086B1 RID: 34481
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname1;

		// Token: 0x040086B2 RID: 34482
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname2;

		// Token: 0x040086B3 RID: 34483
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname3;

		// Token: 0x040086B4 RID: 34484
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname4;

		// Token: 0x040086B5 RID: 34485
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname5;

		// Token: 0x040086B6 RID: 34486
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname6;

		// Token: 0x040086B7 RID: 34487
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname7;

		// Token: 0x040086B8 RID: 34488
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname8;

		// Token: 0x040086B9 RID: 34489
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname9;

		// Token: 0x040086BA RID: 34490
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeItemTo;

		// Token: 0x040086BB RID: 34491
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeBlockTo;

		// Token: 0x040086BC RID: 34492
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDoBlockAction;

		// Token: 0x040086BD RID: 34493
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainHealth;

		// Token: 0x040086BE RID: 34494
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainFood;

		// Token: 0x040086BF RID: 34495
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWater;

		// Token: 0x040086C0 RID: 34496
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainStamina;

		// Token: 0x040086C1 RID: 34497
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainSickness;

		// Token: 0x040086C2 RID: 34498
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWellness;

		// Token: 0x040086C3 RID: 34499
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuff;

		// Token: 0x040086C4 RID: 34500
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuffChance;

		// Token: 0x040086C5 RID: 34501
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDebuff;

		// Token: 0x040086C6 RID: 34502
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pCreateItem;

		// Token: 0x040086C7 RID: 34503
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pConditionRaycastBlock;

		// Token: 0x040086C8 RID: 34504
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pGainGas;

		// Token: 0x040086C9 RID: 34505
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pConsume;

		// Token: 0x040086CA RID: 34506
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockname;

		// Token: 0x040086CB RID: 34507
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthDefault;

		// Token: 0x040086CC RID: 34508
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthMax;

		// Token: 0x040086CD RID: 34509
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMaxStrainTime;

		// Token: 0x040086CE RID: 34510
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pMagazineSize;

		// Token: 0x040086CF RID: 34511
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMagazineItem;

		// Token: 0x040086D0 RID: 34512
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pReloadTime;

		// Token: 0x040086D1 RID: 34513
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletIcon;

		// Token: 0x040086D2 RID: 34514
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRaysPerShot;

		// Token: 0x040086D3 RID: 34515
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRaysSpread;

		// Token: 0x040086D4 RID: 34516
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSphere;

		// Token: 0x040086D5 RID: 34517
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMinDistance;

		// Token: 0x040086D6 RID: 34518
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMaxDistance;

		// Token: 0x040086D7 RID: 34519
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pDamageEntity;

		// Token: 0x040086D8 RID: 34520
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDamageBlock;

		// Token: 0x040086D9 RID: 34521
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleFire;

		// Token: 0x040086DA RID: 34522
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleSmoke;

		// Token: 0x040086DB RID: 34523
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlockRange;

		// Token: 0x040086DC RID: 34524
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pAutoFire;

		// Token: 0x040086DD RID: 34525
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterRate;

		// Token: 0x040086DE RID: 34526
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterDistance;

		// Token: 0x040086DF RID: 34527
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pHitmaskOverride;

		// Token: 0x040086E0 RID: 34528
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pSingleMagazineUsage;

		// Token: 0x040086E1 RID: 34529
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletMaterial;

		// Token: 0x040086E2 RID: 34530
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pInfiniteAmmo;

		// Token: 0x040086E3 RID: 34531
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxOut;

		// Token: 0x040086E4 RID: 34532
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxIn;

		// Token: 0x040086E5 RID: 34533
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pZoomOverlay;

		// Token: 0x040086E6 RID: 34534
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pVelocity;

		// Token: 0x040086E7 RID: 34535
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pFlyTime;

		// Token: 0x040086E8 RID: 34536
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLifeTime;

		// Token: 0x040086E9 RID: 34537
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCollisionRadius;

		// Token: 0x040086EA RID: 34538
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pProjectileInitialVelocity;

		// Token: 0x040086EB RID: 34539
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFertileblock;

		// Token: 0x040086EC RID: 34540
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAdjacentblock;

		// Token: 0x040086ED RID: 34541
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRepairAmount;

		// Token: 0x040086EE RID: 34542
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pUpgradeHitOffset;

		// Token: 0x040086EF RID: 34543
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAllowedUpgradeItems;

		// Token: 0x040086F0 RID: 34544
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRestrictedUpgradeItems;

		// Token: 0x040086F1 RID: 34545
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUpgradeActionSound;

		// Token: 0x040086F2 RID: 34546
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRepairActionSound;

		// Token: 0x040086F3 RID: 34547
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pReferenceItem;

		// Token: 0x040086F4 RID: 34548
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x040086F5 RID: 34549
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pActionIdx;

		// Token: 0x040086F6 RID: 34550
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pTitle;

		// Token: 0x040086F7 RID: 34551
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x040086F8 RID: 34552
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRecipesToLearn;

		// Token: 0x040086F9 RID: 34553
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pInstantiateOnLoad;

		// Token: 0x040086FA RID: 34554
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundDraw;

		// Token: 0x040086FB RID: 34555
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<DamageBonusData> pDamageBonus;

		// Token: 0x040086FC RID: 34556
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ExplosionData> pExplosion;

		// Token: 0x02001663 RID: 5731
		public static class Parser
		{
			// Token: 0x0600B416 RID: 46102 RVA: 0x0042EEA0 File Offset: 0x0042D0A0
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

			// Token: 0x0600B417 RID: 46103 RVA: 0x0042EF24 File Offset: 0x0042D124
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

			// Token: 0x040086FD RID: 34557
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
