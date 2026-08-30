using System;

namespace HarmonyLib
{
	// Token: 0x02000022 RID: 34
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyDelegate : HarmonyPatch
	{
		// Token: 0x06000089 RID: 137 RVA: 0x000045F9 File Offset: 0x000027F9
		public HarmonyDelegate(Type declaringType)
			: base(declaringType)
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004602 File Offset: 0x00002802
		public HarmonyDelegate(Type declaringType, Type[] argumentTypes)
			: base(declaringType, argumentTypes)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000460C File Offset: 0x0000280C
		public HarmonyDelegate(Type declaringType, string methodName)
			: base(declaringType, methodName)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004616 File Offset: 0x00002816
		public HarmonyDelegate(Type declaringType, string methodName, params Type[] argumentTypes)
			: base(declaringType, methodName, argumentTypes)
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004621 File Offset: 0x00002821
		public HarmonyDelegate(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000462E File Offset: 0x0000282E
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType)
			: base(declaringType, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004647 File Offset: 0x00002847
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(declaringType, MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004661 File Offset: 0x00002861
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000467D File Offset: 0x0000287D
		public HarmonyDelegate(Type declaringType, string methodName, MethodDispatchType methodDispatchType)
			: base(declaringType, methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004697 File Offset: 0x00002897
		public HarmonyDelegate(string methodName)
			: base(methodName)
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000046A0 File Offset: 0x000028A0
		public HarmonyDelegate(string methodName, params Type[] argumentTypes)
			: base(methodName, argumentTypes)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000046AA File Offset: 0x000028AA
		public HarmonyDelegate(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000046B5 File Offset: 0x000028B5
		public HarmonyDelegate(string methodName, MethodDispatchType methodDispatchType)
			: base(methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000046CE File Offset: 0x000028CE
		public HarmonyDelegate(MethodDispatchType methodDispatchType)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000046E5 File Offset: 0x000028E5
		public HarmonyDelegate(MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000046FE File Offset: 0x000028FE
		public HarmonyDelegate(MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004718 File Offset: 0x00002918
		public HarmonyDelegate(Type[] argumentTypes)
			: base(argumentTypes)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004721 File Offset: 0x00002921
		public HarmonyDelegate(Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(argumentTypes, argumentVariations)
		{
		}
	}
}
