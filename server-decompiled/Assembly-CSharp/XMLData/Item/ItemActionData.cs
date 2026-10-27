using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001674 RID: 5748
	[Preserve]
	public class ItemActionData : IXMLData
	{
		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x0600B3C0 RID: 46016 RVA: 0x00430C87 File Offset: 0x0042EE87
		// (set) Token: 0x0600B3C1 RID: 46017 RVA: 0x00430C8F File Offset: 0x0042EE8F
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

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x0600B3C2 RID: 46018 RVA: 0x00430C98 File Offset: 0x0042EE98
		// (set) Token: 0x0600B3C3 RID: 46019 RVA: 0x00430CA0 File Offset: 0x0042EEA0
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

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x0600B3C4 RID: 46020 RVA: 0x00430CA9 File Offset: 0x0042EEA9
		// (set) Token: 0x0600B3C5 RID: 46021 RVA: 0x00430CB1 File Offset: 0x0042EEB1
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

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x0600B3C6 RID: 46022 RVA: 0x00430CBA File Offset: 0x0042EEBA
		// (set) Token: 0x0600B3C7 RID: 46023 RVA: 0x00430CC2 File Offset: 0x0042EEC2
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

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600B3C8 RID: 46024 RVA: 0x00430CCB File Offset: 0x0042EECB
		// (set) Token: 0x0600B3C9 RID: 46025 RVA: 0x00430CD3 File Offset: 0x0042EED3
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

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600B3CA RID: 46026 RVA: 0x00430CDC File Offset: 0x0042EEDC
		// (set) Token: 0x0600B3CB RID: 46027 RVA: 0x00430CE4 File Offset: 0x0042EEE4
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

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600B3CC RID: 46028 RVA: 0x00430CED File Offset: 0x0042EEED
		// (set) Token: 0x0600B3CD RID: 46029 RVA: 0x00430CF5 File Offset: 0x0042EEF5
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

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x0600B3CE RID: 46030 RVA: 0x00430CFE File Offset: 0x0042EEFE
		// (set) Token: 0x0600B3CF RID: 46031 RVA: 0x00430D06 File Offset: 0x0042EF06
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

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600B3D0 RID: 46032 RVA: 0x00430D0F File Offset: 0x0042EF0F
		// (set) Token: 0x0600B3D1 RID: 46033 RVA: 0x00430D17 File Offset: 0x0042EF17
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

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600B3D2 RID: 46034 RVA: 0x00430D20 File Offset: 0x0042EF20
		// (set) Token: 0x0600B3D3 RID: 46035 RVA: 0x00430D28 File Offset: 0x0042EF28
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

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600B3D4 RID: 46036 RVA: 0x00430D31 File Offset: 0x0042EF31
		// (set) Token: 0x0600B3D5 RID: 46037 RVA: 0x00430D39 File Offset: 0x0042EF39
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

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x0600B3D6 RID: 46038 RVA: 0x00430D42 File Offset: 0x0042EF42
		// (set) Token: 0x0600B3D7 RID: 46039 RVA: 0x00430D4A File Offset: 0x0042EF4A
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

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600B3D8 RID: 46040 RVA: 0x00430D53 File Offset: 0x0042EF53
		// (set) Token: 0x0600B3D9 RID: 46041 RVA: 0x00430D5B File Offset: 0x0042EF5B
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

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x0600B3DA RID: 46042 RVA: 0x00430D64 File Offset: 0x0042EF64
		// (set) Token: 0x0600B3DB RID: 46043 RVA: 0x00430D6C File Offset: 0x0042EF6C
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

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x0600B3DC RID: 46044 RVA: 0x00430D75 File Offset: 0x0042EF75
		// (set) Token: 0x0600B3DD RID: 46045 RVA: 0x00430D7D File Offset: 0x0042EF7D
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

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x0600B3DE RID: 46046 RVA: 0x00430D86 File Offset: 0x0042EF86
		// (set) Token: 0x0600B3DF RID: 46047 RVA: 0x00430D8E File Offset: 0x0042EF8E
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

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x0600B3E0 RID: 46048 RVA: 0x00430D97 File Offset: 0x0042EF97
		// (set) Token: 0x0600B3E1 RID: 46049 RVA: 0x00430D9F File Offset: 0x0042EF9F
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

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x0600B3E2 RID: 46050 RVA: 0x00430DA8 File Offset: 0x0042EFA8
		// (set) Token: 0x0600B3E3 RID: 46051 RVA: 0x00430DB0 File Offset: 0x0042EFB0
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

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x0600B3E4 RID: 46052 RVA: 0x00430DB9 File Offset: 0x0042EFB9
		// (set) Token: 0x0600B3E5 RID: 46053 RVA: 0x00430DC1 File Offset: 0x0042EFC1
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

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x0600B3E6 RID: 46054 RVA: 0x00430DCA File Offset: 0x0042EFCA
		// (set) Token: 0x0600B3E7 RID: 46055 RVA: 0x00430DD2 File Offset: 0x0042EFD2
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

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x0600B3E8 RID: 46056 RVA: 0x00430DDB File Offset: 0x0042EFDB
		// (set) Token: 0x0600B3E9 RID: 46057 RVA: 0x00430DE3 File Offset: 0x0042EFE3
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

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x0600B3EA RID: 46058 RVA: 0x00430DEC File Offset: 0x0042EFEC
		// (set) Token: 0x0600B3EB RID: 46059 RVA: 0x00430DF4 File Offset: 0x0042EFF4
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

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x0600B3EC RID: 46060 RVA: 0x00430DFD File Offset: 0x0042EFFD
		// (set) Token: 0x0600B3ED RID: 46061 RVA: 0x00430E05 File Offset: 0x0042F005
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

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600B3EE RID: 46062 RVA: 0x00430E0E File Offset: 0x0042F00E
		// (set) Token: 0x0600B3EF RID: 46063 RVA: 0x00430E16 File Offset: 0x0042F016
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

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600B3F0 RID: 46064 RVA: 0x00430E1F File Offset: 0x0042F01F
		// (set) Token: 0x0600B3F1 RID: 46065 RVA: 0x00430E27 File Offset: 0x0042F027
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

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x0600B3F2 RID: 46066 RVA: 0x00430E30 File Offset: 0x0042F030
		// (set) Token: 0x0600B3F3 RID: 46067 RVA: 0x00430E38 File Offset: 0x0042F038
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

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x0600B3F4 RID: 46068 RVA: 0x00430E41 File Offset: 0x0042F041
		// (set) Token: 0x0600B3F5 RID: 46069 RVA: 0x00430E49 File Offset: 0x0042F049
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

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x0600B3F6 RID: 46070 RVA: 0x00430E52 File Offset: 0x0042F052
		// (set) Token: 0x0600B3F7 RID: 46071 RVA: 0x00430E5A File Offset: 0x0042F05A
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

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600B3F8 RID: 46072 RVA: 0x00430E63 File Offset: 0x0042F063
		// (set) Token: 0x0600B3F9 RID: 46073 RVA: 0x00430E6B File Offset: 0x0042F06B
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

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x0600B3FA RID: 46074 RVA: 0x00430E74 File Offset: 0x0042F074
		// (set) Token: 0x0600B3FB RID: 46075 RVA: 0x00430E7C File Offset: 0x0042F07C
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

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x0600B3FC RID: 46076 RVA: 0x00430E85 File Offset: 0x0042F085
		// (set) Token: 0x0600B3FD RID: 46077 RVA: 0x00430E8D File Offset: 0x0042F08D
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

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x0600B3FE RID: 46078 RVA: 0x00430E96 File Offset: 0x0042F096
		// (set) Token: 0x0600B3FF RID: 46079 RVA: 0x00430E9E File Offset: 0x0042F09E
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

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x0600B400 RID: 46080 RVA: 0x00430EA7 File Offset: 0x0042F0A7
		// (set) Token: 0x0600B401 RID: 46081 RVA: 0x00430EAF File Offset: 0x0042F0AF
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

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x0600B402 RID: 46082 RVA: 0x00430EB8 File Offset: 0x0042F0B8
		// (set) Token: 0x0600B403 RID: 46083 RVA: 0x00430EC0 File Offset: 0x0042F0C0
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

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x0600B404 RID: 46084 RVA: 0x00430EC9 File Offset: 0x0042F0C9
		// (set) Token: 0x0600B405 RID: 46085 RVA: 0x00430ED1 File Offset: 0x0042F0D1
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

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x0600B406 RID: 46086 RVA: 0x00430EDA File Offset: 0x0042F0DA
		// (set) Token: 0x0600B407 RID: 46087 RVA: 0x00430EE2 File Offset: 0x0042F0E2
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

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x0600B408 RID: 46088 RVA: 0x00430EEB File Offset: 0x0042F0EB
		// (set) Token: 0x0600B409 RID: 46089 RVA: 0x00430EF3 File Offset: 0x0042F0F3
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

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600B40A RID: 46090 RVA: 0x00430EFC File Offset: 0x0042F0FC
		// (set) Token: 0x0600B40B RID: 46091 RVA: 0x00430F04 File Offset: 0x0042F104
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

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x0600B40C RID: 46092 RVA: 0x00430F0D File Offset: 0x0042F10D
		// (set) Token: 0x0600B40D RID: 46093 RVA: 0x00430F15 File Offset: 0x0042F115
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

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600B40E RID: 46094 RVA: 0x00430F1E File Offset: 0x0042F11E
		// (set) Token: 0x0600B40F RID: 46095 RVA: 0x00430F26 File Offset: 0x0042F126
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

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x0600B410 RID: 46096 RVA: 0x00430F2F File Offset: 0x0042F12F
		// (set) Token: 0x0600B411 RID: 46097 RVA: 0x00430F37 File Offset: 0x0042F137
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

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x0600B412 RID: 46098 RVA: 0x00430F40 File Offset: 0x0042F140
		// (set) Token: 0x0600B413 RID: 46099 RVA: 0x00430F48 File Offset: 0x0042F148
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

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x0600B414 RID: 46100 RVA: 0x00430F51 File Offset: 0x0042F151
		// (set) Token: 0x0600B415 RID: 46101 RVA: 0x00430F59 File Offset: 0x0042F159
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

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x0600B416 RID: 46102 RVA: 0x00430F62 File Offset: 0x0042F162
		// (set) Token: 0x0600B417 RID: 46103 RVA: 0x00430F6A File Offset: 0x0042F16A
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

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x0600B418 RID: 46104 RVA: 0x00430F73 File Offset: 0x0042F173
		// (set) Token: 0x0600B419 RID: 46105 RVA: 0x00430F7B File Offset: 0x0042F17B
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

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x0600B41A RID: 46106 RVA: 0x00430F84 File Offset: 0x0042F184
		// (set) Token: 0x0600B41B RID: 46107 RVA: 0x00430F8C File Offset: 0x0042F18C
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

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x0600B41C RID: 46108 RVA: 0x00430F95 File Offset: 0x0042F195
		// (set) Token: 0x0600B41D RID: 46109 RVA: 0x00430F9D File Offset: 0x0042F19D
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

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x0600B41E RID: 46110 RVA: 0x00430FA6 File Offset: 0x0042F1A6
		// (set) Token: 0x0600B41F RID: 46111 RVA: 0x00430FAE File Offset: 0x0042F1AE
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

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x0600B420 RID: 46112 RVA: 0x00430FB7 File Offset: 0x0042F1B7
		// (set) Token: 0x0600B421 RID: 46113 RVA: 0x00430FBF File Offset: 0x0042F1BF
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

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x0600B422 RID: 46114 RVA: 0x00430FC8 File Offset: 0x0042F1C8
		// (set) Token: 0x0600B423 RID: 46115 RVA: 0x00430FD0 File Offset: 0x0042F1D0
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

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x0600B424 RID: 46116 RVA: 0x00430FD9 File Offset: 0x0042F1D9
		// (set) Token: 0x0600B425 RID: 46117 RVA: 0x00430FE1 File Offset: 0x0042F1E1
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

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x0600B426 RID: 46118 RVA: 0x00430FEA File Offset: 0x0042F1EA
		// (set) Token: 0x0600B427 RID: 46119 RVA: 0x00430FF2 File Offset: 0x0042F1F2
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

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x0600B428 RID: 46120 RVA: 0x00430FFB File Offset: 0x0042F1FB
		// (set) Token: 0x0600B429 RID: 46121 RVA: 0x00431003 File Offset: 0x0042F203
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

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x0600B42A RID: 46122 RVA: 0x0043100C File Offset: 0x0042F20C
		// (set) Token: 0x0600B42B RID: 46123 RVA: 0x00431014 File Offset: 0x0042F214
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

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x0600B42C RID: 46124 RVA: 0x0043101D File Offset: 0x0042F21D
		// (set) Token: 0x0600B42D RID: 46125 RVA: 0x00431025 File Offset: 0x0042F225
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

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x0600B42E RID: 46126 RVA: 0x0043102E File Offset: 0x0042F22E
		// (set) Token: 0x0600B42F RID: 46127 RVA: 0x00431036 File Offset: 0x0042F236
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

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x0600B430 RID: 46128 RVA: 0x0043103F File Offset: 0x0042F23F
		// (set) Token: 0x0600B431 RID: 46129 RVA: 0x00431047 File Offset: 0x0042F247
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

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600B432 RID: 46130 RVA: 0x00431050 File Offset: 0x0042F250
		// (set) Token: 0x0600B433 RID: 46131 RVA: 0x00431058 File Offset: 0x0042F258
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

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x0600B434 RID: 46132 RVA: 0x00431061 File Offset: 0x0042F261
		// (set) Token: 0x0600B435 RID: 46133 RVA: 0x00431069 File Offset: 0x0042F269
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

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x0600B436 RID: 46134 RVA: 0x00431072 File Offset: 0x0042F272
		// (set) Token: 0x0600B437 RID: 46135 RVA: 0x0043107A File Offset: 0x0042F27A
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

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x0600B438 RID: 46136 RVA: 0x00431083 File Offset: 0x0042F283
		// (set) Token: 0x0600B439 RID: 46137 RVA: 0x0043108B File Offset: 0x0042F28B
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

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x0600B43A RID: 46138 RVA: 0x00431094 File Offset: 0x0042F294
		// (set) Token: 0x0600B43B RID: 46139 RVA: 0x0043109C File Offset: 0x0042F29C
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

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x0600B43C RID: 46140 RVA: 0x004310A5 File Offset: 0x0042F2A5
		// (set) Token: 0x0600B43D RID: 46141 RVA: 0x004310AD File Offset: 0x0042F2AD
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

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x0600B43E RID: 46142 RVA: 0x004310B6 File Offset: 0x0042F2B6
		// (set) Token: 0x0600B43F RID: 46143 RVA: 0x004310BE File Offset: 0x0042F2BE
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

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x0600B440 RID: 46144 RVA: 0x004310C7 File Offset: 0x0042F2C7
		// (set) Token: 0x0600B441 RID: 46145 RVA: 0x004310CF File Offset: 0x0042F2CF
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

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x0600B442 RID: 46146 RVA: 0x004310D8 File Offset: 0x0042F2D8
		// (set) Token: 0x0600B443 RID: 46147 RVA: 0x004310E0 File Offset: 0x0042F2E0
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

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x0600B444 RID: 46148 RVA: 0x004310E9 File Offset: 0x0042F2E9
		// (set) Token: 0x0600B445 RID: 46149 RVA: 0x004310F1 File Offset: 0x0042F2F1
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

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x0600B446 RID: 46150 RVA: 0x004310FA File Offset: 0x0042F2FA
		// (set) Token: 0x0600B447 RID: 46151 RVA: 0x00431102 File Offset: 0x0042F302
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

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x0600B448 RID: 46152 RVA: 0x0043110B File Offset: 0x0042F30B
		// (set) Token: 0x0600B449 RID: 46153 RVA: 0x00431113 File Offset: 0x0042F313
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

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x0600B44A RID: 46154 RVA: 0x0043111C File Offset: 0x0042F31C
		// (set) Token: 0x0600B44B RID: 46155 RVA: 0x00431124 File Offset: 0x0042F324
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

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x0600B44C RID: 46156 RVA: 0x0043112D File Offset: 0x0042F32D
		// (set) Token: 0x0600B44D RID: 46157 RVA: 0x00431135 File Offset: 0x0042F335
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

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x0600B44E RID: 46158 RVA: 0x0043113E File Offset: 0x0042F33E
		// (set) Token: 0x0600B44F RID: 46159 RVA: 0x00431146 File Offset: 0x0042F346
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

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x0600B450 RID: 46160 RVA: 0x0043114F File Offset: 0x0042F34F
		// (set) Token: 0x0600B451 RID: 46161 RVA: 0x00431157 File Offset: 0x0042F357
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

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600B452 RID: 46162 RVA: 0x00431160 File Offset: 0x0042F360
		// (set) Token: 0x0600B453 RID: 46163 RVA: 0x00431168 File Offset: 0x0042F368
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

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x0600B454 RID: 46164 RVA: 0x00431171 File Offset: 0x0042F371
		// (set) Token: 0x0600B455 RID: 46165 RVA: 0x00431179 File Offset: 0x0042F379
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

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x0600B456 RID: 46166 RVA: 0x00431182 File Offset: 0x0042F382
		// (set) Token: 0x0600B457 RID: 46167 RVA: 0x0043118A File Offset: 0x0042F38A
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

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x0600B458 RID: 46168 RVA: 0x00431193 File Offset: 0x0042F393
		// (set) Token: 0x0600B459 RID: 46169 RVA: 0x0043119B File Offset: 0x0042F39B
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

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x0600B45A RID: 46170 RVA: 0x004311A4 File Offset: 0x0042F3A4
		// (set) Token: 0x0600B45B RID: 46171 RVA: 0x004311AC File Offset: 0x0042F3AC
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

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x0600B45C RID: 46172 RVA: 0x004311B5 File Offset: 0x0042F3B5
		// (set) Token: 0x0600B45D RID: 46173 RVA: 0x004311BD File Offset: 0x0042F3BD
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

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x0600B45E RID: 46174 RVA: 0x004311C6 File Offset: 0x0042F3C6
		// (set) Token: 0x0600B45F RID: 46175 RVA: 0x004311CE File Offset: 0x0042F3CE
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

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x0600B460 RID: 46176 RVA: 0x004311D7 File Offset: 0x0042F3D7
		// (set) Token: 0x0600B461 RID: 46177 RVA: 0x004311DF File Offset: 0x0042F3DF
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

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x0600B462 RID: 46178 RVA: 0x004311E8 File Offset: 0x0042F3E8
		// (set) Token: 0x0600B463 RID: 46179 RVA: 0x004311F0 File Offset: 0x0042F3F0
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

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600B464 RID: 46180 RVA: 0x004311F9 File Offset: 0x0042F3F9
		// (set) Token: 0x0600B465 RID: 46181 RVA: 0x00431201 File Offset: 0x0042F401
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

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600B466 RID: 46182 RVA: 0x0043120A File Offset: 0x0042F40A
		// (set) Token: 0x0600B467 RID: 46183 RVA: 0x00431212 File Offset: 0x0042F412
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

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600B468 RID: 46184 RVA: 0x0043121B File Offset: 0x0042F41B
		// (set) Token: 0x0600B469 RID: 46185 RVA: 0x00431223 File Offset: 0x0042F423
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

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x0600B46A RID: 46186 RVA: 0x0043122C File Offset: 0x0042F42C
		// (set) Token: 0x0600B46B RID: 46187 RVA: 0x00431234 File Offset: 0x0042F434
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

		// Token: 0x0600B46C RID: 46188 RVA: 0x00431240 File Offset: 0x0042F440
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

		// Token: 0x040086E8 RID: 34536
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDelay;

		// Token: 0x040086E9 RID: 34537
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRange;

		// Token: 0x040086EA RID: 34538
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundStart;

		// Token: 0x040086EB RID: 34539
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundRepeat;

		// Token: 0x040086EC RID: 34540
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEnd;

		// Token: 0x040086ED RID: 34541
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEmpty;

		// Token: 0x040086EE RID: 34542
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundReload;

		// Token: 0x040086EF RID: 34543
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundWarning;

		// Token: 0x040086F0 RID: 34544
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pStaminaUsage;

		// Token: 0x040086F1 RID: 34545
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUseTime;

		// Token: 0x040086F2 RID: 34546
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname1;

		// Token: 0x040086F3 RID: 34547
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname2;

		// Token: 0x040086F4 RID: 34548
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname3;

		// Token: 0x040086F5 RID: 34549
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname4;

		// Token: 0x040086F6 RID: 34550
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname5;

		// Token: 0x040086F7 RID: 34551
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname6;

		// Token: 0x040086F8 RID: 34552
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname7;

		// Token: 0x040086F9 RID: 34553
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname8;

		// Token: 0x040086FA RID: 34554
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname9;

		// Token: 0x040086FB RID: 34555
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeItemTo;

		// Token: 0x040086FC RID: 34556
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeBlockTo;

		// Token: 0x040086FD RID: 34557
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDoBlockAction;

		// Token: 0x040086FE RID: 34558
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainHealth;

		// Token: 0x040086FF RID: 34559
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainFood;

		// Token: 0x04008700 RID: 34560
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWater;

		// Token: 0x04008701 RID: 34561
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainStamina;

		// Token: 0x04008702 RID: 34562
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainSickness;

		// Token: 0x04008703 RID: 34563
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWellness;

		// Token: 0x04008704 RID: 34564
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuff;

		// Token: 0x04008705 RID: 34565
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuffChance;

		// Token: 0x04008706 RID: 34566
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDebuff;

		// Token: 0x04008707 RID: 34567
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pCreateItem;

		// Token: 0x04008708 RID: 34568
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pConditionRaycastBlock;

		// Token: 0x04008709 RID: 34569
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pGainGas;

		// Token: 0x0400870A RID: 34570
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pConsume;

		// Token: 0x0400870B RID: 34571
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockname;

		// Token: 0x0400870C RID: 34572
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthDefault;

		// Token: 0x0400870D RID: 34573
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthMax;

		// Token: 0x0400870E RID: 34574
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMaxStrainTime;

		// Token: 0x0400870F RID: 34575
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pMagazineSize;

		// Token: 0x04008710 RID: 34576
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMagazineItem;

		// Token: 0x04008711 RID: 34577
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pReloadTime;

		// Token: 0x04008712 RID: 34578
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletIcon;

		// Token: 0x04008713 RID: 34579
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRaysPerShot;

		// Token: 0x04008714 RID: 34580
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRaysSpread;

		// Token: 0x04008715 RID: 34581
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSphere;

		// Token: 0x04008716 RID: 34582
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMinDistance;

		// Token: 0x04008717 RID: 34583
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMaxDistance;

		// Token: 0x04008718 RID: 34584
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pDamageEntity;

		// Token: 0x04008719 RID: 34585
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDamageBlock;

		// Token: 0x0400871A RID: 34586
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleFire;

		// Token: 0x0400871B RID: 34587
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleSmoke;

		// Token: 0x0400871C RID: 34588
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlockRange;

		// Token: 0x0400871D RID: 34589
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pAutoFire;

		// Token: 0x0400871E RID: 34590
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterRate;

		// Token: 0x0400871F RID: 34591
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterDistance;

		// Token: 0x04008720 RID: 34592
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pHitmaskOverride;

		// Token: 0x04008721 RID: 34593
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pSingleMagazineUsage;

		// Token: 0x04008722 RID: 34594
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletMaterial;

		// Token: 0x04008723 RID: 34595
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pInfiniteAmmo;

		// Token: 0x04008724 RID: 34596
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxOut;

		// Token: 0x04008725 RID: 34597
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxIn;

		// Token: 0x04008726 RID: 34598
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pZoomOverlay;

		// Token: 0x04008727 RID: 34599
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pVelocity;

		// Token: 0x04008728 RID: 34600
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pFlyTime;

		// Token: 0x04008729 RID: 34601
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLifeTime;

		// Token: 0x0400872A RID: 34602
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCollisionRadius;

		// Token: 0x0400872B RID: 34603
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pProjectileInitialVelocity;

		// Token: 0x0400872C RID: 34604
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFertileblock;

		// Token: 0x0400872D RID: 34605
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAdjacentblock;

		// Token: 0x0400872E RID: 34606
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRepairAmount;

		// Token: 0x0400872F RID: 34607
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pUpgradeHitOffset;

		// Token: 0x04008730 RID: 34608
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAllowedUpgradeItems;

		// Token: 0x04008731 RID: 34609
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRestrictedUpgradeItems;

		// Token: 0x04008732 RID: 34610
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUpgradeActionSound;

		// Token: 0x04008733 RID: 34611
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRepairActionSound;

		// Token: 0x04008734 RID: 34612
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pReferenceItem;

		// Token: 0x04008735 RID: 34613
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x04008736 RID: 34614
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pActionIdx;

		// Token: 0x04008737 RID: 34615
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pTitle;

		// Token: 0x04008738 RID: 34616
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x04008739 RID: 34617
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRecipesToLearn;

		// Token: 0x0400873A RID: 34618
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pInstantiateOnLoad;

		// Token: 0x0400873B RID: 34619
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundDraw;

		// Token: 0x0400873C RID: 34620
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<DamageBonusData> pDamageBonus;

		// Token: 0x0400873D RID: 34621
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ExplosionData> pExplosion;

		// Token: 0x02001675 RID: 5749
		public static class Parser
		{
			// Token: 0x0600B46E RID: 46190 RVA: 0x004312A4 File Offset: 0x0042F4A4
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

			// Token: 0x0600B46F RID: 46191 RVA: 0x00431328 File Offset: 0x0042F528
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

			// Token: 0x0400873E RID: 34622
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
