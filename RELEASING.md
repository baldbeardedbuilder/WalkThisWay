# Releasing Walk This Way

Publishing a GitHub release starts the [release workflow](.github/workflows/release.yml).
It builds and tests the extension, creates signed x64 and ARM64 MSIX packages,
and attaches them to that release.

The current workflow does not submit packages to the Microsoft Store or WinGet.

## Signing Setup

The workflow uses the Azure signing account `bbb-signing` and certificate
profile `BaldBeardedBuilder` at `https://eus.codesigning.azure.net/`.
The Azure service principal needs permission to sign with that profile.

Store its credentials in the repository's Actions secret named `AZURE_CREDS`:

```json
{
  "clientId": "<application ID>",
  "clientSecret": "<client secret>",
  "subscriptionId": "<subscription ID>",
  "tenantId": "<tenant ID>"
}
```

Keep actual credentials in GitHub secrets, never in this repository.
The workflow uses the automatically provided `GITHUB_TOKEN` with
`contents: write` permission to upload release assets.

## Publishing a Release

1. Merge the intended changes into `main` and confirm CI passes.
2. Open [Releases](https://github.com/baldbeardedbuilder/WalkThisWay/releases) and draft a new release.
3. Create a tag from the intended commit using `vMAJOR.MINOR.PATCH`, such as `v1.2.0`.
4. Write release notes and publish the release.
5. Watch the [Release workflow](https://github.com/baldbeardedbuilder/WalkThisWay/actions/workflows/release.yml) and confirm both architecture packages are attached.

The workflow removes the leading `v` and appends `.0` to a three-part version
for the MSIX manifest. For example, `v1.2.0` becomes `1.2.0.0`.
Use numeric versions without prerelease suffixes.

## What the Workflow Does

It builds x64 and ARM64 Release configurations, then runs the x64 tests.
After those pass, it publishes both MSIX packages, signs them with Azure,
and verifies their signatures before uploading them to the GitHub release.

## Troubleshooting

| Failure | What to check |
|---------|---------------|
| Build or tests | Read the failing step's output and reproduce with the commands in [Contributing](CONTRIBUTING.md). |
| Azure login or signing | Check `AZURE_CREDS`, service principal permissions, signing account, and certificate profile. |
| Signature verification | Inspect the signing output and certificate status. Do not distribute packages with invalid signatures. |
| Release upload | Check workflow permissions and whether the packaging step produced both MSIX files. |

A published release can exist before packaging finishes or after a workflow
failure. Confirm the signed assets are available before announcing it.
