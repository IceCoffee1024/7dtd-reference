using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x0200076F RID: 1903
public class ClientInfoCollection
{
	// Token: 0x0600389B RID: 14491 RVA: 0x001726D4 File Offset: 0x001708D4
	public ClientInfoCollection()
	{
		this.List = new ReadOnlyCollection<ClientInfo>(this.list);
	}

	// Token: 0x0600389C RID: 14492 RVA: 0x0017272F File Offset: 0x0017092F
	public void Add(ClientInfo _cInfo)
	{
		this.list.Add(_cInfo);
		this.clientNumberMap.Add(_cInfo.ClientNumber, _cInfo);
	}

	// Token: 0x0600389D RID: 14493 RVA: 0x0017274F File Offset: 0x0017094F
	public void Clear()
	{
		this.list.Clear();
		this.clientNumberMap.Clear();
		this.entityIdMap.Clear();
		this.litenetPeerMap.Clear();
		this.userIdMap.Clear();
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x00172788 File Offset: 0x00170988
	public bool Contains(ClientInfo _cInfo)
	{
		return this.list.Contains(_cInfo);
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x00172798 File Offset: 0x00170998
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

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x060038A0 RID: 14496 RVA: 0x00172826 File Offset: 0x00170A26
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x00172834 File Offset: 0x00170A34
	public ClientInfo ForClientNumber(int _clientNumber)
	{
		ClientInfo clientInfo;
		if (this.clientNumberMap.TryGetValue(_clientNumber, out clientInfo))
		{
			return clientInfo;
		}
		return null;
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x00172854 File Offset: 0x00170A54
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

	// Token: 0x060038A3 RID: 14499 RVA: 0x001728B0 File Offset: 0x00170AB0
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

	// Token: 0x060038A4 RID: 14500 RVA: 0x0017290C File Offset: 0x00170B0C
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

	// Token: 0x060038A5 RID: 14501 RVA: 0x0017298C File Offset: 0x00170B8C
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

	// Token: 0x060038A6 RID: 14502 RVA: 0x00172A08 File Offset: 0x00170C08
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

	// Token: 0x04002E5C RID: 11868
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> list = new List<ClientInfo>();

	// Token: 0x04002E5D RID: 11869
	public readonly ReadOnlyCollection<ClientInfo> List;

	// Token: 0x04002E5E RID: 11870
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> clientNumberMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E5F RID: 11871
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> entityIdMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E60 RID: 11872
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<long, ClientInfo> litenetPeerMap = new Dictionary<long, ClientInfo>();

	// Token: 0x04002E61 RID: 11873
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, ClientInfo> userIdMap = new Dictionary<PlatformUserIdentifierAbs, ClientInfo>();
}
