Param(
    [string]
    $Repo,

    [string]
    $SHA,

    [string]
    $File
)

$SourceTreeFile = $Repo + '/.git/sourcetreeconfig'
[xml]$Xml = Get-Content $SourceTreeFile

$RemoteProject = $Xml.RepositoryCustomSettings.RemoteProjectLinks.RepositoryRemoteProjectLink
$RemoteUrl = $RemoteProject.BaseUrl + '/' + $RemoteProject.Identifier

start ($RemoteUrl + '/commit/' + $SHA + '/' + $File)
