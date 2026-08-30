using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200027C RID: 636
	[NativeHeader("Runtime/Shaders/ComputeShader.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	public sealed class ComputeShader : Object
	{
		// Token: 0x06001A5A RID: 6746
		[NativeMethod(Name = "ComputeShaderScripting::FindKernel", HasExplicitThis = true, IsFreeFunction = true, ThrowsException = true)]
		[RequiredByNativeCode]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int FindKernel(string name);

		// Token: 0x06001A5B RID: 6747
		[FreeFunction(Name = "ComputeShaderScripting::HasKernel", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasKernel(string name);

		// Token: 0x06001A5C RID: 6748
		[FreeFunction(Name = "ComputeShaderScripting::SetValue<float>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float val);

		// Token: 0x06001A5D RID: 6749
		[FreeFunction(Name = "ComputeShaderScripting::SetValue<int>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInt(int nameID, int val);

		// Token: 0x06001A5E RID: 6750 RVA: 0x0002C977 File Offset: 0x0002AB77
		[FreeFunction(Name = "ComputeShaderScripting::SetValue<Vector4f>", HasExplicitThis = true)]
		public void SetVector(int nameID, Vector4 val)
		{
			this.SetVector_Injected(nameID, ref val);
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0002C982 File Offset: 0x0002AB82
		[FreeFunction(Name = "ComputeShaderScripting::SetValue<Matrix4x4f>", HasExplicitThis = true)]
		public void SetMatrix(int nameID, Matrix4x4 val)
		{
			this.SetMatrix_Injected(nameID, ref val);
		}

		// Token: 0x06001A60 RID: 6752
		[FreeFunction(Name = "ComputeShaderScripting::SetArray<float>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatArray(int nameID, float[] values);

		// Token: 0x06001A61 RID: 6753
		[FreeFunction(Name = "ComputeShaderScripting::SetArray<int>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIntArray(int nameID, int[] values);

		// Token: 0x06001A62 RID: 6754
		[FreeFunction(Name = "ComputeShaderScripting::SetArray<Vector4f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetVectorArray(int nameID, Vector4[] values);

		// Token: 0x06001A63 RID: 6755
		[FreeFunction(Name = "ComputeShaderScripting::SetArray<Matrix4x4f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMatrixArray(int nameID, Matrix4x4[] values);

		// Token: 0x06001A64 RID: 6756
		[NativeMethod(Name = "ComputeShaderScripting::SetTexture", HasExplicitThis = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int kernelIndex, int nameID, [NotNull("ArgumentNullException")] Texture texture, int mipLevel);

		// Token: 0x06001A65 RID: 6757
		[NativeMethod(Name = "ComputeShaderScripting::SetRenderTexture", HasExplicitThis = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTexture(int kernelIndex, int nameID, [NotNull("ArgumentNullException")] RenderTexture texture, int mipLevel, RenderTextureSubElement element);

		// Token: 0x06001A66 RID: 6758
		[NativeMethod(Name = "ComputeShaderScripting::SetTextureFromGlobal", HasExplicitThis = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTextureFromGlobal(int kernelIndex, int nameID, int globalTextureNameID);

		// Token: 0x06001A67 RID: 6759
		[FreeFunction(Name = "ComputeShaderScripting::SetBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetBuffer(int kernelIndex, int nameID, [NotNull("ArgumentNullException")] ComputeBuffer buffer);

		// Token: 0x06001A68 RID: 6760
		[FreeFunction(Name = "ComputeShaderScripting::SetBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetGraphicsBuffer(int kernelIndex, int nameID, [NotNull("ArgumentNullException")] GraphicsBuffer buffer);

		// Token: 0x06001A69 RID: 6761 RVA: 0x0002C98D File Offset: 0x0002AB8D
		public void SetBuffer(int kernelIndex, int nameID, ComputeBuffer buffer)
		{
			this.Internal_SetBuffer(kernelIndex, nameID, buffer);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0002C99A File Offset: 0x0002AB9A
		public void SetBuffer(int kernelIndex, int nameID, GraphicsBuffer buffer)
		{
			this.Internal_SetGraphicsBuffer(kernelIndex, nameID, buffer);
		}

		// Token: 0x06001A6B RID: 6763
		[FreeFunction(Name = "ComputeShaderScripting::SetConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetConstantComputeBuffer(int nameID, [NotNull("ArgumentNullException")] ComputeBuffer buffer, int offset, int size);

		// Token: 0x06001A6C RID: 6764
		[FreeFunction(Name = "ComputeShaderScripting::SetConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetConstantGraphicsBuffer(int nameID, [NotNull("ArgumentNullException")] GraphicsBuffer buffer, int offset, int size);

		// Token: 0x06001A6D RID: 6765
		[NativeMethod(Name = "ComputeShaderScripting::GetKernelThreadGroupSizes", HasExplicitThis = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetKernelThreadGroupSizes(int kernelIndex, out uint x, out uint y, out uint z);

		// Token: 0x06001A6E RID: 6766
		[NativeName("DispatchComputeShader")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispatch(int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ);

		// Token: 0x06001A6F RID: 6767
		[FreeFunction(Name = "ComputeShaderScripting::DispatchIndirect", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchIndirect(int kernelIndex, [NotNull("ArgumentNullException")] ComputeBuffer argsBuffer, uint argsOffset);

		// Token: 0x06001A70 RID: 6768
		[FreeFunction(Name = "ComputeShaderScripting::DispatchIndirect", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchIndirectGraphicsBuffer(int kernelIndex, [NotNull("ArgumentNullException")] GraphicsBuffer argsBuffer, uint argsOffset);

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0002C9A8 File Offset: 0x0002ABA8
		public LocalKeywordSpace keywordSpace
		{
			get
			{
				LocalKeywordSpace localKeywordSpace;
				this.get_keywordSpace_Injected(out localKeywordSpace);
				return localKeywordSpace;
			}
		}

		// Token: 0x06001A72 RID: 6770
		[FreeFunction("ComputeShaderScripting::EnableKeyword", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EnableKeyword(string keyword);

		// Token: 0x06001A73 RID: 6771
		[FreeFunction("ComputeShaderScripting::DisableKeyword", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DisableKeyword(string keyword);

		// Token: 0x06001A74 RID: 6772
		[FreeFunction("ComputeShaderScripting::IsKeywordEnabled", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsKeywordEnabled(string keyword);

		// Token: 0x06001A75 RID: 6773 RVA: 0x0002C9BE File Offset: 0x0002ABBE
		[FreeFunction("ComputeShaderScripting::EnableKeyword", HasExplicitThis = true)]
		private void EnableLocalKeyword(LocalKeyword keyword)
		{
			this.EnableLocalKeyword_Injected(ref keyword);
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0002C9C8 File Offset: 0x0002ABC8
		[FreeFunction("ComputeShaderScripting::DisableKeyword", HasExplicitThis = true)]
		private void DisableLocalKeyword(LocalKeyword keyword)
		{
			this.DisableLocalKeyword_Injected(ref keyword);
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0002C9D2 File Offset: 0x0002ABD2
		[FreeFunction("ComputeShaderScripting::SetKeyword", HasExplicitThis = true)]
		private void SetLocalKeyword(LocalKeyword keyword, bool value)
		{
			this.SetLocalKeyword_Injected(ref keyword, value);
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0002C9DD File Offset: 0x0002ABDD
		[FreeFunction("ComputeShaderScripting::IsKeywordEnabled", HasExplicitThis = true)]
		private bool IsLocalKeywordEnabled(LocalKeyword keyword)
		{
			return this.IsLocalKeywordEnabled_Injected(ref keyword);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0002C9E7 File Offset: 0x0002ABE7
		public void EnableKeyword(in LocalKeyword keyword)
		{
			this.EnableLocalKeyword(keyword);
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0002C9F7 File Offset: 0x0002ABF7
		public void DisableKeyword(in LocalKeyword keyword)
		{
			this.DisableLocalKeyword(keyword);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0002CA07 File Offset: 0x0002AC07
		public void SetKeyword(in LocalKeyword keyword, bool value)
		{
			this.SetLocalKeyword(keyword, value);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x0002CA18 File Offset: 0x0002AC18
		public bool IsKeywordEnabled(in LocalKeyword keyword)
		{
			return this.IsLocalKeywordEnabled(keyword);
		}

		// Token: 0x06001A7D RID: 6781
		[FreeFunction("ComputeShaderScripting::IsSupported", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsSupported(int kernelIndex);

		// Token: 0x06001A7E RID: 6782
		[FreeFunction("ComputeShaderScripting::GetShaderKeywords", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string[] GetShaderKeywords();

		// Token: 0x06001A7F RID: 6783
		[FreeFunction("ComputeShaderScripting::SetShaderKeywords", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetShaderKeywords(string[] names);

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x0002CA38 File Offset: 0x0002AC38
		// (set) Token: 0x06001A81 RID: 6785 RVA: 0x0002CA50 File Offset: 0x0002AC50
		public string[] shaderKeywords
		{
			get
			{
				return this.GetShaderKeywords();
			}
			set
			{
				this.SetShaderKeywords(value);
			}
		}

		// Token: 0x06001A82 RID: 6786
		[FreeFunction("ComputeShaderScripting::GetEnabledKeywords", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern LocalKeyword[] GetEnabledKeywords();

		// Token: 0x06001A83 RID: 6787
		[FreeFunction("ComputeShaderScripting::SetEnabledKeywords", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetEnabledKeywords(LocalKeyword[] keywords);

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x0002CA5C File Offset: 0x0002AC5C
		// (set) Token: 0x06001A85 RID: 6789 RVA: 0x0002CA74 File Offset: 0x0002AC74
		public LocalKeyword[] enabledKeywords
		{
			get
			{
				return this.GetEnabledKeywords();
			}
			set
			{
				this.SetEnabledKeywords(value);
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private ComputeShader()
		{
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0002CA7F File Offset: 0x0002AC7F
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0002CA90 File Offset: 0x0002AC90
		public void SetInt(string name, int val)
		{
			this.SetInt(Shader.PropertyToID(name), val);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0002CAA1 File Offset: 0x0002ACA1
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0002CAB2 File Offset: 0x0002ACB2
		public void SetMatrix(string name, Matrix4x4 val)
		{
			this.SetMatrix(Shader.PropertyToID(name), val);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0002CAC3 File Offset: 0x0002ACC3
		public void SetVectorArray(string name, Vector4[] values)
		{
			this.SetVectorArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
		public void SetMatrixArray(string name, Matrix4x4[] values)
		{
			this.SetMatrixArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0002CAE5 File Offset: 0x0002ACE5
		public void SetFloats(string name, params float[] values)
		{
			this.SetFloatArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0002CAF6 File Offset: 0x0002ACF6
		public void SetFloats(int nameID, params float[] values)
		{
			this.SetFloatArray(nameID, values);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0002CB02 File Offset: 0x0002AD02
		public void SetInts(string name, params int[] values)
		{
			this.SetIntArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0002CB13 File Offset: 0x0002AD13
		public void SetInts(int nameID, params int[] values)
		{
			this.SetIntArray(nameID, values);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0002CB1F File Offset: 0x0002AD1F
		public void SetBool(string name, bool val)
		{
			this.SetInt(Shader.PropertyToID(name), val ? 1 : 0);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x0002CB36 File Offset: 0x0002AD36
		public void SetBool(int nameID, bool val)
		{
			this.SetInt(nameID, val ? 1 : 0);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x0002CB48 File Offset: 0x0002AD48
		public void SetTexture(int kernelIndex, int nameID, Texture texture)
		{
			this.SetTexture(kernelIndex, nameID, texture, 0);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0002CB56 File Offset: 0x0002AD56
		public void SetTexture(int kernelIndex, string name, Texture texture)
		{
			this.SetTexture(kernelIndex, Shader.PropertyToID(name), texture, 0);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0002CB69 File Offset: 0x0002AD69
		public void SetTexture(int kernelIndex, string name, Texture texture, int mipLevel)
		{
			this.SetTexture(kernelIndex, Shader.PropertyToID(name), texture, mipLevel);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0002CB7D File Offset: 0x0002AD7D
		public void SetTexture(int kernelIndex, int nameID, RenderTexture texture, int mipLevel, RenderTextureSubElement element)
		{
			this.SetRenderTexture(kernelIndex, nameID, texture, mipLevel, element);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0002CB8E File Offset: 0x0002AD8E
		public void SetTexture(int kernelIndex, string name, RenderTexture texture, int mipLevel, RenderTextureSubElement element)
		{
			this.SetRenderTexture(kernelIndex, Shader.PropertyToID(name), texture, mipLevel, element);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0002CBA4 File Offset: 0x0002ADA4
		public void SetTextureFromGlobal(int kernelIndex, string name, string globalTextureName)
		{
			this.SetTextureFromGlobal(kernelIndex, Shader.PropertyToID(name), Shader.PropertyToID(globalTextureName));
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0002CBBB File Offset: 0x0002ADBB
		public void SetBuffer(int kernelIndex, string name, ComputeBuffer buffer)
		{
			this.SetBuffer(kernelIndex, Shader.PropertyToID(name), buffer);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0002CBCD File Offset: 0x0002ADCD
		public void SetBuffer(int kernelIndex, string name, GraphicsBuffer buffer)
		{
			this.SetBuffer(kernelIndex, Shader.PropertyToID(name), buffer);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0002CBDF File Offset: 0x0002ADDF
		public void SetConstantBuffer(int nameID, ComputeBuffer buffer, int offset, int size)
		{
			this.SetConstantComputeBuffer(nameID, buffer, offset, size);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0002CBEE File Offset: 0x0002ADEE
		public void SetConstantBuffer(string name, ComputeBuffer buffer, int offset, int size)
		{
			this.SetConstantBuffer(Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x0002CC02 File Offset: 0x0002AE02
		public void SetConstantBuffer(int nameID, GraphicsBuffer buffer, int offset, int size)
		{
			this.SetConstantGraphicsBuffer(nameID, buffer, offset, size);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x0002CC11 File Offset: 0x0002AE11
		public void SetConstantBuffer(string name, GraphicsBuffer buffer, int offset, int size)
		{
			this.SetConstantBuffer(Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x0002CC28 File Offset: 0x0002AE28
		public void DispatchIndirect(int kernelIndex, ComputeBuffer argsBuffer, [DefaultValue("0")] uint argsOffset)
		{
			bool flag = argsBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("argsBuffer");
			}
			bool flag2 = argsBuffer.m_Ptr == IntPtr.Zero;
			if (flag2)
			{
				throw new ObjectDisposedException("argsBuffer");
			}
			bool flag3 = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag3)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			this.Internal_DispatchIndirect(kernelIndex, argsBuffer, argsOffset);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0002CC95 File Offset: 0x0002AE95
		[ExcludeFromDocs]
		public void DispatchIndirect(int kernelIndex, ComputeBuffer argsBuffer)
		{
			this.DispatchIndirect(kernelIndex, argsBuffer, 0U);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x0002CCA4 File Offset: 0x0002AEA4
		public void DispatchIndirect(int kernelIndex, GraphicsBuffer argsBuffer, [DefaultValue("0")] uint argsOffset)
		{
			bool flag = argsBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("argsBuffer");
			}
			bool flag2 = argsBuffer.m_Ptr == IntPtr.Zero;
			if (flag2)
			{
				throw new ObjectDisposedException("argsBuffer");
			}
			bool flag3 = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag3)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			this.Internal_DispatchIndirectGraphicsBuffer(kernelIndex, argsBuffer, argsOffset);
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0002CD11 File Offset: 0x0002AF11
		[ExcludeFromDocs]
		public void DispatchIndirect(int kernelIndex, GraphicsBuffer argsBuffer)
		{
			this.DispatchIndirect(kernelIndex, argsBuffer, 0U);
		}

		// Token: 0x06001AA3 RID: 6819
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector_Injected(int nameID, ref Vector4 val);

		// Token: 0x06001AA4 RID: 6820
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMatrix_Injected(int nameID, ref Matrix4x4 val);

		// Token: 0x06001AA5 RID: 6821
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_keywordSpace_Injected(out LocalKeywordSpace ret);

		// Token: 0x06001AA6 RID: 6822
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EnableLocalKeyword_Injected(ref LocalKeyword keyword);

		// Token: 0x06001AA7 RID: 6823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisableLocalKeyword_Injected(ref LocalKeyword keyword);

		// Token: 0x06001AA8 RID: 6824
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalKeyword_Injected(ref LocalKeyword keyword, bool value);

		// Token: 0x06001AA9 RID: 6825
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsLocalKeywordEnabled_Injected(ref LocalKeyword keyword);
	}
}
