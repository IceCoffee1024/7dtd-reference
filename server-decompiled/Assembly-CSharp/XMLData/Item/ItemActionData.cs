using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001628 RID: 5672
	[Preserve]
	public class ItemActionData : IXMLData
	{
		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x0600B1D9 RID: 45529 RVA: 0x00425867 File Offset: 0x00423A67
		// (set) Token: 0x0600B1DA RID: 45530 RVA: 0x0042586F File Offset: 0x00423A6F
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

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x0600B1DB RID: 45531 RVA: 0x00425878 File Offset: 0x00423A78
		// (set) Token: 0x0600B1DC RID: 45532 RVA: 0x00425880 File Offset: 0x00423A80
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

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x0600B1DD RID: 45533 RVA: 0x00425889 File Offset: 0x00423A89
		// (set) Token: 0x0600B1DE RID: 45534 RVA: 0x00425891 File Offset: 0x00423A91
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

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x0600B1DF RID: 45535 RVA: 0x0042589A File Offset: 0x00423A9A
		// (set) Token: 0x0600B1E0 RID: 45536 RVA: 0x004258A2 File Offset: 0x00423AA2
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

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x0600B1E1 RID: 45537 RVA: 0x004258AB File Offset: 0x00423AAB
		// (set) Token: 0x0600B1E2 RID: 45538 RVA: 0x004258B3 File Offset: 0x00423AB3
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

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x0600B1E3 RID: 45539 RVA: 0x004258BC File Offset: 0x00423ABC
		// (set) Token: 0x0600B1E4 RID: 45540 RVA: 0x004258C4 File Offset: 0x00423AC4
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

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x0600B1E5 RID: 45541 RVA: 0x004258CD File Offset: 0x00423ACD
		// (set) Token: 0x0600B1E6 RID: 45542 RVA: 0x004258D5 File Offset: 0x00423AD5
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

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x0600B1E7 RID: 45543 RVA: 0x004258DE File Offset: 0x00423ADE
		// (set) Token: 0x0600B1E8 RID: 45544 RVA: 0x004258E6 File Offset: 0x00423AE6
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

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x0600B1E9 RID: 45545 RVA: 0x004258EF File Offset: 0x00423AEF
		// (set) Token: 0x0600B1EA RID: 45546 RVA: 0x004258F7 File Offset: 0x00423AF7
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

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x0600B1EB RID: 45547 RVA: 0x00425900 File Offset: 0x00423B00
		// (set) Token: 0x0600B1EC RID: 45548 RVA: 0x00425908 File Offset: 0x00423B08
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

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x0600B1ED RID: 45549 RVA: 0x00425911 File Offset: 0x00423B11
		// (set) Token: 0x0600B1EE RID: 45550 RVA: 0x00425919 File Offset: 0x00423B19
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

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x0600B1EF RID: 45551 RVA: 0x00425922 File Offset: 0x00423B22
		// (set) Token: 0x0600B1F0 RID: 45552 RVA: 0x0042592A File Offset: 0x00423B2A
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

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x0600B1F1 RID: 45553 RVA: 0x00425933 File Offset: 0x00423B33
		// (set) Token: 0x0600B1F2 RID: 45554 RVA: 0x0042593B File Offset: 0x00423B3B
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

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x0600B1F3 RID: 45555 RVA: 0x00425944 File Offset: 0x00423B44
		// (set) Token: 0x0600B1F4 RID: 45556 RVA: 0x0042594C File Offset: 0x00423B4C
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

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x0600B1F5 RID: 45557 RVA: 0x00425955 File Offset: 0x00423B55
		// (set) Token: 0x0600B1F6 RID: 45558 RVA: 0x0042595D File Offset: 0x00423B5D
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

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x0600B1F7 RID: 45559 RVA: 0x00425966 File Offset: 0x00423B66
		// (set) Token: 0x0600B1F8 RID: 45560 RVA: 0x0042596E File Offset: 0x00423B6E
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

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x0600B1F9 RID: 45561 RVA: 0x00425977 File Offset: 0x00423B77
		// (set) Token: 0x0600B1FA RID: 45562 RVA: 0x0042597F File Offset: 0x00423B7F
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

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x0600B1FB RID: 45563 RVA: 0x00425988 File Offset: 0x00423B88
		// (set) Token: 0x0600B1FC RID: 45564 RVA: 0x00425990 File Offset: 0x00423B90
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

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x0600B1FD RID: 45565 RVA: 0x00425999 File Offset: 0x00423B99
		// (set) Token: 0x0600B1FE RID: 45566 RVA: 0x004259A1 File Offset: 0x00423BA1
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

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x0600B1FF RID: 45567 RVA: 0x004259AA File Offset: 0x00423BAA
		// (set) Token: 0x0600B200 RID: 45568 RVA: 0x004259B2 File Offset: 0x00423BB2
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

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x0600B201 RID: 45569 RVA: 0x004259BB File Offset: 0x00423BBB
		// (set) Token: 0x0600B202 RID: 45570 RVA: 0x004259C3 File Offset: 0x00423BC3
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

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x0600B203 RID: 45571 RVA: 0x004259CC File Offset: 0x00423BCC
		// (set) Token: 0x0600B204 RID: 45572 RVA: 0x004259D4 File Offset: 0x00423BD4
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

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x0600B205 RID: 45573 RVA: 0x004259DD File Offset: 0x00423BDD
		// (set) Token: 0x0600B206 RID: 45574 RVA: 0x004259E5 File Offset: 0x00423BE5
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

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600B207 RID: 45575 RVA: 0x004259EE File Offset: 0x00423BEE
		// (set) Token: 0x0600B208 RID: 45576 RVA: 0x004259F6 File Offset: 0x00423BF6
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

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x0600B209 RID: 45577 RVA: 0x004259FF File Offset: 0x00423BFF
		// (set) Token: 0x0600B20A RID: 45578 RVA: 0x00425A07 File Offset: 0x00423C07
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

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600B20B RID: 45579 RVA: 0x00425A10 File Offset: 0x00423C10
		// (set) Token: 0x0600B20C RID: 45580 RVA: 0x00425A18 File Offset: 0x00423C18
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

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600B20D RID: 45581 RVA: 0x00425A21 File Offset: 0x00423C21
		// (set) Token: 0x0600B20E RID: 45582 RVA: 0x00425A29 File Offset: 0x00423C29
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

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600B20F RID: 45583 RVA: 0x00425A32 File Offset: 0x00423C32
		// (set) Token: 0x0600B210 RID: 45584 RVA: 0x00425A3A File Offset: 0x00423C3A
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

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600B211 RID: 45585 RVA: 0x00425A43 File Offset: 0x00423C43
		// (set) Token: 0x0600B212 RID: 45586 RVA: 0x00425A4B File Offset: 0x00423C4B
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

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x0600B213 RID: 45587 RVA: 0x00425A54 File Offset: 0x00423C54
		// (set) Token: 0x0600B214 RID: 45588 RVA: 0x00425A5C File Offset: 0x00423C5C
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

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x0600B215 RID: 45589 RVA: 0x00425A65 File Offset: 0x00423C65
		// (set) Token: 0x0600B216 RID: 45590 RVA: 0x00425A6D File Offset: 0x00423C6D
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

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x0600B217 RID: 45591 RVA: 0x00425A76 File Offset: 0x00423C76
		// (set) Token: 0x0600B218 RID: 45592 RVA: 0x00425A7E File Offset: 0x00423C7E
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

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x0600B219 RID: 45593 RVA: 0x00425A87 File Offset: 0x00423C87
		// (set) Token: 0x0600B21A RID: 45594 RVA: 0x00425A8F File Offset: 0x00423C8F
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

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x0600B21B RID: 45595 RVA: 0x00425A98 File Offset: 0x00423C98
		// (set) Token: 0x0600B21C RID: 45596 RVA: 0x00425AA0 File Offset: 0x00423CA0
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

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x0600B21D RID: 45597 RVA: 0x00425AA9 File Offset: 0x00423CA9
		// (set) Token: 0x0600B21E RID: 45598 RVA: 0x00425AB1 File Offset: 0x00423CB1
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

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x0600B21F RID: 45599 RVA: 0x00425ABA File Offset: 0x00423CBA
		// (set) Token: 0x0600B220 RID: 45600 RVA: 0x00425AC2 File Offset: 0x00423CC2
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

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x0600B221 RID: 45601 RVA: 0x00425ACB File Offset: 0x00423CCB
		// (set) Token: 0x0600B222 RID: 45602 RVA: 0x00425AD3 File Offset: 0x00423CD3
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

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x0600B223 RID: 45603 RVA: 0x00425ADC File Offset: 0x00423CDC
		// (set) Token: 0x0600B224 RID: 45604 RVA: 0x00425AE4 File Offset: 0x00423CE4
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

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x0600B225 RID: 45605 RVA: 0x00425AED File Offset: 0x00423CED
		// (set) Token: 0x0600B226 RID: 45606 RVA: 0x00425AF5 File Offset: 0x00423CF5
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

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x0600B227 RID: 45607 RVA: 0x00425AFE File Offset: 0x00423CFE
		// (set) Token: 0x0600B228 RID: 45608 RVA: 0x00425B06 File Offset: 0x00423D06
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

		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x0600B229 RID: 45609 RVA: 0x00425B0F File Offset: 0x00423D0F
		// (set) Token: 0x0600B22A RID: 45610 RVA: 0x00425B17 File Offset: 0x00423D17
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

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x0600B22B RID: 45611 RVA: 0x00425B20 File Offset: 0x00423D20
		// (set) Token: 0x0600B22C RID: 45612 RVA: 0x00425B28 File Offset: 0x00423D28
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

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x0600B22D RID: 45613 RVA: 0x00425B31 File Offset: 0x00423D31
		// (set) Token: 0x0600B22E RID: 45614 RVA: 0x00425B39 File Offset: 0x00423D39
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

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x0600B22F RID: 45615 RVA: 0x00425B42 File Offset: 0x00423D42
		// (set) Token: 0x0600B230 RID: 45616 RVA: 0x00425B4A File Offset: 0x00423D4A
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

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x0600B231 RID: 45617 RVA: 0x00425B53 File Offset: 0x00423D53
		// (set) Token: 0x0600B232 RID: 45618 RVA: 0x00425B5B File Offset: 0x00423D5B
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

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x0600B233 RID: 45619 RVA: 0x00425B64 File Offset: 0x00423D64
		// (set) Token: 0x0600B234 RID: 45620 RVA: 0x00425B6C File Offset: 0x00423D6C
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

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x0600B235 RID: 45621 RVA: 0x00425B75 File Offset: 0x00423D75
		// (set) Token: 0x0600B236 RID: 45622 RVA: 0x00425B7D File Offset: 0x00423D7D
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

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x0600B237 RID: 45623 RVA: 0x00425B86 File Offset: 0x00423D86
		// (set) Token: 0x0600B238 RID: 45624 RVA: 0x00425B8E File Offset: 0x00423D8E
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

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x0600B239 RID: 45625 RVA: 0x00425B97 File Offset: 0x00423D97
		// (set) Token: 0x0600B23A RID: 45626 RVA: 0x00425B9F File Offset: 0x00423D9F
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

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x0600B23B RID: 45627 RVA: 0x00425BA8 File Offset: 0x00423DA8
		// (set) Token: 0x0600B23C RID: 45628 RVA: 0x00425BB0 File Offset: 0x00423DB0
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

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x0600B23D RID: 45629 RVA: 0x00425BB9 File Offset: 0x00423DB9
		// (set) Token: 0x0600B23E RID: 45630 RVA: 0x00425BC1 File Offset: 0x00423DC1
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

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x0600B23F RID: 45631 RVA: 0x00425BCA File Offset: 0x00423DCA
		// (set) Token: 0x0600B240 RID: 45632 RVA: 0x00425BD2 File Offset: 0x00423DD2
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

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x0600B241 RID: 45633 RVA: 0x00425BDB File Offset: 0x00423DDB
		// (set) Token: 0x0600B242 RID: 45634 RVA: 0x00425BE3 File Offset: 0x00423DE3
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

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x0600B243 RID: 45635 RVA: 0x00425BEC File Offset: 0x00423DEC
		// (set) Token: 0x0600B244 RID: 45636 RVA: 0x00425BF4 File Offset: 0x00423DF4
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

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x0600B245 RID: 45637 RVA: 0x00425BFD File Offset: 0x00423DFD
		// (set) Token: 0x0600B246 RID: 45638 RVA: 0x00425C05 File Offset: 0x00423E05
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

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x0600B247 RID: 45639 RVA: 0x00425C0E File Offset: 0x00423E0E
		// (set) Token: 0x0600B248 RID: 45640 RVA: 0x00425C16 File Offset: 0x00423E16
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

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x0600B249 RID: 45641 RVA: 0x00425C1F File Offset: 0x00423E1F
		// (set) Token: 0x0600B24A RID: 45642 RVA: 0x00425C27 File Offset: 0x00423E27
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

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x0600B24B RID: 45643 RVA: 0x00425C30 File Offset: 0x00423E30
		// (set) Token: 0x0600B24C RID: 45644 RVA: 0x00425C38 File Offset: 0x00423E38
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

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x0600B24D RID: 45645 RVA: 0x00425C41 File Offset: 0x00423E41
		// (set) Token: 0x0600B24E RID: 45646 RVA: 0x00425C49 File Offset: 0x00423E49
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

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x0600B24F RID: 45647 RVA: 0x00425C52 File Offset: 0x00423E52
		// (set) Token: 0x0600B250 RID: 45648 RVA: 0x00425C5A File Offset: 0x00423E5A
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

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x0600B251 RID: 45649 RVA: 0x00425C63 File Offset: 0x00423E63
		// (set) Token: 0x0600B252 RID: 45650 RVA: 0x00425C6B File Offset: 0x00423E6B
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

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x0600B253 RID: 45651 RVA: 0x00425C74 File Offset: 0x00423E74
		// (set) Token: 0x0600B254 RID: 45652 RVA: 0x00425C7C File Offset: 0x00423E7C
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

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x0600B255 RID: 45653 RVA: 0x00425C85 File Offset: 0x00423E85
		// (set) Token: 0x0600B256 RID: 45654 RVA: 0x00425C8D File Offset: 0x00423E8D
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

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x0600B257 RID: 45655 RVA: 0x00425C96 File Offset: 0x00423E96
		// (set) Token: 0x0600B258 RID: 45656 RVA: 0x00425C9E File Offset: 0x00423E9E
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

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x0600B259 RID: 45657 RVA: 0x00425CA7 File Offset: 0x00423EA7
		// (set) Token: 0x0600B25A RID: 45658 RVA: 0x00425CAF File Offset: 0x00423EAF
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

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x0600B25B RID: 45659 RVA: 0x00425CB8 File Offset: 0x00423EB8
		// (set) Token: 0x0600B25C RID: 45660 RVA: 0x00425CC0 File Offset: 0x00423EC0
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

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x0600B25D RID: 45661 RVA: 0x00425CC9 File Offset: 0x00423EC9
		// (set) Token: 0x0600B25E RID: 45662 RVA: 0x00425CD1 File Offset: 0x00423ED1
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

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x0600B25F RID: 45663 RVA: 0x00425CDA File Offset: 0x00423EDA
		// (set) Token: 0x0600B260 RID: 45664 RVA: 0x00425CE2 File Offset: 0x00423EE2
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

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x0600B261 RID: 45665 RVA: 0x00425CEB File Offset: 0x00423EEB
		// (set) Token: 0x0600B262 RID: 45666 RVA: 0x00425CF3 File Offset: 0x00423EF3
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

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x0600B263 RID: 45667 RVA: 0x00425CFC File Offset: 0x00423EFC
		// (set) Token: 0x0600B264 RID: 45668 RVA: 0x00425D04 File Offset: 0x00423F04
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

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x0600B265 RID: 45669 RVA: 0x00425D0D File Offset: 0x00423F0D
		// (set) Token: 0x0600B266 RID: 45670 RVA: 0x00425D15 File Offset: 0x00423F15
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

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x0600B267 RID: 45671 RVA: 0x00425D1E File Offset: 0x00423F1E
		// (set) Token: 0x0600B268 RID: 45672 RVA: 0x00425D26 File Offset: 0x00423F26
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

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x0600B269 RID: 45673 RVA: 0x00425D2F File Offset: 0x00423F2F
		// (set) Token: 0x0600B26A RID: 45674 RVA: 0x00425D37 File Offset: 0x00423F37
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

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600B26B RID: 45675 RVA: 0x00425D40 File Offset: 0x00423F40
		// (set) Token: 0x0600B26C RID: 45676 RVA: 0x00425D48 File Offset: 0x00423F48
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

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x0600B26D RID: 45677 RVA: 0x00425D51 File Offset: 0x00423F51
		// (set) Token: 0x0600B26E RID: 45678 RVA: 0x00425D59 File Offset: 0x00423F59
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

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x0600B26F RID: 45679 RVA: 0x00425D62 File Offset: 0x00423F62
		// (set) Token: 0x0600B270 RID: 45680 RVA: 0x00425D6A File Offset: 0x00423F6A
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

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x0600B271 RID: 45681 RVA: 0x00425D73 File Offset: 0x00423F73
		// (set) Token: 0x0600B272 RID: 45682 RVA: 0x00425D7B File Offset: 0x00423F7B
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

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600B273 RID: 45683 RVA: 0x00425D84 File Offset: 0x00423F84
		// (set) Token: 0x0600B274 RID: 45684 RVA: 0x00425D8C File Offset: 0x00423F8C
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

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600B275 RID: 45685 RVA: 0x00425D95 File Offset: 0x00423F95
		// (set) Token: 0x0600B276 RID: 45686 RVA: 0x00425D9D File Offset: 0x00423F9D
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

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600B277 RID: 45687 RVA: 0x00425DA6 File Offset: 0x00423FA6
		// (set) Token: 0x0600B278 RID: 45688 RVA: 0x00425DAE File Offset: 0x00423FAE
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

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600B279 RID: 45689 RVA: 0x00425DB7 File Offset: 0x00423FB7
		// (set) Token: 0x0600B27A RID: 45690 RVA: 0x00425DBF File Offset: 0x00423FBF
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

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600B27B RID: 45691 RVA: 0x00425DC8 File Offset: 0x00423FC8
		// (set) Token: 0x0600B27C RID: 45692 RVA: 0x00425DD0 File Offset: 0x00423FD0
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

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x0600B27D RID: 45693 RVA: 0x00425DD9 File Offset: 0x00423FD9
		// (set) Token: 0x0600B27E RID: 45694 RVA: 0x00425DE1 File Offset: 0x00423FE1
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

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x0600B27F RID: 45695 RVA: 0x00425DEA File Offset: 0x00423FEA
		// (set) Token: 0x0600B280 RID: 45696 RVA: 0x00425DF2 File Offset: 0x00423FF2
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

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x0600B281 RID: 45697 RVA: 0x00425DFB File Offset: 0x00423FFB
		// (set) Token: 0x0600B282 RID: 45698 RVA: 0x00425E03 File Offset: 0x00424003
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

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x0600B283 RID: 45699 RVA: 0x00425E0C File Offset: 0x0042400C
		// (set) Token: 0x0600B284 RID: 45700 RVA: 0x00425E14 File Offset: 0x00424014
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

		// Token: 0x0600B285 RID: 45701 RVA: 0x00425E20 File Offset: 0x00424020
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

		// Token: 0x04008578 RID: 34168
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDelay;

		// Token: 0x04008579 RID: 34169
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRange;

		// Token: 0x0400857A RID: 34170
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundStart;

		// Token: 0x0400857B RID: 34171
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundRepeat;

		// Token: 0x0400857C RID: 34172
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEnd;

		// Token: 0x0400857D RID: 34173
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundEmpty;

		// Token: 0x0400857E RID: 34174
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundReload;

		// Token: 0x0400857F RID: 34175
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundWarning;

		// Token: 0x04008580 RID: 34176
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pStaminaUsage;

		// Token: 0x04008581 RID: 34177
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUseTime;

		// Token: 0x04008582 RID: 34178
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname1;

		// Token: 0x04008583 RID: 34179
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname2;

		// Token: 0x04008584 RID: 34180
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname3;

		// Token: 0x04008585 RID: 34181
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname4;

		// Token: 0x04008586 RID: 34182
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname5;

		// Token: 0x04008587 RID: 34183
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname6;

		// Token: 0x04008588 RID: 34184
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname7;

		// Token: 0x04008589 RID: 34185
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname8;

		// Token: 0x0400858A RID: 34186
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFocusedBlockname9;

		// Token: 0x0400858B RID: 34187
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeItemTo;

		// Token: 0x0400858C RID: 34188
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pChangeBlockTo;

		// Token: 0x0400858D RID: 34189
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDoBlockAction;

		// Token: 0x0400858E RID: 34190
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainHealth;

		// Token: 0x0400858F RID: 34191
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainFood;

		// Token: 0x04008590 RID: 34192
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWater;

		// Token: 0x04008591 RID: 34193
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainStamina;

		// Token: 0x04008592 RID: 34194
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainSickness;

		// Token: 0x04008593 RID: 34195
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGainWellness;

		// Token: 0x04008594 RID: 34196
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuff;

		// Token: 0x04008595 RID: 34197
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBuffChance;

		// Token: 0x04008596 RID: 34198
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDebuff;

		// Token: 0x04008597 RID: 34199
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pCreateItem;

		// Token: 0x04008598 RID: 34200
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pConditionRaycastBlock;

		// Token: 0x04008599 RID: 34201
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pGainGas;

		// Token: 0x0400859A RID: 34202
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pConsume;

		// Token: 0x0400859B RID: 34203
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockname;

		// Token: 0x0400859C RID: 34204
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthDefault;

		// Token: 0x0400859D RID: 34205
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pThrowStrengthMax;

		// Token: 0x0400859E RID: 34206
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMaxStrainTime;

		// Token: 0x0400859F RID: 34207
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pMagazineSize;

		// Token: 0x040085A0 RID: 34208
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMagazineItem;

		// Token: 0x040085A1 RID: 34209
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pReloadTime;

		// Token: 0x040085A2 RID: 34210
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletIcon;

		// Token: 0x040085A3 RID: 34211
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRaysPerShot;

		// Token: 0x040085A4 RID: 34212
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pRaysSpread;

		// Token: 0x040085A5 RID: 34213
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSphere;

		// Token: 0x040085A6 RID: 34214
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMinDistance;

		// Token: 0x040085A7 RID: 34215
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pCrosshairMaxDistance;

		// Token: 0x040085A8 RID: 34216
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pDamageEntity;

		// Token: 0x040085A9 RID: 34217
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pDamageBlock;

		// Token: 0x040085AA RID: 34218
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleFire;

		// Token: 0x040085AB RID: 34219
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pParticlesMuzzleSmoke;

		// Token: 0x040085AC RID: 34220
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlockRange;

		// Token: 0x040085AD RID: 34221
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pAutoFire;

		// Token: 0x040085AE RID: 34222
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterRate;

		// Token: 0x040085AF RID: 34223
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHordeMeterDistance;

		// Token: 0x040085B0 RID: 34224
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pHitmaskOverride;

		// Token: 0x040085B1 RID: 34225
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pSingleMagazineUsage;

		// Token: 0x040085B2 RID: 34226
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBulletMaterial;

		// Token: 0x040085B3 RID: 34227
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pInfiniteAmmo;

		// Token: 0x040085B4 RID: 34228
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxOut;

		// Token: 0x040085B5 RID: 34229
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pZoomMaxIn;

		// Token: 0x040085B6 RID: 34230
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pZoomOverlay;

		// Token: 0x040085B7 RID: 34231
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pVelocity;

		// Token: 0x040085B8 RID: 34232
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pFlyTime;

		// Token: 0x040085B9 RID: 34233
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLifeTime;

		// Token: 0x040085BA RID: 34234
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCollisionRadius;

		// Token: 0x040085BB RID: 34235
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pProjectileInitialVelocity;

		// Token: 0x040085BC RID: 34236
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFertileblock;

		// Token: 0x040085BD RID: 34237
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAdjacentblock;

		// Token: 0x040085BE RID: 34238
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRepairAmount;

		// Token: 0x040085BF RID: 34239
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pUpgradeHitOffset;

		// Token: 0x040085C0 RID: 34240
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAllowedUpgradeItems;

		// Token: 0x040085C1 RID: 34241
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRestrictedUpgradeItems;

		// Token: 0x040085C2 RID: 34242
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUpgradeActionSound;

		// Token: 0x040085C3 RID: 34243
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRepairActionSound;

		// Token: 0x040085C4 RID: 34244
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pReferenceItem;

		// Token: 0x040085C5 RID: 34245
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x040085C6 RID: 34246
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pActionIdx;

		// Token: 0x040085C7 RID: 34247
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pTitle;

		// Token: 0x040085C8 RID: 34248
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x040085C9 RID: 34249
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pRecipesToLearn;

		// Token: 0x040085CA RID: 34250
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pInstantiateOnLoad;

		// Token: 0x040085CB RID: 34251
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pSoundDraw;

		// Token: 0x040085CC RID: 34252
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<DamageBonusData> pDamageBonus;

		// Token: 0x040085CD RID: 34253
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ExplosionData> pExplosion;

		// Token: 0x02001629 RID: 5673
		public static class Parser
		{
			// Token: 0x0600B287 RID: 45703 RVA: 0x00425E84 File Offset: 0x00424084
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

			// Token: 0x0600B288 RID: 45704 RVA: 0x00425F08 File Offset: 0x00424108
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

			// Token: 0x040085CE RID: 34254
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
