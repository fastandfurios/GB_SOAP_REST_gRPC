namespace ClinicService.Models.Requests.Pets
{
    public class UpdatePetRequest
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int ClientId { get; set; }
    }
}
