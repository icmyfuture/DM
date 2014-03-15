namespace DM.Web.SL.Common.Service.Entities
{
    public class ResponseEntity<T>
    {
        public T Result { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }
}