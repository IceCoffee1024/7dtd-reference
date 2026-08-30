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

// Token: 0x02001514 RID: 5396
public static class ProfilerUtils
{
	// Token: 0x0600A93E RID: 43326 RVA: 0x003F2550 File Offset: 0x003F0750
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

	// Token: 0x0600A93F RID: 43327 RVA: 0x003F2634 File Offset: 0x003F0834
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

	// Token: 0x0600A940 RID: 43328 RVA: 0x003F26F3 File Offset: 0x003F08F3
	public static bool AreMipMapsStreamed(this Texture2D texture)
	{
		return texture.streamingMipmaps && !texture.isReadable;
	}

	// Token: 0x0600A941 RID: 43329 RVA: 0x003F2708 File Offset: 0x003F0908
	public static bool AreMipMapsStreamed(this Cubemap texture)
	{
		return texture.streamingMipmaps && !texture.isReadable;
	}

	// Token: 0x0600A942 RID: 43330 RVA: 0x003F2720 File Offset: 0x003F0920
	public static int CalculateTextureSizeBytes(Texture2D texture, int mipLevel = 0)
	{
		int num = ProfilerUtils.CalculateTextureSizeBytes(texture.format, texture.width, texture.height, texture.mipmapCount, mipLevel);
		if (num < 0)
		{
			Debug.LogErrorFormat("Calculating size for {0}: unsupported texture type {1}", new object[] { texture.name, texture.format });
		}
		return num;
	}

	// Token: 0x0600A943 RID: 43331 RVA: 0x003F2778 File Offset: 0x003F0978
	public static int CalculateTextureSizeBytes(Cubemap texture, int mipLevel = 0)
	{
		int num = ProfilerUtils.CalculateTextureSizeBytes(texture.format, texture.width, texture.height, texture.mipmapCount, mipLevel);
		if (num < 0)
		{
			Debug.LogErrorFormat("Calculating size for {0}: unsupported texture type {1}", new object[] { texture.name, texture.format });
		}
		return num * 6;
	}

	// Token: 0x0600A944 RID: 43332 RVA: 0x003F27D4 File Offset: 0x003F09D4
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

	// Token: 0x0600A945 RID: 43333 RVA: 0x003F2820 File Offset: 0x003F0A20
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

	// Token: 0x0600A946 RID: 43334 RVA: 0x003F28CD File Offset: 0x003F0ACD
	public static int CalculateUnsafeParallelHashMapBytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(UnsafeParallelHashMap<TKey, TValue> hashMap) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, TValue>(hashMap.Capacity);
	}

	// Token: 0x0600A947 RID: 43335 RVA: 0x003F28DB File Offset: 0x003F0ADB
	public static int CalculateUnsafeParallelHashSetBytes<[IsUnmanaged] TKey>(UnsafeParallelHashSet<TKey> hashSet) where TKey : struct, ValueType, IEquatable<TKey>
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, bool>(hashSet.Capacity);
	}

	// Token: 0x0600A948 RID: 43336 RVA: 0x003F28E9 File Offset: 0x003F0AE9
	public static int CalculateUnsafeParallelMultiHashMapBytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(UnsafeParallelMultiHashMap<TKey, TValue> hashMap) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		return ProfilerUtils.CalculateUnsafeParallelHashMapBytes<TKey, TValue>(hashMap.Capacity);
	}

	// Token: 0x0600A949 RID: 43337 RVA: 0x003F28F8 File Offset: 0x003F0AF8
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

	// Token: 0x0600A94A RID: 43338 RVA: 0x003F295E File Offset: 0x003F0B5E
	public static int CalculateUnsafeBitArrayBytes(UnsafeBitArray bitArray)
	{
		return ProfilerUtils.AlignUp(bitArray.Length, 64) / 8;
	}

	// Token: 0x0600A94B RID: 43339 RVA: 0x003F296F File Offset: 0x003F0B6F
	public static int AlignDown(int value, int alignPow2)
	{
		return value & ~(alignPow2 - 1);
	}

	// Token: 0x0600A94C RID: 43340 RVA: 0x003F2977 File Offset: 0x003F0B77
	public static int AlignUp(int value, int alignPow2)
	{
		return ProfilerUtils.AlignDown(value + alignPow2 - 1, alignPow2);
	}

	// Token: 0x0600A94D RID: 43341 RVA: 0x00010E62 File Offset: 0x0000F062
	public static int CalculateMeshBytes(Mesh mesh)
	{
		return 0;
	}

	// Token: 0x0600A94E RID: 43342 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CollectDependencies(GameObject[] roots, List<Texture2D> textures = null, List<Mesh> meshes = null)
	{
	}

	// Token: 0x0600A94F RID: 43343 RVA: 0x003F2984 File Offset: 0x003F0B84
	public static void CalculateDependentBytes(GameObject[] roots, out long meshBytes, out long textureBytes)
	{
		meshBytes = 0L;
		textureBytes = 0L;
	}
}
