using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x0200004A RID: 74
	public static class AccessToolsExtensions
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x0000B096 File Offset: 0x00009296
		public static IEnumerable<Type> InnerTypes(this Type type)
		{
			return AccessTools.InnerTypes(type);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000B09E File Offset: 0x0000929E
		public static T FindIncludingBaseTypes<T>(this Type type, Func<Type, T> func) where T : class
		{
			return AccessTools.FindIncludingBaseTypes<T>(type, func);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B0A7 File Offset: 0x000092A7
		public static T FindIncludingInnerTypes<T>(this Type type, Func<Type, T> func) where T : class
		{
			return AccessTools.FindIncludingInnerTypes<T>(type, func);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B0B0 File Offset: 0x000092B0
		public static FieldInfo DeclaredField(this Type type, string name)
		{
			return AccessTools.DeclaredField(type, name);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B0B9 File Offset: 0x000092B9
		public static FieldInfo Field(this Type type, string name)
		{
			return AccessTools.Field(type, name);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000B0C2 File Offset: 0x000092C2
		public static FieldInfo DeclaredField(this Type type, int idx)
		{
			return AccessTools.DeclaredField(type, idx);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000B0CB File Offset: 0x000092CB
		public static PropertyInfo DeclaredProperty(this Type type, string name)
		{
			return AccessTools.DeclaredProperty(type, name);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000B0D4 File Offset: 0x000092D4
		public static PropertyInfo DeclaredIndexer(this Type type, Type[] parameters = null)
		{
			return AccessTools.DeclaredIndexer(type, parameters);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B0DD File Offset: 0x000092DD
		public static MethodInfo DeclaredPropertyGetter(this Type type, string name)
		{
			return AccessTools.DeclaredPropertyGetter(type, name);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B0E6 File Offset: 0x000092E6
		public static MethodInfo DeclaredIndexerGetter(this Type type, Type[] parameters = null)
		{
			return AccessTools.DeclaredIndexerGetter(type, parameters);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000B0EF File Offset: 0x000092EF
		public static MethodInfo DeclaredPropertySetter(this Type type, string name)
		{
			return AccessTools.DeclaredPropertySetter(type, name);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000B0F8 File Offset: 0x000092F8
		public static MethodInfo DeclaredIndexerSetter(this Type type, Type[] parameters)
		{
			return AccessTools.DeclaredIndexerSetter(type, parameters);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000B101 File Offset: 0x00009301
		public static PropertyInfo Property(this Type type, string name)
		{
			return AccessTools.Property(type, name);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B10A File Offset: 0x0000930A
		public static PropertyInfo Indexer(this Type type, Type[] parameters = null)
		{
			return AccessTools.Indexer(type, parameters);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B113 File Offset: 0x00009313
		public static MethodInfo PropertyGetter(this Type type, string name)
		{
			return AccessTools.PropertyGetter(type, name);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000B11C File Offset: 0x0000931C
		public static MethodInfo IndexerGetter(this Type type, Type[] parameters = null)
		{
			return AccessTools.IndexerGetter(type, parameters);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000B125 File Offset: 0x00009325
		public static MethodInfo PropertySetter(this Type type, string name)
		{
			return AccessTools.PropertySetter(type, name);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B12E File Offset: 0x0000932E
		public static MethodInfo IndexerSetter(this Type type, Type[] parameters = null)
		{
			return AccessTools.IndexerSetter(type, parameters);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B137 File Offset: 0x00009337
		public static MethodInfo DeclaredMethod(this Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			return AccessTools.DeclaredMethod(type, name, parameters, generics);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B142 File Offset: 0x00009342
		public static MethodInfo Method(this Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			return AccessTools.Method(type, name, parameters, generics);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B14D File Offset: 0x0000934D
		public static List<string> GetMethodNames(this Type type)
		{
			return AccessTools.GetMethodNames(type);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B155 File Offset: 0x00009355
		public static List<string> GetFieldNames(this Type type)
		{
			return AccessTools.GetFieldNames(type);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B15D File Offset: 0x0000935D
		public static List<string> GetPropertyNames(this Type type)
		{
			return AccessTools.GetPropertyNames(type);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B165 File Offset: 0x00009365
		public static ConstructorInfo DeclaredConstructor(this Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			return AccessTools.DeclaredConstructor(type, parameters, searchForStatic);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B16F File Offset: 0x0000936F
		public static ConstructorInfo Constructor(this Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			return AccessTools.Constructor(type, parameters, searchForStatic);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B179 File Offset: 0x00009379
		public static List<ConstructorInfo> GetDeclaredConstructors(this Type type, bool? searchForStatic = null)
		{
			return AccessTools.GetDeclaredConstructors(type, searchForStatic);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B182 File Offset: 0x00009382
		public static List<MethodInfo> GetDeclaredMethods(this Type type)
		{
			return AccessTools.GetDeclaredMethods(type);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B18A File Offset: 0x0000938A
		public static List<PropertyInfo> GetDeclaredProperties(this Type type)
		{
			return AccessTools.GetDeclaredProperties(type);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B192 File Offset: 0x00009392
		public static List<FieldInfo> GetDeclaredFields(this Type type)
		{
			return AccessTools.GetDeclaredFields(type);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000B19A File Offset: 0x0000939A
		public static Type Inner(this Type type, string name)
		{
			return AccessTools.Inner(type, name);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000B1A3 File Offset: 0x000093A3
		public static Type FirstInner(this Type type, Func<Type, bool> predicate)
		{
			return AccessTools.FirstInner(type, predicate);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000B1AC File Offset: 0x000093AC
		public static MethodInfo FirstMethod(this Type type, Func<MethodInfo, bool> predicate)
		{
			return AccessTools.FirstMethod(type, predicate);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B1B5 File Offset: 0x000093B5
		public static ConstructorInfo FirstConstructor(this Type type, Func<ConstructorInfo, bool> predicate)
		{
			return AccessTools.FirstConstructor(type, predicate);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B1BE File Offset: 0x000093BE
		public static PropertyInfo FirstProperty(this Type type, Func<PropertyInfo, bool> predicate)
		{
			return AccessTools.FirstProperty(type, predicate);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000B1C7 File Offset: 0x000093C7
		public static AccessTools.FieldRef<object, F> FieldRefAccess<F>(this Type type, string fieldName)
		{
			return AccessTools.FieldRefAccess<F>(type, fieldName);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000B1D0 File Offset: 0x000093D0
		public static ref F StaticFieldRefAccess<F>(this Type type, string fieldName)
		{
			return AccessTools.StaticFieldRefAccess<F>(type, fieldName);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000B1D9 File Offset: 0x000093D9
		public static void ThrowMissingMemberException(this Type type, params string[] names)
		{
			AccessTools.ThrowMissingMemberException(type, names);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000B1E2 File Offset: 0x000093E2
		public static object GetDefaultValue(this Type type)
		{
			return AccessTools.GetDefaultValue(type);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000B1EA File Offset: 0x000093EA
		public static object CreateInstance(this Type type)
		{
			return AccessTools.CreateInstance(type);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000B1F2 File Offset: 0x000093F2
		public static bool IsStruct(this Type type)
		{
			return AccessTools.IsStruct(type);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000B1FA File Offset: 0x000093FA
		public static bool IsClass(this Type type)
		{
			return AccessTools.IsClass(type);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000B202 File Offset: 0x00009402
		public static bool IsValue(this Type type)
		{
			return AccessTools.IsValue(type);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000B20A File Offset: 0x0000940A
		public static bool IsInteger(this Type type)
		{
			return AccessTools.IsInteger(type);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000B212 File Offset: 0x00009412
		public static bool IsFloatingPoint(this Type type)
		{
			return AccessTools.IsFloatingPoint(type);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000B21A File Offset: 0x0000941A
		public static bool IsNumber(this Type type)
		{
			return AccessTools.IsNumber(type);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B222 File Offset: 0x00009422
		public static bool IsVoid(this Type type)
		{
			return AccessTools.IsVoid(type);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B22A File Offset: 0x0000942A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(this Type type)
		{
			return AccessTools.IsStatic(type);
		}
	}
}
