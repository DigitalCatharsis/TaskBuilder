Write-Host "Введите число для выбора файла для кодировки" -ForegroundColor Yellow
Write-Host "1 - Формальные 2 - Внутренние 3 - Team3 4 - Team4 5 - Team5 6 - Team6 7 - Team7 8 - Team8 0 - Указать путь до документа вручную" -ForegroundColor green

$path = $null;
$boool = $false;

While ($boool -eq $false)
{    
    $input = Read-Host;
    Switch ($input)
    {
        1 {if (Test-Path ($path = "$PSScriptRoot\Формальные.csv")) {$path = "$PSScriptRoot\Формальные.csv"; $boool = $true} else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        2 {if (Test-Path ($path = "$PSScriptRoot\Внутренние.csv")) { $boool = $true}else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        3 {if (Test-Path ($path = "$PSScriptRoot\Team3.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        4 {if (Test-Path ($path = "$PSScriptRoot\Team4.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        5 {if (Test-Path ($path = "$PSScriptRoot\Team5.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        6 {if (Test-Path ($path = "$PSScriptRoot\Team6.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        7 {if (Test-Path ($path = "$PSScriptRoot\Team7.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        8 {if (Test-Path ($path = "$PSScriptRoot\Team8.csv")) { $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
        0 {
            Write-Host "Введите путь до файла, напр C:\temp\Myfile.csv";
            $input = Read-Host;
            if (Test-Path $input) {$path = $input; $boool = $true }else {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}}
          
        Default {write-host "Данное значение не найдено, попробуйте еще раз или убедитесь, что файл существует"}
    }
}
$data = Import-Csv $path -Delimiter ';' -Encoding Default
$data | Export-Csv -Path $path -Encoding UTF8 -NoTypeInformation -Delimiter ";"

Write-host ("Программа закончила работу");
Read-Host