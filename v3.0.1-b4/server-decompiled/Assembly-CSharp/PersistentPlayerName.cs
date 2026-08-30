using System;

// Token: 0x020008ED RID: 2285
public class PersistentPlayerName
{
	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06004245 RID: 16965 RVA: 0x0019E5B3 File Offset: 0x0019C7B3
	public AuthoredText AuthoredName
	{
		get
		{
			return this.playerName;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06004246 RID: 16966 RVA: 0x0019E5BC File Offset: 0x0019C7BC
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

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06004247 RID: 16967 RVA: 0x0019E6A8 File Offset: 0x0019C8A8
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

	// Token: 0x06004248 RID: 16968 RVA: 0x0019E724 File Offset: 0x0019C924
	public PersistentPlayerName(AuthoredText name)
	{
		this.playerName = name;
		GeneratedTextManager.PrefilterText(name, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x0019E73A File Offset: 0x0019C93A
	public void Update(string name, PlatformUserIdentifierAbs author)
	{
		this.cachedDisplayName = null;
		this.nameSuffix = 0;
		this.playerName.Update(name, author);
		GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
		GameManager.Instance.persistentPlayers.FixNameCollisions(name);
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x0019E774 File Offset: 0x0019C974
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

	// Token: 0x0600424B RID: 16971 RVA: 0x0019E7C0 File Offset: 0x0019C9C0
	public void SetCollisionSuffix(int suffix)
	{
		this.nameSuffix = suffix;
	}

	// Token: 0x04003561 RID: 13665
	[PublicizedFrom(EAccessModifier.Private)]
	public AuthoredText playerName;

	// Token: 0x04003562 RID: 13666
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDisplayName;

	// Token: 0x04003563 RID: 13667
	[PublicizedFrom(EAccessModifier.Private)]
	public int nameSuffix;
}
