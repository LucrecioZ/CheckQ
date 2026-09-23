<div align="center">

  <img src="Assets/CheckQ_icon_1024.png" alt="Ícone do CheckQ" width="112" />

  <h1>CheckQ</h1>

  <p><strong>Seus processos organizados. Uma etapa de cada vez.</strong></p>
  <p>Crie checklists, acompanhe o progresso e retome suas tarefas em um aplicativo desktop para Windows.</p>

  <p>
    <img src="https://img.shields.io/badge/Plataforma-Windows-0077C8?style=flat-square" alt="Plataforma: Windows" />
    <img src="https://img.shields.io/badge/.NET-10-512BD4?style=flat-square" alt=".NET 10" />
    <img src="https://img.shields.io/badge/Interface-WPF-1E3A56?style=flat-square" alt="Interface: WPF" />
    <img src="https://img.shields.io/badge/Dados-JSON_local-25855A?style=flat-square" alt="Dados: JSON local" />
  </p>

  <p>
    <a href="#funcionalidades">Funcionalidades</a> ·
    <a href="#como-executar">Como executar</a> ·
    <a href="#primeiro-checklist">Primeiro checklist</a> ·
    <a href="#dados-e-backup">Dados e backup</a>
  </p>

</div>

---

## Conheça o CheckQ

O CheckQ transforma processos em listas de etapas que você pode marcar conforme avança. Cada processo reúne um título, um responsável e um sistema, mantendo o contexto do trabalho junto do checklist.

Use, por exemplo, para acompanhar a instalação de um sistema, a configuração de um ambiente ou uma rotina de conferência.

## Funcionalidades

| Recurso | O que você pode fazer |
| :--- | :--- |
| 🗂️ **Processos organizados** | Criar, editar e excluir processos com título, responsável e sistema. |
| ✅ **Checklists editáveis** | Adicionar, editar, excluir e marcar etapas como concluídas. |
| 📊 **Progresso visível** | Acompanhar a quantidade de etapas concluídas e a barra de progresso. |
| 🏷️ **Sistemas reutilizáveis** | Cadastrar um sistema e selecioná-lo em novos processos. |
| 💾 **Salvamento automático** | Manter as alterações dos processos em arquivos JSON no computador. |

## Como executar

### 1. Prepare o ambiente

Você precisa de **Windows**, **SDK do .NET 10** e **Git** para seguir os comandos abaixo. O projeto utiliza WPF.

Confira se as ferramentas estão disponíveis no terminal:

```powershell
dotnet --list-sdks
git --version
```

A lista de SDKs deve incluir uma versão `10.0.x`.

### 2. Baixe o projeto

```powershell
git clone https://github.com/LucrecioZ/CheckQ-.git
cd CheckQ-
```

### 3. Abra o aplicativo

```powershell
dotnet run --project ChecklistInstaller.csproj
```

> **Já está com o projeto aberto?** Execute apenas o comando acima no terminal da pasta que contém `ChecklistInstaller.csproj`.

## Primeiro checklist

1. Na tela inicial, clique em **+ Novo processo**.
2. Informe o **usuário responsável**, digite ou selecione o **sistema** e preencha o **título** do processo. Confirme a criação.
3. Abra o processo na lista da tela inicial.
4. Em **Adicionar etapa**, escreva uma tarefa e clique em **Adicionar**. Repita para as próximas tarefas.
5. Marque as etapas conforme concluir o trabalho e acompanhe o **Progresso** na parte inferior da janela.

As alterações são salvas automaticamente. Ao abrir o aplicativo novamente, seus processos são carregados dos dados locais.

### Exemplo de uso

**Processo:** Preparar estação de trabalho · **Sistema:** Sistema interno

- [x] Conferir os pré-requisitos
- [x] Instalar o sistema
- [ ] Configurar o acesso
- [ ] Validar o funcionamento

*Exemplo ilustrativo de um checklist com 2 de 4 etapas concluídas.*

## Dados e backup

Os dados ficam separados do código-fonte, na pasta de dados locais do usuário do Windows:

```text
%LOCALAPPDATA%\CheckQ
├── sistemas.json          # Lista de sistemas cadastrados
└── processo_<id>.json     # Um arquivo para cada processo e suas etapas
```

Para abrir essa pasta, pressione **Win + R**, cole `%LOCALAPPDATA%\CheckQ` e pressione **Enter**. Ela é criada na primeira execução do aplicativo.

**Para fazer backup:** feche o CheckQ e copie essa pasta para um local de sua preferência. Para restaurar, com o aplicativo fechado, copie os arquivos de volta para a mesma pasta.

Os dados são locais a cada usuário do Windows; o aplicativo não sincroniza os checklists entre computadores.

## Desenvolvimento

O projeto utiliza **C#**, **WPF**, **.NET 10** e **System.Text.Json** para persistência dos dados.

<details>
<summary><strong>Ver a estrutura do projeto</strong></summary>

```text
CheckQ-/
├── Assets/                      # Ícones e identidade visual
├── Models/                      # Modelos de processo e etapa
├── Services/
│   └── JsonService.cs           # Leitura e gravação dos dados locais
├── App.xaml                     # Recursos e estilos compartilhados
├── MainWindow.xaml              # Lista de processos
├── NovoProcessoWindow.xaml      # Criação de processos
├── EditarProcessoWindow.xaml    # Edição de processos
├── ProcessoWindow.xaml          # Checklist e progresso
├── EditarEtapaWindow.xaml       # Edição de etapas
└── ChecklistInstaller.csproj    # Configuração do projeto
```

Os arquivos `.xaml.cs` contêm a lógica das respectivas janelas.

</details>

<details>
<summary><strong>Compilar em modo Release</strong></summary>

Na pasta do projeto, execute:

```powershell
dotnet build ChecklistInstaller.csproj -c Release
```

Os arquivos compilados ficam em `bin/Release/net10.0-windows/`. As pastas `bin/` e `obj/` são ignoradas pelo Git.

</details>

## Ideias e problemas

Encontrou um problema ou tem uma sugestão? [Abra uma issue](https://github.com/LucrecioZ/CheckQ-/issues) com uma descrição. Para erros, inclua os passos para reproduzir e o comportamento esperado.

---

<p align="center">Feito por <a href="https://github.com/LucrecioZ">LucrecioZ</a> · CheckQ</p>
