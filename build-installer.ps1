
$version = If (Test-Path env:\APPVEYOR_BUILD_VERSION) {$env:APPVEYOR_BUILD_VERSION} Else {"1.0.0.0"} 
$nuspec = (Resolve-Path ".\src\WifiBegone.Tray\WifiBegone.nuspec").ToString()

$nuget = (Resolve-Path ".\packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe").ToString()
$squirrel = (Resolve-Path ".\packages\squirrel.windows.*\tools\Squirrel.exe").ToString()

& $nuget pack $nuspec -Version $version
& $squirrel --releasify WifiBegone.$version.nupkg   

Start-Sleep -s 2