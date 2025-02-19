using OllamaSharp;
using OllamaSharp.Models;
using ScreenSound.Modelos;
using System.Text;

namespace ScreenSound.Menus;

internal class MenuRegistrarBanda : Menu
{
    public override void Executar(Dictionary<string, Banda> bandasRegistradas)
    {
        base.Executar(bandasRegistradas);
        ExibirTituloDaOpcao("Registro das bandas");
        Console.Write("Digite o nome da banda que deseja registrar: ");
        string nomeDaBanda = Console.ReadLine()!;
        Banda banda = new Banda(nomeDaBanda);
        bandasRegistradas.Add(nomeDaBanda, banda);

        var ollama = new OllamaApiClient("http://localhost:11434");

        var request = new GenerateRequest
        {
            Model = "llama3.2:latest",
            Prompt = $"Resuma a banda {nomeDaBanda} em 1 parágrafo. Adote um estilo informal."
        };

        var resposta = new StringBuilder();

        Task.Run(async () =>
        {
            await foreach (var stream in ollama.GenerateAsync(request))
            {
                resposta.Append(stream.Response);
            }
        }).GetAwaiter().GetResult();

        banda.Resumo = resposta.ToString();

        Console.WriteLine($"A banda {nomeDaBanda} foi registrada com sucesso!");
        Console.WriteLine("Digite uma tecla para voltar ao menu principal");
        Console.ReadKey();
        Console.Clear();
    }
}