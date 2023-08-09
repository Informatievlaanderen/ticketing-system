# Ticketing System [![Build Status](https://github.com/Informatievlaanderen/ticketing-system/workflows/Build/badge.svg)](https://github.com/Informatievlaanderen/ticketing-system/actions)

## NuGet
https://www.nuget.org/packages?q=Be.Vlaanderen.Basisregisters.ticketing

## Components
- abstractions
- service
- inmemory implementation
- marten implementation

## Development

#### NpmInstall

Run an `npm install` to setup Commitizen and Semantic Release.

#### DotNetCli

Checks if the requested .NET Core SDK and runtime version defined in `global.json` are available.
We are pedantic about these being the exact versions to have identical builds everywhere.

#### Clean

Make sure we have a clean build directory to start with.

#### Restore

Restore dependencies for `debian.8-x64` and `win10-x64` using dotnet restore and Paket.

#### Build

Builds the solution in Release mode with the .NET Core SDK and runtime specified in `global.json`
It builds it platform-neutral, `debian.8-x64` and `win10-x64` version.

#### Test

Runs `dotnet test` against the test projects.
## License

[European Union Public Licence (EUPL)](https://joinup.ec.europa.eu/news/understanding-eupl-v12)

The new version 1.2 of the European Union Public Licence (EUPL) is published in the 23 EU languages in the EU Official Journal: [Commission Implementing Decision (EU) 2017/863 of 18 May 2017 updating the open source software licence EUPL to further facilitate the sharing and reuse of software developed by public administrations](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=uriserv:OJ.L_.2017.128.01.0059.01.ENG&toc=OJ:L:2017:128:FULL) ([OJ 19/05/2017 L128 p. 59–64](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=uriserv:OJ.L_.2017.128.01.0059.01.ENG&toc=OJ:L:2017:128:FULL)).

## Credits

### Languages & Frameworks

* [.NET Core](https://github.com/Microsoft/dotnet/blob/master/LICENSE) - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core Runtime](https://github.com/dotnet/coreclr/blob/master/LICENSE.TXT) - _CoreCLR is the runtime for .NET Core. It includes the garbage collector, JIT compiler, primitive data types and low-level classes._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core APIs](https://github.com/dotnet/corefx/blob/master/LICENSE.TXT) - _CoreFX is the foundational class libraries for .NET Core. It includes types for collections, file systems, console, JSON, XML, async and many others._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core SDK](https://github.com/dotnet/sdk/blob/master/LICENSE.TXT) - _Core functionality needed to create .NET Core projects, that is shared between Visual Studio and CLI._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core Docker](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE) - _Base Docker images for working with .NET Core and the .NET Core Tools._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Standard definition](https://github.com/dotnet/standard/blob/master/LICENSE.TXT) - _The principles and definition of the .NET Standard._ - [MIT](https://choosealicense.com/licenses/mit/)
* [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore/blob/master/LICENSE.txt) - _Entity Framework Core is a lightweight and extensible version of the popular Entity Framework data access technology._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)
* [Roslyn and C#](https://github.com/dotnet/roslyn/blob/master/License.txt) - _The Roslyn .NET compiler provides C# and Visual Basic languages with rich code analysis APIs._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)
* [F#](https://github.com/fsharp/fsharp/blob/master/LICENSE) - _The F# Compiler, Core Library & Tools_ - [MIT](https://choosealicense.com/licenses/mit/)
* [F# and .NET Core](https://github.com/dotnet/netcorecli-fsc/blob/master/LICENSE) - _F# and .NET Core SDK working together._ - [MIT](https://choosealicense.com/licenses/mit/)
* [ASP.NET Core framework](https://github.com/aspnet/AspNetCore/blob/master/LICENSE.txt) - _ASP.NET Core is a cross-platform .NET framework for building modern cloud-based web applications on Windows, Mac, or Linux._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)

### Libraries

* [Paket](https://fsprojects.github.io/Paket/license.html) - _A dependency manager for .NET with support for NuGet packages and Git repositories._ - [MIT](https://choosealicense.com/licenses/mit/)
* [FAKE](https://github.com/fsharp/FAKE/blob/release/next/License.txt) - _"FAKE - F# Make" is a cross platform build automation system._ - [MIT](https://choosealicense.com/licenses/mit/)
* [xUnit](https://github.com/xunit/xunit/blob/master/license.txt) - _xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)
* [DataDog](https://github.com/DataDog/dd-trace-dotnet/blob/master/LICENSE) - _.NET Library for Datadog APM._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)
* [FluentAssertions](https://github.com/fluentassertions/fluentassertions/blob/master/LICENSE) - _Fluent API for asserting the results of unit tests._ - [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)
* [Marten](https://github.com/JasperFx/marten/blob/master/LICENSE) - _.NET Transactional Document DB and Event Store on PostgreSQL._ - [MIT](https://choosealicense.com/licenses/mit/)
* [Moq](https://github.com/moq/moq/blob/main/License.txt) - _The most popular and friendly mocking framework for .NET._ - [BSD](https://choosealicense.com/licenses/bsd-3-clause/)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md) - _Json.NET is a popular high-performance JSON framework for .NET._ - [MIT](https://choosealicense.com/licenses/mit/)
* [SwashBuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/LICENSE) - _Swagger tools for documenting API's built on ASP.NET Core._ - [MIT](https://choosealicense.com/licenses/mit/)

### Tooling

* [npm](https://github.com/npm/cli/blob/latest/LICENSE) - _A package manager for JavaScript._ - [Artistic License 2.0](https://choosealicense.com/licenses/artistic-2.0/)
* [semantic-release](https://github.com/semantic-release/semantic-release/blob/master/LICENSE) - _Fully automated version management and package publishing._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/changelog](https://github.com/semantic-release/changelog/blob/master/LICENSE) - _Semantic-release plugin to create or update a changelog file._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/commit-analyzer](https://github.com/semantic-release/commit-analyzer/blob/master/LICENSE) - _Semantic-release plugin to analyze commits with conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/exec](https://github.com/semantic-release/exec/blob/master/LICENSE) - _Semantic-release plugin to execute custom shell commands._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/git](https://github.com/semantic-release/git/blob/master/LICENSE) - _Semantic-release plugin to commit release assets to the project's git repository._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/npm](https://github.com/semantic-release/npm/blob/master/LICENSE) - _Semantic-release plugin to publish a npm package._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/github](https://github.com/semantic-release/github/blob/master/LICENSE) - _Semantic-release plugin to publish a GitHub release._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/release-notes-generator](https://github.com/semantic-release/release-notes-generator/blob/master/LICENSE) - _Semantic-release plugin to generate changelog content with conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitlint](https://github.com/marionebl/commitlint/blob/master/license.md) - _Lint commit messages._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitizen/cz-cli](https://github.com/commitizen/cz-cli/blob/master/LICENSE) - _The commitizen command line utility._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitizen/cz-conventional-changelog](https://github.com/commitizen/cz-conventional-changelog/blob/master/LICENSE) _A commitizen adapter for the angular preset of conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)

### Flemish Government Libraries

* [Be.Vlaanderen.Basisregisters.Build.Pipeline](https://github.com/informatievlaanderen/build-pipeline/blob/main/LICENSE) - _Contains generic files for all Basisregisters Vlaanderen pipelines._ - [MIT](https://choosealicense.com/licenses/mit/)
* [Be.Vlaanderen.Basisregisters.DockerUtilities](https://github.com/Informatievlaanderen/basisregisters-docker-utilities/blob/main/LICENSE) - _Docker-compose utilities for C#.._ - [MIT](https://choosealicense.com/licenses/mit/)
