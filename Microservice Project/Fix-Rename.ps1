# Script: Fix-Rename.ps1
# الهدف: استبدال كلمة catlog بـ catalog في كل ملفات .sln و .csproj

Write-Host "Searching for 'catlog' and replacing with 'catalog' ..."

# دور على كل الملفات المطلوبة
$files = Get-ChildItem -Recurse -Include *.sln, *.csproj

foreach ($file in $files) {
    (Get-Content $file.FullName) -replace 'catlog', 'catalog' | Set-Content $file.FullName
    Write-Host "Updated: $($file.FullName)"
}

Write-Host "Done! Now open the solution again and reload the projects."
