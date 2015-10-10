$cultureNamesByCurrencyIsoCode = New-Object 'System.Collections.Generic.SortedDictionary[System.String, System.String]' ([System.StringComparer]::OrdinalIgnoreCase)

foreach ($cultureInfo in [System.Globalization.CultureInfo]::GetCultures([System.Globalization.CultureTypes]::AllCultures))
{
    try
    {
        [System.Globalization.RegionInfo]$regionInfo = New-Object 'System.Globalization.RegionInfo' $cultureInfo.Name
        if (-Not $cultureNamesByCurrencyIsoCode.ContainsKey($regionInfo.ISOCurrencySymbol))
        {
            [void]$cultureNamesByCurrencyIsoCode.Add($regionInfo.ISOCurrencySymbol, $cultureInfo.Name)
        }
    }
    catch
    {
    }
}

Write-Host "using System.Collections.Generic;"
Write-Host "using System.Globalization;"
Write-Host "using BillPath.Models;"
Write-Host ""
Write-Host "namespace BillPath.Providers"
Write-Host "{"
Write-Host "    public class CurrencyProvider"
Write-Host "    {"
Write-Host "        public IEnumerable<Currency> Currencies { get; } = new[]"
Write-Host "            {"
foreach ($cultureName in [System.Linq.Enumerable]::Take($cultureNamesByCurrencyIsoCode.Values, $cultureNamesByCurrencyIsoCode.Count - 1))
{
    Write-Output "                new Currency(new RegionInfo(""$cultureName"")),"
}
Write-Output "                new Currency(new RegionInfo(""$([System.Linq.Enumerable]::Last($cultureNamesByCurrencyIsoCode.Values))""))"
Write-Host "            };"
Write-Host "    }"
Write-Host "}"