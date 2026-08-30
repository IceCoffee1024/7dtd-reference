using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007A3 RID: 1955
[Preserve]
public class NetPackageAuthState : NetPackage
{
	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06003A16 RID: 14870 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x06003A17 RID: 14871 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x06003A18 RID: 14872 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x001799FC File Offset: 0x00177BFC
	public NetPackageAuthState Setup(string _authStateKey)
	{
		this.stateKey = _authStateKey ?? "";
		return this;
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x00179A0F File Offset: 0x00177C0F
	public override void read(PooledBinaryReader _reader)
	{
		this.stateKey = _reader.ReadString();
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x00179A1D File Offset: 0x00177C1D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.stateKey);
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x00179A34 File Offset: 0x00177C34
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

	// Token: 0x06003A1D RID: 14877 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04002F58 RID: 12120
	[PublicizedFrom(EAccessModifier.Private)]
	public string stateKey;
}
