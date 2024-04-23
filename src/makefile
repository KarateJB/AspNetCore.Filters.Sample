.PHONY: build
.PHONY: clean
.PHONY: run
.PHONY: publish

build:
				dotnet build --verbosity minimal

clean:
				dotnet clean -nologo -consoleLoggerParameters:NoSummary -verbosity:quiet

run:
				cd src/AspNetCore.Filters.WebApi/; \
				dotnet run --project ./AspNetCore.Filters.WebApi.csproj --property:Configuration=Debug

publish:
				MSBuild.exe -clp:ErrorsOnly -nologo -m /property:Configuration=Release
				dotnet publish -c release -r centos.7-x64 --self-contained
				# dotnet publish -c release -r ubuntu.16.04-x64 --self-contained
				# The binary will be in "bin/Release/netcoreapp3.1/ubuntu.16.04-x64/publish"
