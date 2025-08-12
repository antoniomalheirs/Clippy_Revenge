# Clippy Revenge 👻

[![CSharp](https://img.shields.io/badge/Linguagem-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/csharp/)
[![Plataforma](https://img.shields.io/badge/Plataforma-Windows-0078D6?style=for-the-badge&logo=windows)](https://www.microsoft.com/pt-br/windows)
[![Licença](https://img.shields.io/badge/Licença-MIT-green.svg?style=for-the-badge)](LICENSE)

**A vingança nunca foi tão... prestativa?**

`Clippy Revenge` é uma aplicação travessa em C# que se esconde silenciosamente em segundo plano, agindo como um "poltergeist" digital. Ele foi projetado para pregar peças inofensivas, exibindo mensagens aleatórias, controlando o mouse, simulando digitação e, claro, trazendo de volta o nosso querido assistente Clippy para assombrar a tela do usuário.

Construído com WinForms e um toque de magia Win32 (P/Invoke), este projeto é um ótimo exemplo de como interagir com o sistema operacional em um nível mais profundo.

---

## ✨ Funcionalidades (As Assombrações)

O aplicativo executa um ciclo de "pegadinhas" (traps) em intervalos aleatórios para criar uma experiência imprevisível e divertida.

| Trap | Descrição |
| :--- | :--- |
| 💬 **Message Trap** | Exibe mensagens de texto aleatórias e inquietantes em locais variados da tela. As mensagens desaparecem sozinhas após alguns segundos. |
| 🖱️ **Mouse Trap** | Assume o controle do cursor do mouse, fazendo-o se mover erraticamente (jiggle) ou seguir um caminho pré-definido para pontos aleatórios da tela. |
| 📎 **Clippy Trap** | O lendário Clippy aparece em um canto da tela, se move e exibe uma de suas "dicas úteis". Clicar nele para dispensá-lo trava temporariamente o cursor na mensagem! |
| ⌨️ **Keyboard Trap** | Simula uma digitação lenta e "humana", escrevendo frases aleatórias na janela ativa, como se um fantasma estivesse usando o teclado. |
|  stealth **Modo Furtivo e Persistente** | O aplicativo roda completamente em segundo plano, sem ícone na barra de tarefas ou janela visível. Se o processo for finalizado, ele **ressurge automaticamente** após 10 segundos para continuar a assombração. |

## ⚙️ Como Funciona: Um Mergulho Técnico

Este projeto utiliza algumas técnicas interessantes para alcançar seu comportamento furtivo e interativo:

-   **Janela de Overlay Transparente:** O `Form1` principal é uma janela sem bordas, que cobre toda a tela, mas é configurada com os estilos `WS_EX_TRANSPARENT` e `WS_EX_LAYERED` via P/Invoke. Isso a torna "clicável através", permitindo que o usuário interaja normalmente com as janelas abaixo, enquanto o nosso código roda por cima de tudo.
-   **P/Invoke e API do Windows:** Funções da `user32.dll` como `SetWindowLong` e `RegisterHotKey` são usadas para:
    -   Tornar a janela invisível para eventos de mouse.
    -   Remover o ícone da barra de tarefas (`WS_EX_TOOLWINDOW`).
    -   Registrar uma hotkey global (`Ctrl+Alt+Shift+K`) como um "kill switch" de segurança.
-   **Contexto de Aplicação Persistente:** Em vez de rodar o `Form1` diretamente, o `Program.cs` usa um `StealthAppContext` customizado. Esse contexto garante que, se o `Form1` for fechado, ele será recriado após um delay, tornando a "pegadinha" mais persistente.
-   **Timers para Aleatoriedade:** Cada "trap" é controlada por um ou mais `System.Windows.Forms.Timer`, que são ativados em intervalos aleatórios para que as ações pareçam imprevisíveis.

---

## 🚀 Como Usar

### Pré-requisitos

-   Windows 10 ou superior
-   .NET Framework 4.7.2 ou superior
-   Visual Studio

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/SEU-USUARIO/Clippys_Revenge.git](https://github.com/SEU-USUARIO/Clippys_Revenge.git)
    ```
2.  Abra o arquivo da solução `WinSystemHelperF.sln` no Visual Studio.
3.  Compile o projeto (Build > Build Solution).
4.  Execute o aplicativo a partir do Visual Studio (F5) ou rodando o `.exe` gerado na pasta `bin/Debug`.
5.  O programa começará a rodar em segundo plano. Agora é só esperar a mágica acontecer!

> **IMPORTANTE:** O aplicativo começará a agir imediatamente. Use-o em seu próprio computador primeiro para ver os efeitos.

## 🛑 **COMO PARAR A APLICAÇÃO**

Para encerrar completamente o programa (incluindo o mecanismo de respawn), pressione a seguinte combinação de teclas:

**`Ctrl + Alt + Shift + K`**

---

## 🔧 Personalização

É muito fácil adaptar as "pegadinhas". Todas as configurações principais estão no início do arquivo `Form1.cs`.

#### Alterar as Mensagens

Você pode editar as listas de frases para cada trap:

```csharp
// --- Form1.cs ---

// Mensagens que aparecem na tela
private readonly List<string> trollMessages = new List<string> {
    "Sua nova mensagem aqui",
    "Outra mensagem assustadora..."
};

// Mensagens do Clippy
private readonly List<string> clippyMessages = new List<string> {
    "Parece que você está tentando usar o computador...",
    "Posso te ajudar a bagunçar tudo?"
};

// Frases que serão digitadas
private readonly List<string> keyboardTrapPhrases = new List<string>
{
    "ajuda estou preso no pc",
    "eles estao me observando"
};
```

#### Ajustar os Timers
Você pode editar as listas de frases para cada trap:

Você pode alterar a frequência e a duração das traps ajustando os valores `random.Next(min, max)` nos métodos de inicialização (`Initialize...Trap`). Os valores estão em milissegundos.

**Exemplo (Keyboard Trap):**
```csharp
// --- Form1.cs, dentro de initializeKeyboardTrap() ---

// Ativa a trap de digitação a cada 20-30 segundos
keyboardTrapActivation.Interval = random.Next(20000, 30000);

// ...

// --- Dentro de OnKeyboardTrapActivateTick() ---

// Define a velocidade de digitação (150-300 ms por caractere)
keyboardTypingTimer.Interval = random.Next(150, 300);
```

## ⚠️ Aviso de Responsabilidade

Este software foi criado para fins educacionais e de entretenimento (brincadeiras leves e inofensivas entre amigos).

1. NÃO utilize este programa em ambientes de trabalho, computadores públicos ou em sistemas críticos.

2. NÃO distribua este software com intenções maliciosas.

O autor não se responsabiliza por qualquer uso indevido ou consequências negativas causadas por este aplicativo. Use por sua conta e risco.

 ## 📄 Licença
 Este projeto é distribuído sob a Licença MIT. Veja o arquivo LICENSE para mais detalhes. Você é livre para usar, modificar e redistribuir.

 "Clippy morreu... ou será que não?" 👻