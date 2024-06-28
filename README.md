# Expectativas De Mercado

Este repositório contém o projeto **Expectativas De Mercado**.

## Descrição

O projeto **Expectativas De Mercado** é uma aplicação que permite aos usuários verificar diversas taxas de mercado, como IPCA, IGP-M e Selic, utilizando dados fornecidos pelo Banco Central. A aplicação também permite filtrar esses dados por data e exportá-los para CSV.

## Tecnologias

O projeto é baseado em C#, utiliza WPF (Windows Presentation Foundation) para a interface gráfica, e segue a arquitetura MVVM (Model-View-ViewModel).

## Funcionalidades

- Verificar as taxas do mercado como IPCA, IGP-M e Selic
- Verificar os dados dessas taxas fornecidos pelo Banco Central
- Filtrar os dados por data
- Exportar dados para CSV

## Em Andamento

- Salvar dados no Banco SQL Server
- Mostrar gráficos

## Pré-requisitos

Antes de instalar e executar o projeto, é necessário ter o .NET 6 instalado na sua máquina. Você pode baixar e instalar o .NET 6 a partir do [site oficial da Microsoft](https://dotnet.microsoft.com/download/dotnet/6.0).

## Instalação

Para instalar e executar o projeto, siga as instruções abaixo:

1. **Instale o .NET 6:**
   - Acesse o [site oficial do .NET](https://dotnet.microsoft.com/download/dotnet/6.0).
   - Baixe e instale o SDK do .NET 6 para o seu sistema operacional.
   - Verifique a instalação executando `dotnet --version` no terminal/cmd, que deve retornar a versão instalada do .NET 6.

2. **Faça o download do release mais recente:**
   - [Baixe o arquivo ZIP do release](https://github.com/giovannielbaruqui/ExpectativasDeMercado/releases/download/projeto/ExpectativasDeMercado-executavel.zip).
   - Extraia o arquivo ZIP para o diretório desejado.

3. **Execute a aplicação:**
   - Navegue até o diretório onde você extraiu os arquivos.
   - Execute o arquivo `ExpectativasDeMercado.exe`.

## Alterações no Código

Para fazer alterações no código do projeto, siga as instruções abaixo:

1. **Use a IDE de sua preferência:**
   - Recomendo o uso do [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/).

2. **Clone o repositório:**
   - Abra o terminal/cmd e clone o repositório usando o comando:
     ```bash
     git clone https://github.com/giovannielbaruqui/ExpectativasDeMercado.git
     ```

3. **Abra o projeto na IDE:**
   - Abra a IDE de sua escolha.
   - Abra o diretório do projeto clonado na IDE.

4. **Instale as dependências:**
   - No terminal/cmd, navegue até o diretório do projeto.
   - Execute o comando `dotnet restore` para instalar as dependências necessárias.

5. **Execute o projeto:**
   - Na IDE, execute o projeto utilizando as ferramentas integradas da IDE, ou no terminal/cmd, execute o comando `dotnet run`.

## Contato

Para mais informações, entre em contato:

- Nome: Giovanni El Baruqui
- Email: [giovanni.elbaruqui@gmail.com](mailto:giovanni.elbaruqui@gmail.com)
