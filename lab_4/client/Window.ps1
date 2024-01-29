[System.Reflection.Assembly]::LoadWithPartialName("PresentationFramework") | Out-Null

function Import-Xaml {
    [xml]$xaml = Get-Content -Path $PSScriptRoot\WpfWindow.xaml
    $manager = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xaml.NameTable
    $manager.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
    $xamlReader = New-Object System.Xml.XmlNodeReader $xaml
    [Windows.Markup.XamlReader]::Load($xamlReader)
}

$Window = Import-Xaml
$Button = $Window.FindName("MyButton")
$Button.add_Click({Write-Host "Hell"})

$Window.ShowDialog()