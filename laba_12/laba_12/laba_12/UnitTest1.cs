using System.Text;
using Newtonsoft.Json.Linq;

public class StripeApiTests
{
    private static readonly string BaseUrl = "https://api.stripe.com/v1/customers";
    private static readonly string ArtUrl = "https://api.artic.edu/api/v1/artworks?id,title,artist_display";
    private readonly HttpClient _httpClient;
    private readonly string ApiKey = "sk_test_51QYo7jH2UvoV83PCPhxtd3PNyliQnPghuP372Jqc6k4CWWLD2VE8wzkiKhA8dLuIk2u5fdnfIYqv9ojOPDqmZPkT00brGWsHl1";
    private string customerId = "";

    public StripeApiTests()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    }

    // авторизация
    [Fact]
    public async Task TestCreateCustomerWithValidData()
    {
        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(BaseUrl, customerData);

        Assert.Equal(200, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        customerId = JObject.Parse(responseBody)["id"].ToString();
        Assert.False(string.IsNullOrEmpty(customerId));
    }

    // авторизация с неверными данными
    [Fact]
    public async Task TestCreateCustomerWithInvalidApiKey()
    {
        var invalidApiKey = "sk_test_invalidapikey";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {invalidApiKey}");

        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(BaseUrl, customerData);

        Assert.Equal(401, (int)response.StatusCode);
    }

    // удаление не существующего
    [Fact]
    public async Task TestDeleteCustomerWithNonexistentId()
    {
        var nonexistentId = "nonexistent_id";
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{nonexistentId}");

        Assert.Equal(404, (int)response.StatusCode);
    }

    // Вывод пользователя
    [Fact]
    public async Task TestGetCustomers()
    {
        var response = await _httpClient.GetAsync(BaseUrl);

        Assert.Equal(200, (int)response.StatusCode);
    }

    // зайти под несуществующим клиентам
    [Fact]
    public async Task TestUnauthorizedRequest()
    {
        var client = new HttpClient();
        var response = await client.PostAsync(BaseUrl, new StringContent("{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}", Encoding.UTF8, "application/json"));

        Assert.Equal(401, (int)response.StatusCode);
    }


    // CRUD
    [Fact]
    public async Task TestCreateUser()
    {
        var userData = new StringContent(
            "{\"email\": \"new_user@example.com\", \"name\": \"New User\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(BaseUrl, userData);

        Assert.Equal(200, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        customerId = JObject.Parse(responseBody)["id"].ToString();
    }

    [Fact]
    public async Task TestGetCustomerList()
    {
        var response = await _httpClient.GetAsync(BaseUrl);

        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestUpdateCustomerInvalidId()
    {
        var updateData = new StringContent(
            "{\"email\": \"new_email@example.com\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync($"{BaseUrl}/{customerId}", updateData);

        Assert.Equal(404, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteCustomerWithValidId()
    {
        var customerData = new StringContent(
            "{\"email\": \"delete_test_user@example.com\", \"name\": \"Delete Test User\"}",
            Encoding.UTF8,
            "application/json"
        );
        var response = await _httpClient.PostAsync(BaseUrl, customerData);
        var responseBody = await response.Content.ReadAsStringAsync();
        var customerId = JObject.Parse(responseBody)["id"].ToString();

        response = await _httpClient.DeleteAsync($"{BaseUrl}/{customerId}");

        Assert.Equal(200, (int)response.StatusCode);
    }

    // data test
    // отправка с пустым теллом
    [Fact]
    public async Task TestUnauthorizedRequestWithEmptyBody()
    {
        var emptyData = new StringContent("", Encoding.UTF8, "application/json");

        var client = new HttpClient();  
        var response = await client.PostAsync(BaseUrl, emptyData);

        Assert.Equal(401, (int)response.StatusCode);  
    }

    // по несуществующему адресу
    [Fact]
    public async Task TestNonexistentEndpoint()
    {
        var invalidEndpointUrl = "https://api.stripe.com/v1/nonexistent_endpoint";
        var response = await _httpClient.GetAsync(invalidEndpointUrl);

        Assert.Equal(404, (int)response.StatusCode);
    }

    // dostup test
    // попытка создания клиента без данных
    [Fact]
    public async Task TestAccessProtectedEndpointWithoutToken()
    {
        var clientWithoutToken = new HttpClient();  

        var customerData = new StringContent(
            "{\"email\": \"test_user_without_token@example.com\", \"name\": \"Test User Without Token\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await clientWithoutToken.PostAsync(BaseUrl, customerData);

        Assert.Equal(401, (int)response.StatusCode);  
    }

    // попытка входа с неверным ключем
    [Fact]
    public async Task TestAccessDataFromAnotherUser()
    {
        var anotherUserApiKey = "sk_test_anotheruserapikey";  
        var clientForAnotherUser = new HttpClient();
        clientForAnotherUser.DefaultRequestHeaders.Add("Authorization", $"Bearer {anotherUserApiKey}");

        var response = await clientForAnotherUser.GetAsync($"{BaseUrl}/{customerId}");

        Assert.Equal(404, (int)response.StatusCode);  
    }

    // попытка получения результата от несуществующенго
    [Fact]
    public async Task TestAccessCustomerDetailsForAnotherUserWithToken()
    {
        var anotherUserApiKey = "sk_test_anotheruserapikey";  
        var clientForAnotherUser = new HttpClient();
        clientForAnotherUser.DefaultRequestHeaders.Add("Authorization", $"Bearer {anotherUserApiKey}");

        var response = await clientForAnotherUser.GetAsync($"{BaseUrl}/{customerId}");

        Assert.Equal(404, (int)response.StatusCode);  
    }

    // validation test
    // отправка данных неправельным способом
    [Fact]
    public async Task TestRequiredFieldMissing()
    {
        var authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");

        var clientWithoutAuthorization = new HttpClient();
        clientWithoutAuthorization.DefaultRequestHeaders.Authorization = authorization;

        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\"}",  
            Encoding.UTF8,
            "application/json"
        );

        var response = await clientWithoutAuthorization.PostAsync(BaseUrl, customerData);

        Assert.Equal(401, (int)response.StatusCode); 
    }


    // last 
    // попытка чтения страницы которой не существует
    [Fact]
    public async Task TestPagination_WithOutOfRangePage_ReturnsEmptyOrError()
    {
        int page = 1000; 
        int limit = 10;

        var response = await _httpClient.GetAsync($"{ArtUrl}?page={page}&limit={limit}");

        Assert.False(response.IsSuccessStatusCode); 

        var result = await response.Content.ReadAsStringAsync();

        Assert.Contains("error", result); 
    }
}

