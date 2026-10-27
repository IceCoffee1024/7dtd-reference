using System;

// Token: 0x02000908 RID: 2312
public class PersistentPlayerName
{
	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x0600425F RID: 16991 RVA: 0x001A1837 File Offset: 0x0019FA37
	public AuthoredText AuthoredName
	{
		get
		{
			return this.playerName;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06004260 RID: 16992 RVA: 0x001A1840 File Offset: 0x0019FA40
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

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06004261 RID: 16993 RVA: 0x001A192C File Offset: 0x0019FB2C
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

	// Token: 0x06004262 RID: 16994 RVA: 0x001A19A8 File Offset: 0x0019FBA8
	public PersistentPlayerName(AuthoredText name)
	{
		this.playerName = name;
		GeneratedTextManager.PrefilterText(name, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x001A19BE File Offset: 0x0019FBBE
	public void Update(string name, PlatformUserIdentifierAbs author)
	{
		this.cachedDisplayName = null;
		this.nameSuffix = 0;
		this.playerName.Update(name, author);
		GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
		GameManager.Instance.persistentPlayers.FixNameCollisions(name);
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x001A19F8 File Offset: 0x0019FBF8
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

	// Token: 0x06004265 RID: 16997 RVA: 0x001A1A44 File Offset: 0x0019FC44
	public void SetCollisionSuffix(int suffix)
	{
		this.nameSuffix = suffix;
	}

	// Token: 0x040035F6 RID: 13814
	[PublicizedFrom(EAccessModifier.Private)]
	public AuthoredText playerName;

	// Token: 0x040035F7 RID: 13815
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDisplayName;

	// Token: 0x040035F8 RID: 13816
	[PublicizedFrom(EAccessModifier.Private)]
	public int nameSuffix;
}
