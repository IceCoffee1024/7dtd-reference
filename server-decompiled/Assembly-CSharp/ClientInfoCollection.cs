using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x02000763 RID: 1891
public class ClientInfoCollection
{
	// Token: 0x06003802 RID: 14338 RVA: 0x0016FB54 File Offset: 0x0016DD54
	public ClientInfoCollection()
	{
		this.List = new ReadOnlyCollection<ClientInfo>(this.list);
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x0016FBAF File Offset: 0x0016DDAF
	public void Add(ClientInfo _cInfo)
	{
		this.list.Add(_cInfo);
		this.clientNumberMap.Add(_cInfo.ClientNumber, _cInfo);
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x0016FBCF File Offset: 0x0016DDCF
	public void Clear()
	{
		this.list.Clear();
		this.clientNumberMap.Clear();
		this.entityIdMap.Clear();
		this.litenetPeerMap.Clear();
		this.userIdMap.Clear();
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x0016FC08 File Offset: 0x0016DE08
	public bool Contains(ClientInfo _cInfo)
	{
		return this.list.Contains(_cInfo);
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x0016FC18 File Offset: 0x0016DE18
	public void Remove(ClientInfo _cInfo)
	{
		this.list.Remove(_cInfo);
		this.clientNumberMap.Remove(_cInfo.ClientNumber);
		this.entityIdMap.Remove(_cInfo.entityId);
		if (_cInfo.litenetPeerConnectId >= 0L)
		{
			this.litenetPeerMap.Remove(_cInfo.litenetPeerConnectId);
		}
		if (_cInfo.PlatformId != null)
		{
			this.userIdMap.Remove(_cInfo.PlatformId);
		}
		if (_cInfo.CrossplatformId != null)
		{
			this.userIdMap.Remove(_cInfo.CrossplatformId);
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06003807 RID: 14343 RVA: 0x0016FCA6 File Offset: 0x0016DEA6
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x0016FCB4 File Offset: 0x0016DEB4
	public ClientInfo ForClientNumber(int _clientNumber)
	{
		ClientInfo clientInfo;
		if (this.clientNumberMap.TryGetValue(_clientNumber, out clientInfo))
		{
			return clientInfo;
		}
		return null;
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0016FCD4 File Offset: 0x0016DED4
	public ClientInfo ForEntityId(int _entityId)
	{
		ClientInfo clientInfo;
		if (this.entityIdMap.TryGetValue(_entityId, out clientInfo))
		{
			return clientInfo;
		}
		for (int i = 0; i < this.list.Count; i++)
		{
			ClientInfo clientInfo2 = this.list[i];
			if (clientInfo2.entityId == _entityId)
			{
				this.entityIdMap.Add(_entityId, clientInfo2);
				return clientInfo2;
			}
		}
		return null;
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x0016FD30 File Offset: 0x0016DF30
	public ClientInfo ForLiteNetPeer(long _peerConnectId)
	{
		ClientInfo clientInfo;
		if (this.litenetPeerMap.TryGetValue(_peerConnectId, out clientInfo))
		{
			return clientInfo;
		}
		for (int i = 0; i < this.list.Count; i++)
		{
			ClientInfo clientInfo2 = this.list[i];
			if (clientInfo2.litenetPeerConnectId == _peerConnectId)
			{
				this.litenetPeerMap.Add(_peerConnectId, clientInfo2);
				return clientInfo2;
			}
		}
		return null;
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x0016FD8C File Offset: 0x0016DF8C
	public ClientInfo ForUserId(PlatformUserIdentifierAbs _userIdentifier)
	{
		ClientInfo clientInfo;
		if (this.userIdMap.TryGetValue(_userIdentifier, out clientInfo))
		{
			return clientInfo;
		}
		for (int i = 0; i < this.list.Count; i++)
		{
			ClientInfo clientInfo2 = this.list[i];
			if (_userIdentifier.Equals(clientInfo2.PlatformId))
			{
				this.userIdMap[_userIdentifier] = clientInfo2;
				return clientInfo2;
			}
			if (_userIdentifier.Equals(clientInfo2.CrossplatformId))
			{
				this.userIdMap[_userIdentifier] = clientInfo2;
				return clientInfo2;
			}
		}
		return null;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x0016FE0C File Offset: 0x0016E00C
	public ClientInfo GetForPlayerName(string _playerName, bool _ignoreCase = true, bool _ignoreBlanks = false)
	{
		if (_ignoreBlanks)
		{
			_playerName = _playerName.Replace(" ", "");
		}
		for (int i = 0; i < this.list.Count; i++)
		{
			ClientInfo clientInfo = this.list[i];
			string text = clientInfo.playerName ?? string.Empty;
			if (_ignoreBlanks)
			{
				text = text.Replace(" ", "");
			}
			if (string.Equals(text, _playerName, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				return clientInfo;
			}
		}
		return null;
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0016FE88 File Offset: 0x0016E088
	public ClientInfo GetForNameOrId(string _nameOrId, bool _ignoreCase = true, bool _ignoreBlanks = false)
	{
		int num;
		if (int.TryParse(_nameOrId, out num))
		{
			ClientInfo clientInfo = this.ForEntityId(num);
			if (clientInfo != null)
			{
				return clientInfo;
			}
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if (PlatformUserIdentifierAbs.TryFromCombinedString(_nameOrId, out platformUserIdentifierAbs))
		{
			ClientInfo clientInfo2 = this.ForUserId(platformUserIdentifierAbs);
			if (clientInfo2 != null)
			{
				return clientInfo2;
			}
		}
		return this.GetForPlayerName(_nameOrId, _ignoreCase, _ignoreBlanks);
	}

	// Token: 0x04002DEB RID: 11755
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> list = new List<ClientInfo>();

	// Token: 0x04002DEC RID: 11756
	public readonly ReadOnlyCollection<ClientInfo> List;

	// Token: 0x04002DED RID: 11757
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> clientNumberMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002DEE RID: 11758
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> entityIdMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002DEF RID: 11759
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<long, ClientInfo> litenetPeerMap = new Dictionary<long, ClientInfo>();

	// Token: 0x04002DF0 RID: 11760
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, ClientInfo> userIdMap = new Dictionary<PlatformUserIdentifierAbs, ClientInfo>();
}
