using System;

// Token: 0x02000412 RID: 1042
public abstract class AIDirectorComponent
{
	// Token: 0x06002022 RID: 8226 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Connect()
	{
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitNewGame()
	{
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Tick(double _dt)
	{
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Read(PooledBinaryReader _stream, int _version)
	{
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Write(PooledBinaryWriter _stream)
	{
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06002027 RID: 8231 RVA: 0x000C2EFD File Offset: 0x000C10FD
	public GameRandom Random
	{
		get
		{
			return this.Director.random;
		}
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorComponent()
	{
	}

	// Token: 0x040015E2 RID: 5602
	public AIDirector Director;
}
