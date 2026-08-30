using System;
using System.Collections.Generic;

namespace HarmonyLib
{
	// Token: 0x02000021 RID: 33
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyPatch : HarmonyAttribute
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00004259 File Offset: 0x00002459
		public HarmonyPatch()
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004261 File Offset: 0x00002461
		public HarmonyPatch(Type declaringType)
		{
			this.info.declaringType = declaringType;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004275 File Offset: 0x00002475
		public HarmonyPatch(Type declaringType, Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004295 File Offset: 0x00002495
		public HarmonyPatch(Type declaringType, string methodName)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000042B5 File Offset: 0x000024B5
		public HarmonyPatch(Type declaringType, string methodName, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000042E1 File Offset: 0x000024E1
		public HarmonyPatch(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000430A File Offset: 0x0000250A
		public HarmonyPatch(string typeName, string methodName)
		{
			this.info.declaringType = AccessTools.TypeByName(typeName);
			this.info.methodName = methodName;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004330 File Offset: 0x00002530
		public HarmonyPatch(string typeName, string methodName, MethodType methodType, Type[] argumentTypes = null, ArgumentType[] argumentVariations = null)
		{
			this.info.declaringType = AccessTools.TypeByName(typeName);
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
			if (argumentTypes != null)
			{
				this.ParseSpecialArguments(argumentTypes, argumentVariations);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000437F File Offset: 0x0000257F
		public HarmonyPatch(Type declaringType, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000043A4 File Offset: 0x000025A4
		public HarmonyPatch(Type declaringType, MethodType methodType, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000043D5 File Offset: 0x000025D5
		public HarmonyPatch(Type declaringType, MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004403 File Offset: 0x00002603
		public HarmonyPatch(Type declaringType, string methodName, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004434 File Offset: 0x00002634
		public HarmonyPatch(string methodName)
		{
			this.info.methodName = methodName;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004448 File Offset: 0x00002648
		public HarmonyPatch(string methodName, params Type[] argumentTypes)
		{
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004468 File Offset: 0x00002668
		public HarmonyPatch(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004484 File Offset: 0x00002684
		public HarmonyPatch(string methodName, MethodType methodType)
		{
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000044A9 File Offset: 0x000026A9
		public HarmonyPatch(MethodType methodType)
		{
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000044C2 File Offset: 0x000026C2
		public HarmonyPatch(MethodType methodType, params Type[] argumentTypes)
		{
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000044E7 File Offset: 0x000026E7
		public HarmonyPatch(MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004508 File Offset: 0x00002708
		public HarmonyPatch(Type[] argumentTypes)
		{
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000451C File Offset: 0x0000271C
		public HarmonyPatch(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000452C File Offset: 0x0000272C
		public HarmonyPatch(string typeName, string methodName, MethodType methodType = MethodType.Normal)
		{
			this.info.declaringType = AccessTools.TypeByName(typeName);
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004564 File Offset: 0x00002764
		private void ParseSpecialArguments(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			if (argumentVariations == null || argumentVariations.Length == 0)
			{
				this.info.argumentTypes = argumentTypes;
				return;
			}
			if (argumentTypes.Length < argumentVariations.Length)
			{
				throw new ArgumentException("argumentVariations contains more elements than argumentTypes", "argumentVariations");
			}
			List<Type> list = new List<Type>();
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				Type type = argumentTypes[i];
				switch (argumentVariations[i])
				{
				case ArgumentType.Ref:
				case ArgumentType.Out:
					type = type.MakeByRefType();
					break;
				case ArgumentType.Pointer:
					type = type.MakePointerType();
					break;
				}
				list.Add(type);
			}
			this.info.argumentTypes = list.ToArray();
		}
	}
}
