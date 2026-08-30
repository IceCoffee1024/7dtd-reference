using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x0200076D RID: 1901
public class ClientInfoCollection
{
	// Token: 0x06003858 RID: 14424 RVA: 0x00171D70 File Offset: 0x0016FF70
	public ClientInfoCollection()
	{
		this.List = new ReadOnlyCollection<ClientInfo>(this.list);
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x00171DCB File Offset: 0x0016FFCB
	public void Add(ClientInfo _cInfo)
	{
		this.list.Add(_cInfo);
		this.clientNumberMap.Add(_cInfo.ClientNumber, _cInfo);
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x00171DEB File Offset: 0x0016FFEB
	public void Clear()
	{
		this.list.Clear();
		this.clientNumberMap.Clear();
		this.entityIdMap.Clear();
		this.litenetPeerMap.Clear();
		this.userIdMap.Clear();
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x00171E24 File Offset: 0x00170024
	public bool Contains(ClientInfo _cInfo)
	{
		return this.list.Contains(_cInfo);
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x00171E34 File Offset: 0x00170034
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

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x0600385D RID: 14429 RVA: 0x00171EC2 File Offset: 0x001700C2
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x00171ED0 File Offset: 0x001700D0
	public ClientInfo ForClientNumber(int _clientNumber)
	{
		ClientInfo clientInfo;
		if (this.clientNumberMap.TryGetValue(_clientNumber, out clientInfo))
		{
			return clientInfo;
		}
		return null;
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x00171EF0 File Offset: 0x001700F0
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

	// Token: 0x06003860 RID: 14432 RVA: 0x00171F4C File Offset: 0x0017014C
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

	// Token: 0x06003861 RID: 14433 RVA: 0x00171FA8 File Offset: 0x001701A8
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

	// Token: 0x06003862 RID: 14434 RVA: 0x00172028 File Offset: 0x00170228
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

	// Token: 0x06003863 RID: 14435 RVA: 0x001720A4 File Offset: 0x001702A4
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

	// Token: 0x04002E63 RID: 11875
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> list = new List<ClientInfo>();

	// Token: 0x04002E64 RID: 11876
	public readonly ReadOnlyCollection<ClientInfo> List;

	// Token: 0x04002E65 RID: 11877
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> clientNumberMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E66 RID: 11878
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> entityIdMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E67 RID: 11879
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<long, ClientInfo> litenetPeerMap = new Dictionary<long, ClientInfo>();

	// Token: 0x04002E68 RID: 11880
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, ClientInfo> userIdMap = new Dictionary<PlatformUserIdentifierAbs, ClientInfo>();
}
