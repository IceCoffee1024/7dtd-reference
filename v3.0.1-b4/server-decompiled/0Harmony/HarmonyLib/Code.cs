using System;
using System.ComponentModel;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200004B RID: 75
	public static class Code
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000B232 File Offset: 0x00009432
		public static Code.Operand_ Operand
		{
			get
			{
				return new Code.Operand_();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000B239 File Offset: 0x00009439
		public static Code.Nop_ Nop
		{
			get
			{
				return new Code.Nop_
				{
					opcode = OpCodes.Nop
				};
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000B24B File Offset: 0x0000944B
		public static Code.Break_ Break
		{
			get
			{
				return new Code.Break_
				{
					opcode = OpCodes.Break
				};
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000B25D File Offset: 0x0000945D
		public static Code.Ldarg_0_ Ldarg_0
		{
			get
			{
				return new Code.Ldarg_0_
				{
					opcode = OpCodes.Ldarg_0
				};
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000B26F File Offset: 0x0000946F
		public static Code.Ldarg_1_ Ldarg_1
		{
			get
			{
				return new Code.Ldarg_1_
				{
					opcode = OpCodes.Ldarg_1
				};
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000B281 File Offset: 0x00009481
		public static Code.Ldarg_2_ Ldarg_2
		{
			get
			{
				return new Code.Ldarg_2_
				{
					opcode = OpCodes.Ldarg_2
				};
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000B293 File Offset: 0x00009493
		public static Code.Ldarg_3_ Ldarg_3
		{
			get
			{
				return new Code.Ldarg_3_
				{
					opcode = OpCodes.Ldarg_3
				};
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000B2A5 File Offset: 0x000094A5
		public static Code.Ldloc_0_ Ldloc_0
		{
			get
			{
				return new Code.Ldloc_0_
				{
					opcode = OpCodes.Ldloc_0
				};
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000B2B7 File Offset: 0x000094B7
		public static Code.Ldloc_1_ Ldloc_1
		{
			get
			{
				return new Code.Ldloc_1_
				{
					opcode = OpCodes.Ldloc_1
				};
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000B2C9 File Offset: 0x000094C9
		public static Code.Ldloc_2_ Ldloc_2
		{
			get
			{
				return new Code.Ldloc_2_
				{
					opcode = OpCodes.Ldloc_2
				};
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000B2DB File Offset: 0x000094DB
		public static Code.Ldloc_3_ Ldloc_3
		{
			get
			{
				return new Code.Ldloc_3_
				{
					opcode = OpCodes.Ldloc_3
				};
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000B2ED File Offset: 0x000094ED
		public static Code.Stloc_0_ Stloc_0
		{
			get
			{
				return new Code.Stloc_0_
				{
					opcode = OpCodes.Stloc_0
				};
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000B2FF File Offset: 0x000094FF
		public static Code.Stloc_1_ Stloc_1
		{
			get
			{
				return new Code.Stloc_1_
				{
					opcode = OpCodes.Stloc_1
				};
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000B311 File Offset: 0x00009511
		public static Code.Stloc_2_ Stloc_2
		{
			get
			{
				return new Code.Stloc_2_
				{
					opcode = OpCodes.Stloc_2
				};
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000B323 File Offset: 0x00009523
		public static Code.Stloc_3_ Stloc_3
		{
			get
			{
				return new Code.Stloc_3_
				{
					opcode = OpCodes.Stloc_3
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000B335 File Offset: 0x00009535
		public static Code.Ldarg_S_ Ldarg_S
		{
			get
			{
				return new Code.Ldarg_S_
				{
					opcode = OpCodes.Ldarg_S
				};
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000B347 File Offset: 0x00009547
		public static Code.Ldarga_S_ Ldarga_S
		{
			get
			{
				return new Code.Ldarga_S_
				{
					opcode = OpCodes.Ldarga_S
				};
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000B359 File Offset: 0x00009559
		public static Code.Starg_S_ Starg_S
		{
			get
			{
				return new Code.Starg_S_
				{
					opcode = OpCodes.Starg_S
				};
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000B36B File Offset: 0x0000956B
		public static Code.Ldloc_S_ Ldloc_S
		{
			get
			{
				return new Code.Ldloc_S_
				{
					opcode = OpCodes.Ldloc_S
				};
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000B37D File Offset: 0x0000957D
		public static Code.Ldloca_S_ Ldloca_S
		{
			get
			{
				return new Code.Ldloca_S_
				{
					opcode = OpCodes.Ldloca_S
				};
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000B38F File Offset: 0x0000958F
		public static Code.Stloc_S_ Stloc_S
		{
			get
			{
				return new Code.Stloc_S_
				{
					opcode = OpCodes.Stloc_S
				};
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000B3A1 File Offset: 0x000095A1
		public static Code.Ldnull_ Ldnull
		{
			get
			{
				return new Code.Ldnull_
				{
					opcode = OpCodes.Ldnull
				};
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000B3B3 File Offset: 0x000095B3
		public static Code.Ldc_I4_M1_ Ldc_I4_M1
		{
			get
			{
				return new Code.Ldc_I4_M1_
				{
					opcode = OpCodes.Ldc_I4_M1
				};
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000B3C5 File Offset: 0x000095C5
		public static Code.Ldc_I4_0_ Ldc_I4_0
		{
			get
			{
				return new Code.Ldc_I4_0_
				{
					opcode = OpCodes.Ldc_I4_0
				};
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000B3D7 File Offset: 0x000095D7
		public static Code.Ldc_I4_1_ Ldc_I4_1
		{
			get
			{
				return new Code.Ldc_I4_1_
				{
					opcode = OpCodes.Ldc_I4_1
				};
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000B3E9 File Offset: 0x000095E9
		public static Code.Ldc_I4_2_ Ldc_I4_2
		{
			get
			{
				return new Code.Ldc_I4_2_
				{
					opcode = OpCodes.Ldc_I4_2
				};
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000B3FB File Offset: 0x000095FB
		public static Code.Ldc_I4_3_ Ldc_I4_3
		{
			get
			{
				return new Code.Ldc_I4_3_
				{
					opcode = OpCodes.Ldc_I4_3
				};
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000B40D File Offset: 0x0000960D
		public static Code.Ldc_I4_4_ Ldc_I4_4
		{
			get
			{
				return new Code.Ldc_I4_4_
				{
					opcode = OpCodes.Ldc_I4_4
				};
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000B41F File Offset: 0x0000961F
		public static Code.Ldc_I4_5_ Ldc_I4_5
		{
			get
			{
				return new Code.Ldc_I4_5_
				{
					opcode = OpCodes.Ldc_I4_5
				};
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000B431 File Offset: 0x00009631
		public static Code.Ldc_I4_6_ Ldc_I4_6
		{
			get
			{
				return new Code.Ldc_I4_6_
				{
					opcode = OpCodes.Ldc_I4_6
				};
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000B443 File Offset: 0x00009643
		public static Code.Ldc_I4_7_ Ldc_I4_7
		{
			get
			{
				return new Code.Ldc_I4_7_
				{
					opcode = OpCodes.Ldc_I4_7
				};
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B455 File Offset: 0x00009655
		public static Code.Ldc_I4_8_ Ldc_I4_8
		{
			get
			{
				return new Code.Ldc_I4_8_
				{
					opcode = OpCodes.Ldc_I4_8
				};
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000B467 File Offset: 0x00009667
		public static Code.Ldc_I4_S_ Ldc_I4_S
		{
			get
			{
				return new Code.Ldc_I4_S_
				{
					opcode = OpCodes.Ldc_I4_S
				};
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000B479 File Offset: 0x00009679
		public static Code.Ldc_I4_ Ldc_I4
		{
			get
			{
				return new Code.Ldc_I4_
				{
					opcode = OpCodes.Ldc_I4
				};
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000B48B File Offset: 0x0000968B
		public static Code.Ldc_I8_ Ldc_I8
		{
			get
			{
				return new Code.Ldc_I8_
				{
					opcode = OpCodes.Ldc_I8
				};
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000B49D File Offset: 0x0000969D
		public static Code.Ldc_R4_ Ldc_R4
		{
			get
			{
				return new Code.Ldc_R4_
				{
					opcode = OpCodes.Ldc_R4
				};
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000B4AF File Offset: 0x000096AF
		public static Code.Ldc_R8_ Ldc_R8
		{
			get
			{
				return new Code.Ldc_R8_
				{
					opcode = OpCodes.Ldc_R8
				};
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000B4C1 File Offset: 0x000096C1
		public static Code.Dup_ Dup
		{
			get
			{
				return new Code.Dup_
				{
					opcode = OpCodes.Dup
				};
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000B4D3 File Offset: 0x000096D3
		public static Code.Pop_ Pop
		{
			get
			{
				return new Code.Pop_
				{
					opcode = OpCodes.Pop
				};
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B4E5 File Offset: 0x000096E5
		public static Code.Jmp_ Jmp
		{
			get
			{
				return new Code.Jmp_
				{
					opcode = OpCodes.Jmp
				};
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000B4F7 File Offset: 0x000096F7
		public static Code.Call_ Call
		{
			get
			{
				return new Code.Call_
				{
					opcode = OpCodes.Call
				};
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000B509 File Offset: 0x00009709
		public static Code.Calli_ Calli
		{
			get
			{
				return new Code.Calli_
				{
					opcode = OpCodes.Calli
				};
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B51B File Offset: 0x0000971B
		public static Code.Ret_ Ret
		{
			get
			{
				return new Code.Ret_
				{
					opcode = OpCodes.Ret
				};
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000B52D File Offset: 0x0000972D
		public static Code.Br_S_ Br_S
		{
			get
			{
				return new Code.Br_S_
				{
					opcode = OpCodes.Br_S
				};
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000B53F File Offset: 0x0000973F
		public static Code.Brfalse_S_ Brfalse_S
		{
			get
			{
				return new Code.Brfalse_S_
				{
					opcode = OpCodes.Brfalse_S
				};
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000B551 File Offset: 0x00009751
		public static Code.Brtrue_S_ Brtrue_S
		{
			get
			{
				return new Code.Brtrue_S_
				{
					opcode = OpCodes.Brtrue_S
				};
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000B563 File Offset: 0x00009763
		public static Code.Beq_S_ Beq_S
		{
			get
			{
				return new Code.Beq_S_
				{
					opcode = OpCodes.Beq_S
				};
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000B575 File Offset: 0x00009775
		public static Code.Bge_S_ Bge_S
		{
			get
			{
				return new Code.Bge_S_
				{
					opcode = OpCodes.Bge_S
				};
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000B587 File Offset: 0x00009787
		public static Code.Bgt_S_ Bgt_S
		{
			get
			{
				return new Code.Bgt_S_
				{
					opcode = OpCodes.Bgt_S
				};
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000B599 File Offset: 0x00009799
		public static Code.Ble_S_ Ble_S
		{
			get
			{
				return new Code.Ble_S_
				{
					opcode = OpCodes.Ble_S
				};
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000B5AB File Offset: 0x000097AB
		public static Code.Blt_S_ Blt_S
		{
			get
			{
				return new Code.Blt_S_
				{
					opcode = OpCodes.Blt_S
				};
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000B5BD File Offset: 0x000097BD
		public static Code.Bne_Un_S_ Bne_Un_S
		{
			get
			{
				return new Code.Bne_Un_S_
				{
					opcode = OpCodes.Bne_Un_S
				};
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B5CF File Offset: 0x000097CF
		public static Code.Bge_Un_S_ Bge_Un_S
		{
			get
			{
				return new Code.Bge_Un_S_
				{
					opcode = OpCodes.Bge_Un_S
				};
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000B5E1 File Offset: 0x000097E1
		public static Code.Bgt_Un_S_ Bgt_Un_S
		{
			get
			{
				return new Code.Bgt_Un_S_
				{
					opcode = OpCodes.Bgt_Un_S
				};
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000B5F3 File Offset: 0x000097F3
		public static Code.Ble_Un_S_ Ble_Un_S
		{
			get
			{
				return new Code.Ble_Un_S_
				{
					opcode = OpCodes.Ble_Un_S
				};
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000B605 File Offset: 0x00009805
		public static Code.Blt_Un_S_ Blt_Un_S
		{
			get
			{
				return new Code.Blt_Un_S_
				{
					opcode = OpCodes.Blt_Un_S
				};
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000B617 File Offset: 0x00009817
		public static Code.Br_ Br
		{
			get
			{
				return new Code.Br_
				{
					opcode = OpCodes.Br
				};
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000B629 File Offset: 0x00009829
		public static Code.Brfalse_ Brfalse
		{
			get
			{
				return new Code.Brfalse_
				{
					opcode = OpCodes.Brfalse
				};
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000B63B File Offset: 0x0000983B
		public static Code.Brtrue_ Brtrue
		{
			get
			{
				return new Code.Brtrue_
				{
					opcode = OpCodes.Brtrue
				};
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000B64D File Offset: 0x0000984D
		public static Code.Beq_ Beq
		{
			get
			{
				return new Code.Beq_
				{
					opcode = OpCodes.Beq
				};
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000B65F File Offset: 0x0000985F
		public static Code.Bge_ Bge
		{
			get
			{
				return new Code.Bge_
				{
					opcode = OpCodes.Bge
				};
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000B671 File Offset: 0x00009871
		public static Code.Bgt_ Bgt
		{
			get
			{
				return new Code.Bgt_
				{
					opcode = OpCodes.Bgt
				};
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000B683 File Offset: 0x00009883
		public static Code.Ble_ Ble
		{
			get
			{
				return new Code.Ble_
				{
					opcode = OpCodes.Ble
				};
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000B695 File Offset: 0x00009895
		public static Code.Blt_ Blt
		{
			get
			{
				return new Code.Blt_
				{
					opcode = OpCodes.Blt
				};
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000B6A7 File Offset: 0x000098A7
		public static Code.Bne_Un_ Bne_Un
		{
			get
			{
				return new Code.Bne_Un_
				{
					opcode = OpCodes.Bne_Un
				};
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000B6B9 File Offset: 0x000098B9
		public static Code.Bge_Un_ Bge_Un
		{
			get
			{
				return new Code.Bge_Un_
				{
					opcode = OpCodes.Bge_Un
				};
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000B6CB File Offset: 0x000098CB
		public static Code.Bgt_Un_ Bgt_Un
		{
			get
			{
				return new Code.Bgt_Un_
				{
					opcode = OpCodes.Bgt_Un
				};
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B6DD File Offset: 0x000098DD
		public static Code.Ble_Un_ Ble_Un
		{
			get
			{
				return new Code.Ble_Un_
				{
					opcode = OpCodes.Ble_Un
				};
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000B6EF File Offset: 0x000098EF
		public static Code.Blt_Un_ Blt_Un
		{
			get
			{
				return new Code.Blt_Un_
				{
					opcode = OpCodes.Blt_Un
				};
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000B701 File Offset: 0x00009901
		public static Code.Switch_ Switch
		{
			get
			{
				return new Code.Switch_
				{
					opcode = OpCodes.Switch
				};
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B713 File Offset: 0x00009913
		public static Code.Ldind_I1_ Ldind_I1
		{
			get
			{
				return new Code.Ldind_I1_
				{
					opcode = OpCodes.Ldind_I1
				};
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000B725 File Offset: 0x00009925
		public static Code.Ldind_U1_ Ldind_U1
		{
			get
			{
				return new Code.Ldind_U1_
				{
					opcode = OpCodes.Ldind_U1
				};
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000B737 File Offset: 0x00009937
		public static Code.Ldind_I2_ Ldind_I2
		{
			get
			{
				return new Code.Ldind_I2_
				{
					opcode = OpCodes.Ldind_I2
				};
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000B749 File Offset: 0x00009949
		public static Code.Ldind_U2_ Ldind_U2
		{
			get
			{
				return new Code.Ldind_U2_
				{
					opcode = OpCodes.Ldind_U2
				};
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000B75B File Offset: 0x0000995B
		public static Code.Ldind_I4_ Ldind_I4
		{
			get
			{
				return new Code.Ldind_I4_
				{
					opcode = OpCodes.Ldind_I4
				};
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000B76D File Offset: 0x0000996D
		public static Code.Ldind_U4_ Ldind_U4
		{
			get
			{
				return new Code.Ldind_U4_
				{
					opcode = OpCodes.Ldind_U4
				};
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000B77F File Offset: 0x0000997F
		public static Code.Ldind_I8_ Ldind_I8
		{
			get
			{
				return new Code.Ldind_I8_
				{
					opcode = OpCodes.Ldind_I8
				};
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000B791 File Offset: 0x00009991
		public static Code.Ldind_I_ Ldind_I
		{
			get
			{
				return new Code.Ldind_I_
				{
					opcode = OpCodes.Ldind_I
				};
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000B7A3 File Offset: 0x000099A3
		public static Code.Ldind_R4_ Ldind_R4
		{
			get
			{
				return new Code.Ldind_R4_
				{
					opcode = OpCodes.Ldind_R4
				};
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000B7B5 File Offset: 0x000099B5
		public static Code.Ldind_R8_ Ldind_R8
		{
			get
			{
				return new Code.Ldind_R8_
				{
					opcode = OpCodes.Ldind_R8
				};
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000B7C7 File Offset: 0x000099C7
		public static Code.Ldind_Ref_ Ldind_Ref
		{
			get
			{
				return new Code.Ldind_Ref_
				{
					opcode = OpCodes.Ldind_Ref
				};
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000B7D9 File Offset: 0x000099D9
		public static Code.Stind_Ref_ Stind_Ref
		{
			get
			{
				return new Code.Stind_Ref_
				{
					opcode = OpCodes.Stind_Ref
				};
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000B7EB File Offset: 0x000099EB
		public static Code.Stind_I1_ Stind_I1
		{
			get
			{
				return new Code.Stind_I1_
				{
					opcode = OpCodes.Stind_I1
				};
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000B7FD File Offset: 0x000099FD
		public static Code.Stind_I2_ Stind_I2
		{
			get
			{
				return new Code.Stind_I2_
				{
					opcode = OpCodes.Stind_I2
				};
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000B80F File Offset: 0x00009A0F
		public static Code.Stind_I4_ Stind_I4
		{
			get
			{
				return new Code.Stind_I4_
				{
					opcode = OpCodes.Stind_I4
				};
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000B821 File Offset: 0x00009A21
		public static Code.Stind_I8_ Stind_I8
		{
			get
			{
				return new Code.Stind_I8_
				{
					opcode = OpCodes.Stind_I8
				};
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000B833 File Offset: 0x00009A33
		public static Code.Stind_R4_ Stind_R4
		{
			get
			{
				return new Code.Stind_R4_
				{
					opcode = OpCodes.Stind_R4
				};
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000B845 File Offset: 0x00009A45
		public static Code.Stind_R8_ Stind_R8
		{
			get
			{
				return new Code.Stind_R8_
				{
					opcode = OpCodes.Stind_R8
				};
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000B857 File Offset: 0x00009A57
		public static Code.Add_ Add
		{
			get
			{
				return new Code.Add_
				{
					opcode = OpCodes.Add
				};
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000B869 File Offset: 0x00009A69
		public static Code.Sub_ Sub
		{
			get
			{
				return new Code.Sub_
				{
					opcode = OpCodes.Sub
				};
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000B87B File Offset: 0x00009A7B
		public static Code.Mul_ Mul
		{
			get
			{
				return new Code.Mul_
				{
					opcode = OpCodes.Mul
				};
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000B88D File Offset: 0x00009A8D
		public static Code.Div_ Div
		{
			get
			{
				return new Code.Div_
				{
					opcode = OpCodes.Div
				};
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000B89F File Offset: 0x00009A9F
		public static Code.Div_Un_ Div_Un
		{
			get
			{
				return new Code.Div_Un_
				{
					opcode = OpCodes.Div_Un
				};
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000B8B1 File Offset: 0x00009AB1
		public static Code.Rem_ Rem
		{
			get
			{
				return new Code.Rem_
				{
					opcode = OpCodes.Rem
				};
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000B8C3 File Offset: 0x00009AC3
		public static Code.Rem_Un_ Rem_Un
		{
			get
			{
				return new Code.Rem_Un_
				{
					opcode = OpCodes.Rem_Un
				};
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000B8D5 File Offset: 0x00009AD5
		public static Code.And_ And
		{
			get
			{
				return new Code.And_
				{
					opcode = OpCodes.And
				};
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000B8E7 File Offset: 0x00009AE7
		public static Code.Or_ Or
		{
			get
			{
				return new Code.Or_
				{
					opcode = OpCodes.Or
				};
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000B8F9 File Offset: 0x00009AF9
		public static Code.Xor_ Xor
		{
			get
			{
				return new Code.Xor_
				{
					opcode = OpCodes.Xor
				};
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000B90B File Offset: 0x00009B0B
		public static Code.Shl_ Shl
		{
			get
			{
				return new Code.Shl_
				{
					opcode = OpCodes.Shl
				};
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000B91D File Offset: 0x00009B1D
		public static Code.Shr_ Shr
		{
			get
			{
				return new Code.Shr_
				{
					opcode = OpCodes.Shr
				};
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000B92F File Offset: 0x00009B2F
		public static Code.Shr_Un_ Shr_Un
		{
			get
			{
				return new Code.Shr_Un_
				{
					opcode = OpCodes.Shr_Un
				};
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000B941 File Offset: 0x00009B41
		public static Code.Neg_ Neg
		{
			get
			{
				return new Code.Neg_
				{
					opcode = OpCodes.Neg
				};
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000B953 File Offset: 0x00009B53
		public static Code.Not_ Not
		{
			get
			{
				return new Code.Not_
				{
					opcode = OpCodes.Not
				};
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000B965 File Offset: 0x00009B65
		public static Code.Conv_I1_ Conv_I1
		{
			get
			{
				return new Code.Conv_I1_
				{
					opcode = OpCodes.Conv_I1
				};
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000B977 File Offset: 0x00009B77
		public static Code.Conv_I2_ Conv_I2
		{
			get
			{
				return new Code.Conv_I2_
				{
					opcode = OpCodes.Conv_I2
				};
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000B989 File Offset: 0x00009B89
		public static Code.Conv_I4_ Conv_I4
		{
			get
			{
				return new Code.Conv_I4_
				{
					opcode = OpCodes.Conv_I4
				};
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000B99B File Offset: 0x00009B9B
		public static Code.Conv_I8_ Conv_I8
		{
			get
			{
				return new Code.Conv_I8_
				{
					opcode = OpCodes.Conv_I8
				};
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000B9AD File Offset: 0x00009BAD
		public static Code.Conv_R4_ Conv_R4
		{
			get
			{
				return new Code.Conv_R4_
				{
					opcode = OpCodes.Conv_R4
				};
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000B9BF File Offset: 0x00009BBF
		public static Code.Conv_R8_ Conv_R8
		{
			get
			{
				return new Code.Conv_R8_
				{
					opcode = OpCodes.Conv_R8
				};
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000B9D1 File Offset: 0x00009BD1
		public static Code.Conv_U4_ Conv_U4
		{
			get
			{
				return new Code.Conv_U4_
				{
					opcode = OpCodes.Conv_U4
				};
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000B9E3 File Offset: 0x00009BE3
		public static Code.Conv_U8_ Conv_U8
		{
			get
			{
				return new Code.Conv_U8_
				{
					opcode = OpCodes.Conv_U8
				};
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000B9F5 File Offset: 0x00009BF5
		public static Code.Callvirt_ Callvirt
		{
			get
			{
				return new Code.Callvirt_
				{
					opcode = OpCodes.Callvirt
				};
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000BA07 File Offset: 0x00009C07
		public static Code.Cpobj_ Cpobj
		{
			get
			{
				return new Code.Cpobj_
				{
					opcode = OpCodes.Cpobj
				};
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000BA19 File Offset: 0x00009C19
		public static Code.Ldobj_ Ldobj
		{
			get
			{
				return new Code.Ldobj_
				{
					opcode = OpCodes.Ldobj
				};
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000BA2B File Offset: 0x00009C2B
		public static Code.Ldstr_ Ldstr
		{
			get
			{
				return new Code.Ldstr_
				{
					opcode = OpCodes.Ldstr
				};
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000BA3D File Offset: 0x00009C3D
		public static Code.Newobj_ Newobj
		{
			get
			{
				return new Code.Newobj_
				{
					opcode = OpCodes.Newobj
				};
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000BA4F File Offset: 0x00009C4F
		public static Code.Castclass_ Castclass
		{
			get
			{
				return new Code.Castclass_
				{
					opcode = OpCodes.Castclass
				};
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000BA61 File Offset: 0x00009C61
		public static Code.Isinst_ Isinst
		{
			get
			{
				return new Code.Isinst_
				{
					opcode = OpCodes.Isinst
				};
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000BA73 File Offset: 0x00009C73
		public static Code.Conv_R_Un_ Conv_R_Un
		{
			get
			{
				return new Code.Conv_R_Un_
				{
					opcode = OpCodes.Conv_R_Un
				};
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000BA85 File Offset: 0x00009C85
		public static Code.Unbox_ Unbox
		{
			get
			{
				return new Code.Unbox_
				{
					opcode = OpCodes.Unbox
				};
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000BA97 File Offset: 0x00009C97
		public static Code.Throw_ Throw
		{
			get
			{
				return new Code.Throw_
				{
					opcode = OpCodes.Throw
				};
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000BAA9 File Offset: 0x00009CA9
		public static Code.Ldfld_ Ldfld
		{
			get
			{
				return new Code.Ldfld_
				{
					opcode = OpCodes.Ldfld
				};
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000BABB File Offset: 0x00009CBB
		public static Code.Ldflda_ Ldflda
		{
			get
			{
				return new Code.Ldflda_
				{
					opcode = OpCodes.Ldflda
				};
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000BACD File Offset: 0x00009CCD
		public static Code.Stfld_ Stfld
		{
			get
			{
				return new Code.Stfld_
				{
					opcode = OpCodes.Stfld
				};
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000BADF File Offset: 0x00009CDF
		public static Code.Ldsfld_ Ldsfld
		{
			get
			{
				return new Code.Ldsfld_
				{
					opcode = OpCodes.Ldsfld
				};
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000BAF1 File Offset: 0x00009CF1
		public static Code.Ldsflda_ Ldsflda
		{
			get
			{
				return new Code.Ldsflda_
				{
					opcode = OpCodes.Ldsflda
				};
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000BB03 File Offset: 0x00009D03
		public static Code.Stsfld_ Stsfld
		{
			get
			{
				return new Code.Stsfld_
				{
					opcode = OpCodes.Stsfld
				};
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000BB15 File Offset: 0x00009D15
		public static Code.Stobj_ Stobj
		{
			get
			{
				return new Code.Stobj_
				{
					opcode = OpCodes.Stobj
				};
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000BB27 File Offset: 0x00009D27
		public static Code.Conv_Ovf_I1_Un_ Conv_Ovf_I1_Un
		{
			get
			{
				return new Code.Conv_Ovf_I1_Un_
				{
					opcode = OpCodes.Conv_Ovf_I1_Un
				};
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000BB39 File Offset: 0x00009D39
		public static Code.Conv_Ovf_I2_Un_ Conv_Ovf_I2_Un
		{
			get
			{
				return new Code.Conv_Ovf_I2_Un_
				{
					opcode = OpCodes.Conv_Ovf_I2_Un
				};
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000BB4B File Offset: 0x00009D4B
		public static Code.Conv_Ovf_I4_Un_ Conv_Ovf_I4_Un
		{
			get
			{
				return new Code.Conv_Ovf_I4_Un_
				{
					opcode = OpCodes.Conv_Ovf_I4_Un
				};
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000BB5D File Offset: 0x00009D5D
		public static Code.Conv_Ovf_I8_Un_ Conv_Ovf_I8_Un
		{
			get
			{
				return new Code.Conv_Ovf_I8_Un_
				{
					opcode = OpCodes.Conv_Ovf_I8_Un
				};
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000BB6F File Offset: 0x00009D6F
		public static Code.Conv_Ovf_U1_Un_ Conv_Ovf_U1_Un
		{
			get
			{
				return new Code.Conv_Ovf_U1_Un_
				{
					opcode = OpCodes.Conv_Ovf_U1_Un
				};
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000BB81 File Offset: 0x00009D81
		public static Code.Conv_Ovf_U2_Un_ Conv_Ovf_U2_Un
		{
			get
			{
				return new Code.Conv_Ovf_U2_Un_
				{
					opcode = OpCodes.Conv_Ovf_U2_Un
				};
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000BB93 File Offset: 0x00009D93
		public static Code.Conv_Ovf_U4_Un_ Conv_Ovf_U4_Un
		{
			get
			{
				return new Code.Conv_Ovf_U4_Un_
				{
					opcode = OpCodes.Conv_Ovf_U4_Un
				};
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000BBA5 File Offset: 0x00009DA5
		public static Code.Conv_Ovf_U8_Un_ Conv_Ovf_U8_Un
		{
			get
			{
				return new Code.Conv_Ovf_U8_Un_
				{
					opcode = OpCodes.Conv_Ovf_U8_Un
				};
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000BBB7 File Offset: 0x00009DB7
		public static Code.Conv_Ovf_I_Un_ Conv_Ovf_I_Un
		{
			get
			{
				return new Code.Conv_Ovf_I_Un_
				{
					opcode = OpCodes.Conv_Ovf_I_Un
				};
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000BBC9 File Offset: 0x00009DC9
		public static Code.Conv_Ovf_U_Un_ Conv_Ovf_U_Un
		{
			get
			{
				return new Code.Conv_Ovf_U_Un_
				{
					opcode = OpCodes.Conv_Ovf_U_Un
				};
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000BBDB File Offset: 0x00009DDB
		public static Code.Box_ Box
		{
			get
			{
				return new Code.Box_
				{
					opcode = OpCodes.Box
				};
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000BBED File Offset: 0x00009DED
		public static Code.Newarr_ Newarr
		{
			get
			{
				return new Code.Newarr_
				{
					opcode = OpCodes.Newarr
				};
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000BBFF File Offset: 0x00009DFF
		public static Code.Ldlen_ Ldlen
		{
			get
			{
				return new Code.Ldlen_
				{
					opcode = OpCodes.Ldlen
				};
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000BC11 File Offset: 0x00009E11
		public static Code.Ldelema_ Ldelema
		{
			get
			{
				return new Code.Ldelema_
				{
					opcode = OpCodes.Ldelema
				};
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000BC23 File Offset: 0x00009E23
		public static Code.Ldelem_I1_ Ldelem_I1
		{
			get
			{
				return new Code.Ldelem_I1_
				{
					opcode = OpCodes.Ldelem_I1
				};
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000BC35 File Offset: 0x00009E35
		public static Code.Ldelem_U1_ Ldelem_U1
		{
			get
			{
				return new Code.Ldelem_U1_
				{
					opcode = OpCodes.Ldelem_U1
				};
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000BC47 File Offset: 0x00009E47
		public static Code.Ldelem_I2_ Ldelem_I2
		{
			get
			{
				return new Code.Ldelem_I2_
				{
					opcode = OpCodes.Ldelem_I2
				};
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000BC59 File Offset: 0x00009E59
		public static Code.Ldelem_U2_ Ldelem_U2
		{
			get
			{
				return new Code.Ldelem_U2_
				{
					opcode = OpCodes.Ldelem_U2
				};
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000BC6B File Offset: 0x00009E6B
		public static Code.Ldelem_I4_ Ldelem_I4
		{
			get
			{
				return new Code.Ldelem_I4_
				{
					opcode = OpCodes.Ldelem_I4
				};
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000BC7D File Offset: 0x00009E7D
		public static Code.Ldelem_U4_ Ldelem_U4
		{
			get
			{
				return new Code.Ldelem_U4_
				{
					opcode = OpCodes.Ldelem_U4
				};
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000BC8F File Offset: 0x00009E8F
		public static Code.Ldelem_I8_ Ldelem_I8
		{
			get
			{
				return new Code.Ldelem_I8_
				{
					opcode = OpCodes.Ldelem_I8
				};
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000BCA1 File Offset: 0x00009EA1
		public static Code.Ldelem_I_ Ldelem_I
		{
			get
			{
				return new Code.Ldelem_I_
				{
					opcode = OpCodes.Ldelem_I
				};
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000BCB3 File Offset: 0x00009EB3
		public static Code.Ldelem_R4_ Ldelem_R4
		{
			get
			{
				return new Code.Ldelem_R4_
				{
					opcode = OpCodes.Ldelem_R4
				};
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000BCC5 File Offset: 0x00009EC5
		public static Code.Ldelem_R8_ Ldelem_R8
		{
			get
			{
				return new Code.Ldelem_R8_
				{
					opcode = OpCodes.Ldelem_R8
				};
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000BCD7 File Offset: 0x00009ED7
		public static Code.Ldelem_Ref_ Ldelem_Ref
		{
			get
			{
				return new Code.Ldelem_Ref_
				{
					opcode = OpCodes.Ldelem_Ref
				};
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000BCE9 File Offset: 0x00009EE9
		public static Code.Stelem_I_ Stelem_I
		{
			get
			{
				return new Code.Stelem_I_
				{
					opcode = OpCodes.Stelem_I
				};
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000BCFB File Offset: 0x00009EFB
		public static Code.Stelem_I1_ Stelem_I1
		{
			get
			{
				return new Code.Stelem_I1_
				{
					opcode = OpCodes.Stelem_I1
				};
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000BD0D File Offset: 0x00009F0D
		public static Code.Stelem_I2_ Stelem_I2
		{
			get
			{
				return new Code.Stelem_I2_
				{
					opcode = OpCodes.Stelem_I2
				};
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000BD1F File Offset: 0x00009F1F
		public static Code.Stelem_I4_ Stelem_I4
		{
			get
			{
				return new Code.Stelem_I4_
				{
					opcode = OpCodes.Stelem_I4
				};
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000BD31 File Offset: 0x00009F31
		public static Code.Stelem_I8_ Stelem_I8
		{
			get
			{
				return new Code.Stelem_I8_
				{
					opcode = OpCodes.Stelem_I8
				};
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000BD43 File Offset: 0x00009F43
		public static Code.Stelem_R4_ Stelem_R4
		{
			get
			{
				return new Code.Stelem_R4_
				{
					opcode = OpCodes.Stelem_R4
				};
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000BD55 File Offset: 0x00009F55
		public static Code.Stelem_R8_ Stelem_R8
		{
			get
			{
				return new Code.Stelem_R8_
				{
					opcode = OpCodes.Stelem_R8
				};
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000BD67 File Offset: 0x00009F67
		public static Code.Stelem_Ref_ Stelem_Ref
		{
			get
			{
				return new Code.Stelem_Ref_
				{
					opcode = OpCodes.Stelem_Ref
				};
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000BD79 File Offset: 0x00009F79
		public static Code.Ldelem_ Ldelem
		{
			get
			{
				return new Code.Ldelem_
				{
					opcode = OpCodes.Ldelem
				};
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000BD8B File Offset: 0x00009F8B
		public static Code.Stelem_ Stelem
		{
			get
			{
				return new Code.Stelem_
				{
					opcode = OpCodes.Stelem
				};
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000BD9D File Offset: 0x00009F9D
		public static Code.Unbox_Any_ Unbox_Any
		{
			get
			{
				return new Code.Unbox_Any_
				{
					opcode = OpCodes.Unbox_Any
				};
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000BDAF File Offset: 0x00009FAF
		public static Code.Conv_Ovf_I1_ Conv_Ovf_I1
		{
			get
			{
				return new Code.Conv_Ovf_I1_
				{
					opcode = OpCodes.Conv_Ovf_I1
				};
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000BDC1 File Offset: 0x00009FC1
		public static Code.Conv_Ovf_U1_ Conv_Ovf_U1
		{
			get
			{
				return new Code.Conv_Ovf_U1_
				{
					opcode = OpCodes.Conv_Ovf_U1
				};
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000BDD3 File Offset: 0x00009FD3
		public static Code.Conv_Ovf_I2_ Conv_Ovf_I2
		{
			get
			{
				return new Code.Conv_Ovf_I2_
				{
					opcode = OpCodes.Conv_Ovf_I2
				};
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000BDE5 File Offset: 0x00009FE5
		public static Code.Conv_Ovf_U2_ Conv_Ovf_U2
		{
			get
			{
				return new Code.Conv_Ovf_U2_
				{
					opcode = OpCodes.Conv_Ovf_U2
				};
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000BDF7 File Offset: 0x00009FF7
		public static Code.Conv_Ovf_I4_ Conv_Ovf_I4
		{
			get
			{
				return new Code.Conv_Ovf_I4_
				{
					opcode = OpCodes.Conv_Ovf_I4
				};
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000BE09 File Offset: 0x0000A009
		public static Code.Conv_Ovf_U4_ Conv_Ovf_U4
		{
			get
			{
				return new Code.Conv_Ovf_U4_
				{
					opcode = OpCodes.Conv_Ovf_U4
				};
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000BE1B File Offset: 0x0000A01B
		public static Code.Conv_Ovf_I8_ Conv_Ovf_I8
		{
			get
			{
				return new Code.Conv_Ovf_I8_
				{
					opcode = OpCodes.Conv_Ovf_I8
				};
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000BE2D File Offset: 0x0000A02D
		public static Code.Conv_Ovf_U8_ Conv_Ovf_U8
		{
			get
			{
				return new Code.Conv_Ovf_U8_
				{
					opcode = OpCodes.Conv_Ovf_U8
				};
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000BE3F File Offset: 0x0000A03F
		public static Code.Refanyval_ Refanyval
		{
			get
			{
				return new Code.Refanyval_
				{
					opcode = OpCodes.Refanyval
				};
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000BE51 File Offset: 0x0000A051
		public static Code.Ckfinite_ Ckfinite
		{
			get
			{
				return new Code.Ckfinite_
				{
					opcode = OpCodes.Ckfinite
				};
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000BE63 File Offset: 0x0000A063
		public static Code.Mkrefany_ Mkrefany
		{
			get
			{
				return new Code.Mkrefany_
				{
					opcode = OpCodes.Mkrefany
				};
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000BE75 File Offset: 0x0000A075
		public static Code.Ldtoken_ Ldtoken
		{
			get
			{
				return new Code.Ldtoken_
				{
					opcode = OpCodes.Ldtoken
				};
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000BE87 File Offset: 0x0000A087
		public static Code.Conv_U2_ Conv_U2
		{
			get
			{
				return new Code.Conv_U2_
				{
					opcode = OpCodes.Conv_U2
				};
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000BE99 File Offset: 0x0000A099
		public static Code.Conv_U1_ Conv_U1
		{
			get
			{
				return new Code.Conv_U1_
				{
					opcode = OpCodes.Conv_U1
				};
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000BEAB File Offset: 0x0000A0AB
		public static Code.Conv_I_ Conv_I
		{
			get
			{
				return new Code.Conv_I_
				{
					opcode = OpCodes.Conv_I
				};
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000BEBD File Offset: 0x0000A0BD
		public static Code.Conv_Ovf_I_ Conv_Ovf_I
		{
			get
			{
				return new Code.Conv_Ovf_I_
				{
					opcode = OpCodes.Conv_Ovf_I
				};
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000BECF File Offset: 0x0000A0CF
		public static Code.Conv_Ovf_U_ Conv_Ovf_U
		{
			get
			{
				return new Code.Conv_Ovf_U_
				{
					opcode = OpCodes.Conv_Ovf_U
				};
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000BEE1 File Offset: 0x0000A0E1
		public static Code.Add_Ovf_ Add_Ovf
		{
			get
			{
				return new Code.Add_Ovf_
				{
					opcode = OpCodes.Add_Ovf
				};
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		public static Code.Add_Ovf_Un_ Add_Ovf_Un
		{
			get
			{
				return new Code.Add_Ovf_Un_
				{
					opcode = OpCodes.Add_Ovf_Un
				};
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000BF05 File Offset: 0x0000A105
		public static Code.Mul_Ovf_ Mul_Ovf
		{
			get
			{
				return new Code.Mul_Ovf_
				{
					opcode = OpCodes.Mul_Ovf
				};
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000BF17 File Offset: 0x0000A117
		public static Code.Mul_Ovf_Un_ Mul_Ovf_Un
		{
			get
			{
				return new Code.Mul_Ovf_Un_
				{
					opcode = OpCodes.Mul_Ovf_Un
				};
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000BF29 File Offset: 0x0000A129
		public static Code.Sub_Ovf_ Sub_Ovf
		{
			get
			{
				return new Code.Sub_Ovf_
				{
					opcode = OpCodes.Sub_Ovf
				};
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000BF3B File Offset: 0x0000A13B
		public static Code.Sub_Ovf_Un_ Sub_Ovf_Un
		{
			get
			{
				return new Code.Sub_Ovf_Un_
				{
					opcode = OpCodes.Sub_Ovf_Un
				};
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000BF4D File Offset: 0x0000A14D
		public static Code.Endfinally_ Endfinally
		{
			get
			{
				return new Code.Endfinally_
				{
					opcode = OpCodes.Endfinally
				};
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000BF5F File Offset: 0x0000A15F
		public static Code.Leave_ Leave
		{
			get
			{
				return new Code.Leave_
				{
					opcode = OpCodes.Leave
				};
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000BF71 File Offset: 0x0000A171
		public static Code.Leave_S_ Leave_S
		{
			get
			{
				return new Code.Leave_S_
				{
					opcode = OpCodes.Leave_S
				};
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000BF83 File Offset: 0x0000A183
		public static Code.Stind_I_ Stind_I
		{
			get
			{
				return new Code.Stind_I_
				{
					opcode = OpCodes.Stind_I
				};
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000BF95 File Offset: 0x0000A195
		public static Code.Conv_U_ Conv_U
		{
			get
			{
				return new Code.Conv_U_
				{
					opcode = OpCodes.Conv_U
				};
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000BFA7 File Offset: 0x0000A1A7
		public static Code.Prefix7_ Prefix7
		{
			get
			{
				return new Code.Prefix7_
				{
					opcode = OpCodes.Prefix7
				};
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000BFB9 File Offset: 0x0000A1B9
		public static Code.Prefix6_ Prefix6
		{
			get
			{
				return new Code.Prefix6_
				{
					opcode = OpCodes.Prefix6
				};
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000BFCB File Offset: 0x0000A1CB
		public static Code.Prefix5_ Prefix5
		{
			get
			{
				return new Code.Prefix5_
				{
					opcode = OpCodes.Prefix5
				};
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		public static Code.Prefix4_ Prefix4
		{
			get
			{
				return new Code.Prefix4_
				{
					opcode = OpCodes.Prefix4
				};
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000BFEF File Offset: 0x0000A1EF
		public static Code.Prefix3_ Prefix3
		{
			get
			{
				return new Code.Prefix3_
				{
					opcode = OpCodes.Prefix3
				};
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000C001 File Offset: 0x0000A201
		public static Code.Prefix2_ Prefix2
		{
			get
			{
				return new Code.Prefix2_
				{
					opcode = OpCodes.Prefix2
				};
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000C013 File Offset: 0x0000A213
		public static Code.Prefix1_ Prefix1
		{
			get
			{
				return new Code.Prefix1_
				{
					opcode = OpCodes.Prefix1
				};
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000C025 File Offset: 0x0000A225
		public static Code.Prefixref_ Prefixref
		{
			get
			{
				return new Code.Prefixref_
				{
					opcode = OpCodes.Prefixref
				};
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000C037 File Offset: 0x0000A237
		public static Code.Arglist_ Arglist
		{
			get
			{
				return new Code.Arglist_
				{
					opcode = OpCodes.Arglist
				};
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000C049 File Offset: 0x0000A249
		public static Code.Ceq_ Ceq
		{
			get
			{
				return new Code.Ceq_
				{
					opcode = OpCodes.Ceq
				};
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000C05B File Offset: 0x0000A25B
		public static Code.Cgt_ Cgt
		{
			get
			{
				return new Code.Cgt_
				{
					opcode = OpCodes.Cgt
				};
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000C06D File Offset: 0x0000A26D
		public static Code.Cgt_Un_ Cgt_Un
		{
			get
			{
				return new Code.Cgt_Un_
				{
					opcode = OpCodes.Cgt_Un
				};
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000C07F File Offset: 0x0000A27F
		public static Code.Clt_ Clt
		{
			get
			{
				return new Code.Clt_
				{
					opcode = OpCodes.Clt
				};
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000C091 File Offset: 0x0000A291
		public static Code.Clt_Un_ Clt_Un
		{
			get
			{
				return new Code.Clt_Un_
				{
					opcode = OpCodes.Clt_Un
				};
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000C0A3 File Offset: 0x0000A2A3
		public static Code.Ldftn_ Ldftn
		{
			get
			{
				return new Code.Ldftn_
				{
					opcode = OpCodes.Ldftn
				};
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000C0B5 File Offset: 0x0000A2B5
		public static Code.Ldvirtftn_ Ldvirtftn
		{
			get
			{
				return new Code.Ldvirtftn_
				{
					opcode = OpCodes.Ldvirtftn
				};
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
		public static Code.Ldarg_ Ldarg
		{
			get
			{
				return new Code.Ldarg_
				{
					opcode = OpCodes.Ldarg
				};
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000C0D9 File Offset: 0x0000A2D9
		public static Code.Ldarga_ Ldarga
		{
			get
			{
				return new Code.Ldarga_
				{
					opcode = OpCodes.Ldarga
				};
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000C0EB File Offset: 0x0000A2EB
		public static Code.Starg_ Starg
		{
			get
			{
				return new Code.Starg_
				{
					opcode = OpCodes.Starg
				};
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000C0FD File Offset: 0x0000A2FD
		public static Code.Ldloc_ Ldloc
		{
			get
			{
				return new Code.Ldloc_
				{
					opcode = OpCodes.Ldloc
				};
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000C10F File Offset: 0x0000A30F
		public static Code.Ldloca_ Ldloca
		{
			get
			{
				return new Code.Ldloca_
				{
					opcode = OpCodes.Ldloca
				};
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000C121 File Offset: 0x0000A321
		public static Code.Stloc_ Stloc
		{
			get
			{
				return new Code.Stloc_
				{
					opcode = OpCodes.Stloc
				};
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000C133 File Offset: 0x0000A333
		public static Code.Localloc_ Localloc
		{
			get
			{
				return new Code.Localloc_
				{
					opcode = OpCodes.Localloc
				};
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000C145 File Offset: 0x0000A345
		public static Code.Endfilter_ Endfilter
		{
			get
			{
				return new Code.Endfilter_
				{
					opcode = OpCodes.Endfilter
				};
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000C157 File Offset: 0x0000A357
		public static Code.Unaligned_ Unaligned
		{
			get
			{
				return new Code.Unaligned_
				{
					opcode = OpCodes.Unaligned
				};
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000C169 File Offset: 0x0000A369
		public static Code.Volatile_ Volatile
		{
			get
			{
				return new Code.Volatile_
				{
					opcode = OpCodes.Volatile
				};
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000C17B File Offset: 0x0000A37B
		public static Code.Tailcall_ Tailcall
		{
			get
			{
				return new Code.Tailcall_
				{
					opcode = OpCodes.Tailcall
				};
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000C18D File Offset: 0x0000A38D
		public static Code.Initobj_ Initobj
		{
			get
			{
				return new Code.Initobj_
				{
					opcode = OpCodes.Initobj
				};
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000C19F File Offset: 0x0000A39F
		public static Code.Constrained_ Constrained
		{
			get
			{
				return new Code.Constrained_
				{
					opcode = OpCodes.Constrained
				};
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
		public static Code.Cpblk_ Cpblk
		{
			get
			{
				return new Code.Cpblk_
				{
					opcode = OpCodes.Cpblk
				};
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000C1C3 File Offset: 0x0000A3C3
		public static Code.Initblk_ Initblk
		{
			get
			{
				return new Code.Initblk_
				{
					opcode = OpCodes.Initblk
				};
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000C1D5 File Offset: 0x0000A3D5
		public static Code.Rethrow_ Rethrow
		{
			get
			{
				return new Code.Rethrow_
				{
					opcode = OpCodes.Rethrow
				};
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000C1E7 File Offset: 0x0000A3E7
		public static Code.Sizeof_ Sizeof
		{
			get
			{
				return new Code.Sizeof_
				{
					opcode = OpCodes.Sizeof
				};
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000C1F9 File Offset: 0x0000A3F9
		public static Code.Refanytype_ Refanytype
		{
			get
			{
				return new Code.Refanytype_
				{
					opcode = OpCodes.Refanytype
				};
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000C20B File Offset: 0x0000A40B
		public static Code.Readonly_ Readonly
		{
			get
			{
				return new Code.Readonly_
				{
					opcode = OpCodes.Readonly
				};
			}
		}

		// Token: 0x020000CD RID: 205
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Operand_ : CodeMatch
		{
			// Token: 0x17000114 RID: 276
			public Code.Operand_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Operand_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x00018FF8 File Offset: 0x000171F8
			public Operand_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CE RID: 206
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Nop_ : CodeMatch
		{
			// Token: 0x17000115 RID: 277
			public Code.Nop_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Nop_)base.Set(OpCodes.Nop, operand, name);
				}
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x0001902C File Offset: 0x0001722C
			public Nop_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CF RID: 207
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Break_ : CodeMatch
		{
			// Token: 0x17000116 RID: 278
			public Code.Break_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Break_)base.Set(OpCodes.Break, operand, name);
				}
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x00019060 File Offset: 0x00017260
			public Break_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D0 RID: 208
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_0_ : CodeMatch
		{
			// Token: 0x17000117 RID: 279
			public Code.Ldarg_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_0_)base.Set(OpCodes.Ldarg_0, operand, name);
				}
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x00019094 File Offset: 0x00017294
			public Ldarg_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D1 RID: 209
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_1_ : CodeMatch
		{
			// Token: 0x17000118 RID: 280
			public Code.Ldarg_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_1_)base.Set(OpCodes.Ldarg_1, operand, name);
				}
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x000190C8 File Offset: 0x000172C8
			public Ldarg_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D2 RID: 210
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_2_ : CodeMatch
		{
			// Token: 0x17000119 RID: 281
			public Code.Ldarg_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_2_)base.Set(OpCodes.Ldarg_2, operand, name);
				}
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x000190FC File Offset: 0x000172FC
			public Ldarg_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D3 RID: 211
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_3_ : CodeMatch
		{
			// Token: 0x1700011A RID: 282
			public Code.Ldarg_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_3_)base.Set(OpCodes.Ldarg_3, operand, name);
				}
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x00019130 File Offset: 0x00017330
			public Ldarg_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D4 RID: 212
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_0_ : CodeMatch
		{
			// Token: 0x1700011B RID: 283
			public Code.Ldloc_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_0_)base.Set(OpCodes.Ldloc_0, operand, name);
				}
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x00019164 File Offset: 0x00017364
			public Ldloc_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D5 RID: 213
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_1_ : CodeMatch
		{
			// Token: 0x1700011C RID: 284
			public Code.Ldloc_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_1_)base.Set(OpCodes.Ldloc_1, operand, name);
				}
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x00019198 File Offset: 0x00017398
			public Ldloc_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D6 RID: 214
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_2_ : CodeMatch
		{
			// Token: 0x1700011D RID: 285
			public Code.Ldloc_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_2_)base.Set(OpCodes.Ldloc_2, operand, name);
				}
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x000191CC File Offset: 0x000173CC
			public Ldloc_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D7 RID: 215
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_3_ : CodeMatch
		{
			// Token: 0x1700011E RID: 286
			public Code.Ldloc_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_3_)base.Set(OpCodes.Ldloc_3, operand, name);
				}
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x00019200 File Offset: 0x00017400
			public Ldloc_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D8 RID: 216
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_0_ : CodeMatch
		{
			// Token: 0x1700011F RID: 287
			public Code.Stloc_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_0_)base.Set(OpCodes.Stloc_0, operand, name);
				}
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x00019234 File Offset: 0x00017434
			public Stloc_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D9 RID: 217
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_1_ : CodeMatch
		{
			// Token: 0x17000120 RID: 288
			public Code.Stloc_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_1_)base.Set(OpCodes.Stloc_1, operand, name);
				}
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x00019268 File Offset: 0x00017468
			public Stloc_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DA RID: 218
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_2_ : CodeMatch
		{
			// Token: 0x17000121 RID: 289
			public Code.Stloc_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_2_)base.Set(OpCodes.Stloc_2, operand, name);
				}
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0001929C File Offset: 0x0001749C
			public Stloc_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DB RID: 219
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_3_ : CodeMatch
		{
			// Token: 0x17000122 RID: 290
			public Code.Stloc_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_3_)base.Set(OpCodes.Stloc_3, operand, name);
				}
			}

			// Token: 0x060005D1 RID: 1489 RVA: 0x000192D0 File Offset: 0x000174D0
			public Stloc_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DC RID: 220
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_S_ : CodeMatch
		{
			// Token: 0x17000123 RID: 291
			public Code.Ldarg_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_S_)base.Set(OpCodes.Ldarg_S, operand, name);
				}
			}

			// Token: 0x060005D3 RID: 1491 RVA: 0x00019304 File Offset: 0x00017504
			public Ldarg_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DD RID: 221
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarga_S_ : CodeMatch
		{
			// Token: 0x17000124 RID: 292
			public Code.Ldarga_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarga_S_)base.Set(OpCodes.Ldarga_S, operand, name);
				}
			}

			// Token: 0x060005D5 RID: 1493 RVA: 0x00019338 File Offset: 0x00017538
			public Ldarga_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DE RID: 222
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Starg_S_ : CodeMatch
		{
			// Token: 0x17000125 RID: 293
			public Code.Starg_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Starg_S_)base.Set(OpCodes.Starg_S, operand, name);
				}
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x0001936C File Offset: 0x0001756C
			public Starg_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DF RID: 223
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_S_ : CodeMatch
		{
			// Token: 0x17000126 RID: 294
			public Code.Ldloc_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_S_)base.Set(OpCodes.Ldloc_S, operand, name);
				}
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x000193A0 File Offset: 0x000175A0
			public Ldloc_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E0 RID: 224
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloca_S_ : CodeMatch
		{
			// Token: 0x17000127 RID: 295
			public Code.Ldloca_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloca_S_)base.Set(OpCodes.Ldloca_S, operand, name);
				}
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x000193D4 File Offset: 0x000175D4
			public Ldloca_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E1 RID: 225
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_S_ : CodeMatch
		{
			// Token: 0x17000128 RID: 296
			public Code.Stloc_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_S_)base.Set(OpCodes.Stloc_S, operand, name);
				}
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x00019408 File Offset: 0x00017608
			public Stloc_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E2 RID: 226
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldnull_ : CodeMatch
		{
			// Token: 0x17000129 RID: 297
			public Code.Ldnull_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldnull_)base.Set(OpCodes.Ldnull, operand, name);
				}
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x0001943C File Offset: 0x0001763C
			public Ldnull_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E3 RID: 227
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_M1_ : CodeMatch
		{
			// Token: 0x1700012A RID: 298
			public Code.Ldc_I4_M1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_M1_)base.Set(OpCodes.Ldc_I4_M1, operand, name);
				}
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x00019470 File Offset: 0x00017670
			public Ldc_I4_M1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E4 RID: 228
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_0_ : CodeMatch
		{
			// Token: 0x1700012B RID: 299
			public Code.Ldc_I4_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_0_)base.Set(OpCodes.Ldc_I4_0, operand, name);
				}
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x000194A4 File Offset: 0x000176A4
			public Ldc_I4_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E5 RID: 229
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_1_ : CodeMatch
		{
			// Token: 0x1700012C RID: 300
			public Code.Ldc_I4_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_1_)base.Set(OpCodes.Ldc_I4_1, operand, name);
				}
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x000194D8 File Offset: 0x000176D8
			public Ldc_I4_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E6 RID: 230
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_2_ : CodeMatch
		{
			// Token: 0x1700012D RID: 301
			public Code.Ldc_I4_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_2_)base.Set(OpCodes.Ldc_I4_2, operand, name);
				}
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x0001950C File Offset: 0x0001770C
			public Ldc_I4_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E7 RID: 231
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_3_ : CodeMatch
		{
			// Token: 0x1700012E RID: 302
			public Code.Ldc_I4_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_3_)base.Set(OpCodes.Ldc_I4_3, operand, name);
				}
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x00019540 File Offset: 0x00017740
			public Ldc_I4_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E8 RID: 232
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_4_ : CodeMatch
		{
			// Token: 0x1700012F RID: 303
			public Code.Ldc_I4_4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_4_)base.Set(OpCodes.Ldc_I4_4, operand, name);
				}
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x00019574 File Offset: 0x00017774
			public Ldc_I4_4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E9 RID: 233
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_5_ : CodeMatch
		{
			// Token: 0x17000130 RID: 304
			public Code.Ldc_I4_5_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_5_)base.Set(OpCodes.Ldc_I4_5, operand, name);
				}
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x000195A8 File Offset: 0x000177A8
			public Ldc_I4_5_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EA RID: 234
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_6_ : CodeMatch
		{
			// Token: 0x17000131 RID: 305
			public Code.Ldc_I4_6_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_6_)base.Set(OpCodes.Ldc_I4_6, operand, name);
				}
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x000195DC File Offset: 0x000177DC
			public Ldc_I4_6_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EB RID: 235
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_7_ : CodeMatch
		{
			// Token: 0x17000132 RID: 306
			public Code.Ldc_I4_7_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_7_)base.Set(OpCodes.Ldc_I4_7, operand, name);
				}
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x00019610 File Offset: 0x00017810
			public Ldc_I4_7_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EC RID: 236
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_8_ : CodeMatch
		{
			// Token: 0x17000133 RID: 307
			public Code.Ldc_I4_8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_8_)base.Set(OpCodes.Ldc_I4_8, operand, name);
				}
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00019644 File Offset: 0x00017844
			public Ldc_I4_8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000ED RID: 237
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_S_ : CodeMatch
		{
			// Token: 0x17000134 RID: 308
			public Code.Ldc_I4_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_S_)base.Set(OpCodes.Ldc_I4_S, operand, name);
				}
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x00019678 File Offset: 0x00017878
			public Ldc_I4_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EE RID: 238
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I4_ : CodeMatch
		{
			// Token: 0x17000135 RID: 309
			public Code.Ldc_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_)base.Set(OpCodes.Ldc_I4, operand, name);
				}
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x000196AC File Offset: 0x000178AC
			public Ldc_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EF RID: 239
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_I8_ : CodeMatch
		{
			// Token: 0x17000136 RID: 310
			public Code.Ldc_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I8_)base.Set(OpCodes.Ldc_I8, operand, name);
				}
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x000196E0 File Offset: 0x000178E0
			public Ldc_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F0 RID: 240
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_R4_ : CodeMatch
		{
			// Token: 0x17000137 RID: 311
			public Code.Ldc_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_R4_)base.Set(OpCodes.Ldc_R4, operand, name);
				}
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x00019714 File Offset: 0x00017914
			public Ldc_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F1 RID: 241
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldc_R8_ : CodeMatch
		{
			// Token: 0x17000138 RID: 312
			public Code.Ldc_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_R8_)base.Set(OpCodes.Ldc_R8, operand, name);
				}
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x00019748 File Offset: 0x00017948
			public Ldc_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F2 RID: 242
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Dup_ : CodeMatch
		{
			// Token: 0x17000139 RID: 313
			public Code.Dup_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Dup_)base.Set(OpCodes.Dup, operand, name);
				}
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x0001977C File Offset: 0x0001797C
			public Dup_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F3 RID: 243
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Pop_ : CodeMatch
		{
			// Token: 0x1700013A RID: 314
			public Code.Pop_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Pop_)base.Set(OpCodes.Pop, operand, name);
				}
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x000197B0 File Offset: 0x000179B0
			public Pop_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F4 RID: 244
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Jmp_ : CodeMatch
		{
			// Token: 0x1700013B RID: 315
			public Code.Jmp_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Jmp_)base.Set(OpCodes.Jmp, operand, name);
				}
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x000197E4 File Offset: 0x000179E4
			public Jmp_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F5 RID: 245
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Call_ : CodeMatch
		{
			// Token: 0x1700013C RID: 316
			public Code.Call_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Call_)base.Set(OpCodes.Call, operand, name);
				}
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00019818 File Offset: 0x00017A18
			public Call_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F6 RID: 246
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Calli_ : CodeMatch
		{
			// Token: 0x1700013D RID: 317
			public Code.Calli_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Calli_)base.Set(OpCodes.Calli, operand, name);
				}
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x0001984C File Offset: 0x00017A4C
			public Calli_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F7 RID: 247
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ret_ : CodeMatch
		{
			// Token: 0x1700013E RID: 318
			public Code.Ret_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ret_)base.Set(OpCodes.Ret, operand, name);
				}
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x00019880 File Offset: 0x00017A80
			public Ret_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F8 RID: 248
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Br_S_ : CodeMatch
		{
			// Token: 0x1700013F RID: 319
			public Code.Br_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Br_S_)base.Set(OpCodes.Br_S, operand, name);
				}
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x000198B4 File Offset: 0x00017AB4
			public Br_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F9 RID: 249
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Brfalse_S_ : CodeMatch
		{
			// Token: 0x17000140 RID: 320
			public Code.Brfalse_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brfalse_S_)base.Set(OpCodes.Brfalse_S, operand, name);
				}
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x000198E8 File Offset: 0x00017AE8
			public Brfalse_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FA RID: 250
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Brtrue_S_ : CodeMatch
		{
			// Token: 0x17000141 RID: 321
			public Code.Brtrue_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brtrue_S_)base.Set(OpCodes.Brtrue_S, operand, name);
				}
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x0001991C File Offset: 0x00017B1C
			public Brtrue_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FB RID: 251
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Beq_S_ : CodeMatch
		{
			// Token: 0x17000142 RID: 322
			public Code.Beq_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Beq_S_)base.Set(OpCodes.Beq_S, operand, name);
				}
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x00019950 File Offset: 0x00017B50
			public Beq_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FC RID: 252
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bge_S_ : CodeMatch
		{
			// Token: 0x17000143 RID: 323
			public Code.Bge_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_S_)base.Set(OpCodes.Bge_S, operand, name);
				}
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x00019984 File Offset: 0x00017B84
			public Bge_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FD RID: 253
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bgt_S_ : CodeMatch
		{
			// Token: 0x17000144 RID: 324
			public Code.Bgt_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_S_)base.Set(OpCodes.Bgt_S, operand, name);
				}
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x000199B8 File Offset: 0x00017BB8
			public Bgt_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FE RID: 254
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ble_S_ : CodeMatch
		{
			// Token: 0x17000145 RID: 325
			public Code.Ble_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_S_)base.Set(OpCodes.Ble_S, operand, name);
				}
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x000199EC File Offset: 0x00017BEC
			public Ble_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FF RID: 255
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Blt_S_ : CodeMatch
		{
			// Token: 0x17000146 RID: 326
			public Code.Blt_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_S_)base.Set(OpCodes.Blt_S, operand, name);
				}
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x00019A20 File Offset: 0x00017C20
			public Blt_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000100 RID: 256
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bne_Un_S_ : CodeMatch
		{
			// Token: 0x17000147 RID: 327
			public Code.Bne_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bne_Un_S_)base.Set(OpCodes.Bne_Un_S, operand, name);
				}
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x00019A54 File Offset: 0x00017C54
			public Bne_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000101 RID: 257
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bge_Un_S_ : CodeMatch
		{
			// Token: 0x17000148 RID: 328
			public Code.Bge_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_Un_S_)base.Set(OpCodes.Bge_Un_S, operand, name);
				}
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x00019A88 File Offset: 0x00017C88
			public Bge_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000102 RID: 258
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bgt_Un_S_ : CodeMatch
		{
			// Token: 0x17000149 RID: 329
			public Code.Bgt_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_Un_S_)base.Set(OpCodes.Bgt_Un_S, operand, name);
				}
			}

			// Token: 0x0600061F RID: 1567 RVA: 0x00019ABC File Offset: 0x00017CBC
			public Bgt_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000103 RID: 259
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ble_Un_S_ : CodeMatch
		{
			// Token: 0x1700014A RID: 330
			public Code.Ble_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_Un_S_)base.Set(OpCodes.Ble_Un_S, operand, name);
				}
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x00019AF0 File Offset: 0x00017CF0
			public Ble_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000104 RID: 260
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Blt_Un_S_ : CodeMatch
		{
			// Token: 0x1700014B RID: 331
			public Code.Blt_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_Un_S_)base.Set(OpCodes.Blt_Un_S, operand, name);
				}
			}

			// Token: 0x06000623 RID: 1571 RVA: 0x00019B24 File Offset: 0x00017D24
			public Blt_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000105 RID: 261
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Br_ : CodeMatch
		{
			// Token: 0x1700014C RID: 332
			public Code.Br_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Br_)base.Set(OpCodes.Br, operand, name);
				}
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x00019B58 File Offset: 0x00017D58
			public Br_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000106 RID: 262
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Brfalse_ : CodeMatch
		{
			// Token: 0x1700014D RID: 333
			public Code.Brfalse_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brfalse_)base.Set(OpCodes.Brfalse, operand, name);
				}
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x00019B8C File Offset: 0x00017D8C
			public Brfalse_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000107 RID: 263
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Brtrue_ : CodeMatch
		{
			// Token: 0x1700014E RID: 334
			public Code.Brtrue_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brtrue_)base.Set(OpCodes.Brtrue, operand, name);
				}
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x00019BC0 File Offset: 0x00017DC0
			public Brtrue_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000108 RID: 264
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Beq_ : CodeMatch
		{
			// Token: 0x1700014F RID: 335
			public Code.Beq_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Beq_)base.Set(OpCodes.Beq, operand, name);
				}
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x00019BF4 File Offset: 0x00017DF4
			public Beq_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000109 RID: 265
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bge_ : CodeMatch
		{
			// Token: 0x17000150 RID: 336
			public Code.Bge_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_)base.Set(OpCodes.Bge, operand, name);
				}
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x00019C28 File Offset: 0x00017E28
			public Bge_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010A RID: 266
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bgt_ : CodeMatch
		{
			// Token: 0x17000151 RID: 337
			public Code.Bgt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_)base.Set(OpCodes.Bgt, operand, name);
				}
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x00019C5C File Offset: 0x00017E5C
			public Bgt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010B RID: 267
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ble_ : CodeMatch
		{
			// Token: 0x17000152 RID: 338
			public Code.Ble_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_)base.Set(OpCodes.Ble, operand, name);
				}
			}

			// Token: 0x06000631 RID: 1585 RVA: 0x00019C90 File Offset: 0x00017E90
			public Ble_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010C RID: 268
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Blt_ : CodeMatch
		{
			// Token: 0x17000153 RID: 339
			public Code.Blt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_)base.Set(OpCodes.Blt, operand, name);
				}
			}

			// Token: 0x06000633 RID: 1587 RVA: 0x00019CC4 File Offset: 0x00017EC4
			public Blt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010D RID: 269
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bne_Un_ : CodeMatch
		{
			// Token: 0x17000154 RID: 340
			public Code.Bne_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bne_Un_)base.Set(OpCodes.Bne_Un, operand, name);
				}
			}

			// Token: 0x06000635 RID: 1589 RVA: 0x00019CF8 File Offset: 0x00017EF8
			public Bne_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010E RID: 270
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bge_Un_ : CodeMatch
		{
			// Token: 0x17000155 RID: 341
			public Code.Bge_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_Un_)base.Set(OpCodes.Bge_Un, operand, name);
				}
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x00019D2C File Offset: 0x00017F2C
			public Bge_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010F RID: 271
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Bgt_Un_ : CodeMatch
		{
			// Token: 0x17000156 RID: 342
			public Code.Bgt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_Un_)base.Set(OpCodes.Bgt_Un, operand, name);
				}
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x00019D60 File Offset: 0x00017F60
			public Bgt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000110 RID: 272
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ble_Un_ : CodeMatch
		{
			// Token: 0x17000157 RID: 343
			public Code.Ble_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_Un_)base.Set(OpCodes.Ble_Un, operand, name);
				}
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x00019D94 File Offset: 0x00017F94
			public Ble_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000111 RID: 273
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Blt_Un_ : CodeMatch
		{
			// Token: 0x17000158 RID: 344
			public Code.Blt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_Un_)base.Set(OpCodes.Blt_Un, operand, name);
				}
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x00019DC8 File Offset: 0x00017FC8
			public Blt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000112 RID: 274
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Switch_ : CodeMatch
		{
			// Token: 0x17000159 RID: 345
			public Code.Switch_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Switch_)base.Set(OpCodes.Switch, operand, name);
				}
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00019DFC File Offset: 0x00017FFC
			public Switch_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000113 RID: 275
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_I1_ : CodeMatch
		{
			// Token: 0x1700015A RID: 346
			public Code.Ldind_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I1_)base.Set(OpCodes.Ldind_I1, operand, name);
				}
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00019E30 File Offset: 0x00018030
			public Ldind_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000114 RID: 276
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_U1_ : CodeMatch
		{
			// Token: 0x1700015B RID: 347
			public Code.Ldind_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U1_)base.Set(OpCodes.Ldind_U1, operand, name);
				}
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x00019E64 File Offset: 0x00018064
			public Ldind_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000115 RID: 277
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_I2_ : CodeMatch
		{
			// Token: 0x1700015C RID: 348
			public Code.Ldind_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I2_)base.Set(OpCodes.Ldind_I2, operand, name);
				}
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x00019E98 File Offset: 0x00018098
			public Ldind_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000116 RID: 278
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_U2_ : CodeMatch
		{
			// Token: 0x1700015D RID: 349
			public Code.Ldind_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U2_)base.Set(OpCodes.Ldind_U2, operand, name);
				}
			}

			// Token: 0x06000647 RID: 1607 RVA: 0x00019ECC File Offset: 0x000180CC
			public Ldind_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000117 RID: 279
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_I4_ : CodeMatch
		{
			// Token: 0x1700015E RID: 350
			public Code.Ldind_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I4_)base.Set(OpCodes.Ldind_I4, operand, name);
				}
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x00019F00 File Offset: 0x00018100
			public Ldind_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000118 RID: 280
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_U4_ : CodeMatch
		{
			// Token: 0x1700015F RID: 351
			public Code.Ldind_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U4_)base.Set(OpCodes.Ldind_U4, operand, name);
				}
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x00019F34 File Offset: 0x00018134
			public Ldind_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000119 RID: 281
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_I8_ : CodeMatch
		{
			// Token: 0x17000160 RID: 352
			public Code.Ldind_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I8_)base.Set(OpCodes.Ldind_I8, operand, name);
				}
			}

			// Token: 0x0600064D RID: 1613 RVA: 0x00019F68 File Offset: 0x00018168
			public Ldind_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011A RID: 282
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_I_ : CodeMatch
		{
			// Token: 0x17000161 RID: 353
			public Code.Ldind_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I_)base.Set(OpCodes.Ldind_I, operand, name);
				}
			}

			// Token: 0x0600064F RID: 1615 RVA: 0x00019F9C File Offset: 0x0001819C
			public Ldind_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011B RID: 283
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_R4_ : CodeMatch
		{
			// Token: 0x17000162 RID: 354
			public Code.Ldind_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_R4_)base.Set(OpCodes.Ldind_R4, operand, name);
				}
			}

			// Token: 0x06000651 RID: 1617 RVA: 0x00019FD0 File Offset: 0x000181D0
			public Ldind_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011C RID: 284
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_R8_ : CodeMatch
		{
			// Token: 0x17000163 RID: 355
			public Code.Ldind_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_R8_)base.Set(OpCodes.Ldind_R8, operand, name);
				}
			}

			// Token: 0x06000653 RID: 1619 RVA: 0x0001A004 File Offset: 0x00018204
			public Ldind_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011D RID: 285
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldind_Ref_ : CodeMatch
		{
			// Token: 0x17000164 RID: 356
			public Code.Ldind_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_Ref_)base.Set(OpCodes.Ldind_Ref, operand, name);
				}
			}

			// Token: 0x06000655 RID: 1621 RVA: 0x0001A038 File Offset: 0x00018238
			public Ldind_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011E RID: 286
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_Ref_ : CodeMatch
		{
			// Token: 0x17000165 RID: 357
			public Code.Stind_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_Ref_)base.Set(OpCodes.Stind_Ref, operand, name);
				}
			}

			// Token: 0x06000657 RID: 1623 RVA: 0x0001A06C File Offset: 0x0001826C
			public Stind_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011F RID: 287
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_I1_ : CodeMatch
		{
			// Token: 0x17000166 RID: 358
			public Code.Stind_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I1_)base.Set(OpCodes.Stind_I1, operand, name);
				}
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x0001A0A0 File Offset: 0x000182A0
			public Stind_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000120 RID: 288
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_I2_ : CodeMatch
		{
			// Token: 0x17000167 RID: 359
			public Code.Stind_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I2_)base.Set(OpCodes.Stind_I2, operand, name);
				}
			}

			// Token: 0x0600065B RID: 1627 RVA: 0x0001A0D4 File Offset: 0x000182D4
			public Stind_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000121 RID: 289
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_I4_ : CodeMatch
		{
			// Token: 0x17000168 RID: 360
			public Code.Stind_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I4_)base.Set(OpCodes.Stind_I4, operand, name);
				}
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x0001A108 File Offset: 0x00018308
			public Stind_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000122 RID: 290
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_I8_ : CodeMatch
		{
			// Token: 0x17000169 RID: 361
			public Code.Stind_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I8_)base.Set(OpCodes.Stind_I8, operand, name);
				}
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x0001A13C File Offset: 0x0001833C
			public Stind_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000123 RID: 291
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_R4_ : CodeMatch
		{
			// Token: 0x1700016A RID: 362
			public Code.Stind_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_R4_)base.Set(OpCodes.Stind_R4, operand, name);
				}
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x0001A170 File Offset: 0x00018370
			public Stind_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000124 RID: 292
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_R8_ : CodeMatch
		{
			// Token: 0x1700016B RID: 363
			public Code.Stind_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_R8_)base.Set(OpCodes.Stind_R8, operand, name);
				}
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x0001A1A4 File Offset: 0x000183A4
			public Stind_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000125 RID: 293
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Add_ : CodeMatch
		{
			// Token: 0x1700016C RID: 364
			public Code.Add_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_)base.Set(OpCodes.Add, operand, name);
				}
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x0001A1D8 File Offset: 0x000183D8
			public Add_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000126 RID: 294
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Sub_ : CodeMatch
		{
			// Token: 0x1700016D RID: 365
			public Code.Sub_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_)base.Set(OpCodes.Sub, operand, name);
				}
			}

			// Token: 0x06000667 RID: 1639 RVA: 0x0001A20C File Offset: 0x0001840C
			public Sub_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000127 RID: 295
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Mul_ : CodeMatch
		{
			// Token: 0x1700016E RID: 366
			public Code.Mul_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_)base.Set(OpCodes.Mul, operand, name);
				}
			}

			// Token: 0x06000669 RID: 1641 RVA: 0x0001A240 File Offset: 0x00018440
			public Mul_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000128 RID: 296
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Div_ : CodeMatch
		{
			// Token: 0x1700016F RID: 367
			public Code.Div_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Div_)base.Set(OpCodes.Div, operand, name);
				}
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x0001A274 File Offset: 0x00018474
			public Div_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000129 RID: 297
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Div_Un_ : CodeMatch
		{
			// Token: 0x17000170 RID: 368
			public Code.Div_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Div_Un_)base.Set(OpCodes.Div_Un, operand, name);
				}
			}

			// Token: 0x0600066D RID: 1645 RVA: 0x0001A2A8 File Offset: 0x000184A8
			public Div_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012A RID: 298
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Rem_ : CodeMatch
		{
			// Token: 0x17000171 RID: 369
			public Code.Rem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rem_)base.Set(OpCodes.Rem, operand, name);
				}
			}

			// Token: 0x0600066F RID: 1647 RVA: 0x0001A2DC File Offset: 0x000184DC
			public Rem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012B RID: 299
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Rem_Un_ : CodeMatch
		{
			// Token: 0x17000172 RID: 370
			public Code.Rem_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rem_Un_)base.Set(OpCodes.Rem_Un, operand, name);
				}
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x0001A310 File Offset: 0x00018510
			public Rem_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012C RID: 300
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class And_ : CodeMatch
		{
			// Token: 0x17000173 RID: 371
			public Code.And_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.And_)base.Set(OpCodes.And, operand, name);
				}
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x0001A344 File Offset: 0x00018544
			public And_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012D RID: 301
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Or_ : CodeMatch
		{
			// Token: 0x17000174 RID: 372
			public Code.Or_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Or_)base.Set(OpCodes.Or, operand, name);
				}
			}

			// Token: 0x06000675 RID: 1653 RVA: 0x0001A378 File Offset: 0x00018578
			public Or_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012E RID: 302
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Xor_ : CodeMatch
		{
			// Token: 0x17000175 RID: 373
			public Code.Xor_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Xor_)base.Set(OpCodes.Xor, operand, name);
				}
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x0001A3AC File Offset: 0x000185AC
			public Xor_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012F RID: 303
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Shl_ : CodeMatch
		{
			// Token: 0x17000176 RID: 374
			public Code.Shl_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shl_)base.Set(OpCodes.Shl, operand, name);
				}
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x0001A3E0 File Offset: 0x000185E0
			public Shl_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000130 RID: 304
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Shr_ : CodeMatch
		{
			// Token: 0x17000177 RID: 375
			public Code.Shr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shr_)base.Set(OpCodes.Shr, operand, name);
				}
			}

			// Token: 0x0600067B RID: 1659 RVA: 0x0001A414 File Offset: 0x00018614
			public Shr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000131 RID: 305
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Shr_Un_ : CodeMatch
		{
			// Token: 0x17000178 RID: 376
			public Code.Shr_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shr_Un_)base.Set(OpCodes.Shr_Un, operand, name);
				}
			}

			// Token: 0x0600067D RID: 1661 RVA: 0x0001A448 File Offset: 0x00018648
			public Shr_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000132 RID: 306
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Neg_ : CodeMatch
		{
			// Token: 0x17000179 RID: 377
			public Code.Neg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Neg_)base.Set(OpCodes.Neg, operand, name);
				}
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x0001A47C File Offset: 0x0001867C
			public Neg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000133 RID: 307
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Not_ : CodeMatch
		{
			// Token: 0x1700017A RID: 378
			public Code.Not_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Not_)base.Set(OpCodes.Not, operand, name);
				}
			}

			// Token: 0x06000681 RID: 1665 RVA: 0x0001A4B0 File Offset: 0x000186B0
			public Not_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000134 RID: 308
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_I1_ : CodeMatch
		{
			// Token: 0x1700017B RID: 379
			public Code.Conv_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I1_)base.Set(OpCodes.Conv_I1, operand, name);
				}
			}

			// Token: 0x06000683 RID: 1667 RVA: 0x0001A4E4 File Offset: 0x000186E4
			public Conv_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000135 RID: 309
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_I2_ : CodeMatch
		{
			// Token: 0x1700017C RID: 380
			public Code.Conv_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I2_)base.Set(OpCodes.Conv_I2, operand, name);
				}
			}

			// Token: 0x06000685 RID: 1669 RVA: 0x0001A518 File Offset: 0x00018718
			public Conv_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000136 RID: 310
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_I4_ : CodeMatch
		{
			// Token: 0x1700017D RID: 381
			public Code.Conv_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I4_)base.Set(OpCodes.Conv_I4, operand, name);
				}
			}

			// Token: 0x06000687 RID: 1671 RVA: 0x0001A54C File Offset: 0x0001874C
			public Conv_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000137 RID: 311
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_I8_ : CodeMatch
		{
			// Token: 0x1700017E RID: 382
			public Code.Conv_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I8_)base.Set(OpCodes.Conv_I8, operand, name);
				}
			}

			// Token: 0x06000689 RID: 1673 RVA: 0x0001A580 File Offset: 0x00018780
			public Conv_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000138 RID: 312
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_R4_ : CodeMatch
		{
			// Token: 0x1700017F RID: 383
			public Code.Conv_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R4_)base.Set(OpCodes.Conv_R4, operand, name);
				}
			}

			// Token: 0x0600068B RID: 1675 RVA: 0x0001A5B4 File Offset: 0x000187B4
			public Conv_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000139 RID: 313
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_R8_ : CodeMatch
		{
			// Token: 0x17000180 RID: 384
			public Code.Conv_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R8_)base.Set(OpCodes.Conv_R8, operand, name);
				}
			}

			// Token: 0x0600068D RID: 1677 RVA: 0x0001A5E8 File Offset: 0x000187E8
			public Conv_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013A RID: 314
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_U4_ : CodeMatch
		{
			// Token: 0x17000181 RID: 385
			public Code.Conv_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U4_)base.Set(OpCodes.Conv_U4, operand, name);
				}
			}

			// Token: 0x0600068F RID: 1679 RVA: 0x0001A61C File Offset: 0x0001881C
			public Conv_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013B RID: 315
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_U8_ : CodeMatch
		{
			// Token: 0x17000182 RID: 386
			public Code.Conv_U8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U8_)base.Set(OpCodes.Conv_U8, operand, name);
				}
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x0001A650 File Offset: 0x00018850
			public Conv_U8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013C RID: 316
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Callvirt_ : CodeMatch
		{
			// Token: 0x17000183 RID: 387
			public Code.Callvirt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Callvirt_)base.Set(OpCodes.Callvirt, operand, name);
				}
			}

			// Token: 0x06000693 RID: 1683 RVA: 0x0001A684 File Offset: 0x00018884
			public Callvirt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013D RID: 317
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Cpobj_ : CodeMatch
		{
			// Token: 0x17000184 RID: 388
			public Code.Cpobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cpobj_)base.Set(OpCodes.Cpobj, operand, name);
				}
			}

			// Token: 0x06000695 RID: 1685 RVA: 0x0001A6B8 File Offset: 0x000188B8
			public Cpobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013E RID: 318
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldobj_ : CodeMatch
		{
			// Token: 0x17000185 RID: 389
			public Code.Ldobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldobj_)base.Set(OpCodes.Ldobj, operand, name);
				}
			}

			// Token: 0x06000697 RID: 1687 RVA: 0x0001A6EC File Offset: 0x000188EC
			public Ldobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013F RID: 319
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldstr_ : CodeMatch
		{
			// Token: 0x17000186 RID: 390
			public Code.Ldstr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldstr_)base.Set(OpCodes.Ldstr, operand, name);
				}
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x0001A720 File Offset: 0x00018920
			public Ldstr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000140 RID: 320
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Newobj_ : CodeMatch
		{
			// Token: 0x17000187 RID: 391
			public Code.Newobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Newobj_)base.Set(OpCodes.Newobj, operand, name);
				}
			}

			// Token: 0x0600069B RID: 1691 RVA: 0x0001A754 File Offset: 0x00018954
			public Newobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000141 RID: 321
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Castclass_ : CodeMatch
		{
			// Token: 0x17000188 RID: 392
			public Code.Castclass_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Castclass_)base.Set(OpCodes.Castclass, operand, name);
				}
			}

			// Token: 0x0600069D RID: 1693 RVA: 0x0001A788 File Offset: 0x00018988
			public Castclass_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000142 RID: 322
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Isinst_ : CodeMatch
		{
			// Token: 0x17000189 RID: 393
			public Code.Isinst_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Isinst_)base.Set(OpCodes.Isinst, operand, name);
				}
			}

			// Token: 0x0600069F RID: 1695 RVA: 0x0001A7BC File Offset: 0x000189BC
			public Isinst_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000143 RID: 323
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_R_Un_ : CodeMatch
		{
			// Token: 0x1700018A RID: 394
			public Code.Conv_R_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R_Un_)base.Set(OpCodes.Conv_R_Un, operand, name);
				}
			}

			// Token: 0x060006A1 RID: 1697 RVA: 0x0001A7F0 File Offset: 0x000189F0
			public Conv_R_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000144 RID: 324
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Unbox_ : CodeMatch
		{
			// Token: 0x1700018B RID: 395
			public Code.Unbox_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unbox_)base.Set(OpCodes.Unbox, operand, name);
				}
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x0001A824 File Offset: 0x00018A24
			public Unbox_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000145 RID: 325
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Throw_ : CodeMatch
		{
			// Token: 0x1700018C RID: 396
			public Code.Throw_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Throw_)base.Set(OpCodes.Throw, operand, name);
				}
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x0001A858 File Offset: 0x00018A58
			public Throw_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000146 RID: 326
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldfld_ : CodeMatch
		{
			// Token: 0x1700018D RID: 397
			public Code.Ldfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldfld_)base.Set(OpCodes.Ldfld, operand, name);
				}
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x0001A88C File Offset: 0x00018A8C
			public Ldfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000147 RID: 327
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldflda_ : CodeMatch
		{
			// Token: 0x1700018E RID: 398
			public Code.Ldflda_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldflda_)base.Set(OpCodes.Ldflda, operand, name);
				}
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x0001A8C0 File Offset: 0x00018AC0
			public Ldflda_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000148 RID: 328
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stfld_ : CodeMatch
		{
			// Token: 0x1700018F RID: 399
			public Code.Stfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stfld_)base.Set(OpCodes.Stfld, operand, name);
				}
			}

			// Token: 0x060006AB RID: 1707 RVA: 0x0001A8F4 File Offset: 0x00018AF4
			public Stfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000149 RID: 329
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldsfld_ : CodeMatch
		{
			// Token: 0x17000190 RID: 400
			public Code.Ldsfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldsfld_)base.Set(OpCodes.Ldsfld, operand, name);
				}
			}

			// Token: 0x060006AD RID: 1709 RVA: 0x0001A928 File Offset: 0x00018B28
			public Ldsfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014A RID: 330
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldsflda_ : CodeMatch
		{
			// Token: 0x17000191 RID: 401
			public Code.Ldsflda_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldsflda_)base.Set(OpCodes.Ldsflda, operand, name);
				}
			}

			// Token: 0x060006AF RID: 1711 RVA: 0x0001A95C File Offset: 0x00018B5C
			public Ldsflda_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014B RID: 331
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stsfld_ : CodeMatch
		{
			// Token: 0x17000192 RID: 402
			public Code.Stsfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stsfld_)base.Set(OpCodes.Stsfld, operand, name);
				}
			}

			// Token: 0x060006B1 RID: 1713 RVA: 0x0001A990 File Offset: 0x00018B90
			public Stsfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014C RID: 332
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stobj_ : CodeMatch
		{
			// Token: 0x17000193 RID: 403
			public Code.Stobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stobj_)base.Set(OpCodes.Stobj, operand, name);
				}
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x0001A9C4 File Offset: 0x00018BC4
			public Stobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014D RID: 333
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I1_Un_ : CodeMatch
		{
			// Token: 0x17000194 RID: 404
			public Code.Conv_Ovf_I1_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I1_Un_)base.Set(OpCodes.Conv_Ovf_I1_Un, operand, name);
				}
			}

			// Token: 0x060006B5 RID: 1717 RVA: 0x0001A9F8 File Offset: 0x00018BF8
			public Conv_Ovf_I1_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014E RID: 334
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I2_Un_ : CodeMatch
		{
			// Token: 0x17000195 RID: 405
			public Code.Conv_Ovf_I2_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I2_Un_)base.Set(OpCodes.Conv_Ovf_I2_Un, operand, name);
				}
			}

			// Token: 0x060006B7 RID: 1719 RVA: 0x0001AA2C File Offset: 0x00018C2C
			public Conv_Ovf_I2_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014F RID: 335
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I4_Un_ : CodeMatch
		{
			// Token: 0x17000196 RID: 406
			public Code.Conv_Ovf_I4_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I4_Un_)base.Set(OpCodes.Conv_Ovf_I4_Un, operand, name);
				}
			}

			// Token: 0x060006B9 RID: 1721 RVA: 0x0001AA60 File Offset: 0x00018C60
			public Conv_Ovf_I4_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000150 RID: 336
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I8_Un_ : CodeMatch
		{
			// Token: 0x17000197 RID: 407
			public Code.Conv_Ovf_I8_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I8_Un_)base.Set(OpCodes.Conv_Ovf_I8_Un, operand, name);
				}
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x0001AA94 File Offset: 0x00018C94
			public Conv_Ovf_I8_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000151 RID: 337
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U1_Un_ : CodeMatch
		{
			// Token: 0x17000198 RID: 408
			public Code.Conv_Ovf_U1_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U1_Un_)base.Set(OpCodes.Conv_Ovf_U1_Un, operand, name);
				}
			}

			// Token: 0x060006BD RID: 1725 RVA: 0x0001AAC8 File Offset: 0x00018CC8
			public Conv_Ovf_U1_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000152 RID: 338
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U2_Un_ : CodeMatch
		{
			// Token: 0x17000199 RID: 409
			public Code.Conv_Ovf_U2_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U2_Un_)base.Set(OpCodes.Conv_Ovf_U2_Un, operand, name);
				}
			}

			// Token: 0x060006BF RID: 1727 RVA: 0x0001AAFC File Offset: 0x00018CFC
			public Conv_Ovf_U2_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000153 RID: 339
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U4_Un_ : CodeMatch
		{
			// Token: 0x1700019A RID: 410
			public Code.Conv_Ovf_U4_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U4_Un_)base.Set(OpCodes.Conv_Ovf_U4_Un, operand, name);
				}
			}

			// Token: 0x060006C1 RID: 1729 RVA: 0x0001AB30 File Offset: 0x00018D30
			public Conv_Ovf_U4_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000154 RID: 340
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U8_Un_ : CodeMatch
		{
			// Token: 0x1700019B RID: 411
			public Code.Conv_Ovf_U8_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U8_Un_)base.Set(OpCodes.Conv_Ovf_U8_Un, operand, name);
				}
			}

			// Token: 0x060006C3 RID: 1731 RVA: 0x0001AB64 File Offset: 0x00018D64
			public Conv_Ovf_U8_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000155 RID: 341
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I_Un_ : CodeMatch
		{
			// Token: 0x1700019C RID: 412
			public Code.Conv_Ovf_I_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I_Un_)base.Set(OpCodes.Conv_Ovf_I_Un, operand, name);
				}
			}

			// Token: 0x060006C5 RID: 1733 RVA: 0x0001AB98 File Offset: 0x00018D98
			public Conv_Ovf_I_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000156 RID: 342
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U_Un_ : CodeMatch
		{
			// Token: 0x1700019D RID: 413
			public Code.Conv_Ovf_U_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U_Un_)base.Set(OpCodes.Conv_Ovf_U_Un, operand, name);
				}
			}

			// Token: 0x060006C7 RID: 1735 RVA: 0x0001ABCC File Offset: 0x00018DCC
			public Conv_Ovf_U_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000157 RID: 343
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Box_ : CodeMatch
		{
			// Token: 0x1700019E RID: 414
			public Code.Box_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Box_)base.Set(OpCodes.Box, operand, name);
				}
			}

			// Token: 0x060006C9 RID: 1737 RVA: 0x0001AC00 File Offset: 0x00018E00
			public Box_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000158 RID: 344
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Newarr_ : CodeMatch
		{
			// Token: 0x1700019F RID: 415
			public Code.Newarr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Newarr_)base.Set(OpCodes.Newarr, operand, name);
				}
			}

			// Token: 0x060006CB RID: 1739 RVA: 0x0001AC34 File Offset: 0x00018E34
			public Newarr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000159 RID: 345
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldlen_ : CodeMatch
		{
			// Token: 0x170001A0 RID: 416
			public Code.Ldlen_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldlen_)base.Set(OpCodes.Ldlen, operand, name);
				}
			}

			// Token: 0x060006CD RID: 1741 RVA: 0x0001AC68 File Offset: 0x00018E68
			public Ldlen_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015A RID: 346
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelema_ : CodeMatch
		{
			// Token: 0x170001A1 RID: 417
			public Code.Ldelema_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelema_)base.Set(OpCodes.Ldelema, operand, name);
				}
			}

			// Token: 0x060006CF RID: 1743 RVA: 0x0001AC9C File Offset: 0x00018E9C
			public Ldelema_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015B RID: 347
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_I1_ : CodeMatch
		{
			// Token: 0x170001A2 RID: 418
			public Code.Ldelem_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I1_)base.Set(OpCodes.Ldelem_I1, operand, name);
				}
			}

			// Token: 0x060006D1 RID: 1745 RVA: 0x0001ACD0 File Offset: 0x00018ED0
			public Ldelem_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015C RID: 348
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_U1_ : CodeMatch
		{
			// Token: 0x170001A3 RID: 419
			public Code.Ldelem_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U1_)base.Set(OpCodes.Ldelem_U1, operand, name);
				}
			}

			// Token: 0x060006D3 RID: 1747 RVA: 0x0001AD04 File Offset: 0x00018F04
			public Ldelem_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015D RID: 349
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_I2_ : CodeMatch
		{
			// Token: 0x170001A4 RID: 420
			public Code.Ldelem_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I2_)base.Set(OpCodes.Ldelem_I2, operand, name);
				}
			}

			// Token: 0x060006D5 RID: 1749 RVA: 0x0001AD38 File Offset: 0x00018F38
			public Ldelem_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015E RID: 350
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_U2_ : CodeMatch
		{
			// Token: 0x170001A5 RID: 421
			public Code.Ldelem_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U2_)base.Set(OpCodes.Ldelem_U2, operand, name);
				}
			}

			// Token: 0x060006D7 RID: 1751 RVA: 0x0001AD6C File Offset: 0x00018F6C
			public Ldelem_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015F RID: 351
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_I4_ : CodeMatch
		{
			// Token: 0x170001A6 RID: 422
			public Code.Ldelem_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I4_)base.Set(OpCodes.Ldelem_I4, operand, name);
				}
			}

			// Token: 0x060006D9 RID: 1753 RVA: 0x0001ADA0 File Offset: 0x00018FA0
			public Ldelem_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000160 RID: 352
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_U4_ : CodeMatch
		{
			// Token: 0x170001A7 RID: 423
			public Code.Ldelem_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U4_)base.Set(OpCodes.Ldelem_U4, operand, name);
				}
			}

			// Token: 0x060006DB RID: 1755 RVA: 0x0001ADD4 File Offset: 0x00018FD4
			public Ldelem_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000161 RID: 353
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_I8_ : CodeMatch
		{
			// Token: 0x170001A8 RID: 424
			public Code.Ldelem_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I8_)base.Set(OpCodes.Ldelem_I8, operand, name);
				}
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x0001AE08 File Offset: 0x00019008
			public Ldelem_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000162 RID: 354
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_I_ : CodeMatch
		{
			// Token: 0x170001A9 RID: 425
			public Code.Ldelem_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I_)base.Set(OpCodes.Ldelem_I, operand, name);
				}
			}

			// Token: 0x060006DF RID: 1759 RVA: 0x0001AE3C File Offset: 0x0001903C
			public Ldelem_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000163 RID: 355
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_R4_ : CodeMatch
		{
			// Token: 0x170001AA RID: 426
			public Code.Ldelem_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_R4_)base.Set(OpCodes.Ldelem_R4, operand, name);
				}
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x0001AE70 File Offset: 0x00019070
			public Ldelem_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000164 RID: 356
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_R8_ : CodeMatch
		{
			// Token: 0x170001AB RID: 427
			public Code.Ldelem_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_R8_)base.Set(OpCodes.Ldelem_R8, operand, name);
				}
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x0001AEA4 File Offset: 0x000190A4
			public Ldelem_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000165 RID: 357
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_Ref_ : CodeMatch
		{
			// Token: 0x170001AC RID: 428
			public Code.Ldelem_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_Ref_)base.Set(OpCodes.Ldelem_Ref, operand, name);
				}
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x0001AED8 File Offset: 0x000190D8
			public Ldelem_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000166 RID: 358
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_I_ : CodeMatch
		{
			// Token: 0x170001AD RID: 429
			public Code.Stelem_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I_)base.Set(OpCodes.Stelem_I, operand, name);
				}
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x0001AF0C File Offset: 0x0001910C
			public Stelem_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000167 RID: 359
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_I1_ : CodeMatch
		{
			// Token: 0x170001AE RID: 430
			public Code.Stelem_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I1_)base.Set(OpCodes.Stelem_I1, operand, name);
				}
			}

			// Token: 0x060006E9 RID: 1769 RVA: 0x0001AF40 File Offset: 0x00019140
			public Stelem_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000168 RID: 360
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_I2_ : CodeMatch
		{
			// Token: 0x170001AF RID: 431
			public Code.Stelem_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I2_)base.Set(OpCodes.Stelem_I2, operand, name);
				}
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x0001AF74 File Offset: 0x00019174
			public Stelem_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000169 RID: 361
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_I4_ : CodeMatch
		{
			// Token: 0x170001B0 RID: 432
			public Code.Stelem_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I4_)base.Set(OpCodes.Stelem_I4, operand, name);
				}
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x0001AFA8 File Offset: 0x000191A8
			public Stelem_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016A RID: 362
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_I8_ : CodeMatch
		{
			// Token: 0x170001B1 RID: 433
			public Code.Stelem_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I8_)base.Set(OpCodes.Stelem_I8, operand, name);
				}
			}

			// Token: 0x060006EF RID: 1775 RVA: 0x0001AFDC File Offset: 0x000191DC
			public Stelem_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016B RID: 363
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_R4_ : CodeMatch
		{
			// Token: 0x170001B2 RID: 434
			public Code.Stelem_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_R4_)base.Set(OpCodes.Stelem_R4, operand, name);
				}
			}

			// Token: 0x060006F1 RID: 1777 RVA: 0x0001B010 File Offset: 0x00019210
			public Stelem_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016C RID: 364
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_R8_ : CodeMatch
		{
			// Token: 0x170001B3 RID: 435
			public Code.Stelem_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_R8_)base.Set(OpCodes.Stelem_R8, operand, name);
				}
			}

			// Token: 0x060006F3 RID: 1779 RVA: 0x0001B044 File Offset: 0x00019244
			public Stelem_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016D RID: 365
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_Ref_ : CodeMatch
		{
			// Token: 0x170001B4 RID: 436
			public Code.Stelem_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_Ref_)base.Set(OpCodes.Stelem_Ref, operand, name);
				}
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x0001B078 File Offset: 0x00019278
			public Stelem_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016E RID: 366
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldelem_ : CodeMatch
		{
			// Token: 0x170001B5 RID: 437
			public Code.Ldelem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_)base.Set(OpCodes.Ldelem, operand, name);
				}
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x0001B0AC File Offset: 0x000192AC
			public Ldelem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016F RID: 367
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stelem_ : CodeMatch
		{
			// Token: 0x170001B6 RID: 438
			public Code.Stelem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_)base.Set(OpCodes.Stelem, operand, name);
				}
			}

			// Token: 0x060006F9 RID: 1785 RVA: 0x0001B0E0 File Offset: 0x000192E0
			public Stelem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000170 RID: 368
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Unbox_Any_ : CodeMatch
		{
			// Token: 0x170001B7 RID: 439
			public Code.Unbox_Any_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unbox_Any_)base.Set(OpCodes.Unbox_Any, operand, name);
				}
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x0001B114 File Offset: 0x00019314
			public Unbox_Any_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000171 RID: 369
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I1_ : CodeMatch
		{
			// Token: 0x170001B8 RID: 440
			public Code.Conv_Ovf_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I1_)base.Set(OpCodes.Conv_Ovf_I1, operand, name);
				}
			}

			// Token: 0x060006FD RID: 1789 RVA: 0x0001B148 File Offset: 0x00019348
			public Conv_Ovf_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000172 RID: 370
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U1_ : CodeMatch
		{
			// Token: 0x170001B9 RID: 441
			public Code.Conv_Ovf_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U1_)base.Set(OpCodes.Conv_Ovf_U1, operand, name);
				}
			}

			// Token: 0x060006FF RID: 1791 RVA: 0x0001B17C File Offset: 0x0001937C
			public Conv_Ovf_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000173 RID: 371
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I2_ : CodeMatch
		{
			// Token: 0x170001BA RID: 442
			public Code.Conv_Ovf_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I2_)base.Set(OpCodes.Conv_Ovf_I2, operand, name);
				}
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x0001B1B0 File Offset: 0x000193B0
			public Conv_Ovf_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000174 RID: 372
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U2_ : CodeMatch
		{
			// Token: 0x170001BB RID: 443
			public Code.Conv_Ovf_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U2_)base.Set(OpCodes.Conv_Ovf_U2, operand, name);
				}
			}

			// Token: 0x06000703 RID: 1795 RVA: 0x0001B1E4 File Offset: 0x000193E4
			public Conv_Ovf_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000175 RID: 373
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I4_ : CodeMatch
		{
			// Token: 0x170001BC RID: 444
			public Code.Conv_Ovf_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I4_)base.Set(OpCodes.Conv_Ovf_I4, operand, name);
				}
			}

			// Token: 0x06000705 RID: 1797 RVA: 0x0001B218 File Offset: 0x00019418
			public Conv_Ovf_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000176 RID: 374
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U4_ : CodeMatch
		{
			// Token: 0x170001BD RID: 445
			public Code.Conv_Ovf_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U4_)base.Set(OpCodes.Conv_Ovf_U4, operand, name);
				}
			}

			// Token: 0x06000707 RID: 1799 RVA: 0x0001B24C File Offset: 0x0001944C
			public Conv_Ovf_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000177 RID: 375
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I8_ : CodeMatch
		{
			// Token: 0x170001BE RID: 446
			public Code.Conv_Ovf_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I8_)base.Set(OpCodes.Conv_Ovf_I8, operand, name);
				}
			}

			// Token: 0x06000709 RID: 1801 RVA: 0x0001B280 File Offset: 0x00019480
			public Conv_Ovf_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000178 RID: 376
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U8_ : CodeMatch
		{
			// Token: 0x170001BF RID: 447
			public Code.Conv_Ovf_U8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U8_)base.Set(OpCodes.Conv_Ovf_U8, operand, name);
				}
			}

			// Token: 0x0600070B RID: 1803 RVA: 0x0001B2B4 File Offset: 0x000194B4
			public Conv_Ovf_U8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000179 RID: 377
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Refanyval_ : CodeMatch
		{
			// Token: 0x170001C0 RID: 448
			public Code.Refanyval_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Refanyval_)base.Set(OpCodes.Refanyval, operand, name);
				}
			}

			// Token: 0x0600070D RID: 1805 RVA: 0x0001B2E8 File Offset: 0x000194E8
			public Refanyval_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017A RID: 378
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ckfinite_ : CodeMatch
		{
			// Token: 0x170001C1 RID: 449
			public Code.Ckfinite_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ckfinite_)base.Set(OpCodes.Ckfinite, operand, name);
				}
			}

			// Token: 0x0600070F RID: 1807 RVA: 0x0001B31C File Offset: 0x0001951C
			public Ckfinite_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017B RID: 379
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Mkrefany_ : CodeMatch
		{
			// Token: 0x170001C2 RID: 450
			public Code.Mkrefany_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mkrefany_)base.Set(OpCodes.Mkrefany, operand, name);
				}
			}

			// Token: 0x06000711 RID: 1809 RVA: 0x0001B350 File Offset: 0x00019550
			public Mkrefany_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017C RID: 380
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldtoken_ : CodeMatch
		{
			// Token: 0x170001C3 RID: 451
			public Code.Ldtoken_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldtoken_)base.Set(OpCodes.Ldtoken, operand, name);
				}
			}

			// Token: 0x06000713 RID: 1811 RVA: 0x0001B384 File Offset: 0x00019584
			public Ldtoken_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017D RID: 381
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_U2_ : CodeMatch
		{
			// Token: 0x170001C4 RID: 452
			public Code.Conv_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U2_)base.Set(OpCodes.Conv_U2, operand, name);
				}
			}

			// Token: 0x06000715 RID: 1813 RVA: 0x0001B3B8 File Offset: 0x000195B8
			public Conv_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017E RID: 382
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_U1_ : CodeMatch
		{
			// Token: 0x170001C5 RID: 453
			public Code.Conv_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U1_)base.Set(OpCodes.Conv_U1, operand, name);
				}
			}

			// Token: 0x06000717 RID: 1815 RVA: 0x0001B3EC File Offset: 0x000195EC
			public Conv_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017F RID: 383
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_I_ : CodeMatch
		{
			// Token: 0x170001C6 RID: 454
			public Code.Conv_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I_)base.Set(OpCodes.Conv_I, operand, name);
				}
			}

			// Token: 0x06000719 RID: 1817 RVA: 0x0001B420 File Offset: 0x00019620
			public Conv_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000180 RID: 384
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_I_ : CodeMatch
		{
			// Token: 0x170001C7 RID: 455
			public Code.Conv_Ovf_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I_)base.Set(OpCodes.Conv_Ovf_I, operand, name);
				}
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x0001B454 File Offset: 0x00019654
			public Conv_Ovf_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000181 RID: 385
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_Ovf_U_ : CodeMatch
		{
			// Token: 0x170001C8 RID: 456
			public Code.Conv_Ovf_U_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U_)base.Set(OpCodes.Conv_Ovf_U, operand, name);
				}
			}

			// Token: 0x0600071D RID: 1821 RVA: 0x0001B488 File Offset: 0x00019688
			public Conv_Ovf_U_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000182 RID: 386
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Add_Ovf_ : CodeMatch
		{
			// Token: 0x170001C9 RID: 457
			public Code.Add_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_Ovf_)base.Set(OpCodes.Add_Ovf, operand, name);
				}
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x0001B4BC File Offset: 0x000196BC
			public Add_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000183 RID: 387
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Add_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001CA RID: 458
			public Code.Add_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_Ovf_Un_)base.Set(OpCodes.Add_Ovf_Un, operand, name);
				}
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x0001B4F0 File Offset: 0x000196F0
			public Add_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000184 RID: 388
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Mul_Ovf_ : CodeMatch
		{
			// Token: 0x170001CB RID: 459
			public Code.Mul_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_Ovf_)base.Set(OpCodes.Mul_Ovf, operand, name);
				}
			}

			// Token: 0x06000723 RID: 1827 RVA: 0x0001B524 File Offset: 0x00019724
			public Mul_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000185 RID: 389
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Mul_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001CC RID: 460
			public Code.Mul_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_Ovf_Un_)base.Set(OpCodes.Mul_Ovf_Un, operand, name);
				}
			}

			// Token: 0x06000725 RID: 1829 RVA: 0x0001B558 File Offset: 0x00019758
			public Mul_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000186 RID: 390
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Sub_Ovf_ : CodeMatch
		{
			// Token: 0x170001CD RID: 461
			public Code.Sub_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_Ovf_)base.Set(OpCodes.Sub_Ovf, operand, name);
				}
			}

			// Token: 0x06000727 RID: 1831 RVA: 0x0001B58C File Offset: 0x0001978C
			public Sub_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000187 RID: 391
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Sub_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001CE RID: 462
			public Code.Sub_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_Ovf_Un_)base.Set(OpCodes.Sub_Ovf_Un, operand, name);
				}
			}

			// Token: 0x06000729 RID: 1833 RVA: 0x0001B5C0 File Offset: 0x000197C0
			public Sub_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000188 RID: 392
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Endfinally_ : CodeMatch
		{
			// Token: 0x170001CF RID: 463
			public Code.Endfinally_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Endfinally_)base.Set(OpCodes.Endfinally, operand, name);
				}
			}

			// Token: 0x0600072B RID: 1835 RVA: 0x0001B5F4 File Offset: 0x000197F4
			public Endfinally_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000189 RID: 393
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Leave_ : CodeMatch
		{
			// Token: 0x170001D0 RID: 464
			public Code.Leave_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Leave_)base.Set(OpCodes.Leave, operand, name);
				}
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x0001B628 File Offset: 0x00019828
			public Leave_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018A RID: 394
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Leave_S_ : CodeMatch
		{
			// Token: 0x170001D1 RID: 465
			public Code.Leave_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Leave_S_)base.Set(OpCodes.Leave_S, operand, name);
				}
			}

			// Token: 0x0600072F RID: 1839 RVA: 0x0001B65C File Offset: 0x0001985C
			public Leave_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018B RID: 395
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stind_I_ : CodeMatch
		{
			// Token: 0x170001D2 RID: 466
			public Code.Stind_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I_)base.Set(OpCodes.Stind_I, operand, name);
				}
			}

			// Token: 0x06000731 RID: 1841 RVA: 0x0001B690 File Offset: 0x00019890
			public Stind_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018C RID: 396
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Conv_U_ : CodeMatch
		{
			// Token: 0x170001D3 RID: 467
			public Code.Conv_U_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U_)base.Set(OpCodes.Conv_U, operand, name);
				}
			}

			// Token: 0x06000733 RID: 1843 RVA: 0x0001B6C4 File Offset: 0x000198C4
			public Conv_U_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018D RID: 397
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix7_ : CodeMatch
		{
			// Token: 0x170001D4 RID: 468
			public Code.Prefix7_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix7_)base.Set(OpCodes.Prefix7, operand, name);
				}
			}

			// Token: 0x06000735 RID: 1845 RVA: 0x0001B6F8 File Offset: 0x000198F8
			public Prefix7_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018E RID: 398
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix6_ : CodeMatch
		{
			// Token: 0x170001D5 RID: 469
			public Code.Prefix6_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix6_)base.Set(OpCodes.Prefix6, operand, name);
				}
			}

			// Token: 0x06000737 RID: 1847 RVA: 0x0001B72C File Offset: 0x0001992C
			public Prefix6_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200018F RID: 399
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix5_ : CodeMatch
		{
			// Token: 0x170001D6 RID: 470
			public Code.Prefix5_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix5_)base.Set(OpCodes.Prefix5, operand, name);
				}
			}

			// Token: 0x06000739 RID: 1849 RVA: 0x0001B760 File Offset: 0x00019960
			public Prefix5_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000190 RID: 400
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix4_ : CodeMatch
		{
			// Token: 0x170001D7 RID: 471
			public Code.Prefix4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix4_)base.Set(OpCodes.Prefix4, operand, name);
				}
			}

			// Token: 0x0600073B RID: 1851 RVA: 0x0001B794 File Offset: 0x00019994
			public Prefix4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000191 RID: 401
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix3_ : CodeMatch
		{
			// Token: 0x170001D8 RID: 472
			public Code.Prefix3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix3_)base.Set(OpCodes.Prefix3, operand, name);
				}
			}

			// Token: 0x0600073D RID: 1853 RVA: 0x0001B7C8 File Offset: 0x000199C8
			public Prefix3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000192 RID: 402
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix2_ : CodeMatch
		{
			// Token: 0x170001D9 RID: 473
			public Code.Prefix2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix2_)base.Set(OpCodes.Prefix2, operand, name);
				}
			}

			// Token: 0x0600073F RID: 1855 RVA: 0x0001B7FC File Offset: 0x000199FC
			public Prefix2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000193 RID: 403
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefix1_ : CodeMatch
		{
			// Token: 0x170001DA RID: 474
			public Code.Prefix1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix1_)base.Set(OpCodes.Prefix1, operand, name);
				}
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x0001B830 File Offset: 0x00019A30
			public Prefix1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000194 RID: 404
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Prefixref_ : CodeMatch
		{
			// Token: 0x170001DB RID: 475
			public Code.Prefixref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefixref_)base.Set(OpCodes.Prefixref, operand, name);
				}
			}

			// Token: 0x06000743 RID: 1859 RVA: 0x0001B864 File Offset: 0x00019A64
			public Prefixref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000195 RID: 405
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Arglist_ : CodeMatch
		{
			// Token: 0x170001DC RID: 476
			public Code.Arglist_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Arglist_)base.Set(OpCodes.Arglist, operand, name);
				}
			}

			// Token: 0x06000745 RID: 1861 RVA: 0x0001B898 File Offset: 0x00019A98
			public Arglist_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000196 RID: 406
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ceq_ : CodeMatch
		{
			// Token: 0x170001DD RID: 477
			public Code.Ceq_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ceq_)base.Set(OpCodes.Ceq, operand, name);
				}
			}

			// Token: 0x06000747 RID: 1863 RVA: 0x0001B8CC File Offset: 0x00019ACC
			public Ceq_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000197 RID: 407
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Cgt_ : CodeMatch
		{
			// Token: 0x170001DE RID: 478
			public Code.Cgt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cgt_)base.Set(OpCodes.Cgt, operand, name);
				}
			}

			// Token: 0x06000749 RID: 1865 RVA: 0x0001B900 File Offset: 0x00019B00
			public Cgt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000198 RID: 408
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Cgt_Un_ : CodeMatch
		{
			// Token: 0x170001DF RID: 479
			public Code.Cgt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cgt_Un_)base.Set(OpCodes.Cgt_Un, operand, name);
				}
			}

			// Token: 0x0600074B RID: 1867 RVA: 0x0001B934 File Offset: 0x00019B34
			public Cgt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000199 RID: 409
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Clt_ : CodeMatch
		{
			// Token: 0x170001E0 RID: 480
			public Code.Clt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Clt_)base.Set(OpCodes.Clt, operand, name);
				}
			}

			// Token: 0x0600074D RID: 1869 RVA: 0x0001B968 File Offset: 0x00019B68
			public Clt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019A RID: 410
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Clt_Un_ : CodeMatch
		{
			// Token: 0x170001E1 RID: 481
			public Code.Clt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Clt_Un_)base.Set(OpCodes.Clt_Un, operand, name);
				}
			}

			// Token: 0x0600074F RID: 1871 RVA: 0x0001B99C File Offset: 0x00019B9C
			public Clt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019B RID: 411
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldftn_ : CodeMatch
		{
			// Token: 0x170001E2 RID: 482
			public Code.Ldftn_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldftn_)base.Set(OpCodes.Ldftn, operand, name);
				}
			}

			// Token: 0x06000751 RID: 1873 RVA: 0x0001B9D0 File Offset: 0x00019BD0
			public Ldftn_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019C RID: 412
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldvirtftn_ : CodeMatch
		{
			// Token: 0x170001E3 RID: 483
			public Code.Ldvirtftn_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldvirtftn_)base.Set(OpCodes.Ldvirtftn, operand, name);
				}
			}

			// Token: 0x06000753 RID: 1875 RVA: 0x0001BA04 File Offset: 0x00019C04
			public Ldvirtftn_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019D RID: 413
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarg_ : CodeMatch
		{
			// Token: 0x170001E4 RID: 484
			public Code.Ldarg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_)base.Set(OpCodes.Ldarg, operand, name);
				}
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x0001BA38 File Offset: 0x00019C38
			public Ldarg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019E RID: 414
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldarga_ : CodeMatch
		{
			// Token: 0x170001E5 RID: 485
			public Code.Ldarga_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarga_)base.Set(OpCodes.Ldarga, operand, name);
				}
			}

			// Token: 0x06000757 RID: 1879 RVA: 0x0001BA6C File Offset: 0x00019C6C
			public Ldarga_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200019F RID: 415
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Starg_ : CodeMatch
		{
			// Token: 0x170001E6 RID: 486
			public Code.Starg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Starg_)base.Set(OpCodes.Starg, operand, name);
				}
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x0001BAA0 File Offset: 0x00019CA0
			public Starg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A0 RID: 416
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloc_ : CodeMatch
		{
			// Token: 0x170001E7 RID: 487
			public Code.Ldloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_)base.Set(OpCodes.Ldloc, operand, name);
				}
			}

			// Token: 0x0600075B RID: 1883 RVA: 0x0001BAD4 File Offset: 0x00019CD4
			public Ldloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A1 RID: 417
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Ldloca_ : CodeMatch
		{
			// Token: 0x170001E8 RID: 488
			public Code.Ldloca_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloca_)base.Set(OpCodes.Ldloca, operand, name);
				}
			}

			// Token: 0x0600075D RID: 1885 RVA: 0x0001BB08 File Offset: 0x00019D08
			public Ldloca_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A2 RID: 418
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Stloc_ : CodeMatch
		{
			// Token: 0x170001E9 RID: 489
			public Code.Stloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_)base.Set(OpCodes.Stloc, operand, name);
				}
			}

			// Token: 0x0600075F RID: 1887 RVA: 0x0001BB3C File Offset: 0x00019D3C
			public Stloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A3 RID: 419
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Localloc_ : CodeMatch
		{
			// Token: 0x170001EA RID: 490
			public Code.Localloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Localloc_)base.Set(OpCodes.Localloc, operand, name);
				}
			}

			// Token: 0x06000761 RID: 1889 RVA: 0x0001BB70 File Offset: 0x00019D70
			public Localloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A4 RID: 420
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Endfilter_ : CodeMatch
		{
			// Token: 0x170001EB RID: 491
			public Code.Endfilter_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Endfilter_)base.Set(OpCodes.Endfilter, operand, name);
				}
			}

			// Token: 0x06000763 RID: 1891 RVA: 0x0001BBA4 File Offset: 0x00019DA4
			public Endfilter_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A5 RID: 421
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Unaligned_ : CodeMatch
		{
			// Token: 0x170001EC RID: 492
			public Code.Unaligned_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unaligned_)base.Set(OpCodes.Unaligned, operand, name);
				}
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x0001BBD8 File Offset: 0x00019DD8
			public Unaligned_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A6 RID: 422
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Volatile_ : CodeMatch
		{
			// Token: 0x170001ED RID: 493
			public Code.Volatile_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Volatile_)base.Set(OpCodes.Volatile, operand, name);
				}
			}

			// Token: 0x06000767 RID: 1895 RVA: 0x0001BC0C File Offset: 0x00019E0C
			public Volatile_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A7 RID: 423
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Tailcall_ : CodeMatch
		{
			// Token: 0x170001EE RID: 494
			public Code.Tailcall_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Tailcall_)base.Set(OpCodes.Tailcall, operand, name);
				}
			}

			// Token: 0x06000769 RID: 1897 RVA: 0x0001BC40 File Offset: 0x00019E40
			public Tailcall_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A8 RID: 424
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Initobj_ : CodeMatch
		{
			// Token: 0x170001EF RID: 495
			public Code.Initobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Initobj_)base.Set(OpCodes.Initobj, operand, name);
				}
			}

			// Token: 0x0600076B RID: 1899 RVA: 0x0001BC74 File Offset: 0x00019E74
			public Initobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001A9 RID: 425
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Constrained_ : CodeMatch
		{
			// Token: 0x170001F0 RID: 496
			public Code.Constrained_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Constrained_)base.Set(OpCodes.Constrained, operand, name);
				}
			}

			// Token: 0x0600076D RID: 1901 RVA: 0x0001BCA8 File Offset: 0x00019EA8
			public Constrained_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AA RID: 426
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Cpblk_ : CodeMatch
		{
			// Token: 0x170001F1 RID: 497
			public Code.Cpblk_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cpblk_)base.Set(OpCodes.Cpblk, operand, name);
				}
			}

			// Token: 0x0600076F RID: 1903 RVA: 0x0001BCDC File Offset: 0x00019EDC
			public Cpblk_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AB RID: 427
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Initblk_ : CodeMatch
		{
			// Token: 0x170001F2 RID: 498
			public Code.Initblk_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Initblk_)base.Set(OpCodes.Initblk, operand, name);
				}
			}

			// Token: 0x06000771 RID: 1905 RVA: 0x0001BD10 File Offset: 0x00019F10
			public Initblk_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AC RID: 428
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Rethrow_ : CodeMatch
		{
			// Token: 0x170001F3 RID: 499
			public Code.Rethrow_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rethrow_)base.Set(OpCodes.Rethrow, operand, name);
				}
			}

			// Token: 0x06000773 RID: 1907 RVA: 0x0001BD44 File Offset: 0x00019F44
			public Rethrow_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AD RID: 429
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Sizeof_ : CodeMatch
		{
			// Token: 0x170001F4 RID: 500
			public Code.Sizeof_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sizeof_)base.Set(OpCodes.Sizeof, operand, name);
				}
			}

			// Token: 0x06000775 RID: 1909 RVA: 0x0001BD78 File Offset: 0x00019F78
			public Sizeof_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AE RID: 430
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Refanytype_ : CodeMatch
		{
			// Token: 0x170001F5 RID: 501
			public Code.Refanytype_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Refanytype_)base.Set(OpCodes.Refanytype, operand, name);
				}
			}

			// Token: 0x06000777 RID: 1911 RVA: 0x0001BDAC File Offset: 0x00019FAC
			public Refanytype_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020001AF RID: 431
		[EditorBrowsable(EditorBrowsableState.Never)]
		public class Readonly_ : CodeMatch
		{
			// Token: 0x170001F6 RID: 502
			public Code.Readonly_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Readonly_)base.Set(OpCodes.Readonly, operand, name);
				}
			}

			// Token: 0x06000779 RID: 1913 RVA: 0x0001BDE0 File Offset: 0x00019FE0
			public Readonly_()
				: base(null, null, null)
			{
			}
		}
	}
}
