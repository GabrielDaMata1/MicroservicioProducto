using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq.Protected;
using Moq;

namespace TestMicroservicioProducto.ServiceTest
{
    public class UsuarioServiceTest
    {
        private const string BaseUrl = "http://localhost:5001/";
        private const string Endpoint = "api/usuarios/IdUsuario/";

        private HttpClient CrearHttpClient(Mock<HttpMessageHandler> handlerMock)
        {
            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuid_CuandoRespuestaEsExitosa()
        {
            var expectedGuid = Guid.NewGuid();
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{Endpoint}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"\"{expectedGuid}\"")
                })
                .Verifiable();

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(expectedGuid, resultado);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == expectedUrl
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuidEmpty_CuandoRespuestaEsFallida()
        {
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{Endpoint}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(Guid.Empty, resultado);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == expectedUrl
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuidEmpty_CuandoContenidoNoEsGuid()
        {
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{Endpoint}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("\"no-es-un-guid\"")
                })
                .Verifiable();

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(Guid.Empty, resultado);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == expectedUrl
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

    }
}
