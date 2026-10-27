using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x02000777 RID: 1911
public class ClientInfoCollection
{
	// Token: 0x060038B2 RID: 14514 RVA: 0x0017366C File Offset: 0x0017186C
	public ClientInfoCollection()
	{
		this.List = new ReadOnlyCollection<ClientInfo>(this.list);
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x001736C7 File Offset: 0x001718C7
	public void Add(ClientInfo _cInfo)
	{
		this.list.Add(_cInfo);
		this.clientNumberMap.Add(_cInfo.ClientNumber, _cInfo);
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x001736E7 File Offset: 0x001718E7
	public void Clear()
	{
		this.list.Clear();
		this.clientNumberMap.Clear();
		this.entityIdMap.Clear();
		this.litenetPeerMap.Clear();
		this.userIdMap.Clear();
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x00173720 File Offset: 0x00171920
	public bool Contains(ClientInfo _cInfo)
	{
		return this.list.Contains(_cInfo);
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x00173730 File Offset: 0x00171930
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

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x060038B7 RID: 14519 RVA: 0x001737BE File Offset: 0x001719BE
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x001737CC File Offset: 0x001719CC
	public ClientInfo ForClientNumber(int _clientNumber)
	{
		ClientInfo clientInfo;
		if (this.clientNumberMap.TryGetValue(_clientNumber, out clientInfo))
		{
			return clientInfo;
		}
		return null;
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x001737EC File Offset: 0x001719EC
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

	// Token: 0x060038BA RID: 14522 RVA: 0x00173848 File Offset: 0x00171A48
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

	// Token: 0x060038BB RID: 14523 RVA: 0x001738A4 File Offset: 0x00171AA4
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

	// Token: 0x060038BC RID: 14524 RVA: 0x00173924 File Offset: 0x00171B24
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

	// Token: 0x060038BD RID: 14525 RVA: 0x001739A0 File Offset: 0x00171BA0
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

	// Token: 0x04002E78 RID: 11896
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> list = new List<ClientInfo>();

	// Token: 0x04002E79 RID: 11897
	public readonly ReadOnlyCollection<ClientInfo> List;

	// Token: 0x04002E7A RID: 11898
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> clientNumberMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E7B RID: 11899
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, ClientInfo> entityIdMap = new Dictionary<int, ClientInfo>();

	// Token: 0x04002E7C RID: 11900
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<long, ClientInfo> litenetPeerMap = new Dictionary<long, ClientInfo>();

	// Token: 0x04002E7D RID: 11901
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, ClientInfo> userIdMap = new Dictionary<PlatformUserIdentifierAbs, ClientInfo>();
}
