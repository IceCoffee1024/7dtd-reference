using System;
using System.Collections.Generic;

// Token: 0x02000180 RID: 384
public interface ILockable
{
	// Token: 0x06000B62 RID: 2914
	bool IsLocked();

	// Token: 0x06000B63 RID: 2915
	void SetLocked(bool _isLocked);

	// Token: 0x06000B64 RID: 2916
	PlatformUserIdentifierAbs GetOwner();

	// Token: 0x06000B65 RID: 2917
	void SetOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B66 RID: 2918
	bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B67 RID: 2919
	List<PlatformUserIdentifierAbs> GetUsers();

	// Token: 0x06000B68 RID: 2920
	bool LocalPlayerIsOwner();

	// Token: 0x06000B69 RID: 2921
	bool IsOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B6A RID: 2922
	bool HasPassword();

	// Token: 0x06000B6B RID: 2923
	bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B6C RID: 2924
	bool CheckPasswordHash(string _password, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B6D RID: 2925
	string GetPasswordHash();

	// Token: 0x06000B6E RID: 2926 RVA: 0x00048A98 File Offset: 0x00046C98
	string GetHashForPassword(string _password)
	{
		return _password.GetStableHashCode().ToString("X8");
	}
}
