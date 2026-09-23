# CheckQ

Aplicativo desktop para Windows, desenvolvido em C# com WPF, para organizar processos e suas etapas em checklists.

## Requisitos

- Windows
- SDK do .NET 10

## Executar

Na pasta do projeto:

```powershell
dotnet run --project ChecklistInstaller.csproj
```

## Compilar

```powershell
dotnet build ChecklistInstaller.csproj -c Release
```

## Dados locais

Os processos e a lista de sistemas são salvos em arquivos JSON na pasta `%LOCALAPPDATA%\CheckQ` de cada usuário.
