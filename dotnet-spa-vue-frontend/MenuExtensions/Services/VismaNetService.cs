using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MenuExtensions.Abstractions.Services;
using MenuExtensions.Models.VismaNet;
using Microsoft.AspNetCore.Authentication;

namespace MenuExtensions.Services;

public class VismaNetService(IHttpContextAccessor httpContextAccessor) : IVismaNetService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <summary>
    /// Gets all suppliers classes.
    /// </summary>
    /// <returns>Task<IEnumerable<SupplierClassDto>></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<IEnumerable<SupplierClassDto>> GetSupplierClasses()
    {
        using var httpClient = await GetHttpClientWithAuthHeaderSet();

        var supplierClasses =
            await httpClient.GetFromJsonAsync<IEnumerable<SupplierClassDto>>(
                Endpoints.SupplierClass
            ) ?? throw new NullReferenceException("Supplier classes are null");

        return supplierClasses;
    }

    /// <summary>
    /// Creates a supplier invoice.
    /// </summary>
    /// <param name="supplierInvoiceDto"></param>
    /// <returns>Task</returns>
    public async Task CreateSupplierInvoice(CreateSupplierInvoiceDto supplierInvoiceDto)
    {
        using var httpClient = await GetHttpClientWithAuthHeaderSet();

        using var jsonContent = new StringContent(
            JsonSerializer.Serialize(supplierInvoiceDto),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync(Endpoints.SupplierInvoice, jsonContent);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Gets an HttpClient with the access token set in the Authorization header.
    /// </summary>
    /// <returns>Task<HttpClient></returns>
    /// <exception cref="NullReferenceException"></exception>
    private async Task<HttpClient> GetHttpClientWithAuthHeaderSet()
    {
        var accessToken =
            await _httpContextAccessor.HttpContext!.GetTokenAsync(
                Microsoft
                    .IdentityModel
                    .Protocols
                    .OpenIdConnect
                    .OpenIdConnectParameterNames
                    .AccessToken
            ) ?? throw new NullReferenceException("Access token is null");
        ;

        // TODO: Handle different environments
        // This URL is specific for staging environment. For production, it should be https://integration.visma.net
        var vismaConnectBaseAddress = "https://integration.stag.visma.net";

        // Generally speaking, this instance would be created using an HttpClientFactory that is defined in Program.cs
        // This is just a simple example
        var httpClient = new HttpClient() { BaseAddress = new Uri(vismaConnectBaseAddress) };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken
        );
        return httpClient;
    }

    /// <summary>
    /// Used to store the endpoints for the VismaNet API.
    /// </summary>
    private class Endpoints
    {
        private const string _baseEndpoint = "/API/controller/api/v1";
        public const string SupplierClass = $"{_baseEndpoint}/supplier/supplierClass";
        public const string SupplierInvoice = $"{_baseEndpoint}/supplierInvoice";
    }
}
