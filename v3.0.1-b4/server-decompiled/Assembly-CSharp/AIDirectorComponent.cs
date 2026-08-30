using System;
using System.IO;

// Token: 0x0200040C RID: 1036
public abstract class AIDirectorComponent
{
	// Token: 0x06002016 RID: 8214 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Connect()
	{
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitNewGame()
	{
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Tick(double _dt)
	{
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Read(BinaryReader _stream, int _version)
	{
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Write(BinaryWriter _stream)
	{
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600201B RID: 8219 RVA: 0x000C26AD File Offset: 0x000C08AD
	public GameRandom Random
	{
		get
		{
			return this.Director.random;
		}
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorComponent()
	{
	}

	// Token: 0x040015D6 RID: 5590
	public AIDirector Director;
}
