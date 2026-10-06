using System;

// Token: 0x020008FE RID: 2302
public class PersistentPlayerName
{
	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x0600423C RID: 16956 RVA: 0x001A078F File Offset: 0x0019E98F
	public AuthoredText AuthoredName
	{
		get
		{
			return this.playerName;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x0600423D RID: 16957 RVA: 0x001A0798 File Offset: 0x0019E998
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

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x0600423E RID: 16958 RVA: 0x001A0884 File Offset: 0x0019EA84
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

	// Token: 0x0600423F RID: 16959 RVA: 0x001A0900 File Offset: 0x0019EB00
	public PersistentPlayerName(AuthoredText name)
	{
		this.playerName = name;
		GeneratedTextManager.PrefilterText(name, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x001A0916 File Offset: 0x0019EB16
	public void Update(string name, PlatformUserIdentifierAbs author)
	{
		this.cachedDisplayName = null;
		this.nameSuffix = 0;
		this.playerName.Update(name, author);
		GeneratedTextManager.PrefilterText(this.playerName, GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms);
		GameManager.Instance.persistentPlayers.FixNameCollisions(name);
	}

	// Token: 0x06004241 RID: 16961 RVA: 0x001A0950 File Offset: 0x0019EB50
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

	// Token: 0x06004242 RID: 16962 RVA: 0x001A099C File Offset: 0x0019EB9C
	public void SetCollisionSuffix(int suffix)
	{
		this.nameSuffix = suffix;
	}

	// Token: 0x040035D6 RID: 13782
	[PublicizedFrom(EAccessModifier.Private)]
	public AuthoredText playerName;

	// Token: 0x040035D7 RID: 13783
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDisplayName;

	// Token: 0x040035D8 RID: 13784
	[PublicizedFrom(EAccessModifier.Private)]
	public int nameSuffix;
}
