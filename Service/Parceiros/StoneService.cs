using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Service.Parceiros;

public class StoneService : IStoneService
{
    private readonly HttpClient _httpClient;

    public StoneService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginStoneResponseModel> LoginStone(LoginStoneResquestModel request)
    {
        try
        {
            string clientEncryptionString = GenerateClientEncryptionString();
            string signature = GenerateSignature(clientEncryptionString);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ClientApplicationKey", request.ClientApplicationKey);
            _httpClient.DefaultRequestHeaders.Add("X-Authorization-Raw-Data", clientEncryptionString);
            _httpClient.DefaultRequestHeaders.Add("X-Authorization-Encrypted-Data", signature);

            string url = $"https://conciliation.stone.com.br/v1/merchant/{request.StoneCode}/conciliation-file/{request.ReferenceDate}";


            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string xmlContent = await response.Content.ReadAsStringAsync();

            return DeserializeXml<LoginStoneResponseModel>(xmlContent);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Erro na requisição: {ex.Message}", ex);
        }
    }

    private string GenerateClientEncryptionString()
    {
        return "string-gerada-com-algoritmo-especifico";
    }

    private string GenerateSignature(string clientEncryptionString)
    {
        using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes("SecretKey")))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(clientEncryptionString));
            return Convert.ToBase64String(hashBytes);
        }
    }

    private T DeserializeXml<T>(string xmlContent)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StringReader reader = new StringReader(xmlContent))
        {
            return (T)serializer.Deserialize(reader);
        }
    }
}