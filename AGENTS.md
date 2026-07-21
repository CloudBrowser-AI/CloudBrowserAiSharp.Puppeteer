# Repository Guidelines

## Project Structure & Module Organization
The core client library lives in `lib/`, with feature-focused folders like `AI/`, `Browser/`, and `Extensions/` hosting extension methods and service helpers for CloudBrowser integrations. The publishable project file is `lib/CloudBrowserAiSharp.Puppeteer.csproj`; solution files are kept alongside for IDE support. Samples that demonstrate typical usage scenarios live under `samples/`, each with its own console project (`LaunchAsync`, `AIQuery`, `PuppetterHelpers`) wired for quick experimentation.

## Build, Test, and Development Commands
Run `dotnet restore lib/CloudBrowserAiSharp.Puppeteer.csproj` before building to pull the CloudBrowser.AI and PuppeteerSharp dependencies. Use `dotnet build lib/CloudBrowserAiSharp.Puppeteer.csproj -c Release` for release parity. Develop against samples with `dotnet run --project samples/LaunchAsync/LaunchAsync.csproj` (swap the project path to explore other scenarios). Package locally via `dotnet pack lib/CloudBrowserAiSharp.Puppeteer.csproj -c Release` to validate the NuGet output.

## Coding Style & Naming Conventions
Stick to standard C# with file-scoped namespaces, 4-space indentation, and async method names ending in `Async`. Group related helpers with regions only when necessary and keep public XML docs concise, mirroring the existing extension methods. Place extension classes under `CloudBrowserAiSharp.Puppeteer.*` and name files after the contained type to align with the current layout.

## Testing Guidelines
There is no dedicated test suite yet; use samples as smoke tests before raising a PR. When adding tests, prefer xUnit and mimic the library folder layout inside a future `tests/` directory. Name test classes `<TypeName>Tests` and methods `MethodName_Should...`. Ensure new API surface is exercised via headless runs against CloudBrowser staging if available.

## Commit & Pull Request Guidelines
History mixes imperative statements (e.g., `PuppetterSharp updated`) with occasional `feat:` prefixes; follow the imperative style and scope-stamp major changes with a short prefix when helpful. Each PR should explain the scenario, highlight library API changes, and list manual validation (commands run, sample executed). Link issues or release tickets, and attach screenshots or logs when behavior changes. Seek review before tagging releases; tags trigger the NuGet publishing workflow in `.github/workflows/upload-nuget.yml`.

## Release & Packaging Notes
Versioning follows semantic tags (`0.0.x`), which activate the GitHub Action that builds, packs, and pushes to NuGet. Verify `GeneratePackageOnBuild` outputs locally before creating a tag, and confirm secrets are set when introducing new pipelines.

## Codex Delivery Defaults

- Prefer the simplest coherent implementation: reuse and simplify existing paths, minimize files, abstractions, dependencies, services, states, and configuration, and do not build speculative extensibility or a framework for one use case.
- Follow the global solo-founder fast path: keep work scoped to the requested outcome, implement routine multi-file changes directly, and treat unrelated findings as follow-ups.
- Routine work needs targeted checks and at most one proportionate final verification on changed bytes; do not add tasklists, approval or evidence packets, recursive reviews, or soak gates unless requested or required by a concrete high-risk change.
- Block delivery only for a direct risk of security exposure, data loss, financial loss, broken rollback, or failure of the requested behavior.
- For auth, cloud, SSH, deployment, databases, payments, customer data, or production sources, identify the exact target and authoritative source first. Preserve narrow access, secret hygiene, backups and rollback when data changes, and explicit authorization for destructive or irreversible actions.
- Never print or repeat OAuth codes, tokens, keys, cookies, private credentials, or customer data in chat or logs; use approved local secret stores and redacted placeholders.
