using System;

// Token: 0x0200135B RID: 4955
public class WeatherPackage
{
	// Token: 0x06009C10 RID: 39952 RVA: 0x003B0378 File Offset: 0x003AE578
	public WeatherPackage()
	{
		this.param = new float[5];
	}

	// Token: 0x06009C11 RID: 39953 RVA: 0x003B038C File Offset: 0x003AE58C
	public void CopyTo(WeatherManager.BiomeWeather _bw)
	{
		_bw.remainingSeconds = this.remainingSeconds;
		for (int i = 0; i < this.param.Length; i++)
		{
			_bw.parameters[i].target = this.param[i];
		}
	}

	// Token: 0x06009C12 RID: 39954 RVA: 0x003B03D0 File Offset: 0x003AE5D0
	public override string ToString()
	{
		string text = string.Format("id {0}, grp {1}, rsec {2}, params ", this.biomeId, this.groupIndex, this.remainingSeconds);
		for (int i = 0; i < this.param.Length; i++)
		{
			text += string.Format("{0}, ", this.param[i]);
		}
		return text;
	}

	// Token: 0x0400762B RID: 30251
	public byte biomeId;

	// Token: 0x0400762C RID: 30252
	public byte groupIndex;

	// Token: 0x0400762D RID: 30253
	public byte remainingSeconds;

	// Token: 0x0400762E RID: 30254
	public float[] param;
}
