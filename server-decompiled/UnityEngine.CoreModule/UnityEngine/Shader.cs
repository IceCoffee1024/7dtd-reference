using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000168 RID: 360
	[NativeHeader("Runtime/Shaders/Shader.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[NativeHeader("Runtime/Misc/ResourceManager.h")]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	[NativeHeader("Runtime/Shaders/ComputeShader.h")]
	[NativeHeader("Runtime/Shaders/ShaderNameRegistry.h")]
	[NativeHeader("Runtime/Shaders/GpuPrograms/ShaderVariantCollection.h")]
	public sealed class Shader : Object
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x000134F8 File Offset: 0x000116F8
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x0001350F File Offset: 0x0001170F
		[Obsolete("Use Graphics.activeTier instead (UnityUpgradable) -> UnityEngine.Graphics.activeTier", false)]
		public static ShaderHardwareTier globalShaderHardwareTier
		{
			get
			{
				return (ShaderHardwareTier)Graphics.activeTier;
			}
			set
			{
				Graphics.activeTier = (GraphicsTier)value;
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00013519 File Offset: 0x00011719
		public static Shader Find(string name)
		{
			return ResourcesAPI.ActiveAPI.FindShaderByName(name);
		}

		// Token: 0x06000D79 RID: 3449
		[FreeFunction("GetBuiltinResource<Shader>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Shader FindBuiltin(string name);

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000D7A RID: 3450
		// (set) Token: 0x06000D7B RID: 3451
		[NativeProperty("MaxChunksRuntimeOverride")]
		public static extern int maximumChunksOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000D7C RID: 3452
		// (set) Token: 0x06000D7D RID: 3453
		[NativeProperty("MaximumShaderLOD")]
		public extern int maximumLOD
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000D7E RID: 3454
		// (set) Token: 0x06000D7F RID: 3455
		[NativeProperty("GlobalMaximumShaderLOD")]
		public static extern int globalMaximumLOD
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000D80 RID: 3456
		public extern bool isSupported
		{
			[NativeMethod("IsSupported")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000D81 RID: 3457
		// (set) Token: 0x06000D82 RID: 3458
		public static extern string globalRenderPipeline
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00013528 File Offset: 0x00011728
		public static GlobalKeyword[] enabledGlobalKeywords
		{
			get
			{
				return Shader.GetEnabledGlobalKeywords();
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00013540 File Offset: 0x00011740
		public static GlobalKeyword[] globalKeywords
		{
			get
			{
				return Shader.GetAllGlobalKeywords();
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00013558 File Offset: 0x00011758
		public LocalKeywordSpace keywordSpace
		{
			get
			{
				LocalKeywordSpace localKeywordSpace;
				this.get_keywordSpace_Injected(out localKeywordSpace);
				return localKeywordSpace;
			}
		}

		// Token: 0x06000D86 RID: 3462
		[FreeFunction("keywords::GetEnabledGlobalKeywords")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern GlobalKeyword[] GetEnabledGlobalKeywords();

		// Token: 0x06000D87 RID: 3463
		[FreeFunction("keywords::GetAllGlobalKeywords")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern GlobalKeyword[] GetAllGlobalKeywords();

		// Token: 0x06000D88 RID: 3464
		[FreeFunction("ShaderScripting::EnableKeyword")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EnableKeyword(string keyword);

		// Token: 0x06000D89 RID: 3465
		[FreeFunction("ShaderScripting::DisableKeyword")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisableKeyword(string keyword);

		// Token: 0x06000D8A RID: 3466
		[FreeFunction("ShaderScripting::IsKeywordEnabled")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsKeywordEnabled(string keyword);

		// Token: 0x06000D8B RID: 3467 RVA: 0x0001356E File Offset: 0x0001176E
		[FreeFunction("ShaderScripting::EnableKeyword")]
		internal static void EnableKeywordFast(GlobalKeyword keyword)
		{
			Shader.EnableKeywordFast_Injected(ref keyword);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00013577 File Offset: 0x00011777
		[FreeFunction("ShaderScripting::DisableKeyword")]
		internal static void DisableKeywordFast(GlobalKeyword keyword)
		{
			Shader.DisableKeywordFast_Injected(ref keyword);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00013580 File Offset: 0x00011780
		[FreeFunction("ShaderScripting::SetKeyword")]
		internal static void SetKeywordFast(GlobalKeyword keyword, bool value)
		{
			Shader.SetKeywordFast_Injected(ref keyword, value);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0001358A File Offset: 0x0001178A
		[FreeFunction("ShaderScripting::IsKeywordEnabled")]
		internal static bool IsKeywordEnabledFast(GlobalKeyword keyword)
		{
			return Shader.IsKeywordEnabledFast_Injected(ref keyword);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00013593 File Offset: 0x00011793
		public static void EnableKeyword(in GlobalKeyword keyword)
		{
			Shader.EnableKeywordFast(keyword);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000135A2 File Offset: 0x000117A2
		public static void DisableKeyword(in GlobalKeyword keyword)
		{
			Shader.DisableKeywordFast(keyword);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000135B1 File Offset: 0x000117B1
		public static void SetKeyword(in GlobalKeyword keyword, bool value)
		{
			Shader.SetKeywordFast(keyword, value);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000135C4 File Offset: 0x000117C4
		public static bool IsKeywordEnabled(in GlobalKeyword keyword)
		{
			return Shader.IsKeywordEnabledFast(keyword);
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D93 RID: 3475
		public extern int renderQueue
		{
			[FreeFunction("ShaderScripting::GetRenderQueue", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D94 RID: 3476
		internal extern DisableBatchingType disableBatching
		{
			[FreeFunction("ShaderScripting::GetDisableBatchingType", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000D95 RID: 3477
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WarmupAllShaders();

		// Token: 0x06000D96 RID: 3478
		[FreeFunction("ShaderScripting::TagToID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int TagToID(string name);

		// Token: 0x06000D97 RID: 3479
		[FreeFunction("ShaderScripting::IDToTag")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string IDToTag(int name);

		// Token: 0x06000D98 RID: 3480
		[FreeFunction(Name = "ShaderScripting::PropertyToID", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int PropertyToID(string name);

		// Token: 0x06000D99 RID: 3481
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Shader GetDependency(string name);

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000D9A RID: 3482
		public extern int passCount
		{
			[FreeFunction(Name = "ShaderScripting::GetPassCount", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000D9B RID: 3483
		public extern int subshaderCount
		{
			[FreeFunction(Name = "ShaderScripting::GetSubshaderCount", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000D9C RID: 3484
		[FreeFunction(Name = "ShaderScripting::GetPassCountInSubshader", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPassCountInSubshader(int subshaderIndex);

		// Token: 0x06000D9D RID: 3485 RVA: 0x000135E4 File Offset: 0x000117E4
		public ShaderTagId FindPassTagValue(int passIndex, ShaderTagId tagName)
		{
			bool flag = passIndex < 0 || passIndex >= this.passCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("passIndex");
			}
			int num = this.Internal_FindPassTagValue(passIndex, tagName.id);
			return new ShaderTagId
			{
				id = num
			};
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0001363C File Offset: 0x0001183C
		public ShaderTagId FindPassTagValue(int subshaderIndex, int passIndex, ShaderTagId tagName)
		{
			bool flag = subshaderIndex < 0 || subshaderIndex >= this.subshaderCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("subshaderIndex");
			}
			bool flag2 = passIndex < 0 || passIndex >= this.GetPassCountInSubshader(subshaderIndex);
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("passIndex");
			}
			int num = this.Internal_FindPassTagValueInSubShader(subshaderIndex, passIndex, tagName.id);
			return new ShaderTagId
			{
				id = num
			};
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000136B8 File Offset: 0x000118B8
		public ShaderTagId FindSubshaderTagValue(int subshaderIndex, ShaderTagId tagName)
		{
			bool flag = subshaderIndex < 0 || subshaderIndex >= this.subshaderCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Invalid subshaderIndex {0}. Value must be in the range [0, {1})", subshaderIndex, this.subshaderCount));
			}
			int num = this.Internal_FindSubshaderTagValue(subshaderIndex, tagName.id);
			return new ShaderTagId
			{
				id = num
			};
		}

		// Token: 0x06000DA0 RID: 3488
		[FreeFunction(Name = "ShaderScripting::FindPassTagValue", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_FindPassTagValue(int passIndex, int tagName);

		// Token: 0x06000DA1 RID: 3489
		[FreeFunction(Name = "ShaderScripting::FindPassTagValue", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_FindPassTagValueInSubShader(int subShaderIndex, int passIndex, int tagName);

		// Token: 0x06000DA2 RID: 3490
		[FreeFunction(Name = "ShaderScripting::FindSubshaderTagValue", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_FindSubshaderTagValue(int subShaderIndex, int tagName);

		// Token: 0x06000DA3 RID: 3491
		[FreeFunction("ShaderScripting::SetGlobalInt")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalIntImpl(int name, int value);

		// Token: 0x06000DA4 RID: 3492
		[FreeFunction("ShaderScripting::SetGlobalFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalFloatImpl(int name, float value);

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00013723 File Offset: 0x00011923
		[FreeFunction("ShaderScripting::SetGlobalVector")]
		private static void SetGlobalVectorImpl(int name, Vector4 value)
		{
			Shader.SetGlobalVectorImpl_Injected(name, ref value);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0001372D File Offset: 0x0001192D
		[FreeFunction("ShaderScripting::SetGlobalMatrix")]
		private static void SetGlobalMatrixImpl(int name, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl_Injected(name, ref value);
		}

		// Token: 0x06000DA7 RID: 3495
		[FreeFunction("ShaderScripting::SetGlobalTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalTextureImpl(int name, Texture value);

		// Token: 0x06000DA8 RID: 3496
		[FreeFunction("ShaderScripting::SetGlobalRenderTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalRenderTextureImpl(int name, RenderTexture value, RenderTextureSubElement element);

		// Token: 0x06000DA9 RID: 3497
		[FreeFunction("ShaderScripting::SetGlobalBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalBufferImpl(int name, ComputeBuffer value);

		// Token: 0x06000DAA RID: 3498
		[FreeFunction("ShaderScripting::SetGlobalBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalGraphicsBufferImpl(int name, GraphicsBuffer value);

		// Token: 0x06000DAB RID: 3499
		[FreeFunction("ShaderScripting::SetGlobalConstantBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalConstantBufferImpl(int name, ComputeBuffer value, int offset, int size);

		// Token: 0x06000DAC RID: 3500
		[FreeFunction("ShaderScripting::SetGlobalConstantBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalConstantGraphicsBufferImpl(int name, GraphicsBuffer value, int offset, int size);

		// Token: 0x06000DAD RID: 3501
		[FreeFunction("ShaderScripting::GetGlobalInt")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalIntImpl(int name);

		// Token: 0x06000DAE RID: 3502
		[FreeFunction("ShaderScripting::GetGlobalFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetGlobalFloatImpl(int name);

		// Token: 0x06000DAF RID: 3503 RVA: 0x00013738 File Offset: 0x00011938
		[FreeFunction("ShaderScripting::GetGlobalVector")]
		private static Vector4 GetGlobalVectorImpl(int name)
		{
			Vector4 vector;
			Shader.GetGlobalVectorImpl_Injected(name, out vector);
			return vector;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00013750 File Offset: 0x00011950
		[FreeFunction("ShaderScripting::GetGlobalMatrix")]
		private static Matrix4x4 GetGlobalMatrixImpl(int name)
		{
			Matrix4x4 matrix4x;
			Shader.GetGlobalMatrixImpl_Injected(name, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000DB1 RID: 3505
		[FreeFunction("ShaderScripting::GetGlobalTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Texture GetGlobalTextureImpl(int name);

		// Token: 0x06000DB2 RID: 3506
		[FreeFunction("ShaderScripting::SetGlobalFloatArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalFloatArrayImpl(int name, float[] values, int count);

		// Token: 0x06000DB3 RID: 3507
		[FreeFunction("ShaderScripting::SetGlobalVectorArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalVectorArrayImpl(int name, Vector4[] values, int count);

		// Token: 0x06000DB4 RID: 3508
		[FreeFunction("ShaderScripting::SetGlobalMatrixArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalMatrixArrayImpl(int name, Matrix4x4[] values, int count);

		// Token: 0x06000DB5 RID: 3509
		[FreeFunction("ShaderScripting::GetGlobalFloatArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float[] GetGlobalFloatArrayImpl(int name);

		// Token: 0x06000DB6 RID: 3510
		[FreeFunction("ShaderScripting::GetGlobalVectorArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector4[] GetGlobalVectorArrayImpl(int name);

		// Token: 0x06000DB7 RID: 3511
		[FreeFunction("ShaderScripting::GetGlobalMatrixArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix4x4[] GetGlobalMatrixArrayImpl(int name);

		// Token: 0x06000DB8 RID: 3512
		[FreeFunction("ShaderScripting::GetGlobalFloatArrayCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalFloatArrayCountImpl(int name);

		// Token: 0x06000DB9 RID: 3513
		[FreeFunction("ShaderScripting::GetGlobalVectorArrayCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalVectorArrayCountImpl(int name);

		// Token: 0x06000DBA RID: 3514
		[FreeFunction("ShaderScripting::GetGlobalMatrixArrayCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalMatrixArrayCountImpl(int name);

		// Token: 0x06000DBB RID: 3515
		[FreeFunction("ShaderScripting::ExtractGlobalFloatArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalFloatArrayImpl(int name, [Out] float[] val);

		// Token: 0x06000DBC RID: 3516
		[FreeFunction("ShaderScripting::ExtractGlobalVectorArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalVectorArrayImpl(int name, [Out] Vector4[] val);

		// Token: 0x06000DBD RID: 3517
		[FreeFunction("ShaderScripting::ExtractGlobalMatrixArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalMatrixArrayImpl(int name, [Out] Matrix4x4[] val);

		// Token: 0x06000DBE RID: 3518 RVA: 0x00013768 File Offset: 0x00011968
		private static void SetGlobalFloatArray(int name, float[] values, int count)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			bool flag3 = values.Length < count;
			if (flag3)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalFloatArrayImpl(name, values, count);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000137BC File Offset: 0x000119BC
		private static void SetGlobalVectorArray(int name, Vector4[] values, int count)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			bool flag3 = values.Length < count;
			if (flag3)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalVectorArrayImpl(name, values, count);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00013810 File Offset: 0x00011A10
		private static void SetGlobalMatrixArray(int name, Matrix4x4[] values, int count)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			bool flag3 = values.Length < count;
			if (flag3)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalMatrixArrayImpl(name, values, count);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00013864 File Offset: 0x00011A64
		private static void ExtractGlobalFloatArray(int name, List<float> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalFloatArrayCountImpl = Shader.GetGlobalFloatArrayCountImpl(name);
			bool flag2 = globalFloatArrayCountImpl > 0;
			if (flag2)
			{
				NoAllocHelpers.EnsureListElemCount<float>(values, globalFloatArrayCountImpl);
				Shader.ExtractGlobalFloatArrayImpl(name, (float[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000138B8 File Offset: 0x00011AB8
		private static void ExtractGlobalVectorArray(int name, List<Vector4> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalVectorArrayCountImpl = Shader.GetGlobalVectorArrayCountImpl(name);
			bool flag2 = globalVectorArrayCountImpl > 0;
			if (flag2)
			{
				NoAllocHelpers.EnsureListElemCount<Vector4>(values, globalVectorArrayCountImpl);
				Shader.ExtractGlobalVectorArrayImpl(name, (Vector4[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0001390C File Offset: 0x00011B0C
		private static void ExtractGlobalMatrixArray(int name, List<Matrix4x4> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalMatrixArrayCountImpl = Shader.GetGlobalMatrixArrayCountImpl(name);
			bool flag2 = globalMatrixArrayCountImpl > 0;
			if (flag2)
			{
				NoAllocHelpers.EnsureListElemCount<Matrix4x4>(values, globalMatrixArrayCountImpl);
				Shader.ExtractGlobalMatrixArrayImpl(name, (Matrix4x4[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0001395F File Offset: 0x00011B5F
		public static void SetGlobalInt(string name, int value)
		{
			Shader.SetGlobalFloatImpl(Shader.PropertyToID(name), (float)value);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00013970 File Offset: 0x00011B70
		public static void SetGlobalInt(int nameID, int value)
		{
			Shader.SetGlobalFloatImpl(nameID, (float)value);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0001397C File Offset: 0x00011B7C
		public static void SetGlobalFloat(string name, float value)
		{
			Shader.SetGlobalFloatImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0001398C File Offset: 0x00011B8C
		public static void SetGlobalFloat(int nameID, float value)
		{
			Shader.SetGlobalFloatImpl(nameID, value);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00013997 File Offset: 0x00011B97
		public static void SetGlobalInteger(string name, int value)
		{
			Shader.SetGlobalIntImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000139A7 File Offset: 0x00011BA7
		public static void SetGlobalInteger(int nameID, int value)
		{
			Shader.SetGlobalIntImpl(nameID, value);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000139B2 File Offset: 0x00011BB2
		public static void SetGlobalVector(string name, Vector4 value)
		{
			Shader.SetGlobalVectorImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000139C2 File Offset: 0x00011BC2
		public static void SetGlobalVector(int nameID, Vector4 value)
		{
			Shader.SetGlobalVectorImpl(nameID, value);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000139CD File Offset: 0x00011BCD
		public static void SetGlobalColor(string name, Color value)
		{
			Shader.SetGlobalVectorImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000139E2 File Offset: 0x00011BE2
		public static void SetGlobalColor(int nameID, Color value)
		{
			Shader.SetGlobalVectorImpl(nameID, value);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x000139F2 File Offset: 0x00011BF2
		public static void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00013A02 File Offset: 0x00011C02
		public static void SetGlobalMatrix(int nameID, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl(nameID, value);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00013A0D File Offset: 0x00011C0D
		public static void SetGlobalTexture(string name, Texture value)
		{
			Shader.SetGlobalTextureImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00013A1D File Offset: 0x00011C1D
		public static void SetGlobalTexture(int nameID, Texture value)
		{
			Shader.SetGlobalTextureImpl(nameID, value);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00013A28 File Offset: 0x00011C28
		public static void SetGlobalTexture(string name, RenderTexture value, RenderTextureSubElement element)
		{
			Shader.SetGlobalRenderTextureImpl(Shader.PropertyToID(name), value, element);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00013A39 File Offset: 0x00011C39
		public static void SetGlobalTexture(int nameID, RenderTexture value, RenderTextureSubElement element)
		{
			Shader.SetGlobalRenderTextureImpl(nameID, value, element);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00013A45 File Offset: 0x00011C45
		public static void SetGlobalBuffer(string name, ComputeBuffer value)
		{
			Shader.SetGlobalBufferImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00013A55 File Offset: 0x00011C55
		public static void SetGlobalBuffer(int nameID, ComputeBuffer value)
		{
			Shader.SetGlobalBufferImpl(nameID, value);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00013A60 File Offset: 0x00011C60
		public static void SetGlobalBuffer(string name, GraphicsBuffer value)
		{
			Shader.SetGlobalGraphicsBufferImpl(Shader.PropertyToID(name), value);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00013A70 File Offset: 0x00011C70
		public static void SetGlobalBuffer(int nameID, GraphicsBuffer value)
		{
			Shader.SetGlobalGraphicsBufferImpl(nameID, value);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00013A7B File Offset: 0x00011C7B
		public static void SetGlobalConstantBuffer(string name, ComputeBuffer value, int offset, int size)
		{
			Shader.SetGlobalConstantBufferImpl(Shader.PropertyToID(name), value, offset, size);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00013A8D File Offset: 0x00011C8D
		public static void SetGlobalConstantBuffer(int nameID, ComputeBuffer value, int offset, int size)
		{
			Shader.SetGlobalConstantBufferImpl(nameID, value, offset, size);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00013A9A File Offset: 0x00011C9A
		public static void SetGlobalConstantBuffer(string name, GraphicsBuffer value, int offset, int size)
		{
			Shader.SetGlobalConstantGraphicsBufferImpl(Shader.PropertyToID(name), value, offset, size);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00013AAC File Offset: 0x00011CAC
		public static void SetGlobalConstantBuffer(int nameID, GraphicsBuffer value, int offset, int size)
		{
			Shader.SetGlobalConstantGraphicsBufferImpl(nameID, value, offset, size);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00013AB9 File Offset: 0x00011CB9
		public static void SetGlobalFloatArray(string name, List<float> values)
		{
			Shader.SetGlobalFloatArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<float>(values), values.Count);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00013AD4 File Offset: 0x00011CD4
		public static void SetGlobalFloatArray(int nameID, List<float> values)
		{
			Shader.SetGlobalFloatArray(nameID, NoAllocHelpers.ExtractArrayFromListT<float>(values), values.Count);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00013AEA File Offset: 0x00011CEA
		public static void SetGlobalFloatArray(string name, float[] values)
		{
			Shader.SetGlobalFloatArray(Shader.PropertyToID(name), values, values.Length);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00013AFD File Offset: 0x00011CFD
		public static void SetGlobalFloatArray(int nameID, float[] values)
		{
			Shader.SetGlobalFloatArray(nameID, values, values.Length);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00013B0B File Offset: 0x00011D0B
		public static void SetGlobalVectorArray(string name, List<Vector4> values)
		{
			Shader.SetGlobalVectorArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<Vector4>(values), values.Count);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00013B26 File Offset: 0x00011D26
		public static void SetGlobalVectorArray(int nameID, List<Vector4> values)
		{
			Shader.SetGlobalVectorArray(nameID, NoAllocHelpers.ExtractArrayFromListT<Vector4>(values), values.Count);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00013B3C File Offset: 0x00011D3C
		public static void SetGlobalVectorArray(string name, Vector4[] values)
		{
			Shader.SetGlobalVectorArray(Shader.PropertyToID(name), values, values.Length);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00013B4F File Offset: 0x00011D4F
		public static void SetGlobalVectorArray(int nameID, Vector4[] values)
		{
			Shader.SetGlobalVectorArray(nameID, values, values.Length);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00013B5D File Offset: 0x00011D5D
		public static void SetGlobalMatrixArray(string name, List<Matrix4x4> values)
		{
			Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<Matrix4x4>(values), values.Count);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00013B78 File Offset: 0x00011D78
		public static void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
		{
			Shader.SetGlobalMatrixArray(nameID, NoAllocHelpers.ExtractArrayFromListT<Matrix4x4>(values), values.Count);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00013B8E File Offset: 0x00011D8E
		public static void SetGlobalMatrixArray(string name, Matrix4x4[] values)
		{
			Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), values, values.Length);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00013BA1 File Offset: 0x00011DA1
		public static void SetGlobalMatrixArray(int nameID, Matrix4x4[] values)
		{
			Shader.SetGlobalMatrixArray(nameID, values, values.Length);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00013BB0 File Offset: 0x00011DB0
		public static int GetGlobalInt(string name)
		{
			return (int)Shader.GetGlobalFloatImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00013BD0 File Offset: 0x00011DD0
		public static int GetGlobalInt(int nameID)
		{
			return (int)Shader.GetGlobalFloatImpl(nameID);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00013BEC File Offset: 0x00011DEC
		public static float GetGlobalFloat(string name)
		{
			return Shader.GetGlobalFloatImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00013C0C File Offset: 0x00011E0C
		public static float GetGlobalFloat(int nameID)
		{
			return Shader.GetGlobalFloatImpl(nameID);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00013C24 File Offset: 0x00011E24
		public static int GetGlobalInteger(string name)
		{
			return Shader.GetGlobalIntImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00013C44 File Offset: 0x00011E44
		public static int GetGlobalInteger(int nameID)
		{
			return Shader.GetGlobalIntImpl(nameID);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00013C5C File Offset: 0x00011E5C
		public static Vector4 GetGlobalVector(string name)
		{
			return Shader.GetGlobalVectorImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00013C7C File Offset: 0x00011E7C
		public static Vector4 GetGlobalVector(int nameID)
		{
			return Shader.GetGlobalVectorImpl(nameID);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00013C94 File Offset: 0x00011E94
		public static Color GetGlobalColor(string name)
		{
			return Shader.GetGlobalVectorImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00013CB8 File Offset: 0x00011EB8
		public static Color GetGlobalColor(int nameID)
		{
			return Shader.GetGlobalVectorImpl(nameID);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public static Matrix4x4 GetGlobalMatrix(string name)
		{
			return Shader.GetGlobalMatrixImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00013CF8 File Offset: 0x00011EF8
		public static Matrix4x4 GetGlobalMatrix(int nameID)
		{
			return Shader.GetGlobalMatrixImpl(nameID);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00013D10 File Offset: 0x00011F10
		public static Texture GetGlobalTexture(string name)
		{
			return Shader.GetGlobalTextureImpl(Shader.PropertyToID(name));
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00013D30 File Offset: 0x00011F30
		public static Texture GetGlobalTexture(int nameID)
		{
			return Shader.GetGlobalTextureImpl(nameID);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00013D48 File Offset: 0x00011F48
		public static float[] GetGlobalFloatArray(string name)
		{
			return Shader.GetGlobalFloatArray(Shader.PropertyToID(name));
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00013D68 File Offset: 0x00011F68
		public static float[] GetGlobalFloatArray(int nameID)
		{
			return (Shader.GetGlobalFloatArrayCountImpl(nameID) != 0) ? Shader.GetGlobalFloatArrayImpl(nameID) : null;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00013D8C File Offset: 0x00011F8C
		public static Vector4[] GetGlobalVectorArray(string name)
		{
			return Shader.GetGlobalVectorArray(Shader.PropertyToID(name));
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00013DAC File Offset: 0x00011FAC
		public static Vector4[] GetGlobalVectorArray(int nameID)
		{
			return (Shader.GetGlobalVectorArrayCountImpl(nameID) != 0) ? Shader.GetGlobalVectorArrayImpl(nameID) : null;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00013DD0 File Offset: 0x00011FD0
		public static Matrix4x4[] GetGlobalMatrixArray(string name)
		{
			return Shader.GetGlobalMatrixArray(Shader.PropertyToID(name));
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00013DF0 File Offset: 0x00011FF0
		public static Matrix4x4[] GetGlobalMatrixArray(int nameID)
		{
			return (Shader.GetGlobalMatrixArrayCountImpl(nameID) != 0) ? Shader.GetGlobalMatrixArrayImpl(nameID) : null;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00013E13 File Offset: 0x00012013
		public static void GetGlobalFloatArray(string name, List<float> values)
		{
			Shader.ExtractGlobalFloatArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00013E23 File Offset: 0x00012023
		public static void GetGlobalFloatArray(int nameID, List<float> values)
		{
			Shader.ExtractGlobalFloatArray(nameID, values);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00013E2E File Offset: 0x0001202E
		public static void GetGlobalVectorArray(string name, List<Vector4> values)
		{
			Shader.ExtractGlobalVectorArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00013E3E File Offset: 0x0001203E
		public static void GetGlobalVectorArray(int nameID, List<Vector4> values)
		{
			Shader.ExtractGlobalVectorArray(nameID, values);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00013E49 File Offset: 0x00012049
		public static void GetGlobalMatrixArray(string name, List<Matrix4x4> values)
		{
			Shader.ExtractGlobalMatrixArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00013E59 File Offset: 0x00012059
		public static void GetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
		{
			Shader.ExtractGlobalMatrixArray(nameID, values);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private Shader()
		{
		}

		// Token: 0x06000E03 RID: 3587
		[FreeFunction("ShaderScripting::GetPropertyName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPropertyName([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E04 RID: 3588
		[FreeFunction("ShaderScripting::GetPropertyNameId")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPropertyNameId([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E05 RID: 3589
		[FreeFunction("ShaderScripting::GetPropertyType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderPropertyType GetPropertyType([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E06 RID: 3590
		[FreeFunction("ShaderScripting::GetPropertyDescription")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPropertyDescription([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E07 RID: 3591
		[FreeFunction("ShaderScripting::GetPropertyFlags")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderPropertyFlags GetPropertyFlags([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E08 RID: 3592
		[FreeFunction("ShaderScripting::GetPropertyAttributes")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetPropertyAttributes([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E09 RID: 3593
		[FreeFunction("ShaderScripting::GetPropertyDefaultIntValue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPropertyDefaultIntValue([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E0A RID: 3594 RVA: 0x00013E64 File Offset: 0x00012064
		[FreeFunction("ShaderScripting::GetPropertyDefaultValue")]
		private static Vector4 GetPropertyDefaultValue([NotNull("ArgumentNullException")] Shader shader, int propertyIndex)
		{
			Vector4 vector;
			Shader.GetPropertyDefaultValue_Injected(shader, propertyIndex, out vector);
			return vector;
		}

		// Token: 0x06000E0B RID: 3595
		[FreeFunction("ShaderScripting::GetPropertyTextureDimension")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern TextureDimension GetPropertyTextureDimension([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E0C RID: 3596
		[FreeFunction("ShaderScripting::GetPropertyTextureDefaultName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPropertyTextureDefaultName([NotNull("ArgumentNullException")] Shader shader, int propertyIndex);

		// Token: 0x06000E0D RID: 3597
		[FreeFunction("ShaderScripting::FindTextureStack")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool FindTextureStackImpl([NotNull("ArgumentNullException")] Shader s, int propertyIdx, out string stackName, out int layerIndex);

		// Token: 0x06000E0E RID: 3598 RVA: 0x00013E7C File Offset: 0x0001207C
		private static void CheckPropertyIndex(Shader s, int propertyIndex)
		{
			bool flag = propertyIndex < 0 || propertyIndex >= s.GetPropertyCount();
			if (flag)
			{
				throw new ArgumentOutOfRangeException("propertyIndex");
			}
		}

		// Token: 0x06000E0F RID: 3599
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPropertyCount();

		// Token: 0x06000E10 RID: 3600
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int FindPropertyIndex(string propertyName);

		// Token: 0x06000E11 RID: 3601 RVA: 0x00013EAC File Offset: 0x000120AC
		public string GetPropertyName(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyName(this, propertyIndex);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00013ED0 File Offset: 0x000120D0
		public int GetPropertyNameId(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyNameId(this, propertyIndex);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00013EF4 File Offset: 0x000120F4
		public ShaderPropertyType GetPropertyType(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyType(this, propertyIndex);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00013F18 File Offset: 0x00012118
		public string GetPropertyDescription(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyDescription(this, propertyIndex);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00013F3C File Offset: 0x0001213C
		public ShaderPropertyFlags GetPropertyFlags(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyFlags(this, propertyIndex);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00013F60 File Offset: 0x00012160
		public string[] GetPropertyAttributes(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			return Shader.GetPropertyAttributes(this, propertyIndex);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00013F84 File Offset: 0x00012184
		public float GetPropertyDefaultFloatValue(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			ShaderPropertyType propertyType = this.GetPropertyType(propertyIndex);
			bool flag = propertyType != ShaderPropertyType.Float && propertyType != ShaderPropertyType.Range;
			if (flag)
			{
				throw new ArgumentException("Property type is not Float or Range.");
			}
			return Shader.GetPropertyDefaultValue(this, propertyIndex)[0];
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00013FD4 File Offset: 0x000121D4
		public Vector4 GetPropertyDefaultVectorValue(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			ShaderPropertyType propertyType = this.GetPropertyType(propertyIndex);
			bool flag = propertyType != ShaderPropertyType.Color && propertyType != ShaderPropertyType.Vector;
			if (flag)
			{
				throw new ArgumentException("Property type is not Color or Vector.");
			}
			return Shader.GetPropertyDefaultValue(this, propertyIndex);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0001401C File Offset: 0x0001221C
		public Vector2 GetPropertyRangeLimits(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			bool flag = this.GetPropertyType(propertyIndex) != ShaderPropertyType.Range;
			if (flag)
			{
				throw new ArgumentException("Property type is not Range.");
			}
			Vector4 propertyDefaultValue = Shader.GetPropertyDefaultValue(this, propertyIndex);
			return new Vector2(propertyDefaultValue[1], propertyDefaultValue[2]);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00014070 File Offset: 0x00012270
		public int GetPropertyDefaultIntValue(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			bool flag = this.GetPropertyType(propertyIndex) != ShaderPropertyType.Int;
			if (flag)
			{
				throw new ArgumentException("Property type is not Int.");
			}
			return Shader.GetPropertyDefaultIntValue(this, propertyIndex);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000140B0 File Offset: 0x000122B0
		public TextureDimension GetPropertyTextureDimension(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			bool flag = this.GetPropertyType(propertyIndex) != ShaderPropertyType.Texture;
			if (flag)
			{
				throw new ArgumentException("Property type is not TexEnv.");
			}
			return Shader.GetPropertyTextureDimension(this, propertyIndex);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000140F0 File Offset: 0x000122F0
		public string GetPropertyTextureDefaultName(int propertyIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			ShaderPropertyType propertyType = this.GetPropertyType(propertyIndex);
			bool flag = propertyType != ShaderPropertyType.Texture;
			if (flag)
			{
				throw new ArgumentException("Property type is not Texture.");
			}
			return Shader.GetPropertyTextureDefaultName(this, propertyIndex);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00014130 File Offset: 0x00012330
		public bool FindTextureStack(int propertyIndex, out string stackName, out int layerIndex)
		{
			Shader.CheckPropertyIndex(this, propertyIndex);
			ShaderPropertyType propertyType = this.GetPropertyType(propertyIndex);
			bool flag = propertyType != ShaderPropertyType.Texture;
			if (flag)
			{
				throw new ArgumentException("Property type is not Texture.");
			}
			return Shader.FindTextureStackImpl(this, propertyIndex, out stackName, out layerIndex);
		}

		// Token: 0x06000E1E RID: 3614
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_keywordSpace_Injected(out LocalKeywordSpace ret);

		// Token: 0x06000E1F RID: 3615
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableKeywordFast_Injected(ref GlobalKeyword keyword);

		// Token: 0x06000E20 RID: 3616
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableKeywordFast_Injected(ref GlobalKeyword keyword);

		// Token: 0x06000E21 RID: 3617
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeywordFast_Injected(ref GlobalKeyword keyword, bool value);

		// Token: 0x06000E22 RID: 3618
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsKeywordEnabledFast_Injected(ref GlobalKeyword keyword);

		// Token: 0x06000E23 RID: 3619
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalVectorImpl_Injected(int name, ref Vector4 value);

		// Token: 0x06000E24 RID: 3620
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalMatrixImpl_Injected(int name, ref Matrix4x4 value);

		// Token: 0x06000E25 RID: 3621
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGlobalVectorImpl_Injected(int name, out Vector4 ret);

		// Token: 0x06000E26 RID: 3622
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGlobalMatrixImpl_Injected(int name, out Matrix4x4 ret);

		// Token: 0x06000E27 RID: 3623
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPropertyDefaultValue_Injected(Shader shader, int propertyIndex, out Vector4 ret);
	}
}
