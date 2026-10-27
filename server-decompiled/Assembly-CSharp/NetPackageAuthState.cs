using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007B8 RID: 1976
[Preserve]
public class NetPackageAuthState : NetPackage
{
	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x0017D2B0 File Offset: 0x0017B4B0
	public NetPackageAuthState Setup(string _authStateKey)
	{
		this.stateKey = _authStateKey ?? "";
		return this;
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x0017D2C3 File Offset: 0x0017B4C3
	public override void read(PooledBinaryReader _reader)
	{
		this.stateKey = _reader.ReadString();
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0017D2D1 File Offset: 0x0017B4D1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.stateKey);
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x0017D2E8 File Offset: 0x0017B4E8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
		{
			Log.Out("Login: " + this.stateKey);
			if (!string.IsNullOrEmpty(this.stateKey))
			{
				string text = Localization.Get(this.stateKey, false, null);
				string text2 = text;
				object platformDisplayName = PlatformManager.NativePlatform.PlatformDisplayName;
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				text = string.Format(text2, platformDisplayName, (crossplatformPlatform != null) ? crossplatformPlatform.PlatformDisplayName : null);
				XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, text, true);
			}
		}
	}

	// Token: 0x04002FE7 RID: 12263
	[PublicizedFrom(EAccessModifier.Private)]
	public string stateKey;
}
