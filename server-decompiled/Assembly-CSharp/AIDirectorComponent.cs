using System;

// Token: 0x02000414 RID: 1044
public abstract class AIDirectorComponent
{
	// Token: 0x06002027 RID: 8231 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Connect()
	{
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitNewGame()
	{
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Tick(double _dt)
	{
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Read(PooledBinaryReader _stream, int _version)
	{
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Write(PooledBinaryWriter _stream)
	{
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600202C RID: 8236 RVA: 0x000C32A9 File Offset: 0x000C14A9
	public GameRandom Random
	{
		get
		{
			return this.Director.random;
		}
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorComponent()
	{
	}

	// Token: 0x040015F2 RID: 5618
	public AIDirector Director;
}
