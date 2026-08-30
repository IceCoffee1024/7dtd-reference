using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x020002E3 RID: 739
	[NativeHeader("PlatformDependent/Win/Webcam/PhotoCaptureFrame.h")]
	[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	public sealed class PhotoCaptureFrame : IDisposable
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x00032CF3 File Offset: 0x00030EF3
		// (set) Token: 0x06001EF6 RID: 7926 RVA: 0x00032CFB File Offset: 0x00030EFB
		public int dataLength { get; private set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x00032D04 File Offset: 0x00030F04
		// (set) Token: 0x06001EF8 RID: 7928 RVA: 0x00032D0C File Offset: 0x00030F0C
		public bool hasLocationData { get; private set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x00032D15 File Offset: 0x00030F15
		// (set) Token: 0x06001EFA RID: 7930 RVA: 0x00032D1D File Offset: 0x00030F1D
		public CapturePixelFormat pixelFormat { get; private set; }

		// Token: 0x06001EFB RID: 7931
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetDataLength();

		// Token: 0x06001EFC RID: 7932
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetHasLocationData();

		// Token: 0x06001EFD RID: 7933
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern CapturePixelFormat GetCapturePixelFormat();

		// Token: 0x06001EFE RID: 7934 RVA: 0x00032D28 File Offset: 0x00030F28
		public bool TryGetCameraToWorldMatrix(out Matrix4x4 cameraToWorldMatrix)
		{
			cameraToWorldMatrix = Matrix4x4.identity;
			bool hasLocationData = this.hasLocationData;
			bool flag;
			if (hasLocationData)
			{
				cameraToWorldMatrix = this.GetCameraToWorldMatrix();
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x00032D64 File Offset: 0x00030F64
		[NativeConditional("PLATFORM_WIN && !PLATFORM_XBOXONE", "Matrix4x4f()")]
		[ThreadAndSerializationSafe]
		[NativeName("GetCameraToWorld")]
		private Matrix4x4 GetCameraToWorldMatrix()
		{
			Matrix4x4 matrix4x;
			this.GetCameraToWorldMatrix_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x00032D7C File Offset: 0x00030F7C
		public bool TryGetProjectionMatrix(out Matrix4x4 projectionMatrix)
		{
			bool hasLocationData = this.hasLocationData;
			bool flag;
			if (hasLocationData)
			{
				projectionMatrix = this.GetProjection();
				flag = true;
			}
			else
			{
				projectionMatrix = Matrix4x4.identity;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x00032DB8 File Offset: 0x00030FB8
		public bool TryGetProjectionMatrix(float nearClipPlane, float farClipPlane, out Matrix4x4 projectionMatrix)
		{
			bool hasLocationData = this.hasLocationData;
			bool flag3;
			if (hasLocationData)
			{
				float num = 0.01f;
				bool flag = nearClipPlane < num;
				if (flag)
				{
					nearClipPlane = num;
				}
				bool flag2 = farClipPlane < nearClipPlane + num;
				if (flag2)
				{
					farClipPlane = nearClipPlane + num;
				}
				projectionMatrix = this.GetProjection();
				float num2 = 1f / (farClipPlane - nearClipPlane);
				float num3 = -(farClipPlane + nearClipPlane) * num2;
				float num4 = -(2f * farClipPlane * nearClipPlane) * num2;
				projectionMatrix.m22 = num3;
				projectionMatrix.m23 = num4;
				flag3 = true;
			}
			else
			{
				projectionMatrix = Matrix4x4.identity;
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x00032E4C File Offset: 0x0003104C
		[NativeConditional("PLATFORM_WIN && !PLATFORM_XBOXONE", "Matrix4x4f()")]
		[ThreadAndSerializationSafe]
		private Matrix4x4 GetProjection()
		{
			Matrix4x4 matrix4x;
			this.GetProjection_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00032E64 File Offset: 0x00031064
		public void UploadImageDataToTexture(Texture2D targetTexture)
		{
			bool flag = targetTexture == null;
			if (flag)
			{
				throw new ArgumentNullException("targetTexture");
			}
			bool flag2 = this.pixelFormat > CapturePixelFormat.BGRA32;
			if (flag2)
			{
				throw new ArgumentException("Uploading PhotoCaptureFrame to a texture is only supported with BGRA32 CameraFrameFormat!");
			}
			this.UploadImageDataToTexture_Internal(targetTexture);
		}

		// Token: 0x06001F04 RID: 7940
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("UploadImageDataToTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UploadImageDataToTexture_Internal(Texture2D targetTexture);

		// Token: 0x06001F05 RID: 7941
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetUnsafePointerToBuffer();

		// Token: 0x06001F06 RID: 7942 RVA: 0x00032EAC File Offset: 0x000310AC
		public void CopyRawImageDataIntoBuffer(List<byte> byteBuffer)
		{
			bool flag = byteBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("byteBuffer");
			}
			byte[] array = new byte[this.dataLength];
			this.CopyRawImageDataIntoBuffer_Internal(array);
			bool flag2 = byteBuffer.Capacity < array.Length;
			if (flag2)
			{
				byteBuffer.Capacity = array.Length;
			}
			byteBuffer.Clear();
			byteBuffer.AddRange(array);
		}

		// Token: 0x06001F07 RID: 7943
		[NativeName("CopyRawImageDataIntoBuffer")]
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void CopyRawImageDataIntoBuffer_Internal([Out] byte[] byteArray);

		// Token: 0x06001F08 RID: 7944 RVA: 0x00032F0C File Offset: 0x0003110C
		internal PhotoCaptureFrame(IntPtr nativePtr)
		{
			this.m_NativePtr = nativePtr;
			this.dataLength = this.GetDataLength();
			this.hasLocationData = this.GetHasLocationData();
			this.pixelFormat = this.GetCapturePixelFormat();
			GC.AddMemoryPressure((long)this.dataLength);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00032F5C File Offset: 0x0003115C
		private void Cleanup()
		{
			bool flag = this.m_NativePtr != IntPtr.Zero;
			if (flag)
			{
				GC.RemoveMemoryPressure((long)this.dataLength);
				this.Dispose_Internal();
				this.m_NativePtr = IntPtr.Zero;
			}
		}

		// Token: 0x06001F0A RID: 7946
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("Dispose")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Dispose_Internal();

		// Token: 0x06001F0B RID: 7947 RVA: 0x00032F9F File Offset: 0x0003119F
		public void Dispose()
		{
			this.Cleanup();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00032FB0 File Offset: 0x000311B0
		~PhotoCaptureFrame()
		{
			this.Cleanup();
		}

		// Token: 0x06001F0D RID: 7949
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetCameraToWorldMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06001F0E RID: 7950
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetProjection_Injected(out Matrix4x4 ret);

		// Token: 0x04000A3A RID: 2618
		private IntPtr m_NativePtr;
	}
}
