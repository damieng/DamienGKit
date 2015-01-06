Param(
    [string]
    $Repo
)

$SourceTreeFile = $Repo + '/.git/sourcetreeconfig'
[xml]$Xml = Get-Content $SourceTreeFile

$RemoteProject = $Xml.RepositoryCustomSettings.RemoteProjectLinks.RepositoryRemoteProjectLink
$RemoteUrl = $RemoteProject.BaseUrl + '/' + $RemoteProject.Identifier

start ($RemoteUrl)
