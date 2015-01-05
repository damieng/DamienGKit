Param(
    [string]
    $Repo,

    [string]
    $File
)

$SourceTreeFile = $Repo + '/.git/sourcetreeconfig'
[xml]$Xml = Get-Content $SourceTreeFile

$RemoteProject = $Xml.RepositoryCustomSettings.RemoteProjectLinks.RepositoryRemoteProjectLink
$RemoteUrl = $RemoteProject.BaseUrl + '/' + $RemoteProject.Identifier

start ($RemoteUrl + '/tree/master/' + $File)
