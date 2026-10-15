# Security Policy

## Supported Versions

Only the latest release receives security updates.

| Version | Supported |
|---------|-----------|
| Latest release | Yes |
| Older releases | No |

## Reporting a Vulnerability

**Do not report security vulnerabilities through public GitHub issues.**

Use [GitHub's private vulnerability reporting](https://github.com/baldbeardedbuilder/WalkThisWay/security/advisories/new)
to send a report. Include a description, steps to reproduce, the affected
version, potential impact, and a suggested fix if you have one.

Remove unrelated personal information from screenshots and diagnostics,
especially window titles and executable paths.

## Response Timeline

We aim to acknowledge reports within 48 hours and provide an initial assessment
within one week. Fix timing depends on severity and complexity.

## Scope

Walk This Way runs locally and uses Windows APIs to list, switch, and close
windows and end processes. Security concerns include unintended window or
process actions, exposure of window or process information, code injection,
and dependency vulnerabilities.

For details about local data handling, see the [Privacy Policy](PRIVACY_POLICY.md).
