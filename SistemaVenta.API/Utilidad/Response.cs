namespace SistemaVenta.API.Utilidad
{
    //Respuesta a todas las solicitudes de la API
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string msg { get; set; }
    }
}
