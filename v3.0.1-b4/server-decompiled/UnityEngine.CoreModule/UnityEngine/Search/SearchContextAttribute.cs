using System;

namespace UnityEngine.Search
{
	// Token: 0x02000311 RID: 785
	[AttributeUsage(AttributeTargets.Field)]
	public class SearchContextAttribute : PropertyAttribute
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x000357AE File Offset: 0x000339AE
		// (set) Token: 0x06002020 RID: 8224 RVA: 0x000357B6 File Offset: 0x000339B6
		public string query { get; private set; }

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000357BF File Offset: 0x000339BF
		// (set) Token: 0x06002022 RID: 8226 RVA: 0x000357C7 File Offset: 0x000339C7
		public string[] providerIds { get; private set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000357D0 File Offset: 0x000339D0
		// (set) Token: 0x06002024 RID: 8228 RVA: 0x000357D8 File Offset: 0x000339D8
		public Type[] instantiableProviders { get; private set; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000357E1 File Offset: 0x000339E1
		// (set) Token: 0x06002026 RID: 8230 RVA: 0x000357E9 File Offset: 0x000339E9
		public SearchViewFlags flags { get; private set; }

		// Token: 0x06002027 RID: 8231 RVA: 0x000357F2 File Offset: 0x000339F2
		public SearchContextAttribute(string query)
			: this(query, null, SearchViewFlags.None)
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x000357FF File Offset: 0x000339FF
		public SearchContextAttribute(string query, SearchViewFlags flags)
			: this(query, null, flags)
		{
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0003580C File Offset: 0x00033A0C
		public SearchContextAttribute(string query, string providerIdsCommaSeparated)
			: this(query, providerIdsCommaSeparated, SearchViewFlags.None)
		{
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00035819 File Offset: 0x00033A19
		public SearchContextAttribute(string query, string providerIdsCommaSeparated, SearchViewFlags flags)
			: this(query, flags, providerIdsCommaSeparated, null)
		{
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00035827 File Offset: 0x00033A27
		public SearchContextAttribute(string query, params Type[] instantiableProviders)
			: this(query, SearchViewFlags.None, null, instantiableProviders)
		{
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00035835 File Offset: 0x00033A35
		public SearchContextAttribute(string query, SearchViewFlags flags, params Type[] instantiableProviders)
			: this(query, flags, null, instantiableProviders)
		{
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00035844 File Offset: 0x00033A44
		public SearchContextAttribute(string query, SearchViewFlags flags, string providerIdsCommaSeparated, params Type[] instantiableProviders)
		{
			this.query = query;
			this.providerIds = ((providerIdsCommaSeparated != null) ? providerIdsCommaSeparated.Split(new char[] { ',', ';' }) : null) ?? new string[0];
			this.instantiableProviders = instantiableProviders ?? new Type[0];
			this.flags = flags | SearchViewFlags.ObjectPicker;
		}
	}
}
