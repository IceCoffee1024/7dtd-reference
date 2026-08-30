using System;
using System.Collections.Generic;

// Token: 0x0200017F RID: 383
public interface ILockable
{
	// Token: 0x06000B59 RID: 2905
	bool IsLocked();

	// Token: 0x06000B5A RID: 2906
	void SetLocked(bool _isLocked);

	// Token: 0x06000B5B RID: 2907
	PlatformUserIdentifierAbs GetOwner();

	// Token: 0x06000B5C RID: 2908
	void SetOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B5D RID: 2909
	bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B5E RID: 2910
	List<PlatformUserIdentifierAbs> GetUsers();

	// Token: 0x06000B5F RID: 2911
	bool LocalPlayerIsOwner();

	// Token: 0x06000B60 RID: 2912
	bool IsOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B61 RID: 2913
	bool HasPassword();

	// Token: 0x06000B62 RID: 2914
	bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B63 RID: 2915
	bool CheckPasswordHash(string _password, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B64 RID: 2916
	string GetPasswordHash();

	// Token: 0x06000B65 RID: 2917 RVA: 0x00048904 File Offset: 0x00046B04
	string GetHashForPassword(string _password)
	{
		return _password.GetStableHashCode().ToString("X8");
	}
}
