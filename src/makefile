.PHONY: build
.PHONY: clean
.PHONY: run
.PHONY: publish

build:
				# dotnet build --verbosity minimal
				dotnet build --no-restore /clp:ErrorsOnly

clean:
				dotnet clean -nologo -consoleLoggerParameters:NoSummary -verbosity:quiet

run: clean build
				dotnet run --project ./AspNetCore.Filters.WebApi/AspNetCore.Filters.WebApi.csproj --launch-profile AspNetCore.Filters.WebApi --property:Configuration=Debug

publish:
				# MSBuild.exe -clp:ErrorsOnly -nologo -m /property:Configuration=Release
				dotnet build --no-restore --configuration Release /clp:ErrorsOnly
				dotnet publish -c release -r centos.7-x64 --self-contained
				# dotnet publish -c release -r ubuntu.16.04-x64 --self-contained
				# The binary will be in "bin/Release/netcoreapp3.1/ubuntu.16.04-x64/publish"
