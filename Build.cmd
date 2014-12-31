msbuild SettingsAPISample\SettingsAPISample.csproj /p:webpublishmethod=filesystem;PublishUrl=..\Artifacts\SettingsAPISample /t:WebFileSystemPublish
nuget pack SettingsAPISample.nuspec -nopackageanalysis
