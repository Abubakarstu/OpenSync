# Contributing

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/yourusername/OpenSync.git`
3. Create a feature branch: `git checkout -b feature/my-feature`

## Development Setup

1. Install .NET 8 SDK
2. Run `dotnet restore` to restore dependencies
3. Run `dotnet build` to verify the build
4. Run `dotnet test` to run all tests

## CI/CD Pipeline

The repository uses GitHub Actions for automated builds and releases:

| Workflow | Trigger | Description |
|----------|---------|-------------|
| `ci.yml` | PRs + pushes to `main` | Build, test, lint |
| `release.yml` | Tags `v*` | Build, test, pack, publish to NuGet.org & GitHub Packages |

## Code Style

- Follow .NET coding conventions
- Use Clean Architecture patterns
- Write unit tests for all new code
- Keep methods focused and small (ideally <30 lines)

## Pull Request Process

1. Update documentation if needed
2. Add tests for any new functionality
3. Ensure all tests pass
4. Request review from maintainers

## NuGet Package Publishing

The single `OpenSync.AspNetCore` package bundles all 4 source projects:

```bash
# Pack
dotnet pack nupkgs/OpenSync.AspNetCore/OpenSync.AspNetCore.csproj -c Release -o nupkgs/output

# Push to NuGet.org
dotnet nuget push nupkgs/output/OpenSync.AspNetCore.1.0.0.nupkg --source "https://api.nuget.org/v3/index.json" --api-key <key>
```

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
