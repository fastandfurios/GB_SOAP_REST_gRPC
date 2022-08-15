namespace ClinicService.Models.Requests.Pets
{
    public class CreatePetRequest
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int ClientId { get; set; }
    }
}
