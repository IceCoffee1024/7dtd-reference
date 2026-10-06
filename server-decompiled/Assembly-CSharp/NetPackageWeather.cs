using System;
using UnityEngine.Scripting;

// Token: 0x0200139A RID: 5018
[Preserve]
public class NetPackageWeather : NetPackage
{
	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x06009DF8 RID: 40440 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06009DF9 RID: 40441 RVA: 0x003BB5A8 File Offset: 0x003B97A8
	public NetPackageWeather Setup(WeatherPackage[] _packages)
	{
		this.weatherPackages = _packages;
		return this;
	}

	// Token: 0x06009DFA RID: 40442 RVA: 0x003BB5B4 File Offset: 0x003B97B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitPackages()
	{
		if (WeatherManager.Instance)
		{
			int count = WeatherManager.Instance.biomeWeather.Count;
			this.weatherPackages = new WeatherPackage[count];
			for (int i = 0; i < count; i++)
			{
				this.weatherPackages[i] = new WeatherPackage();
			}
		}
	}

	// Token: 0x06009DFB RID: 40443 RVA: 0x003BB604 File Offset: 0x003B9804
	public override void read(PooledBinaryReader _br)
	{
		if (this.weatherPackages == null)
		{
			this.InitPackages();
			if (this.weatherPackages == null)
			{
				return;
			}
		}
		for (int i = 0; i < this.weatherPackages.Length; i++)
		{
			WeatherPackage weatherPackage = this.weatherPackages[i];
			weatherPackage.biomeId = _br.ReadByte();
			weatherPackage.groupIndex = _br.ReadByte();
			weatherPackage.remainingSeconds = _br.ReadByte();
			for (int j = 0; j < weatherPackage.param.Length; j++)
			{
				weatherPackage.param[j] = _br.ReadSingle();
			}
		}
	}

	// Token: 0x06009DFC RID: 40444 RVA: 0x003BB68C File Offset: 0x003B988C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		for (int i = 0; i < this.weatherPackages.Length; i++)
		{
			WeatherPackage weatherPackage = this.weatherPackages[i];
			_bw.Write(weatherPackage.biomeId);
			_bw.Write(weatherPackage.groupIndex);
			_bw.Write(weatherPackage.remainingSeconds);
			for (int j = 0; j < weatherPackage.param.Length; j++)
			{
				_bw.Write(weatherPackage.param[j]);
			}
		}
	}

	// Token: 0x06009DFD RID: 40445 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x040077BE RID: 30654
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherPackage[] weatherPackages;
}
