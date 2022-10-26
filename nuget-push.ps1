# 定义参数
Param(
    # Nuget APIKey
    [string] $apikey
)
$apikey = $apikey.Replace("--apikey=", "");
if ($apikey -eq $null -or $apikey -eq "")
{
    Write-Error "参数apikey不能为空";
    return;
}


Write-Warning "正在发布 nupkgs 目录下的 Nuget 包......";

# 查找 .\nupkgs 下所有目录
cd .\nupkgs;
$project_nupkgs = Get-ChildItem -Filter *.nupkg;

# 遍历所有 *.nupkg 文件
for ($i = 0; $i -le $project_nupkgs.Length - 1; $i++){
    $item = $project_nupkgs[$i];

    $nupkg = $item.FullName;
    $snupkg = $nupkg.Replace(".nupkg", ".snupkg");

    Write-Output "-----------------";
    $nupkg;

    # 发布到 nuget.org 平台
    dotnet nuget push $nupkg --skip-duplicate --api-key $apikey --source https://api.nuget.org/v3/index.json;
    dotnet nuget push $snupkg --skip-duplicate --api-key $apikey --source https://api.nuget.org/v3/index.json;

    Write-Output "-----------------";
}

# 回到项目根目录
cd ../../;

Write-Warning "发布成功";