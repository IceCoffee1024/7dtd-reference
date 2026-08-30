# 7DTD Reference（中文）

英文文档：[README.md](README.md)

## 仓库用途

本私有仓库用于分析《7 Days to Die》专用服务器并开发 7DPanel。反编译源码和运行时文件是兼容性分析证据；共享历史资源仅用于视觉参考。

## 目录结构

仓库只保留最新版本，参考文件位于根目录；历史版本通过 Git 托管平台的 Release 资产保存：

```text
7dtd-reference/
|-- README.md
|-- README.zh-CN.md
|-- metadata.json
|-- assets/
|   `-- Sprites/
|-- scripts/
|   |-- Set-DecompiledTargetFramework.ps1
|   |-- Verify-Reference.ps1
|   `-- New-ReferenceRelease.ps1
|-- server-decompiled/
`-- runtime/
    |-- 7DaysToDieServer_Data/
    |-- Mods/
    |-- MonoBleedingEdge/
    |-- serverconfig.xml
    `-- startdedicated.bat
```

不要混用不同游戏构建的文件。当前版本、构建号、提取工具和归档哈希记录在 `metadata.json` 中，并使用同名 Release tag 标识。

## 运行时参考文件

`runtime/` 下的文件应保持官方 7DTD Dedicated Server 分发内容不变。`serverconfig.xml` 和 `startdedicated.bat` 是原始分发文件，不是贡献者服务器的运行配置。

本地或远程测试时，请将 `serverconfig.xml` 复制到仓库外，再在副本中设置服务器名称、路径、凭据、端口和游戏参数。运行时参考文件不得包含在 7DPanel 发布包中。

## 共享资源与访问边界

`assets/` 保存跨版本稳定的视觉参考资源，不能证明特定服务器构建的兼容性，也不得作为运行时或 API 兼容性证据。仓库应保持私有，仅向授权项目贡献者开放。私有可见性不自动授予复制、托管、分发、逆向工程或反编译游戏内容的权利。参见[官方 EULA](https://7daystodie.com/eula/)。

## 反编译项目目标框架

dnSpy 生成的项目可能以 `net40` 为目标框架。重定向脚本会将根目录 `server-decompiled/` 下的项目更新到 .NET Framework 4.8，并跳过多目标或无法识别目标框架的项目。

预览变更：

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf
```

应用变更：

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1
```

## 前置条件与兼容性

- 推荐使用 PowerShell 7。
- 编译需要 .NET Framework 4.8 Developer Pack、当前 .NET SDK 和 MSBuild。
- Windows 是 .NET Framework 4.8 项目的支持构建环境。

重定向只改变编辑器和编译器引用行为，不会改变原始游戏程序集，也不能证明游戏 Mono 支持所有 .NET Framework 4.8 API。要在服务器中运行的代码必须针对 `runtime/7DaysToDieServer_Data/Managed` 下的实际程序集，并在游戏 Mono 进程中验证。

## 检查

```powershell
pwsh ./scripts/Verify-Reference.ps1
```

该脚本检查项目目标框架和 `.csproj` 中 `HintPath` 指向的文件是否存在。

## 最新版本与 Release 归档

替换当前版本前，先生成完整归档和 SHA-256：

```powershell
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4
```

将以下文件上传到同名 Release tag：

```text
dist/7dtd-reference-v3.0.1-b4.zip
dist/7dtd-reference-v3.0.1-b4.zip.sha256
```

更新新版本时，替换根目录的 `server-decompiled/` 和 `runtime/`，更新 `metadata.json`，执行校验后提交并推送 `main`。不要在 Git 中保留旧版本目录或 tag；历史版本通过 Release 资产保存。

普通提交删除文件后，旧文件仍会存在于 Git 历史中。转换为本策略时需要一次性重写历史，并在推送重写后的 `main` 前备份原仓库。远程更新应使用 `--force-with-lease`，并提前通知所有贡献者。

## 维护脚本

### Set-DecompiledTargetFramework.ps1

将根目录 `server-decompiled/` 下的项目重定向到 .NET Framework 4.8。SDK 风格项目使用 `net48`，旧式项目使用 `v4.8`。多目标项目和无法识别目标框架的项目会跳过并给出警告。脚本具有幂等性。

```powershell
# 仅预览，不修改文件
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf

# 应用变更
pwsh ./scripts/Set-DecompiledTargetFramework.ps1

# 指定其他参考根目录
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -ReferenceRoot C:/refs/7dtd
```

### Verify-Reference.ps1

校验当前根目录参考资料。脚本检查 `metadata.json`、根目录 `server-decompiled/`、项目目标框架，以及所有项目 `HintPath` 指向的文件。校验失败时打印错误并以退出码 1 结束。

```powershell
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/Verify-Reference.ps1 -ReferenceRoot C:/refs/7dtd
```

### New-ReferenceRelease.ps1

为当前参考资料生成 ZIP 归档和 SHA-256 校验文件。归档包含 `server-decompiled/`、`runtime/`、`assets/`、`metadata.json` 以及中英文 README。`-Version` 必须与 `metadata.json.gameVersion` 一致。

```powershell
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4 -OutputDirectory ./dist
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4 -WhatIf
```

默认输出为 `dist/7dtd-reference-<version>.zip` 及对应的 `.sha256` 文件。将两个文件上传到同名 GitHub Release：

```powershell
gh release create v3.0.1-b4 `
  ./dist/7dtd-reference-v3.0.1-b4.zip `
  ./dist/7dtd-reference-v3.0.1-b4.zip.sha256 `
  --title '7DTD Reference v3.0.1-b4'
```

### 推荐更新流程

1. 对当前版本执行 `Verify-Reference.ps1`。
2. 执行 `New-ReferenceRelease.ps1`，将生成的两个文件上传到新的 Release。
3. 用新构建替换根目录的 `server-decompiled/` 和 `runtime/`。
4. 更新 `metadata.json` 中的版本和 Release 校验值。
5. 再次执行 `Verify-Reference.ps1`。
6. 提交并推送 `main`。

### runtime 清单与更新脚本

`runtime.manifest.json` 是运行时相对路径的允许清单。它防止下载的服务端压缩包中存在、但当前参考仓库未跟踪的多余文件进入仓库。

如果新游戏版本的官方文件集合发生变化，在审核后重新生成清单：

```powershell
pwsh ./scripts/New-RuntimeManifest.ps1
```

从本地目录或 SMB 路径复制下载好的服务端运行时。脚本会先要求确认当前版本已经归档到 Release，只复制清单允许的目录和根文件，并以镜像方式同步目标目录，删除旧文件。只有在你已手动完成 Release 检查时才使用 `-SkipReleasePrompt`。

```powershell
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot D:/downloads/7dtd
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot '\\nas\games\7dtd' -SkipReleasePrompt
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot D:/downloads/7dtd -WhatIf
```

提交 `runtime.manifest.json` 前请审核变更。如果新构建新增或删除了运行时文件，请重新生成清单、检查差异，复制完成后再执行 `Verify-Reference.ps1`。

### Update-Reference.ps1 与 `.env`

`Update-Reference.ps1` 是统一的本地更新入口。它从不纳入 Git 的 `.env` 读取 `SEVEN_DAYS_SERVER_ROOT` 和 `DNSPY_CONSOLE_PATH`，确认当前版本已经归档，按清单更新 runtime，调用 `decompile.manifest.json` 中列出的程序集运行 dnSpy.Console，替换生成的 `server-decompiled/`，更新 `metadata.json` 并执行校验。脚本不会自动提交或推送 Git。

```powershell
Copy-Item .env.example .env
# 编辑 .env 后先预览
pwsh ./scripts/Update-Reference.ps1 -Version v3.1.0-b1 -GameBuild '3.1.0 build 1' -WhatIf

# 正式更新
pwsh ./scripts/Update-Reference.ps1 -Version v3.1.0-b1 -GameBuild '3.1.0 build 1'
```

审核生成的源码和元数据后，再按前文命令创建并上传 Release，最后提交并推送。

`Get-ServerVersion.ps1` 从最新的 `output_log*.txt` 中读取类似 `Version: V 3.2.0 (b9)` 的日志行。省略 `Update-Reference.ps1` 的 `-Version` 时会自动执行探测；如果没有可用日志，请显式传入版本号。

使用 SMB 路径时，请传入完整且加引号的 UNC 路径，并先确认共享目录可读：

```powershell
pwsh ./scripts/Get-ServerVersion.ps1 -ServerRoot '\\server\share\7dtd'
```
