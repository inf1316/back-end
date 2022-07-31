using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.FunctionalTests.Shared.Identity;
using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QvaCar.Api.FunctionalTests.SeedWork
{
    public static class HttpClientExtensions
    {
        public static HttpClient FromUser(this HttpClient client, TestApiUser user)
        {
            var tokenEncoded = user.ConvertToAccessToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("FakeBearer", tokenEncoded);
            return client;
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, string uri)
        {
            return await client.PutAsync(uri, null);
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, string uri, object content)
        {
            var json = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            return await client.PutAsync(uri, httpContent);
        }

        #region Get
        public static async Task<T> GetAsync<T>(this HttpClient client, TestApiUser asUser, string url)
        {
            client.FromUser(asUser);
            using var postResponse = await client.GetAsync(url);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            postResponse.IsSuccessStatusCode.Should().BeTrue();
            var responseModel = await postResponse.Deserialize<T>();
            responseModel.Should().NotBeNull();
            return responseModel;
        }

        public static async Task GetAndExpectUnauthorizedAsync(this HttpClient client, string url)
        {
            using var postResponse = await client.GetAsync(url);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        public static async Task<ProblemDetails> GetAndExpectServerErrorAsync(this HttpClient client, TestApiUser asUser, string url)
        {
            client.FromUser(asUser);
            using var postResponse = await client.GetAsync(url);
            postResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
            return responseModel;
        }

        public static async Task GetAndExpectNotFoundAsync(this HttpClient client, TestApiUser asUser, string url)
        {
            client.FromUser(asUser);
            using var postResponse = await client.GetAsync(url);
            postResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }
        #endregion


        #region Post
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string uri, object content)
        {
            var json = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            return await client.PostAsync(uri, httpContent);
        }

        public static async Task<T> PostAsync<T>(this HttpClient client, TestApiUser asUser, string url, object request)
        {
            client.FromUser(asUser);
            using var postResponse = await client.PostAsync(url, request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            postResponse.IsSuccessStatusCode.Should().BeTrue();
            var responseModel = await postResponse.Deserialize<T>();
            responseModel.Should().NotBeNull();
            return responseModel;
        }

        public static async Task PostAndExpectUnauthorizedAsync(this HttpClient client, string url, object request)
        {
            using var postResponse = await client.PostAsync(url, request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        public static async Task PostAndExpectBadRequestAsync(this HttpClient client, TestApiUser asUser, string url, object request)
        {
            client.FromUser(asUser);
            using var postResponse = await client.PostAsync(url, request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        public static async Task PostAndExpectNotFoundAsync(this HttpClient client, TestApiUser asUser, string url, object request)
        {
            client.FromUser(asUser);
            using var postResponse = await client.PostAsync(url, request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        public static async Task<ProblemDetails> PostAndExpectServerErrorAsync(this HttpClient client, TestApiUser asUser, string url, object request)
        {
            client.FromUser(asUser);
            using var postResponse = await client.PostAsync(url, request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            postResponse.IsSuccessStatusCode.Should().BeFalse();
            var responseModel = await postResponse.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
            return responseModel;
        }
        #endregion
    }
}
