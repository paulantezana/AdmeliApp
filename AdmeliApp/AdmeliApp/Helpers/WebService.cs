using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdmeliApp.Helpers
{
    public class WebService
    {
        public string urlBase { get; set; }
        public string domainName { get; set; }
        public string directory { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebService()
        {
            //this.domainName = "http://www.lineatienda.com";
            //this.directory = "services.php";

            this.domainName = "http://190.117.145.7:8080";
            this.directory = "service_fact_test/xcore/services.php";

            //this.domainName = "http://192.168.55.1:8080";
            //this.directory = "admeli/xcore/services.php";

            this.urlBase = String.Format("{0}/{1}", domainName, directory);
        }

        /// <summary>
        /// Verificar la conexion de internet
        /// </summary>
        /// <returns></returns>
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Por favor encienda su configuración de internet.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Comprueba tu conexión a Internet.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }


        /// <summary>
        /// Enviar y recivir datos mediante el metodo POST
        /// </summary>
        /// <typeparam name="T">Data Type enviar</typeparam>
        /// <typeparam name="K">Data Type recivir</typeparam>
        /// <param name="servicio">Servicio</param>
        /// <param name="metodo">metodo</param>
        /// <param name="param">datos enviar al webservie</param>
        /// <returns>K tipo generico</returns>
        public async Task<K> POST<T, K>(string servicio, string metodo, T param)
        {
            try
            {
                // Serializando el objeto
                string request = JsonConvert.SerializeObject(param);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                // Creando un nuevo cliente
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(this.domainName);
                string url = string.Format("{0}/{1}/{2}", this.directory, servicio, metodo);
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Validando la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString()); // si hay un error mandar un exception con statuscode
                }
                string result = await response.Content.ReadAsStringAsync();

                // retornando los valores como un objeto
                K dataResponse = JsonConvert.DeserializeObject<K>(result);
                return dataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Enviar y recivir datos mediante el metodo POST
        /// </summary>
        /// <typeparam name="T">Data Type enviar</typeparam>
        /// <typeparam name="K">Data Type recivir</typeparam>
        /// <param name="servicio">Servicio</param>
        /// <param name="param">datos enviar al webservie</param>
        /// <returns>K tipo generico</returns>
        public async Task<K> POST<T, K>(string servicio, T param)
        {
            try
            {
                /// Serializando el objeto
                string request = JsonConvert.SerializeObject(param);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                /// Creando un nuevo cliente
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(this.domainName);
                string url = string.Format("{0}/{1}", this.directory, servicio);
                HttpResponseMessage response = await client.PostAsync(url, content);

                /// Validando la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString()); /// si hay un error mandar un exception con statuscode
                }
                string result = await response.Content.ReadAsStringAsync();

                /// Retornando los valores en una lista de objetos
                K dataResponse = JsonConvert.DeserializeObject<K>(result);
                return dataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Este Metodo solo es temporal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<T>> POSTResponse<T>(string servicio, T param)
        {
            try
            {
                /// creando el contenido apartir de un jsonString
                string request = JsonConvert.SerializeObject(param);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                /// Creando un nuevo cliente
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(this.domainName);
                string url = string.Format("{0}/{1}", this.directory, servicio);
                HttpResponseMessage response = await client.PostAsync(url, content);

                /// Validando la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString()); /// si hay un error mandar un exception con statuscode
                }
                string result = await response.Content.ReadAsStringAsync();

                /// Retornando los valores en una lista de objetos
                List<T> res = JsonConvert.DeserializeObject<List<T>>(result);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ============== Metododo GET que retorna un objeto generico ==============
        /// <summary>
        /// Metodo GET Que devuelve un objeto genérico pasar como parametro el servicio y el metodo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <param name="metodo"></param>
        /// <returns></returns>
        public async Task<T> GET<T>(string servicio, string metodo)
        {
            try
            {
                /// Creando un nuevo cliente
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(this.domainName);
                string url = string.Format("{0}/{1}/{2}", this.directory, servicio, metodo);
                HttpResponseMessage response = await client.GetAsync(url);

                /// Validando la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString()); /// si hay un error mandar un exception con statuscode
                }
                string result = await response.Content.ReadAsStringAsync();

                /// Retornando los valores en un objeto
                T objeto = JsonConvert.DeserializeObject<T>(result);
                return objeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo GET Que devuelve un objeto genérico pasar como parametro solo el servicio
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <returns></returns>
        public async Task<T> GET<T>(string servicio)
        {
            try
            {
                /// Creando un nuevo cliente
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(this.domainName);
                string url = string.Format("{0}/{1}", this.directory, servicio);
                HttpResponseMessage response = await client.GetAsync(url);

                /// Validando la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString()); /// si hay un error mandar un exception con statuscode
                }
                string result = await response.Content.ReadAsStringAsync();

                /// Retornando los valores en un objeto
                T objeto = JsonConvert.DeserializeObject<T>(result);
                return objeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
