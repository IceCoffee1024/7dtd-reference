using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B67 RID: 2919
public static class MeshDataUtils
{
	// Token: 0x0600585E RID: 22622 RVA: 0x0021C0D8 File Offset: 0x0021A2D8
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

	// Token: 0x0600585F RID: 22623 RVA: 0x0021C128 File Offset: 0x0021A328
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

	// Token: 0x06005860 RID: 22624 RVA: 0x0021C2E4 File Offset: 0x0021A4E4
	public static void SetSubmesh(Mesh.MeshData meshData, int vertexCount, int indexCount, Bounds bounds)
	{
		meshData.subMeshCount = 1;
		meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles)
		{
			bounds = bounds,
			vertexCount = vertexCount
		}, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x0021C320 File Offset: 0x0021A520
	public unsafe static Bounds RecalculateBounds(ReadOnlySpan<Vector3> vertices)
	{
		Bounds bounds = new Bounds(*vertices[0], Vector3.zero);
		for (int i = 1; i < vertices.Length; i++)
		{
			bounds.Encapsulate(*vertices[i]);
		}
		return bounds;
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x0021C36D File Offset: 0x0021A56D
	public static void ApplyMeshDataCompression(Mesh.MeshData meshData, NativeArray<VertexAttributeDescriptor> vertexAttributes)
	{
		GameUtils.ApplyCompressedVertexAttributes(in vertexAttributes, false);
		meshData.SetVertexBufferParams(meshData.vertexCount, vertexAttributes);
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x0021C38C File Offset: 0x0021A58C
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

	// Token: 0x06005864 RID: 22628 RVA: 0x0021C574 File Offset: 0x0021A774
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

	// Token: 0x06005865 RID: 22629 RVA: 0x0021C5F8 File Offset: 0x0021A7F8
	public unsafe static AtomicSafeHandleScope CreateNativeArray<[IsUnmanaged] T>(T* ptr, int length, out NativeArray<T> array) where T : struct, ValueType
	{
		array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)ptr, length, Allocator.None);
		return default(AtomicSafeHandleScope);
	}

	// Token: 0x04004470 RID: 17520
	public const MeshUpdateFlags DoNothing = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;
}
