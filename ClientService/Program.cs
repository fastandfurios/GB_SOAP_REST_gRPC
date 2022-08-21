using ClientServiceProtos;
using ConsultationServiceProtos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using PetServiceProtos;

AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
using var channel = GrpcChannel.ForAddress("http://localhost:5001");

var clientService = new ClientService.ClientServiceClient(channel);
var consultationService = new ConsultationService.ConsultationServiceClient(channel);
var petService = new PetService.PetServiceClient(channel);

CreateClient(clientService);
//CreatePet(petService);
//CreateConsultation(consultationService);
//GetClientById(clientService);

Console.ReadKey();

#region methods
static void CreateClient(ClientService.ClientServiceClient service)
{
    var createClientResponse = service.CreateClient(new CreateClientRequest
    {
        Document = "Документ123",
        FirstName = "Петр",
        Surname = "Петров",
        Patronymic = "Петрович"
    });

    Console.WriteLine($"Client ({createClientResponse.ClientId}) created successfully.");

    var getClientsResponse = service.GetClients(new GetClientsRequest());
    if (getClientsResponse.ErrCode == 0)
    {
        Console.WriteLine("Clients:");
        Console.WriteLine("========\n");
        foreach (var clientDto in getClientsResponse.Clients)
        {
            Console.WriteLine(
                $"({clientDto.ClientId}/{clientDto.Document}) {clientDto.Surname} {clientDto.FirstName} {clientDto.Patronymic}");
        }
    }
}

static void CreatePet(PetService.PetServiceClient service)
{
    var response = service.CreatePet(new CreatePetRequest
    {
        Birthday = Timestamp.FromDateTime(DateTime.UtcNow),
        Name = "Барбос",
        ClientId = 2
    });

    Console.WriteLine($"Pet ({response.PetId}) created successfully.");

    var responsePet = service.GetPets(new GetPetsRequest());
    if (responsePet.ErrCode == 0)
    {
        Console.WriteLine("Pets:");
        Console.WriteLine("========\n");
        foreach (var petDto in responsePet.Pets)
        {
            Console.WriteLine(
                $"({petDto.PetId} {petDto.Name}) {petDto.Birthday} {petDto.ClientId}");
        }
    }
}

static void CreateConsultation(ConsultationService.ConsultationServiceClient service)
{
    var response = service.CreateConsultation(new CreateConsultationRequest
    {
        ConsultationDate = Timestamp.FromDateTime(DateTime.UtcNow),
        ClientId = 2,
        PetId = 1,
        Description = "test"
    });

    Console.WriteLine($"Consultation ({response.ConsultationId}) created successfully.");

    var responseConsultation = service.GetConsultations(new GetConsultationsRequest());
    if (responseConsultation.ErrCode == 0)
    {
        Console.WriteLine("Consultation:");
        Console.WriteLine("========\n");
        foreach (var consultationDto in responseConsultation.Consultations)
        {
            Console.WriteLine(
                $"({consultationDto.ConsultationId} {consultationDto.ConsultationDate} {consultationDto.Description}) {consultationDto.ClientId} {consultationDto.PetId}");
        }
    }
}

static void GetClientById(ClientService.ClientServiceClient service)
{
    var response = service.GetClientById(new GetClientByIdRequest
    {
        ClientId = 2
    });

    Console.WriteLine($"Client: ClientId = {response.ClientId} Document = {response.Document} FirstName = {response.FirstName} Surname = {response.Surname} Patronymic = {response.Patronymic}");
    foreach (var pet in response.Pets)
        Console.WriteLine($"Pet: PetId = {pet.PetId} Name = {pet.Name} Birthday = {pet.Birthday.ToDateTime()}");

    foreach (var consultation in response.Consultations)
        Console.WriteLine($"Consultation: ConsultationId = {consultation.ConsultationId} ConsultationDate = {consultation.ConsultationDate.ToDateTime()} Description = {consultation.Description}");
    
}
#endregion