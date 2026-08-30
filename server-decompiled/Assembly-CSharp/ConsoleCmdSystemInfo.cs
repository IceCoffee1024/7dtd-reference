using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

// Token: 0x020002A6 RID: 678
[Preserve]
public class ConsoleCmdSystemInfo : ConsoleCmdAbstract
{
	// Token: 0x060013AA RID: 5034 RVA: 0x00077DAF File Offset: 0x00075FAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "SystemInfo" };
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x00077DBF File Offset: 0x00075FBF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List SystemInfo";
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x00077DC8 File Offset: 0x00075FC8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Device Model                   :" + SystemInfo.deviceModel);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("deviceModel                    :" + SystemInfo.deviceModel);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("deviceName                     :" + SystemInfo.deviceName);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("deviceType                     :" + SystemInfo.deviceType.ToStringCached<DeviceType>());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("deviceUniqueIdentifier         :" + SystemInfo.deviceUniqueIdentifier);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceID               :" + SystemInfo.graphicsDeviceID.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceName             :" + SystemInfo.graphicsDeviceName);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceType             :" + SystemInfo.graphicsDeviceType.ToStringCached<GraphicsDeviceType>());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceVendor           :" + SystemInfo.graphicsDeviceVendor);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceVendorID         :" + SystemInfo.graphicsDeviceVendorID.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsDeviceVersion          :" + SystemInfo.graphicsDeviceVersion);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsMemorySize             :" + SystemInfo.graphicsMemorySize.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsMultiThreaded          :" + SystemInfo.graphicsMultiThreaded.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("graphicsShaderLevel            :" + SystemInfo.graphicsShaderLevel.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("maxTextureSize                 :" + SystemInfo.maxTextureSize.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("npotSupport                    :" + SystemInfo.npotSupport.ToStringCached<NPOTSupport>());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("operatingSystem                :" + SystemInfo.operatingSystem);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("processorCount                 :" + SystemInfo.processorCount.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("processorType                  :" + SystemInfo.processorType);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportedRenderTargetCount     :" + SystemInfo.supportedRenderTargetCount.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supports3DTextures             :" + SystemInfo.supports3DTextures.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsAccelerometer          :" + SystemInfo.supportsAccelerometer.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsComputeShaders         :" + SystemInfo.supportsComputeShaders.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsGyroscope              :" + SystemInfo.supportsGyroscope.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsImageEffects           : true (always)");
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsInstancing             :" + SystemInfo.supportsInstancing.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsLocationService        :" + SystemInfo.supportsLocationService.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsRenderToCubemap        : true (always)");
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsShadows                :" + SystemInfo.supportsShadows.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsSparseTextures         :" + SystemInfo.supportsSparseTextures.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("supportsVibration              :" + SystemInfo.supportsVibration.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("systemMemorySize               :" + SystemInfo.systemMemorySize.ToString());
	}
}
