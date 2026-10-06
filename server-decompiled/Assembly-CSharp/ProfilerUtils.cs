using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x02001551 RID: 5457
public static class ProfilerUtils
{
	// Token: 0x0600AAEE RID: 43758 RVA: 0x003FC6E4 File Offset: 0x003FA8E4
	public static string GetAvailableMetricsCsv()
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<ProfilerRecorderHandle> list = new List<ProfilerRecorderHandle>();
		ProfilerRecorderHandle.GetAvailable(list);
		stringBuilder.AppendLine("Name,Category,DataType,UnitType");
		foreach (ProfilerRecorderHandle profilerRecorderHandle in list)
		{
			ProfilerRecorderDescription description = ProfilerRecorderHandle.GetDescription(profilerRecorderHandle);
			stringBuilder.AppendFormat("{0},{1},{2},{3}", new object[]
			{
				description.Name,
				description.Category.Name,
				Enum.GetName(typeof(ProfilerMarkerDataType), description.DataType),
				Enum.GetName(typeof(ProfilerMarkerDataUnit), description.UnitType)
			});
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600AAEF RID: 43759 RVA: 0x003FC7C8 File Offset: 0x003FA9C8
	public static void AppendLastValue(ProfilerRecorder recorder, StringBuilder builder)
	{
		switch (recorder.UnitType)
		{
		case ProfilerMarkerDataUnit.Undefined:
			builder.Append(recorder.LastValue);
			return;
		case ProfilerMarkerDataUnit.TimeNanoseconds:
			builder.AppendFormat("{0:F2}", recorder.LastValueAsDouble * 1E-06);
			return;
		case ProfilerMarkerDataUnit.Bytes:
			builder.AppendFormat("{0:F2}", recorder.LastValueAsDouble * 9.5367431640625E-07);
			return;
		case ProfilerMarkerDataUnit.Count:
			builder.Append(recorder.LastValue);
			return;
		case ProfilerMarkerDataUnit.Percent:
			builder.AppendFormat("{0:F2}", recorder.LastValueAsDouble);
			return;
		case ProfilerMarkerDataUnit.FrequencyHz:
			builder.Append(recorder.LastValueAsDouble);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600AAF0 RID: 43760 RVA: 0x003FC887 File Offset: 0x003FAA87
	public static bool AreMipMapsStreamed(this Texture2D texture)
	{
		return texture.streamingMipmaps && !texture.isReadable;
	}

	// Token: 0x0600AAF1 RID: 43761 RVA: 0x003FC89C File Offset: 0x003FAA9C
	public static bool AreMipMapsStreamed(this Cubemap texture)
	{
		return texture.streamingMipmaps && !texture.isReadable;
	}

	// Token: 0x0600AAF2 RID: 43762 RVA: 0x003FC8B4 File Offset: 0x003FAAB4
	public static int CalculateTextureSizeBytes(Texture2D texture, int mipLevel = 0)
	{
		int num = ProfilerUtils.CalculateTextureSizeBytes(texture.format, texture.width, texture.height, texture.mipmapCount, mipLevel);
		if (num < 0)
		{
			Debug.LogErrorFormat("Calculating size for {0}: unsupported texture type {1}", new object[] { texture.name, texture.format });
		}
		return num;
	}

	// Token: 0x0600AAF3 RID: 43763 RVA: 0x003FC90C File Offset: 0x003FAB0C
	public static int CalculateTextureSizeBytes(Cubemap texture, int mipLevel = 0)
	{
		int num = ProfilerUtils.CalculateTextureSizeBytes(texture.format, texture.width, texture.height, texture.mipmapCount, mipLevel);
		if (num < 0)
		{
			Debug.LogErrorFormat("Calculating size for {0}: unsupported texture type {1}", new object[] { texture.name, texture.format });
		}
		return num * 6;
	}

	// Token: 0x0600AAF4 RID: 43764 RVA: 0x003FC968 File Offset: 0x003FAB68
	public static int CalculateTextureSizeBytes(TextureFormat format, int width, int height, int mipCount, int mipLevel = 0)
	{
		int num = 0;
		mipLevel = Math.Max(mipLevel, 0);
		mipLevel = Math.Min(mipLevel, mipCount - 1);
		for (int i = mipLevel; i < mipCount; i++)
		{
			int num2 = 1 << i;
			num += ProfilerUtils.CalculateTextureSizeBytes(format, width / num2, height / num2);
		}
		return num;
	}

	// Token: 0x0600AAF5 RID: 43765 RVA: 0x003FC9B4 File Offset: 0x003FABB4
	public static int CalculateTextureSizeBytes(TextureFormat format, int width, int height)
	{
		int num = width * height;
		switch (format)
		{
		case TextureFormat.Alpha8:
			return num;
		case TextureFormat.ARGB4444:
		case TextureFormat.RGBA4444:
			return num * 2;
		case TextureFormat.RGB24:
			return num * 3;
		case TextureFormat.RGBA32:
		case TextureFormat.ARGB32:
		case TextureFormat.BGRA32:
			return num * 4;
		case TextureFormat.R16:
			return num * 2;
		case TextureFormat.DXT1:
			return Mathf.CeilToInt(0.5f * (float)num);
		case TextureFormat.DXT5:
			return num;
		case TextureFormat.RGBAHalf:
			return num * 8;
		case TextureFormat.RGBAFloat:
			return num * 16;
		case TextureFormat.BC7:
			return num;
		}
		return -1;
	}

	// Token: 0x0600AAF6 RID: 43766 RVA: 0x003FCA61 File Offset: 0x003FAC61
	public static int CalculateUnsafeParallelHashMapBytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(UnsafeParallelHashMap<TKey, TValue> hashMap) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, TValue>(hashMap.Capacity);
	}

	// Token: 0x0600AAF7 RID: 43767 RVA: 0x003FCA6F File Offset: 0x003FAC6F
	public static int CalculateUnsafeParallelHashSetBytes<[IsUnmanaged] TKey>(UnsafeParallelHashSet<TKey> hashSet) where TKey : struct, ValueType, IEquatable<TKey>
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, bool>(hashSet.Capacity);
	}

	// Token: 0x0600AAF8 RID: 43768 RVA: 0x003FCA7D File Offset: 0x003FAC7D
	public static int CalculateUnsafeParallelMultiHashMapBytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(UnsafeParallelMultiHashMap<TKey, TValue> hashMap) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, TValue>(hashMap.Capacity);
	}

	// Token: 0x0600AAF9 RID: 43769 RVA: 0x003FCA8C File Offset: 0x003FAC8C
	public static int CalculateUnsafeParallelHashMapBytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(int length) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		int num = UnsafeUtility.SizeOf<UnsafeParallelHashMap<TKey, TValue>>() + 8256;
		int num2 = length * 2;
		int num3 = UnsafeUtility.SizeOf<TValue>();
		int num4 = UnsafeUtility.SizeOf<TKey>();
		int num5 = UnsafeUtility.SizeOf<int>();
		int num6 = CollectionHelper.Align(num3 * length, 64);
		int num7 = CollectionHelper.Align(num4 * length, 64);
		int num8 = CollectionHelper.Align(num5 * length, 64);
		int num9 = CollectionHelper.Align(num5 * num2, 64);
		return num + (num6 + num7 + num8 + num9);
	}

	// Token: 0x0600AAFA RID: 43770 RVA: 0x003FCAF2 File Offset: 0x003FACF2
	public static int CalculateUnsafeBitArrayBytes(UnsafeBitArray bitArray)
	{
		return ProfilerUtils.AlignUp(bitArray.Length, 64) / 8;
	}

	// Token: 0x0600AAFB RID: 43771 RVA: 0x003FCB03 File Offset: 0x003FAD03
	public static int AlignDown(int value, int alignPow2)
	{
		return value & ~(alignPow2 - 1);
	}

	// Token: 0x0600AAFC RID: 43772 RVA: 0x003FCB0B File Offset: 0x003FAD0B
	public static int AlignUp(int value, int alignPow2)
	{
		return ProfilerUtils.AlignDown(value + alignPow2 - 1, alignPow2);
	}

	// Token: 0x0600AAFD RID: 43773 RVA: 0x00010E62 File Offset: 0x0000F062
	public static int CalculateMeshBytes(Mesh mesh)
	{
		return 0;
	}

	// Token: 0x0600AAFE RID: 43774 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CollectDependencies(GameObject[] roots, List<Texture2D> textures = null, List<Mesh> meshes = null)
	{
	}

	// Token: 0x0600AAFF RID: 43775 RVA: 0x003FCB18 File Offset: 0x003FAD18
	public static void CalculateDependentBytes(GameObject[] roots, out long meshBytes, out long textureBytes)
	{
		meshBytes = 0L;
		textureBytes = 0L;
	}
}
