using System;
using UnityEngine.Scripting;

// Token: 0x020013AC RID: 5036
[Preserve]
public class NetPackageWeather : NetPackage
{
	// Token: 0x170012E9 RID: 4841
	// (get) Token: 0x06009E50 RID: 40528 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06009E51 RID: 40529 RVA: 0x003BD9AC File Offset: 0x003BBBAC
	public NetPackageWeather Setup(WeatherPackage[] _packages)
	{
		this.weatherPackages = _packages;
		return this;
	}

	// Token: 0x06009E52 RID: 40530 RVA: 0x003BD9B8 File Offset: 0x003BBBB8
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

	// Token: 0x06009E53 RID: 40531 RVA: 0x003BDA08 File Offset: 0x003BBC08
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

	// Token: 0x06009E54 RID: 40532 RVA: 0x003BDA90 File Offset: 0x003BBC90
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

	// Token: 0x06009E55 RID: 40533 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x040077FF RID: 30719
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherPackage[] weatherPackages;
}
