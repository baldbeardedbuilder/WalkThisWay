# Privacy Policy

**Walk This Way for PowerToys Command Palette**

*Last updated: October 15, 2026*

## Overview

Walk This Way lets you find and switch between open windows on your device.
This policy covers the extension maintained by Bald Bearded Builder LLC,
not Windows, PowerToys, or GitHub.

## Data Used on Your Device

To show your open windows and respond to your commands, the extension reads
window titles, application icons, process names and IDs, executable paths,
and window and virtual desktop information through Windows APIs. It also
uses the search text you enter in Command Palette to filter results locally.

Window titles and executable paths can contain personal information, such as
document names or your Windows username. The extension processes this
information locally and passes results to Command Palette for display.
Window and process information is cached in memory while the extension runs.
The extension does not save a history of your searches or open windows to disk.

## Data Collection and Sharing

The extension does not send your window information, search text, or settings
to the developer or third-party services. It does not include usage analytics,
telemetry, advertising, or tracking.

## Local Storage

Your preferences, including whether the dock shows only windows on the current
desktop, are saved in a local `settings.json` file in the extension's
Command Palette settings directory. These settings are not uploaded by the
extension.

## Third-Party Services

Walk This Way does not call external APIs or require an online account to
search and manage windows. Windows and PowerToys may handle data separately
under their own policies.

If you choose to open an issue or submit a report on GitHub, GitHub processes
that information under its [privacy statement](https://docs.github.com/en/site-policy/privacy-policies/github-privacy-statement).
Public issues and their attachments are visible to others. Remove private
window titles, file paths, and other personal details before sharing screenshots
or diagnostic information.

## Changes to This Policy

Updates will be posted in this file with a revised date.

## Contact

For questions about this policy, open an issue in the
[Walk This Way repository](https://github.com/baldbeardedbuilder/WalkThisWay/issues).
For security concerns, follow the private reporting instructions in our
[Security Policy](SECURITY.md).
