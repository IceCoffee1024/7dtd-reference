using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007B0 RID: 1968
[Preserve]
public class NetPackageAuthState : NetPackage
{
	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06003AAA RID: 15018 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x0017C318 File Offset: 0x0017A518
	public NetPackageAuthState Setup(string _authStateKey)
	{
		this.stateKey = _authStateKey ?? "";
		return this;
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x0017C32B File Offset: 0x0017A52B
	public override void read(PooledBinaryReader _reader)
	{
		this.stateKey = _reader.ReadString();
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x0017C339 File Offset: 0x0017A539
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.stateKey);
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x0017C350 File Offset: 0x0017A550
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

	// Token: 0x04002FCB RID: 12235
	[PublicizedFrom(EAccessModifier.Private)]
	public string stateKey;
}
