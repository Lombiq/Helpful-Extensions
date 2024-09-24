# Lombiq Helpful Extensions for Orchard Core

[![Lombiq.HelpfulExtensions NuGet](https://img.shields.io/nuget/v/Lombiq.HelpfulExtensions?label=Lombiq.HelpfulExtensions)](https://www.nuget.org/packages/Lombiq.HelpfulExtensions/) [![Lombiq.HelpfulExtensions UI Test Extensions NuGet](https://img.shields.io/nuget/v/Lombiq.HelpfulExtensions.Tests.UI?label=Lombiq.HelpfulExtensions.Tests.UI)](https://www.nuget.org/packages/Lombiq.HelpfulExtensions.Tests.UI/)

## About

Orchard Core module containing some handy extensions (e.g. useful content types and widgets).

We at [Lombiq](https://lombiq.com/) also used this module for the following projects:

- The new [City of Santa Monica website](https://santamonica.gov/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/helping-the-city-of-santa-monica-with-orchard-core-consulting)).
- The new [Smithsonian Folkways Recordings website](https://folkways.si.edu/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/smithsonian-folkways-recordings-now-upgraded-to-orchard-core)).
- The new [Xero4PowerBI website](https://xero4powerbi.com/) ([see case study](https://dotnest.com/blog/xero4powerbi-website-case-study-migration-to-orchard-core)).<!-- #spell-check-ignore-line -->
- The new [Lombiq website](https://lombiq.com/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/how-we-renewed-and-migrated-lombiq-com-from-orchard-1-to-orchard-core)).
- The new [Ik wil een taart website](https://ikwileentaart.nl/) ([see case study](https://dotnest.com/blog/revamping-ik-wil-een-taart-migrating-an-old-version-of-orchard-core-website-with-custom-theme-and-commerce-logic-to-dotnest)).<!-- #spell-check-ignore-line -->
- The new client portal for [WTW](https://www.wtwco.com/) ([see case study](https://lombiq.com/blog/lombiq-s-journey-with-wtw-s-client-portal)).
- The new [Show Orchard website](https://showorchard.com/) when migrating it from Orchard 1 DotNest to DotNest Core ([see case study](https://dotnest.com/blog/show-orchard-case-study-migrating-an-orchard-1-dotnest-site-to-orchard-core)).<!-- #spell-check-ignore-line -->
- The new [Git-hg Mirror website](https://githgmirror.com/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/git-hg-mirror-is-running-on-orchard-core)).<!-- #spell-check-ignore-line -->
- The new [Hastlayer website](https://hastlayer.com/) when migrating it from Orchard 1 to Orchard Core ([see case study](https://lombiq.com/blog/modernization-and-orchard-core-migration-of-hastlayer-com)).<!-- #spell-check-ignore-line -->

This module is also available on all sites of [DotNest, the Orchard Core SaaS](https://dotnest.com/).

Do you want to quickly try out this project and see it in action? Check it out in our [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) full Orchard Core solution and also see our other useful Orchard Core-related open-source projects!

Note that this module has an Orchard 1 version in the [dev-orchard-1 branch](https://github.com/Lombiq/Helpful-Extensions/tree/dev-orchard-1).

## Extensions

The module consists of the following independent extensions (all in their own features):

- [Code Generation Helpful Extensions](Lombiq.HelpfulExtensions/Extensions/CodeGeneration/Readme.md)
- [Orchard 1 Recipe Migration](Lombiq.HelpfulExtensions/Extensions/OrchardRecipeMigration/Readme.md)
- [Content Sets](Lombiq.HelpfulExtensions/Extensions/ContentSets/Readme.md)
- [Flows Helpful Extensions](Lombiq.HelpfulExtensions/Extensions/Flows/Readme.md)
- [Helpful Widgets](Lombiq.HelpfulExtensions/Extensions/Widgets/Readme.md)
- [Helpful Content Types](Lombiq.HelpfulExtensions/Extensions/ContentTypes/Readme.md)
- [Shape Tracing Helpful Extensions](Lombiq.HelpfulExtensions/Extensions/ShapeTracing/Readme.md)
- [Security Extensions](Lombiq.HelpfulExtensions/Extensions/Security/Readme.md)
- [Emails and Email Templates](Lombiq.HelpfulExtensions/Extensions/Emails/Readme.md)
- [Target blank](Lombiq.HelpfulExtensions/Extensions/TargetBlank/Readme.md)
- [Reset Password activity](Lombiq.HelpfulExtensions/Extensions/Workflows/Readme.md)
- [Trumbowyg code-snippet](Lombiq.HelpfulExtensions/Extensions/Trumbowyg/Readme.md)

### Google Tag

Adds a shape along with Razor and Liquid tag helpers for Google Analytics, using <https://tagmanager.google.com/>.

You can use the `<google-tag property-id="..." cookie-domain="auto">` Razor tag helper in _cshtml_ files or the `{% google_tag property_id: "...", cookie_domain: "auto" %}` parser tag in Liquid.

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions and love letters are warmly welcome. You can send them to us via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
