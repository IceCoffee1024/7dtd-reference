using System;

// Token: 0x020008F9 RID: 2297
public class PersistentPlayerName
{
	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x060042AA RID: 17066 RVA: 0x001A068B File Offset: 0x0019E88B
	public AuthoredText AuthoredName
	{
		get
		{
			return this.playerName;
		}
	}

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x060042AB RID: 17067 RVA: 0x001A0694 File Offset: 0x0019E894
	public string DisplayName
	{
		get
		{
			if (this.cachedDisplayName != null)
			{
				return this.cachedDisplayName + ((this.nameSuffix > 0) ? string.Format("({0})", this.nameSuffix) : "");
			}
			if (!GeneratedTextManager.IsFiltered(this.playerName))
			{
				if (!GeneratedTextManager.IsFiltering(this.playerName))
				{
					GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.Filter);
				}
				return GeneratedTextManager.GetDisplayTextImmediately(this.playerName, false, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes) + ((this.nameSuffix > 0) ? string.Format("({0})", this.nameSuffix) : "");
			}
			this.cachedDisplayName = GeneratedTextManager.GetDisplayTextImmediately(this.playerName, false, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			return this.cachedDisplayName + ((this.nameSuffix > 0) ? string.Format("({0})", this.nameSuffix) : "");
		}
	}

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x060042AC RID: 17068 RVA: 0x001A0780 File Offset: 0x0019E980
	public string SafeDisplayName
	{
		get
		{
			if (!GeneratedTextManager.IsFiltered(this.playerName))
			{
				if (!GeneratedTextManager.IsFiltering(this.playerName))
				{
					GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.Filter);
				}
				string text = GeneratedTextManager.GetDisplayTextImmediately(this.playerName, false, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
				if (!text.Equals("{...}") && this.nameSuffix > 0)
				{
					text += string.Format("({0})", this.nameSuffix);
				}
				return text;
			}
			return this.DisplayName;
		}
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x001A07FC File Offset: 0x0019E9FC
	public PersistentPlayerName(AuthoredText name)
	{
		this.playerName = name;
		GeneratedTextManager.PrefilterText(name, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x001A0812 File Offset: 0x0019EA12
	public void Update(string name, PlatformUserIdentifierAbs author)
	{
		this.cachedDisplayName = null;
		this.nameSuffix = 0;
		this.playerName.Update(name, author);
		GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
		GameManager.Instance.persistentPlayers.FixNameCollisions(name);
	}

	// Token: 0x060042AF RID: 17071 RVA: 0x001A084C File Offset: 0x0019EA4C
	public void Update(AuthoredText name)
	{
		if (this.playerName != name)
		{
			this.cachedDisplayName = null;
			this.nameSuffix = 0;
			this.playerName = name;
			GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
			GameManager.Instance.persistentPlayers.FixNameCollisions(name.Text);
		}
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x001A0898 File Offset: 0x0019EA98
	public void SetCollisionSuffix(int suffix)
	{
		this.nameSuffix = suffix;
	}

	// Token: 0x040035E2 RID: 13794
	[PublicizedFrom(EAccessModifier.Private)]
	public AuthoredText playerName;

	// Token: 0x040035E3 RID: 13795
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDisplayName;

	// Token: 0x040035E4 RID: 13796
	[PublicizedFrom(EAccessModifier.Private)]
	public int nameSuffix;
}
