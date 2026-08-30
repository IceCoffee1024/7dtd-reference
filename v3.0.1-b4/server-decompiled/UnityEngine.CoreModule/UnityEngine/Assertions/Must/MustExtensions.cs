using System;
using System.Diagnostics;

namespace UnityEngine.Assertions.Must
{
	// Token: 0x020004F7 RID: 1271
	[DebuggerStepThrough]
	[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
	public static class MustExtensions
	{
		// Token: 0x06002C50 RID: 11344 RVA: 0x0004A780 File Offset: 0x00048980
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeTrue(this bool value)
		{
			Assert.IsTrue(value);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0004A78A File Offset: 0x0004898A
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeTrue(this bool value, string message)
		{
			Assert.IsTrue(value, message);
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0004A795 File Offset: 0x00048995
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeFalse(this bool value)
		{
			Assert.IsFalse(value);
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0004A79F File Offset: 0x0004899F
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeFalse(this bool value, string message)
		{
			Assert.IsFalse(value, message);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0004A7AA File Offset: 0x000489AA
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected)
		{
			Assert.AreApproximatelyEqual(actual, expected);
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x0004A7B5 File Offset: 0x000489B5
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, string message)
		{
			Assert.AreApproximatelyEqual(actual, expected, message);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0004A7C1 File Offset: 0x000489C1
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance)
		{
			Assert.AreApproximatelyEqual(actual, expected, tolerance);
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0004A7CD File Offset: 0x000489CD
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
		{
			Assert.AreApproximatelyEqual(expected, actual, tolerance, message);
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x0004A7DA File Offset: 0x000489DA
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected)
		{
			Assert.AreNotApproximatelyEqual(expected, actual);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x0004A7E5 File Offset: 0x000489E5
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, string message)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, message);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0004A7F1 File Offset: 0x000489F1
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, tolerance);
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0004A7FD File Offset: 0x000489FD
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, tolerance, message);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x0004A80A File Offset: 0x00048A0A
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeEqual<T>(this T actual, T expected)
		{
			Assert.AreEqual<T>(actual, expected);
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x0004A815 File Offset: 0x00048A15
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeEqual<T>(this T actual, T expected, string message)
		{
			Assert.AreEqual<T>(expected, actual, message);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x0004A821 File Offset: 0x00048A21
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeEqual<T>(this T actual, T expected)
		{
			Assert.AreNotEqual<T>(actual, expected);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x0004A82C File Offset: 0x00048A2C
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustNotBeEqual<T>(this T actual, T expected, string message)
		{
			Assert.AreNotEqual<T>(expected, actual, message);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x0004A838 File Offset: 0x00048A38
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeNull<T>(this T expected) where T : class
		{
			Assert.IsNull<T>(expected);
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x0004A842 File Offset: 0x00048A42
		[Conditional("UNITY_ASSERTIONS")]
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		public static void MustBeNull<T>(this T expected, string message) where T : class
		{
			Assert.IsNull<T>(expected, message);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x0004A84D File Offset: 0x00048A4D
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeNull<T>(this T expected) where T : class
		{
			Assert.IsNotNull<T>(expected);
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x0004A857 File Offset: 0x00048A57
		[Obsolete("Must extensions are deprecated. Use UnityEngine.Assertions.Assert instead")]
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeNull<T>(this T expected, string message) where T : class
		{
			Assert.IsNotNull<T>(expected, message);
		}
	}
}
