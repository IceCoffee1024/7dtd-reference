using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B55 RID: 2901
public static class MeshDataUtils
{
	// Token: 0x060057D6 RID: 22486 RVA: 0x002193BC File Offset: 0x002175BC
	public static void CopyInterleaved(int elementSize, int offset, int stride, ReadOnlySpan<byte> src, Span<byte> dest)
	{
		int num = 0;
		int num2 = offset;
		while (num + elementSize <= src.Length && num2 + stride - offset <= dest.Length)
		{
			src.Slice(num, elementSize).CopyTo(dest.Slice(num2, elementSize));
			num += elementSize;
			num2 += stride;
		}
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x0021940C File Offset: 0x0021760C
	public static MeshDataLayout SetAttributes(Mesh.MeshData meshData, ReadOnlySpan<Vector3> vertices, ReadOnlySpan<int> indices, ReadOnlySpan<Vector3> normals, ReadOnlySpan<Vector4> tangents, ReadOnlySpan<Color> colors, ReadOnlySpan<Vector2> texCoord0s, ReadOnlySpan<Vector2> texCoord1s, NativeList<VertexAttributeDescriptor> vertexAttributesOut)
	{
		int num = 0;
		MeshDataLayout meshDataLayout;
		meshDataLayout.PositionSize = 12;
		meshDataLayout.PositionOffset = num;
		num += 12;
		VertexAttributeDescriptor vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0);
		vertexAttributesOut.Add(in vertexAttributeDescriptor);
		if (normals.Length > 0)
		{
			meshDataLayout.NormalSize = 12;
			meshDataLayout.NormalOffset = num;
			num += 12;
			vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0);
			vertexAttributesOut.Add(in vertexAttributeDescriptor);
		}
		else
		{
			meshDataLayout.NormalSize = -1;
			meshDataLayout.NormalOffset = -1;
		}
		if (tangents.Length > 0)
		{
			meshDataLayout.TangentSize = 16;
			meshDataLayout.TangentOffset = num;
			num += 16;
			vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4, 0);
			vertexAttributesOut.Add(in vertexAttributeDescriptor);
		}
		else
		{
			meshDataLayout.TangentSize = -1;
			meshDataLayout.TangentOffset = -1;
		}
		if (colors.Length > 0)
		{
			meshDataLayout.ColorSize = 16;
			meshDataLayout.ColorOffset = num;
			num += 16;
			vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4, 0);
			vertexAttributesOut.Add(in vertexAttributeDescriptor);
		}
		else
		{
			meshDataLayout.ColorSize = -1;
			meshDataLayout.ColorOffset = -1;
		}
		if (texCoord0s.Length > 0)
		{
			meshDataLayout.TexCoord0Size = 8;
			meshDataLayout.TexCoord0Offset = num;
			num += 8;
			vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0);
			vertexAttributesOut.Add(in vertexAttributeDescriptor);
		}
		else
		{
			meshDataLayout.TexCoord0Size = -1;
			meshDataLayout.TexCoord0Offset = -1;
		}
		if (texCoord1s.Length > 0)
		{
			meshDataLayout.TexCoord1Size = 8;
			meshDataLayout.TexCoord1Offset = num;
			num += 8;
			vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2, 0);
			vertexAttributesOut.Add(in vertexAttributeDescriptor);
		}
		else
		{
			meshDataLayout.TexCoord1Size = -1;
			meshDataLayout.TexCoord1Offset = -1;
		}
		meshData.SetVertexBufferParams(vertices.Length, vertexAttributesOut.AsArray());
		meshDataLayout.Stride = num;
		meshData.SetIndexBufferParams(indices.Length, IndexFormat.UInt32);
		return meshDataLayout;
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x002195C8 File Offset: 0x002177C8
	public static void SetSubmesh(Mesh.MeshData meshData, int vertexCount, int indexCount, Bounds bounds)
	{
		meshData.subMeshCount = 1;
		meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles)
		{
			bounds = bounds,
			vertexCount = vertexCount
		}, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x00219604 File Offset: 0x00217804
	public unsafe static Bounds RecalculateBounds(ReadOnlySpan<Vector3> vertices)
	{
		Bounds bounds = new Bounds(*vertices[0], Vector3.zero);
		for (int i = 1; i < vertices.Length; i++)
		{
			bounds.Encapsulate(*vertices[i]);
		}
		return bounds;
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x00219651 File Offset: 0x00217851
	public static void ApplyMeshDataCompression(Mesh.MeshData meshData, NativeArray<VertexAttributeDescriptor> vertexAttributes)
	{
		GameUtils.ApplyCompressedVertexAttributes(in vertexAttributes, false);
		meshData.SetVertexBufferParams(meshData.vertexCount, vertexAttributes);
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x00219670 File Offset: 0x00217870
	public unsafe static void CalculateNormals(ReadOnlySpan<Vector3> vertices, ReadOnlySpan<int> indices, Span<Vector3> normals)
	{
		int num = Math.Min(vertices.Length, normals.Length);
		for (int i = 0; i < normals.Length; i++)
		{
			*normals[i] = Vector3.zero;
		}
		Allocator allocator = (ThreadManager.IsMainThread() ? Allocator.Temp : Allocator.Persistent);
		using (NativeParallelMultiHashMap<Vector3, int> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<Vector3, int>(num, allocator))
		{
			for (int j = 0; j < num; j++)
			{
				nativeParallelMultiHashMap.Add(*vertices[j], j);
			}
			using (NativeArray<int> nativeArray = new NativeArray<int>(num, allocator, NativeArrayOptions.ClearMemory))
			{
				int num2 = 0;
				while (num2 + 2 < indices.Length)
				{
					int num3 = *indices[num2];
					int num4 = *indices[num2 + 1];
					int num5 = *indices[num2 + 2];
					Vector3 vector = *vertices[num3];
					Vector3 vector2 = *vertices[num4];
					Vector3 vector3 = *vertices[num5];
					Vector3 vector4 = Vector3.Cross(vector2 - vector, vector3 - vector);
					vector4.Normalize();
					MeshDataUtils.AddTriangleNormal(ref normals, nativeParallelMultiHashMap, nativeArray, vector, vector4);
					MeshDataUtils.AddTriangleNormal(ref normals, nativeParallelMultiHashMap, nativeArray, vector2, vector4);
					MeshDataUtils.AddTriangleNormal(ref normals, nativeParallelMultiHashMap, nativeArray, vector3, vector4);
					num2 += 3;
				}
				for (int k = 0; k < normals.Length; k++)
				{
					int num6 = nativeArray[k];
					if (num6 > 0)
					{
						*normals[k] /= (float)num6;
					}
				}
			}
		}
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x00219858 File Offset: 0x00217A58
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void AddTriangleNormal(ref Span<Vector3> normals, NativeParallelMultiHashMap<Vector3, int> vertexToIndices, NativeArray<int> normalsAdded, Vector3 vertex, Vector3 normal)
	{
		foreach (int num in vertexToIndices.GetValuesForKey(vertex))
		{
			*normals[num] += normal;
			int num2 = num;
			int num3 = normalsAdded[num2];
			normalsAdded[num2] = num3 + 1;
		}
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x002198DC File Offset: 0x00217ADC
	public unsafe static AtomicSafeHandleScope CreateNativeArray<[IsUnmanaged] T>(T* ptr, int length, out NativeArray<T> array) where T : struct, ValueType
	{
		array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)ptr, length, Allocator.None);
		return default(AtomicSafeHandleScope);
	}

	// Token: 0x040043D5 RID: 17365
	public const MeshUpdateFlags DoNothing = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;
}
