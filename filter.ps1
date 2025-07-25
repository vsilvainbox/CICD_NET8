# Configuración
$searchPattern = "^(?!.*filedelete).*(SELECT|INSERT|UPDATE|DELETE|EXEC|EXECUTE).*[&].*['""]\s*[&]"
$extensions = @("*.vb", "*.cs", "*.bas", "*.cls", "*.frm")
$outputFile = "sql-vulnerabilities-$(Get-Date -Format 'yyyy-MM-dd-HHmm').csv"

# Array para almacenar resultados
$results = @()

foreach ($ext in $extensions) {
    Get-ChildItem -Recurse -Include $ext | ForEach-Object {
        $filePath = $_.FullName
        $relativePath = $_.FullName.Replace((Get-Location).Path + "\", "")
        
        # Leer el archivo para extraer funciones
        $fileContent = Get-Content $filePath
        $currentFunction = "Global"
        
        for ($i = 0; $i -lt $fileContent.Length; $i++) {
            $line = $fileContent[$i]
            $lineNumber = $i + 1
            
            # Detectar funciones/subs en VB6
            if ($line -match "^\s*(Private|Public|Friend)?\s*(Sub|Function)\s+(\w+)") {
                $currentFunction = $matches[3]
            }
            
            # Buscar patrón SQL vulnerable
            if ($line -match $searchPattern -and $line -notmatch "filedelete") {
                $result = [PSCustomObject]@{
                    Archivo = $relativePath
                    Funcion = $currentFunction
                    LineaNumero = $lineNumber
                    CodigoLinea = $line.Trim()
                    ArchivoCompleto = $filePath
                    FechaAnalisis = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
                }
                $results += $result
                
                Write-Host "${relativePath}:${lineNumber} - $($line.Trim())"
            }
        }
    }
}

# Exportar a CSV
if ($results.Count -gt 0) {
    $results | Export-Csv -Path $outputFile -NoTypeInformation -Encoding UTF8
} else {
    Write-Host "`nOK"
}