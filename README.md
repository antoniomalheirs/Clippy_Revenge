# Clippy’s Revenge


[![C#](https://img.shields.io/badge/Code-C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)](https://github.com/antoniomalheirs/Clippys_Revenge)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows-blue?style=flat-square&logo=windows)](https://github.com/antoniomalheirs/Clippys_Revenge)

---

Um app travesso em C# que se esconde silenciosamente em segundo plano, exibindo mensagens aleatórias e perturbadoras na tela — o "poltergeist" digital perfeito para pregar peças inofensivas nos amigos. Construído com WinForms, P/Invoke e um toque de magia Win32.

---

## 🧩 Funcionalidades

- 🎭 Roda em segundo plano, sem janelas visíveis ou notificações.
- 💬 Exibe mensagens aleatórias e inquietantes na tela do usuário.
- 🪟 Utiliza chamadas Win32 para alterar o comportamento da janela.
- ⚙️ Código simples, personalizável e pronto para modificações.

---

## 🛠️ Como usar

### Pré-requisitos

- Windows 10 ou superior
- .NET Framework (ou .NET Core/5+ dependendo da versão usada)
- Visual Studio (ou editor C# compatível)

### Passos

1. Clone o repositório:

```bash
git clone https://github.com/antoniomalheirs/Clippys_Revenge.git
```
2. Abra o projeto WinSystemHelperF.sln no Visual Studio.
3. Compile e execute o projeto.
4. O aplicativo será iniciado e ficará rodando em segundo plano. Aguarde as mensagens começarem a aparecer na tela do alvo


## 🧪 Personalização
Você pode personalizar o comportamento do app modificando o código:
Mensagens exibidas: Edite a lista de strings no código.
Intervalos e delays: Ajuste o temporizador que define quando cada mensagem é exibida.
Estilo visual: Pode alterar o tipo de caixa, tamanho da fonte ou até adicionar sons.
Exemplo (em Form1.cs ou onde as mensagens são geradas):«
```bash
string[] mensagens = {
    "Está tudo bem, certo?",
    "Você está sendo observado...",
    "Algo se moveu atrás de você.",
    "Essa janela apareceu sozinha?"
};
```
## ⚠️ Aviso de Responsabilidade
Este software é destinado apenas a brincadeiras leves e inofensivas entre amigos.
Não utilize em ambientes de trabalho, públicos ou em sistemas críticos.
O autor não se responsabiliza por qualquer uso indevido ou consequências causadas por este software.

## 📄 Licença
Distribuído sob a Licença MIT.
Você é livre para usar, modificar e redistribuir, desde que mantenha os créditos originais.

## ✨ Créditos
Criado por @antoniomalheirs com propósito de diversão e aprendizado em C# + Win32.

"Clippy morreu... ou será que não?" 👻
