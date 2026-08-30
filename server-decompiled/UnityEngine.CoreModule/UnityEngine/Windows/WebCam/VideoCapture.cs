using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x020002E4 RID: 740
	[StaticAccessor("VideoCaptureBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("PlatformDependent/Win/Webcam/VideoCaptureBindings.h")]
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	[StructLayout(LayoutKind.Sequential)]
	public class VideoCapture : IDisposable
	{
		// Token: 0x06001F0F RID: 7951 RVA: 0x00032FE0 File Offset: 0x000311E0
		private static VideoCapture.VideoCaptureResult MakeCaptureResult(VideoCapture.CaptureResultType resultType, long hResult)
		{
			return new VideoCapture.VideoCaptureResult
			{
				resultType = resultType,
				hResult = hResult
			};
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0003300C File Offset: 0x0003120C
		private static VideoCapture.VideoCaptureResult MakeCaptureResult(long hResult)
		{
			VideoCapture.VideoCaptureResult videoCaptureResult = default(VideoCapture.VideoCaptureResult);
			bool flag = hResult == VideoCapture.HR_SUCCESS;
			VideoCapture.CaptureResultType captureResultType;
			if (flag)
			{
				captureResultType = VideoCapture.CaptureResultType.Success;
			}
			else
			{
				captureResultType = VideoCapture.CaptureResultType.UnknownError;
			}
			videoCaptureResult.resultType = captureResultType;
			videoCaptureResult.hResult = hResult;
			return videoCaptureResult;
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x00033050 File Offset: 0x00031250
		public static IEnumerable<Resolution> SupportedResolutions
		{
			get
			{
				bool flag = VideoCapture.s_SupportedResolutions == null;
				if (flag)
				{
					VideoCapture.s_SupportedResolutions = VideoCapture.GetSupportedResolutions_Internal();
				}
				return VideoCapture.s_SupportedResolutions;
			}
		}

		// Token: 0x06001F12 RID: 7954
		[NativeName("GetSupportedResolutions")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Resolution[] GetSupportedResolutions_Internal();

		// Token: 0x06001F13 RID: 7955 RVA: 0x00033080 File Offset: 0x00031280
		public static IEnumerable<float> GetSupportedFrameRatesForResolution(Resolution resolution)
		{
			return VideoCapture.GetSupportedFrameRatesForResolution_Internal(resolution.width, resolution.height);
		}

		// Token: 0x06001F14 RID: 7956
		[NativeName("GetSupportedFrameRatesForResolution")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float[] GetSupportedFrameRatesForResolution_Internal(int resolutionWidth, int resolutionHeight);

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F15 RID: 7957
		public extern bool IsRecording
		{
			[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
			[NativeMethod("VideoCaptureBindings::IsRecording", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000330AC File Offset: 0x000312AC
		public static void CreateAsync(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			VideoCapture.Instantiate_Internal(showHolograms, onCreatedCallback);
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000330D8 File Offset: 0x000312D8
		public static void CreateAsync(VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			VideoCapture.Instantiate_Internal(false, onCreatedCallback);
		}

		// Token: 0x06001F18 RID: 7960
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("Instantiate")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Instantiate_Internal(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback);

		// Token: 0x06001F19 RID: 7961 RVA: 0x00033104 File Offset: 0x00031304
		[RequiredByNativeCode]
		private static void InvokeOnCreatedVideoCaptureResourceDelegate(VideoCapture.OnVideoCaptureResourceCreatedCallback callback, IntPtr nativePtr)
		{
			bool flag = nativePtr == IntPtr.Zero;
			if (flag)
			{
				callback(null);
			}
			else
			{
				callback(new VideoCapture(nativePtr));
			}
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0003313C File Offset: 0x0003133C
		private VideoCapture(IntPtr nativeCaptureObject)
		{
			this.m_NativePtr = nativeCaptureObject;
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x00033150 File Offset: 0x00031350
		public void StartVideoModeAsync(CameraParameters setupParams, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback)
		{
			bool flag = onVideoModeStartedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onVideoModeStartedCallback");
			}
			bool flag2 = setupParams.cameraResolutionWidth == 0 || setupParams.cameraResolutionHeight == 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("setupParams", "The camera resolution must be set to a supported resolution.");
			}
			bool flag3 = setupParams.frameRate == 0f;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("setupParams", "The camera frame rate must be set to a supported recording frame rate.");
			}
			this.StartVideoMode_Internal(setupParams, audioState, onVideoModeStartedCallback);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000331CA File Offset: 0x000313CA
		[NativeMethod("VideoCaptureBindings::StartVideoMode", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		private void StartVideoMode_Internal(CameraParameters cameraParameters, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback)
		{
			this.StartVideoMode_Internal_Injected(ref cameraParameters, audioState, onVideoModeStartedCallback);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000331D6 File Offset: 0x000313D6
		[RequiredByNativeCode]
		private static void InvokeOnVideoModeStartedDelegate(VideoCapture.OnVideoModeStartedCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001F1E RID: 7966
		[NativeMethod("VideoCaptureBindings::StopVideoMode", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopVideoModeAsync([NotNull("ArgumentNullException")] VideoCapture.OnVideoModeStoppedCallback onVideoModeStoppedCallback);

		// Token: 0x06001F1F RID: 7967 RVA: 0x000331E6 File Offset: 0x000313E6
		[RequiredByNativeCode]
		private static void InvokeOnVideoModeStoppedDelegate(VideoCapture.OnVideoModeStoppedCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000331F8 File Offset: 0x000313F8
		public void StartRecordingAsync(string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback)
		{
			bool flag = onStartedRecordingVideoCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onStartedRecordingVideoCallback");
			}
			bool flag2 = string.IsNullOrEmpty(filename);
			if (flag2)
			{
				throw new ArgumentNullException("filename");
			}
			string directoryName = Path.GetDirectoryName(filename);
			bool flag3 = !string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName);
			if (flag3)
			{
				throw new ArgumentException("The specified directory does not exist.", "filename");
			}
			FileInfo fileInfo = new FileInfo(filename);
			bool flag4 = fileInfo.Exists && fileInfo.IsReadOnly;
			if (flag4)
			{
				throw new ArgumentException("Cannot write to the file because it is read-only.", "filename");
			}
			this.StartRecordingVideoToDisk_Internal(fileInfo.FullName, onStartedRecordingVideoCallback);
		}

		// Token: 0x06001F21 RID: 7969
		[NativeMethod("VideoCaptureBindings::StartRecordingVideoToDisk", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StartRecordingVideoToDisk_Internal(string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback);

		// Token: 0x06001F22 RID: 7970 RVA: 0x0003329F File Offset: 0x0003149F
		[RequiredByNativeCode]
		private static void InvokeOnStartedRecordingVideoToDiskDelegate(VideoCapture.OnStartedRecordingVideoCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001F23 RID: 7971
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::StopRecordingVideoToDisk", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopRecordingAsync([NotNull("ArgumentNullException")] VideoCapture.OnStoppedRecordingVideoCallback onStoppedRecordingVideoCallback);

		// Token: 0x06001F24 RID: 7972 RVA: 0x000332AF File Offset: 0x000314AF
		[RequiredByNativeCode]
		private static void InvokeOnStoppedRecordingVideoToDiskDelegate(VideoCapture.OnStoppedRecordingVideoCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001F25 RID: 7973
		[ThreadAndSerializationSafe]
		[NativeMethod("VideoCaptureBindings::GetUnsafePointerToVideoDeviceController", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetUnsafePointerToVideoDeviceController();

		// Token: 0x06001F26 RID: 7974 RVA: 0x000332C0 File Offset: 0x000314C0
		public void Dispose()
		{
			bool flag = this.m_NativePtr != IntPtr.Zero;
			if (flag)
			{
				this.Dispose_Internal();
				this.m_NativePtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F27 RID: 7975
		[NativeMethod("VideoCaptureBindings::Dispose", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Dispose_Internal();

		// Token: 0x06001F28 RID: 7976 RVA: 0x00033300 File Offset: 0x00031500
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_NativePtr != IntPtr.Zero;
				if (flag)
				{
					this.DisposeThreaded_Internal();
					this.m_NativePtr = IntPtr.Zero;
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001F29 RID: 7977
		[NativeMethod("VideoCaptureBindings::DisposeThreaded", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisposeThreaded_Internal();

		// Token: 0x06001F2A RID: 7978
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StartVideoMode_Internal_Injected(ref CameraParameters cameraParameters, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback);

		// Token: 0x04000A3E RID: 2622
		internal IntPtr m_NativePtr;

		// Token: 0x04000A3F RID: 2623
		private static Resolution[] s_SupportedResolutions;

		// Token: 0x04000A40 RID: 2624
		private static readonly long HR_SUCCESS;

		// Token: 0x020002E5 RID: 741
		public enum CaptureResultType
		{
			// Token: 0x04000A42 RID: 2626
			Success,
			// Token: 0x04000A43 RID: 2627
			UnknownError
		}

		// Token: 0x020002E6 RID: 742
		public enum AudioState
		{
			// Token: 0x04000A45 RID: 2629
			MicAudio,
			// Token: 0x04000A46 RID: 2630
			ApplicationAudio,
			// Token: 0x04000A47 RID: 2631
			ApplicationAndMicAudio,
			// Token: 0x04000A48 RID: 2632
			None
		}

		// Token: 0x020002E7 RID: 743
		public struct VideoCaptureResult
		{
			// Token: 0x1700060C RID: 1548
			// (get) Token: 0x06001F2B RID: 7979 RVA: 0x00033354 File Offset: 0x00031554
			public bool success
			{
				get
				{
					return this.resultType == VideoCapture.CaptureResultType.Success;
				}
			}

			// Token: 0x04000A49 RID: 2633
			public VideoCapture.CaptureResultType resultType;

			// Token: 0x04000A4A RID: 2634
			public long hResult;
		}

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x06001F2D RID: 7981
		public delegate void OnVideoCaptureResourceCreatedCallback(VideoCapture captureObject);

		// Token: 0x020002E9 RID: 745
		// (Invoke) Token: 0x06001F31 RID: 7985
		public delegate void OnVideoModeStartedCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002EA RID: 746
		// (Invoke) Token: 0x06001F35 RID: 7989
		public delegate void OnVideoModeStoppedCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002EB RID: 747
		// (Invoke) Token: 0x06001F39 RID: 7993
		public delegate void OnStartedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002EC RID: 748
		// (Invoke) Token: 0x06001F3D RID: 7997
		public delegate void OnStoppedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);
	}
}
