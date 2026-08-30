using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007AD RID: 1965
[Preserve]
public class NetPackageAuthState : NetPackage
{
	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06003A6E RID: 14958 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06003A6F RID: 14959 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06003A70 RID: 14960 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x0017BC7C File Offset: 0x00179E7C
	public NetPackageAuthState Setup(string _authStateKey)
	{
		this.stateKey = _authStateKey ?? "";
		return this;
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x0017BC8F File Offset: 0x00179E8F
	public override void read(PooledBinaryReader _reader)
	{
		this.stateKey = _reader.ReadString();
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0017BC9D File Offset: 0x00179E9D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.stateKey);
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0017BCB4 File Offset: 0x00179EB4
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

	// Token: 0x06003A75 RID: 14965 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04002FD1 RID: 12241
	[PublicizedFrom(EAccessModifier.Private)]
	public string stateKey;
}
