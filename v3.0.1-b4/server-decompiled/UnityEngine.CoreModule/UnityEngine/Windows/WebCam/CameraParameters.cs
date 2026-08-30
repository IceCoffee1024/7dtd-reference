using System;
using System.Linq;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x020002F0 RID: 752
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	[UsedByNativeCode]
	[NativeHeader("PlatformDependent/Win/Webcam/CameraParameters.h")]
	public struct CameraParameters
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x00033370 File Offset: 0x00031570
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x00033388 File Offset: 0x00031588
		public float hologramOpacity
		{
			get
			{
				return this.m_HologramOpacity;
			}
			set
			{
				this.m_HologramOpacity = value;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x00033394 File Offset: 0x00031594
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x000333AC File Offset: 0x000315AC
		public float frameRate
		{
			get
			{
				return this.m_FrameRate;
			}
			set
			{
				this.m_FrameRate = value;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x000333B8 File Offset: 0x000315B8
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x000333D0 File Offset: 0x000315D0
		public int cameraResolutionWidth
		{
			get
			{
				return this.m_CameraResolutionWidth;
			}
			set
			{
				this.m_CameraResolutionWidth = value;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x000333DC File Offset: 0x000315DC
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x000333F4 File Offset: 0x000315F4
		public int cameraResolutionHeight
		{
			get
			{
				return this.m_CameraResolutionHeight;
			}
			set
			{
				this.m_CameraResolutionHeight = value;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x00033400 File Offset: 0x00031600
		// (set) Token: 0x06001F4B RID: 8011 RVA: 0x00033418 File Offset: 0x00031618
		public CapturePixelFormat pixelFormat
		{
			get
			{
				return this.m_PixelFormat;
			}
			set
			{
				this.m_PixelFormat = value;
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x00033424 File Offset: 0x00031624
		public CameraParameters(WebCamMode webCamMode)
		{
			this.m_HologramOpacity = 1f;
			this.m_PixelFormat = CapturePixelFormat.BGRA32;
			this.m_FrameRate = 0f;
			this.m_CameraResolutionWidth = 0;
			this.m_CameraResolutionHeight = 0;
			bool flag = webCamMode == WebCamMode.PhotoMode;
			if (flag)
			{
				Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((Resolution res) => res.width * res.height).First<Resolution>();
				this.m_CameraResolutionWidth = resolution.width;
				this.m_CameraResolutionHeight = resolution.height;
			}
			else
			{
				bool flag2 = webCamMode == WebCamMode.VideoMode;
				if (flag2)
				{
					Resolution resolution2 = VideoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((Resolution res) => res.width * res.height).First<Resolution>();
					float num = (from fps in VideoCapture.GetSupportedFrameRatesForResolution(resolution2)
						orderby fps descending
						select fps).First<float>();
					this.m_CameraResolutionWidth = resolution2.width;
					this.m_CameraResolutionHeight = resolution2.height;
					this.m_FrameRate = num;
				}
			}
		}

		// Token: 0x04000A54 RID: 2644
		private float m_HologramOpacity;

		// Token: 0x04000A55 RID: 2645
		private float m_FrameRate;

		// Token: 0x04000A56 RID: 2646
		private int m_CameraResolutionWidth;

		// Token: 0x04000A57 RID: 2647
		private int m_CameraResolutionHeight;

		// Token: 0x04000A58 RID: 2648
		private CapturePixelFormat m_PixelFormat;
	}
}
