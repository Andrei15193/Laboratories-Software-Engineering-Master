[System.Collections.Generic.List[System.String]] $cultureNames = New-Object 'System.Collections.Generic.List[System.String]'
[System.Collections.Generic.HashSet[System.String]] $addedCurrencyIsoCodes = New-Object 'System.Collections.Generic.HashSet[System.String]' ([System.StringComparer]::OrdinalIgnoreCase)

foreach ($cultureInfo in [System.Globalization.CultureInfo]::GetCultures([System.Globalization.CultureTypes]::AllCultures))
{
    try
    {
        [void](New-Object 'System.Globalization.RegionInfo' $cultureInfo.Name);
        if ($addedCurrencyIsoCodes.Add((New-Object 'System.Globalization.RegionInfo' $cultureInfo.Name).ISOCurrencySymbol))
        {
            [void]$cultureNames.Add($cultureInfo.Name)
        }
    }
    catch
    {
    }
}

foreach ($cultureName in $cultureNames)
{
    Write-Output "yield return new Currency(new RegionInfo(""$cultureName""));"
}