using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004F3 RID: 1267
	[NativeHeader("Runtime/Shaders/RayTracingShader.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[NativeHeader("Runtime/Shaders/RayTracingAccelerationStructure.h")]
	public sealed class RayTracingShader : Object
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002BDB RID: 11227
		public extern float maxRecursionDepth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06002BDC RID: 11228
		[FreeFunction(Name = "RayTracingShaderScripting::SetFloat", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float val);

		// Token: 0x06002BDD RID: 11229
		[FreeFunction(Name = "RayTracingShaderScripting::SetInt", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInt(int nameID, int val);

		// Token: 0x06002BDE RID: 11230 RVA: 0x00049B60 File Offset: 0x00047D60
		[FreeFunction(Name = "RayTracingShaderScripting::SetVector", HasExplicitThis = true)]
		public void SetVector(int nameID, Vector4 val)
		{
			this.SetVector_Injected(nameID, ref val);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00049B6B File Offset: 0x00047D6B
		[FreeFunction(Name = "RayTracingShaderScripting::SetMatrix", HasExplicitThis = true)]
		public void SetMatrix(int nameID, Matrix4x4 val)
		{
			this.SetMatrix_Injected(nameID, ref val);
		}

		// Token: 0x06002BE0 RID: 11232
		[FreeFunction(Name = "RayTracingShaderScripting::SetFloatArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatArray(int nameID, float[] values);

		// Token: 0x06002BE1 RID: 11233
		[FreeFunction(Name = "RayTracingShaderScripting::SetIntArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIntArray(int nameID, int[] values);

		// Token: 0x06002BE2 RID: 11234
		[FreeFunction(Name = "RayTracingShaderScripting::SetVectorArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetVectorArray(int nameID, Vector4[] values);

		// Token: 0x06002BE3 RID: 11235
		[FreeFunction(Name = "RayTracingShaderScripting::SetMatrixArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMatrixArray(int nameID, Matrix4x4[] values);

		// Token: 0x06002BE4 RID: 11236
		[NativeMethod(Name = "RayTracingShaderScripting::SetTexture", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int nameID, [NotNull("ArgumentNullException")] Texture texture);

		// Token: 0x06002BE5 RID: 11237
		[NativeMethod(Name = "RayTracingShaderScripting::SetBuffer", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBuffer(int nameID, [NotNull("ArgumentNullException")] ComputeBuffer buffer);

		// Token: 0x06002BE6 RID: 11238
		[NativeMethod(Name = "RayTracingShaderScripting::SetBuffer", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGraphicsBuffer(int nameID, [NotNull("ArgumentNullException")] GraphicsBuffer buffer);

		// Token: 0x06002BE7 RID: 11239
		[FreeFunction(Name = "RayTracingShaderScripting::SetConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetConstantComputeBuffer(int nameID, [NotNull("ArgumentNullException")] ComputeBuffer buffer, int offset, int size);

		// Token: 0x06002BE8 RID: 11240
		[FreeFunction(Name = "RayTracingShaderScripting::SetConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetConstantGraphicsBuffer(int nameID, [NotNull("ArgumentNullException")] GraphicsBuffer buffer, int offset, int size);

		// Token: 0x06002BE9 RID: 11241
		[NativeMethod(Name = "RayTracingShaderScripting::SetAccelerationStructure", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAccelerationStructure(int nameID, [NotNull("ArgumentNullException")] RayTracingAccelerationStructure accelerationStructure);

		// Token: 0x06002BEA RID: 11242
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetShaderPass(string passName);

		// Token: 0x06002BEB RID: 11243
		[NativeMethod(Name = "RayTracingShaderScripting::SetTextureFromGlobal", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTextureFromGlobal(int nameID, int globalTextureNameID);

		// Token: 0x06002BEC RID: 11244
		[NativeName("DispatchRays")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispatch(string rayGenFunctionName, int width, int height, int depth, Camera camera = null);

		// Token: 0x06002BED RID: 11245 RVA: 0x00049B76 File Offset: 0x00047D76
		public void SetBuffer(int nameID, GraphicsBuffer buffer)
		{
			this.SetGraphicsBuffer(nameID, buffer);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private RayTracingShader()
		{
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x00049B82 File Offset: 0x00047D82
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x00049B93 File Offset: 0x00047D93
		public void SetInt(string name, int val)
		{
			this.SetInt(Shader.PropertyToID(name), val);
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00049BA4 File Offset: 0x00047DA4
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00049BB5 File Offset: 0x00047DB5
		public void SetMatrix(string name, Matrix4x4 val)
		{
			this.SetMatrix(Shader.PropertyToID(name), val);
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x00049BC6 File Offset: 0x00047DC6
		public void SetVectorArray(string name, Vector4[] values)
		{
			this.SetVectorArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00049BD7 File Offset: 0x00047DD7
		public void SetMatrixArray(string name, Matrix4x4[] values)
		{
			this.SetMatrixArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00049BE8 File Offset: 0x00047DE8
		public void SetFloats(string name, params float[] values)
		{
			this.SetFloatArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00049BF9 File Offset: 0x00047DF9
		public void SetFloats(int nameID, params float[] values)
		{
			this.SetFloatArray(nameID, values);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00049C05 File Offset: 0x00047E05
		public void SetInts(string name, params int[] values)
		{
			this.SetIntArray(Shader.PropertyToID(name), values);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00049C16 File Offset: 0x00047E16
		public void SetInts(int nameID, params int[] values)
		{
			this.SetIntArray(nameID, values);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00049C22 File Offset: 0x00047E22
		public void SetBool(string name, bool val)
		{
			this.SetInt(Shader.PropertyToID(name), val ? 1 : 0);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x00049C39 File Offset: 0x00047E39
		public void SetBool(int nameID, bool val)
		{
			this.SetInt(nameID, val ? 1 : 0);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00049C4B File Offset: 0x00047E4B
		public void SetTexture(string name, Texture texture)
		{
			this.SetTexture(Shader.PropertyToID(name), texture);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00049C5C File Offset: 0x00047E5C
		public void SetBuffer(string name, ComputeBuffer buffer)
		{
			this.SetBuffer(Shader.PropertyToID(name), buffer);
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00049C6D File Offset: 0x00047E6D
		public void SetBuffer(string name, GraphicsBuffer buffer)
		{
			this.SetBuffer(Shader.PropertyToID(name), buffer);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x00049C7E File Offset: 0x00047E7E
		public void SetConstantBuffer(int nameID, ComputeBuffer buffer, int offset, int size)
		{
			this.SetConstantComputeBuffer(nameID, buffer, offset, size);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00049C8D File Offset: 0x00047E8D
		public void SetConstantBuffer(string name, ComputeBuffer buffer, int offset, int size)
		{
			this.SetConstantComputeBuffer(Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00049CA1 File Offset: 0x00047EA1
		public void SetConstantBuffer(int nameID, GraphicsBuffer buffer, int offset, int size)
		{
			this.SetConstantGraphicsBuffer(nameID, buffer, offset, size);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00049CB0 File Offset: 0x00047EB0
		public void SetConstantBuffer(string name, GraphicsBuffer buffer, int offset, int size)
		{
			this.SetConstantGraphicsBuffer(Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x00049CC4 File Offset: 0x00047EC4
		public void SetAccelerationStructure(string name, RayTracingAccelerationStructure accelerationStructure)
		{
			this.SetAccelerationStructure(Shader.PropertyToID(name), accelerationStructure);
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x00049CD5 File Offset: 0x00047ED5
		public void SetTextureFromGlobal(string name, string globalTextureName)
		{
			this.SetTextureFromGlobal(Shader.PropertyToID(name), Shader.PropertyToID(globalTextureName));
		}

		// Token: 0x06002C04 RID: 11268
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector_Injected(int nameID, ref Vector4 val);

		// Token: 0x06002C05 RID: 11269
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMatrix_Injected(int nameID, ref Matrix4x4 val);
	}
}
