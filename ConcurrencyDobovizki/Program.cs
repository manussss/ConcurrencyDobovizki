using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

var stopWatch = new Stopwatch();

string[] ceps = ["07155081", "15800100", "38407369", "77445100", "78015818"];

var service = new ViaCepService();

stopWatch.Start();

//reduced from 3 seconds to 803ms (8 threads), depends on the MaxDegreeOfParallelism
var parallelOptions = new ParallelOptions();
parallelOptions.MaxDegreeOfParallelism = 2;
Parallel.ForEach(ceps, parallelOptions, cep => 
{
    Console.WriteLine(service.GetCep(cep) + $" | Thread: {Thread.CurrentThread.ManagedThreadId}");
});

stopWatch.Stop();

Console.WriteLine($"Tempo de processamento: {stopWatch.ElapsedMilliseconds}ms");

public class CepModel
{
    [JsonPropertyName("cep")]
    public string Cep { get; set; }
    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; }
    [JsonPropertyName("complemento")]
    public string Complemento { get; set; }
    [JsonPropertyName("bairro")]
    public string Bairro { get; set; }
    [JsonPropertyName("localidade")]        
    public string Localidade { get; set; }
    [JsonPropertyName("uf")]
    public string Uf { get; set; }
    [JsonPropertyName("ibge")]
    public string Ibge { get; set; }
    [JsonPropertyName("gia")]
    public string Gia { get; set; }
    [JsonPropertyName("ddd")]
    public string Ddd { get; set; }
    [JsonPropertyName("siafi")]
    public string Siafi { get; set; }

    public override string ToString()
    {
        return $"{this.Cep} - {this.Uf} - {this.Bairro} - {this.Localidade}";
    }
}

public class ViaCepService
{
    public CepModel GetCep(string cep)
    {
        var client = new HttpClient();
        var response = client.GetAsync($"https://viacep.com.br/ws/{cep}/json/").Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var cepResult = JsonSerializer.Deserialize<CepModel>(content);

        return cepResult;            
    }
}