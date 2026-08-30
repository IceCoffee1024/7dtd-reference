using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x0200014B RID: 331
	[NativeHeader("Runtime/Shaders/ComputeShader.h")]
	[NativeHeader("Runtime/Misc/PlayerSettings.h")]
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/ColorGamut.h")]
	[NativeHeader("Runtime/Camera/LightProbeProxyVolume.h")]
	[NativeHeader("Runtime/Graphics/CopyTexture.h")]
	public class Graphics
	{
		// Token: 0x0600095C RID: 2396
		[FreeFunction("GraphicsScripting::GetMaxDrawMeshInstanceCount", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetMaxDrawMeshInstanceCount();

		// Token: 0x0600095D RID: 2397
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ColorGamut GetActiveColorGamut();

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0000F060 File Offset: 0x0000D260
		public static ColorGamut activeColorGamut
		{
			get
			{
				return Graphics.GetActiveColorGamut();
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600095F RID: 2399
		// (set) Token: 0x06000960 RID: 2400
		[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
		public static extern GraphicsTier activeTier
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000961 RID: 2401
		[NativeMethod(Name = "GetPreserveFramebufferAlpha")]
		[StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetPreserveFramebufferAlpha();

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0000F078 File Offset: 0x0000D278
		public static bool preserveFramebufferAlpha
		{
			get
			{
				return Graphics.GetPreserveFramebufferAlpha();
			}
		}

		// Token: 0x06000963 RID: 2403
		[NativeMethod(Name = "GetMinOpenGLESVersion")]
		[StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern OpenGLESVersion GetMinOpenGLESVersion();

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0000F090 File Offset: 0x0000D290
		public static OpenGLESVersion minOpenGLESVersion
		{
			get
			{
				return Graphics.GetMinOpenGLESVersion();
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		[FreeFunction("GraphicsScripting::GetActiveColorBuffer")]
		private static RenderBuffer GetActiveColorBuffer()
		{
			RenderBuffer renderBuffer;
			Graphics.GetActiveColorBuffer_Injected(out renderBuffer);
			return renderBuffer;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		[FreeFunction("GraphicsScripting::GetActiveDepthBuffer")]
		private static RenderBuffer GetActiveDepthBuffer()
		{
			RenderBuffer renderBuffer;
			Graphics.GetActiveDepthBuffer_Injected(out renderBuffer);
			return renderBuffer;
		}

		// Token: 0x06000967 RID: 2407
		[FreeFunction("GraphicsScripting::SetNullRT")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetNullRT();

		// Token: 0x06000968 RID: 2408 RVA: 0x0000F0D5 File Offset: 0x0000D2D5
		[NativeMethod(Name = "GraphicsScripting::SetRTSimple", IsFreeFunction = true, ThrowsException = true)]
		private static void Internal_SetRTSimple(RenderBuffer color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice)
		{
			Graphics.Internal_SetRTSimple_Injected(ref color, ref depth, mip, face, depthSlice);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
		[NativeMethod(Name = "GraphicsScripting::SetMRTSimple", IsFreeFunction = true, ThrowsException = true)]
		private static void Internal_SetMRTSimple([NotNull("ArgumentNullException")] RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice)
		{
			Graphics.Internal_SetMRTSimple_Injected(color, ref depth, mip, face, depthSlice);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
		[NativeMethod(Name = "GraphicsScripting::SetMRTFull", IsFreeFunction = true, ThrowsException = true)]
		private static void Internal_SetMRTFullSetup([NotNull("ArgumentNullException")] RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice, [NotNull("ArgumentNullException")] RenderBufferLoadAction[] colorLA, [NotNull("ArgumentNullException")] RenderBufferStoreAction[] colorSA, RenderBufferLoadAction depthLA, RenderBufferStoreAction depthSA)
		{
			Graphics.Internal_SetMRTFullSetup_Injected(color, ref depth, mip, face, depthSlice, colorLA, colorSA, depthLA, depthSA);
		}

		// Token: 0x0600096B RID: 2411
		[NativeMethod(Name = "GraphicsScripting::SetRandomWriteTargetRT", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetRandomWriteTargetRT(int index, RenderTexture uav);

		// Token: 0x0600096C RID: 2412
		[FreeFunction("GraphicsScripting::SetRandomWriteTargetBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetRandomWriteTargetBuffer(int index, ComputeBuffer uav, bool preserveCounterValue);

		// Token: 0x0600096D RID: 2413
		[FreeFunction("GraphicsScripting::SetRandomWriteTargetBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetRandomWriteTargetGraphicsBuffer(int index, GraphicsBuffer uav, bool preserveCounterValue);

		// Token: 0x0600096E RID: 2414
		[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearRandomWriteTargets();

		// Token: 0x0600096F RID: 2415
		[FreeFunction("CopyTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyTexture_Full(Texture src, Texture dst);

		// Token: 0x06000970 RID: 2416
		[FreeFunction("CopyTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyTexture_Slice_AllMips(Texture src, int srcElement, Texture dst, int dstElement);

		// Token: 0x06000971 RID: 2417
		[FreeFunction("CopyTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyTexture_Slice(Texture src, int srcElement, int srcMip, Texture dst, int dstElement, int dstMip);

		// Token: 0x06000972 RID: 2418
		[FreeFunction("CopyTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyTexture_Region(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY);

		// Token: 0x06000973 RID: 2419
		[FreeFunction("ConvertTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ConvertTexture_Full(Texture src, Texture dst);

		// Token: 0x06000974 RID: 2420
		[FreeFunction("ConvertTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ConvertTexture_Slice(Texture src, int srcElement, Texture dst, int dstElement);

		// Token: 0x06000975 RID: 2421
		[FreeFunction("GraphicsScripting::CopyBuffer", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyBufferImpl([NotNull("ArgumentNullException")] GraphicsBuffer source, [NotNull("ArgumentNullException")] GraphicsBuffer dest);

		// Token: 0x06000976 RID: 2422 RVA: 0x0000F115 File Offset: 0x0000D315
		[FreeFunction("GraphicsScripting::DrawMeshNow")]
		private static void Internal_DrawMeshNow1([NotNull("NullExceptionObject")] Mesh mesh, int subsetIndex, Vector3 position, Quaternion rotation)
		{
			Graphics.Internal_DrawMeshNow1_Injected(mesh, subsetIndex, ref position, ref rotation);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0000F122 File Offset: 0x0000D322
		[FreeFunction("GraphicsScripting::DrawMeshNow")]
		private static void Internal_DrawMeshNow2([NotNull("NullExceptionObject")] Mesh mesh, int subsetIndex, Matrix4x4 matrix)
		{
			Graphics.Internal_DrawMeshNow2_Injected(mesh, subsetIndex, ref matrix);
		}

		// Token: 0x06000978 RID: 2424
		[VisibleToOtherModules(new string[] { "UnityEngine.IMGUIModule" })]
		[FreeFunction("GraphicsScripting::DrawTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_DrawTexture(ref Internal_DrawTextureArguments args);

		// Token: 0x06000979 RID: 2425 RVA: 0x0000F12D File Offset: 0x0000D32D
		[FreeFunction("GraphicsScripting::RenderMesh")]
		private unsafe static void Internal_RenderMesh(RenderParams rparams, [NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, Matrix4x4 objectToWorld, Matrix4x4* prevObjectToWorld)
		{
			Graphics.Internal_RenderMesh_Injected(ref rparams, mesh, submeshIndex, ref objectToWorld, prevObjectToWorld);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0000F13C File Offset: 0x0000D33C
		[FreeFunction("GraphicsScripting::RenderMeshInstanced")]
		private static void Internal_RenderMeshInstanced(RenderParams rparams, [NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, IntPtr instanceData, RenderInstancedDataLayout layout, uint instanceCount)
		{
			Graphics.Internal_RenderMeshInstanced_Injected(ref rparams, mesh, submeshIndex, instanceData, ref layout, instanceCount);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0000F14C File Offset: 0x0000D34C
		[FreeFunction("GraphicsScripting::RenderMeshIndirect")]
		private static void Internal_RenderMeshIndirect(RenderParams rparams, [NotNull("NullExceptionObject")] Mesh mesh, [NotNull("NullExceptionObject")] GraphicsBuffer commandBuffer, int commandCount, int startCommand)
		{
			Graphics.Internal_RenderMeshIndirect_Injected(ref rparams, mesh, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0000F15A File Offset: 0x0000D35A
		[FreeFunction("GraphicsScripting::RenderMeshPrimitives")]
		private static void Internal_RenderMeshPrimitives(RenderParams rparams, [NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, int instanceCount)
		{
			Graphics.Internal_RenderMeshPrimitives_Injected(ref rparams, mesh, submeshIndex, instanceCount);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0000F166 File Offset: 0x0000D366
		[FreeFunction("GraphicsScripting::RenderPrimitives")]
		private static void Internal_RenderPrimitives(RenderParams rparams, MeshTopology topology, int vertexCount, int instanceCount)
		{
			Graphics.Internal_RenderPrimitives_Injected(ref rparams, topology, vertexCount, instanceCount);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0000F172 File Offset: 0x0000D372
		[FreeFunction("GraphicsScripting::RenderPrimitivesIndexed")]
		private static void Internal_RenderPrimitivesIndexed(RenderParams rparams, MeshTopology topology, [NotNull("NullExceptionObject")] GraphicsBuffer indexBuffer, int indexCount, int startIndex, int instanceCount)
		{
			Graphics.Internal_RenderPrimitivesIndexed_Injected(ref rparams, topology, indexBuffer, indexCount, startIndex, instanceCount);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000F182 File Offset: 0x0000D382
		[FreeFunction("GraphicsScripting::RenderPrimitivesIndirect")]
		private static void Internal_RenderPrimitivesIndirect(RenderParams rparams, MeshTopology topology, [NotNull("NullExceptionObject")] GraphicsBuffer commandBuffer, int commandCount, int startCommand)
		{
			Graphics.Internal_RenderPrimitivesIndirect_Injected(ref rparams, topology, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000F190 File Offset: 0x0000D390
		[FreeFunction("GraphicsScripting::RenderPrimitivesIndexedIndirect")]
		private static void Internal_RenderPrimitivesIndexedIndirect(RenderParams rparams, MeshTopology topology, [NotNull("NullExceptionObject")] GraphicsBuffer indexBuffer, [NotNull("NullExceptionObject")] GraphicsBuffer commandBuffer, int commandCount, int startCommand)
		{
			Graphics.Internal_RenderPrimitivesIndexedIndirect_Injected(ref rparams, topology, indexBuffer, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
		[FreeFunction("GraphicsScripting::DrawMesh")]
		private static void Internal_DrawMesh(Mesh mesh, int submeshIndex, Matrix4x4 matrix, Material material, int layer, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
		{
			Graphics.Internal_DrawMesh_Injected(mesh, submeshIndex, ref matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x06000982 RID: 2434
		[FreeFunction("GraphicsScripting::DrawMeshInstanced")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshInstanced([NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, [NotNull("NullExceptionObject")] Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

		// Token: 0x06000983 RID: 2435 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		[FreeFunction("GraphicsScripting::DrawMeshInstancedProcedural")]
		private static void Internal_DrawMeshInstancedProcedural([NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, [NotNull("NullExceptionObject")] Material material, Bounds bounds, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
		{
			Graphics.Internal_DrawMeshInstancedProcedural_Injected(mesh, submeshIndex, material, ref bounds, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		[FreeFunction("GraphicsScripting::DrawMeshInstancedIndirect")]
		private static void Internal_DrawMeshInstancedIndirect([NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, [NotNull("NullExceptionObject")] Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
		{
			Graphics.Internal_DrawMeshInstancedIndirect_Injected(mesh, submeshIndex, material, ref bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000F21C File Offset: 0x0000D41C
		[FreeFunction("GraphicsScripting::DrawMeshInstancedIndirect")]
		private static void Internal_DrawMeshInstancedIndirectGraphicsBuffer([NotNull("NullExceptionObject")] Mesh mesh, int submeshIndex, [NotNull("NullExceptionObject")] Material material, Bounds bounds, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
		{
			Graphics.Internal_DrawMeshInstancedIndirectGraphicsBuffer_Injected(mesh, submeshIndex, material, ref bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x06000986 RID: 2438
		[FreeFunction("GraphicsScripting::DrawProceduralNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralNow(MeshTopology topology, int vertexCount, int instanceCount);

		// Token: 0x06000987 RID: 2439
		[FreeFunction("GraphicsScripting::DrawProceduralIndexedNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexedNow(MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount);

		// Token: 0x06000988 RID: 2440
		[FreeFunction("GraphicsScripting::DrawProceduralIndirectNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndirectNow(MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset);

		// Token: 0x06000989 RID: 2441
		[FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirectNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexedIndirectNow(MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset);

		// Token: 0x0600098A RID: 2442
		[FreeFunction("GraphicsScripting::DrawProceduralIndirectNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndirectNowGraphicsBuffer(MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset);

		// Token: 0x0600098B RID: 2443
		[FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirectNow")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexedIndirectNowGraphicsBuffer(MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer bufferWithArgs, int argsOffset);

		// Token: 0x0600098C RID: 2444 RVA: 0x0000F248 File Offset: 0x0000D448
		[FreeFunction("GraphicsScripting::DrawProcedural")]
		private static void Internal_DrawProcedural(Material material, Bounds bounds, MeshTopology topology, int vertexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProcedural_Injected(material, ref bounds, topology, vertexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0000F26C File Offset: 0x0000D46C
		[FreeFunction("GraphicsScripting::DrawProceduralIndexed")]
		private static void Internal_DrawProceduralIndexed(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProceduralIndexed_Injected(material, ref bounds, topology, indexBuffer, indexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0000F294 File Offset: 0x0000D494
		[FreeFunction("GraphicsScripting::DrawProceduralIndirect")]
		private static void Internal_DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProceduralIndirect_Injected(material, ref bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		[FreeFunction("GraphicsScripting::DrawProceduralIndirect")]
		private static void Internal_DrawProceduralIndirectGraphicsBuffer(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProceduralIndirectGraphicsBuffer_Injected(material, ref bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		[FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirect")]
		private static void Internal_DrawProceduralIndexedIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProceduralIndexedIndirect_Injected(material, ref bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0000F304 File Offset: 0x0000D504
		[FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirect")]
		private static void Internal_DrawProceduralIndexedIndirectGraphicsBuffer(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.Internal_DrawProceduralIndexedIndirectGraphicsBuffer_Injected(material, ref bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x06000992 RID: 2450
		[FreeFunction("GraphicsScripting::BlitMaterial")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_BlitMaterial5(Texture source, RenderTexture dest, [NotNull("ArgumentNullException")] Material mat, int pass, bool setRT);

		// Token: 0x06000993 RID: 2451
		[FreeFunction("GraphicsScripting::BlitMaterial")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_BlitMaterial6(Texture source, RenderTexture dest, [NotNull("ArgumentNullException")] Material mat, int pass, bool setRT, int destDepthSlice);

		// Token: 0x06000994 RID: 2452
		[FreeFunction("GraphicsScripting::BlitMultitap")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_BlitMultiTap4(Texture source, RenderTexture dest, [NotNull("ArgumentNullException")] Material mat, [NotNull("ArgumentNullException")] Vector2[] offsets);

		// Token: 0x06000995 RID: 2453
		[FreeFunction("GraphicsScripting::BlitMultitap")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_BlitMultiTap5(Texture source, RenderTexture dest, [NotNull("ArgumentNullException")] Material mat, [NotNull("ArgumentNullException")] Vector2[] offsets, int destDepthSlice);

		// Token: 0x06000996 RID: 2454
		[FreeFunction("GraphicsScripting::Blit")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Blit2(Texture source, RenderTexture dest);

		// Token: 0x06000997 RID: 2455
		[FreeFunction("GraphicsScripting::Blit")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Blit3(Texture source, RenderTexture dest, int sourceDepthSlice, int destDepthSlice);

		// Token: 0x06000998 RID: 2456 RVA: 0x0000F329 File Offset: 0x0000D529
		[FreeFunction("GraphicsScripting::Blit")]
		private static void Blit4(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset)
		{
			Graphics.Blit4_Injected(source, dest, ref scale, ref offset);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0000F336 File Offset: 0x0000D536
		[FreeFunction("GraphicsScripting::Blit")]
		private static void Blit5(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
		{
			Graphics.Blit5_Injected(source, dest, ref scale, ref offset, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x0600099A RID: 2458
		[NativeMethod(Name = "GraphicsScripting::CreateGPUFence", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateGPUFenceImpl(GraphicsFenceType fenceType, SynchronisationStageFlags stage);

		// Token: 0x0600099B RID: 2459
		[NativeMethod(Name = "GraphicsScripting::WaitOnGPUFence", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WaitOnGPUFenceImpl(IntPtr fencePtr, SynchronisationStageFlags stage);

		// Token: 0x0600099C RID: 2460
		[NativeMethod(Name = "GraphicsScripting::ExecuteCommandBuffer", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCommandBuffer([NotNull("ArgumentNullException")] CommandBuffer buffer);

		// Token: 0x0600099D RID: 2461
		[NativeMethod(Name = "GraphicsScripting::ExecuteCommandBufferAsync", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCommandBufferAsync([NotNull("ArgumentNullException")] CommandBuffer buffer, ComputeQueueType queueType);

		// Token: 0x0600099E RID: 2462 RVA: 0x0000F348 File Offset: 0x0000D548
		internal static void CheckLoadActionValid(RenderBufferLoadAction load, string bufferType)
		{
			bool flag = load != RenderBufferLoadAction.Load && load != RenderBufferLoadAction.DontCare;
			if (flag)
			{
				throw new ArgumentException(UnityString.Format("Bad {0} LoadAction provided.", new object[] { bufferType }));
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0000F384 File Offset: 0x0000D584
		internal static void CheckStoreActionValid(RenderBufferStoreAction store, string bufferType)
		{
			bool flag = store != RenderBufferStoreAction.Store && store != RenderBufferStoreAction.DontCare;
			if (flag)
			{
				throw new ArgumentException(UnityString.Format("Bad {0} StoreAction provided.", new object[] { bufferType }));
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000F3C0 File Offset: 0x0000D5C0
		internal static void SetRenderTargetImpl(RenderTargetSetup setup)
		{
			bool flag = setup.color.Length == 0;
			if (flag)
			{
				throw new ArgumentException("Invalid color buffer count for SetRenderTarget");
			}
			bool flag2 = setup.color.Length != setup.colorLoad.Length;
			if (flag2)
			{
				throw new ArgumentException("Color LoadAction and Buffer arrays have different sizes");
			}
			bool flag3 = setup.color.Length != setup.colorStore.Length;
			if (flag3)
			{
				throw new ArgumentException("Color StoreAction and Buffer arrays have different sizes");
			}
			foreach (RenderBufferLoadAction renderBufferLoadAction in setup.colorLoad)
			{
				Graphics.CheckLoadActionValid(renderBufferLoadAction, "Color");
			}
			foreach (RenderBufferStoreAction renderBufferStoreAction in setup.colorStore)
			{
				Graphics.CheckStoreActionValid(renderBufferStoreAction, "Color");
			}
			Graphics.CheckLoadActionValid(setup.depthLoad, "Depth");
			Graphics.CheckStoreActionValid(setup.depthStore, "Depth");
			bool flag4 = setup.cubemapFace < CubemapFace.Unknown || setup.cubemapFace > CubemapFace.NegativeZ;
			if (flag4)
			{
				throw new ArgumentException("Bad CubemapFace provided");
			}
			Graphics.Internal_SetMRTFullSetup(setup.color, setup.depth, setup.mipLevel, setup.cubemapFace, setup.depthSlice, setup.colorLoad, setup.colorStore, setup.depthLoad, setup.depthStore);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0000F510 File Offset: 0x0000D710
		internal static void SetRenderTargetImpl(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face, int depthSlice)
		{
			Graphics.Internal_SetRTSimple(colorBuffer, depthBuffer, mipLevel, face, depthSlice);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0000F520 File Offset: 0x0000D720
		internal static void SetRenderTargetImpl(RenderTexture rt, int mipLevel, CubemapFace face, int depthSlice)
		{
			bool flag = rt;
			if (flag)
			{
				Graphics.SetRenderTargetImpl(rt.colorBuffer, rt.depthBuffer, mipLevel, face, depthSlice);
			}
			else
			{
				Graphics.Internal_SetNullRT();
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0000F558 File Offset: 0x0000D758
		internal static void SetRenderTargetImpl(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer, int mipLevel, CubemapFace face, int depthSlice)
		{
			Graphics.Internal_SetMRTSimple(colorBuffers, depthBuffer, mipLevel, face, depthSlice);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0000F574 File Offset: 0x0000D774
		public static void SetRenderTarget(RenderTexture rt, [DefaultValue("0")] int mipLevel, [DefaultValue("CubemapFace.Unknown")] CubemapFace face, [DefaultValue("0")] int depthSlice)
		{
			Graphics.SetRenderTargetImpl(rt, mipLevel, face, depthSlice);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0000F581 File Offset: 0x0000D781
		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, [DefaultValue("0")] int mipLevel, [DefaultValue("CubemapFace.Unknown")] CubemapFace face, [DefaultValue("0")] int depthSlice)
		{
			Graphics.SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, face, depthSlice);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0000F590 File Offset: 0x0000D790
		public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer)
		{
			Graphics.SetRenderTargetImpl(colorBuffers, depthBuffer, 0, CubemapFace.Unknown, 0);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0000F59E File Offset: 0x0000D79E
		public static void SetRenderTarget(RenderTargetSetup setup)
		{
			Graphics.SetRenderTargetImpl(setup);
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0000F5A8 File Offset: 0x0000D7A8
		public static RenderBuffer activeColorBuffer
		{
			get
			{
				return Graphics.GetActiveColorBuffer();
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		public static RenderBuffer activeDepthBuffer
		{
			get
			{
				return Graphics.GetActiveDepthBuffer();
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		public static void SetRandomWriteTarget(int index, RenderTexture uav)
		{
			bool flag = index < 0 || index >= SystemInfo.supportedRandomWriteTargetCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("must be non-negative less than {0}.", SystemInfo.supportedRandomWriteTargetCount));
			}
			Graphics.Internal_SetRandomWriteTargetRT(index, uav);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000F624 File Offset: 0x0000D824
		public static void SetRandomWriteTarget(int index, ComputeBuffer uav, [DefaultValue("false")] bool preserveCounterValue)
		{
			bool flag = uav == null;
			if (flag)
			{
				throw new ArgumentNullException("uav");
			}
			bool flag2 = uav.m_Ptr == IntPtr.Zero;
			if (flag2)
			{
				throw new ObjectDisposedException("uav");
			}
			bool flag3 = index < 0 || index >= SystemInfo.supportedRandomWriteTargetCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("must be non-negative less than {0}.", SystemInfo.supportedRandomWriteTargetCount));
			}
			Graphics.Internal_SetRandomWriteTargetBuffer(index, uav, preserveCounterValue);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		public static void SetRandomWriteTarget(int index, GraphicsBuffer uav, [DefaultValue("false")] bool preserveCounterValue)
		{
			bool flag = uav == null;
			if (flag)
			{
				throw new ArgumentNullException("uav");
			}
			bool flag2 = uav.m_Ptr == IntPtr.Zero;
			if (flag2)
			{
				throw new ObjectDisposedException("uav");
			}
			bool flag3 = index < 0 || index >= SystemInfo.supportedRandomWriteTargetCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("must be non-negative less than {0}.", SystemInfo.supportedRandomWriteTargetCount));
			}
			Graphics.Internal_SetRandomWriteTargetGraphicsBuffer(index, uav, preserveCounterValue);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0000F722 File Offset: 0x0000D922
		public static void CopyTexture(Texture src, Texture dst)
		{
			Graphics.CopyTexture_Full(src, dst);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0000F72D File Offset: 0x0000D92D
		public static void CopyTexture(Texture src, int srcElement, Texture dst, int dstElement)
		{
			Graphics.CopyTexture_Slice_AllMips(src, srcElement, dst, dstElement);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0000F73A File Offset: 0x0000D93A
		public static void CopyTexture(Texture src, int srcElement, int srcMip, Texture dst, int dstElement, int dstMip)
		{
			Graphics.CopyTexture_Slice(src, srcElement, srcMip, dst, dstElement, dstMip);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0000F74C File Offset: 0x0000D94C
		public static void CopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY)
		{
			Graphics.CopyTexture_Region(src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstElement, dstMip, dstX, dstY);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0000F774 File Offset: 0x0000D974
		public static bool ConvertTexture(Texture src, Texture dst)
		{
			return Graphics.ConvertTexture_Full(src, dst);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0000F790 File Offset: 0x0000D990
		public static bool ConvertTexture(Texture src, int srcElement, Texture dst, int dstElement)
		{
			return Graphics.ConvertTexture_Slice(src, srcElement, dst, dstElement);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public static GraphicsFence CreateAsyncGraphicsFence([DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStage stage)
		{
			return Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		public static GraphicsFence CreateAsyncGraphicsFence()
		{
			return Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
		public static GraphicsFence CreateGraphicsFence(GraphicsFenceType fenceType, [DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStageFlags stage)
		{
			GraphicsFence graphicsFence = default(GraphicsFence);
			graphicsFence.m_FenceType = fenceType;
			graphicsFence.m_Ptr = Graphics.CreateGPUFenceImpl(fenceType, stage);
			graphicsFence.InitPostAllocation();
			graphicsFence.Validate();
			return graphicsFence;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000F829 File Offset: 0x0000DA29
		public static void WaitOnAsyncGraphicsFence(GraphicsFence fence)
		{
			Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0000F834 File Offset: 0x0000DA34
		public static void WaitOnAsyncGraphicsFence(GraphicsFence fence, [DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStage stage)
		{
			bool flag = fence.m_FenceType > GraphicsFenceType.AsyncQueueSynchronisation;
			if (flag)
			{
				throw new ArgumentException("Graphics.WaitOnGraphicsFence can only be called with fences created with GraphicsFenceType.AsyncQueueSynchronization.");
			}
			fence.Validate();
			bool flag2 = fence.IsFencePending();
			if (flag2)
			{
				Graphics.WaitOnGPUFenceImpl(fence.m_Ptr, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0000F880 File Offset: 0x0000DA80
		internal static void ValidateCopyBuffer(GraphicsBuffer source, GraphicsBuffer dest)
		{
			bool flag = source == null;
			if (flag)
			{
				throw new ArgumentNullException("source");
			}
			bool flag2 = dest == null;
			if (flag2)
			{
				throw new ArgumentNullException("dest");
			}
			long num = (long)source.count * (long)source.stride;
			long num2 = (long)dest.count * (long)dest.stride;
			bool flag3 = num != num2;
			if (flag3)
			{
				throw new ArgumentException(string.Format("CopyBuffer source and destination buffers must be the same size, source was {0} bytes, dest was {1} bytes", num, num2));
			}
			bool flag4 = (source.target & GraphicsBuffer.Target.CopySource) == (GraphicsBuffer.Target)0;
			if (flag4)
			{
				throw new ArgumentException("CopyBuffer source must have CopySource target", "source");
			}
			bool flag5 = (dest.target & GraphicsBuffer.Target.CopyDestination) == (GraphicsBuffer.Target)0;
			if (flag5)
			{
				throw new ArgumentException("CopyBuffer destination must have CopyDestination target", "dest");
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000F93F File Offset: 0x0000DB3F
		public static void CopyBuffer(GraphicsBuffer source, GraphicsBuffer dest)
		{
			Graphics.ValidateCopyBuffer(source, dest);
			Graphics.CopyBufferImpl(source, dest);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0000F954 File Offset: 0x0000DB54
		private static void DrawTextureImpl(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat, int pass)
		{
			Internal_DrawTextureArguments internal_DrawTextureArguments = default(Internal_DrawTextureArguments);
			internal_DrawTextureArguments.screenRect = screenRect;
			internal_DrawTextureArguments.sourceRect = sourceRect;
			internal_DrawTextureArguments.leftBorder = leftBorder;
			internal_DrawTextureArguments.rightBorder = rightBorder;
			internal_DrawTextureArguments.topBorder = topBorder;
			internal_DrawTextureArguments.bottomBorder = bottomBorder;
			internal_DrawTextureArguments.color = color;
			internal_DrawTextureArguments.leftBorderColor = Color.black;
			internal_DrawTextureArguments.topBorderColor = Color.black;
			internal_DrawTextureArguments.rightBorderColor = Color.black;
			internal_DrawTextureArguments.bottomBorderColor = Color.black;
			internal_DrawTextureArguments.pass = pass;
			internal_DrawTextureArguments.texture = texture;
			internal_DrawTextureArguments.smoothCorners = true;
			internal_DrawTextureArguments.mat = mat;
			Graphics.Internal_DrawTexture(ref internal_DrawTextureArguments);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0000FA00 File Offset: 0x0000DC00
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass)
		{
			Graphics.DrawTextureImpl(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, pass);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0000FA24 File Offset: 0x0000DC24
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass)
		{
			Color32 color = new Color32(128, 128, 128, 128);
			Graphics.DrawTextureImpl(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, pass);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000FA68 File Offset: 0x0000DC68
		public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass)
		{
			Graphics.DrawTexture(screenRect, texture, new Rect(0f, 0f, 1f, 1f), leftBorder, rightBorder, topBorder, bottomBorder, mat, pass);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000FAA1 File Offset: 0x0000DCA1
		public static void DrawTexture(Rect screenRect, Texture texture, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass)
		{
			Graphics.DrawTexture(screenRect, texture, 0, 0, 0, 0, mat, pass);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		public unsafe static void RenderMesh(in RenderParams rparams, Mesh mesh, int submeshIndex, Matrix4x4 objectToWorld, [DefaultValue("null")] Matrix4x4? prevObjectToWorld = null)
		{
			bool flag = prevObjectToWorld != null;
			if (flag)
			{
				Matrix4x4 value = prevObjectToWorld.Value;
				Graphics.Internal_RenderMesh(rparams, mesh, submeshIndex, objectToWorld, &value);
			}
			else
			{
				Graphics.Internal_RenderMesh(rparams, mesh, submeshIndex, objectToWorld, null);
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		private static RenderInstancedDataLayout GetCachedRenderInstancedDataLayout(Type type)
		{
			int hashCode = type.GetHashCode();
			RenderInstancedDataLayout renderInstancedDataLayout;
			bool flag = !Graphics.s_RenderInstancedDataLayouts.TryGetValue(hashCode, out renderInstancedDataLayout);
			if (flag)
			{
				renderInstancedDataLayout = new RenderInstancedDataLayout(type);
				Graphics.s_RenderInstancedDataLayouts.Add(hashCode, renderInstancedDataLayout);
			}
			return renderInstancedDataLayout;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public unsafe static void RenderMeshInstanced<[IsUnmanaged] T>(in RenderParams rparams, Mesh mesh, int submeshIndex, T[] instanceData, [DefaultValue("-1")] int instanceCount = -1, [DefaultValue("0")] int startInstance = 0) where T : struct, ValueType
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !rparams.material.enableInstancing;
			if (flag2)
			{
				throw new InvalidOperationException("Material needs to enable instancing for use with RenderMeshInstanced.");
			}
			bool flag3 = instanceData == null;
			if (flag3)
			{
				throw new ArgumentNullException("instanceData");
			}
			RenderInstancedDataLayout cachedRenderInstancedDataLayout = Graphics.GetCachedRenderInstancedDataLayout(typeof(T));
			uint num = Math.Min((uint)instanceCount, (uint)Math.Max(0, instanceData.Length - startInstance));
			fixed (T[] array = instanceData)
			{
				T* ptr;
				if (instanceData == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				Graphics.Internal_RenderMeshInstanced(rparams, mesh, submeshIndex, (IntPtr)((void*)(ptr + (IntPtr)startInstance * (IntPtr)sizeof(T) / (IntPtr)sizeof(T))), cachedRenderInstancedDataLayout, num);
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0000FC04 File Offset: 0x0000DE04
		public unsafe static void RenderMeshInstanced<[IsUnmanaged] T>(in RenderParams rparams, Mesh mesh, int submeshIndex, List<T> instanceData, [DefaultValue("-1")] int instanceCount = -1, [DefaultValue("0")] int startInstance = 0) where T : struct, ValueType
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !rparams.material.enableInstancing;
			if (flag2)
			{
				throw new InvalidOperationException("Material needs to enable instancing for use with RenderMeshInstanced.");
			}
			bool flag3 = instanceData == null;
			if (flag3)
			{
				throw new ArgumentNullException("instanceData");
			}
			RenderInstancedDataLayout cachedRenderInstancedDataLayout = Graphics.GetCachedRenderInstancedDataLayout(typeof(T));
			uint num = Math.Min((uint)instanceCount, (uint)Math.Max(0, instanceData.Count - startInstance));
			T[] array;
			T* ptr;
			if ((array = NoAllocHelpers.ExtractArrayFromListT<T>(instanceData)) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			Graphics.Internal_RenderMeshInstanced(rparams, mesh, submeshIndex, (IntPtr)((void*)(ptr + (IntPtr)startInstance * (IntPtr)sizeof(T) / (IntPtr)sizeof(T))), cachedRenderInstancedDataLayout, num);
			array = null;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0000FCCC File Offset: 0x0000DECC
		public unsafe static void RenderMeshInstanced<[IsUnmanaged] T>(RenderParams rparams, Mesh mesh, int submeshIndex, NativeArray<T> instanceData, [DefaultValue("-1")] int instanceCount = -1, [DefaultValue("0")] int startInstance = 0) where T : struct, ValueType
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !rparams.material.enableInstancing;
			if (flag2)
			{
				throw new InvalidOperationException("Material needs to enable instancing for use with RenderMeshInstanced.");
			}
			RenderInstancedDataLayout cachedRenderInstancedDataLayout = Graphics.GetCachedRenderInstancedDataLayout(typeof(T));
			uint num = Math.Min((uint)instanceCount, (uint)Math.Max(0, instanceData.Length - startInstance));
			Graphics.Internal_RenderMeshInstanced(rparams, mesh, submeshIndex, (IntPtr)((void*)((byte*)instanceData.GetUnsafePtr<T>() + (IntPtr)startInstance * (IntPtr)sizeof(T))), cachedRenderInstancedDataLayout, num);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000FD60 File Offset: 0x0000DF60
		public static void RenderMeshIndirect(in RenderParams rparams, Mesh mesh, GraphicsBuffer commandBuffer, [DefaultValue("1")] int commandCount = 1, [DefaultValue("0")] int startCommand = 0)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			Graphics.Internal_RenderMeshIndirect(rparams, mesh, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0000FDB0 File Offset: 0x0000DFB0
		public static void RenderMeshPrimitives(in RenderParams rparams, Mesh mesh, int submeshIndex, [DefaultValue("1")] int instanceCount = 1)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			Graphics.Internal_RenderMeshPrimitives(rparams, mesh, submeshIndex, instanceCount);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		public static void RenderPrimitives(in RenderParams rparams, MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount = 1)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			Graphics.Internal_RenderPrimitives(rparams, topology, vertexCount, instanceCount);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0000FE18 File Offset: 0x0000E018
		public static void RenderPrimitivesIndexed(in RenderParams rparams, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, [DefaultValue("0")] int startIndex = 0, [DefaultValue("1")] int instanceCount = 1)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			Graphics.Internal_RenderPrimitivesIndexed(rparams, topology, indexBuffer, indexCount, startIndex, instanceCount);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0000FE50 File Offset: 0x0000E050
		public static void RenderPrimitivesIndirect(in RenderParams rparams, MeshTopology topology, GraphicsBuffer commandBuffer, [DefaultValue("1")] int commandCount = 1, [DefaultValue("0")] int startCommand = 0)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			Graphics.Internal_RenderPrimitivesIndirect(rparams, topology, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		public static void RenderPrimitivesIndexedIndirect(in RenderParams rparams, MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer commandBuffer, [DefaultValue("1")] int commandCount = 1, [DefaultValue("0")] int startCommand = 0)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			Graphics.Internal_RenderPrimitivesIndexedIndirect(rparams, topology, indexBuffer, commandBuffer, commandCount, startCommand);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
		{
			bool flag = mesh == null;
			if (flag)
			{
				throw new ArgumentNullException("mesh");
			}
			Graphics.Internal_DrawMeshNow1(mesh, materialIndex, position, rotation);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0000FF20 File Offset: 0x0000E120
		public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix, int materialIndex)
		{
			bool flag = mesh == null;
			if (flag)
			{
				throw new ArgumentNullException("mesh");
			}
			Graphics.Internal_DrawMeshNow2(mesh, materialIndex, matrix);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0000FF4D File Offset: 0x0000E14D
		public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Graphics.DrawMeshNow(mesh, position, rotation, -1);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000FF5A File Offset: 0x0000E15A
		public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix)
		{
			Graphics.DrawMeshNow(mesh, matrix, -1);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000FF68 File Offset: 0x0000E168
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("true")] bool useLightProbes)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000FFA8 File Offset: 0x0000E1A8
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor, [DefaultValue("true")] bool useLightProbes)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("true")] bool useLightProbes)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00010018 File Offset: 0x0000E218
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, [DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
		{
			bool flag = lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null;
			if (flag)
			{
				throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
			}
			Graphics.Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00010064 File Offset: 0x0000E264
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, [DefaultValue("matrices.Length")] int count, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("0")] int layer, [DefaultValue("null")] Camera camera, [DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = mesh == null;
			if (flag2)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag3 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag4 = material == null;
			if (flag4)
			{
				throw new ArgumentNullException("material");
			}
			bool flag5 = !material.enableInstancing;
			if (flag5)
			{
				throw new InvalidOperationException("Material needs to enable instancing for use with DrawMeshInstanced.");
			}
			bool flag6 = matrices == null;
			if (flag6)
			{
				throw new ArgumentNullException("matrices");
			}
			bool flag7 = count < 0 || count > Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length);
			if (flag7)
			{
				throw new ArgumentOutOfRangeException("count", string.Format("Count must be in the range of 0 to {0}.", Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length)));
			}
			bool flag8 = lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null;
			if (flag8)
			{
				throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
			}
			bool flag9 = count > 0;
			if (flag9)
			{
				Graphics.Internal_DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001019C File Offset: 0x0000E39C
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("0")] int layer, [DefaultValue("null")] Camera camera, [DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
		{
			bool flag = matrices == null;
			if (flag)
			{
				throw new ArgumentNullException("matrices");
			}
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, NoAllocHelpers.ExtractArrayFromListT<Matrix4x4>(matrices), matrices.Count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public static void DrawMeshInstancedProcedural(Mesh mesh, int submeshIndex, Material material, Bounds bounds, int count, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes, LightProbeProxyVolume lightProbeProxyVolume = null)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = mesh == null;
			if (flag2)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag3 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag4 = material == null;
			if (flag4)
			{
				throw new ArgumentNullException("material");
			}
			bool flag5 = count <= 0;
			if (flag5)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag6 = lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null;
			if (flag6)
			{
				throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
			}
			bool flag7 = count > 0;
			if (flag7)
			{
				Graphics.Internal_DrawMeshInstancedProcedural(mesh, submeshIndex, material, bounds, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000102BC File Offset: 0x0000E4BC
		public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("0")] int layer, [DefaultValue("null")] Camera camera, [DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag3 = mesh == null;
			if (flag3)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag4 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag5 = material == null;
			if (flag5)
			{
				throw new ArgumentNullException("material");
			}
			bool flag6 = bufferWithArgs == null;
			if (flag6)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			bool flag7 = lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null;
			if (flag7)
			{
				throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
			}
			Graphics.Internal_DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000103A8 File Offset: 0x0000E5A8
		public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, GraphicsBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("0")] int layer, [DefaultValue("null")] Camera camera, [DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag3 = mesh == null;
			if (flag3)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag4 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag5 = material == null;
			if (flag5)
			{
				throw new ArgumentNullException("material");
			}
			bool flag6 = bufferWithArgs == null;
			if (flag6)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			bool flag7 = lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null;
			if (flag7)
			{
				throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
			}
			Graphics.Internal_DrawMeshInstancedIndirectGraphicsBuffer(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00010491 File Offset: 0x0000E691
		public static void DrawProceduralNow(MeshTopology topology, int vertexCount, int instanceCount = 1)
		{
			Graphics.Internal_DrawProceduralNow(topology, vertexCount, instanceCount);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000104A0 File Offset: 0x0000E6A0
		public static void DrawProceduralNow(MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount = 1)
		{
			bool flag = indexBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			Graphics.Internal_DrawProceduralIndexedNow(topology, indexBuffer, indexCount, instanceCount);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000104CC File Offset: 0x0000E6CC
		public static void DrawProceduralIndirectNow(MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = bufferWithArgs == null;
			if (flag2)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndirectNow(topology, bufferWithArgs, argsOffset);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00010510 File Offset: 0x0000E710
		public static void DrawProceduralIndirectNow(MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndexedIndirectNow(topology, indexBuffer, bufferWithArgs, argsOffset);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00010568 File Offset: 0x0000E768
		public static void DrawProceduralIndirectNow(MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = bufferWithArgs == null;
			if (flag2)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndirectNowGraphicsBuffer(topology, bufferWithArgs, argsOffset);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x000105AC File Offset: 0x0000E7AC
		public static void DrawProceduralIndirectNow(MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer bufferWithArgs, int argsOffset = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndexedIndirectNowGraphicsBuffer(topology, indexBuffer, bufferWithArgs, argsOffset);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00010604 File Offset: 0x0000E804
		public static void DrawProcedural(Material material, Bounds bounds, MeshTopology topology, int vertexCount, int instanceCount = 1, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			Graphics.Internal_DrawProcedural(material, bounds, topology, vertexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00010628 File Offset: 0x0000E828
		public static void DrawProcedural(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount = 1, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			bool flag = indexBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			Graphics.Internal_DrawProceduralIndexed(material, bounds, topology, indexBuffer, indexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00010664 File Offset: 0x0000E864
		public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = bufferWithArgs == null;
			if (flag2)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndirect(material, bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000106B4 File Offset: 0x0000E8B4
		public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = bufferWithArgs == null;
			if (flag2)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndirectGraphicsBuffer(material, bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00010704 File Offset: 0x0000E904
		public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndexedIndirect(material, bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00010768 File Offset: 0x0000E968
		public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Graphics.Internal_DrawProceduralIndexedIndirectGraphicsBuffer(material, bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000107CC File Offset: 0x0000E9CC
		public static void Blit(Texture source, RenderTexture dest)
		{
			Graphics.Blit2(source, dest);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000107D7 File Offset: 0x0000E9D7
		public static void Blit(Texture source, RenderTexture dest, int sourceDepthSlice, int destDepthSlice)
		{
			Graphics.Blit3(source, dest, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000107E4 File Offset: 0x0000E9E4
		public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset)
		{
			Graphics.Blit4(source, dest, scale, offset);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000107F1 File Offset: 0x0000E9F1
		public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
		{
			Graphics.Blit5(source, dest, scale, offset, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00010802 File Offset: 0x0000EA02
		public static void Blit(Texture source, RenderTexture dest, Material mat, [DefaultValue("-1")] int pass)
		{
			Graphics.Internal_BlitMaterial5(source, dest, mat, pass, true);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00010810 File Offset: 0x0000EA10
		public static void Blit(Texture source, RenderTexture dest, Material mat, int pass, int destDepthSlice)
		{
			Graphics.Internal_BlitMaterial6(source, dest, mat, pass, true, destDepthSlice);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00010820 File Offset: 0x0000EA20
		public static void Blit(Texture source, RenderTexture dest, Material mat)
		{
			Graphics.Blit(source, dest, mat, -1);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001082D File Offset: 0x0000EA2D
		public static void Blit(Texture source, Material mat, [DefaultValue("-1")] int pass)
		{
			Graphics.Internal_BlitMaterial5(source, null, mat, pass, false);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001083B File Offset: 0x0000EA3B
		public static void Blit(Texture source, Material mat, int pass, int destDepthSlice)
		{
			Graphics.Internal_BlitMaterial6(source, null, mat, pass, false, destDepthSlice);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001084A File Offset: 0x0000EA4A
		public static void Blit(Texture source, Material mat)
		{
			Graphics.Blit(source, mat, -1);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00010858 File Offset: 0x0000EA58
		public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, params Vector2[] offsets)
		{
			bool flag = offsets.Length == 0;
			if (flag)
			{
				throw new ArgumentException("empty offsets list passed.", "offsets");
			}
			Graphics.Internal_BlitMultiTap4(source, dest, mat, offsets);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0001088C File Offset: 0x0000EA8C
		public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, int destDepthSlice, params Vector2[] offsets)
		{
			bool flag = offsets.Length == 0;
			if (flag)
			{
				throw new ArgumentException("empty offsets list passed.", "offsets");
			}
			Graphics.Internal_BlitMultiTap5(source, dest, mat, offsets, destDepthSlice);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000108C0 File Offset: 0x0000EAC0
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, null, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000108EC File Offset: 0x0000EAEC
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001091C File Offset: 0x0000EB1C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001094C File Offset: 0x0000EB4C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001097C File Offset: 0x0000EB7C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x000109B4 File Offset: 0x0000EBB4
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x000109EC File Offset: 0x0000EBEC
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00010A1C File Offset: 0x0000EC1C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00010A50 File Offset: 0x0000EC50
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00010A84 File Offset: 0x0000EC84
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, null, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00010AA4 File Offset: 0x0000ECA4
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00010AC8 File Offset: 0x0000ECC8
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00010AEC File Offset: 0x0000ECEC
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00010B10 File Offset: 0x0000ED10
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00010B3C File Offset: 0x0000ED3C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00010B68 File Offset: 0x0000ED68
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, true, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00010B8C File Offset: 0x0000ED8C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor, [DefaultValue("true")] bool useLightProbes)
		{
			Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00010C0C File Offset: 0x0000EE0C
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage)
		{
			Graphics.Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, null);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00010C34 File Offset: 0x0000EE34
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, matrices.Length, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00010C58 File Offset: 0x0000EE58
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00010C7C File Offset: 0x0000EE7C
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00010CA0 File Offset: 0x0000EEA0
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00010CC4 File Offset: 0x0000EEC4
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00010D10 File Offset: 0x0000EF10
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00010D38 File Offset: 0x0000EF38
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00010D60 File Offset: 0x0000EF60
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00010D80 File Offset: 0x0000EF80
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00010DA0 File Offset: 0x0000EFA0
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, true, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, 0, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, null, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00010E0C File Offset: 0x0000F00C
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, LightProbeUsage.BlendProbes, null);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00010E30 File Offset: 0x0000F030
		[ExcludeFromDocs]
		public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
		{
			Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00010E58 File Offset: 0x0000F058
		[ExcludeFromDocs]
		public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset = 0, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes)
		{
			Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00010E84 File Offset: 0x0000F084
		[ExcludeFromDocs]
		public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, GraphicsBuffer bufferWithArgs, int argsOffset = 0, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes)
		{
			Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00010EB0 File Offset: 0x0000F0B0
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat)
		{
			Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, -1);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color)
		{
			Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, null, -1);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
		{
			Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00010F1C File Offset: 0x0000F11C
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, null, -1);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00010F3C File Offset: 0x0000F13C
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
		{
			Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00010F50 File Offset: 0x0000F150
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, null, -1);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00010F63 File Offset: 0x0000F163
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Material mat)
		{
			Graphics.DrawTexture(screenRect, texture, mat, -1);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00010F70 File Offset: 0x0000F170
		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture)
		{
			Graphics.DrawTexture(screenRect, texture, null, -1);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00010F7D File Offset: 0x0000F17D
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderTexture rt)
		{
			Graphics.SetRenderTarget(rt, 0, CubemapFace.Unknown, 0);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00010F8A File Offset: 0x0000F18A
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderTexture rt, int mipLevel)
		{
			Graphics.SetRenderTarget(rt, mipLevel, CubemapFace.Unknown, 0);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00010F97 File Offset: 0x0000F197
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderTexture rt, int mipLevel, CubemapFace face)
		{
			Graphics.SetRenderTarget(rt, mipLevel, face, 0);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00010FA4 File Offset: 0x0000F1A4
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			Graphics.SetRenderTarget(colorBuffer, depthBuffer, 0, CubemapFace.Unknown, 0);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00010FB2 File Offset: 0x0000F1B2
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel)
		{
			Graphics.SetRenderTarget(colorBuffer, depthBuffer, mipLevel, CubemapFace.Unknown, 0);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00010FC0 File Offset: 0x0000F1C0
		[ExcludeFromDocs]
		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
		{
			Graphics.SetRenderTarget(colorBuffer, depthBuffer, mipLevel, face, 0);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00010FCE File Offset: 0x0000F1CE
		[ExcludeFromDocs]
		public static void SetRandomWriteTarget(int index, ComputeBuffer uav)
		{
			Graphics.SetRandomWriteTarget(index, uav, false);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00010FDA File Offset: 0x0000F1DA
		[ExcludeFromDocs]
		public static void SetRandomWriteTarget(int index, GraphicsBuffer uav)
		{
			Graphics.SetRandomWriteTarget(index, uav, false);
		}

		// Token: 0x06000A26 RID: 2598
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetActiveColorBuffer_Injected(out RenderBuffer ret);

		// Token: 0x06000A27 RID: 2599
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetActiveDepthBuffer_Injected(out RenderBuffer ret);

		// Token: 0x06000A28 RID: 2600
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetRTSimple_Injected(ref RenderBuffer color, ref RenderBuffer depth, int mip, CubemapFace face, int depthSlice);

		// Token: 0x06000A29 RID: 2601
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetMRTSimple_Injected(RenderBuffer[] color, ref RenderBuffer depth, int mip, CubemapFace face, int depthSlice);

		// Token: 0x06000A2A RID: 2602
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetMRTFullSetup_Injected(RenderBuffer[] color, ref RenderBuffer depth, int mip, CubemapFace face, int depthSlice, RenderBufferLoadAction[] colorLA, RenderBufferStoreAction[] colorSA, RenderBufferLoadAction depthLA, RenderBufferStoreAction depthSA);

		// Token: 0x06000A2B RID: 2603
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshNow1_Injected(Mesh mesh, int subsetIndex, ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06000A2C RID: 2604
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshNow2_Injected(Mesh mesh, int subsetIndex, ref Matrix4x4 matrix);

		// Token: 0x06000A2D RID: 2605
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void Internal_RenderMesh_Injected(ref RenderParams rparams, Mesh mesh, int submeshIndex, ref Matrix4x4 objectToWorld, Matrix4x4* prevObjectToWorld);

		// Token: 0x06000A2E RID: 2606
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderMeshInstanced_Injected(ref RenderParams rparams, Mesh mesh, int submeshIndex, IntPtr instanceData, ref RenderInstancedDataLayout layout, uint instanceCount);

		// Token: 0x06000A2F RID: 2607
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderMeshIndirect_Injected(ref RenderParams rparams, Mesh mesh, GraphicsBuffer commandBuffer, int commandCount, int startCommand);

		// Token: 0x06000A30 RID: 2608
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderMeshPrimitives_Injected(ref RenderParams rparams, Mesh mesh, int submeshIndex, int instanceCount);

		// Token: 0x06000A31 RID: 2609
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderPrimitives_Injected(ref RenderParams rparams, MeshTopology topology, int vertexCount, int instanceCount);

		// Token: 0x06000A32 RID: 2610
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderPrimitivesIndexed_Injected(ref RenderParams rparams, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int startIndex, int instanceCount);

		// Token: 0x06000A33 RID: 2611
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderPrimitivesIndirect_Injected(ref RenderParams rparams, MeshTopology topology, GraphicsBuffer commandBuffer, int commandCount, int startCommand);

		// Token: 0x06000A34 RID: 2612
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RenderPrimitivesIndexedIndirect_Injected(ref RenderParams rparams, MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer commandBuffer, int commandCount, int startCommand);

		// Token: 0x06000A35 RID: 2613
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMesh_Injected(Mesh mesh, int submeshIndex, ref Matrix4x4 matrix, Material material, int layer, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

		// Token: 0x06000A36 RID: 2614
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshInstancedProcedural_Injected(Mesh mesh, int submeshIndex, Material material, ref Bounds bounds, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

		// Token: 0x06000A37 RID: 2615
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshInstancedIndirect_Injected(Mesh mesh, int submeshIndex, Material material, ref Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

		// Token: 0x06000A38 RID: 2616
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawMeshInstancedIndirectGraphicsBuffer_Injected(Mesh mesh, int submeshIndex, Material material, ref Bounds bounds, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

		// Token: 0x06000A39 RID: 2617
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProcedural_Injected(Material material, ref Bounds bounds, MeshTopology topology, int vertexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3A RID: 2618
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexed_Injected(Material material, ref Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3B RID: 2619
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndirect_Injected(Material material, ref Bounds bounds, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3C RID: 2620
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndirectGraphicsBuffer_Injected(Material material, ref Bounds bounds, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3D RID: 2621
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexedIndirect_Injected(Material material, ref Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3E RID: 2622
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_DrawProceduralIndexedIndirectGraphicsBuffer_Injected(Material material, ref Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, GraphicsBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

		// Token: 0x06000A3F RID: 2623
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Blit4_Injected(Texture source, RenderTexture dest, ref Vector2 scale, ref Vector2 offset);

		// Token: 0x06000A40 RID: 2624
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Blit5_Injected(Texture source, RenderTexture dest, ref Vector2 scale, ref Vector2 offset, int sourceDepthSlice, int destDepthSlice);

		// Token: 0x04000428 RID: 1064
		internal static readonly int kMaxDrawMeshInstanceCount = Graphics.Internal_GetMaxDrawMeshInstanceCount();

		// Token: 0x04000429 RID: 1065
		internal static Dictionary<int, RenderInstancedDataLayout> s_RenderInstancedDataLayouts = new Dictionary<int, RenderInstancedDataLayout>();
	}
}
