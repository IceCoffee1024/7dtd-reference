using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine.U2D
{
	// Token: 0x020002B5 RID: 693
	[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
	[NativeHeader("Runtime/Graphics/SpriteFrame.h")]
	public static class SpriteDataAccessExtensions
	{
		// Token: 0x06001D7F RID: 7551 RVA: 0x00030980 File Offset: 0x0002EB80
		private static void CheckAttributeTypeMatchesAndThrow<T>(VertexAttribute channel)
		{
			bool flag;
			switch (channel)
			{
			case VertexAttribute.Position:
			case VertexAttribute.Normal:
				flag = typeof(T) == typeof(Vector3);
				break;
			case VertexAttribute.Tangent:
				flag = typeof(T) == typeof(Vector4);
				break;
			case VertexAttribute.Color:
				flag = typeof(T) == typeof(Color32);
				break;
			case VertexAttribute.TexCoord0:
			case VertexAttribute.TexCoord1:
			case VertexAttribute.TexCoord2:
			case VertexAttribute.TexCoord3:
			case VertexAttribute.TexCoord4:
			case VertexAttribute.TexCoord5:
			case VertexAttribute.TexCoord6:
			case VertexAttribute.TexCoord7:
				flag = typeof(T) == typeof(Vector2);
				break;
			case VertexAttribute.BlendWeight:
				flag = typeof(T) == typeof(BoneWeight);
				break;
			default:
				throw new InvalidOperationException(string.Format("The requested channel '{0}' is unknown.", channel));
			}
			bool flag2 = !flag;
			if (flag2)
			{
				throw new InvalidOperationException(string.Format("The requested channel '{0}' does not match the return type {1}.", channel, typeof(T).Name));
			}
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x00030AA8 File Offset: 0x0002ECA8
		public unsafe static NativeSlice<T> GetVertexAttribute<T>(this Sprite sprite, VertexAttribute channel) where T : struct
		{
			SpriteDataAccessExtensions.CheckAttributeTypeMatchesAndThrow<T>(channel);
			SpriteChannelInfo channelInfo = SpriteDataAccessExtensions.GetChannelInfo(sprite, channel);
			byte* ptr = (byte*)channelInfo.buffer + channelInfo.offset;
			return NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<T>((void*)ptr, channelInfo.stride, channelInfo.count);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x00030AEF File Offset: 0x0002ECEF
		public static void SetVertexAttribute<T>(this Sprite sprite, VertexAttribute channel, NativeArray<T> src) where T : struct
		{
			SpriteDataAccessExtensions.CheckAttributeTypeMatchesAndThrow<T>(channel);
			SpriteDataAccessExtensions.SetChannelData(sprite, channel, src.GetUnsafeReadOnlyPtr<T>());
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x00030B08 File Offset: 0x0002ED08
		public static NativeArray<Matrix4x4> GetBindPoses(this Sprite sprite)
		{
			SpriteChannelInfo bindPoseInfo = SpriteDataAccessExtensions.GetBindPoseInfo(sprite);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(bindPoseInfo.buffer, bindPoseInfo.count, Allocator.None);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00030B37 File Offset: 0x0002ED37
		public static void SetBindPoses(this Sprite sprite, NativeArray<Matrix4x4> src)
		{
			SpriteDataAccessExtensions.SetBindPoseData(sprite, src.GetUnsafeReadOnlyPtr<Matrix4x4>(), src.Length);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x00030B50 File Offset: 0x0002ED50
		public static NativeArray<ushort> GetIndices(this Sprite sprite)
		{
			SpriteChannelInfo indicesInfo = SpriteDataAccessExtensions.GetIndicesInfo(sprite);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ushort>(indicesInfo.buffer, indicesInfo.count, Allocator.Invalid);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x00030B7F File Offset: 0x0002ED7F
		public static void SetIndices(this Sprite sprite, NativeArray<ushort> src)
		{
			SpriteDataAccessExtensions.SetIndicesData(sprite, src.GetUnsafeReadOnlyPtr<ushort>(), src.Length);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00030B98 File Offset: 0x0002ED98
		public static SpriteBone[] GetBones(this Sprite sprite)
		{
			return SpriteDataAccessExtensions.GetBoneInfo(sprite);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00030BB0 File Offset: 0x0002EDB0
		public static void SetBones(this Sprite sprite, SpriteBone[] src)
		{
			SpriteDataAccessExtensions.SetBoneData(sprite, src);
		}

		// Token: 0x06001D88 RID: 7560
		[NativeName("HasChannel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasVertexAttribute([NotNull("ArgumentNullException")] this Sprite sprite, VertexAttribute channel);

		// Token: 0x06001D89 RID: 7561
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetVertexCount([NotNull("ArgumentNullException")] this Sprite sprite, int count);

		// Token: 0x06001D8A RID: 7562
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetVertexCount([NotNull("ArgumentNullException")] this Sprite sprite);

		// Token: 0x06001D8B RID: 7563 RVA: 0x00030BBC File Offset: 0x0002EDBC
		private static SpriteChannelInfo GetBindPoseInfo([NotNull("ArgumentNullException")] Sprite sprite)
		{
			SpriteChannelInfo spriteChannelInfo;
			SpriteDataAccessExtensions.GetBindPoseInfo_Injected(sprite, out spriteChannelInfo);
			return spriteChannelInfo;
		}

		// Token: 0x06001D8C RID: 7564
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetBindPoseData([NotNull("ArgumentNullException")] Sprite sprite, void* src, int count);

		// Token: 0x06001D8D RID: 7565 RVA: 0x00030BD4 File Offset: 0x0002EDD4
		private static SpriteChannelInfo GetIndicesInfo([NotNull("ArgumentNullException")] Sprite sprite)
		{
			SpriteChannelInfo spriteChannelInfo;
			SpriteDataAccessExtensions.GetIndicesInfo_Injected(sprite, out spriteChannelInfo);
			return spriteChannelInfo;
		}

		// Token: 0x06001D8E RID: 7566
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetIndicesData([NotNull("ArgumentNullException")] Sprite sprite, void* src, int count);

		// Token: 0x06001D8F RID: 7567 RVA: 0x00030BEC File Offset: 0x0002EDEC
		private static SpriteChannelInfo GetChannelInfo([NotNull("ArgumentNullException")] Sprite sprite, VertexAttribute channel)
		{
			SpriteChannelInfo spriteChannelInfo;
			SpriteDataAccessExtensions.GetChannelInfo_Injected(sprite, channel, out spriteChannelInfo);
			return spriteChannelInfo;
		}

		// Token: 0x06001D90 RID: 7568
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetChannelData([NotNull("ArgumentNullException")] Sprite sprite, VertexAttribute channel, void* src);

		// Token: 0x06001D91 RID: 7569
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SpriteBone[] GetBoneInfo([NotNull("ArgumentNullException")] Sprite sprite);

		// Token: 0x06001D92 RID: 7570
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBoneData([NotNull("ArgumentNullException")] Sprite sprite, SpriteBone[] src);

		// Token: 0x06001D93 RID: 7571
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPrimaryVertexStreamSize(Sprite sprite);

		// Token: 0x06001D94 RID: 7572
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBindPoseInfo_Injected(Sprite sprite, out SpriteChannelInfo ret);

		// Token: 0x06001D95 RID: 7573
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetIndicesInfo_Injected(Sprite sprite, out SpriteChannelInfo ret);

		// Token: 0x06001D96 RID: 7574
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetChannelInfo_Injected(Sprite sprite, VertexAttribute channel, out SpriteChannelInfo ret);
	}
}
