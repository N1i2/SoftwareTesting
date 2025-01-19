using System.Text;
using Newtonsoft.Json.Linq;


public class StripeApiTests
{
    private static readonly string BaseUrl = "https://api.stripe.com/v1/customers";  // Базовый URL для работы с API Stripe для клиентов
    private static readonly string ArtUrl = "https://api.artic.edu/api/v1/artworks?id,title,artist_display";  // URL для работы с API Art Institute
    private readonly HttpClient _httpClient;  // Экземпляр HttpClient для выполнения запросов
    private readonly string ApiKey = "sk_test_51QYo7jH2UvoV83PCPhxtd3PNyliQnPghuP372Jqc6k4CWWLD2VE8wzkiKhA8dLuIk2u5fdnfIYqv9ojOPDqmZPkT00brGWsHl1";  // API ключ для авторизации в Stripe
    private string customerId = "";  // Идентификатор клиента (позже будет использован в тестах)

    public StripeApiTests()
    {
        _httpClient = new HttpClient();  // Создаем экземпляр HttpClient
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");  // Добавляем заголовок для авторизации с API ключом
    }

    // Тест для создания клиента с корректными данными
    [Fact]
    public async Task TestCreateCustomerWithValidData()
    {
        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}",  // Данные клиента для создания
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(BaseUrl, customerData);  // Отправляем POST-запрос на создание клиента

        Assert.Equal(200, (int)response.StatusCode);  // Проверяем, что код ответа 200 (успех)

        var responseBody = await response.Content.ReadAsStringAsync();  // Получаем ответ от сервера
        customerId = JObject.Parse(responseBody)["id"].ToString();  // Извлекаем id клиента из ответа
        Assert.False(string.IsNullOrEmpty(customerId));  // Проверяем, что id клиента не пустой
    }

    // Тест для создания клиента с неверным API ключом
    [Fact]
    public async Task TestCreateCustomerWithInvalidApiKey()
    {
        var invalidApiKey = "sk_test_invalidapikey";  // Неверный API ключ
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {invalidApiKey}");  // Добавляем неверный ключ авторизации

        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(BaseUrl, customerData);  // Отправляем запрос

        Assert.Equal(401, (int)response.StatusCode);  // Проверяем, что код ответа 401 (неавторизован)
    }

    // Тест для удаления клиента с несуществующим id
    [Fact]
    public async Task TestDeleteCustomerWithNonexistentId()
    {
        var nonexistentId = "nonexistent_id";  // Некорректный id клиента
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{nonexistentId}");  // Отправляем запрос на удаление

        Assert.Equal(404, (int)response.StatusCode);  // Проверяем, что код ответа 404 (не найдено)
    }

    // Тест для получения списка клиентов
    [Fact]
    public async Task TestGetCustomers()
    {
        var response = await _httpClient.GetAsync(BaseUrl);  // Отправляем запрос на получение списка клиентов

        Assert.Equal(200, (int)response.StatusCode);  // Проверяем, что код ответа 200 (успех)
    }

    // Тест для неавторизованного запроса (без ключа)
    [Fact]
    public async Task TestUnauthorizedRequest()
    {
        var client = new HttpClient();  // Создаем клиент без ключа авторизации
        var response = await client.PostAsync(BaseUrl, new StringContent("{\"email\": \"test_user@example.com\", \"name\": \"Test User\"}", Encoding.UTF8, "application/json"));  // Отправляем запрос

        Assert.Equal(401, (int)response.StatusCode);  // Проверяем, что код ответа 401 (неавторизован)
    }

    // Тест для создания нового клиента
    [Fact]
    public async Task TestCreateUser()
    {
        var userData = new StringContent(
            "{\"email\": \"new_user@example.com\", \"name\": \"New User\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(BaseUrl, userData);  // Отправляем запрос на создание нового клиента

        Assert.Equal(200, (int)response.StatusCode);  // Проверяем, что код ответа 200 (успех)

        var responseBody = await response.Content.ReadAsStringAsync();  // Получаем ответ
        customerId = JObject.Parse(responseBody)["id"].ToString();  // Извлекаем id нового клиента
    }

    // Тест для получения списка клиентов
    [Fact]
    public async Task TestGetCustomerList()
    {
        var response = await _httpClient.GetAsync(BaseUrl);  // Отправляем запрос на получение списка клиентов

        Assert.Equal(200, (int)response.StatusCode);  // Проверяем, что код ответа 200 (успех)
    }

    // Тест для обновления клиента с некорректным id
    [Fact]
    public async Task TestUpdateCustomerInvalidId()
    {
        var updateData = new StringContent(
            "{\"email\": \"new_email@example.com\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync($"{BaseUrl}/{customerId}", updateData);  // Отправляем запрос на обновление клиента

        Assert.Equal(404, (int)response.StatusCode);  // Проверяем, что код ответа 404 (не найдено)
    }

    // Тест для удаления клиента с валидным id
    [Fact]
    public async Task TestDeleteCustomerWithValidId()
    {
        var customerData = new StringContent(
            "{\"email\": \"delete_test_user@example.com\", \"name\": \"Delete Test User\"}",
            Encoding.UTF8,
            "application/json"
        );
        var response = await _httpClient.PostAsync(BaseUrl, customerData);  // Создаем клиента
        var responseBody = await response.Content.ReadAsStringAsync();
        var customerId = JObject.Parse(responseBody)["id"].ToString();  // Получаем id клиента

        response = await _httpClient.DeleteAsync($"{BaseUrl}/{customerId}");  // Отправляем запрос на удаление клиента

        Assert.Equal(200, (int)response.StatusCode);  // Проверяем, что код ответа 200 (успех)
    }

    // Тест для запроса с пустым телом (неавторизованный запрос)
    [Fact]
    public async Task TestUnauthorizedRequestWithEmptyBody()
    {
        var emptyData = new StringContent("", Encoding.UTF8, "application/json");

        var client = new HttpClient();
        var response = await client.PostAsync(BaseUrl, emptyData);  // Отправляем запрос с пустым телом

        Assert.Equal(401, (int)response.StatusCode);  // Проверяем, что код ответа 401 (неавторизован)
    }

    // Тест для запроса к несуществующему эндпоинту
    [Fact]
    public async Task TestNonexistentEndpoint()
    {
        var invalidEndpointUrl = "https://api.stripe.com/v1/nonexistent_endpoint";  // Некорректный URL
        var response = await _httpClient.GetAsync(invalidEndpointUrl);  // Отправляем запрос

        Assert.Equal(404, (int)response.StatusCode);  // Проверяем, что код ответа 404 (не найдено)
    }

    // Тест для доступа к защищенному эндпоинту без токена
    [Fact]
    public async Task TestAccessProtectedEndpointWithoutToken()
    {
        var clientWithoutToken = new HttpClient();  // Клиент без токена авторизации

        var customerData = new StringContent(
            "{\"email\": \"test_user_without_token@example.com\", \"name\": \"Test User Without Token\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await clientWithoutToken.PostAsync(BaseUrl, customerData);  // Отправляем запрос без токена

        Assert.Equal(401, (int)response.StatusCode);  // Проверяем, что код ответа 401 (неавторизован)
    }

    // Тест для запроса данных другого пользователя
    [Fact]
    public async Task TestAccessDataFromAnotherUser()
    {
        var anotherUserApiKey = "sk_test_anotheruserapikey";  // API ключ другого пользователя
        var clientForAnotherUser = new HttpClient();
        clientForAnotherUser.DefaultRequestHeaders.Add("Authorization", $"Bearer {anotherUserApiKey}");  // Используем ключ другого пользователя

        var response = await clientForAnotherUser.GetAsync($"{BaseUrl}/{customerId}");  // Запрос данных другого клиента

        Assert.Equal(404, (int)response.StatusCode);  // Проверяем, что код ответа 404 (не найдено)
    }

    // Тест для запроса данных другого клиента с токеном
    [Fact]
    public async Task TestAccessCustomerDetailsForAnotherUserWithToken()
    {
        var anotherUserApiKey = "sk_test_anotheruserapikey";  // API ключ другого пользователя
        var clientForAnotherUser = new HttpClient();
        clientForAnotherUser.DefaultRequestHeaders.Add("Authorization", $"Bearer {anotherUserApiKey}");  // Используем ключ другого пользователя

        var response = await clientForAnotherUser.GetAsync($"{BaseUrl}/{customerId}");  // Запрос данных другого клиента

        Assert.Equal(404, (int)response.StatusCode);  // Проверяем, что код ответа 404 (не найдено)
    }

    // Тест для проверки отсутствия обязательных полей
    [Fact]
    public async Task TestRequiredFieldMissing()
    {
        var authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");  // Создаем пустой авторизационный заголовок

        var clientWithoutAuthorization = new HttpClient();
        clientWithoutAuthorization.DefaultRequestHeaders.Authorization = authorization;  // Устанавливаем пустой заголовок авторизации

        var customerData = new StringContent(
            "{\"email\": \"test_user@example.com\"}",  // Отсутствует обязательное поле "name"
            Encoding.UTF8,
            "application/json"
        );

        var response = await clientWithoutAuthorization.PostAsync(BaseUrl, customerData);  // Отправляем запрос

        Assert.Equal(401, (int)response.StatusCode);  // Проверяем, что код ответа 401 (неавторизован)
    }

    // Тест для проверки пагинации с неверными параметрами
    [Fact]
    public async Task TestPagination_WithOutOfRangePage_ReturnsEmptyOrError()
    {
        int page = 1000;  // Пагинация на 1000-й странице
        int limit = 10;

        var response = await _httpClient.GetAsync($"{ArtUrl}?page={page}&limit={limit}");  // Запрос с несуществующей страницей

        Assert.False(response.IsSuccessStatusCode);  // Проверяем, что запрос не успешен

        var result = await response.Content.ReadAsStringAsync();  // Получаем результат запроса

        Assert.Contains("error", result);  // Проверяем, что в ответе есть ошибка
    }
}
