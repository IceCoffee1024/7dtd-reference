using System;
using UnityEngine.Scripting;

// Token: 0x0200137A RID: 4986
[Preserve]
public class NetPackageWeather : NetPackage
{
	// Token: 0x17001298 RID: 4760
	// (get) Token: 0x06009D4C RID: 40268 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06009D4D RID: 40269 RVA: 0x003B6A2C File Offset: 0x003B4C2C
	public NetPackageWeather Setup(WeatherPackage[] _packages)
	{
		this.weatherPackages = _packages;
		return this;
	}

	// Token: 0x06009D4E RID: 40270 RVA: 0x003B6A38 File Offset: 0x003B4C38
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

	// Token: 0x06009D4F RID: 40271 RVA: 0x003B6A88 File Offset: 0x003B4C88
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

	// Token: 0x06009D50 RID: 40272 RVA: 0x003B6B10 File Offset: 0x003B4D10
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

	// Token: 0x06009D51 RID: 40273 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06009D52 RID: 40274 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04007743 RID: 30531
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherPackage[] weatherPackages;
}
