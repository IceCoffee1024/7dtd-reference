using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200000B RID: 11
	public class DelegateTypeFactory
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002240 File Offset: 0x00000440
		public Type CreateDelegateType(Type returnType, Type[] argTypes)
		{
			return this.CreateDelegateType(returnType, argTypes, null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002260 File Offset: 0x00000460
		public Type CreateDelegateType(Type returnType, Type[] argTypes, CallingConvention? convention)
		{
			DelegateTypeFactory.counter++;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("HarmonyDTFAssembly");
			defaultInterpolatedStringHandler.AppendFormatted<int>(DelegateTypeFactory.counter);
			AssemblyNameDefinition assemblyNameDefinition = new AssemblyNameDefinition(defaultInterpolatedStringHandler.ToStringAndClear(), new Version(1, 0));
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("HarmonyDTFModule");
			defaultInterpolatedStringHandler.AppendFormatted<int>(DelegateTypeFactory.counter);
			AssemblyDefinition assemblyDefinition = AssemblyDefinition.CreateAssembly(assemblyNameDefinition, defaultInterpolatedStringHandler.ToStringAndClear(), ModuleKind.Dll);
			ModuleDefinition module = assemblyDefinition.MainModule;
			string text = "";
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("HarmonyDTFType");
			defaultInterpolatedStringHandler.AppendFormatted<int>(DelegateTypeFactory.counter);
			TypeDefinition typeDefinition = new TypeDefinition(text, defaultInterpolatedStringHandler.ToStringAndClear(), Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Sealed)
			{
				BaseType = module.ImportReference(typeof(MulticastDelegate))
			};
			module.Types.Add(typeDefinition);
			if (convention != null)
			{
				CustomAttribute customAttribute = new CustomAttribute(module.ImportReference(DelegateTypeFactory.CallingConvAttr));
				customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(module.ImportReference(typeof(CallingConvention)), convention.Value));
				typeDefinition.CustomAttributes.Add(customAttribute);
			}
			MethodDefinition methodDefinition = new MethodDefinition(".ctor", Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.HideBySig | Mono.Cecil.MethodAttributes.RTSpecialName, module.ImportReference(typeof(void)))
			{
				ImplAttributes = Mono.Cecil.MethodImplAttributes.CodeTypeMask
			};
			methodDefinition.Parameters.AddRange<ParameterDefinition>(new ParameterDefinition[]
			{
				new ParameterDefinition(module.ImportReference(typeof(object))),
				new ParameterDefinition(module.ImportReference(typeof(IntPtr)))
			});
			typeDefinition.Methods.Add(methodDefinition);
			MethodDefinition methodDefinition2 = new MethodDefinition("Invoke", Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Virtual | Mono.Cecil.MethodAttributes.HideBySig, module.ImportReference(returnType))
			{
				ImplAttributes = Mono.Cecil.MethodImplAttributes.CodeTypeMask
			};
			methodDefinition2.Parameters.AddRange<ParameterDefinition>(argTypes.Select<Type, ParameterDefinition>((Type t) => new ParameterDefinition(module.ImportReference(t))));
			typeDefinition.Methods.Add(methodDefinition2);
			Assembly assembly = ReflectionHelper.Load(assemblyDefinition.MainModule);
			Assembly assembly2 = assembly;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("HarmonyDTFType");
			defaultInterpolatedStringHandler.AppendFormatted<int>(DelegateTypeFactory.counter);
			return assembly2.GetType(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024CC File Offset: 0x000006CC
		public Type CreateDelegateType(MethodInfo method)
		{
			return this.CreateDelegateType(method, null);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024EC File Offset: 0x000006EC
		public Type CreateDelegateType(MethodInfo method, CallingConvention? convention)
		{
			List<DelegateTypeFactory.DelegateEntry> list;
			DelegateTypeFactory.DelegateEntry delegateEntry;
			if (DelegateTypeFactory.TypeCache.TryGetValue(method, out list) && (delegateEntry = list.FirstOrDefault<DelegateTypeFactory.DelegateEntry>(delegate(DelegateTypeFactory.DelegateEntry e)
			{
				CallingConvention? callingConvention = e.callingConvention;
				CallingConvention? convention2 = convention;
				return (callingConvention.GetValueOrDefault() == convention2.GetValueOrDefault()) & (callingConvention != null == (convention2 != null));
			})) != null)
			{
				return delegateEntry.delegateType;
			}
			if (list == null)
			{
				list = (DelegateTypeFactory.TypeCache[method] = new List<DelegateTypeFactory.DelegateEntry>());
			}
			delegateEntry = new DelegateTypeFactory.DelegateEntry
			{
				delegateType = this.CreateDelegateType(method.ReturnType, method.GetParameters().Types().ToArray<Type>(), convention),
				callingConvention = convention
			};
			list.Add(delegateEntry);
			return delegateEntry.delegateType;
		}

		// Token: 0x04000016 RID: 22
		private static int counter;

		// Token: 0x04000017 RID: 23
		private static readonly Dictionary<MethodInfo, List<DelegateTypeFactory.DelegateEntry>> TypeCache = new Dictionary<MethodInfo, List<DelegateTypeFactory.DelegateEntry>>();

		// Token: 0x04000018 RID: 24
		private static readonly MethodBase CallingConvAttr = AccessTools.Constructor(typeof(UnmanagedFunctionPointerAttribute), new Type[] { typeof(CallingConvention) }, false);

		// Token: 0x04000019 RID: 25
		public static readonly DelegateTypeFactory instance = new DelegateTypeFactory();

		// Token: 0x02000068 RID: 104
		private class DelegateEntry
		{
			// Token: 0x04000126 RID: 294
			public CallingConvention? callingConvention;

			// Token: 0x04000127 RID: 295
			public Type delegateType;
		}
	}
}
